using DungeonCloud.DungeonCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCloud.DungeonService.DepthFirstGenerator
{
    enum CellContents
    {
        Bedrock,
        Room,
        Hall
    }

    internal class DungeonGeneratorDepthFirst : IDungeonGenerator
    {
        #region Private Fields
        
        DungeonConfigEntity _configuration;
        IRandomizer _randomizer;
        CellContents[,] _dungeonBoard;
        int _roomCounter = 0;

        #endregion 

        #region Protected Properties

        protected string RoomPrefix { get; set; }

        protected string RoomSeperator { get; set; }

        protected int RoomCounter { get; set; }

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

            InitializeBoard();
        }
        
        #endregion

        #region IDungeonGenerator

        public Dungeon Generate()
        {
            //Generate a boring dungeon with a single room to start
            Room room = CreateRoom();
            Dungeon result = new Dungeon(Name, Length, Width);

            result.Rooms = new Room[] { room };

            return result;
        }

        #endregion 

        #region Private Methods

        private void InitializeBoard()
        {
            _dungeonBoard = new CellContents[Length, Width];
        }

        private Room CreateRoom()
        {
            Dimension roomSize = _randomizer.ChooseDimension(MaxRoomSize);
            Position roomLocation = _randomizer.ChoosePosition(Length, Width);
            Room result = new Room(GetNextRoomName(), roomLocation, roomSize);

            return result;            
        }

        private string GetNextRoomName()
        {
            return string.Format("{0}{1}{2}", RoomPrefix, RoomSeperator, ++RoomCounter);
        }

        #endregion 
    
    }
}
