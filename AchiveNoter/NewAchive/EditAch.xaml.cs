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

namespace AchiveNoter.NewAchive
{
    /// <summary>
    /// Interaction logic for EditAch.xaml
    /// </summary>
    public partial class EditAch : Window
    {
        bool th;
       
        public EditAch(bool theme)
        {
            InitializeComponent();
            th = theme;
            if (th)
            {
                BDel.IsEnabled = true;
            }
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
            TBlHeader.Text += th ? " тем" : " подтем";
        }

        private void LVCat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LVCat.SelectedItem == null)
                LVCat.SelectedIndex = 0;
            if(th)
            {
                Theme cat = LVCat.SelectedItem as Theme;
                TBName.Text = cat.Name;
            }      
            
            else
            {
                Subtheme cat = LVCat.SelectedItem as Subtheme;
                TBName.Text = cat.Name;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (th)
            {
                using (AchievmentsEntities ach = new AchievmentsEntities())
                {
                    Theme thm = ach.Themes.Find((LVCat.SelectedItem as Theme).ID);

                   
                    thm.Name = TBName.Text;
                    ach.SaveChanges();

                } 
            }
            else
            {
                using (AchievmentsEntities ach = new AchievmentsEntities())
                {
                    Subtheme sthm = ach.Subthemes.Find((LVCat.SelectedItem as Subtheme).ID);

                    List<string> sl=new List<string>();
                    foreach (var item in ach.Themes)
                    {
                        sl.Add(item.Name);
                    }
                    if (sl.Contains(sthm.Name))
                    {
                        MessageBox.Show("Изменение основной подтемы невозможно!");
                        return;
                    }
                    sthm.Name = TBName.Text;
                    ach.SaveChanges();

                } 
            }
            CreateCategoryList();
            
        }

        private void BDel_Click(object sender, RoutedEventArgs e)
        {
            using (AchievmentsEntities ach = new AchievmentsEntities())
            {
                Subtheme sthm = ach.Subthemes.Find((LVCat.SelectedItem as Subtheme).ID);

                List<string> sl = new List<string>();
                foreach (var item in ach.Themes)
                {
                    sl.Add(item.Name);
                }
                if (sl.Contains(sthm.Name))
                {
                    MessageBox.Show("Изменение основной подтемы невозможно!");
                    return;
                }
                ach.Subthemes.Remove(sthm);
                ach.SaveChanges();

                
            }
            CreateCategoryList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }

        
    }
}
