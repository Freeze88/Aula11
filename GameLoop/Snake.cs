namespace GameLoop
{
    class Snake : MapPiece
    {
        public Snake()
        {
            PosX = 2;
            PosY = 1;
            Visuals = '#';
            BoxCollider = new int[4] { PosX, PosY, PosX + 1, PosY + 1 };
        }
    }
}
