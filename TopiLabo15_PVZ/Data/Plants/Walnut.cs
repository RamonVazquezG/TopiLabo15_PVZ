using System;
using System.Diagnostics;
using TopiLabo15_PVZ.Classes.Bases;
using TopiLabo15_PVZ.Classes.Entities;
using TopiLabo15_PVZ.Data.GameStates;
using TopiLabo15_PVZ.Data.Others;

namespace TopiLabo15_PVZ.Data.Plants
{
    public class Walnut : Plant
    {
        private const float WALLNUT_HEALTH = 4000f;
        public Walnut(EntityManager manager, int boardX, int boardY)
            : base(manager, (int?)PlantSubtypes.WallNut, boardX, boardY, WALLNUT_HEALTH)
        {
        }

        public override void InitCallback()
        {
            base.InitCallback();
            this.Sprite = new SpriteAnimator("walnut", "good");
        }

        public override void UpdateCallback(float dt)
        {
            Sprite.SpeedMultiplier = 0.5f + Random.Shared.NextSingle();

            if ( Health <= WALLNUT_HEALTH * 0.1f)
            {
                Sprite.Play("bad");
            } 
            else if (Health <= WALLNUT_HEALTH * 0.5f)
            {
                Sprite.Play("medium");
            }
            else
            {
                Sprite.Play("good");
            }
        }
    }
}