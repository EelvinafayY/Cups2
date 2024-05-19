using Cups.Models;
using Cups.Services;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для TestsPage.xaml
    /// </summary>
    public partial class TestsPage : Page
    {
        Stajer contextStajer;
        public Tests contextTest = new Tests();
        public TestsPage(Stajer stajer)
        {
            InitializeComponent();
            contextStajer = stajer;
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (contextStajer != null)
            {
                var a = DBConnection.tests.FirstOrDefault(t => t.Id_Stajer == contextStajer.Id_Stajer && t.Name == "Тест 1");
                if(a != null)
                {
                    MessageBox.Show("Вы уже прошли данный тест!");
                }
                else
                {
                    a = contextTest;
                    
                    a.Name = "Тест 1";
                    a.Id_Stajer = contextStajer.Id_Stajer;
                    a.Points = 0;
                    await NetManager.Post("api/Tests/Add", a);
                    await DBConnection.RefreshData();

                    // Создаем экземпляр HttpClient
                    using (HttpClient httpClient = new HttpClient())
                    {
                        HttpResponseMessage response = await httpClient.GetAsync("http://localhost:50401/api/Tests/LastInsertedId");
                        int lastInsertedId = 0;
                        if (response.IsSuccessStatusCode)
                        {
                            // Читаем содержимое ответа как строку
                            string content = await response.Content.ReadAsStringAsync();
                            // Преобразуем содержимое в целое число
                            if (int.TryParse(content, out lastInsertedId))
                            {
                                a.Number = lastInsertedId;
                                contextTest = a;
                                await DBConnection.RefreshData();
                                
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
                    NavigationService.Navigate(new Test1.Query1Page(contextStajer));
                }
            }
            
        }

        private async void TestTwo_Click(object sender, RoutedEventArgs e)
        {
            if (contextStajer != null)
            {
                var a = DBConnection.tests.FirstOrDefault(t => t.Id_Stajer == contextStajer.Id_Stajer && t.Name == "Тест 2");
                if (a != null)
                {
                    MessageBox.Show("Вы уже прошли данный тест!");
                }
                else
                {
                    a = contextTest;

                    a.Name = "Тест 2";
                    a.Id_Stajer = contextStajer.Id_Stajer;
                    a.Points = 0;
                    await NetManager.Post("api/Tests/Add", a);

                    // Создаем экземпляр HttpClient
                    using (HttpClient httpClient = new HttpClient())
                    {
                        HttpResponseMessage response = await httpClient.GetAsync("http://localhost:50401/api/Tests/LastInsertedId");
                        int lastInsertedId = 0;
                        if (response.IsSuccessStatusCode)
                        {
                            // Читаем содержимое ответа как строку
                            string content = await response.Content.ReadAsStringAsync();
                            // Преобразуем содержимое в целое число
                            if (int.TryParse(content, out lastInsertedId))
                            {
                                a.Number = lastInsertedId;
                                contextTest = a;
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
                    NavigationService.Navigate(new Test1.Query1Page(contextStajer));
                }
            }
        }

        private async void TestThree_Click(object sender, RoutedEventArgs e)
        {
            if (contextStajer != null)
            {
                var a = DBConnection.tests.FirstOrDefault(t => t.Id_Stajer == contextStajer.Id_Stajer && t.Name == "Тест 3");
                if (a != null)
                {
                    MessageBox.Show("Вы уже прошли данный тест!");
                }
                else
                {
                    a = contextTest;

                    a.Name = "Тест 3";
                    a.Id_Stajer = contextStajer.Id_Stajer;
                    a.Points = 0;
                    await NetManager.Post("api/Tests/Add", a);

                    // Создаем экземпляр HttpClient
                    using (HttpClient httpClient = new HttpClient())
                    {
                        HttpResponseMessage response = await httpClient.GetAsync("http://localhost:50401/api/Tests/LastInsertedId");
                        int lastInsertedId = 0;
                        if (response.IsSuccessStatusCode)
                        {
                            // Читаем содержимое ответа как строку
                            string content = await response.Content.ReadAsStringAsync();
                            // Преобразуем содержимое в целое число
                            if (int.TryParse(content, out lastInsertedId))
                            {
                                a.Number = lastInsertedId;
                                contextTest = a;
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
                    NavigationService.Navigate(new Test1.Query1Page(contextStajer));
                }
            }
        }

        private async void ItogTest_Click(object sender, RoutedEventArgs e)
        {
            if (contextStajer != null)
            {
                var a = DBConnection.tests.FirstOrDefault(t => t.Id_Stajer == contextStajer.Id_Stajer && t.Name == "Итоговый тест");
                if (a != null)
                {
                    MessageBox.Show("Вы уже прошли данный тест!");
                }
                else
                {
                    a = contextTest;

                    a.Name = "Итоговый тест";
                    a.Id_Stajer = contextStajer.Id_Stajer;
                    a.Points = 0;
                    await NetManager.Post("api/Tests/Add", a);

                    // Создаем экземпляр HttpClient
                    using (HttpClient httpClient = new HttpClient())
                    {
                        HttpResponseMessage response = await httpClient.GetAsync("http://localhost:50401/api/Tests/LastInsertedId");
                        int lastInsertedId = 0;
                        if (response.IsSuccessStatusCode)
                        {
                            // Читаем содержимое ответа как строку
                            string content = await response.Content.ReadAsStringAsync();
                            // Преобразуем содержимое в целое число
                            if (int.TryParse(content, out lastInsertedId))
                            {
                                a.Number = lastInsertedId;
                                contextTest = a;
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
                    NavigationService.Navigate(new Test1.Query1Page(contextStajer));
                }
            }
        }
    }
}
