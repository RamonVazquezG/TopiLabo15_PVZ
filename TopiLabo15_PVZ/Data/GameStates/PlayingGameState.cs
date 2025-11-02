using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Linq; // NECESARIO para LINQ (Any, FirstOrDefault)
using System.Collections.Generic; // NECESARIO para List<T>
using TopiLabo15_PVZ.Classes.Entities;
using TopiLabo15_PVZ.Data.Others;
using TopiLabo15_PVZ.Data.Plants;
using TopiLabo15_PVZ.Data.Zombies;
using TopiLabo15_PVZ.Data.UI;


namespace TopiLabo15_PVZ.Data.GameStates
{
    public class PlayingGameState : GameState

    {
        // 🚨 CORRECCIÓN CS0122: Hacemos SunCount público (get) para SeedPacketUI
        public int SunCount { get; private set; } = 100;
        private float SunGenerationTimer = 0f;

        private int WaveCounter = 1;

        private int NextZombieLane = Random.Shared.Next(0, 5);
        private float ZombieGenerationTimer = 0f;
        private Vector2 ZombieRate = new Vector2(45f, 15f);
        private float NextZombieRate;
        private bool IsOnFinalWave = false;

        private float WaveTimeProgress = 0f;
        private float WaveDuration = 1f; // La duracion se define constantemente en PreUpdateCallback().

        SpriteAnimator BGPatio; //Nunca inicialicen sprites aquí. Haganlo en OnInit.
        SpriteAnimator UIWaveBar;
        SpriteAnimator UISun;

        // ¡NUEVO! Referencia a la fuente
        private SpriteFont _pixelFont;

        // INICIO: CAMPOS DE PLANTADO/DESPLANTADO
        public Mouse MouseEntity { get; private set; } // Referencia al mouse para interacción
        public PlantSubtypes? SelectedPlantType { get; set; } = null; // Planta seleccionada
        private List<SeedPacketUI> seedPackets = new List<SeedPacketUI>(); // Lista de paquetes

        public ShovelUI ShovelEntity { get; private set; } // ¡NUEVO! Referencia a la pala

        // Constantes del tablero
        private readonly Vector2 BoardOffset;
        private readonly float TileSize;
        // FIN: CAMPOS DE PLANTADO/DESPLANTADO

        // NOTA: Se necesita una SpriteFont para dibujar texto en DrawString.
        // public SpriteFont myFont; 

        public PlayingGameState() : base()
        {
            NextZombieRate = ZombieRate.X;

            // INICIO: Inicializar constantes del tablero
            // Hacemos que la clase PlayingGameState tenga sus propias copias inmutables de estas constantes
            // para que los métodos como GetTileAtMouse sean más limpios.
            this.TileSize = Globals.TILE_SIZE;
            this.BoardOffset = new Vector2(this.TileSize * 1.0f, this.TileSize * 2.0f);
            // FIN: Inicializar constantes del tablero
        }

        private int GetNextZombieLane() //Metodo para obtener la siguiente línea de zombies de forma aleatoria pero sin repetir la misma línea consecutivamente
        {
            NextZombieLane = (NextZombieLane + Random.Shared.Next(1, 5)) % 5;
            return NextZombieLane;
        }

        private float GetNextZombieRate()
        {
            float divide = MathF.Pow(WaveCounter, 1.75f); //Aumenta la dificultad con cada ola, aumentando la velocidad de aparición de los zombies por un 3% por ola.
            return Globals.Lerp(ZombieRate.X, ZombieRate.Y, WaveTimeProgress / WaveDuration) / divide;
        }

        public override void OnInIt()
        {
            Console.WriteLine("PlayingGameState init");

            // --- ¡CORRECCIÓN! Usando GameManager.GameInstance para cargar fuente ---
            if (GameManager.GameInstance is Game1 game1)
            {
                this._pixelFont = game1._pixelFont;
            }

            BGPatio = new SpriteAnimator("patio", "default");
            UIWaveBar = new SpriteAnimator("uiWaveBar", "waveBarBox");
            UISun = new SpriteAnimator("uiSun", "default");

            this.MouseEntity = new Mouse(this.EntityManager);

            // INICIO: Inicializar paquetes de semillas y pala
            float startX = 38f;
            float offsetY = 12f;
            float packetSpacing = 24f;
            int numSeedPackets = 5; // Hay 5 paquetes en el mockup

            // Paquetes de Semillas
            seedPackets.Add(new SeedPacketUI(this.EntityManager, this, PlantSubtypes.PeaShooter, new Vector2(startX + 0 * packetSpacing, offsetY), this._pixelFont));
            seedPackets.Add(new SeedPacketUI(this.EntityManager, this, PlantSubtypes.SunFlower, new Vector2(startX + 1 * packetSpacing, offsetY), this._pixelFont));
            seedPackets.Add(new SeedPacketUI(this.EntityManager, this, PlantSubtypes.WallNut, new Vector2(startX + 2 * packetSpacing, offsetY), this._pixelFont));
            // Se asume que el cuarto y el quinto paquete son los siguientes subtipos en el enum o placeholders
            // Para el cálculo de posición, solo nos importa el número y el espaciado

            // Calculando la posición de la pala:
            // Coordenadas exactas: x=158, y=12 (Corregido por el usuario)
            float shovelX = 158f;
            float shovelY = 12f;

            this.ShovelEntity = new ShovelUI(this.EntityManager, this, new Vector2(shovelX, shovelY));

            // FIN: Inicializar paquetes de semillas y pala
        }

