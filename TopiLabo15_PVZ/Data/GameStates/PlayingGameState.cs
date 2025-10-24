using Microsoft.Xna.Framework;


namespace TopiLabo15_PVZ.Data.GameStates
{
    public class PlayingGameState : GameState

    {
        public PlayingGameState() { 
        
            Entity prueba = this.EntityManager.Spawn<Entity>(new Vector2(100, 100));
            prueba.Sprite = new SpriteAnimator("peaShooter", "idle");
        }


    }
}
