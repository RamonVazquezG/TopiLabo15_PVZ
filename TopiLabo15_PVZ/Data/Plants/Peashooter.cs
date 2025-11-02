using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using TopiLabo15_PVZ.Classes.Bases;
using TopiLabo15_PVZ.Classes.Entities;

namespace TopiLabo15_PVZ.Data.Plants
{
    public class Peashooter : Plant
    {
        // Estadísticas del Lanzaguisantes
        private const float SHOOT_COOLDOWN = 1.4f; // Segundos entre disparos
        public const float PEA_DAMAGE = 20.0f;

        private float _shootTimer = 0.0f;

        public Peashooter(EntityManager manager, int boardX, int boardY)
            : base(manager, (int?)PlantSubtypes.PeaShooter, boardX, boardY)
        {
            this.SunCost = 100;
            this.RechargeTime = 7.5f;
        }

        public override void InitCallback()
        {
            base.InitCallback();
            Sprite = new SpriteAnimator("peaShooter", "idle");

            Hitbox sightHtibox = new Hitbox(this, this.BoardY, new Vector2(Globals.TILE_SIZE * 9f, 1f), new Vector2(), "sight");
            AddHitbox(sightHtibox);
        }

        public override void UpdateCallback(float dt)
        {
            base.UpdateCallback(dt);

            _shootTimer += dt;
            if (_shootTimer >= SHOOT_COOLDOWN)
            {
                // Solo dispara si hay un zombie en el carril
                if (this.HasZombieSight)
                {
                    _shootTimer = Random.Shared.NextSingle() * -0.25f; //Un pequeño retraso aleatorio para que no todos disparen exactamente al mismo tiempo.
                    Sprite.Play("shoot", true); // JC: Recuerden que el true hara que se reinicie la animacion desde el frame 0.
                }
                else
                {
                    _shootTimer = SHOOT_COOLDOWN; 
                    Sprite.Play("idle");
                }
            }

            if (Sprite.IsPlaying("shoot"))
            {
                if (Sprite.FrameIndex == 3 & Sprite.JustChangedFrame) { ShootPea(); }
            }
        }

        private void ShootPea()
        {
            //Debug.WriteLine("pea shoot");

            ProjectilePea pea = new ProjectilePea(this.Manager, PEA_DAMAGE, BoardX+1, BoardY);
            pea.SetZ(8f);
        }

        public override void HitboxCallback(Entity other, Hitbox otherHitbox, string tag, string otherTag)
        {
            if (tag == "sight" && other.TYPE == EntityTypes.Zombie)
            {
                // Aquí podrías manejar la lógica cuando un zombie entra en el campo de visión
                // Por ejemplo, podrías establecer una bandera o iniciar una animación
                this.HasZombieSight = true;
            }
        }
    }
}