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

namespace AchiveNoter.Info
{
    /// <summary>
    /// Interaction logic for ThemeRatio.xaml
    /// </summary>
    public partial class ThemeRatio : Window
    {
        /// <summary>
        /// День или период
        /// </summary>
        bool day;
        public ThemeRatio(bool day)
        {
            InitializeComponent();
            this.day = day;
            var ach = GetFullAchList();
            if (day)
                TodayListCreate(ach);
            else
                FullPeriodCreate(ach);
        }

                /// <summary>
        /// Конструктор для периода
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public ThemeRatio(DateTime from, DateTime to) 
        {
            InitializeComponent();
            var ach = GetFullAchList();

           
            dpFrom.SelectedDate=from.Date;
            dpTo.SelectedDate = to.Date;

            Button_Click_1(this, new RoutedEventArgs());
        }
        /// <summary>
        /// Заполнение стартовой информации (не по достижениям)
        /// </summary>
        void FillingNP()
        {
            using (AchievmentsEntities ach = new AchievmentsEntities())
            {
                Password p = ach.Passwords.Where(x => x.ID == App.curPnID).FirstOrDefault();
                TBlInfo.Text += "Пользователь: " + p.Name+"\n";
                
            }
        }

         /// <summary>
        /// Выбор способа заполнения очков
        /// </summary>
        /// <param name="ach"></param>
        void FillingPoints(IQueryable<AchieveInfo> ach)
        {
            try
            {
                int sum = ach.Sum(p => p.Points);
                TBlInfo.Text += "Очки: " + sum.ToString()+"\n";
            }
            catch 
            {
                
               /*if(day)
               {

                   if (ExpTh.IsExpanded == false && ExpSTh.IsExpanded == false)
                {
                    MessageBox.Show("Сегодня нет достижений");
                    this.Close();
                }

                else 
                {
                    MessageBox.Show("Сегодня нет таких достижений");
                    ExpTh.IsExpanded = false;
                    ExpSTh.IsExpanded = false;
                    TodayListCreate(GetFullAchList());
                }  
               }
               else
               {
                   MessageBox.Show("Нет таких достижений");
                   ExpTh.IsExpanded = false;
                   ExpSTh.IsExpanded = false;
                   ExpBD.IsExpanded = false;
                   FullPeriodCreate(GetFullAchList());
               }*/
            }
            
        }

        /// <summary>
        /// Получить все достижения для текущего пользователя
        /// </summary>
        /// <returns></returns>
        IQueryable<AchieveInfo> GetFullAchList()
        {
            AchievmentsEntities ach = new AchievmentsEntities();
            var aa = ach.AchieveInfoes.Where(p => p.Password.ID == App.curPnID);
            return aa;

        }

        #region Создание и вывод списка достижений
        /// <summary>
        /// Создание списка достижений на сегодня
        /// </summary>
        void TodayListCreate(IQueryable<AchieveInfo> ach)
        {
            tblFr.Visibility = Visibility.Collapsed;
            tblTo.Visibility = Visibility.Collapsed;
            dpFrom.Visibility = Visibility.Collapsed;
            dpTo.Visibility = Visibility.Collapsed;

            MainGrid.RowDefinitions.RemoveAt(2);
            DateTime d=DateTime.Now.Date;
            ach = ach.Where(p => p.Date == d);
            DataMainPreparatons(ach);
        }

        /// <summary>
        /// Создание полного списка достижений
        /// </summary>
        void FullPeriodCreate(IQueryable<AchieveInfo> ach)
        {
            DataMainPreparatons(ach);
        }

        /// <summary>
        /// Выборка достижений в определенный период
        /// </summary>
        void PeriodDayCreate(IQueryable<AchieveInfo> ach)
        {
            DateTime d = dpFrom.SelectedDate.Value.Date;
            DateTime d2=dpTo.SelectedDate.Value.Date;

            ach = ach.Where(p => p.Date >= d && p.Date <= d2);
            DataMainPreparatons(ach);
        }

        /// <summary>
        /// Сортировка достижений, вывод основной информации по пользователю и переход на вывод рейтинга
        /// </summary>
        /// <param name="ach"></param>
        void DataMainPreparatons(IQueryable<AchieveInfo> ach) 
        {
            TBlInfo.Text = "";
            FillingNP();
            FillingPoints(ach);

            //Сортировка по темам дотижений
            var sortAch = ach.GroupBy(p => p.Theme.Name).
                Select(
                gnew => new 
                {
                    Name=gnew.FirstOrDefault().Theme.Name, 
                    Points=gnew.Sum(c=>c.Points) 
                });
            sortAch = sortAch.OrderByDescending(p => p.Points);
            FillingTextBlock(sortAch);
        }

        void FillingTextBlock(dynamic ach)
        {
            int i=1;
            foreach (var item in ach)
            {
                TBlInfo.Text += i.ToString() + ": " + item.Name + " " + item.Points + "\n";
                i++;
            }
        }
        #endregion



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
           
            
                var ach=GetFullAchList();
            //если от меньше до
                if (dpTo.SelectedDate < dpFrom.SelectedDate)
                    FullPeriodCreate(ach);
                else
                    PeriodDayCreate(ach);
            
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
                    var ach = GetFullAchList();
                    FullPeriodCreate(ach);      
        }

        
    }
}

