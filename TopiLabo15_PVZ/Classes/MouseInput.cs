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

        // 3. Actualizar propiedades de posición
        Position = currentState.Position.ToVector2();
        Delta = (currentState.Position - previousState.Position).ToVector2();

        // 4. Calcular los estados del botón izquierdo

        // ¿Estaba suelto el frame pasado Y presionado este frame?
        LeftButtonPressed =
            (previousState.LeftButton == ButtonState.Released) &&
            (currentState.LeftButton == ButtonState.Pressed);

        // ¿Estaba presionado el frame pasado Y sigue presionado este frame?
        LeftButtonHeld =
            (previousState.LeftButton == ButtonState.Pressed) &&
            (currentState.LeftButton == ButtonState.Pressed);

        // ¿Estaba presionado el frame pasado Y suelto este frame?
        LeftButtonReleased =
            (previousState.LeftButton == ButtonState.Pressed) &&
            (currentState.LeftButton == ButtonState.Released);
    }
}