// --- MouseInput.cs ---
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public static class MouseInput
{
    private static MouseState currentState;
    private static MouseState previousState;

    // --- Propiedades Públicas ---

    // --- Botón Izquierdo ---
    public static bool LeftButtonPressed { get; private set; }
    public static bool LeftButtonHeld { get; private set; }
    public static bool LeftButtonReleased { get; private set; }

    // --- NUEVO: Botón Derecho ---
    public static bool RightButtonPressed { get; private set; }
    public static bool RightButtonHeld { get; private set; }
    public static bool RightButtonReleased { get; private set; }

    // --- Posición y Delta ---
    public static Vector2 Position { get; private set; }
    public static Vector2 Delta { get; private set; }

    public static void Update()
    {
        previousState = currentState;
        currentState = Mouse.GetState();

        Vector2 windowPosition = currentState.Position.ToVector2();
        Position = ScreenManager.ScreenToWorld(windowPosition);

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

        // --- NUEVO: 5. Calcular los estados del botón derecho ---
        RightButtonPressed =
            (previousState.RightButton == ButtonState.Released) &&
            (currentState.RightButton == ButtonState.Pressed);

        RightButtonHeld =
            (previousState.RightButton == ButtonState.Pressed) &&
            (currentState.RightButton == ButtonState.Pressed);

        RightButtonReleased =
            (previousState.RightButton == ButtonState.Pressed) &&
            (currentState.RightButton == ButtonState.Released);
    }
}