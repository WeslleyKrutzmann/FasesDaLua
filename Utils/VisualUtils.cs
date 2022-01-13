using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FasesDaLua.Utils
{
    public static class VisualUtils
    {
        public static void SaveVisualBitmap(Visual visual, Size imageSize, string filePath)
        {
            var bmp = GetVisualBitmap(visual, imageSize);

            var pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(bmp));

            using (var fs = File.OpenWrite(filePath))
            {
                pngEncoder.Save(fs);
            }
        }

        private static RenderTargetBitmap GetVisualBitmap(Visual visual, Size imageSize)
        {
            var bmp = new RenderTargetBitmap((int)imageSize.Width, (int)imageSize.Height, 96, 96, PixelFormats.Default);

            var drawingvisual = new DrawingVisual();
            
            using (var context = drawingvisual.RenderOpen())
            {
                context.DrawRectangle(new VisualBrush(visual), null, new Rect(new Point(), imageSize));
                context.Close();
            }

            bmp.Render(drawingvisual);
            bmp.Freeze();

            return bmp;
        }
    }
}
