open System
open Eto
open Eto.Drawing
open System.Diagnostics
open Central

[<STAThread>]
[<EntryPoint>]
let main _ =
  let app = new Forms.Application()
  let form =
    (new Forms.Form(
      Title = "Central Launcher",
      Topmost = true,
      Resizable = false,
      Height = 500,
      WindowStyle = Eto.Forms.WindowStyle.None,
      MovableByWindowBackground = true
    ))

  form.KeyUp.Add(fun key -> if key.Key = Forms.Keys.Escape then form.Close())

  let layout =
    new Forms.StackLayout(
      Orientation = Forms.Orientation.Vertical,
      Spacing = 4,
      Padding = Padding(8),
      BackgroundColor = Color(0.2f, 0.2f, 0.2f)
    )

  let searchFont = new Font("Segoe UI", 22.0f, FontStyle.Bold)
  let defaultFont = new Font("Segoe UI", 18.0f, FontStyle.None)
  let color = Color(0.9f, 0.9f, 0.9f)
  let backgroundColor = Color(0.2f, 0.2f, 0.2f)
  let highlighted = Color(0.15f, 0.15f, 0.15f)

  let items =
    Ini.load "config.ini"
    |> Icon.loadAll

  let createItem text image (exe: string) =
    let row = new Forms.StackLayout(Orientation = Forms.Orientation.Horizontal, Padding = Padding(4), Width = 500)
    let imgControl = new Forms.ImageView(Image = image, Width = 32)
    let labelControl = new Forms.Label(Text = text, TextColor = color, Font = defaultFont, BackgroundColor = Color(A = 0.0f), Width = 400)
    labelControl.MouseEnter.Add(fun _ -> row.BackgroundColor <- highlighted)
    labelControl.MouseLeave.Add(fun _ -> row.BackgroundColor <- backgroundColor)

    row.Items.Add(Forms.StackLayoutItem(imgControl))
    row.Items.Add(Forms.StackLayoutItem(labelControl))

    let panel = new Forms.Panel(Content = row)
    panel.MouseEnter.Add(fun _ -> printfn "Entered!")
    panel.MouseLeave.Add(fun _ -> printfn "Left!")
    panel.MouseDown.Add(fun _ -> Process.Start(exe) |> ignore )
    Forms.StackLayoutItem(panel)


  let textBox = new Forms.TextBox(Width = 500, Height = int (searchFont.LineHeight * 1.333f), Font = searchFont, TextColor = color, BackgroundColor = highlighted, ShowBorder = false)
  let panel = new Forms.Panel(Content = textBox, Padding = Padding(4), BackgroundColor = highlighted)

  layout.Items.Add(Forms.StackLayoutItem(panel))
  items
  |> List.iter (fun ini ->
    layout.Items.Add(createItem (" " + ini.title) ini.icon ini.exe)
  )
  form.Content <- layout
  form.Show()
  app.Run(form)
  0
