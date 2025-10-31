using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using TopiLabo15_PVZ.Classes.Entities;
using TopiLabo15_PVZ.Data.Others;
using TopiLabo15_PVZ.Data.Plants;
using TopiLabo15_PVZ.Data.Zombies;


namespace TopiLabo15_PVZ.Data.GameStates
{
    public class PlayingGameState : GameState

    {
        private int SunCount = 0;
        private float SunGenerationTimer = 0f;

        private int WaveCounter = 1;

        private int NextZombieLane = Random.Shared.Next(0, 5);
        private float ZombieGenerationTimer = 0f;
        private Vector2 ZombieRate = new Vector2(12f, 6f);
        private float NextZombieRate;
        private bool IsOnFinalWave = false;

        private float WaveTimeProgress = 0f; 
        private float WaveDuration = 60f;

        SpriteAnimator patio; //Nunca inicialicen objetos aquí. Haganlo en OnInit.

        Mouse mouseEntity;
        public PlayingGameState() : base()
        {
            NextZombieRate = ZombieRate.X;
        }

        private int GetNextZombieLane() //Metodo para obtener la siguiente línea de zombies de forma aleatoria pero sin repetir la misma línea consecutivamente
        {
            NextZombieLane = (NextZombieLane + Random.Shared.Next(1, 5)) % 5;
            return NextZombieLane;
        }

        private float GetNextZombieRate()
        {
            float divide = 1f + WaveCounter * 0.1f; //Aumenta la dificultad con cada ola, aumentando la velocidad de aparición de los zombies por un 10% por ola.
            return Globals.Lerp(ZombieRate.X, ZombieRate.Y, WaveTimeProgress / WaveDuration) / divide;
        }

        public override void OnInIt()
        {
            Console.WriteLine("PlayingGameState init");

            patio = new SpriteAnimator("patio", "default");
            //patio.LayerDepth = -0.1f;

            this.mouseEntity = new Mouse(this.EntityManager);

            Peashooter peashooter = new Peashooter(this.EntityManager, 1, 1);
            Sunflower sunflower = new Sunflower(this.EntityManager, 0, 1);
            Walnut walnut = new Walnut(this.EntityManager, 8, 3);
            //NormalZombie zombie = new NormalZombie(this.EntityManager, 1);

            sunflower = new Sunflower(this.EntityManager, 0, 2);
            peashooter = new Peashooter(this.EntityManager, 1, 2);
            peashooter = new Peashooter(this.EntityManager, 1, 4);
            peashooter = new Peashooter(this.EntityManager, 1, 3);
            peashooter = new Peashooter(this.EntityManager, 1, 0);
            //zombie = new NormalZombie(this.EntityManager, 2);
            //zombie = new NormalZombie(this.EntityManager, 3);

            //zombie.SetPositionFromBoard(8, zombie.LaneY);

            SunPickup sun = new SunPickup(this, this.EntityManager, 4, 2);
        }

        public void IncrementSunCount(int amount)
        {
            SunCount += amount;
            Debug.WriteLine($"Sun Count: {SunCount}");
        }

        private void SpawnZombie(float offsetX = 0)
        {
            int lane = GetNextZombieLane();
            NormalZombie zombie = new NormalZombie(this.EntityManager, lane);
            zombie.Position.X += offsetX;
        }

        public void DoFinalWave()
        {
            float zombiesToSpawn = WaveCounter * 5;
            float tileOffset = 1.5f + WaveCounter * 0.5f;

            Debug.WriteLine($"Final Wave! Spawning {zombiesToSpawn} zombies.");
            for (float i = 0; i < zombiesToSpawn; i++)
            {
                float offsetX = Globals.Lerp(0f, Globals.TILE_SIZE * tileOffset, i/zombiesToSpawn);
                SpawnZombie(offsetX);
            }

            ZombieGenerationTimer = -Globals.HUGE;
            WaveTimeProgress = -Globals.HUGE;
            WaveCounter++;

            IsOnFinalWave = true;
        }

        public void TryEndFinalWave()
        {
            foreach (Entity entity in this.EntityManager.GetEntities())
            {
                if (entity.TYPE == EntityTypes.Zombie)
                    return;
            }

            //Si todavia hay una entidad zombie, no va a llegar a este punto
            IsOnFinalWave = false;
            WaveTimeProgress = 0f;
            ZombieGenerationTimer = 0f;
        }

        public override void PreUpdateCallback(float dt)
        {
            // Generar soles automáticamente cada 10 segundos
            SunGenerationTimer += dt;
            if (SunGenerationTimer >= 10f)
            {
                SunGenerationTimer = 0f;
                SunPickup sun = new SunPickup(this, this.EntityManager, Random.Shared.Next(0, 9), Random.Shared.Next(0, 5));
            }
             
            WaveDuration = 60f + WaveCounter * 15f; //Cada ola dura 15 segundos más que la anterior
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
        }

        public override void PreDrawCallback(SpriteBatch spriteBatch)
        {
            patio.Draw(spriteBatch, Vector2.Zero);
        }
    }
}
