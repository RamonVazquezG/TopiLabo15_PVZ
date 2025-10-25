using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics; // Para Math.Max

namespace TopiLabo15_PVZ.Classes.Bases
{
    public class BoardEntity : Entity
    {
        const float EPSILON = Globals.EPSILON;
        const float TILE_SIZE = Globals.TILE_SIZE;
        const float HALF_TILE_SIZE = Globals.HALF_TILE_SIZE;
        public readonly Vector2 BOARD_OFFSET = new Vector2(TILE_SIZE*1.0f, TILE_SIZE*2.0f); // Offset en pixeles, desde la esquina superior izquierda de la pantalla hasta la esquina superior izquierda del tablero.

        private float Z = 0.0f; // Altura sobre el tablero (en píxeles). Positivo es hacia arriba y negativo hacia abajo. No debe ser menor a 0.0f.
        public float ZVelocity = 0.0f; // Velocidad para Z (en píxeles por segundo). Positivo es hacia arriba y negativo hacia abajo.
        public float Gravity = 0.0f; // Acceleración para Z (en píxeles por segundo al cuadrado). Positivo es hacia arriba y negativo hacia abajo.

        public BoardEntity(EntityManager manager, int uid, Vector2 position, Vector2? velocity, Entity spawner) 
            : base(manager, uid, position, velocity, spawner)
        {
        }

        public void SetZ(float z) { Z = Math.Abs(z); }
        public float GetZ() { return Math.Abs(Z); }

        public bool IsOnGround() { return this.GetZ() == 0.0f; }

        public void SetPositionFromBoard(int boardX, int boardY)
        {
            this.Position = new Vector2(
                HALF_TILE_SIZE + BOARD_OFFSET.X + boardX * TILE_SIZE, //El HALF_TILE_SIZE es para centrar la entidad en el tile.
                HALF_TILE_SIZE + BOARD_OFFSET.Y + boardY * TILE_SIZE
            );
        }
        public Vector2 GetPositionToBoard()
        {
            return new Vector2(
                MathF.Floor(this.Position.X - BOARD_OFFSET.X) / TILE_SIZE,
                MathF.Floor(this.Position.Y - BOARD_OFFSET.Y) / TILE_SIZE
            );
        }

        /// <summary>
        /// ¡NUEVA FUNCIÓN!
        /// Inicia un salto que alcanzará una altura máxima específica
        /// y durará un tiempo total específico en el aire.
        /// 
        /// Esto calculará y establecerá la 'ZVelocity' (velocidad inicial)
        /// y la 'Gravity' (gravedad) necesarias para este salto.
        /// </summary>
        /// <param name="peakHeight">La altura máxima a alcanzar (en píxeles).</param>
        /// <param name="jumpDuration">La duración total del salto (en segundos).</param>
        public void Jump(float peakHeight, float jumpDuration)
        {
            // Moverlo un poqutio arriba, para que cuando se use con IsOnGround no se quede atascado en el suelo, o algo asi :p
            if (this.IsOnGround()) { this.SetZ(EPSILON); }

            // Asegura que los valores SIEMPRE sean positivos!!
            peakHeight = Math.Max(0.0f, peakHeight - EPSILON); // Le restamos EPSILON porque lo subimos un poquito arribita.
            jumpDuration = Math.Abs(jumpDuration);

            // 1. Calcular el tiempo para alcanzar el pico (la mitad del total)
            float timeToPeak = jumpDuration / 2.0f;

            // 2. Calcular la gravedad necesaria
            // Fórmula: g = -2 * altura / (tiempo_pico^2)c            
            // (La gravedad debe ser negativa, y peakHeight es positiva)
            this.Gravity = (-2.0f * peakHeight) / (timeToPeak * timeToPeak);

            // 3. Calcular la velocidad inicial necesaria
            // Fórmula: v0 = 2 * altura / tiempo_pico
            // (Velocidad Z debe ser positiva para ir hacia arriba)
            this.ZVelocity = (2.0f * peakHeight) / timeToPeak;

            // Alternativa para v0: v0 = 4 * altura / tiempo_total
            // this.ZVelocity = (4.0f * peakHeight) / airDuration;
        }

        public void SimpleJump(float initialVelocityZ, float gravity)
        {
            // Moverlo un poqutio arriba, para que cuando se use con IsOnGround no se quede atascado en el suelo, o algo asi :p
            if (this.IsOnGround()) { this.SetZ(EPSILON); }
            this.ZVelocity = initialVelocityZ; // Asegura que sea positivo.
            this.Gravity = gravity;
        }
        public override void ApplyPhysics(float dt) // Maneja la posición Z (altura sobre el tablero, se podria utilizar para zombies que hagan saltos).
        {
            base.ApplyPhysics(dt); // Aplica la física normal (posición X,Y)

            ZVelocity += Gravity * dt;

            this.SetZ( Math.Max(0.0f, this.GetZ() + ZVelocity * dt) ); // Asegura que Z no sea negativa si ZVelocity es negativa.
        }

        
        public override void DrawSpriteCallback(SpriteBatch spriteBatch)
        {
            Vector2 PositionWithZ = new Vector2(Position.X, Position.Y - GetZ());
            Sprite.Draw(spriteBatch, PositionWithZ);
        }
    }
}
