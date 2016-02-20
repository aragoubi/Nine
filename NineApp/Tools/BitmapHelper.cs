using System.IO;
using System.Windows.Media.Imaging;

namespace Nine.Tools
{
    /// <summary>
    /// Provide some methods to improve Bitmap management
    /// </summary>
    internal class BitmapHelper
    {
        /// <summary>
        /// Shortcut to load a BitmapImage
        /// </summary>
        /// <param name="path">Path to the image file to load</param>
        /// <returns>Returns a BitmapImage</returns>
        public static BitmapImage Load(string path)
        {
            var bitmap = new BitmapImage();
            using (var stream = File.OpenRead(path))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
            }
            return bitmap;
        }
    }
}