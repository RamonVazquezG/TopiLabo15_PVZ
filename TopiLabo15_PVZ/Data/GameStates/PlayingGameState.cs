using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using TopiLabo15_PVZ.Data.Others;
using TopiLabo15_PVZ.Data.Plants;
using TopiLabo15_PVZ.Data.Zombies;


namespace TopiLabo15_PVZ.Data.GameStates
{
    public class PlayingGameState : GameState

    {
        private int SunCount = 0;
        private float SunGenerationTimer = 0f;

        SpriteAnimator patio; //Nunca inicialicen objetos aquí. Haganlo en OnInit.

        Mouse mouseEntity;
        public PlayingGameState() : base()
        {
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
            NormalZombie zombie = new NormalZombie(this.EntityManager, 1);

            sunflower = new Sunflower(this.EntityManager, 0, 2);
            peashooter = new Peashooter(this.EntityManager, 1, 2);
            zombie = new NormalZombie(this.EntityManager, 2);
            zombie = new NormalZombie(this.EntityManager, 3);

            //zombie.SetPositionFromBoard(8, zombie.LaneY);

            SunPickup sun = new SunPickup(this, this.EntityManager, 4, 2);
        }

        public void IncrementSunCount(int amount)
        {
            SunCount += amount;
            Debug.WriteLine($"Sun Count: {SunCount}");
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
        }

        public override void PreDrawCallback(SpriteBatch spriteBatch)
        {
            patio.Draw(spriteBatch, Vector2.Zero);
        }
    }
}
