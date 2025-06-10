using NetChip8.Emulator.Shared;

public interface IInputService
{
    public void Update(Chip8Key key, bool state);
    public bool IsKeyPressed(Chip8Key key);
    public Chip8Key? GetPressedKey();
    void ClearState();
    bool IsAnyKeyPressed();
}