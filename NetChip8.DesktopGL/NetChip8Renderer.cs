using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using NetChip8.Emulator.Shared.Interfaces;

namespace NetChip8.DesktopGL;

public class NetChip8Renderer : Game, IGame
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _whiteRectangle;

    private readonly IFrameBufferService _framebufferService;
    public NetChip8Renderer(IFrameBufferService framebufferService)
    {
        _framebufferService = framebufferService;
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
        
        _graphics.ApplyChanges();

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

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
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
                
                
                _spriteBatch.Draw(_whiteRectangle, new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize), colour);
            }
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    public Game Game => this;
}
