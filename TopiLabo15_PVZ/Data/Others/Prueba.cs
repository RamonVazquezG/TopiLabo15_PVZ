using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopiLabo15_PVZ.Classes.Bases;

namespace TopiLabo15_PVZ.Data.Others
{
    public class Prueba : BoardEntity
    {
        public Prueba(EntityManager manager, Vector2 position, Vector2? velocity, Entity spawner) 
            : base(manager, EntityTypes.None, null, position, velocity, spawner)
        {
        }

        public override void InitCallback()
        {
            base.InitCallback();

            Debug.WriteLine("prupruprupruriurururur");

            Sprite = new SpriteAnimator("peaShooter", "idle");

            Velocity = new Vector2(Globals.TILE_SIZE, 0.0f);
        }

        public override void UpdateCallback(float dt)
        {
            if (this.IsOnGround())
            {
                this.Jump(12.0f, 0.6f);

                if (this.TimeCount > 4.0f)
                {
                    //this.SimpleJump(Random.Shared.Next(10, 320), -Random.Shared.Next(100, 1000));
                    this.Velocity.Y = Random.Shared.Next(-24, 24);
                    this.Velocity.X = Random.Shared.Next(-24, 24);
                } else
                {
                    //this.Jump(12.0f, 0.6f);
                }
            }
        }
    }
}
