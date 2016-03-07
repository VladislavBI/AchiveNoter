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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AchiveNoter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AchieveName ac = new AchieveName();
            ac.Show();
            this.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            UserInfo us = new UserInfo();
            us.ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            DayInfo di = new DayInfo(true);
            //для обхода ошибки с отсутствием достижений
            try
            {
                di.Show();
            }
            catch { }
            this.Close();
           
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
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

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            Filter f = new Filter();
            f.Show();
            this.Close();
        }



        #region Создание отображения в трее
       

        private void Window_StateChanged(object sender, EventArgs e)
        {
            base.OnStateChanged(e); // системная обработка
            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                // если окно минимизировали, просто спрячем
                Hide();
                // и поменяем надпись на менюшке
                (TrayMenu.Items[0] as MenuItem).Header = "Show";
            }
            else
            {
                // в противном случае запомним текущее состояние
                CurrentWindowState = WindowState;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            createTrayIcon(); // создание нашей иконки
            
            if (!closed) 
            {
                closed = true;
                base.OnClosing(e); // встроенная обработка
            }
            if (!CanClose)
            {    // если нельзя закрывать
                e.Cancel = true; //выставляем флаг отмены закрытия
                // запоминаем текущее состояние окна
                CurrentWindowState = this.WindowState;
                // меняем надпись в менюшке
                (TrayMenu.Items[0] as MenuItem).Header = "Отобразить";
                // прячем окно
                Hide();
                closed = false;
            }
            else
            { // все-таки закрываемся
                // убираем иконку из трея
                TrayIcon.Visible = false;
            }
        }


        /// <summary>
        /// Класс для иконкм
        /// </summary>
        private System.Windows.Forms.NotifyIcon TrayIcon = null;
        /// <summary>
        /// Класс для контекстного меню трея - ссылка на XAML
        /// </summary>
        private ContextMenu TrayMenu = null;

        private WindowState fCurrentWindowState = WindowState.Normal;
        /// <summary>
        /// Сохранение состояния окна, возврат окна в норм состояние в момент показа
        /// </summary>
        public WindowState CurrentWindowState
        {
            get { return fCurrentWindowState; }
            set { fCurrentWindowState = value; }
        }
       
        private bool fCanClose = false;
        /// <summary>
        /// флаг, позволяющий или запрещающий выход из приложения
        /// </summary>
        public bool CanClose
        {  
            get { return fCanClose; }
            set { fCanClose = value; }
        }
        /// <summary>
        /// Для выхода из беск цикла закрытия
        /// </summary>
        bool closed = false;

        /// <summary>
        /// Создает новую иконку
        /// </summary>
        /// <returns></returns>
        private bool createTrayIcon()
        {
            bool result = false;
            if (TrayIcon == null)
            { // только если мы не создали иконку ранее
                TrayIcon = new System.Windows.Forms.NotifyIcon(); // создаем новую
                TrayIcon.Icon = AchiveNoter.Properties.Resources.TrayIcon__2_;// изображение для трея
                // обратите внимание, за ресурсом с картинкой мы лезем в свойства проекта, а не окна,
                // поэтому нужно указать полный namespace
                TrayIcon.Text = "AchiveNoter"; // текст подсказки, всплывающей над иконкой
                TrayMenu = Resources["TrayMenu"] as ContextMenu; // а здесь уже ресурсы окна и тот самый x:Key

                // сразу же опишем поведение при щелчке мыши, о котором мы говорили ранее
                // это будет просто анонимная функция, незачем выносить ее в класс окна
                TrayIcon.Click += delegate(object sender, EventArgs e)
                {
                    if ((e as System.Windows.Forms.MouseEventArgs).Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        // по левой кнопке показываем или прячем окно
                        ShowHideMainWindow(sender, null);
                    }
                    else
                    {
                        // по правой кнопке (и всем остальным) показываем меню
                        TrayMenu.IsOpen = true;
                        Activate(); // отдать окну фокус
                    }
                };
                result = true;
            }
            else
            { // все переменные были созданы ранее
                result = true;
            }
            TrayIcon.Visible = true; // делаем иконку видимой в трее
            return result;
        }

        /// <summary>
        /// Функция показа или скрытия главного окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowHideMainWindow(object sender, RoutedEventArgs e)
        {
            TrayMenu.IsOpen = false; // спрячем менюшку, если она вдруг видима
            if (IsVisible)
            {// если окно видно на экране
                // прячем его
                Hide();
                // меняем надпись на пункте меню
                (TrayMenu.Items[0] as MenuItem).Header = "Отобразить";
            }
            else
            { // а если не видно
                // показываем
                Show();
                // меняем надпись на пункте меню
                (TrayMenu.Items[0] as MenuItem).Header = "Спрятать";
                WindowState = CurrentWindowState;
                Activate(); // обязательно нужно отдать фокус окну,
                // иначе пользователь сильно удивится, когда увидит окно
                // но не сможет в него ничего ввести с клавиатуры
            }
        }

        private void MenuExitClick(object sender, RoutedEventArgs e)
        {
            CanClose = true;
            Close();
        }
        

        #endregion

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            MenuItem m=sender as MenuItem;
            ThemeRatio th;
                switch (m.Header.ToString())
	            {
                    case "Сегодня":
                        th = new ThemeRatio(true);
                        //для обхода ошибки с отсутствием достижений
                        try
                        {
                            th.Show();
                        }
                        catch { }
                        this.Close();
                        break;

                    case "Все время":
                        th = new ThemeRatio(false);
                        //для обхода ошибки с отсутствием достижений
                        try
                        {
                            th.Show();
                        }
                        catch { }
                        this.Close();
                        break;

                    case "Период":
                        Filter f = new Filter();
                        f.Show();
                        this.Close();
                        break;

		            default:
                        break;
	            }
            
        }

        

        


    }
}
