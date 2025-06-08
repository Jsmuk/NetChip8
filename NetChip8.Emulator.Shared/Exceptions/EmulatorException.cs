
namespace NetChip8.Emulator.Shared.Exceptions;
public class EmulatorException : Exception
{
    public EmulatorException(string message) : base(message) { }
    public EmulatorException(string message, Exception innerException) : base(message, innerException) { }
}
