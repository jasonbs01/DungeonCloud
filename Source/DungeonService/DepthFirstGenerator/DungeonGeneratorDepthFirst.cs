using DungeonCloud.DungeonCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCloud.DungeonService.DepthFirstGenerator
{
    enum CellContentsType
    {
        Bedrock,
        Room,
        Hall
    }

    internal struct CellContents
    {
        public CellContents(CellContentsType type, string name)
        {
            ContentType = type;
            Name = name;
        }

        public readonly CellContentsType ContentType;
        public readonly string Name;

        public override string ToString()
        {
            return string.Format("{0}:{1}", ContentType, Name);
        }
    }

    
    internal class DungeonGeneratorDepthFirst : IDungeonGenerator
    {
        #region Private Fields
        
        DungeonConfigEntity _configuration;
        IRandomizer _randomizer;
        CellContents[,] _dungeonBoard;
        int _roomCounter = 0;

        Dictionary<string, Room> _rooms;
        Dictionary<string, Hall> _halls;

        #endregion 

        #region Protected Properties

        protected string RoomPrefix { get; set; }

        protected string RoomSeperator { get; set; }

        protected int RoomCounter { get; set; }

        protected string DoorPrefix { get; set; }

        protected string DoorSeperator { get; set; }

        protected int DoorCounter { get; set; }

        protected string HallPrefix { get; set; }

        protected string HallSeperator { get; set; }

        protected int HallCounter { get; set; }

        protected int MaxPlacementRetries { get; set; }
        
        protected int Length
        {
            get { return _configuration.Size.Length; }           
        }

        protected int Width
        {
            get { return _configuration.Size.Width; }
        }

        protected string Name
        {
            get { return _configuration.Name; }
        }

        protected int MaxRoomSize
        {
            get 
            { 
                if(_maxRoomSize.HasValue == false)
                {
                    _maxRoomSize = ((Length / 4) * (Width / 4)) / 4;
                }

                return _maxRoomSize.Value;
            }
        }
        int? _maxRoomSize = null;


        #endregion 

        #region Constructors

        public DungeonGeneratorDepthFirst(DungeonConfigEntity configuration, IRandomizer randomizer)
        {
            _configuration = configuration;
            _randomizer = randomizer;
            _randomizer.SetSeed(_configuration.GenerationSeed);

            RoomPrefix = "R";
            RoomSeperator = "";
            RoomCounter = 0;

            DoorPrefix = "D";
            DoorSeperator = "";
            DoorCounter = 0;

            HallPrefix = "H";
            HallSeperator = "";
            HallCounter = 0;


            //TODO: get this from the config
            MaxPlacementRetries = 10;

            InitializeBoard();
        }
        
        #endregion

        #region IDungeonGenerator

        public Dungeon Generate()
        {
            //Generate a boring dungeon with a single room to start
            Dungeon result = new Dungeon(Name, Length, Width);
            bool firstRoomPlaced = false;
            Room room = null;  

            for (int i = 0; i < MaxPlacementRetries; i++)
            {
                room = CreateRoom();  
                if (CanPlaceRoom(room) == true)
                {
                    firstRoomPlaced = true;
                    break;
                }
            }

            if (firstRoomPlaced == true)
            {
                AddRoom(room);
                CreateConnectedRooms(room);
            }
            else
            {
                throw new ApplicationException("Unable to place the first room.");
            }

            result.Rooms = _rooms.Values.ToArray();
            result.Halls = _halls.Values.ToArray();

            return result;
        }


        #endregion 

        #region Private Methods

        private void CreateConnectedRooms(Room room)
        {
            List<Door> doors = CreateDoors(room);
            List<Door> connectedDoors = new List<Door>();
            foreach (Door door in doors)
            {
                bool roomPlaced = false;
                //tryplace room at end of hall 10 times
                for (int retry = 0; retry < MaxPlacementRetries; retry++)
                {                 
                    //generate hall
                    Hall hall = CreateHall(room, door);
                    if (hall == null)
                    {
                        //unable to find a hall out of this door
                        break;
                    }

                    Room nextRoom = CreateConnectedRoom(hall);
                    if (CanPlaceRoom(nextRoom) == true)
                    {
                        roomPlaced = true;
                        AddRoom(nextRoom);
                        AddHall(hall);                                                
                        door.ConnectedHallName = hall.Name;
                        hall.StartRoomName = room.Name;
                        //recursive make next rooms
                        CreateConnectedRooms(nextRoom);
                        //stop with the retries
                        break;
                    }                                        
                }
                //if not placed remove door
                if(roomPlaced == true)
                {
                    connectedDoors.Add(door);
                }
            }
            room.Doors = connectedDoors.ToArray();
        }

        private void AddHall(Hall hall)
        {
            _halls.Add(hall.Name, hall);
            
            foreach (Position segment in hall.Segments)
            {
                _dungeonBoard[segment.Column, segment.Row] = new CellContents(CellContentsType.Hall, hall.Name);        
            }
        }

        private void AddRoom(Room room)
        {
            _rooms.Add(room.Name, room);
            for (int column = 0; column < room.Size.Length; column++)
            {
                for (int row = 0; row < room.Size.Width; row++)
                {
                    int boardCol = column + room.Location.Column;
                    int boardRow = row + room.Location.Row;
                    _dungeonBoard[boardCol, boardRow] = new CellContents(CellContentsType.Room, room.Name);
                }   
            }
        }

        private Room CreateConnectedRoom(Hall hall)
        {                        
            Dimension roomSize = _randomizer.ChooseDimension(MaxRoomSize);
            Direction doorWall = GetOppositeDirection( CalculateFinalHallDirection(hall) );
            int doorPosition = _randomizer.ChooseCount(1, roomSize.Length);
            Position roomLocation = new Position(); ;
            Position lastSegment = hall.Segments[hall.Segments.Length - 1];

            switch (doorWall)
            {
                case Direction.North:
                    {
                        roomLocation.Row = lastSegment.Row + 1;
                        roomLocation.Column = lastSegment.Column - doorPosition; 
                        break;
                    }    
                case Direction.West:
                    {
                        roomLocation.Row = lastSegment.Row - doorPosition;
                        roomLocation.Column = lastSegment.Column - roomSize.Length;
                        break;
                    }
                case Direction.East:
                    {
                        roomLocation.Row = lastSegment.Row - doorPosition;
                        roomLocation.Column = lastSegment.Column + 1;
                        break;
                    }
                case Direction.South:
                    {
                        roomLocation.Row = lastSegment.Row - roomSize.Width;
                        roomLocation.Column = lastSegment.Column - doorPosition; 
                        break;
                    }    
                default:
                    throw new ApplicationException(string.Format("Unknown wall direction {0}", doorWall));
            }

            Door hallDoor = new Door(GetNextDoorName(), doorWall, doorPosition);

            Room resultRoom = new Room(GetNextRoomName(), roomLocation, roomSize)
                {
                    Doors = new Door[]{hallDoor}
                };

            //link up the references
            hallDoor.ConnectedHallName = hall.Name;
            hall.EndRoomName = resultRoom.Name;

            return resultRoom; 

        }

        private Direction GetOppositeDirection(Direction direction)
        {
            Direction result; 
            switch (direction)
            {
                case Direction.North:
                    result = Direction.South;
                    break;
                case Direction.West:
                    result = Direction.East;
                    break;
                case Direction.East:
                    result = Direction.West;
                    break;
                case Direction.South:
                    result = Direction.North;
                    break;
                default:
                    throw new ApplicationException(string.Format("Unknown direction {0}", direction));
            }

            return result;
        }

        private Direction CalculateFinalHallDirection(Hall hall)
        {
            Direction hallDirection;
            int count = hall.Segments.Length;
            Position last = hall.Segments[count - 1];
            Position prev = hall.Segments[count - 2];

            if(last.Row > prev.Row)
            {
                //---->
                hallDirection = Direction.West;
            }
            else if(last.Row < prev.Row)
            {
                //<----
                hallDirection = Direction.East;
            }
            else if(last.Column < prev.Column)
            {
                // ^
                // |
                hallDirection = Direction.North;
            }
            else if (last.Column > prev.Column)
            {
                hallDirection = Direction.South;
            }
            else
            {
                throw new ApplicationException("Hall is not flowing in a valid direction.");
            }

            return hallDirection;
        }

        private Hall CreateHall(Room room, Door door)
        {
            //Calculate the position of the first segment
            List<Position> segments = new List<Position>();
            Position start = new Position(room.Location);
            int hallLength = _randomizer.ChooseCount(2, 10);
            Hall result;

            switch (door.Wall)
            {
                case Direction.North:
                    {
                        start.Row--;
                        start.Column += door.Position - 1;
                        break;
                    }                    
                case Direction.West:
                    {
                        start.Row += door.Position - 1;
                        start.Column += room.Size.Length + 1;
                        break;
                    }
                case Direction.East:
                    {
                        start.Row += door.Position - 1;
                        start.Column--;
                        break;
                    }
                case Direction.South:
                    {
                        start.Row += room.Size.Width + 1;
                        start.Column += door.Position - 1; 
                        break;
                    }
                default:
                    throw new ApplicationException(string.Format("Unknown direction for wall {0}", door.Wall));
            }

            bool hallSuccess = false;
            segments.Add(start);
            hallLength--;
            for (int retry = 0; retry < MaxPlacementRetries; retry++)
            {
                Position prevSegment = start;
                Direction backtrackDirection = GetOppositeDirection(door.Wall);
                Direction prevBacktrackDirection = backtrackDirection;                
                if (segments.Count == hallLength + 1)
                {
                    //success!
                    hallSuccess = true;
                    break;
                }
                for (int i = 0; i < hallLength; i++)
                {
                    Direction nextDirection;
                    Position nextSegment = new Position(prevSegment);
                    do
                    {
                        nextDirection = _randomizer.ChooseDirection();
                    } while (nextDirection == backtrackDirection || nextDirection == prevBacktrackDirection);

                    switch (nextDirection)
                    {
                        case Direction.North:
                            nextSegment.Row--;
                            break;
                        case Direction.West:
                            nextSegment.Column++;
                            break;
                        case Direction.East:
                            nextSegment.Column--;
                            break;
                        case Direction.South:
                            nextSegment.Row++;
                            break;
                        default:
                            throw new ApplicationException(string.Format("Unknown next direction for hall {0}", door.Wall));
                    }                    
                    if (CanPlaceHallSegment(nextSegment) == false)
                    {
                        //reset the entire hall and retry
                        segments.Clear();
                        segments.Add(start);
                        break;
                    }
                    else
                    {
                        prevSegment = nextSegment;
                        prevBacktrackDirection = GetOppositeDirection(nextDirection);
                        segments.Add(nextSegment);
                    }
                    
                }
            }

            if(hallSuccess == true)
            {
                //hall was successfully placed create it
                result = new Hall(GetNextHallName());
                result.StartRoomName = room.Name;
                result.Segments = segments.ToArray();

                door.ConnectedHallName = result.Name;                
            }
            else
            {
                //hall could not be placed return null
                result = null;
            }

            return result;
        }

        private bool CanPlaceHallSegment(Position nextSegment)
        {
            bool result = false;

            if (nextSegment.Column <= 0 ||
                nextSegment.Column >= _configuration.Size.Length ||
                nextSegment.Row <= 0 ||
                nextSegment.Row >= _configuration.Size.Width)
            {
                //off the board
                result = false;
            }
            else
            {
                switch (_dungeonBoard[nextSegment.Column, nextSegment.Row].ContentType)
                {
                    case CellContentsType.Bedrock:
                        result = true;
                        break;
                    case CellContentsType.Room:
                        result = false;
                        break;
                    case CellContentsType.Hall:
                        result = true;
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        private bool CanPlaceRoom(Room room)
        {
            bool result = true;
            Position loc = room.Location;

            if(room.Location.Column <= 0 || 
                room.Location.Row <= 0)
            {
                //off the left or top of the board
               return false;
            }

            for (int roomColumn = 0; roomColumn < room.Size.Length; roomColumn++)
            {
                int boardColumn = roomColumn + loc.Column;    
                if(result == false)
                {
                    //inner loop broke so also need to break
                    break;
                }
                if (boardColumn >= _dungeonBoard.GetLength(0))
                {
                    //off the edge of the board can't place
                    result = false;
                    break;
                }
                for (int roomRow = 0; roomRow < room.Size.Width; roomRow++)
                {
                    int boardRow = roomRow + loc.Row;
                    if (boardRow >= _dungeonBoard.GetLength(1))
                    {
                        //off the bottom of the board can't place
                        result = false;
                        break;
                    }

                    if (_dungeonBoard[boardColumn, boardRow].ContentType != CellContentsType.Bedrock)
                    {
                        //something is already placed here
                        result = false;
                        break;
                    }                    
                }
            }

            return result;
        }


        private void InitializeBoard()
        {
            _dungeonBoard = new CellContents[Length, Width];
            _rooms = new Dictionary<string,Room>();
            _halls = new Dictionary<string, Hall>();
        }

        private Room CreateRoom()
        {
            Dimension roomSize = _randomizer.ChooseDimension(MaxRoomSize);
            Position roomLocation = _randomizer.ChoosePosition(Length, Width);
            Room result = new Room(GetNextRoomName(), roomLocation, roomSize);

            return result;            
        }

        private List<Door> CreateDoors(Room room)
        {
            Dictionary<string, Door> doors = new Dictionary<string, Door>();
            int doorCount = _randomizer.ChooseCount(1, 5);

            //add the doors that already exist in the room
            if (room.Doors != null)
            {
                foreach (Door existingDoor in room.Doors)
                {
                    string doorKey = string.Format("{0}_{1}", existingDoor.Wall, existingDoor.Position);
                    doors.Add(doorKey, existingDoor);
                }
            }

            for (int doorIndx = 0; doorIndx < doorCount; doorIndx++)
            {                
                Direction wall = _randomizer.ChooseDirection();
                int position;

                switch (wall)
                {
                    case Direction.North:
                    case Direction.South:
                    {
                       position = _randomizer.ChooseCount(1, room.Size.Length);
                       break;
                    }
                    

                    case Direction.West:
                    case Direction.East:
                    {
                        position = _randomizer.ChooseCount(1, room.Size.Width);
                        break;
                    }
                    default:
                    {
                        throw new ApplicationException(string.Format("Unknown direction {0} for placing a door", wall));
                    }
                }
                string doorKey = string.Format("{0}_{1}", wall, position);
                
                if(doors.ContainsKey(doorKey) == false)
                {
                    Door door = new Door(GetNextDoorName(), wall, position);
                    doors.Add(doorKey, door);
                }                                
            }

            return doors.Values.ToList();
        }

        private string GetNextRoomName()
        {
            return string.Format("{0}{1}{2}", RoomPrefix, RoomSeperator, ++RoomCounter);
        }

        private string GetNextDoorName()
        {
            return string.Format("{0}{1}{2}", DoorPrefix, DoorSeperator, ++DoorCounter);
        }

        private string GetNextHallName()
        {
            return string.Format("{0}{1}{2}", HallPrefix, HallSeperator, ++HallCounter);
        }

        #endregion 
    
    }
}
