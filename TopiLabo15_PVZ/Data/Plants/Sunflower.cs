using System;
using System.Diagnostics;
using TopiLabo15_PVZ.Classes.Bases;
using TopiLabo15_PVZ.Classes.Entities;
using TopiLabo15_PVZ.Data.GameStates;
using TopiLabo15_PVZ.Data.Others;

namespace TopiLabo15_PVZ.Data.Plants
{
    public class Sunflower : Plant
    {
        // Estadísticas del Girasol
        private const float SUN_PRODUCTION_TIME = 22.0f; // Segundos

        private float _sunTimer;

        public Sunflower(EntityManager manager, int boardX, int boardY)
            : base(manager, (int?)PlantSubtypes.SunFlower, boardX, boardY)
        {
            _sunTimer = SUN_PRODUCTION_TIME/2 - (Random.Shared.NextSingle() * 3f); // Generar con mas anticipacion el primer sol y con algo de aleatoriedad.
        }

        public override void InitCallback()
        {
            base.InitCallback();
            this.Sprite = new SpriteAnimator("sunFlower", "idle");
        }

        public override void UpdateCallback(float dt)
        {
            _sunTimer += dt;
            if (_sunTimer >= SUN_PRODUCTION_TIME)
            {
                _sunTimer = Random.Shared.NextSingle() * -2f; ; // Delay random de hasta un segundo, para que sea un poco mas impredecible la generacion de soles, o algo asi :p
                Sprite.Play("sunGive", true);
                //Debug.WriteLine("sun give");
            }

            if ( Sprite.IsPlaying("sunGive") )
            {
                if (Sprite.FrameIndex == 1 & Sprite.JustChangedFrame)
                {
                    SpawnSun();
                }

                if (Sprite.JustFinishedAnimation)
                {
                    Sprite.Play("idle", true);
                }
            }
        }

        private void SpawnSun()
        {
            SunPickup sun = new SunPickup((PlayingGameState)this.Manager.GameState, this.Manager, BoardX, BoardY, this);
        }
    }
}