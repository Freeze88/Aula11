using System;
using System.Collections.Generic;

namespace GameLoop
{
    class Ghost : MapPiece
    {
        List<MapPiece> open = new List<MapPiece>();
        List<MapPiece> closed = new List<MapPiece>();
        List<MapPiece> neibors = new List<MapPiece>();
        List<MapPiece> allPieces = new List<MapPiece>();
        public Ghost(int x, int y, List<MapPiece> allMapPieces)
        {
            position = new Vector2(x, y);
            Visuals = 'U';
            BoxCollider = new int[4] { x, y, x + 1, y + 1 };
            allPieces = allMapPieces;
        }

        public List<Vector2> CalcuatePath(MapPiece target)
        {
            open.Clear();
            closed.Clear();
            neibors.Clear();
            open.Add(this);

            while (open.Count > 0)
            {
                MapPiece current = open[0];

                for (int i = 1; i < open.Count; i++)
                {
                    if (open[i].f_Cost < current.f_Cost || open[i].f_Cost == current.f_Cost)
                        if (open[i].h_Cost < current.h_Cost)
                            current = open[i];
                }

                open.Remove(current);
                closed.Add(current);

                if (current.position.Equals(target.position))
                {
                    return TracePath(target);
                }

                for (int c = 0; c < allPieces.Count; c++)
                {
                    if ((allPieces[c].position.PosX == current.position.PosX + 1 && allPieces[c].position.PosY == current.position.PosY) ||
                        (allPieces[c].position.PosX == current.position.PosX - 1 && allPieces[c].position.PosY == current.position.PosY) ||
                        (allPieces[c].position.PosY == current.position.PosY + 1 && allPieces[c].position.PosX == current.position.PosX) ||
                        (allPieces[c].position.PosY == current.position.PosY - 1 && allPieces[c].position.PosX == current.position.PosX))
                    {
                        if (current.parent != allPieces[c])
                            neibors.Add(allPieces[c]);
                    }
                }

                for (int b = 0; b < neibors.Count; b++)
                {
                    if (neibors[b].GetType() == typeof(Map) || closed.Contains(neibors[b]))
                    {
                        continue;
                    }

                    int newCostMov = current.g_Cost + GetDistace(current.position, neibors[b].position);
                    if (newCostMov < neibors[b].g_Cost || !open.Contains(neibors[b]))
                    {
                        neibors[b].g_Cost = newCostMov;
                        neibors[b].h_Cost = GetDistace(neibors[b].position, target.position);
                        neibors[b].parent = current;

                        if (!open.Contains(neibors[b]))
                            open.Add(neibors[b]);
                    }
                }
                neibors.Clear();
            }
            return null;
        }
        private List<Vector2> TracePath(MapPiece end)
        {
            List<Vector2> path = new List<Vector2>();
            MapPiece currentPiece = end;

            while (currentPiece != this)
            {
                path.Add(currentPiece.position);
                currentPiece = currentPiece.parent;
            }
            path.Reverse();
            return path;
        }
        private int GetDistace(Vector2 A, Vector2 B)
        {
            int distanceX = Math.Abs(A.PosX - B.PosX);
            int distanceY = Math.Abs(A.PosY - B.PosY);

            if (distanceX > distanceY)
                return 14 * distanceY + 10 * (distanceX - distanceY);
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }
    }
}
