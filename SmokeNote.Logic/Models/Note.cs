using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmokeNote.Logic.Models
{
    public class Note : ModelBase
    {
        private Guid _notebookID;

        public Guid NotebookID
        {
            get { return _notebookID; }
            set
            {
                if (_notebookID != value)
                {
                    _notebookID = value;
                    this.RaisePropertyChanged("NotebookID");
                }
            }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    this.RaisePropertyChanged("Title");
                }
            }
        }

        private string _author;

        public string Author
        {
            get { return _author; }
            set
            {
                if (_author != value)
                {
                    _author = value;
                    this.RaisePropertyChanged("Author");
                }
            }
        }

        private string _from;

        public string From
        {
            get { return _from; }
            set
            {
                if (_from != value)
                {
                    _from = value;
                    this.RaisePropertyChanged("From");
                }
            }
        }

        private string _content;

        public string Content
        {
            get { return _content; }
            set
            {
                if (_content != value)
                {
                    _content = value;
                    this.RaisePropertyChanged("Content");
                }
            }
        }

        private string _tags;

        public string Tags
        {
            get { return _tags; }
            set
            {
                if (_tags != value)
                {
                    _tags = value;
                    this.RaisePropertyChanged("Tags");
                }
            }
        }

        private bool _isDelete;

        /// <summary>
        /// 删除标记
        /// </summary>
        public bool IsDelete
        {
            get { return _isDelete; }
            set
            {
                if (_isDelete != value)
                {
                    _isDelete = value;
                    this.RaisePropertyChanged("IsDelete");
                }
            }
        }
    }
}
