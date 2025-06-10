using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NetChip8.DesktopGL.GameStates;

public interface IGameState
{
    string Name { get; }
    void Initialize(IGame game);
    void LoadContent(ContentManager content);
    void UnloadContent();
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
    
}