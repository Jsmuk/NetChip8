using NetChip8.Emulator.Shared.Interfaces;

namespace NetChip8.EmulatorCore.Services;
internal class FrameBufferService : IFrameBufferService
{
    private const int Width = 64;
    private const int Height = 32;
    private bool[,] _buffer = new bool[Height, Width];
    public void ClearBuffer()
    {
        _buffer = new bool[Height, Width];
    }

    public void SetPixel(int x, int y, bool value)
    {
        _buffer[y, x] = value;
    }

    public bool GetPixel(int x, int y) => _buffer[y, x];
}
