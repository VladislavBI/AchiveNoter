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
using System.Windows.Shapes;

namespace AchiveNoter
{
    /// <summary>
    /// Interaction logic for HelloWindow.xaml
    /// </summary>
    public partial class HelloWindow : Window
    {
        public HelloWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            SigningIn si = new SigningIn();
            si.Show();
            this.Close();
        }
    }
}
