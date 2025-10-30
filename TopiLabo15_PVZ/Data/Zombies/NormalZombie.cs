using Microsoft.Xna.Framework;
using TopiLabo15_PVZ.Classes.Entities;
using TopiLabo15_PVZ.Classes.Bases;
using System;

namespace TopiLabo15_PVZ.Data.Zombies
{
    public class NormalZombie : Zombie
    {
        // Estadísticas del Zombie Común
        private const float ZOMBIE_HEALTH = 200.0f;
        private const float TILE_SIZE = Globals.TILE_SIZE;
        // SC: TILE_SIZE se obtiene de Globals, que está en .Bases
        private const float ZOMBIE_SPEED_TILE_PER_SEC = 1.0f / 4.7f; // Casillas por segundo. JC: Que especifico, pero si es de echo la velocidad del juego oriignal xdddd.
        private const float ZOMBIE_EAT_DAMAGE = 100.0f; // Daño por segundo

        public NormalZombie(EntityManager manager, int laneY)
            : base(manager, (int?)ZombieSubtypes.Normal, laneY, ZOMBIE_HEALTH)
        {
            // Convertimos la velocidad de "casillas/seg" a "píxeles/seg"
            this.MoveSpeedPixelsPerSecond = ZOMBIE_SPEED_TILE_PER_SEC * TILE_SIZE;
            this.EatDamagePerSecond = ZOMBIE_EAT_DAMAGE;

            // Actualizamos la velocidad en la clase base
            // SC: Esta línea es la que causaba el error CS0246
            this.Velocity = new Vector2(-this.MoveSpeedPixelsPerSecond, 0);
        }

        public override void InitCallback()
        {
            base.InitCallback();
            this.Sprite = new SpriteAnimator("zombieNormal", "walk");
            this.Sprite.LayerDepth = 0.5f; // Para que se dibuje encima de las plantas.
        }

        public override void UpdateCallback(float dt)
        {
            // Lógica específica del Zombie Común (si la hay)
            base.UpdateCallback(dt);

             if (IsEating) {
                 Sprite.Play("eat");
             } else {
                 Sprite.Play("walk");
             }
        }
    }
}