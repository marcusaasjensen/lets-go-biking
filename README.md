# Lets Go Biking!
School Project by Marcus Aas Jensen.

## Before launching the app
Setup your API keys in the .env file inside the Server folder.
Replace the fields accordingly:
```dotenv
OPEN_ROUTE_SERVICE_API_KEY=your_api_key
JCDECAUX_API_KEY=your_api_key
```
## Launch the app simply
Launch the app by clicking on LetsGoBiking.bat.

## Launch the app manually
First, run both the Routing and the ProxyCache servers inside the associated folders:
- Server/ProxyCacheServer/bin/Release/ProxycacheServer.exe
- Server/RoutingServer/bin/Release/RoutingServer.exe

Secondly, open the command prompt in the Client folder and run in order:

```mvn clean install```
```mvn compile```
```mvn exec:java -Dexec.mainClass="com.soc.testwsclient.Main"```

*Or (using IntelliJ)*

Open the Client project inside IntelliJ idea, and run in the local terminal: 
```mvn clean install```
Finally, run the main class using the IntelliJ play button.

## To know
Map legend:
- Blue 🟦 : walking
- green 🟩 : bicycling

Enjoy.