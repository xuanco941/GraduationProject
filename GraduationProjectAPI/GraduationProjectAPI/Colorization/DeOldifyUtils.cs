using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProjectAPI.Colorization
{
    public class DeOldifyUtils
    {
        private static Bitmap __Blurify(Bitmap source)
        {
            var output = new Bitmap(source.Width, source.Height);
            for (int y = 0; y < output.Height; ++y)
            {
                for (int x = 0; x < output.Width; ++x)
                {
                    var a = 0f;
                    var r = 0f;
                    var g = 0f;
                    var b = 0f;
                    for (int ky = 0; ky < 5; ++ky)
                    {
                        var iy = y + ky - 2;
                        if (iy < 0 || iy >= source.Height)
                        {
                            continue;
                        }
                        for (int kx = 0; kx < 5; ++kx)
                        {
                            var ix = x + kx - 2;
                            if (ix < 0 || ix >= source.Width)
                            {
                                continue;
                            }
                            var c = source.GetPixel(ix, iy);
                            a += c.A;
                            r += c.R;
                            g += c.G;
                            b += c.B;
                        }
                    }
                    output.SetPixel(x, y, Color.FromArgb((byte)(a / 25), (byte)(r / 25), (byte)(g / 25), (byte)(b / 25)));
                }
            }
            return output;
        }

        /// <summary>
        /// Converts the image to greyscale.
        /// </summary>
        /// <param name="source">Input image.</param>
        /// <returns>Greyscale image.</returns>
        private static Bitmap __Decolorize(Bitmap source)
        {
            var result = new Bitmap(source);
            for (int y = 0; y < result.Height; ++y)
            {
                for (int x = 0; x < result.Width; ++x)
                {
                    var c = result.GetPixel(x, y);
                    var l = (byte)((c.R + c.G + c.B) / 3);
                    result.SetPixel(x, y, Color.FromArgb(c.A, l, l, l));
                }
            }
            return result;
        }

    }
}
