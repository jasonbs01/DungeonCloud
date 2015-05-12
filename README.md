# DungeonCloud
Generate dungeons in the cloud.  A RESTful dungeon generator.

#Overview
The goal of this project is to create a RESTful Roguelike dungeon generator. What is a Rouguelike Dungeon generator click here http://lmgtfy.com/?q=Roguelike+Dungeon+Generator?  

#To Build and Run
1. This is written in Visual Studio 2013 targeting .Net 4.5.  Trying to build and run in any other enviroment is at your own risk
2. Make sure you have nuget installed and package restore enabled.  The solution is configured so that all external dependencies should be restored at compile time via nuget
3. DungeonCloudConsole is a self hosted WCF console application that exposes the RESTful interface on localhost.  Set it to be the startup project, build the solution, and press F5 to run the console.
4. The dungeon resource will be available at http://localhost:8080/DungeonCloud/Dungeons .

#Basic API Usage 
To create a new dungeon POST a dungeon configuration to the dungeons resource.  
```Batchfile
curl -v -H "Content-Type: application/json" -X POST --data "@DungeonConfig.json" http://localhost:8089/DungeonCloud/Dungeons
```

A new id will be returned such as MyDungeon_1.

Then to retrieve the dungeon generated from that config GET at the id url:
```Batchfile
curl -v -H "Content-Type: application/json" -X GET http://localhost:8089/DungeonCloud/Dungeons/MyDungeon_1
```

#Detailed Design
Detailed design and other documentation is available as a Google doc at the link below: https://drive.google.com/folderview?id=0B23RIIlOv0XYfmpucnZDMnVGdnczdnBzcW9OTTlXLWg5OGljaUdoaEZtSl80WE1qTGNHbkk&usp=sharing

test

