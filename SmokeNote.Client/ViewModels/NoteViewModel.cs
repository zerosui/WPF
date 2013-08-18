using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Common.ViewModel;
using SmokeNote.Logic.Models;
using Microsoft.Practices.Prism.Commands;
using SmokeNote.Logic.IService;
using Microsoft.Practices.Unity;
using System.Windows.Documents;
using System.IO;
using System.Windows;
using Framework.Common.Dialog;

namespace SmokeNote.Client.ViewModels
{
    public class NoteViewModel : ViewModelBase
    {
        #region 构造器

        public NoteViewModel(Note note)
        {
            this.Note = note;
            this.Title = note.Title;
            this.Author = note.Author;
            this.From = note.From;
            this.Content = note.Content;
            this.Tags = note.Tags;

            if (!IsInDesignMode)
            {
                this.NoteService = this.UnityContainer.Resolve<INoteService>();
            }
        }

        #endregion

        #region 属性

        private string _title;

        /// <summary>
        /// 编辑的标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    this.RaisePropertyChanged("Title", "IsModified");
                    this.ModifyDate = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary
        {
            get 
            {
                return Framework.Common.Helpers.RichTextHelper.GetPlainText(this.Content);
            }
        }

        private string _author;

        /// <summary>
        /// 作者
        /// </summary>
        public string Author
        {
            get { return _author; }
            set
            {
                if (_author != value)
                {
                    _author = value;
                    this.RaisePropertyChanged("Author", "IsModified");
                    this.ModifyDate = DateTime.Now;
                }
            }
        }

        private string _from;

        /// <summary>
        /// 来源
        /// </summary>
        public string From
        {
            get { return _from; }
            set
            {
                if (_from != value)
                {
                    _from = value;
                    this.RaisePropertyChanged("From", "IsModified");
                    this.ModifyDate = DateTime.Now;
                }
            }
        }

        private string _content;

        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            get { return _content; }
            set
            {
                if (_content != value)
                {
                    _content = value;
                    this.RaisePropertyChanged("Content", "Summary", "IsModified");
                    this.ModifyDate = DateTime.Now;
                }
            }
        }

        private string _tags;

        /// <summary>
        /// 标签
        /// </summary>
        public string Tags
        {
            get { return _tags; }
            set
            {
                if (_tags != value)
                {
                    _tags = value;
                    this.RaisePropertyChanged("Tags", "IsModified");
                    this.ModifyDate = DateTime.Now;
                }
            }
        }

        private DateTime _modifyDate;

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyDate
        {
            get { return _modifyDate; }
            set
            {
                if (_modifyDate != value)
                {
                    _modifyDate = value;
                    this.RaisePropertyChanged("ModifyDate");
                }
            }
        }

        private Note _note;

        /// <summary>
        /// 笔记本实体
        /// </summary>
        public Note Note
        {
            get { return _note; }
            set
            {
                if (_note != value)
                {
                    _note = value;
                    this.RaisePropertyChanged("Note", "IsModified");
                }
            }
        }

        /// <summary>
        /// 笔记本服务类
        /// </summary>
        public INoteService NoteService
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否被修改
        /// </summary>
        public bool IsModified
        {
            get
            {
                if (this.Title != this.Note.Title)
                {
                    return true;
                }
                if (this.Author != this.Note.Author)
                {
                    return true;
                }
                if (this.From != this.Note.From)
                {
                    return true;
                }
                if (this.Tags != this.Note.Tags)
                {
                    return true;
                }
                if (this.Content != this.Note.Content)
                {
                    return true;
                }
                return false;
            }
        }

        #endregion

        #region 命令

        private DelegateCommand _saveCommand;

