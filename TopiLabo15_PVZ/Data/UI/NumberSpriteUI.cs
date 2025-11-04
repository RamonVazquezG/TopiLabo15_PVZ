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
    public class NumberSpriteUI : Entity
    {
        private int count = 0;
        public NumberSpriteUI(EntityManager manager, Vector2 screenPosition, string animationName)
            : base(manager, EntityTypes.UI, null, screenPosition, Vector2.Zero, null)
        {
            this.Sprite = new SpriteAnimator("numberSprite", animationName); //La animacion se llama por el PlantSubtypes como string.
        }

        public void SetCount(int newCount)
        {
            Math.Abs(newCount);
            this.count = newCount;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.Sprite == null) return;

            string countInString = count.ToString();
            int countDigits = countInString.Length;
            for (int i = 0; i < countDigits; i++)
            {
                char digitChar = countInString[i];
                int digit = digitChar - '0'; // Convertir char a int

                Vector2 digitPosition = new Vector2(
                    this.Position.X + (i * 6) - 3, // Ajusta el espaciado entre dígitos
                    this.Position.Y
                );

                this.Sprite.SetFrame(digit);
                this.Sprite.LayerDepth = 0.98f; // Dibujar detrás del sprite principal
                this.Sprite.Draw(spriteBatch, digitPosition);
            }
        }
    }
}