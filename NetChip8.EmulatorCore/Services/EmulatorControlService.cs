using NetChip8.Emulator.Shared.Interfaces;

namespace NetChip8.EmulatorCore;

internal class EmulatorControlService : IEmulatorControlService
{
    private EmulatorState _state = EmulatorState.Stopped;
    
    private readonly IMemoryService _memory;
    private readonly IProcessorService _processor;
    private readonly IFrameBufferService _frameBuffer;
    private string _currentRom = "ibm-logo";

    public EmulatorControlService(IMemoryService memory, IProcessorService processor, IFrameBufferService frameBuffer)
    {
        _memory = memory;
        _processor = processor;
        _frameBuffer = frameBuffer;
    }


    public void Start()
    {
        switch (_state)
        {
            case EmulatorState.Running:
                return;
            case EmulatorState.Paused:
            case EmulatorState.Stopped:
                _state = EmulatorState.Running;
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Pause()
    {
        _state = EmulatorState.Paused;
    }

    public void Reset()
    {
        LoadRom(_currentRom);
        _state = EmulatorState.Running;
    }

    public void LoadRom(string romName)
    {
        _state = EmulatorState.Stopped;
        _currentRom = romName;
        _frameBuffer.ClearBuffer();
        _frameBuffer.ClearRedrawFlag();
        _memory.ClearMemory();
        _memory.LoadProgram(romName);
        _processor.Jump(0x200);
    }

    public EmulatorState GetState() => _state;
}