﻿using Cups.Models;
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
    /// Логика взаимодействия для ListOtherMenuStajer.xaml
    /// </summary>
    public partial class ListOtherMenuStajer : Page
    {
        public static List<Type> types { get; set; }
        public Cups.Models.Type contextType;
        public static List<Models.Menu> menus { get; set; }
        Stajer contextStajer;
        public ListOtherMenuStajer(string typeName)
        {
            InitializeComponent();
            contextStajer = DBConnection.loginedStajer;
            var a = DBConnection.types.FirstOrDefault(t => t.Name == typeName);
            if (a != null)
            {
                contextType = a; // Присваиваем сам объект типа Type
            }
            SetUp();
        }

        private async void SetUp()
        {
            await Services.DBConnection.InitializeDBConnection();
            types = DBConnection.types.ToList();
            menus = DBConnection.menu.ToList();
            menus = menus.Where(m => m.Kod_Type == contextType.Kod_Type).ToList();
            NameTB.Text = contextType.Name;
            DrinksLV.ItemsSource = menus;
            this.DataContext = this;
        }

        private void MenuTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new MenuPageStajer());
        }

        private void ProfileTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new StajerHomePage(contextStajer));
        }

        private void BackBT_Click(object sender, RoutedEventArgs e)
        {
            if (contextType.Kod_Type == 7 || contextType.Kod_Type == 8 || contextType.Kod_Type == 9 ||
                contextType.Kod_Type == 10 || contextType.Kod_Type == 11 || contextType.Kod_Type == 12)
            {
                NavigationService.Navigate(new EatPageStajer());
            }
            else
            {
                NavigationService.Navigate(new DesertsPageStajer());
            }
        }

        private void TestsTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new TestsPage(contextStajer));
        }
        private void ExitTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DBConnection.loginedStajer = null;
            NavigationService.Navigate(new AuthorizationPage());
        }
        

        private void ViewPositionBT_Click(object sender, RoutedEventArgs e)
        {
            var drink = (sender as Button).DataContext as Models.Menu;
            if (drink != null)
            {
                NavigationService.Navigate(new ViewPositionStajerPage(drink));
            }
        }
    }
}
