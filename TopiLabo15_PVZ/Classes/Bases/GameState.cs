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
        this.PreUpdateCallback(dt);

        EntityManager.Update(dt);

        this.PostUpdateCallback(dt);
    }

    // Equivale a draw()
    public void Draw(SpriteBatch spriteBatch)
    {
        this.PreDrawCallback(spriteBatch);

        this.OnDraw(spriteBatch);

        // Llama al 'OnDraw' de la clase hija
        this.PostDrawCallback(spriteBatch);
    }

    // --- Callbacks Virtuales (para que las clases hijas las implementen) ---

    // Equivale a onInit()
    public virtual void OnInIt() { }

    // Equivale a onEnter()
    public virtual void OnEnter() { }

    // Equivale a onUpdate(dt)
    public virtual void PreUpdateCallback(float dt) { }
    public virtual void PostUpdateCallback(float dt) { }

    // Equivale a onDraw()
    // Nota: Pasamos SpriteBatch para que sea útil
    public virtual void PreDrawCallback(SpriteBatch spriteBatch) { }
    public virtual void OnDraw(SpriteBatch spriteBatch) { // Simplemente dibuja a todas las entidades.
        foreach (var entity in EntityManager.GetEntities())
        {
            entity.Draw(spriteBatch);
        }
    }
    public virtual void PostDrawCallback(SpriteBatch spriteBatch) { }

    // Equivale a onExit()
    public virtual void OnExit() { }

    // Equivale a onStop(stoppedByGamesateSwitch)
    public virtual void OnStop(bool stoppedByGameStateSwitch = false) { }
}