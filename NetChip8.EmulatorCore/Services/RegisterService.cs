using NetChip8.Emulator.Shared.Exceptions;
using NetChip8.Emulator.Shared.Interfaces;

namespace NetChip8.EmulatorCore.Services;
internal class RegisterService : IRegisterService
{
    private byte[] _variableRegister;

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
    

    public bool ReadFlagRegister()
    {
        throw new NotImplementedException();
    }

    public void SetFlagRegister(bool value)
    {
        throw new NotImplementedException();
    }

    public byte this[byte index] => ReadRegister(index);

    private static void AssertValidRegisterIndex(byte register)
    {
        if (register >= 16)
        {
            throw new RegisterIndexOutOfRangeException(register);
        }
    }
}

