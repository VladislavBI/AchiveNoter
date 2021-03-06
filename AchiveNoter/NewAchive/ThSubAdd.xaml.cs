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
using System.Data.SqlClient;

namespace AchiveNoter.NewAchive
{
    /// <summary>
    /// Interaction logic for ThSubAdd.xaml
    /// </summary>
    public partial class ThSubAdd : Window
    {
        bool theme;
        public ThSubAdd(bool theme)
        {
            InitializeComponent();

            this.theme = theme;
           
            if (theme)
            {
                this.LabelTheme.Visibility = Visibility.Hidden;
                this.ComboBoxTheme.Visibility = Visibility.Hidden;
                MainGrid.RowDefinitions.RemoveAt(2);
                TextBlockHeader.Text = "Добавить новую тему";
            }
            else
            {
                FillThemeCombobox();
            }
        }
        /// <summary>
        /// Заполнение тем
        /// </summary>
        void FillThemeCombobox()
        {
            //строка подключения
            

            //выборка имени из Theme
            using(SqlConnection con=new SqlConnection(App.GetConnectString()))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Name FROM Theme", con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ComboBoxTheme.Items.Add(reader[0].ToString());
                }
                ComboBoxTheme.SelectedIndex = 0;
            }
        }
        private void ButtonNO_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxName.Text == "")
            {
                MessageBox.Show("Имя не может быть пустым");
                return;
            }

            using (AchievmentsEntities ach = new AchievmentsEntities())
            {
                if (theme)
                    CreateNewTheme(ach);
                else
                    CreateNewSubTheme(ach);
            }
            if(DelegatesData.HandlerAchAddDelCBRefresf!=null)
                DelegatesData.HandlerAchAddDelCBRefresf();
            this.Close();
        }

        /// <summary>
        /// Полная операция создания новой темы
        /// </summary>
        void CreateNewTheme(AchievmentsEntities ach)
        {
           if(ach.Themes.Select(x=>x.Name==TextBoxName.Text).FirstOrDefault())
           {
               MessageBox.Show("Тема уже существует");
               return;
           }

           ach.Themes.Add(new Theme() { Name = TextBoxName.Text });
           ComboBoxTheme.Items.Add(TextBoxName.Text);
           ComboBoxTheme.SelectedIndex=0;
           CreateNewSubTheme(ach);

           ach.SaveChanges();
        }

        /// <summary>
        /// Полная операция создания новой подтемы
        /// </summary>
        void CreateNewSubTheme(AchievmentsEntities ach)
        {
            if (ach.SubThemeRels.Select(x => 
                x.Theme.Name == ComboBoxTheme.SelectedValue.ToString() 
                && x.Subtheme.Name == TextBoxName.Text).FirstOrDefault())
            {
                MessageBox.Show("Подтема уже существует в этой теме");
                return;
            }

            CreateManyToManyRel(ach);
        }

        /// <summary>
        /// Добавление подтемы и создание связи - многие ко многим
        /// </summary>
        /// <param name="ach"></param>
       void CreateManyToManyRel(AchievmentsEntities ach)
        {
            //выбор темы из бд
            Theme th = ach.Themes.Where(x => x.Name == ComboBoxTheme.SelectedValue.ToString()).FirstOrDefault();
             
           Subtheme st;

           //Если подтема есть, связать её еще одной темой
           if (ach.Subthemes.Select(t => t.Name.ToUpper().Equals(TextBoxName.Text.ToUpper())).FirstOrDefault())
               st = ach.Subthemes.Where(t => t.Name == TextBoxName.Text).FirstOrDefault();
           //если нет - создать новую
           else
           {
               st = new Subtheme() { Name = TextBoxName.Text };
               ach.Subthemes.Add(st);
           }
           
            

            //добавление связи к теме 
            ach.SubThemeRels.Add(
                new SubThemeRel() 
            { 
                Theme=th,
                Subtheme=st
            }
                );
            

            
            ach.SaveChanges();
        }

    }
}
