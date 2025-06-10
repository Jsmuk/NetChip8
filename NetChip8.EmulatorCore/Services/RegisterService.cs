using NetChip8.Emulator.Shared.Exceptions;
using NetChip8.Emulator.Shared.Interfaces;

namespace NetChip8.EmulatorCore.Services;
internal class RegisterService : IRegisterService
{
    private byte[] _variableRegister;
    private bool _flagRegister;
    private byte _delayRegister = 0;
    private byte _soundRegister = 0;

    public RegisterService()
    {
        _variableRegister = new byte[16];
    }


    public void ClearRegisters()
    {
        for (int i = 0; i < 16; i++)
        {
            _variableRegister[i] = 0;
        }
    }
    
    public byte ReadRegister(byte register)
    {
        AssertValidRegisterIndex(register);
        return _variableRegister[register];
    }

    public byte SetRegister(byte register, byte value)
    {
        AssertValidRegisterIndex(register);
        _variableRegister[register] = value;
        return value;
    }

    public byte Add(byte register, byte value)
    {
        AssertValidRegisterIndex(register);
        return _variableRegister[register] = (byte)(_variableRegister[register] + value);
    }
    

    public bool ReadFlagRegister() => _flagRegister;

    public void SetFlagRegister(bool value)
    {
        _flagRegister = value;
    }

    public void SetDelayTimer(byte value)
    {
        _delayRegister = value;
    }

    public void DecrementDelayTimer()
    {
        if (_delayRegister > 0)
        {
            _delayRegister--;
        }
    }

    public byte ReadDelayTimer() => _delayRegister;

    public void SetSoundTimer(byte value)
    {
        if (_soundRegister > 0)
        {
            _soundRegister = value;
        }
    }

    public void DecrementSoundtimer()
    {
        _soundRegister--;
    }

    public byte ReadSoundtimer() => _soundRegister;

    public byte this[byte index] => ReadRegister(index);

    private static void AssertValidRegisterIndex(byte register)
    {
        if (register >= 16)
        {
            throw new RegisterIndexOutOfRangeException(register);
        }
    }
}

