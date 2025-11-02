// staarmaan/topilabo15_pvz/TopiLabo15_PVZ-Juan/TopiLabo15_PVZ/Data/Animations/UIAnims.cs

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TopiLabo15_PVZ;
using TopiLabo15_PVZ.Data.Plants;

namespace TopiLabo15_PVZ.Data.Animations
{
    public class UIAnims
    {
        public UIAnims(ContentManager content)
        {
            // --- UI Shovel ---
            Texture2D shovelTex = content.Load<Texture2D>("shovel");
            var shovelGrup = new AnimationGroup(shovelTex, 22, 22);
            var shovelAnim = new Animation(0, 11, 11);
            shovelAnim.AddFrame(0, new Frame(Globals.HUGE));
            shovelGrup.AddAnimation("default", shovelAnim);
            AnimationData.Add("uiShovel", shovelGrup);


            // --- UI Sun Icon ---
            Texture2D uiSunTex = content.Load<Texture2D>("uiSun-Sheet");
            var uiSunGrup = new AnimationGroup(uiSunTex, 11, 11);
            var uiSunAnim = new Animation(0, 4, 5);
            uiSunAnim.AddFrame(0, new Frame(0.5f));
            uiSunAnim.AddFrame(1, new Frame(0.5f));
            uiSunGrup.AddAnimation("default", uiSunAnim);
            AnimationData.Add("uiSun", uiSunGrup);


            // --- UI Seed Packets (uiSeeds) ---
            Texture2D seedsTex = content.Load<Texture2D>("seeds");
            var seedsGrup = new AnimationGroup(seedsTex, 18, 18);

            // 3. Crear una ÚNICA animación "default".
            var peaShooterPacketAnims = new Animation((int)PlantSubtypes.PeaShooter, 9, 9); // Row 0, Origen (9, 9) - Centro del slot
            peaShooterPacketAnims.AddFrame(0, new Frame(Globals.HUGE)); // Frame 0: Icono Gris/Recarga
            peaShooterPacketAnims.AddFrame(1, new Frame(Globals.HUGE)); // Frame 1: Icono OK (Brillante)

            var sunFlowerPacketAnims = new Animation((int)PlantSubtypes.SunFlower, 9, 9); 
            sunFlowerPacketAnims.AddFrame(0, new Frame(Globals.HUGE)); 
            sunFlowerPacketAnims.AddFrame(1, new Frame(Globals.HUGE));

            var walnutPacketAnims = new Animation((int)PlantSubtypes.WallNut, 9, 9);
            walnutPacketAnims.AddFrame(0, new Frame(Globals.HUGE));
            walnutPacketAnims.AddFrame(1, new Frame(Globals.HUGE));
            // 4. Añadir la animación al grupo.
            seedsGrup.AddAnimation((int)PlantSubtypes.PeaShooter+"", peaShooterPacketAnims); // Corresponden a los IDs de PlantSubtypes, para que acceder fácilmente a estas animaciones.
            seedsGrup.AddAnimation((int)PlantSubtypes.SunFlower+"", sunFlowerPacketAnims); // JC: Quiazas no sea intuitivo, pero creo que es mas eficiente.
            seedsGrup.AddAnimation((int)PlantSubtypes.WallNut+"", walnutPacketAnims);

            // 5. Añadir el grupo al diccionario principal.
            AnimationData.Add("uiSeeds", seedsGrup);


            // --- UI Wave Bar ---
            // 🚨 CORRECCIÓN CS1002/CS1526: Eliminamos el 'new new' redundante y usamos nombres locales limpios.
            Texture2D waveBarTex = content.Load<Texture2D>("waveBar");
            var waveBarGrup = new AnimationGroup(waveBarTex, 88, 8);

            var waveBarAnimBox = new Animation(0, 4, 2); // ⬅️ Corregido: solo 'new Animation'
            waveBarAnimBox.AddFrame(0, new Frame(Globals.HUGE));

            var waveBarAnimFiller = new Animation(1, 4, 2); // ⬅️ Corregido: solo 'new Animation'
            waveBarAnimFiller.AddFrame(0, new Frame(Globals.HUGE));

            // 4. Añadir animaciones al grupo
            waveBarGrup.AddAnimation("waveBarBox", waveBarAnimBox);
            waveBarGrup.AddAnimation("waveBarFiller", waveBarAnimFiller);

            // 5. Añadir el grupo al diccionario principal. 
            AnimationData.Add("uiWaveBar", waveBarGrup);
        }
    }
}