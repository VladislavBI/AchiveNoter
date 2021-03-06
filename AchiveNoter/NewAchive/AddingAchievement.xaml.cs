﻿using System;
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
    /// Interaction logic for AddingAchievement.xaml
    /// </summary>
    public partial class AddingAchievement : Window
    {
        
        public AddingAchievement(string name)
        {
            InitializeComponent();
            TextBlockName.Text = name;
            
            ComboBoxFill();

            CreateDelegates();
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

            Theme th=ach.Themes.Where(t=>t.Name==ComboBoxTheme.SelectedValue.ToString()).FirstOrDefault();
            foreach (var item in ach.SubThemeRels)
            {
                if (item.Theme==th)
                {
                    ComboBoxSubtheme.Items.Add(item.Subtheme.Name);
                }
            }

            if (ComboBoxSubtheme.Items.Count > 0)
                ComboBoxSubtheme.SelectedIndex = 0;
        }

        /// <summary>
        /// Выбор изменяемого - тема/подтема
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ThemSubChooser ch = new ThemSubChooser();
            ch.ShowDialog();
            
        }

        /// <summary>
        /// Добавление нового достижения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (AchievmentsEntities ach = new AchievmentsEntities())
            {
                Password p=ach.Passwords.Where(t=>t.ID==App.curPnID).FirstOrDefault();
                Theme th = ach.Themes.Where(t => t.Name == ComboBoxTheme.SelectedValue.ToString()).FirstOrDefault();
                Subtheme sth=new Subtheme();
                try 
                { 
                  sth= ach.Subthemes.Where(t => t.Name == ComboBoxSubtheme.SelectedValue.ToString()).FirstOrDefault();
                }
                catch { }
                AchieveInfo achI = new AchieveInfo()
                {
                    Date = TextBlockDate.SelectedDate.Value.Date,
                    Points = (int)PointsSlider.Value,
                    Name = TextBlockName.Text,
                    Subscribe = TextBoxSubscr.Text,
                    Password = p,
                    Theme=th
                };
                if (sth != null)
                    achI.Subtheme = sth;
                
                ach.AchieveInfoes.Add(achI);
                ach.SaveChanges();

                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }
        }

        /// <summary>
        /// Изменение индекса CB темы - редактирование CB подтемы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ComboBoxTheme.SelectedIndex!=-1)
            using(AchievmentsEntities ach=new AchievmentsEntities())
	        {
                RefrefsSubTh(ach);
	        }
            
        }

        /// <summary>
        /// Возврат к окну названий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AchieveName an = new AchieveName(TextBlockName.Text);
            an.Show();
            this.Close();
        }
    }
}
