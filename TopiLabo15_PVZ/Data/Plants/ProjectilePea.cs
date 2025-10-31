using Microsoft.Xna.Framework;
using System.Diagnostics;
using TopiLabo15_PVZ.Classes.Bases;
using TopiLabo15_PVZ.Classes.Entities;

namespace TopiLabo15_PVZ.Data.Plants
{
    public class ProjectilePea : BoardEntity
    {
        public float Damage { get; private set; }
        private bool TouchedZombie = false;
        public ProjectilePea(EntityManager manager, float damage, int boardX, int boardY) : base(manager, EntityTypes.PlantHazard, null, Vector2.Zero, Vector2.Zero, null, 1.0f)
        {
            this.Damage = damage;
            this.SetPositionFromBoard(boardX, boardY);
            this.Velocity = new Vector2(100f, 0); // Velocidad del guisante hacia la derecha
        }


        public override void InitCallback()
        {
            base.InitCallback();
            Sprite = new SpriteAnimator("peaShooter", "projectile");

            Hitbox hitbox = new Hitbox(this, (int?)this.GetPositionToBoard().Y, new Vector2(1f,11f));
            AddHitbox(hitbox);
        }

        public override void UpdateCallback(float dt)
        {
            if (TouchedZombie || this.GetPositionToBoard().X > 10f)
            {
                this.Remove(); // Elimina el guisante después de tocar uno o mas zombis en un frame.
            }
        }

        public override void HitboxCallback(Entity other, Hitbox otherHitbox, string tag, string otherTag)
        {
            if (other.TYPE == EntityTypes.Zombie)
            {
                TouchedZombie = true;
                Zombie zombie = (Zombie)other;
                zombie.TakeDamage(Damage);
            }
        }
    }
}