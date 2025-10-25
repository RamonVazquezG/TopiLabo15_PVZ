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

    public float GameTime = 0f;
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

        ScreenManager.Initialize(_graphics, GraphicsDevice);
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
        
        dt = MathF.Min(dt, 1f/30f); // Limita dt a un máximo de 30 FPS para evitar problemas en caso de lag severo (Se puede conseguir facilmente agarrando la ventana por mucho tiempo).

        GameTime += dt;

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

        // 1. "Empezar" el renderizado a la pantalla virtual
        ScreenManager.BeginRender();

        // 2. Dibujar TODO tu juego (con SpriteBatch)
        // Usa PointClamp aquí también para sprites individuales si quieres
        _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

        GameManager.Draw(_spriteBatch); // Dibuja entidades

        // Ejemplo de texto (se dibujará en la resolución virtual)
        // _spriteBatch.DrawString(miFuente, "Hola", new Vector2(10, 10), Color.White);

        _spriteBatch.End();

        // 3. "Terminar" el renderizado y estirar a la ventana
        ScreenManager.EndRender(_spriteBatch);

        base.Draw(gameTime);
    }
}