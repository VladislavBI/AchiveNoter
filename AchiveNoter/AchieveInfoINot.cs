using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchiveNoter
{
    /// <summary>
    /// Класс для привязки AchieveNoter к WPF окну
    /// </summary>
    class AchieveInfoINot: INotifyPropertyChanged
    {
        private System.DateTime Date;
        private int Points;
        private string Name;
        private string Subscribe;
        

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public System.DateTime aDate
        {
            get { return Date; }
            set
            {
                Date = value;
                RaisePropertyChanged("Date");
            }
        }
        public int aPoints
        {
            get { return Points; }
            set
            {
                Points = value;
                RaisePropertyChanged("Points");
            }
        }
        public string aName
        {
            get { return Name; }
            set
            {
                Name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string aSubscribe
        {
            get { return Subscribe; }
            set
            {
                Subscribe = value;
                RaisePropertyChanged("Subscribe");
            }
        }
       
    }
}
