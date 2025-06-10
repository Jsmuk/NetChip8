using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using NetChip8.Emulator.Shared.Interfaces;

namespace NetChip8.DesktopGL.GameStates;

public class EmulatorGameState : IGameState
{

    private readonly IFrameBufferService _frameBuffer;
    private readonly IInputService _input;
    private readonly IEmulatorControlService _emulatorControlService;
    private readonly IProcessorService _processor;
    private readonly KeyboardMapProvider _keyboardMapProvider;
    private IGame? _game;
    
    private Texture2D? _whiteRectangle;
    
    private const int InstructionsPerTick = 10; // TODO: Config 

    public EmulatorGameState(IFrameBufferService frameBuffer, IInputService input, IEmulatorControlService emulatorControlService, IProcessorService processor, KeyboardMapProvider keyboardMapProvider)
    {
        _frameBuffer = frameBuffer;
        _input = input;
        _emulatorControlService = emulatorControlService;
        _processor = processor;
        _keyboardMapProvider = keyboardMapProvider;
    }

    private bool _contentLoaded;
    public string Name => "Emulator";
    public void Initialize(IGame game)
    {
        _game = game;
    }

    public void LoadContent(ContentManager content)
    {
        if (_game is null)
        {
            throw new NotImplementedException();
        }
        _whiteRectangle = new Texture2D(_game.Game.GraphicsDevice, 1, 1);
        _whiteRectangle.SetData(new[] { Color.White });

        _contentLoaded = true;
    }

    public void UnloadContent()
    {
    }

    public void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.F5))
        {
            _emulatorControlService.Reset();
        }
        
        if (_emulatorControlService.GetState() != EmulatorState.Running)
        {
            return;
        }
        
        _input.ClearState();
        
        _processor.TickTimers();
        
        foreach (var kvp in _keyboardMapProvider.GetMap().Where(kvp => keyboardState.IsKeyDown(kvp.Key)))
        {
            _input.Update(kvp.Value, true);
        }
        
        for (var i = 0; i < InstructionsPerTick; i++)
        {
            _processor.Cycle();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_game is null)
        {
            throw new NotImplementedException();
        }
        if (!_contentLoaded)
        {
            LoadContent(_game.Game.Content);
        }
        /*
        if (!_framebufferService.RedrawNeeded)
        {
            return;
        }
        */

        _game.Game.GraphicsDevice.Clear(Color.SlateGray);

        var tileSize = 12; // Size of one pixel
        var chip8Width = 64 * tileSize;
        var chip8Height = 32 * tileSize;

        var windowWidth = _game.Game.GraphicsDevice.Viewport.Width;
        var windowHeight = _game.Game.GraphicsDevice.Viewport.Height;

        var offsetX = (windowWidth - chip8Width) / 2;
        var offsetY = (windowHeight - chip8Height) / 2;


        
        spriteBatch.Begin();

        for (var y = 0; y < 32; ++y)
        {
            for (var x = 0; x < 64; ++x)
            {
                var pixel = _frameBuffer.GetPixel(x, y);
                var colour = pixel ? Color.White : Color.Black;

                if (pixel)
                {
                    Console.WriteLine(pixel);
                }

                var position = new Vector2(
                    offsetX + x * tileSize,
                    offsetY + y * tileSize
                );


                spriteBatch.Draw(_whiteRectangle,
                    new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize), colour);
            }
        }

        spriteBatch.End();
            
        _frameBuffer.ClearRedrawFlag();
    }
}