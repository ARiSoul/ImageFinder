// See https://aka.ms/new-console-template for more information


using AS.VirtualHuman;
using ImageFinderNS;
using System.Drawing;
using System.Drawing.Imaging;

VirtualHuman vh = new VirtualHuman();

// acer - LC27G5xT 
// samsung quad - ED242QR
// tv - SAMSUNG
var screen = vh.ScreenManager.GetScreenInfoByFriendlyName("ED242QR");
var screenCapture = vh.CreateScreenCapture(screen.RealArea, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
var findImg = Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "find.png"));

screenCapture.Save("screenCapture.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

ImageFinder.SetSource(screenCapture);
var matches = ImageFinder.Find(findImg, 0.6f);

Console.WriteLine("Found {0} matches", matches.Count);

foreach (var match in matches)
{
    Console.WriteLine("Match: " + match.Zone.ToString());
    Console.WriteLine("Similarity: " + match.Similarity);

    vh.MouseMove((uint)(screen.RealXPos + match.Zone.Location.X), (uint)(screen.RealYPos + match.Zone.Location.Y));

    //Console.WriteLine("Click? (Y/N)");
    //var response = Console.ReadLine();

    //if (response == "Y")
    //    vh.Click();

    Console.WriteLine("Press C to cancel. Any other key to continue to next.");
    var response = Console.ReadKey();

    if (response.Key == ConsoleKey.C)
        break;
}

static void ProcessUsingLockbitsAndUnsafeAndParallel(Bitmap processedBitmap)
{
    unsafe
    {
        BitmapData bitmapData = processedBitmap.LockBits(new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.ReadWrite, processedBitmap.PixelFormat);

        int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
        int heightInPixels = bitmapData.Height;
        int widthInBytes = bitmapData.Width * bytesPerPixel;
        byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

        Parallel.For(0, heightInPixels, y =>
        {
            byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
            for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
            {
                int oldBlue = currentLine[x];
                int oldGreen = currentLine[x + 1];
                int oldRed = currentLine[x + 2];

                currentLine[x] = (byte)oldBlue;
                currentLine[x + 1] = (byte)oldGreen;
                currentLine[x + 2] = (byte)oldRed;
            }
        });
        processedBitmap.UnlockBits(bitmapData);
    }
}