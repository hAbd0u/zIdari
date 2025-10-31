using System.Drawing;

namespace zIdari.Model
{
    public class ScannedPage
    {
        public int PageNumber { get; set; }
        public Image ImageData { get; set; }
        public int Rotation { get; set; }  // 0, 90, 180, 270 degrees
        public Rectangle? CropRegion { get; set; }  // Null if not cropped
        public bool IsDuplex { get; set; }
        public bool IsImported { get; set; }  // True if imported from file
        public string ImportSourcePath { get; set; }  // Original file path if imported
        public int OriginalPdfPageNumber { get; set; }  // If from PDF, which page number

        /// <summary>
        /// Gets the image with rotation and crop applied (for display)
        /// </summary>
        public Image GetDisplayImage()
        {
            if (ImageData == null) return null;

            Image result = (Image)ImageData.Clone();

            // Apply rotation
            if (Rotation != 0)
            {
                result = RotateImage(result, Rotation);
            }

            // Apply crop
            if (CropRegion.HasValue)
            {
                var crop = CropRegion.Value;
                var cropped = new Bitmap(crop.Width, crop.Height);
                using (var g = Graphics.FromImage(cropped))
                {
                    g.DrawImage(result, new Rectangle(0, 0, crop.Width, crop.Height), crop, GraphicsUnit.Pixel);
                }
                result?.Dispose();
                result = cropped;
            }

            return result;
        }

        private Image RotateImage(Image img, int degrees)
        {
            if (degrees == 0) return (Image)img.Clone();

            degrees = degrees % 360;
            if (degrees < 0) degrees += 360;

            // Calculate new dimensions (swap if 90 or 270)
            int newWidth = (degrees == 90 || degrees == 270) ? img.Height : img.Width;
            int newHeight = (degrees == 90 || degrees == 270) ? img.Width : img.Height;

            var rotated = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(rotated))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.TranslateTransform(newWidth / 2f, newHeight / 2f);
                g.RotateTransform(degrees);
                g.TranslateTransform(-img.Width / 2f, -img.Height / 2f);
                g.DrawImage(img, new Point(0, 0));
            }

            return rotated;
        }
    }
}

