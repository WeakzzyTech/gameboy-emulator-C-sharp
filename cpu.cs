using System;

class CPU
{
    public ushort pc = 0x100;

    public bool Step(byte[] rom)
    {
        byte opcode = rom[pc];
        Console.WriteLine($"opcode: {opcode:X2}");
        pc++;
        return ExecuteOpcode(opcode, rom);
    }

    public byte regA = 0;
    public short Zflag = 0;

    private bool ExecuteOpcode(byte opcode, byte[] rom)
    {
        switch (opcode)
        {
            case 0x00:
                NOP();
                return true;

            case 0xC3:
                JPa16(rom);
                return true;

            case 0xFE:
                CPd8(rom);
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

    private void JPa16(byte[] rom)
    {
        Console.WriteLine("JPa16");
        byte low = rom[pc];
        byte high = rom[pc + 1];

        ushort address = (ushort)(low | (high << 8));

        pc = address;

    }

    private void CPd8(byte[] rom)
    {
        Console.WriteLine("CPd8");
        byte d8 = rom[pc];
        pc++;

        int result = regA - d8;
        if (result == 0)
        {
            Zflag = 1;
        }else
        {
            Zflag = 0;
        }
    }
}