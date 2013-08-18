using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;
using System.ComponentModel;

namespace SmokeNote.Client.Models
{
    class userCustom
    {

    }

    public partial class mycheck: INotifyPropertyChanged
    {
        //标记是否删除
        private bool isChecked = false;
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                NotifyPropertyChanged("IsChecked");
            }
        }
        public string ActionUser
        {
            get;
            set;
        }
        public int ActionId
        {
            get;
            set;
        }
        public string ActionName
        {
            get;
            set;
        }
        public string ActionRole
        {
            get;
            set;
        }
        public string ActionMoney
        {
            get;
            set;
        }
        public DateTime? ActionTime
        {
            get;
            set;
        }
        public string ActionCommet
        {
            get;
            set;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public partial class myaction
    {
        public string ActionUser
        {
            get;
            set;
        }
        public int ActionId
        {
            get;
            set;
        }
        public string ActionName
        {
            get;
            set;
        }
        public string ActionRole
        {
            get;
            set;
        }
        public string ActionMoney
        {
            get;
            set;
        }
        public DateTime? ActionTime
        {
            get;
            set;
        }
        public string ActionCommet
        {
            get;
            set;
        }
    }

    public partial class myaccount
    {

        public string ActionUser
        {
            get;
            set;
        }
        public string AcountTotal
        {
            get;
            set;
        }
        public int ActionId
        {
            get;
            set;
        }
        public string ActionName
        {
            get;
            set;
        }
        public string ActionRole
        {
            get;
            set;
        }
        public string ActionMoney
        {
            get;
            set;
        }
        public DateTime? ActionTime
        {
            get;
            set;
        }
        public string ActionCommet
        {
            get;
            set;
        }
    }
}