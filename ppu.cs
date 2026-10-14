using System;

public class PPU
{
    private int cycleCounter = 0;
    private int scanline = 0;

    public uint[] Framebuffer { get; private set; } = new uint[160 * 144];
    public bool VBlankRequested { get; private set; } = false;

    public void ClearVBlank() => VBlankRequested = false;

    public void Step(int cycles)
    {
        cycleCounter += cycles;

        while (cycleCounter >= 456)
        {
            cycleCounter -= 456;
            scanline++;

            if (scanline == 144)
            {
                VBlankRequested = true;
            }

            if (scanline >= 154)
            {
                scanline = 0;
            }
        }
    }
}