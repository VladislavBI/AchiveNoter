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
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.InitialCatalog = "Achievments";
            builder.IntegratedSecurity = true;
            builder.DataSource = "localhost";

            using(SqlConnection con=new SqlConnection(builder.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Name FROM Theme", con);
                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    ComboBoxTheme.Items.Add(reader[0].ToString());
                }
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
           ach.SaveChanges();
        }

        /// <summary>
        /// Полная операция создания новой подтемы
        /// </summary>
        void CreateNewSubTheme(AchievmentsEntities ach)
        {
            if (ach.Subthemes.Select(x => x.Name == TextBoxName.Text).FirstOrDefault())
            {
                MessageBox.Show("Подтема уже существует");
                return;
            }

            //CreateManyToManyRel(ach);
        }

        /// <summary>
        /// Добавление подтемы и создание связи - многие ко многим
        /// </summary>
        /// <param name="ach"></param>
        /*void CreateManyToManyRel(AchievmentsEntities ach)
        {
            //выбор темы из бд
            Theme th = ach.Themes.Where(x => x.Name == ComboBoxTheme.SelectedValue.ToString()).FirstOrDefault();
            
            //Добавлление подтемы, определение связи с темой
            Subtheme st = new Subtheme() { Name = TextBoxName.Text };
            st.Themes.Add(th);
            

            //добавление связи к теме 
            ach.Themes.Where(x => x.Name == ComboBoxTheme.SelectedValue.ToString()).FirstOrDefault().Subthemes.Add(st);
            ach.Subthemes.Add(st);

            
            ach.SaveChanges();
        }*/

    }
}
