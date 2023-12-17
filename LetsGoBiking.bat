@echo off

title Lets Go Biking!

cd .\Server

start "Proxy Cache Server" .\ProxyCacheServer\bin\Release\ProxyCacheServer.exe
start "Routing Server" .\RoutingServer\bin\Release\RoutingServer.exe

cd ..\Client

call mvn clean install

call mvn compile

echo Ready to run.
pause

cls

call mvn exec:java -Dexec.mainClass="com.soc.testwsclient.Main"

timeout /nobreak /t 2 >nul

exit