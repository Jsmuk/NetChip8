namespace NetChip8.Emulator.Shared;

public enum Chip8Key
{
    D0, D1, D2, D3,
    D4, D5, D6, D7,
    D8, D9, A, B, 
    C, D, E, F,
    
}

public static class Chip8KeyExtensions
{
    public static byte ToByte(this Chip8Key key)
    {
        return key switch
        {
            Chip8Key.D0 => 0x00,
            Chip8Key.D1 => 0x01,
            Chip8Key.D2 => 0x02,
            Chip8Key.D3 => 0x03,
            Chip8Key.D4 => 0x04,
            Chip8Key.D5 => 0x05,
            Chip8Key.D6 => 0x06,
            Chip8Key.D7 => 0x07,
            Chip8Key.D8 => 0x08,
            Chip8Key.D9 => 0x09,
            Chip8Key.A => 0x0A,
            Chip8Key.B => 0x0B,
            Chip8Key.C => 0x0C,
            Chip8Key.D => 0x0D,
            Chip8Key.E => 0x0E,
            Chip8Key.F => 0x0F,
            _ => throw new ArgumentOutOfRangeException(nameof(key), key, null)
        };
    }
}