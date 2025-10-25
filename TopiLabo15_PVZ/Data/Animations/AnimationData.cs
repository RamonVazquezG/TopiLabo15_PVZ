// Esta clase estática contendrá todas las definiciones de animación cargadas.
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TopiLabo15_PVZ;
using TopiLabo15_PVZ.Data.Animations;

public static class AnimationData
{
    // El diccionario principal que equivale a tu tabla 'anims'
    public static Dictionary<string, AnimationGroup> Groups { get; private set; }

    static AnimationData()
    {
        Groups = new Dictionary<string, AnimationGroup>();
    }

    public static void Add(string name, AnimationGroup group)
    {
        Groups[name] = group;
    }

    // --- Carga de datos (Ejemplo) ---
    // En tu juego real, llamarías a esta función una vez al inicio.
    // 'Content' es el 'ContentManager' de MonoGame.
    public static void LoadContent(ContentManager content)
    {
        new PeaShooterAnims(content); //Con solo hacer una instancia, se registran las animaciones.
    }
}