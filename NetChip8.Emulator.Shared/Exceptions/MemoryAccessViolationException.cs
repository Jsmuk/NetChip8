namespace NetChip8.Emulator.Shared.Exceptions;
public class MemoryAccessViolationException(ushort address) : EmulatorException($"Memory access violation at {address:X4}")
{
    public ushort Address { get; } = address;
}
