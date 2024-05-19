using Cups.Models;
using Cups.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using System.Xml.Linq;

namespace Cups.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddPositionPage.xaml
    /// </summary>
    public partial class AddPositionPage : Page
    {
        public static List<Models.Menu> menus { get; set; }
        public static List<Models.Type> types { get; set; }
        public Cups.Models.Type contextType;
        Admin contextAdmin;
        Models.Menu contextDish = new Models.Menu();
        public int click = 0;

        public AddPositionPage(string name)
        {
            InitializeComponent();
            contextType = DBConnection.types.FirstOrDefault(t => t.Name == name);
            contextAdmin = DBConnection.loginedAdmin;
            AddDish();
            Refresh();
            this.DataContext = this;
        }
        private void Proverka1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, является ли введенный символ буквой
            if (!char.IsLetter(e.Text, 0))
            {
                // Если это не буква, отменяем ввод
                e.Handled = true;
            }
        }

        public async void AddDish()
        {
            var a = contextDish;
            a.Kod_Type = contextType.Kod_Type;
            await NetManager.Post("api/Menu/Add", a);

            // Создаем экземпляр HttpClient
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync("http://localhost:50401/api/Menu/LastInsertedId");
                int lastInsertedId = 0;
                if (response.IsSuccessStatusCode)
                {
                    // Читаем содержимое ответа как строку
                    string content = await response.Content.ReadAsStringAsync();
                    // Преобразуем содержимое в целое число
                    if (int.TryParse(content, out lastInsertedId))
                    {
                        a.Id_Menu = lastInsertedId;
                        contextDish = a;
                        await DBConnection.RefreshEnums();
                        
                    }
                    else
                    {
                        MessageBox.Show("Ошибка: Не удалось преобразовать содержимое ответа в целое число.");
                    }
                }
                else
                {
                    MessageBox.Show($"Ошибка HTTP: {response.StatusCode}");
                }
            }
        }

        public void Refresh()
        {
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
        }

        private async void SaveBT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем, что все поля заполнены
                if (string.IsNullOrWhiteSpace(NameTB.Text) ||

                    string.IsNullOrWhiteSpace(SostavTB.Text) ||
                    string.IsNullOrWhiteSpace(DescriptionTB.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;
                }


                contextDish.NamePosition = NameTB.Text.Trim();
                contextDish.Description = DescriptionTB.Text.Trim();
                contextDish.Compound = "," + SostavTB.Text.Trim();

                var existing = DBConnection.menu.FirstOrDefault(s => s.NamePosition == contextDish.NamePosition && s.Id_Menu != contextDish.Id_Menu);

                if (existing != null)
                {
                    MessageBox.Show("Позиция с таким наименованием уже существует!");
                    return;
                }

                var response = await NetManager.Put("api/Menu/Edit", contextDish);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка HTTP: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private async void BackBT_Click(object sender, RoutedEventArgs e)
        {
            var a = DBConnection.menu.FirstOrDefault(s => s.Id_Menu == contextDish.Id_Menu && s.NamePosition != null);
            if(contextDish.NamePosition != NameTB.Text.Trim() || contextDish.Description != DescriptionTB.Text.Trim() || contextDish.Compound != "," + SostavTB.Text.Trim())
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите продолжить?", "Не сохраненные данные будут потеряны", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (a == null)
                    {
                        var st = contextDish;
                        await NetManager.Delete<bool>($"api/Menu/Delete/{st.Id_Menu}");
                        await DBConnection.RefreshEnums();
                        await Services.DBConnection.InitializeDBConnection();
                        NavigationService.Navigate(new ListOtherMenu(contextType.Name));
                    }
                    else
                    {
                        NavigationService.Navigate(new ListOtherMenu(contextType.Name));
                    }

                }
                if (result == MessageBoxResult.No)
                {

                }
            }
            
            else
            {
                NavigationService.Navigate(new ListOtherMenu(contextType.Name));
            }

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

        private void DeletePhotoBT_Click(object sender, RoutedEventArgs e)
        {
            if (contextDish.Photo != null)
            {
                var drink = contextDish;
                drink.Photo = null;
                Refresh();
            }
            else
            {
                MessageBox.Show("Фотография отсутствует");
            }
        }
    }
}
