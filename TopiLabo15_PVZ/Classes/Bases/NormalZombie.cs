using Microsoft.Xna.Framework;
using TopiLabo15_PVZ.Classes.Entities;
using TopiLabo15_PVZ.Classes.Bases;
using System;

namespace TopiLabo15_PVZ.Classes.Entities.Zombies
{
    public class NormalZombie : Zombie
    {
        // Estadísticas del Zombie Común
        private const float ZOMBIE_HEALTH = 270.0f;
        // SC: TILE_SIZE se obtiene de Globals, que está en .Bases
        private const float ZOMBIE_SPEED_TILE_PER_SEC = 1.0f / 4.7f; // Casillas por segundo
        private const float ZOMBIE_EAT_DAMAGE = 100.0f; // Daño por segundo

        public NormalZombie(EntityManager manager, int uid, int laneY)
            : base(manager, uid, laneY, ZOMBIE_HEALTH)
        {
            // Convertimos la velocidad de "casillas/seg" a "píxeles/seg"
            this.MoveSpeedPixelsPerSecond = ZOMBIE_SPEED_TILE_PER_SEC * Globals.TILE_SIZE;
            this.EatDamagePerSecond = ZOMBIE_EAT_DAMAGE;

            // Actualizamos la velocidad en la clase base
            // SC: Esta línea es la que causaba el error CS0246
            this.Velocity = new Vector2(-this.MoveSpeedPixelsPerSecond, 0);
        }

        public override void UpdateCallback(float dt)
        {
            // Lógica específica del Zombie Común (si la hay)
            base.UpdateCallback(dt);

            // Por ejemplo, aquí iría la lógica de animación:
            // if (IsEating) {
            //     Sprite.Play("eat");
            // } else {
            //     Sprite.Play("walk");
            // }
        }

        public override void OnRemove()
        {
            // Cuando el zombie muere (HP <= 0)
            base.OnRemove();
        }
    }
}