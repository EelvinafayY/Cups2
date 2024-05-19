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
using Type = Cups.Models.Type;

namespace Cups.Pages
{
    /// <summary>
    /// Логика взаимодействия для DrinksPage.xaml
    /// </summary>
    public partial class DrinksPage : Page
    {
        Admin contextAdmin;
        public static List<Type> types { get; set; }
        public DrinksPage()
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

        private void SpecialBT_Click(object sender, RoutedEventArgs e)
        {
            var typeName = (sender as Button)?.Tag as string;
            if (typeName != null)
            {
                NavigationService.Navigate(new ListDrinksPage(typeName));
            }
        }

        private void BackBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MenuPage());
        }
        private void ExitTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DBConnection.loginedAdmin = null;
            NavigationService.Navigate(new AuthorizationPage());
        }
    }
}
