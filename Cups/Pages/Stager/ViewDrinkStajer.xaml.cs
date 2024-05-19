using Cups.Models;
using Cups.Pages.Test;
using Cups.Services;
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

namespace Cups.Pages
{
    /// <summary>
    /// Логика взаимодействия для ViewDrinkStajer.xaml
    /// </summary>
    public partial class ViewDrinkStajer : Page
    {
        Stajer contextStajer;
        public static List<Models.Menu> menu { get; set; }
        Models.Menu contextDrink;
        public static List<Lesson_Drink> lessons { get; set; }
        Lesson_Drink contextLesson;
        public static List<LessonDrink_Stajer> lessonDrink_Stajers { get; set; }
        public ViewDrinkStajer(Models.Menu drink)
        {
            InitializeComponent();
            contextStajer = DBConnection.loginedStajer;
            contextDrink = drink;

            Refresh();
            this.DataContext = this;
        }
        
        public void Refresh()
        {
            DataContext = null;
            DataContext = contextDrink;
            Step.stepNumber = 1;
            
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
            NameTB.Text = contextDrink.NamePosition;
            DescriptionTB.Text = contextDrink.Description;
            if (contextDrink.Compound != null)
            {
                string sostav = contextDrink.Compound;
                string[] ingredients = sostav.Split(',');
                string newSostav = string.Join("\n- ", ingredients);
                SostavTB.Text = newSostav;
            }
            if (contextDrink.Volume != null)
            {
                VolumeTB.Text = "Объем - " + contextDrink.Volume + " мл";
            }
            contextLesson = DBConnection.lesson_Drinks.FirstOrDefault(l => l.Id_Menu == contextDrink.Id_Menu);
            if (contextLesson != null)
            {
                var steps = new List<Step>(DBConnection.steps.Where(s => s.Number_Lesson == contextLesson.Number_Lesson).ToList());
                DrinkStepsLV.ItemsSource = steps;
            }
            var thisLesson = DBConnection.lessons_Stajer.FirstOrDefault(ls => ls.Id_Stajer == contextStajer.Id_Stajer && ls.Number_Lesson == contextLesson.Number_Lesson);
            if (thisLesson != null)
            {
                DoneCB.IsChecked = thisLesson.Done;
                BasicRatingBar.Value = (int)thisLesson.Rating;
                
            }
            else
            {
                DoneCB.IsChecked = false;
                BasicRatingBar.Value = 1;
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

        private void BackBT_Click(object sender, RoutedEventArgs e)
        {
            var typeName = contextDrink.Type.Name;
            if (typeName != null)
            {
                NavigationService.Navigate(new ListDrinksPageStajer(typeName));
            }
        }
        private async void Lesson()
        {
            await DBConnection.InitializeDBConnection();
            var lesson1 = DBConnection.lessons_Stajer.FirstOrDefault(ls => ls.Number_Lesson == contextLesson.Number_Lesson && ls.Id_Stajer == contextStajer.Id_Stajer);
            if (lesson1 != null)
            {
                lesson1.Done = true;
                await NetManager.Put("api/LessonDrink_Stajer/Edit", lesson1);
            }
            else
            {
                LessonDrink_Stajer lessonDrink_Stajer = new LessonDrink_Stajer
                {
                    Id_Stajer = contextStajer.Id_Stajer,
                    Number_Lesson = contextLesson.Number_Lesson,
                    Done = true
                };

                await NetManager.Post("api/LessonDrink_Stajer/Add", lessonDrink_Stajer);

                // Создаем экземпляр HttpClient
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync("http://localhost:50401/api/LessonDrink_Stajer/LastInsertedId");
                    int lastInsertedId = 0;
                    if (response.IsSuccessStatusCode)
                    {
                        // Читаем содержимое ответа как строку
                        string content = await response.Content.ReadAsStringAsync();
                        // Преобразуем содержимое в целое число
                        if (int.TryParse(content, out lastInsertedId))
                        {
                            lessonDrink_Stajer.Number = lastInsertedId;
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
            await DBConnection.RefreshData();
        }

        private async void LessonFalse()
        {
            var lesson1 = DBConnection.lessons_Stajer.FirstOrDefault(ls => ls.Number_Lesson == contextLesson.Number_Lesson && ls.Id_Stajer == contextStajer.Id_Stajer);
            if (lesson1 != null)
            {
                lesson1.Done = false;
                lesson1.Rating = 1;
                await NetManager.Put("api/LessonDrink_Stajer/Edit", lesson1);
            }
            else
            {
                LessonDrink_Stajer lessonDrink_Stajer = new LessonDrink_Stajer
                {
                    Id_Stajer = contextStajer.Id_Stajer,
                    Number_Lesson = contextLesson.Number_Lesson,
                    Done = false,
                    Rating = 1
                };

                await NetManager.Post("api/LessonDrink_Stajer/Add", lessonDrink_Stajer);

                // Создаем экземпляр HttpClient
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync("http://localhost:50401/api/LessonDrink_Stajer/LastInsertedId");
                    int lastInsertedId = 0;
                    if (response.IsSuccessStatusCode)
                    {
                        // Читаем содержимое ответа как строку
                        string content = await response.Content.ReadAsStringAsync();
                        // Преобразуем содержимое в целое число
                        if (int.TryParse(content, out lastInsertedId))
                        {
                            lessonDrink_Stajer.Number = lastInsertedId;
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
            await DBConnection.RefreshData();
        }
        private async void DoneCB_Checked(object sender, RoutedEventArgs e)
        {
            if (contextLesson != null)
            {
                var lesson = DBConnection.lessons_Stajer.FirstOrDefault(ls => ls.Number_Lesson == contextLesson.Number_Lesson && ls.Id_Stajer == contextStajer.Id_Stajer);
                if (lesson != null)
                {
                    lesson.Done = true;
                    await NetManager.Put("api/LessonDrink_Stajer/Edit", lesson);
                }
                else
                {
                    LessonDrink_Stajer lessonDrink_Stajer = new LessonDrink_Stajer
                    {
                        Id_Stajer = contextStajer.Id_Stajer,
                        Number_Lesson = contextLesson.Number_Lesson,
                        Done = true
                    };

                    await NetManager.Post("api/LessonDrink_Stajer/Add", lessonDrink_Stajer);

                    // Создаем экземпляр HttpClient
                    using (HttpClient httpClient = new HttpClient())
                    {
                        HttpResponseMessage response = await httpClient.GetAsync("http://localhost:50401/api/LessonDrink_Stajer/LastInsertedId");
                        int lastInsertedId = 0;
                        if (response.IsSuccessStatusCode)
                        {
                            // Читаем содержимое ответа как строку
                            string content = await response.Content.ReadAsStringAsync();
                            // Преобразуем содержимое в целое число
                            if (int.TryParse(content, out lastInsertedId))
                            {
                                lessonDrink_Stajer.Number = lastInsertedId;
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
                await DBConnection.RefreshData();
            }
            else
            {
                Lesson_Drink lesson = new Lesson_Drink
                {
                    Id_Menu = contextDrink.Id_Menu
                };

                await NetManager.Post("api/Lesson_Drink/Add", lesson);
                await DBConnection.RefreshEnums();

                // Создаем экземпляр HttpClient
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
                            lesson.Number_Lesson = lastInsertedId;
                            contextLesson = lesson;
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
                await DBConnection.RefreshEnums();
                Lesson();
            }
            DoneCB.IsChecked = true;


        }

        private async void DoneCB_Unchecked(object sender, RoutedEventArgs e)
        {
            if (contextLesson != null)
            {
                var lesson = DBConnection.lessons_Stajer.FirstOrDefault(ls => ls.Number_Lesson == contextLesson.Number_Lesson && ls.Id_Stajer == contextStajer.Id_Stajer);
                if (lesson != null)
                {
                    lesson.Done = false;
                    await NetManager.Put("api/LessonDrink_Stajer/Edit", lesson);
                }
                else
                {
                    LessonDrink_Stajer lessonDrink_Stajer = new LessonDrink_Stajer
                    {
                        Id_Stajer = contextStajer.Id_Stajer,
                        Number_Lesson = contextLesson.Number_Lesson,
                        Done = false
                    };

                    await NetManager.Post("api/LessonDrink_Stajer/Add", lessonDrink_Stajer);

                    // Создаем экземпляр HttpClient
                    using (HttpClient httpClient = new HttpClient())
                    {
                        HttpResponseMessage response = await httpClient.GetAsync("http://localhost:50401/api/LessonDrink_Stajer/LastInsertedId");
                        int lastInsertedId = 0;
                        if (response.IsSuccessStatusCode)
                        {
                            // Читаем содержимое ответа как строку
                            string content = await response.Content.ReadAsStringAsync();
                            // Преобразуем содержимое в целое число
                            if (int.TryParse(content, out lastInsertedId))
                            {
                                lessonDrink_Stajer.Number = lastInsertedId;
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
                await DBConnection.RefreshData();
            }
            else
            {
                Lesson_Drink lesson = new Lesson_Drink
                {
                    Id_Menu = contextDrink.Id_Menu
                };

                await NetManager.Post("api/Lesson_Drink/Add", lesson);

                // Создаем экземпляр HttpClient
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
                            lesson.Number_Lesson = lastInsertedId;
                            contextLesson = lesson;
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
                await DBConnection.RefreshEnums();
                LessonFalse();
            }
            DoneCB.IsChecked = false;
            BasicRatingBar.Value = 1;
           
        }

        private async void BasicRatingBar_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            // Получаем текущее значение рейтинга
            int newRating = e.NewValue;
            if(DoneCB.IsChecked == true)
            {
                var thisLesson = DBConnection.lessons_Stajer.FirstOrDefault(ls => ls.Id_Stajer == contextStajer.Id_Stajer && ls.Number_Lesson == contextLesson.Number_Lesson);
                if (thisLesson != null)
                {
                    thisLesson.Rating = newRating;
                    await NetManager.Put("api/LessonDrink_Stajer/Edit", thisLesson);
                }
            }
            else
            {
                BasicRatingBar.Value = 1;
            }
           
        }
    }
}
