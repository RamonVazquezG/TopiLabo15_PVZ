using Microsoft.Xna.Framework;
using System.Diagnostics;
using TopiLabo15_PVZ.Classes.Bases;
using TopiLabo15_PVZ.Data; // Asegúrate que el namespace sea correcto

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

        public Zombie(EntityManager manager, int? subtype, int laneY, float maxHealth)
            : base(manager, EntityTypes.Zombie, subtype, Vector2.Zero, Vector2.Zero, null, maxHealth)
        {
            this.LaneY = laneY;

            // Posición inicial (fuera de la pantalla, a la derecha)
            this.SetPositionFromBoard(10, laneY); // Coloca el zombi en la columna 9 (fuera de la pantalla, pero cercas de la orilla).

            // Los zombies se mueven hacia la izquierda
            this.Velocity = new Vector2(-MoveSpeedPixelsPerSecond, 0);
        }

        public override void InitCallback()
        {
            this.Hitbox = new Hitbox(this, this.LaneY, new Vector2(20f, 30f)); //Si este no es el tamaño que se quiere, solo haz override sin llamar base.InitCallback e inicializa el Hitbox a tu manera.
        }

        public override void UpdateCallback(float dt)
        {
            // Lógica de comer
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

        public override void PostPhysicsCallback(float dt)
        {
            this.LaneY = (int)this.GetPositionToBoard().Y;
            base.PostPhysicsCallback(dt);
        }
        public override void HitboxCallback(Entity other)
        {
            Debug.WriteLine("Yo zombi comer");
            if (other.TYPE == EntityTypes.Plant)
            {
                Plant plant = (Plant)other;
                _currentTarget = plant;
            }
        }
    }
}