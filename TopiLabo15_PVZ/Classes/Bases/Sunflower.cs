using TopiLabo15_PVZ.Classes.Bases;

namespace TopiLabo15_PVZ.Classes.Entities.Plants
{
    public class Sunflower : Plant
    {
        // Estadísticas del Girasol
        private const float SUN_PRODUCTION_TIME = 24.0f; // Segundos
        private const int SUN_VALUE = 25;

        private float _sunTimer = 0.0f;

        public Sunflower(EntityManager manager, int uid, int boardX, int boardY)
            : base(manager, uid, boardX, boardY)
        {
            // Establece las estadísticas específicas del Girasol
            this.SunCost = 50;
            this.RechargeTime = 7.5f;
            // La vida (300) ya se estableció en la clase base Plant

            _sunTimer = 10.0f; // Un delay inicial antes del primer sol
        }

        public override void UpdateCallback(float dt)
        {
            base.UpdateCallback(dt);

            _sunTimer += dt;
            if (_sunTimer >= SUN_PRODUCTION_TIME)
            {
                _sunTimer = 0.0f; // Reinicia el contador
                SpawnSun();
            }
        }

        private void SpawnSun()
        {
            // Aquí es donde crearías una entidad "Sun"
            // Por ejemplo:
            // Manager.CreateEntity<Sun>(this.Position, this);

            // Debug.WriteLine($"Girasol (UID: {GetUID()}) produjo {SUN_VALUE} de sol.");
        }
    }
}