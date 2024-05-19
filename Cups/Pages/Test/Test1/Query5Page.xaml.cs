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
using Cups.Pages.Test;

namespace Cups.Pages.Test.Test1
{
    /// <summary>
    /// Логика взаимодействия для Query5Page.xaml
    /// </summary>
    public partial class Query5Page : Page
    {
        public static List<Stajer> stajers { get; set; }
        Stajer contextStajer;
        public Query5Page(Stajer stajer)
        {
            InitializeComponent();
            contextStajer = stajer;
            this.DataContext = this;

        }
        private async void RightCb_Checked(object sender, RoutedEventArgs e)
        {

            var a = DBConnection.tests.FirstOrDefault(t => t.Id_Stajer == contextStajer.Id_Stajer && t.Name == "Тест 1");

            a.Points = a.Points + 1;
            await NetManager.Put("api/Tests/Edit", a);
            await DBConnection.RefreshData();
            if(a.Points <= 2)
            {
                MessageBox.Show("К сожалению, результат тестирования плохой. Вы набрали " + a.Points + " баллов");
                NavigationService.Navigate(new TestsPage(contextStajer));

            }
            else
            {
                MessageBox.Show("Поздравляем! Вы прошли тест и набрали " + a.Points + " баллов!");
                NavigationService.Navigate(new TestsPage(contextStajer));

            }

        }

        private async void UnRightCb_Checked(object sender, RoutedEventArgs e)
        {
            var a = DBConnection.tests.FirstOrDefault(t => t.Id_Stajer == contextStajer.Id_Stajer && t.Name == "Тест 1");

            a.Points = a.Points;
            await NetManager.Put("api/Tests/Edit", a);
            await DBConnection.RefreshData();
            if (a.Points <= 2)
            {
                MessageBox.Show("К сожалению, результат тестирования плохой. Вы набрали " + a.Points + " баллов");
                NavigationService.Navigate(new TestsPage(contextStajer));
            }
            else
            {
                MessageBox.Show("Поздравляем! Вы прошли тест и набрали " + a.Points + " баллов!");
                NavigationService.Navigate(new TestsPage(contextStajer));

            }

        }
    }
}
