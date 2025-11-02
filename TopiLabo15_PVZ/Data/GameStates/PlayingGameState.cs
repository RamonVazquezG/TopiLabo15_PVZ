// TopiLabo15_PVZ/Data/GameStates/PlayingGameState.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Collections.Generic; // Añadido
using TopiLabo15_PVZ.Data.Others;
using TopiLabo15_PVZ.Data.Plants;
using TopiLabo15_PVZ.Data.Zombies;
using TopiLabo15_PVZ.Classes.UI;      // Añadido
using TopiLabo15_PVZ.Classes.World;   // Añadido
using TopiLabo15_PVZ.Data;            // Añadido (para PlantSubtypes y UIManager)
using TopiLabo15_PVZ.Classes.Entities; // Añadido (para Plant)


namespace TopiLabo15_PVZ.Data.GameStates
{
    public class PlayingGameState : GameState
    {
        // --- Variables Originales ---
        private int SunCount = 150; // Empezamos con 150 de sol
        private float SunGenerationTimer = 0f;
        SpriteAnimator patio;
        Mouse mouseEntity;

        // --- Nuevos Sistemas para Plantar ---
        private Grid _grid;
        private List<SeedPacket> _seedPackets;
        private PlantSubtypes? _selectedPlantSubtype; // La planta en el cursor

        public PlayingGameState() : base()
        {
        }

        public override void OnInIt()
        {
            Console.WriteLine("PlayingGameState init");

            patio = new SpriteAnimator("patio", "default");

            // Crea la entidad del mouse (¡importante!)
            this.mouseEntity = new Mouse(this.EntityManager);

            // --- Inicializa el Grid ---
            // Ajusta estos valores a tu juego
            int rows = 5;
            int cols = 9;
            int tileWidth = 80;
            int tileHeight = 100;
            Vector2 gridOffset = new Vector2(100, 150); // Dónde empieza el tablero
            _grid = new Grid(rows, cols, tileWidth, tileHeight, gridOffset);

            // --- Inicializa los Paquetes ---
            _seedPackets = new List<SeedPacket>();
            _seedPackets.Add(new SeedPacket(PlantSubtypes.SunFlower, 50, 5.0f, UIManager.TexPacketSunflower, UIManager.TexCooldownOverlay));
            _seedPackets.Add(new SeedPacket(PlantSubtypes.PeaShooter, 100, 7.5f, UIManager.TexPacketPeashooter, UIManager.TexCooldownOverlay));
            _seedPackets.Add(new SeedPacket(PlantSubtypes.WallNut, 50, 20.0f, UIManager.TexPacketWalnut, UIManager.TexCooldownOverlay));

            // --- ¡YA NO PLANTAMOS POR DEFECTO! ---
            // Peashooter peashooter = new Peashooter(this.EntityManager, 1, 1);
            // Sunflower sunflower = new Sunflower(this.EntityManager, 0, 1);
            // Walnut walnut = new Walnut(this.EntityManager, 8, 3);

            // Los zombies de prueba y soles sí los dejamos por ahora
            NormalZombie zombie = new NormalZombie(this.EntityManager, 1);
            zombie = new NormalZombie(this.EntityManager, 2);
            zombie = new NormalZombie(this.EntityManager, 3);
            SunPickup sun = new SunPickup(this, this.EntityManager, 4, 2);
        }

        public void IncrementSunCount(int amount)
        {
            SunCount += amount;
            Debug.WriteLine($"Sun Count: {SunCount}");
        }

        public override void PreUpdateCallback(float dt)
        {
            // --- Lógica Original: Generar soles del cielo ---
            SunGenerationTimer += dt;
            if (SunGenerationTimer >= 10f)
            {
                SunGenerationTimer = 0f;
                SunPickup sun = new SunPickup(this, this.EntityManager, Random.Shared.Next(0, 9), Random.Shared.Next(0, 5));
            }

            // --- Nueva Lógica: Actualizar Cooldowns ---
            foreach (var packet in _seedPackets)
            {
                packet.Update(dt);
            }

            // --- Nueva Lógica: Manejar Input ---
            HandleInput();
        }

        // TopiLabo15_PVZ/Data/GameStates/PlayingGameState.cs

