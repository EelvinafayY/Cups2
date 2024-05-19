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
    /// Логика взаимодействия для ViewPositionPage.xaml
    /// </summary>
    public partial class ViewPositionPage : Page
    {
        Admin contextAdmin;
        public static List<Models.Menu> menu { get; set; }
        Models.Menu contextDish;
        public ViewPositionPage(Models.Menu menu)
        {
            InitializeComponent();
            contextAdmin = DBConnection.loginedAdmin;
            contextDish = menu;
            Refresh();
            this.DataContext = this;
        }

        public void Refresh()
        {
            DataContext = null;
            DataContext = contextDish;
            Step.stepNumber = 1;
            byte[] photoBytes = contextDish.Photo;
            if (photoBytes != null && photoBytes.Length > 0)
            {
                BitmapImage imageSource = new BitmapImage();
                using (MemoryStream stream = new MemoryStream(photoBytes))
                {
                    imageSource.BeginInit();
                    imageSource.CacheOption = BitmapCacheOption.OnLoad;
                    imageSource.StreamSource = stream;
                    imageSource.EndInit();
                }
                PhotoIB.Source = imageSource;
            }
            else
            {
                BitmapImage defaultImage = new BitmapImage(new Uri("C:\\Users\\user\\Downloads\\Cups1-master\\Cups1-master\\Cups\\Images\\qqq.png"));
                PhotoIB.Source = defaultImage;
            }
            NameTB.Text = contextDish.NamePosition;
            DescriptionTB.Text = contextDish.Description;
            if (contextDish.Compound != null)
            {
                string sostav = contextDish.Compound;
                string[] ingredients = sostav.Split(',');
                string newSostav = string.Join("\n- ", ingredients);
                SostavTB.Text = newSostav;
            }
            this.DataContext = this;
        }

        private void BackBT_Click(object sender, RoutedEventArgs e)
        {
            var typeName = contextDish.Type.Name;
            if (typeName != null)
            {
                NavigationService.Navigate(new ListOtherMenu(typeName));
            }
        }
        private void ExitTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DBConnection.loginedAdmin = null;
            NavigationService.Navigate(new AuthorizationPage());
        }

        private async void AddPhotoBT_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { Filter = ".png, .jpg, .jpeg| *.png; *.jpg; *.jpeg" };
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                var image = File.ReadAllBytes(dialog.FileName);
                contextDish.Photo = image;
                DataContext = null;
                DataContext = contextDish;
            }
            var response = await NetManager.Put("api/Menu/Edit", contextDish);
            response.EnsureSuccessStatusCode();
            Refresh();
        }

        private async void DeletePhotoBT_Click(object sender, RoutedEventArgs e)
        {
            var drink = contextDish;
            drink.Photo = null;
            var response = await NetManager.Put("api/Menu/Edit", drink);
            response.EnsureSuccessStatusCode();
            contextDish = drink;
            Refresh();
        }

        private void StajersTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new ViewStajersPage(contextAdmin));
        }

        private void MenuTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new MenuPage());
        }

        private void ProfileTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AdminHomePage(contextAdmin));
        }
    }
}
