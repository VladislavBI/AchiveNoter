﻿using AchiveNoter.Info;
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

namespace AchiveNoter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AchieveName ac = new AchieveName();
            ac.Show();
            this.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            UserInfo us = new UserInfo();
            us.ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            DayInfo di = new DayInfo(true);
            //для обхода ошибки с отсутствием достижений
            try
            {
                di.Show();
            }
            catch { }
            this.Close();
           
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            DayInfo di = new DayInfo(false);
            //для обхода ошибки с отсутствием достижений
            try
            {
                di.Show();
            }
            catch { }
            this.Close();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            Filter f = new Filter();
            f.Show();
            this.Close();
        }
    }
}
