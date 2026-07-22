using System;
using System.IO;

class Program
{
    static void Main()
    {
        string path = "/home/WeakzzyTech/Documents/Gameboy_emulator_C#/roms/tetris.gb";
        
        byte[] rom = File.ReadAllBytes(path);

        Memory memory = new Memory(rom);

        CPU cpu = new CPU();

        Console.WriteLine($"Rom size: {rom.Length} bytes");
        while (cpu.Step(memory))
        {
        }
    }
}