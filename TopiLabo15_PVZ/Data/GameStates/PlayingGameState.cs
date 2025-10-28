using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TopiLabo15_PVZ.Data.Others;
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
            patio = new SpriteAnimator("patio", "default");

            //Console.WriteLine("PlayingGameState init");
            // Metodo deprecado de spawn, no se podian usar argumentos en el constructor de clases especificas como los zombies.
            //Prueba pruebaOld = this.EntityManager.Spawn<Prueba>(null, null, null);

            // Ahora, se puede usar el constructor directamente, solo es obligatorio pasar el EntityManager.
            Prueba prueba = new Prueba(this.EntityManager, Vector2.Zero, null, null);
            prueba.SetPositionFromBoard(0,2);

            // Ejemplo
            NormalZombie zombie = new NormalZombie(this.EntityManager, 2); // 2 es la fila del medio.

        }

        public override void PreDrawCallback(SpriteBatch spriteBatch)
        {
            patio.Draw(spriteBatch, Vector2.Zero);
        }
    }
}
