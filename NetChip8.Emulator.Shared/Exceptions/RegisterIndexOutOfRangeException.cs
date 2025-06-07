namespace NetChip8.Emulator.Shared.Exceptions;
public class RegisterIndexOutOfRangeException(byte value) : Exception($"Register index {value} out of range");
