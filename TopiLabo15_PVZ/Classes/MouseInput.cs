// --- MouseInput.cs ---
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// Usamos una clase estática para poder acceder a ella globalmente (ej. MouseInput.IsPressed)
public static class MouseInput
{
    private static MouseState currentState;
    private static MouseState previousState;

    // --- Propiedades Públicas ---

    /// <summary>
    /// Verdadero SÓLO en el frame en que se HACE clic (transición de Soltado a Presionado).
    /// </summary>
    public static bool LeftButtonPressed { get; private set; }

    /// <summary>
    /// Verdadero mientras el clic se MANTIENE presionado (después del primer frame).
    /// </summary>
    public static bool LeftButtonHeld { get; private set; }

    /// <summary>
    /// Verdadero SÓLO en el frame en que se SUELTA el clic (transición de Presionado a Soltado).
    /// </summary>
    public static bool LeftButtonReleased { get; private set; }

    // INICIO: CORRECCIÓN CS0117
    /// <summary>
    /// Verdadero SÓLO en el frame en que se HACE clic derecho (transición de Soltado a Presionado).
    /// </summary>
    public static bool RightButtonPressed { get; private set; }

    /// <summary>
    /// Verdadero SÓLO en el frame en que se SUELTA el clic derecho (transición de Presionado a Soltado).
    /// </summary>
    public static bool RightButtonReleased { get; private set; }
    // FIN: CORRECCIÓN CS0117

    /// <summary>
    /// La posición actual del cursor en la pantalla (como Vector2).
    /// </summary>
    public static Vector2 Position { get; private set; }

    /// <summary>
    /// El movimiento del cursor desde el último frame (delta).
    /// </summary>
    public static Vector2 Delta { get; private set; }

    // --- Bucle de Actualización ---

    /// <summary>
    /// Este método DEBE ser llamado UNA VEZ por frame, al inicio del bucle Update.
    /// </summary>
    public static void Update()
    {
        // 1. Mover el estado "actual" al "anterior"
        previousState = currentState;

        // 2. Obtener el nuevo estado "actual"
        currentState = Mouse.GetState();

        // 3. ¡¡CONVERTIR COORDENADAS!!
        // Obtiene la posición de la ventana...
        Vector2 windowPosition = currentState.Position.ToVector2();
        // ...y la convierte a la posición del "mundo" del juego
        Position = ScreenManager.ScreenToWorld(windowPosition);

        // El resto de la lógica no cambia...

        // Calcular el Delta (cambio) en coordenadas del mundo
        Vector2 previousWorldPosition = ScreenManager.ScreenToWorld(previousState.Position.ToVector2());
        Delta = Position - previousWorldPosition;

        // 4. Calcular los estados del botón izquierdo
        LeftButtonPressed =
            (previousState.LeftButton == ButtonState.Released) &&
            (currentState.LeftButton == ButtonState.Pressed);

        LeftButtonHeld =
            (previousState.LeftButton == ButtonState.Pressed) &&
            (currentState.LeftButton == ButtonState.Pressed);

        LeftButtonReleased =
            (previousState.LeftButton == ButtonState.Pressed) &&
            (currentState.LeftButton == ButtonState.Released);

        // INICIO: CORRECCIÓN CS0117
        // 5. Calcular los estados del botón derecho
        RightButtonPressed =
            (previousState.RightButton == ButtonState.Released) &&
            (currentState.RightButton == ButtonState.Pressed);

        RightButtonReleased =
            (previousState.RightButton == ButtonState.Pressed) &&
            (currentState.RightButton == ButtonState.Released);
        // FIN: CORRECCIÓN CS0117
    }
}