module Central.Icon
open System
open System.Text.RegularExpressions

let toArray (img: System.Drawing.Image) =
  use stream = new IO.MemoryStream()
  img.Save(stream, Drawing.Imaging.ImageFormat.Png)
  stream.ToArray()


let fromExe path =
  let icon = System.Drawing.Icon.ExtractAssociatedIcon(path)
  new Eto.Drawing.Bitmap(toArray (icon.ToBitmap()))


let loadAll config =
  config
  |> Map.map (fun section map ->
    let icon = Map.find "icon" map
    let icon =
      if Regex.IsMatch(icon, "\.exe$") then
        fromExe icon
      else
        new Eto.Drawing.Bitmap(icon)
    { title = section; exe = map.["exe"]; icon = icon; args = map.["args"]}
  )
  |> Map.toList
  |> List.map (fun (_, record) -> record)

