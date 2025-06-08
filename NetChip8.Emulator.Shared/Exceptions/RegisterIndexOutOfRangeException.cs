namespace NetChip8.Emulator.Shared.Exceptions;
public class RegisterIndexOutOfRangeException(byte value) : EmulatorException($"Register index {value} out of range");
