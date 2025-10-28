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

        public Peashooter(EntityManager manager, int uid, int boardX, int boardY)
            : base(manager, uid, (int?)PlantSubtypes.PeaShooter, boardX, boardY)
        {
            this.SunCost = 100;
            this.RechargeTime = 7.5f;
        }

        public override void UpdateCallback(float dt)
        {
            base.UpdateCallback(dt);

            _shootTimer += dt;
            if (_shootTimer >= SHOOT_COOLDOWN)
            {
                // Solo dispara si hay un zombie en el carril
                if (IsZombieInLane())
                {
                    _shootTimer = 0.0f;
                    ShootPea();
                }
                else
                {
                    _shootTimer = SHOOT_COOLDOWN; // No dejes que el timer se acumule
                }
            }
        }

        private bool IsZombieInLane()
        {
            // Lógica de detección de zombies.
            // Deberías preguntar al EntityManager si hay algún Zombie
            // en this.BoardY con una Position.X mayor a la tuya.
            // return Manager.CheckForZombiesInLane(this.BoardY, this.Position.X);

            return true; // Placeholder: Asume que siempre hay un zombie
        }

        private void ShootPea()
        {
            // Aquí es donde crearías la entidad "Pea" (Guisante)
            // El guisante debe heredar de BoardEntity y moverse hacia la derecha.
            // Vector2 spawnPos = this.Position + new Vector2(20, -15); // Ajusta la pos
            // Manager.CreateEntity<Pea>(spawnPos, PEA_DAMAGE, this);

            // Debug.WriteLine($"Lanzaguisantes (UID: {GetUID()}) disparó un guisante.");
        }
    }
}