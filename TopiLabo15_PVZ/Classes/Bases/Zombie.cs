using Microsoft.Xna.Framework;
using TopiLabo15_PVZ.Classes.Bases; // Asegúrate que el namespace sea correcto

namespace TopiLabo15_PVZ.Classes.Entities
{
    public abstract class Zombie : BoardEntity
    {
        // --- Estadísticas de Zombi ---
        public float MoveSpeedPixelsPerSecond { get; protected set; } = 10.0f;
        public float EatDamagePerSecond { get; protected set; } = 100.0f;

        // --- Estado ---
        public int LaneY { get; private set; }
        protected bool IsEating = false;
        private float _eatTimer = 0.0f;
        private Plant _currentTarget = null; // La planta que está comiendo

        public Zombie(EntityManager manager, int uid, int laneY, float maxHealth)
            : base(manager, uid, Vector2.Zero, Vector2.Zero, null, maxHealth)
        {
            this.LaneY = laneY;

            // Posición inicial (fuera de la pantalla, a la derecha)
            // Asumimos un ancho de pantalla, ej. 1280. Deberías obtener esto de Globals.
            float startX = 1300; // O Globals.SCREEN_WIDTH + TILE_SIZE
            float startY = HALF_TILE_SIZE + BOARD_OFFSET.Y + laneY * TILE_SIZE;

            this.Position = new Vector2(startX, startY);

            // Los zombies se mueven hacia la izquierda
            this.Velocity = new Vector2(-MoveSpeedPixelsPerSecond, 0);
        }

        public override void UpdateCallback(float dt)
        {
            base.UpdateCallback(dt);

            // 1. Buscar una planta para comer
            // ESTA ES LA PARTE MÁS COMPLEJA. Necesitarás un sistema de colisiones.
            // Por ahora, simulamos una búsqueda simple:

            // _currentTarget = Manager.Find<Plant>(p => 
            //     p.LaneY == this.LaneY && 
            //     MathF.Abs(p.Position.X - this.Position.X) < 20 // Si está muy cerca
            // );

            // Simulación simple (esto debes reemplazarlo con lógica de colisión real):
            CheckForPlants();

            // 2. Lógica de comer
            if (_currentTarget != null)
            {
                // Si tiene un objetivo, deja de moverse
                if (!IsEating)
                {
                    IsEating = true;
                    this.Velocity = Vector2.Zero;
                    _eatTimer = 0.0f; // Reinicia el timer de mordisco
                }

                // Aplica daño según la cadencia
                _eatTimer += dt;
                if (_eatTimer >= 1.0f) // 1 segundo por mordisco (basado en EatDamagePerSecond)
                {
                    _eatTimer -= 1.0f;
                    _currentTarget.TakeDamage(this.EatDamagePerSecond, this);

                    // Si la planta murió, deja de comerla
                    if (_currentTarget.IsRemoved)
                    {
                        _currentTarget = null;
                    }
                }
            }
            else
            {
                // Si no hay objetivo, o el objetivo murió, sigue caminando
                if (IsEating)
                {
                    IsEating = false;
                    this.Velocity = new Vector2(-MoveSpeedPixelsPerSecond, 0);
                }
            }
        }

        // Esto es un placeholder. Necesitarás una forma de detectar colisiones.
        // Podrías usar los Hitbox de tus entidades o pedirle al EntityManager
        // que te dé las plantas en tu carril.
        private void CheckForPlants()
        {
            if (_currentTarget != null && _currentTarget.IsRemoved)
            {
                _currentTarget = null; // El objetivo murió
            }

            if (_currentTarget == null)
            {
                // Lógica para encontrar una nueva planta
                // _currentTarget = ...
            }
        }
    }
}