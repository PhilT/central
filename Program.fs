open System
open Eto
open Eto.Drawing
open System.Diagnostics
open Central

[<STAThread>]
[<EntryPoint>]
let main _ =
  let app = new Forms.Application()

  let searchFont = new Font("Segoe UI", 22.0f, FontStyle.Bold)
  let defaultFont = new Font("Segoe UI", 18.0f, FontStyle.None)
  let color = Color(0.9f, 0.9f, 0.9f)
  let borderColor = Color(0.25f, 0.25f, 0.25f)
  let backgroundColor = Color(0.2f, 0.2f, 0.2f)
  let highlighted = Color(0.15f, 0.15f, 0.15f)


  let tray = new Forms.TrayIndicator(Image = new Bitmap("icon.png"))
  tray.Show()

  let form =
    (new Forms.Form(
      Title = "Central Launcher",
      Resizable = false,
      WindowStyle = Eto.Forms.WindowStyle.None,
      MovableByWindowBackground = true
    ))


  form.KeyUp.Add(fun key -> if key.Key = Forms.Keys.Escape then form.Visible <- false)
  form.KeyDown.Add(fun key -> if key.Key = Forms.Keys.W && key.Control then app.Quit())
  tray.Activated.Add(fun _ -> Form.focus form)
  form.Closing.Add(fun _ -> app.Quit())


  let layout =
    new Forms.StackLayout(
      Orientation = Forms.Orientation.Vertical,
      Padding = Padding(6),
      BackgroundColor = borderColor
    )

  let items =
    Ini.load "config.ini"
    |> Icon.loadAll

  let createItem text image (exe: string) =
    let row = new Forms.StackLayout(Orientation = Forms.Orientation.Horizontal, Padding = Padding(6), Width = 500, BackgroundColor = backgroundColor)
    let imgControl = new Forms.ImageView(Image = image, Width = 32, BackgroundColor = Color(A = 0.0f))
    let labelControl = new Forms.Label(Text = text, TextColor = color, Font = defaultFont, BackgroundColor = Color(A = 0.0f), Width = 400)
    labelControl.MouseEnter.Add(fun _ -> row.BackgroundColor <- highlighted)
    labelControl.MouseLeave.Add(fun _ -> row.BackgroundColor <- backgroundColor)

    row.Items.Add(Forms.StackLayoutItem(imgControl))
    row.Items.Add(Forms.StackLayoutItem(labelControl))

    let panel = new Forms.Panel(Content = row)
    panel.MouseEnter.Add(fun _ -> printfn "Entered!")
    panel.MouseLeave.Add(fun _ -> printfn "Left!")
    panel.MouseDown.Add(fun _ ->
      Process.Start(exe) |> ignore
      form.Visible <- false
    )
    Forms.StackLayoutItem(panel)


  let textBox = new Forms.TextBox(Width = 492, Height = int (searchFont.LineHeight * 1.333f), Font = searchFont, TextColor = color, BackgroundColor = highlighted, ShowBorder = false)
  let padding = new Forms.Panel(Content = textBox, Padding = Padding(4), BackgroundColor = highlighted)
  let border = new Forms.Panel(Content = padding, Padding = Padding(0, 0, 0, 6), BackgroundColor = borderColor)

  layout.Items.Add(Forms.StackLayoutItem(border))
  items
  |> List.iter (fun ini ->
    layout.Items.Add(createItem (" " + ini.title) ini.icon ini.exe)
  )
  form.Content <- layout

  Tray.getMessages form
  printfn "App running"
  form.Visible <- false
  let screenSize = form.Screen.Bounds.Size
  form.Location <- Eto.Drawing.Point(
    (int screenSize.Width - 500) / 2,
    200
  )
  form.Opacity <- 0.8
  app.Run()

  tray.Hide()
  0
