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
                SqlCommand cmd = new SqlCommand(string.Format("SELECT Name FROM Password WHERE ID={0}", App.curPnID), con);
                TBlName.Text = cmd.ExecuteScalar().ToString();

                cmd = new SqlCommand(string.Format("SELECT SUM(Points) FROM AchieveInfo WHERE PersonID={0}", App.curPnID), con);
                TBlPAll.Text = "Всего: "+cmd.ExecuteScalar().ToString();

                cmd = new SqlCommand(string.Format("SELECT SUM(Points) FROM AchieveInfo WHERE PersonID={0} AND Date Between convert(varchar(6), getdate(), 112) + '01' and dateadd(day, -1, dateadd(month, 1, convert(varchar(6), getdate(), 112) + '01'))", App.curPnID), con);
                TBlPMnt.Text = DateTime.Now.Month.ToString()+": " + cmd.ExecuteScalar().ToString();
            }
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
