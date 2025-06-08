# 🕹️ NetChip8

*aka Enterprise Chip-8*  

A CHIP-8 emulator implemented in **C# / .NET** with **MonoGame**, featuring completely unnecessary layers of abstraction and general over-engineering.

---

## 🧱 Project Structure

- 🎮 `NetChip8.DesktopGL` – MonoGame frontend  
- 🧠 `NetChip8.EmulatorCore` – Core CHIP-8 CPU/Emulator logic  
- 🗂️ `NetChip8.Emulator.Shared` – Shared interfaces, abstractions, and artifacts of software architecture cosplay

---

## ✅ Opcode Implementation Status

| Opcode | Label   | Mnemonic  | Status |
|--------|---------|-----------|--------|
| 0NNN   | Sys     | SYS addr  | 🚫     |
| 00E0   | Clr     | CLS       | ✅     |
| 00EE   | Rts     | RET       | ✅     |
| 1NNN   | Jump    | JP addr   | ✅     |
| 2NNN   | Call    | CALL addr | ✅     |
| 3XNN   | Ske     | SE Vx, byte | ✅   |
| 4XNN   | Skne    | SNE Vx, byte | ✅  |
| 5XY0   | Skre    | SE Vx, Vy | ✅     |
| 6XNN   | Load    | LD Vx, byte | ✅   |
| 7XNN   | Add     | ADD Vx, byte | ✅  |
| 8XY0   | Move    | LD Vx, Vy | ✅     |
| 8XY1   | Or      | OR Vx, Vy | ✅     |
| 8XY2   | And     | AND Vx, Vy | ✅    |
| 8XY3   | Xor     | XOR Vx, Vy | ✅    |
| 8XY4   | Addr    | ADD Vx, Vy | ✅    |
| 8XY5   | Sub     | SUB Vx, Vy | ✅    |
| 8XY6   | Shr     | SHR Vx {, Vy} | ✅  |
| 8XY7   | Subn    | SUBN Vx, Vy | ✅   |
| 8XYE   | Shl     | SHL Vx {, Vy} | ✅ |
| 9XY0   | Skrne   | SNE Vx, Vy | ✅    |
| ANNN   | Loadi   | LD I, addr | ✅    |
| BNNN   | Jumpi   | JP V0, addr | ✅   |
| CXNN   | Rand    | RND Vx, byte | ✅  |
| DXYN   | Draw    | DRW Vx, Vy, nibble | ✅ |
| EX9E   | Skpr    | SKP Vx    | ❌     |
| EXA1   | Skup    | SKNP Vx   | ❌     |
| FX07   | Moved   | LD Vx, DT | ❌     |
| FX0A   | Keyd    | LD Vx, K  | ❌     |
| FX15   | Loadd   | LD DT, Vx | ❌     |
| FX18   | Loads   | LD ST, Vx | ❌     |
| FX1E   | Addi    | ADD I, Vx | ✅     |
| FX29   | Ldspr   | LD F, Vx  | ✅     |
| FX33   | Bcd     | LD B, Vx  | ✅     |
| FX55   | Stor    | LD [I], Vx | ❌    |
| FX65   | Read    | LD Vx, [I] | ❌    |

- ✅ – Implemented  
- ❌ – Not yet implemented  
- 🚫 – Intentionally not implemented

---

Made with ~~🧡~~🧠 using **Rider**, **Visual Studio** and **ReSharper** *(because after 15+ years of writing C# I still can't decide which IDE I like)*