using NetChip8.EmulatorCore;

namespace NetChip8.Emulator.Shared.Interfaces;

public interface IProcessorService
{
    void Cycle();
    InstructionLabel Decode(OpCode opCode);
}