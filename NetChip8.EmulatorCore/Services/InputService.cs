using NetChip8.Emulator.Shared;
using NetChip8.Emulator.Shared.Exceptions;

namespace NetChip8.EmulatorCore.Services;

public class InputService : IInputService
{
    private Dictionary<Chip8Key, bool> _keyStates = new();

    public InputService()
    {
        ClearState();
    }
    
    public void ClearState()
    {
        _keyStates = new Dictionary<Chip8Key, bool>
        {
            [Chip8Key.D0] = false,
            [Chip8Key.D1] = false,
            [Chip8Key.D2] = false,
            [Chip8Key.D3] = false,
            [Chip8Key.D4] = false,
            [Chip8Key.D5] = false,
            [Chip8Key.D6] = false,
            [Chip8Key.D7] = false,
            [Chip8Key.D8] = false,
            [Chip8Key.D9] = false,
            [Chip8Key.A] = false,
            [Chip8Key.B] = false,
            [Chip8Key.C] = false,
            [Chip8Key.D] = false,
            [Chip8Key.E] = false,
            [Chip8Key.F] = false
        };
    }
    
    public void Update(Chip8Key key, bool state)
    {
        AssertValidKey(key);
        _keyStates[key] = state;
    }

    public bool IsKeyPressed(Chip8Key key) => _keyStates[key];
    
    public bool IsAnyKeyPressed() => _keyStates.Any(x => x.Value);

    public Chip8Key? GetPressedKey()
    {
        var result = _keyStates.FirstOrDefault(x => x.Value);
        return result.Value ? result.Key : null;
    }

    private void AssertValidKey(Chip8Key key)
    {
        if (!_keyStates.ContainsKey(key))
        {
            throw new InvalidChip8KeyException(key);
        }
    }
}