// staarmaan/topilabo15_pvz/TopiLabo15_PVZ-Juan/TopiLabo15_PVZ/Data/Others/SeedPacketUI.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TopiLabo15_PVZ.Classes.Bases;
using TopiLabo15_PVZ.Classes.Entities;
using TopiLabo15_PVZ.Data.GameStates;
using TopiLabo15_PVZ.Data.Plants;

namespace TopiLabo15_PVZ.Data.Others
{
    public class SeedPacketUI : Entity
    {
        public PlantSubtypes PlantType { get; private set; }
        public int Cost { get; private set; }
        public float RechargeDuration { get; private set; }
        public float RechargeTimer { get; private set; }

        private PlayingGameState _gameState;
        private Vector2 _screenPosition;

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
                    return (9999, 99.0f);
            }
        }

        public SeedPacketUI(EntityManager manager, PlayingGameState gameState, PlantSubtypes plantType, Vector2 screenPosition)
            : base(manager, EntityTypes.UI, (int?)plantType, screenPosition, Vector2.Zero, null)
        {
            this._gameState = gameState;
            this.PlantType = plantType;
            this._screenPosition = screenPosition;

            var data = GetPlantData(plantType);
            this.Cost = data.cost;
            this.RechargeDuration = data.recharge;
            this.RechargeTimer = 0f; // Inicialmente listo

            // 🚨 CORRECCIÓN: Usamos la animación "default" de UIAnims.cs
            this.Sprite = new SpriteAnimator("uiSeeds", "default");

            // Hitbox para la interacción del mouse (18x33)
            this.Hitbox = new Hitbox(this, null, new Vector2(18f, 33f), Vector2.Zero, "seedPacket");
            this.Hitbox.Offset = new Vector2(0f, 0f);

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

            // INICIO: Lógica para forzar el Row y Frame correctos
            int targetRow = (int)this.PlantType; // Row: 0=Pea, 1=Sun, 2=Walnut
            int targetFrame = IsReady() ? 0 : 1; // Frame: 0=OK, 1=Recarga (Gris)

            // 🚨 CORRECCIÓN: Forzamos la fila (Row) y el frame.
            this.Sprite.currentAnimation.Row = targetRow;
            this.Sprite.SetFrame(targetFrame);

            // FIN: Lógica para forzar el Row y Frame correctos

            // Dibujamos en this.Position (esquina superior izquierda del slot 18x33)
            this.Sprite.Draw(spriteBatch, this.Position);

            // DIBUJADO DE TEXTO (Costo)
            if (this.Hitbox.Intersects(_gameState.MouseEntity.Hitbox))
            {
                // NOTA: Para que esto se vea, necesitas cargar una SpriteFont.
                // string costText = $"${Cost}";
                // spriteBatch.DrawString(font, costText, this.Position + new Vector2(18f, 18f), Color.Black);
            }
        }

        public override void DrawSpriteCallback(SpriteBatch spriteBatch) { /* Se omite para usar el override Draw completo */ }
    }
}