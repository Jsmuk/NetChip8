
namespace NetChip8.Emulator.Shared.Interfaces;

public interface IRegisterService
{
    public void ClearRegisters();

    public byte ReadRegister(byte register);
    public byte SetRegister(byte register, byte value);
    public byte Add(byte register, byte value);
    public bool ReadFlagRegister();
    public void SetFlagRegister(bool value);

    byte this[byte index]
    {
        get;
    }

}
