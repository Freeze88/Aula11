namespace GameLoop
{
    class Snake : MapPiece
    {
        public Snake()
        {
            PosX = 25;
            PosY = 1;
            Visuals = 'c';
            BoxCollider = new int[4] { PosX, PosY, PosX + 1, PosY + 1 };
        }
    }
}
