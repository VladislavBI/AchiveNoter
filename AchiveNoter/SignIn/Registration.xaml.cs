using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(PassBoxP.Password!=PassBoxPAg.Password)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }
            
            MD5 md = new MD5CryptoServiceProvider();
            byte[] bt = Encoding.UTF8.GetBytes(PassBoxP.Password);

            using (AchievmentsEntities ach = new AchievmentsEntities())
            {
                
                if (ach.Passwords.Select(t => t.Name == TextBoxName.Text).FirstOrDefault())
                {
                    MessageBox.Show("Такое имя уже занято");
                    return;
                }

                Password p = new Password()
                {
                    Name = TextBoxName.Text,
                    Password1 = bt
                };
                ach.Passwords.Add(p);
                ach.SaveChanges();
            }

            this.Close();
        }
    }
}
