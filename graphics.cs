using System;
using SDL3;

class Graphics
{
    private nint window = nint.Zero;
    private nint renderer = nint.Zero;
    private nint texture = nint.Zero;

    public const int ScreenWidth = 160;
    public const int ScreenHeight = 144;

    public bool Initialize()
    {
        if (!SDL.Init(SDL.InitFlags.Video))
        {
            Console.WriteLine($"SDL Init Failed: {SDL.GetError()}");
            return false;
        }

        window = SDL.CreateWindow("SCGB - A game boy emulator in C#", ScreenWidth * 3, ScreenHeight * 3, SDL.WindowFlags.Resizable);
        if (window == nint.Zero)
        {
            Console.WriteLine($"Window Creation Failed: {SDL.GetError()}");
            SDL.Quit();
            return false;
        }

        renderer = SDL.CreateRenderer(window, null);
        if (renderer == nint.Zero)
        {
            Console.WriteLine($"Renderer Creation Failed: {SDL.GetError()}");
            Shutdown();
            return false;
        }

        texture = SDL.CreateTexture(renderer, (SDL.PixelFormat)0x16111804, SDL.TextureAccess.Streaming, ScreenWidth, ScreenHeight);
        if (texture == nint.Zero)
        {
            Console.WriteLine($"Texture Creation Failed: {SDL.GetError()}");
            Shutdown();
            return false;
        }

        return true;
    }

    public void RenderFrame(uint[] pixels)
    {
        unsafe
        {
            fixed (uint* ptr = pixels)
            {
                SDL.UpdateTexture(texture, IntPtr.Zero, (nint)ptr, ScreenWidth * sizeof(uint));
            }
        }

        SDL.RenderClear(renderer);
        SDL.RenderTexture(renderer, texture, IntPtr.Zero, IntPtr.Zero);
        SDL.RenderPresent(renderer);
    }

    public void Shutdown()
    {
        if (texture != nint.Zero) SDL.DestroyTexture(texture);
        if (renderer != nint.Zero) SDL.DestroyRenderer(renderer);
        if (window != nint.Zero) SDL.DestroyWindow(window);
        SDL.Quit();
    }
}