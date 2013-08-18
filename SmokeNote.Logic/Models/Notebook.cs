using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using SmokeNote.Logic.Enums;

namespace SmokeNote.Logic.Models
{
    public class Notebook : ModelBase
    {
        /// <summary>
        /// 代表默认笔记本ID
        /// </summary>
        public static readonly Guid DefaultNotebookID = new Guid("00000000-0000-0000-0000-000000000001");

        private string _name;

        /// <summary>
        /// 笔记本名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }

        private NoteBookType _type;

        /// <summary>
        /// 笔记本类型
        /// </summary>
        public NoteBookType Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    this.RaisePropertyChanged("Type");
                }
            }
        }

        private bool _isDefault;

        /// <summary>
        /// 是否是默认笔记本
        /// </summary>
        public bool IsDefault
        {
            get { return _isDefault; }
            set
            {
                if (_isDefault != value)
                {
                    _isDefault = value;
                    this.RaisePropertyChanged("IsDefault");
                }
            }
        }

        private int _notes;

        /// <summary>
        /// 日记数
        /// </summary>
        public int Notes
        {
            get { return _notes; }
            set
            {
                if (_notes != value)
                {
                    _notes = value;
                    this.RaisePropertyChanged("Notes");
                }
            }
        }
    }
}
