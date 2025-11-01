using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TopiLabo15_PVZ;

namespace TopiLabo15_PVZ.Data.Animations
{
    public class UIAnims
    {
        public UIAnims(ContentManager content) //JC: Puro copy paste alv
        {
            // 1. Cargar la textura
            // NOTA: MonoGame elimina el tipo de archivo (como el .png) y la ruta 'Content/'.
            // Ejemplo, "Content/peaShooterAnimTest.png" se vuelve "peaShooterAnimTest"
            Texture2D shovelTex = content.Load<Texture2D>("shovel");

            // 2. Crear el grupo.
            var shovelGrup = new AnimationGroup(shovelTex, 22, 22);

            // 3. Crear animaciónes.
            var shovelAnim = new Animation(0, 11, 11);
            shovelAnim.AddFrame(0, new Frame(Globals.HUGE));

            // 4. Añadir animaciones al grupo
            shovelGrup.AddAnimation("default", shovelAnim);

            // 5. Añadir el grupo al diccionario principal. JC: Nunca olviden tambien instanciar esta clase en AnimationData.LoadContent().
            AnimationData.Add("uiShovel", shovelGrup);


            // 1. Cargar la textura
            // NOTA: MonoGame elimina el tipo de archivo (como el .png) y la ruta 'Content/'.
            // Ejemplo, "Content/peaShooterAnimTest.png" se vuelve "peaShooterAnimTest"
            Texture2D uiSunTex = content.Load<Texture2D>("uiSun-Sheet");

            // 2. Crear el grupo.
            var uiSunGrup = new AnimationGroup(uiSunTex, 11, 11);

            // 3. Crear animaciónes.
            var uiSunAnim = new Animation(0, 4, 5);
            uiSunAnim.AddFrame(0, new Frame(0.5f));
            uiSunAnim.AddFrame(1, new Frame(0.5f));

            // 4. Añadir animaciones al grupo
            uiSunGrup.AddAnimation("default", uiSunAnim);

            // 5. Añadir el grupo al diccionario principal. JC: Nunca olviden tambien instanciar esta clase en AnimationData.LoadContent().
            AnimationData.Add("uiSun", uiSunGrup);


            // 1. Cargar la textura
            // NOTA: MonoGame elimina el tipo de archivo (como el .png) y la ruta 'Content/'.
            // Ejemplo, "Content/peaShooterAnimTest.png" se vuelve "peaShooterAnimTest"
            Texture2D uiPlantSeedsTex = content.Load<Texture2D>("seeds");

            // 2. Crear el grupo.
            var uiPlantSeedsGrup = new AnimationGroup(uiPlantSeedsTex, 18, 18);

            // 3. Crear animaciónes.
            var uiPlantSeedsAnimPeaShooter = new Animation(0, 9, 9);
            uiPlantSeedsAnimPeaShooter.AddFrame(0, new Frame(Globals.HUGE));
            uiPlantSeedsAnimPeaShooter.AddFrame(1, new Frame(Globals.HUGE));

            var uiPlantSeedsAnimSunflower = new Animation(1, 9, 9);
            uiPlantSeedsAnimSunflower.AddFrame(0, new Frame(Globals.HUGE));
            uiPlantSeedsAnimSunflower.AddFrame(1, new Frame(Globals.HUGE));

            var uiPlantSeedsAnimWallnut = new Animation(2, 9, 9);
            uiPlantSeedsAnimWallnut.AddFrame(0, new Frame(Globals.HUGE));
            uiPlantSeedsAnimWallnut.AddFrame(1, new Frame(Globals.HUGE));

            // 4. Añadir animaciones al grupo
            uiPlantSeedsGrup.AddAnimation("0", uiPlantSeedsAnimPeaShooter); //Estos son los subtypes de PlantSubtypesEnum.
            uiPlantSeedsGrup.AddAnimation("1", uiPlantSeedsAnimSunflower);
            uiPlantSeedsGrup.AddAnimation("2", uiPlantSeedsAnimWallnut);

            // 5. Añadir el grupo al diccionario principal. JC: Nunca olviden tambien instanciar esta clase en AnimationData.LoadContent().
            AnimationData.Add("uiSeeds", uiPlantSeedsGrup);


            // 1. Cargar la textura
            // NOTA: MonoGame elimina el tipo de archivo (como el .png) y la ruta 'Content/'.
            // Ejemplo, "Content/peaShooterAnimTest.png" se vuelve "peaShooterAnimTest"
            Texture2D waveBarBoxTex = content.Load<Texture2D>("waveBar");

            // 2. Crear el grupo.
            var waveBarBoxGrup = new AnimationGroup(waveBarBoxTex, 88, 8);

            // 3. Crear animaciónes.
            var waveBarAnimBox = new Animation(0, 4, 2);
            waveBarAnimBox.AddFrame(0, new Frame(Globals.HUGE));

            var waveBarAnimFiller = new Animation(1, 4, 2);
            waveBarAnimFiller.AddFrame(0, new Frame(Globals.HUGE));

            // 4. Añadir animaciones al grupo
            waveBarBoxGrup.AddAnimation("waveBarBox", waveBarAnimBox);
            waveBarBoxGrup.AddAnimation("waveBarFiller", waveBarAnimFiller);

            // 5. Añadir el grupo al diccionario principal. JC: Nunca olviden tambien instanciar esta clase en AnimationData.LoadContent().
            AnimationData.Add("uiWaveBar", waveBarBoxGrup);
        }
    }
}
