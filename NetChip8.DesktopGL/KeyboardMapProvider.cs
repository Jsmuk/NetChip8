using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework.Input;
using NetChip8.Emulator.Shared;

namespace NetChip8.DesktopGL;

public class KeyboardMapProvider
{
    private Dictionary<Keys, Chip8Key> _map = new Dictionary<Keys, Chip8Key>()
    {
        [Keys.X] = Chip8Key.D0, [Keys.D1] = Chip8Key.D1, [Keys.D2] = Chip8Key.D2, [Keys.D3] = Chip8Key.D3,
        [Keys.Q] = Chip8Key.D4, [Keys.W] = Chip8Key.D5, [Keys.E] = Chip8Key.D6, [Keys.A] = Chip8Key.D7,
        [Keys.S] = Chip8Key.D8, [Keys.D] = Chip8Key.D9, [Keys.Z] = Chip8Key.A, [Keys.C] = Chip8Key.B,
        [Keys.D4] = Chip8Key.C, [Keys.R] = Chip8Key.D, [Keys.F] = Chip8Key.E, [Keys.V] = Chip8Key.A, 
        
    };

    public Dictionary<Keys, Chip8Key> GetMap() => _map;

    public Keys GetKey(Chip8Key key)
    {
        return _map.First(x => x.Value == key).Key;
    }

    public Chip8Key GetKey(Keys key)
    {
        return _map[key];
    }
}