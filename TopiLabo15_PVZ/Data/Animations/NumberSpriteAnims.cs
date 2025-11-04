using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TopiLabo15_PVZ;

namespace TopiLabo15_PVZ.Data.Animations
{
    public class NumberSpritesAnims
    {
        public NumberSpritesAnims(ContentManager content)
        {
            // 1. Cargar la textura
            // NOTA: MonoGame elimina el tipo de archivo (como el .png) y la ruta 'Content/'.
            // Ejemplo, "Content/peaShooterAnimTest.png" se vuelve "peaShooterAnimTest"
            Texture2D tex = content.Load<Texture2D>("numbers-Sheet");

            // 2. Crear el grupo.
            var grup = new AnimationGroup(tex, 7, 8);

            // 3. Crear animaciónes.
            var sunCountAnim = new Animation(0, 0, 0);
            for (int i = 0; i <= 9; i++) { sunCountAnim.AddFrame(i, new Frame(Globals.HUGE)); }
            
            var waveCountAnim = new Animation(1, 0, 0);
            for (int i = 0; i <= 9; i++) { waveCountAnim.AddFrame(i, new Frame(Globals.HUGE)); }

            // 4. Añadir animaciones al grupo
            grup.AddAnimation("suns", sunCountAnim);
            grup.AddAnimation("waves", waveCountAnim);

            // 5. Añadir el grupo al diccionario principal. JC: Nunca olviden tambien instanciar esta clase en AnimationData.LoadContent().
            AnimationData.Add("numberSprite", grup);
        }
    }
}
