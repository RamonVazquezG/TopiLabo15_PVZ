using Microsoft.Xna.Framework;
using System;
using TopiLabo15_PVZ.Data.Others;


namespace TopiLabo15_PVZ.Data.GameStates
{
    public class PlayingGameState : GameState

    {
        public PlayingGameState() : base()
        {
        }

        public override void OnInIt()
        {
            base.OnInIt();
            Console.WriteLine("PlayingGameState init");
            // Aquí puedes inicializar entidades específicas para este estado
            this.EntityManager.Spawn<Prueba>(new Vector2(20.0f, 20.0f), null, null);
        }
    }
}
