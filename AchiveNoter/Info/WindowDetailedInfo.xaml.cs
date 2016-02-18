using AchiveNoter.Info;
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

namespace AchiveNoter
{
    /// <summary>
    /// Interaction logic for WindowDetailedInfo.xaml
    /// </summary>
    public partial class WindowDetailedInfo : Window
    {
        AchieveInfo achI;
        public WindowDetailedInfo(AchieveInfo aI)
        {
            InitializeComponent();

            TBlAchName.Text = aI.Name;
            TBlDateName.Text = aI.Date.Date.ToShortDateString();
            TBlPointsName.Text = aI.Points.ToString();
            TBlThemeName.Text = aI.Theme.Name;
            TBlSubThemeName.Text = aI.Subtheme.Name;
            TBlUserName.Text = aI.Password.Name;
            TBlSummary.Text = aI.Subscribe;

            achI = aI;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeInfo ci = new ChangeInfo(achI);
            this.Close();
            ci.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DayInfo di = new DayInfo(false);
            //для обхода ошибки с отсутствием достижений
            try
            {
                di.Show();
            }
            catch { }
            this.Close();
        }
    }
}
