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
    /// Логика взаимодействия для DesertsPageStajer.xaml
    /// </summary>
    public partial class DesertsPageStajer : Page
    {
        Stajer contextStajer;
        public DesertsPageStajer()
        {
            InitializeComponent();
            contextStajer = DBConnection.loginedStajer;
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
            NavigationService.Navigate(new MenuPageStajer());
        }

        private void MussBT_Click(object sender, RoutedEventArgs e)
        {
            var typeName = (sender as Button)?.Tag as string;
            if (typeName != null)
            {
                NavigationService.Navigate(new ListOtherMenuStajer(typeName));
            }
        }
    }
}
