using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TopiLabo15_PVZ;

namespace TopiLabo15_PVZ.Data.Animations
{
    public class SunFlowerAnims
    {
        public SunFlowerAnims(ContentManager content)
        {
            // 1. Cargar la textura
            // NOTA: MonoGame elimina el tipo de archivo (como el .png) y la ruta 'Content/'.
            // Ejemplo, "Content/peaShooterAnimTest.png" se vuelve "peaShooterAnimTest"
            Texture2D tex = content.Load<Texture2D>("sunFlower-sheet");

            // 2. Crear el grupo.
            var grup = new AnimationGroup(tex, 48, 48);

            // 3. Crear animaciónes.
            var idleAnim = new Animation(0, 23, 32);
            idleAnim.AddFrame(0, new Frame(0.1f));
            idleAnim.AddFrame(1, new Frame(0.75f));
            idleAnim.AddFrame(2, new Frame(0.2f));
            idleAnim.AddFrame(3, new Frame(0.1f));
            idleAnim.AddFrame(4, new Frame(0.75f));
            idleAnim.AddFrame(5, new Frame(0.2f));

            var sunAnim = new Animation(1, 23, 32);
            sunAnim.AddFrame(0, new Frame(0.5f));
            sunAnim.AddFrame(1, new Frame(0.1f));
            sunAnim.AddFrame(2, new Frame(0.1f));
            sunAnim.AddFrame(3, new Frame(0.1f));
            sunAnim.AddFrame(4, new Frame(1f));

            // 4. Añadir animaciones al grupo
            grup.AddAnimation("idle", idleAnim);
            grup.AddAnimation("sunGive", sunAnim);

            // 5. Añadir el grupo al diccionario principal. JC: Nunca olviden tambien instanciar esta clase en AnimationData.LoadContent().
            AnimationData.Add("sunFlower", grup);
        }
    }
}