        public void IncrementSunCount(int amount)
        {
            SunCount += amount;
            Debug.WriteLine($"Sun Count: {SunCount}");
        }

        // Función para obtener las coordenadas de la casilla del tablero bajo el mouse.
        // Devuelve (x, y) en coordenadas de tablero (0-8, 0-4).
        private (int boardX, int boardY) GetTileAtMouse()
        {
            Vector2 mouseWorldPos = MouseEntity.Position;
            int boardX = (int)MathF.Floor((mouseWorldPos.X - BoardOffset.X) / TileSize);
            int boardY = (int)MathF.Floor((mouseWorldPos.Y - BoardOffset.Y) / TileSize);
            return (boardX, boardY);
        }

        // Función para verificar si la posición del tablero es válida (dentro de los límites 0-8, 0-4)
        private bool IsBoardPositionValid(int boardX, int boardY)
        {
            return boardX >= 0 && boardX <= 8 && boardY >= 0 && boardY <= 4;
        }

        private void SpawnZombie(float offsetX = 0)
        {
            int lane = GetNextZombieLane();
            // 🚨 CORRECCIÓN CS1729: El constructor de NormalZombie solo toma EntityManager y laneY
            NormalZombie zombie = new NormalZombie(this.EntityManager, lane);
            zombie.Position.X += offsetX;
        }

        public void DoFinalWave()
        {
            float zombiesToSpawn = 5 + (WaveCounter * WaveCounter) * 2f;
            float tileOffset = WaveCounter * WaveCounter * 0.125f;

            Debug.WriteLine($"Final Wave! Spawning {zombiesToSpawn} zombies.");
            for (float i = 0; i < zombiesToSpawn; i++)
            {
                float offsetX = Globals.Lerp(0f, Globals.TILE_SIZE * tileOffset, i / zombiesToSpawn);
                SpawnZombie(offsetX);
            }

            ZombieGenerationTimer = -Globals.HUGE;
            WaveTimeProgress = -Globals.HUGE;
            WaveCounter++;

            IsOnFinalWave = true;
        }

        public void TryEndFinalWave()
        {
            // Usamos Linq.Any para una comprobación más limpia
            if (!this.EntityManager.GetEntities().Any(e => e.TYPE == EntityTypes.Zombie))
            {
                //Si todavia hay una entidad zombie, no va a llegar a este punto
                IsOnFinalWave = false;
                WaveTimeProgress = 0f;
                ZombieGenerationTimer = 0f;
            }
        }

        // --- LÓGICA DE DESPLANTADO ---
        // Método para eliminar una planta en una casilla específica
        public bool TryRemovePlant(int boardX, int boardY)
        {
            if (!IsBoardPositionValid(boardX, boardY)) return false;

            // Busca la primera planta que coincida con las coordenadas del tablero
            Plant plantToRemove = EntityManager.GetEntities()
                .OfType<Plant>() // Filtra solo las entidades de tipo Plant
                .FirstOrDefault(p => p.BoardX == boardX && p.BoardY == boardY);

            if (plantToRemove != null)
            {
                // ¡Eliminar la planta!
                plantToRemove.Remove();
                Debug.WriteLine($"Removed plant at ({boardX}, {boardY})");
                return true;
            }

            return false;
        }
        // --- FIN LÓGICA DE DESPLANTADO ---

        // Método para verificar si una casilla está ocupada
        public bool IsTileOccupied(int boardX, int boardY)
        {
            // Usamos LINQ para una comprobación más limpia
            return EntityManager.GetEntities()
                .OfType<Plant>() // Solo busca entre plantas
                .Any(plant => plant.BoardX == boardX && plant.BoardY == boardY);
        }

