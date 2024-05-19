using Cups.Models;
using Cups.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
            
        }

        private void ComeBT_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTB.Text.Trim();
            string password = PasswordPB.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            var admins = DBConnection.admins.ToList();
            Admin currentAdmin = admins.FirstOrDefault(i => i.Login == login && i.Password == password);
            Services.DBConnection.loginedAdmin = currentAdmin;

            if (currentAdmin != null)
            {
                NavigationService.Navigate(new AdminHomePage(currentAdmin));
                return;
            }

            var stajers = DBConnection.stajers.ToList();
            Stajer currentStager = stajers.FirstOrDefault(i => i.Login == login && i.Password == password);
            Services.DBConnection.loginedStajer = currentStager;

            if (currentStager != null)
            {
                NavigationService.Navigate(new StajerHomePage(currentStager));
            }
            else
            {
                MessageBox.Show("Пользователь не найден. Пожалуйста, проверьте правильность введенных данных.");
            }
        }
    }
}
