// See https://aka.ms/new-console-template for more information


using AS.VirtualHuman;
using ImageFinderNS;
using System.Drawing;

VirtualHuman vh = new VirtualHuman();

var screen = vh.ScreenManager.GetScreenInfoByFriendlyName("LC27G5xT");
var screenCapture = vh.CreateScreenCapture(screen.RealArea, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
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

    Console.WriteLine("Press N to cancel. Any other key to continue to next.");
    var response = Console.ReadKey();

    if (response.Key == ConsoleKey.N)
        break;
}