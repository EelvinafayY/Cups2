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
    /// Логика взаимодействия для MenuPageStajer.xaml
    /// </summary>
    public partial class MenuPageStajer : Page
    {
        Stajer contextStajer;
        public MenuPageStajer()
        {
            InitializeComponent();
            contextStajer = DBConnection.loginedStajer;
            this.DataContext = this;
        }

       

        private void MenuTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new MenuPage());
        }

        private void ProfileTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new StajerHomePage(contextStajer));
        }

        private void DrinksBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DrinksPageStajer());
        }

        private void EatBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EatPageStajer());
        }

        private void DesertsBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DesertsPageStajer());
        }
        private void ExitTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DBConnection.loginedStajer = null;
            NavigationService.Navigate(new AuthorizationPage());
        }

        private void TestTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new TestsPage(contextStajer));
        }
    }
}
