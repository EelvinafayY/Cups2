using Cups.Models;
using Cups.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

namespace Cups.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddDrinkPage.xaml
    /// </summary>
    public partial class AddDrinkPage : Page
    {
        public static List<Models.Menu> menus {  get; set; }
        public static List<Models.Type> types { get; set; }
        public Cups.Models.Type contextType;
        Admin contextAdmin;
        Models.Menu contextDrink = new Models.Menu();
        List<Step> recipeSteps = new List<Step>();
        public static List<Lesson_Drink> lessons { get; set; }
        Lesson_Drink contextLesson = new Lesson_Drink();
        public int click = 0;
        public AddDrinkPage(string name)
        {
            InitializeComponent();
            contextType = DBConnection.types.FirstOrDefault(t => t.Name == name);
            contextAdmin = DBConnection.loginedAdmin;
            AddDrink();
            Refresh();
            this.DataContext = this;
        }

        public async void AddDrink()
        {
            var a = contextDrink;
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
                        await DBConnection.RefreshEnums();
                        contextDrink = a;
                        AddLesson();
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


        public async void AddLesson()
        {
            try
            {
                var b = contextLesson;
                b.Id_Menu = contextDrink.Id_Menu;
                var a = DBConnection.lesson_Drinks.FirstOrDefault(l => l.Id_Menu == contextDrink.Id_Menu);
                if (a == null)
                {
                    await NetManager.Post("api/Lesson_Drink/Add", b);
                    using (HttpClient httpClient = new HttpClient())
                    {
                        HttpResponseMessage response = await httpClient.GetAsync("http://localhost:50401/api/Lesson_Drink/LastInsertedId");
                        int lastInsertedId = 0;
                        if (response.IsSuccessStatusCode)
                        {
                            // Читаем содержимое ответа как строку
                            string content = await response.Content.ReadAsStringAsync();
                            // Преобразуем содержимое в целое число
                            if (int.TryParse(content, out lastInsertedId))
                            {
                                b.Number_Lesson = lastInsertedId;
                                contextLesson = b;
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        public void Refresh()
        {
            byte[] photoBytes = contextDrink.Photo;
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
            Step.stepNumber = 1;
            DrinkStepsLV.ItemsSource = recipeSteps;
        }

        private async void BackBT_Click(object sender, RoutedEventArgs e)
        {
            var a = DBConnection.menu.FirstOrDefault(s => s.Id_Menu == contextDrink.Id_Menu && s.NamePosition != null);
            if(contextDrink.NamePosition != NameTB.Text || contextDrink.Description != DescriptionTB.Text || contextDrink.Compound != "," + SostavTB.Text.Trim() ||
                contextDrink.Volume != VolumeTB.Text.Trim())
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите продолжить?", "Не сохраненные данные будут потеряны", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (a == null)
                    {
                        var st = contextDrink;
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
                NavigationService.Navigate(new ListDrinksPage(contextType.Name));
            }

        }

        private async void AddPhotoBT_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { Filter = ".png, .jpg, .jpeg| *.png; *.jpg; *.jpeg" };
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                var image = File.ReadAllBytes(dialog.FileName);
                contextDrink.Photo = image;
                DataContext = null;
                DataContext = contextDrink;
            }
            var response = await NetManager.Put("api/Menu/Edit", contextDrink);
            response.EnsureSuccessStatusCode();
            Refresh();
        }

        private void DeletePhotoBT_Click(object sender, RoutedEventArgs e)
        {
            if(contextDrink.Photo != null)
            {
                var drink = contextDrink;
                drink.Photo = null;
                Refresh();
            }
            else
            {
                MessageBox.Show("Фотография отсутствует");
            }
        }

        private async void AddStepBt_Click(object sender, RoutedEventArgs e)
        {
            AddLesson();
            var step = new Step();
            string error = "";
            if (String.IsNullOrWhiteSpace(DescriptionStepTB.Text))
                error += "Заполните описание действий!\n";
            if (!String.IsNullOrWhiteSpace(error))
            {
                MessageBox.Show(error);
                return;
            }
            step.Description = DescriptionStepTB.Text;
            if (contextLesson.Number_Lesson != 0)
            {
                recipeSteps = DBConnection.steps.Where(r => r.Number_Lesson == contextLesson.Number_Lesson).ToList();
                step.Number_Lesson = contextLesson.Number_Lesson;
                recipeSteps.Add(step);
                await NetManager.Post("api/Step/Add", step);
                await DBConnection.RefreshData();
            }
            if (contextLesson.Number_Lesson == 0)
            {
                MessageBox.Show("Какая-то ошибка(");
            }
            DescriptionStepTB.Text = "";
            Refresh();
           
        }

        private async void SaveBT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем, что все поля заполнены
                if (string.IsNullOrWhiteSpace(NameTB.Text) ||
                    
                    string.IsNullOrWhiteSpace(SostavTB.Text) ||
                    string.IsNullOrWhiteSpace(VolumeTB.Text) ||
                    string.IsNullOrWhiteSpace(DescriptionTB.Text)) 
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;
                }


                contextDrink.NamePosition = NameTB.Text.Trim();
                contextDrink.Description = DescriptionTB.Text.Trim();
                contextDrink.Compound = "," + SostavTB.Text.Trim();
                contextDrink.Volume = VolumeTB.Text.Trim();

                var existing = DBConnection.menu.FirstOrDefault(s => s.NamePosition == contextDrink.NamePosition && s.Volume == contextDrink.Volume && s.Id_Menu != contextDrink.Id_Menu);
                
                if (existing != null)
                {
                    MessageBox.Show("Напиток с таким наименованием и объемом уже существует!");
                    return;
                }

                var response = await NetManager.Put("api/Menu/Edit", contextDrink);
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

        private void Proverka1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, является ли введенный символ буквой
            if (!char.IsLetter(e.Text, 0))
            {
                // Если это не буква, отменяем ввод
                e.Handled = true;
            }
        }

        private void Proverka2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, является ли введенный символ цифрой
            if (!char.IsDigit(e.Text, 0))
            {
                // Если это не цифра, отменяем ввод
                e.Handled = true;
            }
        }
    }
}
