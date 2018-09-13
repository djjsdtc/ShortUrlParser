using ShortUrlParse;
using System;

namespace ShortUrlParserSample
{
    class Program
    {
        static void Main(string[] args)
        {
            ShortUrlParser parser = new ShortUrlParser();
            QRCodeParser imageParser = new QRCodeParser();
            ParseQRCodeSample(imageParser, parser);
        }

        static void ParseSimpleUrlSample(ShortUrlParser parser)
        {
            Console.WriteLine(parser.IsGranBlueFantasy("http://%67%61%6D%65%2E%67%72%61%6E%62%6C%75%65%66%61%6E%74%61%73%79%2E%6A%70/"));
            Console.WriteLine(parser.IsGranBlueFantasy("http://dwz.cn/5oY8eKnB"));
            Console.WriteLine(parser.IsGranBlueFantasy("http://t.cn/RfKVOyV"));
            Console.WriteLine(parser.IsGranBlueFantasy("http://game.granbluefantasy.jp/"));
            Console.WriteLine(parser.IsGranBlueFantasy("http://%73%69%6E%61%2E%6C%74/fGqe"));
        }

        static void ParseQRCodeSample(QRCodeParser imageParser, ShortUrlParser parser)
        {
            Console.WriteLine(parser.ParseShortUrl(imageParser.Parse(@"images\image1.png")));
            Console.WriteLine(parser.ParseShortUrl(imageParser.Parse(@"images\image2.png")));
            Console.WriteLine(parser.ParseShortUrl(imageParser.Parse(@"images\image3.png")));
            Console.WriteLine(parser.ParseShortUrl(imageParser.Parse(@"images\image4.png")));
            Console.WriteLine(parser.ParseShortUrl(imageParser.Parse(@"images\image5.png")));
            Console.WriteLine(parser.ParseShortUrl(imageParser.Parse(@"images\image6.png")));
            Console.WriteLine(parser.ParseShortUrl(imageParser.Parse(@"images\image7.png")));
            Console.WriteLine(parser.ParseShortUrl(imageParser.Parse(@"images\image8.png")));
            Console.WriteLine(parser.ParseShortUrl(imageParser.Parse(@"images\image9.png")));
        }
    }
}
