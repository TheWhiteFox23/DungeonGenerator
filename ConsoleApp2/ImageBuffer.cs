using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DungeonGenerator 
{
    class ImageBuffer
    {
        byte[] buffer;
        public ImageBuffer()
        {
            buffer = new byte[(4 * 256) * 256];
            GrayScale();
        }

        private void PlotPixel(int X, int Y, byte redValue, byte greenValue, byte blueValue)
        {
            int offset = ((256 * 4) * Y) + (X * 4);
            buffer[offset] = blueValue;
            buffer[offset + 1] = greenValue;
            buffer[offset + 2] = redValue;
            // Fixed alpha value (No transparency)
            buffer[offset + 3] = 255;
        }

        private void GrayScale()
        {
            for (int y = 0; y < 256; y++)
            {
                for (int x = 0; x < 256; x++)
                {
                    byte val = (byte)(x%256);
                    byte valY = (byte)(y % 256);
                    PlotPixel(x, y, valY, 0, val);
                }
            }
        }

        public void save()
        {
            unsafe
            {
                fixed (byte* ptr = buffer)
                {
                    using (Bitmap image = new Bitmap(256, 256, 256 * 4,
                        System.Drawing.Imaging.PixelFormat.Format32bppRgb, new IntPtr(ptr)))
                    {
                        image.Save(@"greyscale.png");
                    }
                }
            }
        }
    }
}
