Console.WriteLine("Hello, World!");


var engine = new Pixel.PixelEngine<float>(new Pixel.GLES.PixelWindow(1920, 1080), new Pixel.GLES.Graphics());

engine.Start();