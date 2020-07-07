open System
open Eto

[<STAThread>]
[<EntryPoint>]
let main _ =
  let app = new Forms.Application()
  let form = new Forms.Form()

  let layout = new Forms.StackLayout(Orientation = Forms.Orientation.Vertical)
  let textBox = new Forms.TextBox(Width = 100)
  let backgroundColor = Drawing.Color(0.2f, 0.2f, 0.2f)
  let panel = new Forms.Panel(BackgroundColor = backgroundColor)
  panel.Content <- textBox
  layout.Items.Add(Forms.StackLayoutItem(textBox))

  form.Content <- layout
  form.Show()
  app.Run(form)
  0
