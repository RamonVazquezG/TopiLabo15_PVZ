using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TopiLabo15_PVZ;

namespace TopiLabo15_PVZ.Data.Animations
{
    public class SunPickupAnims
    {
        public SunPickupAnims(ContentManager content)
        {
            // 1. Cargar la textura
            // NOTA: MonoGame elimina el tipo de archivo (como el .png) y la ruta 'Content/'.
            // Ejemplo, "Content/peaShooterAnimTest.png" se vuelve "peaShooterAnimTest"
            Texture2D tex = content.Load<Texture2D>("sunPickUp-sheet");

            // 2. Crear el grupo.
            var grup = new AnimationGroup(tex, 31, 31);

            // 3. Crear animaciónes.
            var defaultAnim = new Animation(0, 15, 18);
            defaultAnim.AddFrame(0, new Frame(0.2f));
            defaultAnim.AddFrame(1, new Frame(0.2f));
            defaultAnim.AddFrame(2, new Frame(0.2f));
            defaultAnim.AddFrame(3, new Frame(0.2f));

            var shineAnim = new Animation(1, 15, 15);
            shineAnim.AddFrame(0, new Frame(0.3f));
            shineAnim.AddFrame(1, new Frame(0.3f));
            shineAnim.AddFrame(2, new Frame(0.3f));
            shineAnim.AddFrame(3, new Frame(0.3f));

            // 4. Añadir animaciones al grupo
            grup.AddAnimation("default", defaultAnim);
            grup.AddAnimation("shine", shineAnim);

            // 5. Añadir el grupo al diccionario principal. JC: Nunca olviden tambien instanciar esta clase en AnimationData.LoadContent().
            AnimationData.Add("sunPickup", grup);
        }
    }
}
