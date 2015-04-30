using DungeonCloud.DungeonCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCloud.DungeonService
{
    internal class Randomizer : IRandomizer
    {
        Random _random;
        
        public Randomizer()
        {
            _random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
        }

        public void SetSeed(long seed)
        {
            _random = new Random((int)seed & 0x0000FFFF);
        }

        public Dimension ChooseDimension(int maxSize)
        {
            int length = _random.Next(2, maxSize + 1);
            int width = _random.Next(2, maxSize + 1);;
            Dimension result = new Dimension(length, width);

            return result;
        }

        public Position ChoosePosition(int length, int width)
        {
            int row = _random.Next(length + 1);
            int column = _random.Next(length + 1);
            Position result = new Position(row, column);

            return result;
        }

        public int ChooseCount(int min, int max)
        {
            return _random.Next(min, max + 1);
        }

        public Direction ChooseDirection()
        {
            return (Direction)_random.Next((int)Direction.North, (int)Direction.East + 1);
        }
    }
}
