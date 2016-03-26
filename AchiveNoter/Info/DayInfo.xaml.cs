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
       // List<Ahcts> achie { get; set; }
        DataTable achievesInfo {get;set;}
        /// <summary>
        /// День или период
        /// </summary>
        bool day;
        /// <summary>
        /// Открывается окно детальной информации (чтобы не открывалось главное окно)  
        /// </summary>
        bool detailedInfo = false;
        public DayInfo(bool day)
        {
            InitializeComponent();
            
            achITableCreate();
            
            
            this.day = day;
            FillingNP();

            


            var ach = GetFullAchList();
                if (day)
                    TodayListCreate(ach);
                else
                    FullPeriodCreate(ach);
            
            
        }




        /// <summary>
        /// Констурктор для периода
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public DayInfo(DateTime from, DateTime to) 
        {
            InitializeComponent();
            FillingNP();
            achITableCreate();
            var ach = GetFullAchList();

            ExpBD.IsExpanded = true;
            dpFrom.Text=from.Date.ToString();
            dpTo.Text = to.ToString() ;

            Button_Click_1(this, new RoutedEventArgs());
        }

        /// <summary>
        /// создание таблицы для заполнение в Datagrid
        /// </summary>
        void achITableCreate()
        {
            achievesInfo = new DataTable();
            achievesInfo.Columns.Add("Дата", typeof(DateTime));
            achievesInfo.Columns.Add("Тема");
            achievesInfo.Columns.Add("Название");
            achievesInfo.Columns.Add("Очки", typeof(int));

            
        }

        /// <summary>
        /// Получить все достижения для текущего пользователя
        /// </summary>
        /// <returns></returns>
        IQueryable<AchieveInfo> GetFullAchList()
        {
                AchievmentsEntities ach = new AchievmentsEntities();
                var aa=ach.AchieveInfoes.Where(p=>p.Password.ID==App.curPnID);
                return aa;
            
        }
        /// <summary>
        /// Заполнение стартовой информации (не таблицы)
        /// </summary>
        void FillingNP()
        {
            using (AchievmentsEntities ach = new AchievmentsEntities())
            {
                Password p = ach.Passwords.Where(x => x.ID == App.curPnID).FirstOrDefault();
                tblName.Text ="Пользователь: "+ p.Name;
                ComboBoxFill();
            }
        }

        void ComboBoxFill()
        {
            cbTheme.Items.Clear();
            using (AchievmentsEntities ach = new AchievmentsEntities())
            {
                foreach (var item in ach.Themes)
                {
                    cbTheme.Items.Add(item.Name);
                }
                cbTheme.SelectedIndex = 0;

                RefrefsSubTh(ach);
            }

        }

        /// <summary>
        /// Обновление ComboBox - подтем
        /// </summary>
        /// <param name="ach">Соединение с бд</param>
        void RefrefsSubTh(AchievmentsEntities ach)
        {
            cbSubtheme.Items.Clear();

            Theme th = ach.Themes.Where(t => t.Name == cbTheme.SelectedValue.ToString()).FirstOrDefault();
            foreach (var item in ach.SubThemeRels)
            {
                if (item.Theme == th)
                {
                    cbSubtheme.Items.Add(item.Subtheme.Name);
                }
            }

            if (cbSubtheme.Items.Count > 0)
                cbSubtheme.SelectedIndex = 0;
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
                tblPoints.Text = "Очки: " + sum.ToString();
            }
            catch 
            {
                
               if(day)
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
               }
            }
            
        }

  
        #region создание таблицы
        /// <summary>
        /// Создание таблицы на сегодня
        /// </summary>
        void TodayListCreate(IQueryable<AchieveInfo> ach)
        {
            ExpBD.Visibility = Visibility.Collapsed;
            tblFr.Visibility = Visibility.Collapsed;
            tblTo.Visibility = Visibility.Collapsed;
            dpFrom.Visibility = Visibility.Collapsed;
            dpTo.Visibility = Visibility.Collapsed;
            DateTime d=DateTime.Now.Date;
            ach = ach.Where(p => p.Date == d);
            TableToDGBinding(ach);
        }

        /// <summary>
        /// Создание полной таблицы достижений
        /// </summary>
        void FullPeriodCreate(IQueryable<AchieveInfo> ach)
        {
            TableToDGBinding(ach);
        }

        /// <summary>
        /// Создание DataGrid достижений в определенный период
        /// </summary>
        void PeriodDayCreate(IQueryable<AchieveInfo> ach)
        {
            DateTime d = dpFrom.SelectedDate.Value.Date;
            DateTime d2=dpTo.SelectedDate.Value.Date;

            ach = ach.Where(p => p.Date >= d && p.Date <= d2);
            TableToDGBinding(ach);
        }

        /// <summary>
        /// Привязка полученных данных к DataGrid
        /// </summary>
        /// <param name="ach"></param>
        void TableToDGBinding(IQueryable<AchieveInfo> ach) 
        {
            achievesInfo.Rows.Clear();
            foreach (var item in ach)
            {
                DataRow row = achievesInfo.NewRow();
                row[0] = item.Date;
                row[1] = item.Theme.Name;
                row[2] = item.Name;
                row[3] = item.Points;
                achievesInfo.Rows.Add(row);
            }
            FillingPoints(ach);
           // DGInfo.Items.Refresh();
            DGInfo.ItemsSource=achievesInfo.AsDataView();
        }
        #endregion


        #region Применение comboboxов
        IQueryable<AchieveInfo> GetSelectedTheme(IQueryable<AchieveInfo> ach)
        {
            return ach.Where(a => a.Theme.Name == cbTheme.SelectedValue.ToString());
        }

        IQueryable<AchieveInfo> GetSelectedSubTheme(IQueryable<AchieveInfo> ach)
        {
            return ach.Where(a =>a.Subtheme.Name == cbSubtheme.SelectedValue.ToString());
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
                if (ExpTh.IsExpanded == true)
                    ach = GetSelectedTheme(ach);
                if (ExpSTh.IsExpanded == true)
                    ach = GetSelectedSubTheme(ach);


                if (ExpBD.IsExpanded == true)
                    PeriodDayCreate(ach);

                else
                {
                    if (day)
                        TodayListCreate(ach);
                    else
                        FullPeriodCreate(ach);
                }
            
        }

        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!detailedInfo)
            {
                MainWindow mw = new MainWindow();
                mw.Show();
            }
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
                    var ach = GetFullAchList();
                    if (day)
                        TodayListCreate(ach);
                    else
                        FullPeriodCreate(ach);      
        }

        private void cbTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using(AchievmentsEntities ach=new AchievmentsEntities())
	        {
                RefrefsSubTh(ach);
	        }
          
        }

        private void DGInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataRowView dRowView = (DataRowView)DGInfo.SelectedItems[0];
                DataRow dRow = dRowView.Row;
                string cellContent = dRow[2].ToString();

                using (AchievmentsEntities ach = new AchievmentsEntities())
                {
                    AchieveInfo aI = GetFullAchList().Where(p => p.Name == cellContent).FirstOrDefault();
                    WindowDetailedInfo wdi = new WindowDetailedInfo(aI);
                    detailedInfo = true;
                    this.Close();
                    wdi.Show();
                }

            }
            catch { }
        }

        
    }

   
}
