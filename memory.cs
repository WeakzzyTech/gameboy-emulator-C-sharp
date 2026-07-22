using System;

class Memory
{
    private byte[] rom;
    private byte[] ram = new byte[0x2000];

    public Memory(byte[] rom)
    {
        this.rom = rom;
    }

    public byte ReadByte(ushort address)
    {
        if (address <= 0x7FFF)
            return rom[address];

        if (address >= 0xC000 && address <=  0xDFFF)
            return ram[address - 0xC000];

        return 0xFF; //placeholder, bc I didn't implement VRAM yet
    }

    public void  WriteByte(ushort address, byte value)
    {
        if (address >= 0xC000 && address  <=  0xDFFF)
        {
            ram[address - 0xC000] = value;
        }
    }
}