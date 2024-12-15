
namespace LibraryManagementSystem

open System
open System.Windows.Forms

module Program =
    [<EntryPoint>]
    let main argv =
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(false)
        Application.Run(new GUI.MainForm())
        0