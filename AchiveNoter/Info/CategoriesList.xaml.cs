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
using System.Data.Entity;

namespace AchiveNoter.Info
{
    /// <summary>
    /// Interaction logic for CategoriesList.xaml
    /// </summary>
    public partial class CategoriesList : Window
    {
        bool th;
        public CategoriesList(bool theme)
        {
            InitializeComponent();
            th = theme;
            CreateCategoryList();
            ChangeTextBox();
        }

        void CreateCategoryList()
        {
            using (AchievmentsEntities ach = new AchievmentsEntities())
            {
                if (th)
                {
                    ach.Themes.Load();
                    var themes = ach.Themes.Local;
                    LVCat.ItemsSource = themes.AsEnumerable();
                }

                else
                {
                    ach.Subthemes.Load();
                    var subThemes = ach.Subthemes.Local;
                    LVCat.ItemsSource = subThemes.AsEnumerable();
                }
                LVCat.SelectedIndex = 0;
            }
        }

        void ChangeTextBox()
        {
            TBlCat.Text = th ? "Темы" : "Подтемы";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ThemeDetaiInfo td=th?new ThemeDetaiInfo((Theme)LVCat.SelectedItem, true):new ThemeDetaiInfo((Subtheme)LVCat.SelectedItem, false);
            td.Show();
        }
    }
}
