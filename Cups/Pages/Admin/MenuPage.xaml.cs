using Cups.Models;
using Cups.Services;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        Admin contextAdmin;
        public MenuPage()
        {
            InitializeComponent();
            contextAdmin = DBConnection.loginedAdmin;
            this.DataContext = this;
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

        private void DrinksBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DrinksPage());
        }

        private void EatBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EatPage());
        }

        private void DesertsBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DesertsPage());
        }
        private void ExitTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DBConnection.loginedAdmin = null;
            NavigationService.Navigate(new AuthorizationPage());
        }
    }
}
