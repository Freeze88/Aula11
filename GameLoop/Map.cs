namespace GameLoop
{
    class Map : MapPiece
    {
        public Map(int x, int y, int l, int w)
        {
            PosX = x;
            PosY = y;
            Visuals = 'O';
            BoxCollider = new int[4] { x, y, l, w };
        }
    }
}
