// En tu clase Game1.cs

// ...

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using TopiLabo15_PVZ.Data.GameStates;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Instancias de tus estados de juego
    // (Asumiendo que has creado estas clases que heredan de GameState)
    private GameState _mainMenuState;
    private GameState _playingState;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Debug.WriteLine("aaaaaaa");

        // Crea las instancias de los estados
        //_mainMenuState = new MainMenuState(); // Asumiendo que esta clase existe
        _playingState = new PlayingGameState(); // Asumiendo que esta clase existe

        // Registra los estados en el GameManager
        //GameManager.RegisterNewGameState(_mainMenuState);
        GameManager.RegisterNewGameState(_playingState);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Carga las animaciones (¡importante!)
        AnimationData.LoadContent(Content);

        Debug.WriteLine("PAPUUUUU");

        // ¡Inicia el juego cambiando al primer estado!
        GameManager.SwitchGameState(_playingState);
    }

    protected override void Update(GameTime gameTime)
    {
        // 1. Calcular dt.
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds; //dt es cuanto tiempo ha pasado desde el último frame. Es importante para que el juego corra igual de rápido en computadoras rápidas y lentas.
        
        // 2. Actualiza el input antes que el juego en si.
        // Esto "prepara" todas las propiedades (IsPressed, IsHeld) para este frame.
        MouseInput.Update();


        // 3. Actualiza tu GameManager (el juego en si).
        // Ahora, cualquier estado de juego o entidad que se actualice puede checar de forma segura las propiedades de MouseInput.
        GameManager.Update(dt);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // Configura el SpriteBatch (ej. PointClamp para pixel art)
        _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

        // El GameManager se encarga de dibujar el estado correcto
        GameManager.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}