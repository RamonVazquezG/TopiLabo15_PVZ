using Microsoft.Xna.Framework;
using TopiLabo15_PVZ.Classes.Bases;
using TopiLabo15_PVZ.Data; // Asegúrate que el namespace sea correcto

namespace TopiLabo15_PVZ.Classes.Entities
{
    // Una clase abstracta para todas las plantas.
    public abstract class Plant : BoardEntity
    {
        // --- Estadísticas de Planta ---
        public int SunCost { get; protected set; } = 9999;
        public float RechargeTime { get; protected set; } = 99.0f;

        // --- Estado ---
        public int BoardX { get; private set; }
        public int BoardY { get; private set; }

        // Vida estándar de una planta
        private const float DEFAULT_PLANT_HEALTH = 300.0f;
        
        public Plant(EntityManager manager, int uid, int? subtype, int boardX, int boardY)
            : base(manager, uid, EntityTypes.Plant, subtype, Vector2.Zero, Vector2.Zero, null, DEFAULT_PLANT_HEALTH) // Llama al constructor base con la vida
        {
            this.BoardX = boardX;
            this.BoardY = boardY;

            // Usamos el método de BoardEntity para calcular la posición en píxeles
            SetPositionFromBoard(boardX, boardY);

            // Las plantas no se mueven (normalmente)
            this.Velocity = Vector2.Zero;
        }

        public override void UpdateCallback(float dt)
        {
            // Lógica común de todas las plantas (si la hay)
            base.UpdateCallback(dt);
        }
    }
}