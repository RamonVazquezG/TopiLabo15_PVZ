using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TopiLabo15_PVZ.Data.Others;
using TopiLabo15_PVZ.Data.Plants;
using TopiLabo15_PVZ.Data.Zombies;


namespace TopiLabo15_PVZ.Data.GameStates
{
    public class PlayingGameState : GameState

    {
        SpriteAnimator patio; //Nunca inicialicen objetos aquí. Haganlo en OnInit.
        public PlayingGameState() : base()
        {
        }

        public override void OnInIt()
        {
            Console.WriteLine("PlayingGameState init");

            patio = new SpriteAnimator("patio", "default");

            Peashooter peashooter = new Peashooter(this.EntityManager, 0, 1);
            NormalZombie zombie = new NormalZombie(this.EntityManager, 1); 

            peashooter = new Peashooter(this.EntityManager, 0, 2);
            zombie = new NormalZombie(this.EntityManager, 2);
            //zombie.SetPositionFromBoard(8, zombie.LaneY);

        }

        public override void PreDrawCallback(SpriteBatch spriteBatch)
        {
            patio.Draw(spriteBatch, Vector2.Zero);
        }
    }
}
