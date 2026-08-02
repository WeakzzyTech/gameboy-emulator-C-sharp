using System;

public class PPU
{
    private int cycleCounter = 0;
    private int scanline = 0;

    public void Step(int cycles)
    {
        cycleCounter += cycles;

        while (cycleCounter >= 456)
        {
            cycleCounter -= 456;
            scanline++;

            Console.WriteLine($"Scanline: {scanline}");

            if (scanline == 144)
            {
                Console.WriteLine("Entering VBlank");
            }

            if (scanline >= 154)
            {
                scanline = 0;
                Console.WriteLine("New Frame");
            }
        }
    }
}