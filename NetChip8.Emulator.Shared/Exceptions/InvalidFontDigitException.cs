namespace NetChip8.Emulator.Shared.Exceptions;

public class InvalidFontDigitException(int value) : EmulatorException($"Invalid digit {value} (above 0xF) ");