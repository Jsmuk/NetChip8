namespace NetChip8.Emulator.Shared.Interfaces;
public interface IFrameBufferService
{
    public void ClearBuffer();
    public void SetPixel(int x, int y, bool value);
    public bool GetPixel(int x, int y);
    public bool RedrawNeeded { get; }
    public void ClearRedrawFlag();
    void SetRedrawFlag();
}
