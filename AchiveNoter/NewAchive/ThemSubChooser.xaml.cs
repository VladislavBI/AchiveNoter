using AchiveNoter.NewAchive;
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
    /// Interaction logic for ThemSubChooser.xaml
    /// </summary>
    public partial class ThemSubChooser : Window
    {
        bool add = true;
        public ThemSubChooser()
        {
            InitializeComponent();
        }

        public ThemSubChooser(bool add)
        {
            InitializeComponent();
            this.add = add;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (DelegatesData.HandlerAchCloseMW != null)
                DelegatesData.HandlerAchCloseMW();
            if (add)
            {
                ThSubAdd th;
                if (b.Content.ToString() == "Тема")
                    th = new ThSubAdd(true);
                else
                    th = new ThSubAdd(false);
                th.Show();
                this.Close();
            }

            else
            {
                EditAch th;
                if (b.Content.ToString() == "Тема")
                    th = new EditAch(true);
                else
                    th = new EditAch(false);
                th.Show();
                this.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
