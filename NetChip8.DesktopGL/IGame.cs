using System;

using Microsoft.Xna.Framework;

namespace NetChip8.DesktopGL;
internal interface IGame : IDisposable
{
    public Game Game { get; }

    void Run();
}
