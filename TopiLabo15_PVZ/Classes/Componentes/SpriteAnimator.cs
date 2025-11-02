using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System; // Para Math.Floor

// --- Equivalente a la clase 'Sprite' en sprite.lua ---
public class SpriteAnimator
{
    // INICIO: CORRECCIÓN CS1061
    /// <summary>
    /// Proporciona acceso a los datos de la animación actual.
    /// </summary>
    public (int frameIndex, int row, Vector2 Origin) CurrentFrameData
    {
        get
        {
            if (currentAnimation == null || !currentAnimation.Frames.ContainsKey(FrameIndex))
            {
                // Devolver valores seguros si la animación no se ha cargado/inicializado
                return (0, 0, Vector2.Zero);
            }
            // Devolver la información del frame y el origen de la animación.
            return (FrameIndex, currentAnimation.Row, currentAnimation.Origin);
        }
    }
    // FIN: CORRECCIÓN CS1061

    // --- Campos de Estado ---
    public bool Visible { get; set; }
    public bool FlipX { get; set; }
    public bool FlipY { get; set; }
    public bool FloorXY { get; set; } // Redondear posición al dibujar

    public bool JustChangedFrame { get; private set; }
    public bool JustFinishedAnimation { get; private set; }

    public int FrameIndex { get; private set; }
    private float frameTimer;

    // --- Propiedades de Transformación ---
    // Origen/pivote manual
    public Vector2 Offset { get; set; }

    // Multiplicador de tiempo dt
    public float SpeedMultiplier { get; set; }

    public float LayerDepth { get; set; }

    public Vector2 Scale { get; set; }

    // Rotación en radianes 
    public float Angle { get; set; }

    // --- Referencias de Animación ---
    private string currentGroupKey;
    private string currentAnimationKey;

    public AnimationGroup currentGroup;
    // 🚨 CORRECCIÓN: Hacemos public para permitir la manipulación de Row en SeedPacketUI
    public Animation currentAnimation; 

    public SpriteAnimator(string group, string animation = "default",
                          bool floorXY = true, float angle = 0f,
                          float sx = 1f, float sy = 1f,
                          float ox = 0f, float oy = 0f, float dtMulti = 1f, float layerDepth = 0f)
    {
        Visible = true;
        FloorXY = floorXY;
        Angle = angle;
        Scale = new Vector2(sx, sy);
        Offset = new Vector2(ox, oy);
        SpeedMultiplier = dtMulti;
        LayerDepth = layerDepth;

        // Llama a Play para configurar la animación inicial
        Play(animation, true, group);
    }

    // --- Equivalente a Sprite:update ---
    public void Update(float dt)
    {
        JustChangedFrame = false;
        JustFinishedAnimation = false;

        frameTimer += dt * SpeedMultiplier;

        // Asegurarse de que el frame actual existe
        if (!currentAnimation.Frames.ContainsKey(FrameIndex))
        {
            SetFrame(0);
        }

        // Obtener el frame actual
        Frame currentFrame = currentAnimation.Frames[FrameIndex];

        // Avanzar frames mientras haya tiempo acumulado
        // (Esto replica el 'while' en tu código de Lua)
        while (frameTimer >= currentFrame.Duration)
        {
            frameTimer -= currentFrame.Duration;
            JustChangedFrame = true;

            int nextFrameIndex = FrameIndex + 1;

            // Si el siguiente frame existe...
            if (currentAnimation.Frames.ContainsKey(nextFrameIndex))
            {
                SetFrame(nextFrameIndex);
            }
            else // Si no, la animación terminó
            {
                JustFinishedAnimation = true;
                SetFrame(0); // Volver al inicio
            }

            // Obtener el nuevo frame para la siguiente iteración del 'while'
            currentFrame = currentAnimation.Frames[FrameIndex];
        }
    }

    // --- Equivalente a Sprite:play ---
    public void Play(string animation, bool resetFrame = false, string group = null)
    {
        // Cambiar de grupo si se especifica uno nuevo
        if (group != null && currentGroupKey != group)
        {
            currentGroupKey = group;
            currentGroup = AnimationData.Groups[currentGroupKey];
        }

        // Cambiar de animación
        if (currentAnimationKey != animation)
        {
            currentAnimationKey = animation;
            currentAnimation = currentGroup.Animations[currentAnimationKey];
            if (resetFrame)
            {
                SetFrame(0);
            }
        }
        // Caso especial: si se llama a Play("misma_animacion", true)
        else if (resetFrame)
        {
            SetFrame(0);
        }
    }

    public bool IsPlaying(string animName)
    {
        return currentAnimationKey.Equals(animName);
    }

    // --- Equivalente a Sprite:setFrame ---
    public void SetFrame(int index)
    {
        FrameIndex = index;
        frameTimer = 0.0f;

        // Validar que el frame exista
        if (!currentAnimation.Frames.ContainsKey(FrameIndex))
        {
            // Fallback seguro: ir al frame 0
            FrameIndex = 0;
            // (Opcional: lanzar un error)
            // throw new IndexOutOfRangeException($"Frame {index} no existe en {currentGroupKey}/{currentAnimationKey}");
        }
    }

    // --- Equivalente a Sprite:draw ---
    // Necesita el SpriteBatch de MonoGame para dibujar
    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        if (!Visible || currentAnimation == null)
            return;

        // --- 1. Obtener el frame y el índice a usar ---
        Frame frameData = currentAnimation.Frames[FrameIndex];
        int frameToDraw = frameData.ReusedFrame ?? FrameIndex; // Usa el frame reutilizado si existe

        // --- 2. Calcular el 'sourceRectangle' (el 'Quad' en LÖVE) ---
        int quadX = frameToDraw * currentGroup.GridWidth;
        int quadY = currentAnimation.Row * currentGroup.GridHeight;

        Rectangle sourceRectangle = new Rectangle(
            quadX,
            quadY,
            currentGroup.GridWidth,
            currentGroup.GridHeight
        );

        // --- 3. Calcular origen/pivote ---
        // Suma el origen de la animación + el offset manual del sprite
        Vector2 origin = currentAnimation.Origin + Offset;

        // --- 4. Calcular escala y efectos de 'flip' ---
        Vector2 finalScale = Scale;
        SpriteEffects effects = SpriteEffects.None;

        if (FlipX)
        {
            finalScale.X *= -1;
            effects |= SpriteEffects.FlipHorizontally;
        }
        if (FlipY)
        {
            finalScale.Y *= -1;
            effects |= SpriteEffects.FlipVertically;
        }

        // --- 5. Redondear posición (si está activado) ---
        if (FloorXY)
        {
            position.X = (float)Math.Floor(position.X);
            position.Y = (float)Math.Floor(position.Y);
        }

        // --- 6. ¡Dibujar! ---
        spriteBatch.Draw(
            currentGroup.Texture,  // La textura (spritesheet)
            position,              // Posición en pantalla
            sourceRectangle,       // El 'Quad' (qué parte de la textura dibujar)
            Color.White,           // Tinte (blanco = sin tinte)
            Angle,                 // Rotación (en radianes)
            origin,                // Origen/Pivote (equivale a ox, oy)
            Scale,                 // Escala (¡Ojo! MonoGame maneja el flip con 'effects')
            effects,               // Aquí es donde van FlipX y FlipY
            LayerDepth                     // Profundidad de capa (layer depth)
        );
    }

    // ... (El resto del código)
}