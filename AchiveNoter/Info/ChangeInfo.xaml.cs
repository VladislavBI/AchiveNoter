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
    /// Interaction logic for ChangeInfo.xaml
    /// </summary>
    public partial class ChangeInfo : Window
    {
        AchieveInfoINot achI;
        AchieveInfo ai;
        public ChangeInfo(AchieveInfo ai)
        {
            this.ai=ai;
            achI = new AchieveInfoINot()
            {
                aName = ai.Name,
                aDate = ai.Date,
                aPoints = ai.Points,
                aSubscribe = ai.Subscribe,
            };
            InitializeComponent();
            CreateDelegates();
            ComboBoxFill();
            DataContext = achI;

            
            ComboBoxTheme.SelectedValue = ai.Theme.Name;
            ComboBoxSubtheme.SelectedValue=ai.Subtheme.Name;
            
        }

        /// <summary>
        /// Создание делегатов для окна
        /// </summary>
        void CreateDelegates()
        {
            DelegatesData.HandlerAchAddDelCBRefresf = new DelegatesData.AchAddDelCBRefresf(ComboBoxFill);
        }

        /// <summary>
        /// Обновление ComboBox - тем и подтем
        /// </summary>
        void ComboBoxFill()
        {
            ComboBoxTheme.Items.Clear();


            using (AchievmentsEntities ach = new AchievmentsEntities())
            {
                foreach (var item in ach.Themes)
                {
                    ComboBoxTheme.Items.Add(item.Name);
                }
                ComboBoxTheme.SelectedIndex = 0;


            }

        }

        /// <summary>
        /// Обновление ComboBox - подтем
        /// </summary>
        /// <param name="ach">Соединение с бд</param>
        void RefrefsSubTh(AchievmentsEntities ach)
        {
            ComboBoxSubtheme.Items.Clear();

            Theme th = ach.Themes.Where(t => t.Name == ComboBoxTheme.SelectedValue.ToString()).FirstOrDefault();
            foreach (var item in ach.SubThemeRels)
            {
                if (item.Theme == th)
                {
                    ComboBoxSubtheme.Items.Add(item.Subtheme.Name);
                }
            }

            if (ComboBoxSubtheme.Items.Count > 0)
                ComboBoxSubtheme.SelectedIndex = 0;
        }

        private void ComboBoxTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxTheme.SelectedIndex != -1)
                using (AchievmentsEntities ach = new AchievmentsEntities())
                {
                    RefrefsSubTh(ach);
                }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ThemSubChooser ch = new ThemSubChooser();
            ch.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ai.Date = achI.aDate;
            ai.Name = achI.aName;
            ai.Points = achI.aPoints;
            ai.Subscribe = achI.aSubscribe;

            using (AchievmentsEntities ach=new AchievmentsEntities())
            {
                ai.Theme = ach.Themes.Where(p => p.Name == ComboBoxTheme.SelectedValue).FirstOrDefault();
                ai.Subtheme = ach.Subthemes.Where(p => p.Name == ComboBoxSubtheme.SelectedValue).FirstOrDefault();

                AchieveInfo temp= ach.AchieveInfoes.Find(ai.ID);
                temp.Date = ai.Date;
                temp.Name = ai.Name;
                temp.Points = ai.Points;
                temp.Subscribe = ai.Subscribe;                
                temp.Subtheme = ai.Subtheme;
                temp.Theme = ai.Theme;

                ach.SaveChanges();
            }
            Button_Click_2(this, new RoutedEventArgs());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            WindowDetailedInfo wdi = new WindowDetailedInfo(ai);
            this.Close();
            wdi.Show();
        }


    }
}
