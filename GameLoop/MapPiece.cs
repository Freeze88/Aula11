namespace GameLoop
{
    class MapPiece
    {
        public int[] BoxCollider;
        public int PosX { get; set; }
        public int PosY { get; set; }
        public char Visuals { get; protected set; }

        public void UpdatePhysics(Snake s)
        {
            BoxCollider[0] = s.PosX;
            BoxCollider[1] = s.PosY;
            BoxCollider[2] = s.PosX + 1;
            BoxCollider[3] = s.PosY + 1;
        }
    }
}
