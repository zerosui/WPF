using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Common.ViewModel;
using Microsoft.Practices.Prism.Commands;
using SmokeNote.Logic.IService;
using SmokeNote.Logic.Models;
using Microsoft.Practices.Unity;

namespace SmokeNote.Client.ViewModels
{
    public class NotebookViewModel : WindowViewModelBase
    {
        #region 构造器

        public NotebookViewModel()
            : base(Consts.WindowNames.ModifyNotebook)
        {
            this.Title = "新建笔记本";
            this.Type = Logic.Enums.NoteBookType.Local;

            if (!this.IsInDesignMode)
            {
                this.NotebookService = this.UnityContainer.Resolve<INotebookService>();
                this.SubscribeEvent<Events.AddNoteEventArgs>(this.OnAddNote);
                this.SubscribeEvent<Events.MoveNoteEventArgs>(this.OnMoveNote);
                this.SubscribeEvent<Events.DeleteNoteEventArgs>(this.OnDeleteNote);
                this.SubscribeEvent<Events.RestoreNoteEvent>(this.OnRestoreNote);
            }
        }

        public NotebookViewModel(Notebook notebook)
            : this()
        {
            this.Notebook = notebook;
            this.Title = "修改笔记本";
            this.Type = notebook.Type;
            this.Name = notebook.Name;
            this.Notes = notebook.Notes;
        }

        #endregion

        #region 属性

        private Notebook _notebook;

        /// <summary>
        /// 笔记本实体
        /// </summary>
        public Notebook Notebook
        {
            get { return _notebook; }
            set
            {
                if (_notebook != value)
                {
                    _notebook = value;
                    this.RaisePropertyChanged("Notebook", "IsDefault");
                }
            }
        }

        private bool _isChecked;

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    this.RaisePropertyChanged("IsChecked");
                }
            }
        }

        private string _name;

        /// <summary>
        /// 编辑的名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    this.RaisePropertyChanged("Name", "CanSubmit");
                }
            }
        }

        private Logic.Enums.NoteBookType _type;

        /// <summary>
        /// 编辑的类型
        /// </summary>
        public Logic.Enums.NoteBookType Type
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

        /// <summary>
        /// 是否允许编辑类型
        /// </summary>
        public bool IsDefault
        {
            get
            {
                if (this.Notebook != null && this.Notebook.IsDefault == true)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 是否允许提交
        /// </summary>
        public bool CanSubmit
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Name);
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

        /// <summary>
        /// 笔记本服务类
        /// </summary>
        public INotebookService NotebookService { get; private set; }

        #endregion

        #region 命令

        private DelegateCommand _submitCommand;

        /// <summary>
        /// 提交命令
        /// </summary>
        public DelegateCommand SubmitCommand
        {
            get
            {
                if (_submitCommand == null)
                {
                    _submitCommand = new DelegateCommand(this.Submit);
                }
                return _submitCommand;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 提交
        /// </summary>
        private void Submit()
        {
            if (!this.CanSubmit)
            {
                return;
            }

            var note = new Notebook
            {
                Name = this.Name,
                Type = this.Type
            };
            if (this.Notebook != null)
            {
                note.ID = this.Notebook.ID;
            }

            string message = null;

            if (this.NotebookService.ModifyNotebook(note, ref message))
            {
                bool isNew = false;

                if (this.Notebook == null)
                {
                    this.Notebook = note;
                    isNew = true;
                }
                else
                {
                    this.Notebook.Name = note.Name;
                    this.Notebook.Type = note.Type;
                    this.Notebook.ModifyDate = note.ModifyDate;
                }
                this.Close();

                if (isNew)
                {
                    var e = new Events.AddNotebookEventArgs(this);
                    this.PublishEvent(e);
                }
            }
            else
            {
                this.DialogService.Alert(message, "操作失败", null);
            }
        }

        /// <summary>
        /// 重写窗口关闭方法,将修改后的数据还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnWindowClosed(object sender, EventArgs e)
        {
            base.OnWindowClosed(sender, e);

            if (this.Notebook != null)
            {
                this.Name = this.Notebook.Name;
                this.Type = this.Notebook.Type;
            }
            else
            {
                this.Name = null;
                this.Type = Logic.Enums.NoteBookType.Local;
            }
        }

        /// <summary>
        /// 在添加笔记时会执行的方法
        /// </summary>
        /// <param name="e"></param>
        private void OnAddNote(Events.AddNoteEventArgs e)
        {
            if (this.Notebook == null)
            {
                return;
            }

            if (e.Note.NotebookID == this.Notebook.ID && !e.Note.IsDelete)
            {
                this.Notes++;
            }
        }

        /// <summary>
        /// 在移动笔记时会执行的方法
        /// </summary>
        /// <param name="e"></param>
        private void OnMoveNote(Events.MoveNoteEventArgs e)
        {
            if (this.Notebook == null)
            {
                return;
            }

            if (e.OldNotebookID == this.Notebook.ID && !e.IsDelete)
            {
                this.Notes--;
            }
            if (e.NewNotebookID == this.Notebook.ID)
            {
                this.Notes++;
            }
        }

        /// <summary>
        /// 在删除笔记时会执行的方法
        /// </summary>
        /// <param name="e"></param>
        private void OnDeleteNote(Events.DeleteNoteEventArgs e)
        {
            if (this.Notebook == null)
            {
                return;
            }

            if (e.Note.NotebookID == this.Notebook.ID && e.IsDeleteToRecycle)
            {
                this.Notes--;
            }
        }

        /// <summary>
        /// 在还原笔记时会执行的方法
        /// </summary>
        /// <param name="e"></param>
        private void OnRestoreNote(Events.RestoreNoteEvent e)
        {
            if (this.Notebook == null)
            {
                return;
            }

            if (e.Note.NotebookID == this.Notebook.ID)
            {
                this.Notes++;
            }
        }

        #endregion
    }
}
