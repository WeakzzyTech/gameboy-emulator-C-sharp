using System;

class CPU
{
    public ushort pc = 0x100;

    public bool Step(Memory memory)
    {
        byte opcode = memory.ReadByte(pc);
        Console.WriteLine($"opcode: {opcode:X2}");
        pc++;
        return ExecuteOpcode(opcode, memory);
    }

    public byte regA = 0;
    public byte regH = 0;
    public byte regL = 0;
    public bool Zflag;
    public bool Nflag;
    public bool Hflag;
    public bool Cflag;

    private bool ExecuteOpcode(byte opcode, Memory memory)
    {
        switch (opcode)
        {
            case 0x00:
                NOP();
                return true;

            case 0x21:
                LDHLd16(memory);
                return true;

            case 0xAF:
                XORA();
                return true;

            case 0xC3:
                JPa16(memory);
                return true;

            case 0xFE:
                CPd8(memory);
                return true;

            default:
                Console.WriteLine($"Unknown opcode");
                return false;
        }
    }

    private void NOP()
    {
        Console.WriteLine("NOP");
    }

    private void LDHLd16(Memory memory)
    {
        Console.WriteLine("LDHLd16");
        regH = memory.ReadByte(pc);
        regL  = memory.ReadByte((ushort)(pc + 1));
    }

    private void XORA()
    {
        Console.WriteLine("XORA");
        regA ^= regA;

        Zflag = true;
        Nflag = false;
        Hflag = false;
        Cflag = false;
    }

    private void JPa16(Memory memory)
    {
        Console.WriteLine("JPa16");
        byte low = memory.ReadByte(pc);
        byte high = memory.ReadByte((ushort)(pc + 1));

        ushort address = (ushort)(low | (high << 8));

        pc = address;

    }

    private void CPd8(Memory memory)
    {
        Console.WriteLine("CPd8");
        byte d8 = memory.ReadByte(pc);
        pc++;

        int result = regA - d8;
        if (result == 0)
        {
            Zflag = true;
        }else
        {
            Zflag = false;
        }
    }
}