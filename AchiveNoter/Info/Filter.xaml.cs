﻿using System;
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
using System.Windows.Shapes;

namespace AchiveNoter.Info
{
    /// <summary>
    /// Interaction logic for Filter.xaml
    /// </summary>
    public partial class Filter : Window
    {
        public Filter()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DayInfo di;
            if (From.SelectedDate.Value > To.SelectedDate.Value)
            {
                di = new DayInfo(To.SelectedDate.Value, To.SelectedDate.Value);
            }
            else
            {
                di = new DayInfo(From.SelectedDate.Value, To.SelectedDate.Value);
            }

            di.Show();
            this.Close();
        }
    }
}