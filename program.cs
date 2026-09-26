using System;
using System.IO;

class Program
{
    static void Main()
    {
        string path = "roms/pokemonred.gb";
        
        byte[] rom = File.ReadAllBytes(path);

        Memory memory = new Memory(rom);
        CPU cpu = new CPU();
        PPU ppu = new PPU();

        Console.WriteLine($"Rom size: {rom.Length} bytes");

        bool running = true;

        while (running)
        {
            int cycles = cpu.Step(memory);

            if (cycles == -1)
                break;

            ppu.Step(cycles);
        }
    }
}