// --- GameManager.cs ---
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics; // Para Debug.WriteLine (en lugar de 'print')

// Una clase estática es un Singleton. No se pueden crear instancias de ella.
// Equivale a 'Game = {} ... return Game:new()'
public static class GameManager
{
    // Equivale a self.currentGamestate
    public static GameState CurrentGameState { get; private set; }

    // Equivale a self.gamestates
    private static List<GameState> allGameStates = new List<GameState>();

    // No necesitamos 'new()'. Los campos estáticos existen desde que se inicia el programa.

    // Equivale a registerNewGamestate
    public static void RegisterNewGameState(GameState gameStateInstance)
    {
        allGameStates.Add(gameStateInstance);
    }

    // Equivale a switchGamestate
    public static void SwitchGameState(GameState newGameState)
    {
        if (newGameState == null) return;
        if (newGameState == CurrentGameState) return;

        GameState previousGameState = CurrentGameState;
        CurrentGameState = newGameState;

        // 1. Llamar OnExit en el estado anterior (si existe)
        previousGameState?.OnExit(); // El '?' evita errores si es nulo

        // 2. Inicializar el nuevo estado si es la primera vez
        if (!newGameState.IsInitialized)
        {
            newGameState.IsInitialized = true;
            newGameState.OnInIt();
        }

        // 3. Llamar OnEnter en el nuevo estado
        newGameState.OnEnter();

        // 4. Detener otros estados según las reglas (si el estado anterior existe)
        previousGameState?.TryToStopStatesOnSwitch(newGameState);
    }

    // Equivale a stopGamestate
    public static void StopGameState(GameState gameStateInstance)
    {
        if (gameStateInstance == null) return;

        if (gameStateInstance == CurrentGameState)
        {
            Debug.WriteLine("¡No se puede detener un estado de juego que está activo!");
            return;
        }

        if (!gameStateInstance.IsInitialized) return;

        gameStateInstance.OnStop();
        gameStateInstance.IsInitialized = false;

        // Nota: 'collectgarbage("collect")' no es necesario en C#.
        // El Garbage Collector de .NET maneja la memoria automáticamente.
    }

    // --- Métodos de Bucle de Juego (para llamar desde Game1.cs) ---
    // (Equivalentes a updateCurrentGamestate y drawCurrentGamestate)

    public static void Update(float dt)
    {
        // El '?' (null-conditional operator) se asegura de no hacer nada
        // si CurrentGameState es nulo.
        CurrentGameState?.GenericUpdate(dt);
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        CurrentGameState?.Draw(spriteBatch);
    }
}