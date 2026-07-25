using System;

class Memory
{
    private byte[] rom;
    private byte[] ram = new byte[0x2000];
    private byte[] vram = new byte[0x2000];
    private byte[] eram = new byte[0x2000];
    private byte[] oam = new byte[0xA0];
    private byte[] io = new byte[0x80];
    private byte[] hhram = new byte{0x7F};

    private byte interruptEnable;

    public Memory(byte[] rom)
    {
        this.rom = rom;
    }

    public byte ReadByte(ushort address)
    {
        if (address <= 0x7FFF)
            return rom[address];

        if (address >= 0x8000 && address <= 0x9FFF)
            return vram[address - 0x8000];

        if (address >= 0xA000 && address <= 0xBFFF)
            return eram[address - 0xA000];

        if (address >= 0xC000 && address <= 0xDFFF)
            return ram[address - 0xC000];

        if (address >= 0xE000 && address <= 0xFDFF)
            return ram[address - 0xE000];

        if (address >= 0xFE00 && address <= 0xFE9F)
            return oam[address - 0xFE00];

        if (address >= 0xFEA0 && address <= 0xFEFF)
            return 0xFF;

        if (address >= 0xFF00 && address <= 0xFF7F)
            return io[address - 0xFF00];

        if (address >= 0xFF80 && address <= 0xFFFE)
            return hram[address - 0xFF80];

        if (address == 0xFFFF)
            return interruptEnable;

        return 0xFF; //placeholder
    }

    public void  WriteByte(ushort address, byte value)
    {
        if (address >= 0x8000 && address <= 0x9FFF)
        {
            vram[address - 0x8000] = value;
            return;
        }

        if (address >= 0xA000 && address <= 0xBFFF)
        {
            eram[address - 0xA000] = value;
            return;
        }

        if (address >= 0xC000 && address  <=  0xDFFF)
        {
            ram[address - 0xC000] = value;
            return;
        }

        if (address >= 0xE000 && address <= 0xFDFF)
        {
            ram[address - 0xE000] = value;
            return;
        }

        if (address >= 0xFE00 && address <= 0xFE9F)
        {
            oam[address - 0xFE00] = value;
            return;
        }

        if (address >= 0xFEA0 && address <= 0xFEFF)
        {
            return;
        }

        if (address >= 0xFF00 && address <= 0xFF7F)
        {
            io[address - 0xFF00] = value;
            return;
        }

        if (address >= 0xFF80 && address <= 0xFFFE)
        {
            hram[address - 0xFF80] = value;
            return;
        }

        if (address == 0xFFFF)
        {
            interruptEnable = value;
            return;
        }
    }
}