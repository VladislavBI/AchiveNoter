using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Interaction logic for UserInfo.xaml
    /// </summary>
    public partial class UserInfo : Window
    {
        public UserInfo()
        {
            InitializeComponent();
            BaseCreation();
           
            
        }

        #region Создание новой инфы по пользователю

        /// <summary>
        /// весь процесс заполнения информации
        /// </summary>
        void BaseCreation()
        {
            using (SqlConnection con = new SqlConnection(App.GetConnectString()))
            {
                con.Open();
                //Имя пользователя
                SqlCommand cmd = new SqlCommand(string.Format("SELECT Name FROM Password WHERE ID={0}", App.curPnID), con);
                TBlName.Text = cmd.ExecuteScalar().ToString();

                //Всего очков
                cmd = new SqlCommand(string.Format("SELECT SUM(Points) FROM AchieveInfo WHERE PersonID={0}", App.curPnID), con);
                TBlPAll.Text = "Всего: "+cmd.ExecuteScalar().ToString();

                //Очки месяц
                cmd = new SqlCommand(string.Format("SELECT SUM(Points) FROM AchieveInfo WHERE PersonID={0} AND Date Between convert(varchar(6), getdate(), 112) + '01' and dateadd(day, -1, dateadd(month, 1, convert(varchar(6), getdate(), 112) + '01'))", App.curPnID), con);
                TBlPMnt.Text = App.MonthName[DateTime.Now.Month]+": " + cmd.ExecuteScalar().ToString();

                //Лучший/худший результат всего
                cmd = new SqlCommand(string.Format("SELECT DISTINCT(p.Name) , Sum(Points) FROM AchieveInfo ac LEFT OUTER JOIN Theme p ON ac.ThemeID=p.ID WHERE PersonID={0} GROUP BY p.Name", App.curPnID), con);
                TBlBSAll.Text = "Всего: "+FindMax(cmd.ExecuteReader());
                TBlWSAll.Text = "Всего: "+FindMin(cmd.ExecuteReader());

                //Лучший/худший результат - месяц
                cmd = new SqlCommand(string.Format("SELECT DISTINCT(p.Name) , Sum(Points) FROM AchieveInfo ac LEFT OUTER JOIN Theme p ON ac.ThemeID=p.ID WHERE PersonID={0} AND Date Between convert(varchar(6), getdate(), 112) + '01' and dateadd(day, -1, dateadd(month, 1, convert(varchar(6), getdate(), 112) + '01')) GROUP BY p.Name", App.curPnID), con);
                TBlBSMnt.Text = App.MonthName[DateTime.Now.Month] + ": " + FindMax(cmd.ExecuteReader());
                TBlWSMnt.Text = App.MonthName[DateTime.Now.Month] + ": " + FindMin(cmd.ExecuteReader());
            }
        }

        /// <summary>
        /// Находит максимальное значение выборки
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        string FindMax(SqlDataReader reader)
        {
            int max = -1;
            string name = "";
            while (reader.Read())
            {
                if (Convert.ToInt32(reader[1]) > max)
                {
                    max = Convert.ToInt32(reader[1]);
                    name = reader[0].ToString();
                }                  
            }
            reader.Close();
            return name + ": " + max.ToString();
        }
        /// <summary>
        /// Находит минимальное значение выборки
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        string FindMin(SqlDataReader reader)
        {
            reader.Read();
            int min = Convert.ToInt32(reader[1]);
            string name = reader[0].ToString();
            while (reader.Read())
            {
                if (Convert.ToInt32(reader[1]) < min)
                {
                    min = Convert.ToInt32(reader[1]);
                    name = reader[0].ToString();
                }
            }
            reader.Close();
            return name + ": " + min.ToString();
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void btnTargets_Click(object sender, RoutedEventArgs e)
        {
            SignIn.Edit ed = new AchiveNoter.SignIn.Edit();
            ed.ShowDialog();
        }
    }
}
