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
    /// Логика взаимодействия для ViewDrinkAdmin.xaml
    /// </summary>
    public partial class ViewDrinkAdmin : Page
    {
        Admin contextAdmin;
        public static List<Models.Menu> menu {  get; set; }
        Models.Menu contextDrink;
        public static List<Lesson_Drink> lessons { get; set; }
        Lesson_Drink contextLesson;
        public ViewDrinkAdmin(Models.Menu drink)
        {
            InitializeComponent();
            contextAdmin = DBConnection.loginedAdmin;
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
            if(contextDrink.Compound != null)
            {
                string sostav = contextDrink.Compound;
                string[] ingredients = sostav.Split(',');
                string newSostav = string.Join("\n- ", ingredients);
                SostavTB.Text = newSostav;
            }
            if(contextDrink.Volume != null)
            {
                VolumeTB.Text = "Объем - " + contextDrink.Volume + " мл";
            }
            contextLesson = DBConnection.lesson_Drinks.FirstOrDefault(l => l.Id_Menu == contextDrink.Id_Menu);
            if(contextLesson != null)
            {
                var steps = new List<Step>(DBConnection.steps.Where(s => s.Number_Lesson == contextLesson.Number_Lesson).ToList());
                DrinkStepsLV.ItemsSource = steps;
                int averageRating = (int)CalculateAverageRating(contextDrink.Id_Menu);
                BasicRatingBar.Value = averageRating;
            }
            else
            {
                BasicRatingBar.Value = 1;
            }
           
            this.DataContext = this;
        }
        public double CalculateAverageRating(int idMenu)
        {
            // Выбираем все записи из таблицы LessonDrink_Stajer, где Done == true и Id_Menu == idMenu
            var relevantLessons = DBConnection.lessons_Stajer
                                              .Where(lesson => lesson.Done && lesson.Lesson_Drink.Id_Menu == idMenu)
                                              .ToList();

            // Если нет записей, удовлетворяющих условию, вернуть 0
            if (relevantLessons.Count == 0)
               
                return 1;

            // Считаем сумму всех значений Rating
            double totalRating = relevantLessons.Sum(lesson => lesson.Rating);

            // Считаем количество записей
            int numberOfLessons = relevantLessons.Count;

            // Вычисляем среднее значение Rating
            double averageRating = totalRating / numberOfLessons;

            return averageRating;
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

        private void BackBT_Click(object sender, RoutedEventArgs e)
        {
            var typeName = contextDrink.Type.Name;
            if (typeName != null)
            {
                NavigationService.Navigate(new ListDrinksPage(typeName));
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

        private async void DeletePhotoBT_Click(object sender, RoutedEventArgs e)
        {
            var drink = contextDrink;
            if(drink != null)
            {
                drink.Photo = null;
                var response = await NetManager.Put("api/Menu/Edit", drink);
                response.EnsureSuccessStatusCode();
                contextDrink = drink;
                Refresh();
            }
            else
            {
                MessageBox.Show("Фото отсутствует!");
            }
            
        }
        private void ExitTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DBConnection.loginedAdmin = null;
            NavigationService.Navigate(new AuthorizationPage());
        }
    }
}
