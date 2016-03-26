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
using System.Data;
using System.Data.Entity;

namespace AchiveNoter.Info
{
    /// <summary>
    /// Interaction logic for ThemeDetaiInfo.xaml
    /// </summary>
    public partial class ThemeDetaiInfo : Window
    {
        List<AchieveInfo> achI=new List<AchieveInfo>();
        bool theme;
        DataTable DTAch { get; set; }
        public ThemeDetaiInfo(dynamic th, bool theme)
        {
            InitializeComponent();
            SetName(th);
            this.theme = theme;

            if (!IsEmpty(th.ID)) 
            {
                ChangeHeader();
                CreateAchList(th);
                TableCreate();
                SetPoints();
                FillTable();
                FillDataContext();                    
            }
            
        }

        void SetName(dynamic t)
        {
            TBName.Text = t.Name;
        }

        bool IsEmpty(int ID)
        {
            using(AchievmentsEntities ach=new AchievmentsEntities())
	        {
                if (theme)
                {
                    if (ach.AchieveInfoes.Where(p => p.ThemeID == ID && p.PersonID == App.curPnID).Count() > 0)
                    {

                        return false;
                    }
                }
                else
                    if (ach.AchieveInfoes.Where(p => p.SubthemeID == ID && p.PersonID == App.curPnID).Count() > 0)
                    {
                        return false;
                    }


                return true;
	        }
            
        }

        void CreateAchList(Theme th)
        {
            AchievmentsEntities ach = new AchievmentsEntities();
            achI.AddRange(ach.AchieveInfoes.Where(p => p.ThemeID == th.ID && p.PersonID == App.curPnID).AsEnumerable());
        }
        void CreateAchList(Subtheme sTh)
        {
            AchievmentsEntities ach = new AchievmentsEntities();
            achI.AddRange(ach.AchieveInfoes.Where(p => p.SubthemeID == sTh.ID && p.PersonID == App.curPnID).AsEnumerable());
        }


        void TableCreate()
        {
            DTAch = new DataTable();
            DTAch.Columns.Add("Категория");
            DTAch.Columns.Add("Название");
            DTAch.Columns.Add("Очки", typeof(int));
        }
        void SetPoints()
        {
            int sum = achI.Sum(p => p.Points);
            TBlScore.Text = "Очки: " + sum.ToString();
        }

        void FillTable()
        {
            foreach (var item in achI)
            {
                DataRow row = DTAch.NewRow();

                if(theme)
                    row[0] = item.Subtheme.Name;
                else
                    row[0] = item.Theme.Name;

                row[1] = item.Name;
                row[2] = item.Points;

                DTAch.Rows.Add(row);
            }
        }

        void ChangeHeader()
        {
            if (!theme)
            {
                DataGridColumn dgCol = DGList.Columns.Where(p => p.Header.ToString() == "Подтема").FirstOrDefault();
                dgCol.Header = "Тема";
            }
        }
        void FillDataContext()
        {

            DGList.ItemsSource = DTAch.AsDataView();
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CategoriesList cl = new CategoriesList(theme);
            cl.Show();
            this.Close();
        }

        
    }


}
