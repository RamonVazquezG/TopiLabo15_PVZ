using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TopiLabo15_PVZ;

namespace TopiLabo15_PVZ.Data.Animations
{
    public class ZombiesAnims
    {
        public ZombiesAnims(ContentManager content)
        {
            // 1. Cargar la textura
            // NOTA: MonoGame elimina el tipo de archivo (como el .png) y la ruta 'Content/'.
            // Ejemplo, "Content/peaShooterAnimTest.png" se vuelve "peaShooterAnimTest"
            Texture2D text = content.Load<Texture2D>("zombiePlaceholder-Sheet");

            // 2. Crear el grupo.
            var grup = new AnimationGroup(text, 48, 48);

            // 3. Crear animaciónes.
            var walkAnim = new Animation(0, 24, 24);
            walkAnim.AddFrame(0, new Frame(0.1f));
            walkAnim.AddFrame(1, new Frame(0.1f));
            walkAnim.AddFrame(2, new Frame(0.1f));
            walkAnim.AddFrame(3, new Frame(0.1f));

            var eatAnim = new Animation(1, 24, 24);
            eatAnim.AddFrame(0, new Frame(0.2f));
            eatAnim.AddFrame(1, new Frame(0.2f));


            // 4. Añadir animaciones al grupo
            grup.AddAnimation("walk", walkAnim);
            grup.AddAnimation("eat", eatAnim);

            // 5. Añadir el grupo al diccionario principal. JC: Nunca olviden tambien instanciar esta clase en AnimationData.LoadContent().
            AnimationData.Add("zombieNormal", grup);
        }
    }
}
