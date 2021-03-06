﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AchiveNoter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// ID текущего пользователя
        /// </summary>
        public static int curPnID = -1;
        /// <summary>
        /// Предотвращение создания нескольких иконок трея
        /// </summary>
        public static bool TrayIsOn = false;

        /// <summary>
        /// Получение строки подключение к БД
        /// </summary>
        /// <returns>Строка подключения</returns>
        public static string GetConnectString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.InitialCatalog = "Achievments";
            builder.IntegratedSecurity = true;
            builder.DataSource = "localhost";

            return builder.ConnectionString;
        }

        /// <summary>
        /// Название месяцев
        /// </summary>
        public static List<string> MonthName = new List<string>()
        {
        "",
        "Январь",
        "Февраль",
        "Март",
        "Апрель",
        "Май",
        "Июнь",
        "Июль",
        "Август",
        "Сентябрь",
        "Октябрь",
        "Ноябрь",
        "Декабрь",
        };
    }
}
