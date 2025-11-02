// --- Necesitarás estas librerías de MonoGame ---
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

// --- 1. Equivalente a newFrame ---
// Almacena la duración de un frame y si reutiliza otro.
public class Frame
{
    // Duración del frame en segundos (equivale a _dur)
    public float Duration { get; private set; }

    // Índice del frame a reutilizar (equivale a _reusedFrame)
    // Usamos 'int?' (nullable int) para permitir un valor nulo.
    public int? ReusedFrame { get; private set; }

    public Frame(float duration, int? reusedFrame = null)
    {
        Duration = duration;
        ReusedFrame = reusedFrame;
    }
}

// --- 2. Equivalente a newAnimation ---
// Almacena una secuencia de frames, su fila en el spritesheet y su origen.
public class Animation
{
    // Fila en el spritesheet (equivale a _row)
    public int Row { get;  set; }

    // Origen/Pivote de la animación (equivale a _ox, _oy)
    public Vector2 Origin { get; private set; }

    // Diccionario de frames, usando el índice (0, 1, 2...) como clave.
    // Esto replica perfectamente la tabla de Lua (ej. anim[0] = newFrame(...))
    public Dictionary<int, Frame> Frames { get; private set; }

    public Animation(int row, float ox, float oy)
    {
        Row = row;
        Origin = new Vector2(ox, oy);
        Frames = new Dictionary<int, Frame>();
    }

    // Método de ayuda para añadir frames fácilmente
    public void AddFrame(int index, Frame frame)
    {
        Frames[index] = frame;
    }
}

// --- 3. Equivalente a newGroup ---
// Almacena la textura (spritesheet) y todas las animaciones que contiene.
public class AnimationGroup
{
    // La imagen del spritesheet (equivale a _image)
    public Texture2D Texture { get; private set; }

    // Dimensiones de cada celda (equivale a _gridWidth, _gridHeight)
    public int GridWidth { get; private set; }
    public int GridHeight { get; private set; }

    // Dimensiones de la textura completa (equivale a _sourceWidth, _sourceHeight)
    public int SourceWidth { get; private set; }
    public int SourceHeight { get; private set; }

    // Diccionario de animaciones, usando el nombre ("walk", "air") como clave.
    public Dictionary<string, Animation> Animations { get; private set; }

    public AnimationGroup(Texture2D texture, int gridWidth, int gridHeight)
    {
        Texture = texture;
        GridWidth = gridWidth;
        GridHeight = gridHeight;
        SourceWidth = texture.Width;
        SourceHeight = texture.Height;
        Animations = new Dictionary<string, Animation>();
    }

    // Método de ayuda para añadir animaciones fácilmente
    public void AddAnimation(string name, Animation animation)
    {
        Animations[name] = animation;
    }
}