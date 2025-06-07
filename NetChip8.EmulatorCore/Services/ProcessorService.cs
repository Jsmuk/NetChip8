using System.ComponentModel;
using System.Reflection.Emit;

using Microsoft.Extensions.Logging;

using NetChip8.Emulator.Shared;
using NetChip8.Emulator.Shared.Interfaces;

namespace NetChip8.EmulatorCore.Services;

internal class ProcessorService : IProcessorService
{
    private readonly IMemoryService _memory;
    private readonly IRegisterService _register;
    private readonly IFrameBufferService _frameBuffer;
    private readonly ILogger<ProcessorService> _logger;

    private ushort _programCounter;

    private ushort _indexRegister;
    private Stack<ushort> _stack;

    public ProcessorService(IMemoryService memory, IRegisterService register, ILogger<ProcessorService> logger, IFrameBufferService frameBuffer)
    {
        _memory = memory;
        _register = register;
        _logger = logger;
        _frameBuffer = frameBuffer;
        _indexRegister = 0;
        _programCounter = 0x200;
        _stack = new Stack<ushort>(16);

    }


    public void Cycle()
    {
        var nextInstruction = Fetch();
        var label = Decode(nextInstruction);
        Execute(label, nextInstruction);
    }

    private OpCode Fetch()
    {
        var nextOpCode = _memory.ReadShort(_programCounter);
        _programCounter += 2;

        OpCode opCode = new()
        {
            Op = nextOpCode,
            NNN = (ushort)(nextOpCode & 0x0FFF),
            NN = (byte)(nextOpCode & 0x00FF),
            N = (byte)(nextOpCode & 0x000F),
            X = (byte)((nextOpCode & 0x0F00) >> 8),
            Y = (byte)((nextOpCode & 0x00F0) >> 4)
        };
        return opCode;
    }

    public InstructionLabel Decode(OpCode opCode)
    {
        return (opCode.Op & 0xF000) switch
        {
            0x0000 when opCode.Op == 0x00E0 => InstructionLabel.Clr,
            0x0000 when opCode.Op == 0x00EE => InstructionLabel.Rts,
            0x0000 => InstructionLabel.Sys,
            0x1000 => InstructionLabel.Jump,
            0x2000 => InstructionLabel.Call,
            0x3000 => InstructionLabel.Ske,
            0x4000 => InstructionLabel.Skne,
            0x5000 => InstructionLabel.Skre,
            0x6000 => InstructionLabel.Load,
            0x7000 => InstructionLabel.Add,
            0x8000 when opCode.N == 0 => InstructionLabel.Move,
            0x8000 when opCode.N == 1 => InstructionLabel.Or,
            0x8000 when opCode.N == 2 => InstructionLabel.And,
            0x8000 when opCode.N == 3 => InstructionLabel.Xor,
            0x8000 when opCode.N == 4 => InstructionLabel.Addr,
            0x8000 when opCode.N == 5 => InstructionLabel.Sub,
            0x8000 when opCode.N == 6 => InstructionLabel.Shr,
            0x8000 when opCode.N == 7 => InstructionLabel.Subn,
            0x8000 when opCode.N == 0xE => InstructionLabel.Shl,
            0x9000 => InstructionLabel.Skrne,
            0xA000 => InstructionLabel.Loadi,
            0xB000 => InstructionLabel.Jumpi,
            0xC000 => InstructionLabel.Rand,
            0xD000 => InstructionLabel.Draw,
            0xE000 when opCode.NN == 0x9E => InstructionLabel.Skpr,
            0xE000 when opCode.NN == 0xA1 => InstructionLabel.Skup,
            0xF000 when opCode.NN == 0x07 => InstructionLabel.Moved,
            0xF000 when opCode.NN == 0x0A => InstructionLabel.Keyd,
            0xF000 when opCode.NN == 0x15 => InstructionLabel.Loadd,
            0xF000 when opCode.NN == 0x18 => InstructionLabel.Loads,
            0xF000 when opCode.NN == 0x1E => InstructionLabel.Addi,
            0xF000 when opCode.NN == 0x29 => InstructionLabel.Ldspr,
            0xF000 when opCode.NN == 0x33 => InstructionLabel.Bcd,
            0xF000 when opCode.NN == 0x55 => InstructionLabel.Stor,
            0xF000 when opCode.NN == 0x65 => InstructionLabel.Read,
            _ => throw new NotImplementedException()
        };
    }
    
    private void Execute(InstructionLabel label, OpCode opCode)
    {
        switch (label)
        {
            case InstructionLabel.Sys:
                // Intentionally not implemented 
                break;
            case InstructionLabel.Clr:
                break;
            case InstructionLabel.Rts:
                break;
            case InstructionLabel.Jump:
                _programCounter = opCode.NNN;
                break;
            case InstructionLabel.Call:
                break;
            case InstructionLabel.Ske:
                break;
            case InstructionLabel.Skne:
                break;
            case InstructionLabel.Skre:
                break;
            case InstructionLabel.Load:
                _register.SetRegister(opCode.X, opCode.NN);
                break;
            case InstructionLabel.Add:
                _register.AddRegister(opCode.X, opCode.NN);
                break;
            case InstructionLabel.Move:
                break;
            case InstructionLabel.Or:
                break;
            case InstructionLabel.And:
                break;
            case InstructionLabel.Xor:
                break;
            case InstructionLabel.Addr:
                break;
            case InstructionLabel.Sub:
                break;
            case InstructionLabel.Shr:
                break;
            case InstructionLabel.Subn:
                break;
            case InstructionLabel.Shl:
                break;
            case InstructionLabel.Skrne:
                break;
            case InstructionLabel.Loadi:
                _indexRegister = opCode.NNN;
                break;
            case InstructionLabel.Jumpi:
                break;
            case InstructionLabel.Rand:
                break;
            case InstructionLabel.Draw:
                Draw(opCode);
                break;
            case InstructionLabel.Skpr:
                break;
            case InstructionLabel.Skup:
                break;
            case InstructionLabel.Moved:
                break;
            case InstructionLabel.Keyd:
                break;
            case InstructionLabel.Loadd:
                break;
            case InstructionLabel.Loads:
                break;
            case InstructionLabel.Addi:
                break;
            case InstructionLabel.Ldspr:
                break;
            case InstructionLabel.Bcd:
                break;
            case InstructionLabel.Stor:
                break;
            case InstructionLabel.Read:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(label), label, null);
        }
    }

    private void Draw(OpCode opCode)
    {
        var startX = _register.ReadRegister(opCode.X);
        var startY = _register.ReadRegister(opCode.Y);

        _register.SetRegister(0xF, 0); // Collision flag reset 

        for (ushort rowNumber = 0; rowNumber < opCode.N; rowNumber++) // Row
        {
            var spriteByte = _memory.ReadByte((ushort)(_indexRegister + rowNumber));
            for (ushort columnNumber = 0; columnNumber < 8; columnNumber++)
            {
                var spritePixel = (spriteByte >> (7 - columnNumber)) & 1;
                if (spritePixel == 0) continue;
                
                var screenX = (startX + columnNumber) % 64;
                var screenY = (startY + rowNumber) % 32;


                var oldState = _frameBuffer.GetPixel(screenX, screenY);
                var newState = oldState ^ true;

                _frameBuffer.SetPixel(screenX, screenY, newState);

                if (oldState && !newState)
                {
                    _register.SetRegister(0xF, 1); // Collision 
                }

            }
        }
    }
}

