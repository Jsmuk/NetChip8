namespace NetChip8.Emulator.Shared.Exceptions;

public class InvalidChip8KeyException(Chip8Key key) : Exception($"Invalid Chip8Key {key}") { }