        /// <summary>
        /// 保存命令
        /// </summary>
        public DelegateCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new DelegateCommand(() => 
                    {
                        var action = new Action(this.Save);
                        action.BeginInvoke(ar => 
                        {
                            action.EndInvoke(ar);
                        }, null);
                    });
                }
                return _saveCommand;
            }
        }

        private DelegateCommand _showNoteHistoryCommand;

        /// <summary>
        /// 显示历史记录命令
        /// </summary>
        public DelegateCommand ShowNoteHistoryCommand
        {
            get
            {
                if (_showNoteHistoryCommand == null)
                {
                    _showNoteHistoryCommand = new DelegateCommand(this.ShowNoteHistory);
                }
                return _showNoteHistoryCommand;
            }
        }

        private DelegateCommand<object> _exportCommand;

        /// <summary>
        /// 导出命令
        /// </summary>
        public DelegateCommand<object> ExportCommand
        {
            get
            {
                if (_exportCommand == null)
                {
                    _exportCommand = new DelegateCommand<object>(this.Export);
                }
                return _exportCommand;
            }
        }

        /// <summary>
        /// 导出笔记
        /// </summary>
        /// <param name="parameter"></param>
        private void Export(object parameter)
        {
            if (parameter == null)
            {
                return;
            }

            byte b;
            if (!byte.TryParse(parameter.ToString(), out b))
            {
                return;
            }

            Logic.Enums.ExportTypes type = (Logic.Enums.ExportTypes)b;


            var fileConfig = new Framework.Common.Dialog.FileConfig
            {
                FileName = this.GetFileName(type),
                Filter = this.GetFileFilter(type),
                Title = "导出路径"
            };

            this.DialogService.SaveFile(fileConfig, stream => 
            {
                using (var writer = new StreamWriter(stream))
                {
                    switch (type)
                    {
                        case Logic.Enums.ExportTypes.PlainText:
                            writer.Write(this.Summary);
                            break;
                        case Logic.Enums.ExportTypes.Word:
                            writer.Write(this.Content);
                            break;
                        case Logic.Enums.ExportTypes.Html:
                            var converter = new MarkupConverter.MarkupConverter();
                            writer.Write(converter.ConvertRtfToHtml(this.Content));
                            break;
                    }
                }
            });
        }

        #endregion

        #region 方法

        private string GetFileName(Logic.Enums.ExportTypes type)
        {
            string extension = null;
            switch (type)
            {
                case Logic.Enums.ExportTypes.Html:
                    extension = ".html";
                    break;
                case Logic.Enums.ExportTypes.PlainText:
                    extension = ".txt";
                    break;
                case Logic.Enums.ExportTypes.Word:
                    extension = ".doc";
                    break;
            }
            return this.Title + extension;
        }

        private string GetFileFilter(Logic.Enums.ExportTypes type)
        {
            string result = null;
            switch (type)
            {
                case Logic.Enums.ExportTypes.Html:
                    result = "Html文件(*.html)|*.html";
                    break;
                case Logic.Enums.ExportTypes.PlainText:
                    result = "文本文件(.txt)|*.txt";
                    break;
                case Logic.Enums.ExportTypes.Word:
                    result = "Word文档(*.doc,*.docx)|*.doc,*.docx";
                    break;
            }
            return result;
        }

        /// <summary>
        /// 保存日记
        /// 不要直接调用此方法,调用SaveCommand的Execute()方法会异步调用此方法,并实现过渡效果
        /// </summary>
        private void Save()
        {
            if (!this.IsModified || this.Note.IsDelete)
            {
                return;
            }
            if (this.IsLoading)
            {
                return;
            }

            this.IsLoading = true;

            var note = this.CreateNote();

            string message = null;

            if (this.NoteService.ModifyNote(note, ref message))
            {
                this.Note.Author = note.Author;
                this.Note.Content = note.Content;
                this.Note.From = note.From;
                this.Note.ModifyDate = note.ModifyDate;
                this.Note.NotebookID = note.NotebookID;
                this.Note.Tags = note.Tags;
                this.Note.Title = note.Title;

                this.RaisePropertyChanged("IsModified");
                this.InvokeOnUIDispatcher(() =>
                {
                    this.DialogService.Notify("保存成功", null, MessageType.Success, null);
                });
            }
            else
            {
                this.DialogService.Alert(message, "保存笔记本失败!", null);
            }
            this.IsLoading = false;
        }

        /// <summary>
        /// 显示历史记录
        /// </summary>
        private void ShowNoteHistory()
        {
            var note = this.CreateNote();
            var vm = new NoteHistoryViewModel(note);
            vm.ShowDialog();
        }

        /// <summary>
        /// 根据当前编辑的临时值,创造一个笔记本实体
        /// 用于数据库操作
        /// </summary>
        /// <returns></returns>
        private Note CreateNote()
        {
            var note = new Note
            {
                Author = this.Author,
                Content = this.Content,
                From = this.From,
                ID = this.Note.ID,
                NotebookID = this.Note.NotebookID,
                Tags = this.Tags,
                Title = this.Title,
                ModifyDate = this.ModifyDate
            };
            return note;
        }

        #endregion
    }
}
