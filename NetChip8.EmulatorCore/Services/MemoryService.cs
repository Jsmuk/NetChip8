using Microsoft.Extensions.Logging;

using NetChip8.Emulator.Shared.Exceptions;
using NetChip8.Emulator.Shared.Interfaces;

namespace NetChip8.EmulatorCore.Services;

internal class MemoryService : IMemoryService
{
    private readonly ILogger<MemoryService> _logger;
    
    private byte[] _memory = new byte[4096];

    public MemoryService(ILogger<MemoryService> logger)
    {
        _logger = logger;
        LoadFont();
    }

    public byte ReadByte(ushort address)
    {
        AssertAddressWithinRangeShort(address);
        return _memory[address];
    }

    public ushort ReadShort(ushort address)
    {
        AssertAddressWithinRangeShort(address);
        return (ushort)((_memory[address] << 8) | _memory[address + 1]);
    }

    public void WriteByte(ushort address, byte value)
    {
        AssertAddressWithinRangeShort(address);
        _memory[address] = value;
    }

    public void WriteShort(ushort address, ushort value)
    {
        AssertAddressWithinRangeShort(address);
        _memory[address] = (byte)(value >> 8);
        _memory[address + 1] = (byte)(value & 0xFF);
    }

    public void ClearMemory()
    {
        Array.Clear(_memory);
        LoadFont();
    }

    public bool LoadProgram(string romName)
    {
        var path = Path.Combine("ROMs", $"{romName}.ch8");
        if (!File.Exists(path))
        {
            _logger.LogError("Failed to load ROM {RomName}, file does not exist at {Path}", romName, path);
            return false;
        }
        
        using var fs = new FileStream(path, FileMode.Open);

        if (fs.Length >= _memory.Length - 512)
        {
            _logger.LogError("ROM {RomName} is too large to load into memory. ROM size: {RomSize}, Available Memory: {AvailableMemory}", romName, fs.Length, _memory.Length - 512);
            return false;
        }
        
        for (var i = 0; i < fs.Length; i++)
        {
            _memory[i + 512] = (byte)fs.ReadByte();
        }
        _logger.LogInformation("Loaded {RomName} from {Path}", romName, path);

        return true;
    }

    private void LoadFont()
    {
        byte[] fontSet =
        [
            0xF0, 0x90, 0x90, 0x90, 0xF0, //0
            0x20, 0x60, 0x20, 0x20, 0x70, //1
            0xF0, 0x10, 0xF0, 0x80, 0xF0, //2
            0xF0, 0x10, 0xF0, 0x10, 0xF0, //3
            0x90, 0x90, 0xF0, 0x10, 0x10, //4
            0xF0, 0x80, 0xF0, 0x10, 0xF0, //5
            0xF0, 0x80, 0xF0, 0x90, 0xF0, //6
            0xF0, 0x10, 0x20, 0x40, 0x40, //7
            0xF0, 0x90, 0xF0, 0x90, 0xF0, //8
            0xF0, 0x90, 0xF0, 0x10, 0xF0, //9
            0xF0, 0x90, 0xF0, 0x90, 0x90, //A
            0xE0, 0x90, 0xE0, 0x90, 0xE0, //B
            0xF0, 0x80, 0x80, 0x80, 0xF0, //C
            0xE0, 0x90, 0x90, 0x90, 0xE0, //D
            0xF0, 0x80, 0xF0, 0x80, 0xF0, //E
            0xF0, 0x80, 0xF0, 0x80, 0x80  //F
        ];

        Array.Copy(fontSet,0, _memory, 0x50, 80);
    }

    private void AssertAddressWithinRange(ushort address)
    {
        if (address >= _memory.Length)
        {
            throw new MemoryAccessViolationException(address);
        } 
    }

    private void AssertAddressWithinRangeShort(ushort address)
    {
        if (address >= _memory.Length -1)
        {
            throw new MemoryAccessViolationException(address);
        }
    }
}
