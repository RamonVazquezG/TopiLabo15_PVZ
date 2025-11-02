// staarmaan/topilabo15_pvz/TopiLabo15_PVZ-Juan/TopiLabo15_PVZ/Data/Others/SeedPacketUI.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TopiLabo15_PVZ.Classes.Bases;
using TopiLabo15_PVZ.Classes.Entities;
using TopiLabo15_PVZ.Data.GameStates;
using TopiLabo15_PVZ.Data.Plants;

namespace TopiLabo15_PVZ.Data.UI
{
    public class SeedPacketUI : Entity
    {
        public PlantSubtypes PlantType { get; private set; }
        public int Cost { get; private set; }
        public float RechargeDuration { get; private set; }
        public float RechargeTimer { get; private set; }

        private PlayingGameState _gameState;
        private Vector2 _screenPosition;

        // ¡NUEVO! Referencia a la fuente
        private SpriteFont _pixelFont;

        // Función estática para obtener la data de la planta (valores fijos).
        public static (int cost, float recharge) GetPlantData(PlantSubtypes plantType)
        {
            switch (plantType)
            {
                case PlantSubtypes.PeaShooter:
                    return (100, 7.5f);
                case PlantSubtypes.SunFlower:
                    return (50, 7.5f);
                case PlantSubtypes.WallNut:
                    return (50, 30.0f);
                default:
                    return (9999, 99.0f); //JC: wtf
            }
        }

        public SeedPacketUI(EntityManager manager, PlayingGameState gameState, PlantSubtypes plantType, Vector2 screenPosition, SpriteFont pixelFont)
            : base(manager, EntityTypes.UI, (int?)plantType, screenPosition, Vector2.Zero, null)
        {
            this._gameState = gameState;
            this.PlantType = plantType;
            this._screenPosition = screenPosition;
            this._pixelFont = pixelFont;

            var data = GetPlantData(plantType);
            this.Cost = data.cost;
            this.RechargeDuration = data.recharge;
            this.RechargeTimer = 0f; // Inicialmente listo

            this.Sprite = new SpriteAnimator("uiSeeds", (int?)plantType + ""); //La animacion se llama por el PlantSubtypes como string.

            // Hitbox para la interacción del mouse (18x18)
            this.Hitbox = new Hitbox(this, null, new Vector2(18f, 18f));

            // Re-aplicar posición para asegurar que el Hitbox esté en la coordenada correcta
            this.Position = screenPosition;
        }

        public bool IsReady()
        {
            // Listo si el temporizador es 0 y tenemos suficiente sol.
            return RechargeTimer <= 0f && _gameState.SunCount >= this.Cost;
        }

        public void StartRecharge()
        {
            RechargeTimer = RechargeDuration;
        }

        public override void UpdateCallback(float dt)
        {
            if (RechargeTimer > 0f)
            {
                RechargeTimer -= dt;
            }

            // La posición de esta entidad SIEMPRE es la posición estática de la UI
            this.Position = _screenPosition;

            // Lógica de click
            if (this.Hitbox != null && this.Hitbox.Intersects(_gameState.MouseEntity.Hitbox))
            {
                if (MouseInput.LeftButtonPressed)
                {
                    if (IsReady())
                    {
                        if (_gameState.SelectedPlantType == this.PlantType)
                        {
                            _gameState.SelectedPlantType = null;
                        }
                        else
                        {
                            _gameState.SelectedPlantType = this.PlantType;
                        }
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.Sprite == null) return;

            Sprite.LayerDepth = 0.99f; // Dibujar siempre al frente

            int targetFrame = IsReady() ? 1 : 0; // Frame: 0=OK, 1=Recarga (Gris)
            this.Sprite.SetFrame(targetFrame);

            this.Sprite.Draw(spriteBatch, this.Position);

            // --- ¡NUEVO! DIBUJADO DE TEXTO (Costo) ---
            if (_pixelFont != null)
            {
                string costText = Cost.ToString();
                Vector2 textSize = _pixelFont.MeasureString(costText);

                // Posición para el costo: Centrado horizontalmente en la parte inferior del paquete.
                // Offset horizontal: 9f (mitad del ancho del paquete) - textSize.X / 2f
                // Offset vertical: 12f (borde inferior)
                Vector2 textPosition = this.Position + new Vector2(9f - textSize.X / 2f, 12f);

                // Color: Blanco o Amarillo si está listo y con suficiente sol
                Color textColor = IsReady() ? Color.White : Color.Gray;

                spriteBatch.DrawString(_pixelFont, costText, textPosition, textColor);
            }
            // FIN: DIBUJADO DE TEXTO

            // Dibujar capa de recarga (opcional, para visualización de cooldown)
            if (RechargeTimer > 0f && !IsReady())
            {
                float fillRatio = RechargeTimer / RechargeDuration;
                float fillHeight = 18f * fillRatio;

                // Rectángulo que simula la sombra (oscurece la parte superior del paquete)
                Rectangle sourceRectangle = new Rectangle(0, 0, 18, (int)fillHeight);

                // La posición se ajusta hacia abajo para que el llenado empiece desde abajo
                Vector2 drawPosition = this.Position + new Vector2(0f, 18f - fillHeight);

                // El color negro o semitransparente puede simular la recarga
                spriteBatch.Draw(
                    this.Sprite.currentGroup.Texture, // La textura completa del paquete
                    drawPosition,
                    sourceRectangle,
                    Color.Black * 0.75f, // Oscurece
                    0f,
                    Vector2.Zero,
                    Vector2.One,
                    SpriteEffects.None,
                    0.991f // Dibujar LIGERAMENTE por encima del ícono (0.99f)
                );
            }
        }

        public override void DrawSpriteCallback(SpriteBatch spriteBatch) { /* Se omite para usar el override Draw completo */ }
    }
}
