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
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MD5 md = new MD5CryptoServiceProvider();

            using (AchievmentsEntities ach = new AchievmentsEntities())
            {
                Password p = ach.Passwords.Where(x => x.Name == TextBoxName.Text).FirstOrDefault();
                byte[]b= Encoding.UTF8.GetBytes(PassBoxP.Password);
                if (p != null)
                    if (b.SequenceEqual(p.Password1))
                    {
                        App.curPnID = p.ID;
                        MainWindow mw = new MainWindow();
                        mw.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Неверный пароль");
                    }
                else
                    MessageBox.Show("Такой пользователь не зарегистрирован");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Registration reg = new Registration();
            reg.ShowDialog();
        }

        private void TextBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
