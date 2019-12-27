namespace GameLoop
{
    class Teleporter : MapPiece
    {
        public Teleporter(int x, int y)
        {
            PosX = x;
            PosY = y;
            Visuals = 'T';
            BoxCollider = new int[4] { PosX, PosY, PosX + 1, PosY + 1 };
        }
    }
}
