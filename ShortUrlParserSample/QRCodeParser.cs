using System.Drawing;
using ZXing;

namespace ShortUrlParserSample
{
    public class QRCodeParser
    {
        public string Parse(string imageFilename)
        {
            BarcodeReader reader = new BarcodeReader();
            reader.Options.CharacterSet = "UTF-8";
            Bitmap map = new Bitmap(imageFilename);
            Result result = reader.Decode(map);
            return result == null ? string.Empty : result.Text;
        }
    }
}
