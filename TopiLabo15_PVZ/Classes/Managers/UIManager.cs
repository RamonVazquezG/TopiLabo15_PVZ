using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TopiLabo15_PVZ.Data
{
    public static class UIManager
    {
        // --- Paquetes de Semillas ---
        public static Texture2D TexPacketPeashooter { get; private set; }
        public static Texture2D TexPacketSunflower { get; private set; }
        public static Texture2D TexPacketWalnut { get; private set; }

        // --- Fantasmas del Cursor ---
        public static Texture2D TexGhostPeashooter { get; private set; }
        public static Texture2D TexGhostSunflower { get; private set; }
        public static Texture2D TexGhostWalnut { get; private set; }

        // --- Otros UI ---
        public static Texture2D TexCooldownOverlay { get; private set; }
        public static SpriteFont GameFont { get; private set; }

        // Llámalo desde Game1.LoadContent()
        public static void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            // --- Carga Texturas de Paquetes ---
            // ¡¡¡ASEGÚRATE DE AÑADIRLOS A Content.mgcb!!!
            TexPacketPeashooter = content.Load<Texture2D>("ui/packet_peashooter");
            TexPacketSunflower = content.Load<Texture2D>("ui/packet_sunflower");
            TexPacketWalnut = content.Load<Texture2D>("ui/packet_walnut");

            // --- Carga Texturas "Fantasma" (para el cursor) ---
            TexGhostPeashooter = content.Load<Texture2D>("plants/peashooter_ghost");
            TexGhostSunflower = content.Load<Texture2D>("plants/sunflower_ghost");
            TexGhostWalnut = content.Load<Texture2D>("plants/walnut_ghost");

            // --- Carga Fuente ---
            GameFont = content.Load<SpriteFont>("ui/gameFont"); // Asegúrate de tener este .spritefont

            // Crea una textura simple de 1x1 para el overlay
            TexCooldownOverlay = new Texture2D(graphicsDevice, 1, 1);
            TexCooldownOverlay.SetData(new[] { Color.Black });
        }
    }
}