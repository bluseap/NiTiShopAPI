using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;


namespace NiTiAPI.WebErp.Extensions
{
    public static class ImageResizerUpfileExtensions
    {
        public static void ResizeAndSaveImageW60h90(Stream stream, string filename)
        {
            using (Image<Rgba32> image = Image.Load(stream))
            {
                image.Mutate(x => x.Resize(90, 90));
                image.Save(filename); // Automatic encoder selected based on extension.
            }
        }

        public static void ResizeAndSaveImage(Stream stream, string filename)
        {
            using (Image<Rgba32> image = Image.Load(stream))
            {
                image.Mutate(x => x
                     .Resize(image.Width / 2, image.Height / 2)
                 );
                image.Save(filename); // Automatic encoder selected based on extension.
            }
        }
    }
}
