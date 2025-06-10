
namespace NetChip8.Emulator.Shared.Interfaces;

public interface IRegisterService
{
    public void ClearRegisters();

    public byte ReadRegister(byte register);
    public byte SetRegister(byte register, byte value);
    public byte Add(byte register, byte value);
    public bool ReadFlagRegister();
    public void SetFlagRegister(bool value);
    public void SetDelayTimer(byte value);
    public void DecrementDelayTimer();
    public byte ReadDelayTimer();
    public void SetSoundTimer(byte value);
    public void DecrementSoundtimer();
    public byte ReadSoundtimer();

    byte this[byte index]
    {
        get;
    }

}
