using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TopiLabo15_PVZ;

namespace TopiLabo15_PVZ.Data.Animations
{
    public class PeaShooterAnims
    {
        public PeaShooterAnims(ContentManager content)
        {
            // 1. Cargar la textura
            // NOTA: MonoGame elimina el tipo de archivo (como el .png) y la ruta 'Content/'.
            // Ejemplo, "Content/peaShooterAnimTest.png" se vuelve "peaShooterAnimTest"
            Texture2D peaShooterTex = content.Load<Texture2D>("peaShooterAnimTest");

            // 2. Crear el grupo.
            var peaShooterGrup = new AnimationGroup(peaShooterTex, 48, 48);

            // 3. Crear animaciónes.
            var peaShootIdleAnim = new Animation(0, 23, 32);
            peaShootIdleAnim.AddFrame(0, new Frame(0.1f));
            peaShootIdleAnim.AddFrame(1, new Frame(0.2f));
            peaShootIdleAnim.AddFrame(2, new Frame(0.3f));
            peaShootIdleAnim.AddFrame(3, new Frame(0.1f));
            peaShootIdleAnim.AddFrame(4, new Frame(0.2f));
            peaShootIdleAnim.AddFrame(5, new Frame(0.3f));

            var peaShootShootAnim = new Animation(1, 23, 32);
            peaShootShootAnim.AddFrame(0, new Frame(0.08f));
            peaShootShootAnim.AddFrame(0, new Frame(0.16f));
            peaShootShootAnim.AddFrame(0, new Frame(0.32f));
            peaShootShootAnim.AddFrame(0, new Frame(0.05f));
            peaShootShootAnim.AddFrame(0, new Frame(0.100f));
            peaShootShootAnim.AddFrame(0, new Frame(0.1f));

            var peaShootProjectileAnim = new Animation(2, 24, 24);
            peaShootProjectileAnim.AddFrame(0, new Frame(0.1f));
            peaShootProjectileAnim.AddFrame(0, new Frame(0.1f));

            // 4. Añadir animaciones al grupo
            peaShooterGrup.AddAnimation("idle", peaShootIdleAnim);
            peaShooterGrup.AddAnimation("shoot", peaShootShootAnim);
            peaShooterGrup.AddAnimation("projectile", peaShootProjectileAnim);

            // 5. Añadir el grupo al diccionario principal. JC: Nunca olviden tambien instanciar esta clase en AnimationData.LoadContent().
            AnimationData.Add("peaShooter", peaShooterGrup);
        }
    }
}
