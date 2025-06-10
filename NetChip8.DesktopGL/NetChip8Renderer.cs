using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using NetChip8.DesktopGL.GameStates;
using NetChip8.Emulator.Shared.Interfaces;
using NetChip8.EmulatorCore.Services;

namespace NetChip8.DesktopGL;

public class NetChip8Renderer : Game, IGame
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;

    private readonly IEmulatorControlService _emulatorControlService;
    
    private readonly IGameStateManager _gameStateManager;
    
    private readonly KeyboardMapProvider _keyboardMapProvider;
    
    public NetChip8Renderer(KeyboardMapProvider keyboardMapProvider, IEmulatorControlService emulatorControlService, IGameStateManager gameStateManager, EmulatorGameState emulatorGameState)
    {
        _keyboardMapProvider = keyboardMapProvider;
        _emulatorControlService = emulatorControlService;
        _gameStateManager = gameStateManager;
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        _gameStateManager.RegisterGameState(emulatorGameState, this);
        _gameStateManager.SetCurrentGameState("Emulator");
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferHeight = 480;
        _graphics.PreferredBackBufferWidth = 960;

        Window.Title = "NetChip8";

        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);

        _graphics.SynchronizeWithVerticalRetrace = true;
        
        _graphics.ApplyChanges();
        
        _emulatorControlService.LoadRom("tetris");
        _emulatorControlService.Start();
        
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        base.Initialize();
    }

    protected override void LoadContent()
    {

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        var currentGameState = _gameStateManager.CurrentGameState;
        if (currentGameState is null)
        {
            return;
        }
        
        currentGameState.Update(gameTime);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        var currentGameState = _gameStateManager.CurrentGameState;
        if (currentGameState is null || _spriteBatch is null)
        {
            // Blank the screen and do nothing
            GraphicsDevice.Clear(Color.Violet);
            return; 
        }
        
        currentGameState.Draw(_spriteBatch);
            
        base.Draw(gameTime);

    }

    public Game Game => this;
}
