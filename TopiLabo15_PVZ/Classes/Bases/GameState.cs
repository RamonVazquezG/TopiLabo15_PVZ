// --- GameState.cs ---
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq; // Para .ToList()

// Clase base abstracta, equivale a 'Gamestate: Object'
public abstract class GameState
{
    // Equivale a self.init
    public bool IsInitialized { get; set; } = false;

    // Equivale a self.entityManager
    public EntityManager EntityManager { get; private set; }

    // Equivale a self.stopTheseWhenSwitchToThese
    // Un diccionario donde la clave es el estado al que cambias,
    // y el valor es la lista de estados que deben detenerse.
    private Dictionary<GameState, List<GameState>> StopTheseWhenSwitchToThese;

    // --- Constructor (Equivale a Gamestate:new) ---
    public GameState()
    {
        EntityManager = new EntityManager();
        StopTheseWhenSwitchToThese = new Dictionary<GameState, List<GameState>>();
    }

    // --- Lógica de la Máquina de Estados ---

    // Equivale a tryToStopTheseWhenSwitchToThis
    public void TryToStopStatesOnSwitch(GameState newState)
    {
        if (StopTheseWhenSwitchToThese.TryGetValue(newState, out List<GameState> statesToStop))
        {
            foreach (var gs in statesToStop)
            {
                GameManager.StopGameState(gs);
            }
        }
    }

    // Equivale a setGamestatesToStop
    // Usamos 'params' para replicar la función '...' de Lua
    public void SetGamestatesToStop(GameState gameStateToSwitchTo, params GameState[] statesToStop)
    {
        StopTheseWhenSwitchToThese[gameStateToSwitchTo] = statesToStop.ToList();
    }

    // Equivale a resizeLayersToCurrentWindowSize
    public virtual void ResizeLayersToCurrentWindowSize()
    {
        // Aquí iría la lógica para recrear los RenderTargets (Canvases)
    }

    // --- Bucle Principal (Update/Draw) ---

    // Equivale a update(dt)
    public void GenericUpdate(float dt)
    {
        // Actualiza todas las entidades de este estado
        EntityManager.Update(dt);

        // Llama al 'OnUpdate' de la clase hija (ej. MainMenuState)
        this.OnUpdate(dt);
    }

    // Equivale a draw()
    public void Draw(SpriteBatch spriteBatch)
    {
        // Dibuja todas las entidades (¡esto faltaba en tu GameState de Lua!)
        // Asumimos que quieres dibujar las entidades.
        foreach (var entity in EntityManager.GetEntities())
        {
            entity.Draw(spriteBatch);
        }

        // Llama al 'OnDraw' de la clase hija
        this.OnDraw(spriteBatch);
    }

    // --- Callbacks Virtuales (para que las clases hijas las implementen) ---

    // Equivale a onInit()
    public virtual void OnInIt() { }

    // Equivale a onEnter()
    public virtual void OnEnter() { }

    // Equivale a onUpdate(dt)
    public virtual void OnUpdate(float dt) { }

    // Equivale a onDraw()
    // Nota: Pasamos SpriteBatch para que sea útil
    public virtual void OnDraw(SpriteBatch spriteBatch) { }

    // Equivale a onExit()
    public virtual void OnExit() { }

    // Equivale a onStop(stoppedByGamesateSwitch)
    public virtual void OnStop(bool stoppedByGameStateSwitch = false) { }
}