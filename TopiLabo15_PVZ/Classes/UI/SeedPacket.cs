// TopiLabo15_PVZ/Classes/UI/SeedPacket.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TopiLabo15_PVZ.Data;
using TopiLabo15_PVZ.Data.Plants; // <-- Asegúrate que este 'using' apunte a donde tengas tu enum 'PlantSubtypes'

namespace TopiLabo15_PVZ.Classes.UI
{
    public class SeedPacket
    {
        // ¡Cambiamos 'PlantType' por 'PlantSubtypes' de tu proyecto!
        public PlantSubtypes PlantSubtype { get; }
        public int Cost { get; }
        public float CooldownTime { get; }
        public Rectangle Bounds { get; private set; }

        private float _currentCooldown;
        private Texture2D _texture;
        private Texture2D _cooldownOverlay;

        public SeedPacket(PlantSubtypes type, int cost, float cooldown, Texture2D texture, Texture2D overlay)
        {
            PlantSubtype = type;
            Cost = cost;
            CooldownTime = cooldown;
            _texture = texture;
            _cooldownOverlay = overlay;
            _currentCooldown = 0;
        }

        public bool IsReady => _currentCooldown <= 0;

        public bool CanSelect(int currentSun)
        {
            return IsReady && currentSun >= Cost;
        }

        public void StartCooldown()
        {
            _currentCooldown = CooldownTime;
        }

        public void Update(float dt)
        {
            if (_currentCooldown > 0)
            {
                _currentCooldown -= dt;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Bounds = new Rectangle((int)position.X, (int)position.Y, _texture.Width, _texture.Height);

            spriteBatch.Draw(_texture, position, Color.White);

            if (!IsReady)
            {
                float cooldownPercent = _currentCooldown / CooldownTime;
                int overlayHeight = (int)(Bounds.Height * cooldownPercent);
                int overlayY = Bounds.Y + (Bounds.Height - overlayHeight); // El overlay sube

                Rectangle overlayRect = new Rectangle(Bounds.X, overlayY, Bounds.Width, overlayHeight);

                spriteBatch.Draw(_cooldownOverlay, overlayRect, new Color(0, 0, 0, 0.7f));
            }
        }
    }
}