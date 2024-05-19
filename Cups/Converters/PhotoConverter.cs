using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Cups.Converters
{
   
        public class PhotoConverter : IMultiValueConverter
        {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] photoBytes = values[0] as byte[];
            string defaultImagePath = values[1] as string;

            // Проверяем, если фото не задано
            if (photoBytes == null || photoBytes.Length == 0)
            {
                // Создаем BitmapImage из пути к изображению по умолчанию
                BitmapImage imageSource = new BitmapImage(new Uri(defaultImagePath));
                return imageSource;
            }

            // Создаем BitmapImage из двоичных данных
            BitmapImage image = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(photoBytes))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
            }

            return image;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotSupportedException();
            }
        }
}
