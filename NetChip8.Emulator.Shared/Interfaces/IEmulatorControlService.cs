namespace NetChip8.Emulator.Shared.Interfaces;

public interface IEmulatorControlService
{
    void Start();
    void Pause();
    void Reset();
    void LoadRom(string romName);
    EmulatorState GetState();
}

public enum EmulatorState
{
    Stopped,
    Running,
    Paused
}