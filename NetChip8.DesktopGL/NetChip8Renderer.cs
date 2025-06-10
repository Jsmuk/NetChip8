using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using NetChip8.Emulator.Shared.Interfaces;
using NetChip8.EmulatorCore.Services;

namespace NetChip8.DesktopGL;

public class NetChip8Renderer : Game, IGame
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _whiteRectangle;
    
    private const int InstructionsPerTick = 10; // TODO: Move to config 

    private readonly IFrameBufferService _framebufferService;
    private readonly IProcessorService _processor;
    private readonly IMemoryService _memory;
    private readonly IInputService _input;
    
    private readonly KeyboardMapProvider _keyboardMapProvider;
    public NetChip8Renderer(IFrameBufferService framebufferService, IProcessorService processor, IMemoryService memory, KeyboardMapProvider keyboardMapProvider, IInputService input)
    {
        _framebufferService = framebufferService;
        _processor = processor;
        _memory = memory;
        _keyboardMapProvider = keyboardMapProvider;
        _input = input;
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        _graphics.PreferredBackBufferHeight = 480;
        _graphics.PreferredBackBufferWidth = 960;

        Window.Title = "NetChip8";

        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);

        _graphics.SynchronizeWithVerticalRetrace = true;
        
        _graphics.ApplyChanges();

        _memory.LoadProgram("tetris");

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _whiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
        _whiteRectangle.SetData(new[] { Color.White });
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        var keyboardState = Keyboard.GetState();
        _input.ClearState();
        
        _processor.TickTimers();
        
        foreach (var kvp in _keyboardMapProvider.GetMap())
        {
            if (keyboardState.IsKeyDown(kvp.Key))
            {
                _input.Update(kvp.Value, true);
            }
        }
        
        for (var i = 0; i < InstructionsPerTick; i++)
        {
            _processor.Cycle();
        }
        

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        /*
        if (!_framebufferService.RedrawNeeded)
        {
            return;
        }
        */

        GraphicsDevice.Clear(Color.SlateGray);

        var tileSize = 12; // Size of one pixel
        var chip8Width = 64 * tileSize;
        var chip8Height = 32 * tileSize;

        var windowWidth = GraphicsDevice.Viewport.Width;
        var windowHeight = GraphicsDevice.Viewport.Height;

        var offsetX = (windowWidth - chip8Width) / 2;
        var offsetY = (windowHeight - chip8Height) / 2;



        _spriteBatch.Begin();

        for (var y = 0; y < 32; ++y)
        {
            for (var x = 0; x < 64; ++x)
            {
                var pixel = _framebufferService.GetPixel(x, y);
                var colour = pixel ? Color.White : Color.Black;


                var position = new Vector2(
                    offsetX + x * tileSize,
                    offsetY + y * tileSize
                );


                _spriteBatch.Draw(_whiteRectangle,
                    new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize), colour);
            }
        }

        _spriteBatch.End();
            
        _framebufferService.ClearRedrawFlag();
            
        base.Draw(gameTime);

    }

    public Game Game => this;
}
