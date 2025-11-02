using Microsoft.Xna.Framework;
using TopiLabo15_PVZ.Classes.Entities;

namespace TopiLabo15_PVZ.Classes.World
{
    public class Tile
    {
        public Rectangle Bounds { get; }
        public Plant Occupant { get; set; }
        public bool IsOccupied => Occupant != null;

        public int Row { get; }
        public int Col { get; }

        public Tile(Rectangle bounds, int row, int col) // Constructor actualizado
        {
            Bounds = bounds;
            Occupant = null;
            Row = row;
            Col = col;
        }
    }
}