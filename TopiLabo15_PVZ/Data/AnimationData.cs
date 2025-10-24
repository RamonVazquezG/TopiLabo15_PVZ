// Esta clase estática contendrá todas las definiciones de animación cargadas.
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TopiLabo15_PVZ;

public static class AnimationData
{
    // El diccionario principal que equivale a tu tabla 'anims'
    public static Dictionary<string, AnimationGroup> Groups { get; private set; }

    static AnimationData()
    {
        Groups = new Dictionary<string, AnimationGroup>();
    }

    // --- Carga de datos (Ejemplo) ---
    // En tu juego real, llamarías a esta función una vez al inicio.
    // 'Content' es el 'ContentManager' de MonoGame.
    public static void LoadContent(ContentManager content)
    {
        // Esta es la parte que harías con un cargador de JSON/XML.
        // Por ahora, replicamos a mano una entrada de 'animations.lua'
        // para demostrar cómo funciona.

        // Suponiendo que tienes valores para FRAME y HUGE
        const float FRAME = Globals.FRAME;
        const float HUGE = Globals.HUGE;

        // 1. Cargar la textura
        // NOTA: MonoGame elimina la extensión y la ruta 'Content/'.
        // 'assets/images/player/playerSkin_1.png' se vuelve 'assets/images/player/playerSkin_1'
        Texture2D playerSkin1Tex = content.Load<Texture2D>("assets/images/player/playerSkin_1");

        // 2. Crear el grupo (newGroup)
        var playerSkin1 = new AnimationGroup(playerSkin1Tex, 8, 8);

        // 3. Crear animación 'walk' (newAnimation)
        var walkAnim = new Animation(0, 4, 4);
        walkAnim.AddFrame(0, new Frame(FRAME));       // walk[0]
        walkAnim.AddFrame(1, new Frame(FRAME * 3));   // walk[1]
        walkAnim.AddFrame(2, new Frame(FRAME));       // walk[2]
        walkAnim.AddFrame(3, new Frame(FRAME * 3));   // walk[3]

        // 4. Crear animación 'air' (newAnimation)
        var airAnim = new Animation(1, 4, 4);
        airAnim.AddFrame(0, new Frame(HUGE)); // air[0]

        // 5. Añadir animaciones al grupo
        playerSkin1.AddAnimation("walk", walkAnim);
        playerSkin1.AddAnimation("air", airAnim);

        // 6. Añadir el grupo al diccionario principal
        Groups["playerSkin_1"] = playerSkin1;

        // ... Repetirías esto para todos los grupos ...
    }
}