using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TopiLabo15_PVZ.Data.Others;


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
            patio = new SpriteAnimator("patio", "default");

            Console.WriteLine("PlayingGameState init");
            // Aquí puedes inicializar entidades específicas para este estado
            Prueba prueba = this.EntityManager.Spawn<Prueba>(null, null, null);
            prueba.SetPositionFromBoard(0,2);
        }

        public override void PreDrawCallback(SpriteBatch spriteBatch)
        {
            patio.Draw(spriteBatch, Vector2.Zero);
        }
    }
}
