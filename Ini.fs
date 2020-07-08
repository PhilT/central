namespace Central

type Ini = {
  title: string
  exe: string
  icon: Eto.Drawing.Bitmap
  args: string
}

module Ini =
  open System.Text.RegularExpressions

  let (|Section|_|) input =
    let result = Regex.Match(input, "^\[(.+)\]$")
    if result.Success then Some(result.Groups.[1].Value) else None


  let (|KeyValue|_|) input =
    let result = Regex.Match(input, "^(.+)=(.+)$")
    if result.Success then Some(result.Groups.[1].Value.Trim(), result.Groups.[2].Value.Trim())
    else None


  let emptyIniMap = Map.add "args" "" Map.empty

  let load path =
    System.IO.File.ReadAllLines(path)
    |> Array.fold (fun (config, section) next ->
      match next with
      | Section newSection ->
        Map.add newSection emptyIniMap config, newSection
      | KeyValue (key, value) ->
        Map.add section (Map.add key value config.[section]) config, section
      | _ -> config, section
    ) (Map.empty, "")
    |> (fun (map, _) -> map)
