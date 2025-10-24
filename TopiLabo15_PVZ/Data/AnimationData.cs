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
        Texture2D peaShooterTex = content.Load<Texture2D>("assets/images/plants/peaShooterAnimTest");

        // 2. Crear el grupo (newGroup)
        var peaShooterGrup = new AnimationGroup(peaShooterTex, 48, 48);

        // 3. Crear animaciónes
        var peaShootIdleAnim = new Animation(0, 23, 23);
        peaShootIdleAnim.AddFrame(0, new Frame(0.1f));
        peaShootIdleAnim.AddFrame(1, new Frame(0.2f));
        peaShootIdleAnim.AddFrame(2, new Frame(0.3f));
        peaShootIdleAnim.AddFrame(3, new Frame(0.1f));
        peaShootIdleAnim.AddFrame(4, new Frame(0.2f));
        peaShootIdleAnim.AddFrame(5, new Frame(0.3f));

        // 4. Añadir animaciones al grupo
        peaShooterGrup.AddAnimation("idle", peaShootIdleAnim);

        // 5. Añadir el grupo al diccionario principal
        Groups["peaShoter"] = peaShooterGrup;
    }
}