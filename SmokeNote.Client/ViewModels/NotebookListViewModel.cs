using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Common.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;
using SmokeNote.Logic.IService;
using Microsoft.Practices.Unity;
using SmokeNote.Logic.Models;
using Microsoft.Practices.Prism.Commands;

namespace SmokeNote.Client.ViewModels
{
    public class NotebookListViewModel : ViewModelBase
    {
        #region 构造器

        public NotebookListViewModel()
        {
            this.InnerNotebookList = new ObservableCollection<NotebookViewModel>();
            this.NotebookList = new ListCollectionView(this.InnerNotebookList);            

            //默认以类型和名称排序
            this.NotebookList.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));   

            if (!IsInDesignMode)
            {
                this.NotebookService = this.UnityContainer.Resolve<INotebookService>();

                this.SubscribeEvent<Events.AddNotebookEventArgs>(this.OnAddNotebook);
                this.SubscribeEvent<Events.AddNoteEventArgs>(this.OnAddNote);
                this.SubscribeEvent<Events.DeleteNoteEventArgs>(this.OnDeleteNote);
                this.SubscribeEvent<Events.RestoreNoteEvent>(this.OnRestoreNote);
                this.SubscribeEvent<Events.MoveNoteEventArgs>(this.OnMoveNote);
                this.SubscribeEvent<Events.ClearRecycleEvent>(this.OnClearRecycle);
            }
        }

        #endregion

        #region 属性

        /// <summary>
        /// 笔记本列表
        /// </summary>
        private ObservableCollection<NotebookViewModel> InnerNotebookList { get; set; }

        public ListCollectionView NotebookList { get; private set; }

        private int _notes;

        /// <summary>
        /// 总日记数
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

        private int _recycleNotes;

        /// <summary>
        /// 回收站日记数
        /// </summary>
        public int RecycleNotes
        {
            get { return _recycleNotes; }
            set
            {
                if (_recycleNotes != value)
                {
                    _recycleNotes = value;
                    this.RaisePropertyChanged("RecycleNotes");
                }
            }
        }

        /// <summary>
        /// 笔记本服务类
        /// </summary>
        public INotebookService NotebookService { get; private set; }

        #endregion

        #region 命令

        private DelegateCommand<NotebookViewModel> _deleteNotebookCommand;

        public DelegateCommand<NotebookViewModel> DeleteNotebookCommand
        {
            get
            {
                if (_deleteNotebookCommand == null)
                {
                    _deleteNotebookCommand = new DelegateCommand<NotebookViewModel>(this.DeleteNotebook);
                }
                return _deleteNotebookCommand;
            }
        }

        private DelegateCommand<NotebookViewModel> _editNotebookCommand;

        public DelegateCommand<NotebookViewModel> EditNotebookCommand
        {
            get
            {
                if (_editNotebookCommand == null)
                {
                    _editNotebookCommand = new DelegateCommand<NotebookViewModel>(this.EditNotebook);
                }
                return _editNotebookCommand;
            }
        }


        #endregion

        #region 方法

        /// <summary>
        /// 初始化笔记本列表
        /// </summary>
        public void InitNotebookList()
        {
            //加载笔记本列表
            var list = this.NotebookService.GetNotebookList();

            this.InvokeOnUIDispatcher(() =>
            {
                foreach (var item in list)
                {
                    var vm = new NotebookViewModel(item);
                    this.InnerNotebookList.Add(vm);
                }
            });
        }

        /// <summary>
        /// 新建笔记本完成时会触发的方法
        /// </summary>
        /// <param name="e"></param>
        private void OnAddNotebook(Events.AddNotebookEventArgs e)
        {
            this.InnerNotebookList.Add(e.NotebookViewModel);
        }

        /// <summary>
        /// 删除笔记本
        /// </summary>
        /// <param name="parameter"></param>
        private void DeleteNotebook(NotebookViewModel parameter)
        {
            if (parameter.Notebook == null)
            {
                return;
            }

            string message = null;

            if (parameter.Notebook.IsDefault)
            {
                this.DialogService.Alert("不允许删除默认笔记本!", "操作失败", null);
                return;
            }

            this.DialogService.Confirm("删除笔记本的同时会删除里面的笔记,同时无法恢复!", "确定要删除笔记本吗?", b => 
            {
                if (!b)
                {
                    return;
                }

                int notes;

                if (this.NotebookService.DeleteNotebook(parameter.Notebook.ID, out notes, ref message))
                {
                    //移除子项
                    this.InnerNotebookList.Remove(parameter);

                    //发出通知
                    var e = new Events.DeleteNotebookEvent(parameter.Notebook.ID);
                    this.PublishEvent(e);

                    //修改剩余的日记数
                    int recycles = notes - parameter.Notes;
                    this.Notes -= parameter.Notes;
                    this.RecycleNotes -= recycles;
                }
                else
                {
                    this.DialogService.Alert(message, "操作失败", null);
                }
            });
        }

        /// <summary>
        /// 修改笔记本
        /// </summary>
        /// <param name="parameter"></param>
        private void EditNotebook(NotebookViewModel parameter)
        {
            parameter.ShowDialog();
        }

        /// <summary>
        /// 增加日记时会触发的方法
        /// </summary>
        /// <param name="e"></param>
        private void OnAddNote(Events.AddNoteEventArgs e)
        {
            if (e.Note.IsDelete)
            {
                this.RecycleNotes++;
            }
            else
            {
                this.Notes++;
            }
        }

        /// <summary>
        /// 删除笔记时会执行的方法
        /// </summary>
        /// <param name="e"></param>
        private void OnDeleteNote(Events.DeleteNoteEventArgs e)
        {
            if (e.IsDeleteToRecycle)
            {
                this.RecycleNotes++;
                this.Notes--;
            }
            else
            {
                this.RecycleNotes--;
            }
        }

        /// <summary>
        /// 在还原笔记时会执行的方法
        /// </summary>
        /// <param name="e"></param>
        private void OnRestoreNote(Events.RestoreNoteEvent e)
        {
            this.RecycleNotes--;
            this.Notes++;
        }

        /// <summary>
        /// 在移动笔记时会执行的方法
        /// </summary>
        /// <param name="e"></param>
        private void OnMoveNote(Events.MoveNoteEventArgs e)
        {
            if (e.IsDelete)
            {
                this.RecycleNotes--;
                this.Notes++;
            }
        }

        /// <summary>
        /// 在清空回收站时会执行的方法
        /// </summary>
        private void OnClearRecycle(Events.ClearRecycleEvent e)
        {
            this.RecycleNotes = 0;
        }

        #endregion
    }
}
