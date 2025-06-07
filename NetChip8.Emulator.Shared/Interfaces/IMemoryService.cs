namespace NetChip8.Emulator.Shared.Interfaces;

public interface IMemoryService
{
    byte ReadByte(ushort address);
    ushort ReadShort(ushort address);
    void WriteByte(ushort address, byte value);
    void WriteShort(ushort address, ushort value);
    void ClearMemory();
    bool LoadProgram(string romName);
}