        private void HandleInput()
        {
            // --- CORRECCIÓN ---
            // Usamos las propiedades estáticas de tu clase MouseInput
            Vector2 mousePos = MouseInput.Position;

            // Cancelar selección con clic derecho
            if (MouseInput.RightButtonPressed) // ¡Ahora existe!
            {
                _selectedPlantSubtype = null;
            }

            // Clic izquierdo
            if (MouseInput.LeftButtonPressed)
            {
                // --- 1. ¿Clic en un Paquete? ---
                bool clickedPacket = false;
                foreach (var packet in _seedPackets)
                {
                    if (packet.Bounds.Contains(mousePos) && packet.CanSelect(SunCount))
                    {
                        _selectedPlantSubtype = packet.PlantSubtype;
                        clickedPacket = true;
                        break;
                    }
                }

                // --- 2. ¿Clic en el Tablero (para plantar)? ---
                if (!clickedPacket && _selectedPlantSubtype != null)
                {
                    Tile clickedTile = _grid.GetTileAt(mousePos);

                    if (clickedTile != null && !clickedTile.IsOccupied)
                    {
                        SeedPacket packetToUse = _seedPackets.Find(p => p.PlantSubtype == _selectedPlantSubtype.Value);

                        if (packetToUse.CanSelect(SunCount))
                        {
                            Plant newPlant = CreatePlant(_selectedPlantSubtype.Value, clickedTile.Col, clickedTile.Row);

                            if (newPlant != null)
                            {
                                _grid.PlantAt(clickedTile, newPlant);
                                SunCount -= packetToUse.Cost;
                                packetToUse.StartCooldown();
                                _selectedPlantSubtype = null;
                            }
                        }
                    }
                }

                // --- 3. ¿Clic en un Sol? ---
                // Esto lo debe manejar tu entidad Mouse (mouseEntity)
                // colisionando con la entidad SunPickup.
            }
        }

        // Fábrica para crear la planta y añadirla al EntityManager
        private Plant CreatePlant(PlantSubtypes subtype, int col, int row)
        {
            switch (subtype)
            {
                case PlantSubtypes.PeaShooter:
                    return new Peashooter(this.EntityManager, col, row);
                case PlantSubtypes.SunFlower:
                    return new Sunflower(this.EntityManager, col, row);
                case PlantSubtypes.WallNut:
                    return new Walnut(this.EntityManager, col, row);
            }
            return null;
        }

        public override void PreDrawCallback(SpriteBatch spriteBatch)
        {
            // 1. Dibuja el fondo (original)
            patio.Draw(spriteBatch, Vector2.Zero);

            // 2. Dibuja la UI (nuevo)
            DrawUI(spriteBatch);
        }

        private void DrawUI(SpriteBatch spriteBatch)
        {
            // Dibuja la barra de paquetes
            Vector2 packetPosition = new Vector2(20, 10);
            foreach (var packet in _seedPackets)
            {
                packet.Draw(spriteBatch, packetPosition);
                packetPosition.X += packet.Bounds.Width + 10;
            }

            // Dibuja el contador de Sol
            string sunText = $"{SunCount}";
            spriteBatch.DrawString(UIManager.GameFont, sunText, new Vector2(packetPosition.X + 52, 27), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.91f);
            spriteBatch.DrawString(UIManager.GameFont, sunText, new Vector2(packetPosition.X + 50, 25), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);

            // Dibuja la planta "fantasma" en el cursor
            if (_selectedPlantSubtype != null)
            {
                Texture2D ghostTexture = GetGhostTexture(_selectedPlantSubtype.Value);
                if (ghostTexture != null)
                {
                    Vector2 ghostPos = mouseEntity.Position - new Vector2(ghostTexture.Width / 2, ghostTexture.Height / 2);

                    Color ghostColor = Color.White * 0.5f;
                    Tile targetTile = _grid.GetTileAt(mouseEntity.Position);

                    if (targetTile != null)
                    {
                        ghostPos = targetTile.Bounds.Center.ToVector2() - new Vector2(ghostTexture.Width / 2, ghostTexture.Height / 2);
                        if (targetTile.IsOccupied)
                        {
                            ghostColor = Color.Red * 0.5f;
                        }
                    }
                    spriteBatch.Draw(ghostTexture, ghostPos, null, ghostColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
                }
            }
        }

        // Método de ayuda para el fantasma
        private Texture2D GetGhostTexture(PlantSubtypes subtype)
        {
            switch (subtype)
            {
                case PlantSubtypes.PeaShooter: return UIManager.TexGhostPeashooter;
                case PlantSubtypes.SunFlower: return UIManager.TexGhostSunflower;
                case PlantSubtypes.WallNut: return UIManager.TexGhostWalnut;
                default: return null;
            }
        }
    }
}