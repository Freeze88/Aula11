using System;
using System.Collections.Generic;
using System.Text;

namespace GameLoop
{
    class Snake
    {
        public int length { get; set; }
        public int posX { get; set; }
        public int posY { get; set; }
        public char visuals { get; }

        public Snake()
        {
            posX = 0;
            posY = 0;
            length = 0;
            visuals = '#';
        }
    }
}
