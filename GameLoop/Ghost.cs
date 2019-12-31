using System;
using System.Collections.Generic;

namespace GameLoop
{
    class Ghost : MapPiece
    {
        List<MapPiece> open = new List<MapPiece>();
        HashSet<MapPiece> closed = new HashSet<MapPiece>();
        List<MapPiece> neibors = new List<MapPiece>();
        List<MapPiece> allPieces = new List<MapPiece>();
        public Ghost(int x, int y, List<MapPiece> allMapPieces)
        {
            PosX = x;
            PosY = y;
            Visuals = 'U';
            BoxCollider = new int[4] { PosX, PosY, PosX + 1, PosY + 1 };
            allPieces = allMapPieces;
        }

        public List<MapPiece> CalcuatePath(MapPiece target)
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

                if (current == target)
                {
                    return TracePath(target);
                }

                for (int c = 0; c < allPieces.Count; c++)
                {
                    if ((allPieces[c].PosX == current.PosX + 1 && allPieces[c].PosY == current.PosY) ||
                        (allPieces[c].PosX == current.PosX - 1 && allPieces[c].PosY == current.PosY) ||
                        (allPieces[c].PosY == current.PosY + 1 && allPieces[c].PosX == current.PosX) ||
                        (allPieces[c].PosY == current.PosY - 1 && allPieces[c].PosX == current.PosX))
                    {
                        neibors.Add(allPieces[c]);
                    }
                }

                for (int b = 0; b < neibors.Count; b++)
                {
                    if (neibors[b].GetType() == typeof(Map) || closed.Contains(neibors[b]))
                    {
                        continue;
                    }

                    int newCostMov = current.g_Cost + GetDistace(current, neibors[b]);
                    if (newCostMov < neibors[b].g_Cost || !open.Contains(neibors[b]))
                    {
                        neibors[b].g_Cost = newCostMov;
                        neibors[b].h_Cost = GetDistace(neibors[b], target);
                        neibors[b].parent = current;

                        if (!open.Contains(neibors[b]))
                            open.Add(neibors[b]);
                    }
                }
            }
            return null;
        }
        private List<MapPiece> TracePath(MapPiece end)
        {
            List<MapPiece> path = new List<MapPiece>();
            MapPiece currentPiece = end;

            while (currentPiece != this)
            {
                path.Add(currentPiece);
                currentPiece = currentPiece.parent;
            }
            path.Reverse();
            return path;
        }
        private int GetDistace(MapPiece A, MapPiece B)
        {
            int distanceX = Math.Abs(A.PosX - B.PosX);
            int distanceY = Math.Abs(A.PosY - B.PosY);

            if (distanceX > distanceY)
                return 14 * distanceY + 10 * (distanceX - distanceY);
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }
    }
}
