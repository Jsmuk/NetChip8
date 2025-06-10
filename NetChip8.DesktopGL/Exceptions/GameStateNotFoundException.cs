using System;

namespace NetChip8.DesktopGL.Exceptions;

public class GameStateNotFoundException(string gameStateName) : Exception($"GameState {gameStateName} not found") { }