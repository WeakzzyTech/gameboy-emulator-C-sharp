using System;
using System.IO;
using SDL3; // Access SDL structures

class Program
{
    static void Main()
    {
        string path = "roms/pokemonred.gb";
        
        Graphics graphics = new Graphics();
        if (!graphics.Initialize())
        {
            Console.WriteLine("Failed to initialize graphics. Exiting.");
            return;
        }

        byte[] rom = File.ReadAllBytes(path);

        Memory memory = new Memory(rom);
        CPU cpu = new CPU();
        PPU ppu = new PPU();

        Console.WriteLine($"Rom size: {rom.Length} bytes");

        bool running = true;

        while (running)
        {
            while (SDL.PollEvent(out SDL.Event e))
            {
                if ((SDL.EventType)e.Type == SDL.EventType.Quit)
                {
                    running = false;
                    break;
                }
            }

            if (!running) break;

            int cycles = cpu.Step(memory);

            if (cycles == -1)
                break;

            ppu.Step(cycles);
        }

        graphics.Shutdown();
        Console.WriteLine("Emulator closed cleanly.");
    }
}
