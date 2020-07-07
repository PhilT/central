module Central.Icon
open System
open System.Text.RegularExpressions
open Eto.Drawing

let imageToBytes (img: System.Drawing.Image) =
  use stream = new IO.MemoryStream()
  img.Save(stream, Drawing.Imaging.ImageFormat.Png)
  stream.ToArray()


let loadAll config =
  config
  |> Map.map (fun section map ->
    let icon = Map.find "icon" map
    let icon =
      if Regex.IsMatch(icon, "\.exe$") then
        let icon = System.Drawing.Icon.ExtractAssociatedIcon(icon)
        new Bitmap(imageToBytes (icon.ToBitmap()))
      else
        new Bitmap(icon)
    { title = section; exe = map.["exe"]; icon = icon; args = map.["args"]}
  )
  |> Map.toList
  |> List.map (fun (_, record) -> record)