        // Método auxiliar para intentar colocar la planta
        private bool TryPlacePlant(PlantSubtypes plantType, int boardX, int boardY)
        {
            var packet = seedPackets.FirstOrDefault(p => p.PlantType == plantType);
            if (packet == null) return false;

            if (IsTileOccupied(boardX, boardY)) return false;
            if (!packet.IsReady()) return false;

            // Se asume que el costo ya fue validado en IsReady()
            SunCount -= packet.Cost;
            packet.StartRecharge();

            switch (plantType)
            {
                case PlantSubtypes.PeaShooter:
                    new Peashooter(this.EntityManager, boardX, boardY);
                    break;
                case PlantSubtypes.SunFlower:
                    new Sunflower(this.EntityManager, boardX, boardY);
                    break;
                case PlantSubtypes.WallNut:
                    new Walnut(this.EntityManager, boardX, boardY);
                    break;
                default:
                    return false;
            }

            IncrementSunCount(0); // Llama para actualizar el contador en consola

            return true;
        }

        private void UpdatePlantingAndShovel()
        {
            var (boardX, boardY) = GetTileAtMouse();
            bool isOverValidTile = IsBoardPositionValid(boardX, boardY);

            if (ShovelEntity.IsSelected)
            {
                // 1. Deseleccionar planta si está seleccionada
                SelectedPlantType = null;

                // 2. Intentar desplantar con clic izquierdo
                if (MouseInput.LeftButtonPressed)
                {
                    if (isOverValidTile)
                    {
                        if (TryRemovePlant(boardX, boardY))
                        {
                            // Después de usar la pala, se deselecciona
                            ShovelEntity.Deselect();
                        }
                    }
                    else
                    {
                        // Si hace clic fuera del tablero, se deselecciona
                        ShovelEntity.Deselect();
                    }
                }
            }
            else if (SelectedPlantType != null)
            {
                // 1. Si tenemos una planta seleccionada, la pala se deselecciona (manejado en ShovelUI.UpdateCallback)

                // 2. Manejar la colocación (clic izquierdo)
                if (MouseInput.LeftButtonPressed)
                {
                    if (isOverValidTile)
                    {
                        if (TryPlacePlant((PlantSubtypes)SelectedPlantType, boardX, boardY))
                        {
                            SelectedPlantType = null;
                        }
                    }
                    else
                    {
                        SelectedPlantType = null;
                    }
                }

                // 3. Deseleccionar si el paquete ya no está listo (por falta de sol o recarga)
                var selectedPacket = seedPackets.FirstOrDefault(p => p.PlantType == SelectedPlantType);
                if (selectedPacket != null && !selectedPacket.IsReady())
                {
                    SelectedPlantType = null;
                }
            }

            // 4. Clic derecho siempre deselecciona planta o pala
            if (MouseInput.RightButtonPressed)
            {
                SelectedPlantType = null;
                ShovelEntity.Deselect();
            }
        }
        // FIN: LÓGICA DE PLANTADO Y DESPLANTADO

        public override void PreUpdateCallback(float dt)
        {
            UISun.Update(dt);
            UpdatePlantingAndShovel(); // Llama a la lógica de plantado y desplantado

            // Generar soles automáticamente cada 10 segundos
            SunGenerationTimer += dt;
            if (SunGenerationTimer >= 10f)
            {
                SunGenerationTimer = 0f;
                // Usamos el constructor de SunPickup que toma PlayingGameState para generar el sol.
                SunPickup sun = new SunPickup(this, this.EntityManager, Random.Shared.Next(0, 9), Random.Shared.Next(0, 5));
            }

            WaveDuration = 70f + WaveCounter * 5f; //Cada ola dura 5 segundos más que la anterior
            WaveTimeProgress += dt;
            ZombieGenerationTimer += dt;

            if (ZombieGenerationTimer >= NextZombieRate)
            {
                ZombieGenerationTimer = 0f;

                if (WaveTimeProgress >= WaveDuration)
                {
                    DoFinalWave();
                }
                else
                {
                    NextZombieRate = GetNextZombieRate();
                    SpawnZombie();
                }
            }

            if (IsOnFinalWave)
            {
                TryEndFinalWave();
            }

            CheckForZombieWinCondition();
        }

        public override void PreDrawCallback(SpriteBatch spriteBatch)
        {
            BGPatio.Draw(spriteBatch, Vector2.Zero);
        }

