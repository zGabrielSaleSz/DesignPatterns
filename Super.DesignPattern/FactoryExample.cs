using System.Drawing;
using System.Drawing.Imaging;

namespace Super.DesignPattern
{
    public class FactoryExample : IDesignPatternExample
    {
        public void Run()
        {
            Banner banner = BannerFactory.Create("That's how Factory works", "https://raw.githubusercontent.com/zGabrielSaleSz/SuperORM/main/assets/super_icon_normal.png");

            string path = Path.Combine(Directory.GetCurrentDirectory(), "banner.jpeg");

            banner.GetBanner().GetSource().Save(path, ImageFormat.Jpeg);
            Console.WriteLine($"Your banner is at {path}");
        }
    }

    public class BannerFactory
    {
        public static Banner Create(string title, string pictureUrl)
        {
            ImageDownloader imageDownloader = new ImageDownloader();
            SuperPicture superPicture = imageDownloader.Download(pictureUrl).GetAwaiter().GetResult();

            Banner banner = new Banner
            {
                Title = title,
                ExpirationDate = DateTime.Now.AddDays(30),
                SourcePicture = superPicture
            };
            return banner;
        }
    }

    public class Banner
    {
        public string? Title { get; set; }
        public SuperPicture? SourcePicture { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public SuperPicture GetBanner()
        {
            if (SourcePicture == null)
                throw new InvalidDataException();

            Bitmap newImage = SourcePicture.GetSource();
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                Font drawFont = new Font("Arial", 16);
                SolidBrush drawBrush = new SolidBrush(Color.White);

                float x = SourcePicture.WidthPixels / 5;
                float y = SourcePicture.HeightPixels - (SourcePicture.HeightPixels / 5);

                PointF drawPoint = new PointF(x, y);
                graphics.DrawString(Title, drawFont, drawBrush, drawPoint);
            }
            return new SuperPicture(newImage);
        }
    }

    public class ImageDownloader
    {
        public async Task<SuperPicture> Download(string url)
        {
            HttpClient client = new HttpClient();
            byte[] result = await client.GetByteArrayAsync(url);
            SuperPicture superPicture = new SuperPicture(result);
            return superPicture;
        }
    }

    public class SuperPicture
    {
        public SuperPicture(byte[] pictureBytes)
        {
            MemoryStream memoryStream = new MemoryStream(pictureBytes);
            Source = new Bitmap(memoryStream);
            HeightPixels = Source.Height;
            WidthPixels = Source.Width;
        }

        public SuperPicture(Bitmap source)
        {
            Source = (Bitmap)source.Clone();
            HeightPixels = source.Height;
            WidthPixels = source.Width;
        }
        public int HeightPixels { get; set; }
        public int WidthPixels { get; set; }
        private Bitmap Source { get; set; }

        public Bitmap GetSource()
        {
            return (Bitmap)Source.Clone();
        }
    }
}
