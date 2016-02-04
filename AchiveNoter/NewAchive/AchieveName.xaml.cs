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
    /// Interaction logic for AchieveName.xaml
    /// </summary>
    public partial class AchieveName : Window
    {
        public AchieveName()
        {
            InitializeComponent();
        }

        public AchieveName(string name)
        {
            InitializeComponent();
            TextBoxName.Text = name;
        } 

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxName.Text != "")
            {
                AddingAchievement add = new AddingAchievement(TextBoxName.Text);
                add.Show();
                this.Close();
            }
            else
                MessageBox.Show("Достижение не может быть безымянным");

        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw=new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
