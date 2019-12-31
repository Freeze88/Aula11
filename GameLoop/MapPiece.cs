namespace GameLoop
{
    class MapPiece
    {
        public int[] BoxCollider;
        public int PosX { get; set; }
        public int PosY { get; set; }
        public char Visuals { get; set; }

        public int g_Cost = 0;

        public int h_Cost = 0;
        public int f_Cost { get => g_Cost + h_Cost; }

        public MapPiece parent;
        public void UpdatePhysics(MapPiece s)
        {
            BoxCollider[0] = s.PosX;
            BoxCollider[1] = s.PosY;
            BoxCollider[2] = s.PosX + 1;
            BoxCollider[3] = s.PosY + 1;
        }
    }
}
