namespace GameLoop
{
    class Snake : MapPiece
    {
        public Snake()
        {
            position = new Vector2(25, 1);
            Visuals = 'c';
            BoxCollider = new int[4] { position.PosX, position.PosY, position.PosX + 1, position.PosY + 1 };
        }
    }
}
