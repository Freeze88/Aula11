namespace GameLoop
{
    class Pellet : MapPiece
    {
        public Pellet(int x, int y)
        {
            PosX = x;
            PosY = y;
            Visuals = '.';
            BoxCollider = new int[4] { PosX, PosY, PosX + 1, PosY + 1 };
        }
    }
}
