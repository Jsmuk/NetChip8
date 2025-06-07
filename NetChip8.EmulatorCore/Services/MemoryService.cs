using Microsoft.Extensions.Logging;

using NetChip8.Emulator.Shared.Interfaces;

namespace NetChip8.EmulatorCore.Services;

internal class MemoryService : IMemoryService
{
    private readonly ILogger<MemoryService> _logger;
    
    private byte[] _memory = new byte[4096];

    public MemoryService(ILogger<MemoryService> logger)
    {
        _logger = logger;
    }

    public byte ReadByte(ushort address) => _memory[address];

    public ushort ReadShort(ushort address) => (ushort)((_memory[address] << 8) | _memory[address + 1]);
    
    public void WriteByte(ushort address, byte value) => _memory[address] = value;

    public void WriteShort(ushort address, ushort value)
    {
        _memory[address] = (byte)(value >> 8);
        _memory[address + 1] = (byte)(value & 0xFF);
    }

    public void ClearMemory()
    {
        _memory = new byte[4096];
    }

    public bool LoadProgram(string romName)
    {
        var path = Path.Combine("ROMs", $"{romName}.c8");
        if (!File.Exists(path))
        {
            _logger.LogError("Failed to load ROM {RomName}, file does not exist at {Path}", romName, path);
            return false;
        }

        using var fs = new FileStream(path, FileMode.Open);
        for (var i = 0; i < fs.Length; i++)
        {
            _memory[i + 512] = (byte)fs.ReadByte();
        }
        _logger.LogInformation("Loaded {RomName} from {Path}", romName, path);

        return true;
    }
}
