using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for DayInfo.xaml
    /// </summary>
    public partial class DayInfo : Window
    {
        DataTable achievesInfo;
        bool day;
        public DayInfo(bool day)
        {
            InitializeComponent();
            this.day = day;
            FillingNP();

            if (day)
                TodayListCreate();
            else
                PeriodDayCreate();
        }

        void FillingNP()
        {
            using (AchievmentsEntities ach = new AchievmentsEntities())
            {
                Password p = ach.Passwords.Where(x => x.ID == App.curPnID).FirstOrDefault();
                tblName.Text = p.Name;
                FillingPoints(ach);
            }
        }
        
        /// <summary>
        /// Заполнение очков
        /// </summary>
        /// <param name="ach"></param>
        void FillingPoints(AchievmentsEntities ach)
        {
            int sum = 0;
            DateTime d;
            //Если только за сегодня
            if (day)
            {
                //приведение к виду даты  в 00:00 для LINQ
                d = DateTime.Now.Date;
                sum = ach.AchieveInfoes.Where
                    (a => a.Date == d && a.PersonID == App.curPnID).Sum(a => a.Points);
            }

                //если за период
            else
            {
                //приведение к виду даты в 00:00 для LINQ
                d = dpFrom.SelectedDate.Value.Date;
                DateTime d2 = dpTo.SelectedDate.Value.Date;

                sum = ach.AchieveInfoes.Where
                     (a => a.Date >= d && a.Date <= d2 && a.PersonID == App.curPnID).Sum(a => a.Points);
            }

            tblPoints.Text = sum.ToString();
        }


        /// <summary>
        /// Создание таблицы на сегодня
        /// </summary>
        void TodayListCreate() 
        {
            using (SqlConnection con = new SqlConnection(App.GetConnectString()))
            {
                con.Open();
                achievesInfo = new DataTable();
                //Команда для получение всех достижений для конкретного пользователя
                SqlCommand cmd =
                    new SqlCommand(
                        string.Format("SELECT ac.Date AS 'Дата', th.Name AS 'Тема', ac.Name AS 'Название',ac.Points AS 'Очки' FROM AchieveInfo ac LEFT OUTER JOIN Theme th ON ac.ThemeID=th.ID JOIN Password p ON ac.PersonID=p.ID WHERE p.ID={0} AND ac.Date=@Dta", App.curPnID), con);
                cmd.Parameters.Add("Dta", SqlDbType.Date).Value = DateTime.Now;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(achievesInfo);


                DGInfo.ItemsSource = achievesInfo.DefaultView;
            }
        }

        void PeriodDayCreate() 
        {
            using (SqlConnection con = new SqlConnection(App.GetConnectString()))
            {
                con.Open();
                achievesInfo = new DataTable();
                //Команда для получение всех достижений для конкретного пользователя
                SqlCommand cmd =
                    new SqlCommand(
                        string.Format("SELECT ac.Date AS 'Дата', th.Name AS 'Тема', ac.Name AS 'Название',ac.Points AS 'Очки' FROM AchieveInfo ac LEFT OUTER JOIN Theme th ON ac.ThemeID=th.ID JOIN Password p ON ac.PersonID=p.ID WHERE p.ID={0} AND ac.Date BETWEEN @Dta AND @Dta2", App.curPnID), con);
                cmd.Parameters.Add("Dta", SqlDbType.Date).Value = dpFrom.SelectedDate.Value.Date;
                cmd.Parameters.Add("Dta2", SqlDbType.Date).Value = dpTo.SelectedDate.Value.Date;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(achievesInfo);


                DGInfo.ItemsSource = achievesInfo.DefaultView;
            }
        }
        
        /// <summary>
        /// Выход
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();

            
            

        }

        /// <summary>
        /// Применить фильтр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PeriodDayCreate();
            using (AchievmentsEntities ach = new AchievmentsEntities())
            {
                FillingPoints(ach);
            }
        }
    }
}
