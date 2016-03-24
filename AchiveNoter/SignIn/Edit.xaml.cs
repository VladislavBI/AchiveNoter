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

namespace AchiveNoter.SignIn
{
    /// <summary>
    /// Interaction logic for Edit.xaml
    /// </summary>
    public partial class Edit : Window
    {

        bool oldPasOk = false;
        Password user;
        public Edit()
        {
            InitializeComponent();
            using (AchievmentsEntities ach=new AchievmentsEntities())
            {

                user = ach.Passwords.Find(App.curPnID);
                TBLog.Text = user.Name.Clone().ToString();
               
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!oldPasOk)
            {
                CheckOldPass();
                oldPasOk = true;
                BCheck.Content = "Изменить";
            }
            else
            {
                CreateNewPass();
                this.Close();
            }
        }

        void CheckOldPass()
        {
            MD5 md = new MD5CryptoServiceProvider();
            byte[] bt = Encoding.UTF8.GetBytes(PBOld.Password);


            using (AchievmentsEntities ach = new AchievmentsEntities())
            {
                if (bt.SequenceEqual(user.Password1))
                {
                    PBOld.IsEnabled = false;
                    TBLog.IsEnabled = true;
                    PBNP.IsEnabled = true;
                    PBRNP.IsEnabled = true;
                    
                    MessageBox.Show("Правильный пароль");
                }
                else
                    MessageBox.Show("Неправильный пароль");
            }
        }

        void CreateNewPass()
        {
            if (PBNP.Password!=PBRNP.Password)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }
            using (AchievmentsEntities ach=new AchievmentsEntities())
            {
                user = ach.Passwords.Find(App.curPnID);
                if (user.Name != TBLog.Text)
                    user.Name = TBLog.Text;

                MD5 md = new MD5CryptoServiceProvider();
                byte[] bt = Encoding.UTF8.GetBytes(PBNP.Password);

                user.Password1 = bt;
                ach.SaveChanges();
            }
        }
    }
}
