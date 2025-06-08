using System.ComponentModel;
using System.Reflection.Emit;

using Microsoft.Extensions.Logging;

using NetChip8.Emulator.Shared;
using NetChip8.Emulator.Shared.Exceptions;
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

    private const ushort FontStartAddress = 0x050;
    private const int FontSize = 5;

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
        byte xValue;
        byte yValue;
        bool overflow;
        switch (label)
        {
            case InstructionLabel.Sys:
                // Intentionally not implemented 
                break;
            case InstructionLabel.Clr:
                _frameBuffer.ClearBuffer();
                break;
            case InstructionLabel.Rts:
                var address = _stack.Pop();
                _programCounter = address;
                break;
            case InstructionLabel.Jump:
                _programCounter = opCode.NNN;
                break;
            case InstructionLabel.Call:
                _stack.Push(_programCounter);
                _programCounter = opCode.NNN;
                break;
            case InstructionLabel.Ske:
                if (_register[opCode.X] == opCode.NN)
                {
                    _programCounter += 2; // We have already advanced the PC by 2 during Fetch 
                }
                break;
            case InstructionLabel.Skne:
                if (_register[opCode.X] != opCode.NN)
                {
                    _programCounter += 2;
                }
                break;
            case InstructionLabel.Skre:
                if (_register[opCode.X] == _register[opCode.Y])
                {
                    _programCounter += 2;
                }
                break;
            case InstructionLabel.Load:
                _register.SetRegister(opCode.X, opCode.NN);
                break;
            case InstructionLabel.Add:
                _register.Add(opCode.X, opCode.NN);
                break;
            case InstructionLabel.Move:
                _register.SetRegister(opCode.X, _register[opCode.Y]);
                break;
            case InstructionLabel.Or:
                xValue = _register[opCode.X];
                yValue = _register[opCode.Y];
                _register.SetRegister(opCode.X, xValue |= yValue);
                break;
            case InstructionLabel.And:
                xValue = _register[opCode.X];
                yValue = _register[opCode.Y];
                _register.SetRegister(opCode.X, xValue &= yValue);
                break;
            case InstructionLabel.Xor:
                xValue = _register[opCode.X];
                yValue = _register[opCode.Y];
                _register.SetRegister(opCode.X, xValue ^= yValue);
                break;
            case InstructionLabel.Addr:
                overflow = (_register[opCode.X] + _register[opCode.Y]) > 0xFF;
                _register.SetFlagRegister(overflow);
                _register.Add(opCode.X, _register[opCode.Y]);
                break;
            case InstructionLabel.Sub:
                overflow = (_register[opCode.X] > _register[opCode.Y]);
                _register.SetFlagRegister(overflow);

                _register.SetRegister(opCode.X, (byte)(_register[opCode.X] - _register[opCode.Y]));
                break;
            case InstructionLabel.Shr:
                overflow = (_register[opCode.X] & 0x01) != 0;
                _register.SetFlagRegister(overflow);
                _register.SetRegister(opCode.X, (byte)(_register[opCode.X] >> 1));
                break;
            case InstructionLabel.Subn:
                overflow = (_register[opCode.Y] > _register[opCode.X]);
                _register.SetFlagRegister(overflow);
                _register.SetRegister(opCode.X, (byte)(_register[opCode.Y] - _register[opCode.X]));
                break;
            case InstructionLabel.Shl:
                overflow = (_register[opCode.X] & 0x80) != 0;
                _register.SetFlagRegister(overflow);
                _register.SetRegister(opCode.X, (byte)(_register[opCode.X] << 1));
                break;
            case InstructionLabel.Skrne:
                if (_register[opCode.X] != _register[opCode.Y])
                {
                    _programCounter += 2;
                }
                break;
            case InstructionLabel.Loadi:
                _indexRegister = opCode.NNN;
                break;
            case InstructionLabel.Jumpi:
                // TODO: Support other implementations of this as a quirk
                _programCounter = (ushort)(_register[0] + opCode.NNN);
                break;
            case InstructionLabel.Rand:
                _register.SetRegister(opCode.X, (byte)((ushort)Random.Shared.Next(255) & (ushort)opCode.NN));
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
                overflow = (_register[opCode.X] + _indexRegister) > 0xFF;
                _register.SetFlagRegister(overflow);
                _indexRegister += _register[opCode.X];
                break;
            case InstructionLabel.Ldspr:
                var digit = _register[opCode.X];
                if (digit > 0xF)
                {
                    throw new InvalidFontDigitException(digit);
                }
                _indexRegister = (ushort)(FontStartAddress + (digit * FontSize));
                break;
            case InstructionLabel.Bcd:
                BCD(opCode);
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

    private void BCD(OpCode opcode)
    {
        var value = _register[opcode.X];

        var hundreds = value / 100;
        var tens = (value / 10) % 10;
        var ones = value % 10;
        
        _memory.WriteByte(_indexRegister, (byte)hundreds);
        _memory.WriteByte((ushort)(_indexRegister + 1), (byte)tens);
        _memory.WriteByte((ushort)(_indexRegister + 2), (byte)ones);
    }
}

