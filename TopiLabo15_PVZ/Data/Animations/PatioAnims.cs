using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TopiLabo15_PVZ;

namespace TopiLabo15_PVZ.Data.Animations
{
    public class PatioAnims
    {
        public PatioAnims(ContentManager content)
        {
            // 1. Cargar la textura
            // NOTA: MonoGame elimina el tipo de archivo (como el .png) y la ruta 'Content/'.
            // "Content/peaShooterAnimTest.png" se vuelve "peaShooterAnimTest"
            Texture2D tex = content.Load<Texture2D>("patio");

            // 2. Crear el grupo.
            var grup = new AnimationGroup(tex, 264, 192);

            // 3. Crear animaciónes.
            var anim = new Animation(0, 0, 0);
            anim.AddFrame(0, new Frame(Globals.HUGE));

            // 4. Añadir animaciones al grupo
            grup.AddAnimation("default", anim);

            // 5. Añadir el grupo al diccionario principal
            AnimationData.Add("patio", grup);
        }
    }
}
