@ECHO OFF

set rootpath=%~dp0
set destinationprod="C:\inetpub\wwwroot\RRHH"
set destination=""C:\Users\Adrian Rojas\Desktop\dev\wwwAddon""

call :strLen rootpath strlen
set /a strlen=%strlen%-8

CALL SET prevpath=%%rootpath:~0,%strlen%%%
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe  "C:\Users\Adrian Rojas\Desktop\Project\AddonSolicitudesCompras\AddonSolicitudesCompras.sln" /p:Configuration=Debug /p:Platform="Any CPU" /p:VisualStudioVersion=12.0 /t:Rebuild


:: rmdir /s /q "%destination%\"

mkdir "%destination%\Areas"
robocopy "%rootpath%\Areas" "C:\Users\Adrian Rojas\Desktop\dev\wwwAddon\Areas" /E /COPYALL /it /NFL /NDL /NJH /NJS /nc /ns /np

mkdir "%destination%\bin"
robocopy "%rootpath%\bin" "C:\Users\Adrian Rojas\Desktop\dev\wwwAddon\bin" /E /COPYALL /it /NFL /NDL /NJH /NJS /nc /ns /np

mkdir "%destination%\Content"
robocopy "%rootpath%\Content" "C:\Users\Adrian Rojas\Desktop\dev\wwwAddon\Content" /E /COPYALL /it /NFL /NDL /NJH /NJS /nc /ns /np

mkdir "%destination%\fonts"
robocopy "%rootpath%\fonts" "C:\Users\Adrian Rojas\Desktop\dev\wwwAddon\fonts" /E /COPYALL /it /NFL /NDL /NJH /NJS /nc /ns /np

mkdir "%destination%\Scripts"
robocopy "%rootpath%\Scripts" "C:\Users\Adrian Rojas\Desktop\dev\wwwAddon\Scripts" /E /COPYALL /it /NFL /NDL /NJH /NJS /nc /ns /np

mkdir "%destination%\Views"
robocopy "%rootpath%\Views" "C:\Users\Adrian Rojas\Desktop\dev\wwwAddon\Views" /E /COPYALL /it /NFL /NDL /NJH /NJS /nc /ns /np

robocopy "%rootpath%\" "C:\Users\Adrian Rojas\Desktop\dev\wwwAddon\\" favicon.ico /COPYALL /it /NFL /NDL /NJH /NJS /nc /ns /np
robocopy "%rootpath%\" "C:\Users\Adrian Rojas\Desktop\dev\wwwAddon\\" Global.asax /COPYALL /it /NFL /NDL /NJH /NJS /nc /ns /np
robocopy "%rootpath%\" "C:\Users\Adrian Rojas\Desktop\dev\wwwAddon\\" "packages.config" /COPYALL /it /NFL /NDL /NJH /NJS /nc /ns /np
robocopy "%rootpath%\" "C:\Users\Adrian Rojas\Desktop\dev\wwwAddon\\" "Web.config" /COPYALL /it /NFL /NDL /NJH /NJS /nc /ns /np



echo "@{    Layout = "";   }" > "C:\Users\Adrian Rojas\Desktop\dev\wwwAddon\Views\Home\Index.cshtml"
type "C:\Users\Adrian Rojas\Desktop\dev\wwwAddon\Views\Home\index.html" >> "C:\Users\Adrian Rojas\Desktop\dev\www2\Views\Home\Index.cshtml"

ECHO ON
exit /b

:strLen
setlocal enabledelayedexpansion
:strLen_Loop
  if not "!%1:~%len%!"=="" set /A len+=1 & goto :strLen_Loop
(endlocal & set %2=%len%)
goto :eof