        public override void PostDrawCallback(SpriteBatch spriteBatch)
        {
            //Dibujar la barra de ola
            UIWaveBar.Play("waveBarBox");
            UIWaveBar.Scale = Vector2.One;
            UIWaveBar.Draw(spriteBatch, new Vector2(172f, 176f));

            UIWaveBar.Play("waveBarFiller");
            var scale = Vector2.One;
            scale.X = Math.Max(0f, Math.Min(1f, WaveTimeProgress / WaveDuration));
            //Debug.WriteLine(scale.X);
            UIWaveBar.Scale = scale;
            UIWaveBar.Draw(spriteBatch, new Vector2(172f, 176f));

            //Dibujar el solesito de la esquina
            Vector2 sunIconPosition = new Vector2(11f, 11f);
            UISun.Draw(spriteBatch, sunIconPosition);

            // --- Dibuja el contador de Soles ---
            if (_pixelFont != null)
            {
                string sunText = SunCount.ToString();
                Vector2 textSize = _pixelFont.MeasureString(sunText);

                // Posición X: Centrada en el icono del Sol (ancho del icono es 22, centro es 11) -> 11f
                // Posición Y: Ligeramente debajo del icono (icono mide 22, el texto debe ir debajo, ej. en y=22 o 24)

                // Icono del sol: 22x22. Centro horizontal: 11f. Borde inferior: 22f.
                // Queremos centrar el texto horizontalmente en x=11f y situarlo en y=24f (justo debajo)
                float targetX = sunIconPosition.X + (11f - textSize.X / 2); // 11f (centro) - textSize.X / 2.
                float targetY = sunIconPosition.Y + 13f; // 11f (inicio) + 13f de offset, para que se vea bien centrado en el hueco del sprite.

                Vector2 textPosition = new Vector2(targetX, targetY);

                // 🚨 AJUSTE FINAL: Usar 1.0f para asegurar que esté ABSOLUTAMENTE en la capa frontal.
                spriteBatch.DrawString(
                    _pixelFont,
                    sunText,
                    textPosition,
                    Color.White, // Aseguramos que el color sea blanco
                    0f, // rotation
                    Vector2.Zero, // origin
                    1f, // scale
                    SpriteEffects.None,
                    1.0f // LayerDepth: ¡ABSOLUTAMENTE al frente!
                );
            }
            // FIN: Dibuja el contador de Soles

            // INICIO: LÓGICA DE CONTADOR DE SOL EN HOVER
            float sunIconWidth = 20f; // Área de hover
            float sunIconHeight = 20f;

            Vector2 mousePos = MouseEntity.Position;

            // Comprueba si el mouse está sobre el área del icono (11, 11) a (31, 31)
            if (mousePos.X >= sunIconPosition.X && mousePos.X < sunIconPosition.X + sunIconWidth &&
                mousePos.Y >= sunIconPosition.Y && mousePos.Y < sunIconPosition.Y + sunIconHeight)
            {
                // La lógica de dibujo de texto ya está fuera del hover
            }
            // FIN: LÓGICA DE CONTADOR DE SOL EN HOVER

            // --- Previsualizador de Planta o Pala (Follow-Mouse) ---
            if (ShovelEntity.IsSelected)
            {
                // Si la pala está seleccionada, dibuja su ícono siguiendo el mouse
                SpriteAnimator shovelCursor = new SpriteAnimator("uiShovel", "default");
                shovelCursor.LayerDepth = 0.9f;

                // Opcional: Centrar el sprite de la pala debajo del cursor.
                Vector2 shovelPosition = MouseEntity.Position;
                // Ajustamos la posición para que el origen (11,11) esté en la posición del mouse
                shovelCursor.Draw(spriteBatch, shovelPosition);
            }
            else if (SelectedPlantType != null)
            {
                Vector2 mouseWorldPos = MouseEntity.Position;
                var packet = seedPackets.FirstOrDefault(p => p.PlantType == SelectedPlantType);

                // Solo dibujar si tenemos sol suficiente
                if (packet != null && SunCount >= packet.Cost)
                {
                    SpriteAnimator drawSprite = null;
                    switch (SelectedPlantType)
                    {
                        case PlantSubtypes.PeaShooter:
                            drawSprite = new SpriteAnimator("peaShooter", "idle");
                            break;
                        case PlantSubtypes.SunFlower:
                            drawSprite = new SpriteAnimator("sunFlower", "idle");
                            break;
                        case PlantSubtypes.WallNut:
                            drawSprite = new SpriteAnimator("walnut", "good");
                            break;
                    }

                    if (drawSprite != null)
                    {
                        drawSprite.LayerDepth = 0.9f;
                        drawSprite.Draw(spriteBatch, mouseWorldPos);
                    }
                }
            }
        }

        private void CheckForZombieWinCondition()
        {
            // Usamos LINQ para encontrar si algún zombi ha llegado al final.
            bool zombieReachedEnd = EntityManager.GetEntities()
                .Any(e => e.TYPE == EntityTypes.Zombie && e.Position.X < (BoardOffset.X - TileSize));

            if (zombieReachedEnd)
            {
                Debug.WriteLine("¡Un zombi ha llegado a tu casa! Fin del juego.");
                // Aquí puedes añadir la lógica para terminar o reiniciar el juego.
                // Por ejemplo, podrías cambiar a un nuevo estado de juego:
                GameManager.SwitchGameState(new PlayingGameState());
            }
        }
    }
}
