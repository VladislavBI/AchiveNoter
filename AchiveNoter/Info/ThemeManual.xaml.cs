using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    /// Interaction logic for ThemeManual.xaml
    /// </summary>
    public partial class ThemeManual : Window
    {
       
        public ThemeManual()
        {
            InitializeComponent();
            GetThemeList();
        }

        void GetThemeList()
        {
            using (AchievmentsEntities ach=new AchievmentsEntities())
            {
                
                ach.Themes.Load();
                var themes = ach.Themes.Local;
                 
                LVTheme.ItemsSource=themes.AsEnumerable();
                
            }
            if (LVTheme.Items.Count!=0)
	        {
                LVTheme.SelectedIndex = 0;
                ChangeViews();
	        }
            
        }

        private void LVTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeViews();
        }

        void ChangeViews()
        {
            Theme th = LVTheme.SelectedItem as Theme;
            UpdateLVSubTh(th);
        }
        void UpdateLVSubTh(Theme th)
        {
            using (AchievmentsEntities ach=new AchievmentsEntities())
            {
                List<Subtheme> sTh = new List<Subtheme>();

                var sThR=ach.SubThemeRels.Where(p => p.ThemeID == th.ID).ToList();

                sTh.AddRange(sThR.Select(p => p.Subtheme));
                LVSubTh.ItemsSource = sTh;
            }
           
          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ThemeDetaiInfo td = new ThemeDetaiInfo((Theme)LVTheme.SelectedItem, false);
            td.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
