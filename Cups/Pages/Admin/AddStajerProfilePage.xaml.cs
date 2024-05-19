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

namespace Cups.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddStajerProfilePage.xaml
    /// </summary>
    public partial class AddStajerProfilePage : Page
    {
        Admin contextAdmin;
        public static List<Stajer> stajers {  get; set; }
        public static List<Gender> genders { get; set; }
        Stajer stajer = new Stajer();
        
        public AddStajerProfilePage()
        {
            InitializeComponent();
            contextAdmin = DBConnection.loginedAdmin;
            AddStajer();
            Refresh();
            
            genders = new List<Gender>(DBConnection.genders.ToList());
            this.DataContext = this;
        }

        public void Refresh()
        {
            DataContext = stajer;
            genders = new List<Gender>(DBConnection.genders.ToList());
            GenderCB.SelectedIndex = stajer.Id_Gender - 1;
            this.DataContext = this;
            byte[] photoBytes = stajer.Photo;

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
            
        }
        public async void AddStajer()
        {

            var a = stajer;
            a.Id_Gender = 3;
            a.Id_Admin = contextAdmin.Id_admin;
            a.Id_Role = 2;
           
            
            await NetManager.Post("api/Stajer/Add", a);

            // Создаем экземпляр HttpClient
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync("http://localhost:50401/api/Stajer/LastInsertedId");
                int lastInsertedId = 0;
                if (response.IsSuccessStatusCode)
                {
                    // Читаем содержимое ответа как строку
                    string content = await response.Content.ReadAsStringAsync();
                    // Преобразуем содержимое в целое число
                    if (int.TryParse(content, out lastInsertedId))
                    {
                        a.Id_Stajer = lastInsertedId;
                        stajer = a;
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

        private async void SaveBT_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                // Проверяем, что все поля заполнены
                if (string.IsNullOrWhiteSpace(SurnameTB.Text) ||
                    string.IsNullOrWhiteSpace(NameTB.Text) ||
                    string.IsNullOrWhiteSpace(PatronymicTB.Text) ||
                    GenderCB.SelectedItem == null ||
                    DateOfBirthdayDP.SelectedDate == null ||
                    
                    string.IsNullOrWhiteSpace(LoginTB.Text) ||
                    string.IsNullOrWhiteSpace(PasswordTB.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;

                }
                if(PhoneTB.Text.Length < 14)
                {
                    MessageBox.Show("Номер телефона заполнен неккоректно!");
                    return;
                }
                if(LoginTB.Text.Length < 3)
                {
                    MessageBox.Show("Логин заполнен некорректно!");
                    return;
                }
                if(PasswordTB.Text.Length < 5)
                {
                    MessageBox.Show("Пароль заполнен неккоректно!");
                    return;
                }
               

                stajer.Surname = SurnameTB.Text.Trim();
                stajer.Name = NameTB.Text.Trim();
                stajer.Patronymic = PatronymicTB.Text.Trim();
                var b = GenderCB.SelectedItem as Gender;
                stajer.Id_Gender = b.Id_Gender;
                stajer.BirthDay = (DateTime)DateOfBirthdayDP.SelectedDate;
                stajer.DateOfRegistr = DateTime.Today;
                stajer.Id_Admin = contextAdmin.Id_admin;
                stajer.Id_Role = 2;
                stajer.Login = LoginTB.Text;
                stajer.Password = PasswordTB.Text;

                var existingStajerWithSameLogin = DBConnection.stajers.FirstOrDefault(s => s.Login == stajer.Login && s.Id_Stajer != stajer.Id_Stajer);
                var existingAdminWithSameLogin = DBConnection.admins.FirstOrDefault(a => a.Login == stajer.Login);

                if (existingStajerWithSameLogin != null || existingAdminWithSameLogin != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует. Пожалуйста, выберите другой логин.");
                    return;
                }

                var response = await NetManager.Put("api/Stajer/Edit", stajer);
                response.EnsureSuccessStatusCode();
                await DBConnection.RefreshData();
                
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

        private async void AddPhotoBT_Click(object sender, RoutedEventArgs e)
        {
            
            var dialog = new OpenFileDialog() { Filter = ".png, .jpg, .jpeg| *.png; *.jpg; *.jpeg" };
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                genders = new List<Gender>(DBConnection.genders.ToList());
                var b = GenderCB.SelectedItem as Gender;
                if (b != null)
                {
                    stajer.Id_Gender = b.Id_Gender;
                    await NetManager.Put("api/Stajer/Edit", stajer);
                    await DBConnection.RefreshData();
                }
                var image = File.ReadAllBytes(dialog.FileName);
                stajer.Photo = image;
                DataContext = null;
                DataContext = stajer;
            }
            await NetManager.Put("api/Stajer/Edit", stajer);

            Refresh();
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

        private void PhoneTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            // Удаляем все ненужные символы
            string rawText = new string(textBox.Text.Where(char.IsDigit).ToArray());

            // Проверяем длину номера и начинаем форматирование
            if (rawText.Length > 0)
            {
 
                rawText = "+7" + rawText.Substring(1);


                // Форматируем текст
                if (rawText.Length > 1)
                {
                    rawText = rawText.Insert(2, "(");
                    if (rawText.Length > 5)
                    {
                        rawText = rawText.Insert(6, ")");
                    }
                }
            }

            // Присваиваем отформатированный текст обратно в TextBox
            textBox.Text = rawText;

            // Перемещаем курсор в конец текста
            textBox.SelectionStart = textBox.Text.Length;
            if (PhoneTB.Text.Length > 14)
            {
                PhoneTB.Text = PhoneTB.Text.Substring(0, 14);
                PhoneTB.CaretIndex = PhoneTB.Text.Length; // Перемещаем курсор в конец текста
            }
        }
        private void LoginTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LoginTB.Text.Length > 3)
            {
                LoginTB.Text = LoginTB.Text.Substring(0, 3);
                LoginTB.CaretIndex = LoginTB.Text.Length; // Перемещаем курсор в конец текста
            }
        }
        private void PasswordTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PasswordTB.Text.Length > 5)
            {
                PasswordTB.Text = PasswordTB.Text.Substring(0, 5);
                PasswordTB.CaretIndex = PasswordTB.Text.Length; // Перемещаем курсор в конец текста
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DateOfBirthdayDP.DisplayDateEnd = DateTime.Today.AddYears(-18).AddDays(-1);

        }
        private async void BackBT_Click(object sender, RoutedEventArgs e)
        {
            await DBConnection.RefreshEnums();
            var a = DBConnection.stajers.FirstOrDefault(s => s.Id_Stajer == stajer.Id_Stajer && stajer.Name != null);
            if(stajer.Surname != SurnameTB.Text || stajer.Name != NameTB.Text || stajer.Patronymic != PatronymicTB.Text ||
               stajer.BirthDay != (DateTime)DateOfBirthdayDP.SelectedDate || stajer.Id_Gender - 1 != GenderCB.SelectedIndex ||
               stajer.Login != LoginTB.Text || stajer.Password != PasswordTB.Text)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите продолжить? Данные не будут сохранены", "Убедитесь, что вы все заполнили и сохранили", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if(a == null)
                    {
                        var st = stajer;
                        await NetManager.Delete<bool>($"api/Stajer/Delete/{st.Id_Stajer}");
                        await DBConnection.RefreshEnums();
                        await Services.DBConnection.InitializeDBConnection();
                        NavigationService.Navigate(new ViewStajersPage(contextAdmin));
                    }
                    else
                    {
                        NavigationService.Navigate(new ViewStajersPage(contextAdmin));
                    }
                    
                }
                if (result == MessageBoxResult.No)
                {

                }
            }
            else
            {
                NavigationService.Navigate(new ViewStajersPage(contextAdmin));
            }


        }

       
    }
}
