using NetChip8.EmulatorCore;

namespace NetChip8.Emulator.Shared.Interfaces;

public interface IProcessorService
{
    void Cycle();
    void Jump(ushort address);
    InstructionLabel Decode(OpCode opCode);
    void TickTimers();
}