$result = curl -v -H "Content-Type: application/json" -X POST --data '@DungeonConfig.json' 'http://localhost:8089/DungeonCloud/Dungeons'

curl -v -H "Content-Type: application/json" -X GET "http://localhost:8089/DungeonCloud/Dungeons/$result" | clip
