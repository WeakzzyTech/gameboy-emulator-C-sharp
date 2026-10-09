using System;
using SDL3;

class Graphics
{
    private nint window = nint.Zero;

    public bool Initialize()
    {
        if (!SDL.Init(SDL.InitFlags.Video))
        {
            Console.WriteLine($"SDL Init Failed: {SDL.GetError()}");
            return false;
        }

        window = SDL.CreateWindow("Test", 600, 480, SDL.WindowFlags.Resizable);
        
        if (window == nint.Zero)
        {
            Console.WriteLine($"Window Creation Failed: {SDL.GetError()}");
            SDL.Quit();
            return false;
        }

        return true;
    }

    public void Shutdown()
    {
        if (window != nint.Zero)
        {
            SDL.DestroyWindow(window);
        }
        SDL.Quit();
    }
}
