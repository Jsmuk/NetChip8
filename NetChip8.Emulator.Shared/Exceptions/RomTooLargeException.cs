using NetChip8.Emulator.Shared.Exceptions;

internal class RomTooLargeException(string name, int size) : EmulatorException($"Rom {name} is too large, size is {size}")
{
    public string Name { get; } = name;
    public int Size { get; } = size;
}
