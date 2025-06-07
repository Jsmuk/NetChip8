namespace NetChip8.EmulatorCore;

public record OpCode
{
    /// <summary>
    /// OpCode
    /// </summary>
    public ushort Op { get; init; }
    
    /// <summary>
    /// Second Nipple (Register lookup)
    /// </summary>
    public byte X { get; init; }

    /// <summary>
    /// Third nibble (Register lookup)
    /// </summary>
    public byte Y { get; init; }

    /// <summary>
    /// Fourth Nibble (4 bit number)
    /// </summary>
    public byte N { get; init; }

    /// <summary>
    /// Second Byte (8-bit immediate number)
    /// </summary>
    public byte NN { get; init; }

    /// <summary>
    /// Last 3 nibbles (12 bit immediate memory address)
    /// </summary>
    public ushort NNN { get; init; }

    public override string ToString()
    {
        return $"{Op:X4} (X: {X:X}, Y: {Y:X}, N: {N:X}, NN: {NN:X2}, NNN: {NNN:X3})";
    }
}