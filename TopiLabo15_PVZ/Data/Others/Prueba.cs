using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopiLabo15_PVZ.Data.Others
{
    public class Prueba : Entity
    {
        public Prueba(EntityManager manager, int uid, Vector2 position, Vector2? velocity, Entity spawner) : base(manager, uid, position, velocity, spawner)
        {
        }

        public override void InitCallback()
        {
            base.InitCallback();

            Debug.WriteLine("prupruprupruriurururur");

            Sprite = new SpriteAnimator("peaShooter", "idle");
        }   
    }
}
