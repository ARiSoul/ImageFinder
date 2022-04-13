// See https://aka.ms/new-console-template for more information


using AS.VirtualHuman;
using ImageFinderNS;
using System.Drawing;

VirtualHuman vh = new VirtualHuman();

var screen = vh.ScreenManager.GetScreenInfoByFriendlyName("LC27G5xT");
var screenCapture = vh.CreateScreenCapture(screen.RealArea, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
var findImg = Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "find.png"));

ImageFinder.SetSource(screenCapture);
var matches = ImageFinder.Find(findImg, 0.8f);

foreach (var match in matches)
{
    vh.MouseMove((uint)(screen.RealXPos + match.Zone.Location.X), (uint)(screen.RealYPos + match.Zone.Location.Y));
    vh.Delay(2000);
    vh.Click();
    vh.Delay(5000);
}