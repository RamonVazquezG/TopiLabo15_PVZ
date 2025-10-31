using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TopiLabo15_PVZ;

namespace TopiLabo15_PVZ.Data.Animations
{
    public class WalnutAnims
    {
        public WalnutAnims(ContentManager content)
        {
            // 1. Cargar la textura
            // NOTA: MonoGame elimina el tipo de archivo (como el .png) y la ruta 'Content/'.
            // Ejemplo, "Content/peaShooterAnimTest.png" se vuelve "peaShooterAnimTest"
            Texture2D tex = content.Load<Texture2D>("walnut-Sheet");

            // 2. Crear el grupo.
            var grup = new AnimationGroup(tex, 48, 48);

            // 3. Crear animaciónes.
            var goodAnim = new Animation(0, 24, 32);
            goodAnim.AddFrame(0, new Frame(7.5f));
            goodAnim.AddFrame(1, new Frame(0.1f));

            var mediumAnim = new Animation(1, 24, 32);
            mediumAnim.AddFrame(0, new Frame(5f));
            mediumAnim.AddFrame(1, new Frame(0.1f));

            var badAnim = new Animation(2, 24, 32);
            badAnim.AddFrame(0, new Frame(0.05f));
            badAnim.AddFrame(1, new Frame(0.05f));
            badAnim.AddFrame(3, new Frame(0.05f));

            // 4. Añadir animaciones al grupo
            grup.AddAnimation("good", goodAnim);
            grup.AddAnimation("medium", mediumAnim);
            grup.AddAnimation("bad", badAnim);

            // 5. Añadir el grupo al diccionario principal. JC: Nunca olviden tambien instanciar esta clase en AnimationData.LoadContent().
            AnimationData.Add("walnut", grup);
        }
    }
}
