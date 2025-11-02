// TopiLabo15_PVZ/Classes/World/Grid.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TopiLabo15_PVZ.Classes.Entities; // Usamos el namespace correcto

namespace TopiLabo15_PVZ.Classes.World
{
    public class Grid
    {
        private Tile[,] _tiles;
        private int _rows;
        private int _cols;

        public Grid(int rows, int cols, int tileWidth, int tileHeight, Vector2 offset)
        {
            _rows = rows;
            _cols = cols;
            _tiles = new Tile[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Rectangle bounds = new Rectangle(
                        (int)offset.X + c * tileWidth,
                        (int)offset.Y + r * tileHeight,
                        tileWidth,
                        tileHeight);

                    _tiles[r, c] = new Tile(bounds, r, c);
                }
            }
        }

        public Tile GetTileAt(Vector2 screenPosition)
        {
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    if (_tiles[r, c].Bounds.Contains(screenPosition))
                    {
                        return _tiles[r, c];
                    }
                }
            }
            return null; // No se encontró
        }

        // Esta función ahora es mucho más simple.
        // El EntityManager se encargará de crear y actualizar la planta.
        public bool PlantAt(Tile tile, Plant newPlant)
        {
            if (tile == null || tile.IsOccupied)
            {
                return false;
            }

            tile.Occupant = newPlant;
            return true;
        }

        // Agregamos una función para limpiar el tile cuando la planta muera
        public void ClearTile(Tile tile)
        {
            if (tile != null)
            {
                tile.Occupant = null;
            }
        }
    }
}