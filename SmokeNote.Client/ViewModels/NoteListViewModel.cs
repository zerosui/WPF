using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Common.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;
using SmokeNote.Logic.Models;
using SmokeNote.Logic.IService;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Commands;
using System.Text.RegularExpressions;

namespace SmokeNote.Client.ViewModels
{
    public class NoteListViewModel : ViewModelBase
    {
        public NoteListViewModel()
        {
            this.InnerNoteList = new ObservableCollection<NoteViewModel>();
            this.NoteList = new ListCollectionView(this.InnerNoteList);
            this.NoteList.SortDescriptions.Add(new SortDescription("Note.ModifyDate", ListSortDirection.Descending));
            this.NoteList.Filter = this.FilterNote;
            this.NoteList.CurrentChanged += NoteList_CurrentChanged;

            if (!IsInDesignMode)
            {
                this.QueryCondition = this.UnityContainer.Resolve<NoteQueryCondition>();
                this.QueryCondition.PropertyChanged += QueryCondition_PropertyChanged;
                this.NoteService = this.UnityContainer.Resolve<INoteService>();
                this.SubscribeEvent<Events.DeleteNotebookEvent>(this.OnDeleteNotebook);
            }
        }

        #region 属性

        private ObservableCollection<NoteViewModel> InnerNoteList { get; set; }

        public ListCollectionView NoteList { get; private set; }

        public INoteService NoteService { get; private set; }

        public NoteQueryCondition QueryCondition { get; private set; }
        
        #endregion

        #region 命令

        private DelegateCommand _addNoteCommand;

        /// <summary>
        /// 添加笔记命令
        /// </summary>
        public DelegateCommand AddNoteCommand
        {
            get
            {
                if (_addNoteCommand == null)
                {
                    _addNoteCommand = new DelegateCommand(this.AddNote, this.CanAddNote);
                }
                return _addNoteCommand;
            }
        }

        /// <summary>
        /// 是否允许添加笔记
        /// </summary>
        /// <returns></returns>
        private bool CanAddNote()
        {
            return this.QueryCondition.IsRecycle == false;
        }

        /// <summary>
        /// 添加笔记
        /// </summary>
        private void AddNote()
        {
            var note = new Note
            {
                Title = "未命名标题",
                NotebookID = this.QueryCondition.CurrentNotebook != null ? this.QueryCondition.CurrentNotebook.ID : Notebook.DefaultNotebookID
            };
            string message = null;

            if (this.NoteService.ModifyNote(note, ref message))
            {
                var item = new NoteViewModel(note);
                this.AddNoteToList(item);
                this.NoteList.MoveCurrentTo(item);
            }
            else
            {
                this.DialogService.Alert(message, "添加笔记失败", null);
            }
        }

        private DelegateCommand<Notebook> _moveCommand;

        /// <summary>
        /// 移动笔记命令
        /// </summary>
        public DelegateCommand<Notebook> MoveCommand
        {
            get
            {
                if (_moveCommand == null)
                {
                    _moveCommand = new DelegateCommand<Notebook>(this.Move);
                }
                return _moveCommand;
            }
        }

        /// <summary>
        /// 移动笔记操作
        /// </summary>
        /// <param name="parameter"></param>
        private void Move(Notebook parameter)
        {
            var noteViewModel = this.NoteList.CurrentItem as NoteViewModel;
            if (noteViewModel == null)
            {
                return;
            }
            var note = noteViewModel.Note;
            if (note.NotebookID == parameter.ID && !note.IsDelete)
            {
                this.DialogService.Alert("不能移动到相同的目录!", "移动笔记失败!", null);
                return;
            }

            var e = new Events.MoveNoteEventArgs(note, note.NotebookID, parameter.ID, note.IsDelete);

            string message = null;
            if (this.NoteService.MoveNote(note.ID, parameter.ID, ref message))
            {
                note.NotebookID = parameter.ID;
                note.IsDelete = false;
                this.PublishEvent(e);
                this.NoteList.Refresh();
            }
            else
            {
                this.DialogService.Alert(message, "移动笔记失败!", null);
            }
        }

        private DelegateCommand _deleteNoteCommand;

        /// <summary>
        /// 删除笔记命令
        /// </summary>
        public DelegateCommand DeleteNoteCommand
        {
            get
            {
                if (_deleteNoteCommand == null)
                {
                    _deleteNoteCommand = new DelegateCommand(this.DeleteNote, this.CanDeleteNote);
                }
                return _deleteNoteCommand;
            }
        }

        private bool CanDeleteNote()
        {
            return this.NoteList.CurrentItem != null;
        }

        /// <summary>
        /// 删除笔记
        /// </summary>
        private void DeleteNote()
        {
            var note = this.NoteList.CurrentItem as NoteViewModel;

            if (note == null)
            {
                return;
            }

            string message = null;

            Events.DeleteNoteEventArgs e = new Events.DeleteNoteEventArgs(note.Note, !note.Note.IsDelete);

            if (!note.Note.IsDelete)
            {
                //如果是删除到回收站,则直接执行
                this.NoteService.DeleteToRecycle(note.Note.ID, ref message);
                note.Note.IsDelete = true;
                this.PublishEvent(e);
                this.NoteList.Refresh();
            }
            else
            {
                //如果是彻底删除,则弹出提示
                this.DialogService.Confirm("笔记一旦删除将不可恢复!", "确定删除该笔记?", b => 
                {
                    if (!b)
                    {
                        return;
                    }

                    this.NoteService.DeleteNote(note.Note.ID, ref message);
                    this.PublishEvent(e);
                    this.NoteList.Remove(note);
                    this.NoteList.Refresh();
                });
            }
        }

