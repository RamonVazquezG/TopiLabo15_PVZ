using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TopiLabo15_PVZ.Classes.Bases;
using TopiLabo15_PVZ.Classes.Entities;
using TopiLabo15_PVZ.Data.GameStates;

namespace TopiLabo15_PVZ.Data.UI
{
    /// <summary>
    /// Entidad UI que representa el ícono de la pala para desplantar.
    /// Su principal función es manejar la selección de la pala.
    /// </summary>
    public class ShovelUI : Entity
    {
        private PlayingGameState _gameState;
        private Vector2 _screenPosition;

        // Propiedad que guarda el estado de si la pala está seleccionada
        public bool IsSelected { get; private set; } = false;

        // Dimensiones del sprite (basado en uiShovel)
        private const float SHOVEL_WIDTH = 22f;
        private const float SHOVEL_HEIGHT = 22f;

        // Offset de la pala respecto a su centro (ajustado para que el centro del slot sea 11x11)
        private static readonly Vector2 SHOVEL_ORIGIN = new Vector2(11f, 11f);

        public ShovelUI(EntityManager manager, PlayingGameState gameState, Vector2 screenPosition)
            : base(manager, EntityTypes.Shovel, null, screenPosition, Vector2.Zero, null) // Usamos EntityTypes.Shovel
        {
            this._gameState = gameState;
            this._screenPosition = screenPosition;

            this.Sprite = new SpriteAnimator("uiShovel", "default");

            // Hitbox para la interacción del mouse, centrada en el slot.
            this.Hitbox = new Hitbox(this, null, new Vector2(SHOVEL_WIDTH, SHOVEL_HEIGHT), -SHOVEL_ORIGIN);

            // Re-aplicar posición para asegurar que el Hitbox esté en la coordenada correcta
            this.Position = screenPosition;
        }

        public override void UpdateCallback(float dt)
        {
            // Lógica de click
            if (this.Hitbox != null && this.Hitbox.Intersects(_gameState.MouseEntity.Hitbox))
            {
                if (MouseInput.LeftButtonPressed)
                {
                    // Si la pala está seleccionada, deseleccionarla
                    if (IsSelected)
                    {
                        Deselect();
                    }
                    // Si la pala NO está seleccionada, seleccionarla y deseleccionar la planta
                    else
                    {
                        Select();
                    }
                }
            }

            // Lógica para deselección externa (por ejemplo, si el juego entra en otro estado)
            if (_gameState.SelectedPlantType != null)
            {
                // Si seleccionamos una planta, la pala se deselecciona.
                Deselect();
            }

            // Un clic derecho siempre deselecciona la pala (solo si no estamos sobre un paquete)
            if (MouseInput.RightButtonPressed && _gameState.SelectedPlantType == null)
            {
                Deselect();
            }
        }

        public void Select()
        {
            IsSelected = true;
            // Deseleccionar cualquier planta que esté seleccionada
            _gameState.SelectedPlantType = null;
        }

        public void Deselect()
        {
            IsSelected = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.Sprite == null) return;

            Sprite.LayerDepth = 0.99f; // Dibujar siempre al frente

            // Dibujar el sprite de la pala
            this.Sprite.Draw(spriteBatch, this.Position);

            // Resaltar la pala si está seleccionada
            if (IsSelected)
            {
                // JC: Puedes usar un sprite de "brillo" o simplemente dibujar un cuadrado semitransparente.
                // Como no tenemos un sprite de brillo, usaremos un truco visual:
                // Cambiar el color para indicar que está seleccionada.
                // Opcional: Esto requiere un SpriteBatch.Begin() sin Color.White o un shader.
                // Por ahora, solo se dibujará el sprite normal.
            }
        }

        public override void DrawSpriteCallback(SpriteBatch spriteBatch) { /* Se omite para usar el override Draw completo */ }
    }
}
