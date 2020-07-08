module Central.Tray

open System
open System.Runtime.InteropServices

[<Struct; StructLayout(LayoutKind.Sequential)>]
type Point =
    val X: int32
    val Y: int32

[<Struct; StructLayout(LayoutKind.Sequential)>]
type Msg =
  val hwnd: IntPtr
  val message: uint32
  val wParam: uint64
  val lParam: int64
  val time: uint32
  val pt: Point
  val lPrivate: uint32

[<DllImport("user32.dll", SetLastError = true)>]
extern bool RegisterHotKey(IntPtr, int, uint32, int)

[<DllImport("user32.dll", SetLastError = true)>]
extern int GetMessage(Msg&, IntPtr, uint32, uint32)

let MOD_ALT = 0x0001u
let MOD_CONTROL = 0x0002u
let MOD_SHIFT = 0x004u
let MOD_NOREPEAT = 0x4000u
let WM_HOTKEY = 0x0312u
let VK_SPACE = 0x20

let register () =
  RegisterHotKey(IntPtr.Zero, 1, MOD_ALT ||| MOD_NOREPEAT, VK_SPACE) |> ignore


let getMessages form =
  async {
    register ()
    let mutable finished = false
    while not finished do
      let mutable msg = Msg()
      finished <- GetMessage(&msg, IntPtr.Zero, 0u, 0u) = 0
      if msg.message = WM_HOTKEY then
        Form.focus form
  }
  |> Async.Start