        private DelegateCommand _restoreNoteCommand;

        /// <summary>
        /// 还原笔记命令
        /// </summary>
        public DelegateCommand RestoreNoteCommand
        {
            get
            {
                if (_restoreNoteCommand == null)
                {
                    _restoreNoteCommand = new DelegateCommand(this.RestoreNote);
                }
                return _restoreNoteCommand;
            }
        }

        private void RestoreNote()
        {
            var note = this.NoteList.CurrentItem as NoteViewModel;

            if (note == null)
            {
                return;
            }

            this.RestoreNote(note.Note);
        }

        private DelegateCommand _restoreAllNoteCommand;

        /// <summary>
        /// 全部还原命令
        /// </summary>
        public DelegateCommand RestoreAllNoteCommand
        {
            get
            {
                if (_restoreAllNoteCommand == null)
                {
                    _restoreAllNoteCommand = new DelegateCommand(this.RestoreAllNote);
                }
                return _restoreAllNoteCommand;
            }
        }

        /// <summary>
        /// 还原全部笔记
        /// </summary>
        private void RestoreAllNote()
        {
            var noteList = this.InnerNoteList.Where(t => t.Note.IsDelete).ToList();
            foreach (var note in noteList)
            {
                this.RestoreNote(note.Note);
            }
        }

        private DelegateCommand _clearRecycleCommand;

        /// <summary>
        /// 清空回收站命令
        /// </summary>
        public DelegateCommand ClearRecycleCommand
        {
            get
            {
                if (_clearRecycleCommand == null)
                {
                    _clearRecycleCommand = new DelegateCommand(this.ClearRecycle);
                }
                return _clearRecycleCommand;
            }
        }

        /// <summary>
        /// 清空回收站
        /// </summary>
        private void ClearRecycle()
        {
            var recycleList = this.InnerNoteList.Where(t => t.Note.IsDelete).ToList();
            if (recycleList.Count == 0)
            {
                this.DialogService.Alert("", "回收站为空!", null);
                return;
            }

            string message = null;

            string title = string.Format("确定删除回收站中{0}个笔记吗?", recycleList.Count.ToString());

            this.DialogService.Confirm("永久删除的笔记无法恢复", title, b => 
            {
                this.NoteService.ClearRecycle(ref message);
                foreach (var note in recycleList)
                {
                    this.InnerNoteList.Remove(note);
                }
                this.NoteList.Refresh();
                var e = new Events.ClearRecycleEvent();
                this.PublishEvent(e);
            });
        }

        private DelegateCommand _saveAllCommand;

        /// <summary>
        /// 全部保存命令
        /// </summary>
        public DelegateCommand SaveAllCommand
        {
            get
            {
                if (_saveAllCommand == null)
                {
                    _saveAllCommand = new DelegateCommand(this.SaveAll);
                }
                return _saveAllCommand;
            }
        }

        /// <summary>
        /// 保存全部日记
        /// </summary>
        private void SaveAll()
        {
            foreach (var item in this.InnerNoteList)
            {
                item.SaveCommand.Execute();
            }
        }

        #endregion

        #region 方法

        public void InitNoteList()
        {
            //加载笔记
            var list = this.NoteService.GetNoteList();

            this.InvokeOnUIDispatcher(() =>
            {
                foreach (var item in list)
                {
                    var vm = new NoteViewModel(item);
                    this.AddNoteToList(vm);
                }
                this.NoteList.Refresh();
            });
        }

        private bool FilterNote(object o)
        {
            var note = o as NoteViewModel;

            if (!string.IsNullOrWhiteSpace(this.QueryCondition.Keywords))
            {
                var regex = new Regex(this.QueryCondition.Keywords, RegexOptions.Singleline);

                if (!regex.IsMatch(note.Note.Title ?? ""))
                {
                    if (!regex.IsMatch(note.Summary ?? ""))
                    {
                        return false;
                    }
                }
            }

            if (this.QueryCondition.CurrentNotebook != null)
            {
                return note.Note.NotebookID == this.QueryCondition.CurrentNotebook.ID && !note.Note.IsDelete;
            }
            else if (this.QueryCondition.IsRecycle)
            {
                return note.Note.IsDelete;
            }

            return !note.Note.IsDelete;
        }

        private void AddNoteToList(NoteViewModel note)
        {
            this.InnerNoteList.Add(note);
            var e = new Events.AddNoteEventArgs(note.Note);
            this.PublishEvent(e);
        }

        private void QueryCondition_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.AddNoteCommand.RaiseCanExecuteChanged();
            this.NoteList.Refresh();
        }

        private void NoteList_CurrentChanged(object sender, EventArgs e)
        {
            this.DeleteNoteCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// 还原某个笔记
        /// </summary>
        /// <param name="note"></param>
        private void RestoreNote(Note note)
        {
            if (!note.IsDelete)
            {
                return;
            }
            string message = null;

            if (this.NoteService.RestoreFromRecycle(note.ID, ref message))
            {
                note.IsDelete = false;
                var e = new Events.RestoreNoteEvent(note);
                this.PublishEvent(e);
                this.NoteList.Refresh();
            }
            else
            {
                this.DialogService.Alert(message, "还原笔记失败!", null);
            }
        }

        /// <summary>
        /// 删除笔记本时会触发的方法
        /// </summary>
        /// <param name="e"></param>
        private void OnDeleteNotebook(Events.DeleteNotebookEvent e)
        {
            foreach (var item in this.InnerNoteList.ToList())
            {
                if (item.Note.NotebookID == e.NotebookID)
                {
                    this.InnerNoteList.Remove(item);
                }
            }
        }

        #endregion
    }
}
