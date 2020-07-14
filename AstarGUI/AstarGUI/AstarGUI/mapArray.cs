using System;
using System.Collections.Generic;
using System.Text;
using Astar_Algorithm;

namespace AstarGUI
{
    public class mapArray
    {
        public int Size { get; set; }
        public NodeInformation[,] map { get; set; }

        public mapArray(int size)
        {
            Size = size;

            map = new NodeInformation[Size + 2, Size + 2];

            for(int i = 1; i <= Size; i++)
            {
                for(int j = 1; j <= Size; j++)
                {
                    map[i,j] = new NodeInformation { Y = i, X = j};
                }
            }
        }
    }
}
