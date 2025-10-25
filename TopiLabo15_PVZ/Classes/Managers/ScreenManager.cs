// --- ScreenManager.cs ---
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public static class ScreenManager
{
    // 1. Define tus resoluciones
    public const int VIRTUAL_WIDTH = 264;  // La resolución baja de tu "juego" (nose porque gemini me lo puso en comillas xddd)
    public const int VIRTUAL_HEIGHT = 192; // (Ejemplo 16:9)
    //Por cierto el ratio es de 11:8 pero no importa mucho :p

    public static int WINDOW_WIDTH { get; private set; }
    public static int WINDOW_HEIGHT { get; private set; }

    // El "lienzo" virtual donde dibujaremos el juego
    private static RenderTarget2D _virtualRenderTarget;

    // El rectángulo que define dónde se dibujará el lienzo en la ventana (para letterboxing)
    private static Rectangle _destinationRectangle;

    // Referencias de MonoGame
    private static GraphicsDeviceManager _graphics;
    private static GraphicsDevice _graphicsDevice;

    public static void Initialize(GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice)
    {
        _graphics = graphics;
        _graphicsDevice = graphicsDevice;

        // Configura el tamaño de la ventana. Puedes cambiar esto.
        WINDOW_WIDTH = 1280;
        WINDOW_HEIGHT = 720;

        _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
        _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
        _graphics.ApplyChanges();

        // Crea el RenderTarget
        _virtualRenderTarget = new RenderTarget2D(
            _graphicsDevice,
            VIRTUAL_WIDTH,
            VIRTUAL_HEIGHT
        );

        // Calcula el rectángulo de destino (letterboxing)
        UpdateDestinationRectangle();
    }

    // Calcula cómo escalar y centrar la pantalla virtual dentro de la ventana real
    public static void UpdateDestinationRectangle()
    {
        float scaleX = (float)WINDOW_WIDTH / VIRTUAL_WIDTH;
        float scaleY = (float)WINDOW_HEIGHT / VIRTUAL_HEIGHT;

        // Elige la escala más pequeña para mantener el aspect ratio (letterbox)
        float scale = Math.Min(scaleX, scaleY);

        int newWidth = (int)(VIRTUAL_WIDTH * scale);
        int newHeight = (int)(VIRTUAL_HEIGHT * scale);

        // Centra el rectángulo en la pantalla
        int posX = (WINDOW_WIDTH - newWidth) / 2;
        int posY = (WINDOW_HEIGHT - newHeight) / 2;

        _destinationRectangle = new Rectangle(posX, posY, newWidth, newHeight);
    }

    // Llama a esto ANTES de empezar a dibujar tu juego
    public static void BeginRender()
    {
        // Redirige todo el dibujado a nuestro lienzo virtual
        _graphicsDevice.SetRenderTarget(_virtualRenderTarget);
        _graphicsDevice.Clear(Color.Black); // Limpia el lienzo virtual
    }

    // Llama a esto DESPUÉS de que hayas dibujado todo tu juego
    public static void EndRender(SpriteBatch spriteBatch)
    {
        // Vuelve a dibujar en la ventana principal
        _graphicsDevice.SetRenderTarget(null);
        _graphicsDevice.Clear(Color.Black); // Limpia la ventana real (para las barras negras)

        // Dibuja el lienzo virtual en la ventana, estirado
        spriteBatch.Begin(
            SpriteSortMode.Deferred, // No importa el orden
            BlendState.Opaque,       // Sin transparencias
            SamplerState.PointClamp, // ¡¡LA CLAVE!! Mantiene los píxeles nítidos
            null, null, null
        );

        spriteBatch.Draw(
            _virtualRenderTarget,   // La textura de nuestro juego
            _destinationRectangle,  // Dónde dibujarla en la ventana
            Color.White
        );

        spriteBatch.End();
    }

    // --- ¡Importante! Conversión de Coordenadas del Mouse ---
    // El mouse estará en coordenadas de VENTANA (ej. 0-1280)
    // Necesitamos convertirlas a coordenadas de JUEGO (ej. 0-640)

    public static Vector2 ScreenToWorld(Vector2 screenPosition)
    {
        // Primero, quita el offset de las barras negras
        float mouseX = screenPosition.X - _destinationRectangle.X;
        float mouseY = screenPosition.Y - _destinationRectangle.Y;

        // Ahora, escala la posición
        float scaleX = (float)VIRTUAL_WIDTH / _destinationRectangle.Width;
        float scaleY = (float)VIRTUAL_HEIGHT / _destinationRectangle.Height;

        return new Vector2(mouseX * scaleX, mouseY * scaleY);
    }
}