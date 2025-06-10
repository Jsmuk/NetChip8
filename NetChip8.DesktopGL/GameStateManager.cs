using System.Collections.Generic;
using System.Linq;

using NetChip8.DesktopGL.Exceptions;
using NetChip8.DesktopGL.GameStates;

namespace NetChip8.DesktopGL;

public class GameStateManager : IGameStateManager
{
    private List<IGameState> _gameStates = new List<IGameState>();
    public void RegisterGameState(IGameState gameState, IGame game)
    {
        gameState.Initialize(game);
        _gameStates.Add(gameState);
    }

    public void SetCurrentGameState(string gameStateName)
    {
        if (_gameStates.All(x => x.Name != gameStateName))
        {
            throw new GameStateNotFoundException(gameStateName);
        }
        
        CurrentGameState = _gameStates.First(x => x.Name == gameStateName);
    }


    public IGameState? CurrentGameState { get; private set; }
}

public interface IGameStateManager
{
    void RegisterGameState(IGameState gameState, IGame game);
    void SetCurrentGameState(string gameStateName);
    IGameState? CurrentGameState { get; }
}