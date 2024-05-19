using Cups.Models;
using Cups.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cups.Pages
{
    /// <summary>
    /// Логика взаимодействия для StajerHomePage.xaml
    /// </summary>
    public partial class StajerHomePage : Page
    {
        public static List<Stajer> stajers {  get; set; }
        Stajer contextStajer;
        
        public StajerHomePage(Stajer stajer)
        {
            InitializeComponent();
            contextStajer = stajer;
            contextStajer = DBConnection.loginedStajer;
            Refresh();
            this.DataContext = this;

        }
        public void Refresh()
        {
            DataContext = null;
            DataContext = contextStajer;
            SurnameTB.Text = contextStajer.Surname;
            NameTB.Text = contextStajer.Name;
            PatronymicTB.Text = contextStajer.Patronymic;
            ProgressLessonPB.Value = contextStajer.NumberOfDoneLesson;
            ProgressLessonPB.Maximum = contextStajer.NumberOfLessons;

            byte[] photoBytes = contextStajer.Photo;

            // Проверяем, есть ли у пользователя фото
            if (photoBytes != null && photoBytes.Length > 0)
            {
                // Создаем BitmapImage из двоичных данных
                BitmapImage imageSource = new BitmapImage();
                using (MemoryStream stream = new MemoryStream(photoBytes))
                {
                    imageSource.BeginInit();
                    imageSource.CacheOption = BitmapCacheOption.OnLoad;
                    imageSource.StreamSource = stream;
                    imageSource.EndInit();
                }

                // Привязываем BitmapImage к свойству ImageSource ImageBrush
                PhotoIB.ImageSource = imageSource;
            }
            else
            {
                // Если фото отсутствует, загружаем изображение по умолчанию
                BitmapImage defaultImage = new BitmapImage(new Uri("C:\\Users\\user\\Downloads\\Cups1-master\\Cups1-master\\Cups\\Images\\nullphoto.jpg"));
                PhotoIB.ImageSource = defaultImage;
            }

            this.DataContext = this;
        }


        private void TestsTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new TestsPage(contextStajer));
        }
       

        private void MenuTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new MenuPageStajer());
        }

        private void ProfileTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new StajerHomePage(contextStajer));
        }
        private void ExitTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DBConnection.loginedStajer = null;
            NavigationService.Navigate(new AuthorizationPage());
        }

        private async void AddPhotoBT_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { Filter = ".png, .jpg, .jpeg| *.png; *.jpg; *.jpeg" };
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                var image = File.ReadAllBytes(dialog.FileName);
                contextStajer.Photo = image;
                DataContext = null;
                DataContext = contextStajer;
            }
            var response = await NetManager.Put("api/Stajer/Edit", contextStajer);
            response.EnsureSuccessStatusCode();
            Refresh();
        }

        private async void DeletePhotoBT_Click(object sender, RoutedEventArgs e)
        {
            var stajer = contextStajer;
            if (stajer.Photo != null)
            {
                stajer.Photo = null;
                var response = await NetManager.Put("api/Stajer/Edit", stajer);
                response.EnsureSuccessStatusCode();
                contextStajer = stajer;
                Refresh();
            }
            else
            {
                MessageBox.Show("Фото отсутствует!");
            }
        }
    }
}
