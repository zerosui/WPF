using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;

namespace SmokeNote.Logic.Models
{
    public class NoteQueryCondition : NotificationObject
    {
        private string _keywords;

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords
        {
            get { return _keywords; }
            set
            {
                if (_keywords != value)
                {
                    _keywords = value;
                    this.RaisePropertyChanged("Keywords");
                }
            }
        }

        private Notebook _currentNotebook;

        /// <summary>
        /// 当前选中的笔记本
        /// </summary>
        public Notebook CurrentNotebook
        {
            get { return _currentNotebook; }
            set
            {
                if (_currentNotebook != value)
                {
                    _currentNotebook = value;
                    this.RaisePropertyChanged("CurrentNotebook");
                }
            }
        }

        private bool _isRecycle;

        /// <summary>
        /// 当前选中的是否是回收站
        /// </summary>
        public bool IsRecycle
        {
            get { return _isRecycle; }
            set
            {
                if (_isRecycle != value)
                {
                    _isRecycle = value;
                    this.RaisePropertyChanged("IsRecycle");
                }
            }
        }

        private DateRange _createDateCondition;

        public DateRange CreateDateCondition
        {
            get { return _createDateCondition; }
            set
            {
                if (_createDateCondition != value)
                {
                    _createDateCondition = value;
                    this.RaisePropertyChanged("CreateDateCondition");
                }
            }
        }

        private DateRange _modifyDateCondition;

        public DateRange ModifyDateCondition
        {
            get { return _modifyDateCondition; }
            set
            {
                if (_modifyDateCondition != value)
                {
                    _modifyDateCondition = value;
                    this.RaisePropertyChanged("ModifyDateCondition");
                }
            }
        }
    }
}
