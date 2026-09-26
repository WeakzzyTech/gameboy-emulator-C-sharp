using System;

class CPU
{
    public ushort pc = 0x100;

    public int Step(Memory memory)
    {
        byte opcode = memory.ReadByte(pc);
        Console.WriteLine($"opcode: {opcode:X2}");
        pc++;
        return ExecuteOpcode(opcode, memory);
    }

    public byte regA = 0;
    public byte regB = 0;
    public byte regC = 0;
    public byte regH = 0;
    public byte regL = 0;
    public bool Zflag;
    public bool Nflag;
    public bool Hflag;
    public bool Cflag;
    public bool InterruptMasterEnable = true;
    public ushort SP = 0xFFFE;

    private int ExecuteOpcode(byte opcode, Memory memory)
    {
        switch (opcode)
        {
            case 0x00:
                NOP();
                return 4;

            case 0x05:
                DecB();
                return 4;

            case 0x06:
                LDBd8(memory);
                return 8;

            case 0x0D:
                DECC();
                return 4;

            case 0x0E:
                LDCd8(memory);
                return 8;

            case 0x18:
                JRs8(memory);
                return 12;

            case 0x20:
                return JRNZs8(memory);

            case 0x21:
                LDHLd16(memory);
                return 12;

            case 0x28:
                return JRZs8(memory);

            case 0x32:
                LDHLDecA(memory);
                return 8;

            case 0x3E:
                LDAd8(memory);
                return 8;

            case 0x47:
                LDBA();
                return 4;

            case 0xAF:
                XORA();
                return 4;

            case 0xC3:
                JPa16(memory);
                return 16;

            case 0xCD:
                CALLa16(memory);
                return 24;

            case 0xE0:
                LDa8A(memory);
                return 12;

            case 0xEA:
                LDa16A(memory);
                return 16;

            case 0xF0:
                LDAa8(memory);
                return 12;

            case 0xF3:
                DI();
                return 4;

            case 0xFE:
                CPd8(memory);
                return 8;

            default:
                Console.WriteLine($"Unknown opcode");
                return -1;
        }
    }

    private void NOP()
    {
        Console.WriteLine("NOP");
    }

    private void DecB()
    {
        Console.WriteLine("DecB");
        
        Hflag = (regB & 0x0F) == 0;

        regB--;
        
        Zflag = regB == 0;
        Nflag = true;
    }

    private void LDHLd16(Memory memory)
    {
        Console.WriteLine("LDHLd16");
        regL = memory.ReadByte(pc);
        regH  = memory.ReadByte((ushort)(pc + 1));
        pc += 2;
    }

    private void JRs8(Memory memory)
    {
        Console.WriteLine("JRs8");
        sbyte offset = (sbyte)memory.ReadByte(pc);
        pc++;
        pc  = (ushort)(pc +  offset);
    }

    private int JRZs8(Memory memory)
    {
        Console.WriteLine("JRZs8");
        sbyte offset = (sbyte)memory.ReadByte(pc);
        pc++;

        if (Zflag)
        {
            pc = (ushort)(pc + offset);
            return 12;
        }

        return 8;
    }

    private void CALLa16(Memory memory)
    {
        Console.WriteLine("CALLa16");
        byte low = memory.ReadByte(pc);
        byte high = memory.ReadByte((ushort)(pc + 1));
        ushort destAddress = (ushort)(low | (high << 8));
        pc = (ushort)(pc + 2);

        SP--;
        memory.WriteByte(SP, (byte)(pc >> 8));

        SP--;
        memory.WriteByte(SP, (byte)(pc & 0xFF));

        pc = destAddress;
    }

    private void DI()
    {
        Console.WriteLine("DI");
        InterruptMasterEnable = false;
    }

    private void LDAd8(Memory memory)
    {
        Console.WriteLine("LDAd8");
        byte d8 = memory.ReadByte(pc);
        pc++;

        regA = d8;
    }

    private void LDBA()
    {
        Console.WriteLine("LDBA");
        regB = regA;
    }

    private void LDa16A(Memory memory)
    {
        Console.WriteLine("LDa16A");
        byte low = memory.ReadByte(pc);
        byte high = memory.ReadByte((ushort)(pc + 1));
        ushort address = (ushort)(low | (high << 8));
        memory.WriteByte(address, regA);
        pc = (ushort)(pc + 2);
    }

    private void LDHLDecA(Memory memory)
    {
        Console.WriteLine("LDHLDecA");
        ushort HL = (ushort)((regH << 8) | regL);

        memory.WriteByte(HL, regA);
        HL = (ushort)(HL - 1);

        regH = (byte)(HL >> 8);
        regL = (byte)(HL & 0xFF);
    }

    private void LDa8A(Memory memory)
    {
        Console.WriteLine("LDa8A");
        byte a8 = memory.ReadByte(pc);
        pc++;

        ushort address = (ushort)(0xFF00 + a8);
        memory.WriteByte(address, regA);
    }

    private void LDAa8(Memory memory)
    {
        Console.WriteLine("LDAa8");
        byte a8 = memory.ReadByte(pc);
        pc++;

        ushort address = (ushort)(0xFF00 + a8);
        regA = memory.ReadByte(address);
    }

    private int JRNZs8(Memory memory)
    {
        Console.WriteLine("JRNZs8");
        sbyte offset = (sbyte)memory.ReadByte(pc);
        pc++;

        if (Zflag == false)
        {
            pc = (ushort)(pc + offset);
            return 12;
        }

        return 8;
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

        pc = (ushort)(low | (high << 8));
    }

    private void DECC()
    {
        Console.WriteLine("DECC");
        Hflag = (regC & 0x0F) == 0;
        regC--;
        Zflag = regC == 0;
        Nflag = true;
    }

    private void CPd8(Memory memory)
    {
        Console.WriteLine("CPd8");
        byte d8 = memory.ReadByte(pc);
        pc++;

        Zflag = regA == d8;
        Nflag = true;
        Hflag = (regA & 0x0F) < (d8 & 0x0F);
        Cflag = regA < d8;
    }

    private void LDBd8(Memory memory)
    {
        Console.WriteLine("LDBd8");
        byte d8 = memory.ReadByte(pc);
        pc++;

        regB = d8;
    }

    private void LDCd8(Memory memory)
    {
        Console.WriteLine("LDCd8");
        byte d8 = memory.ReadByte(pc);
        pc++;

        regC = d8;
    }
}