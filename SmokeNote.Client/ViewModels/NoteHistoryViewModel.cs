using Framework.Common.ViewModel;
using Microsoft.Practices.Prism.Commands;
using SmokeNote.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SmokeNote.Logic.IService;
using Microsoft.Practices.Unity;

namespace SmokeNote.Client.ViewModels
{
    public class NoteHistoryViewModel : WindowViewModelBase
    {
        public NoteHistoryViewModel(Note note)
            : base(Consts.WindowNames.NoteHistory)
        {
            this.CurrentNote = note;
            this.HistoryList = new ObservableCollection<Note>();
            this.NoteService = this.UnityContainer.Resolve<INoteService>();            
        }

        #region 属性

        private Note _currentNote;

        /// <summary>
        /// 日记当前版本
        /// </summary>
        public Note CurrentNote
        {
            get { return _currentNote; }
            set
            {
                if (_currentNote != value)
                {
                    _currentNote = value;
                    this.RaisePropertyChanged("CurrentNote");
                }
            }
        }

        /// <summary>
        /// 历史记录列表
        /// </summary>
        public ObservableCollection<Note> HistoryList { get; private set; }

        private Note _currentHistoryNote;

        /// <summary>
        /// 选中的历史记录版本
        /// </summary>
        public Note CurrentHistoryNote
        {
            get { return _currentHistoryNote; }
            set
            {
                if (_currentHistoryNote != value)
                {
                    _currentHistoryNote = value;
                    this.RaisePropertyChanged("CurrentHistoryNote");
                }
            }
        }

        /// <summary>
        /// 笔记服务类
        /// </summary>
        public INoteService NoteService { get; private set; }

        #endregion

        #region 命令

        /// <summary>
        /// UI加载完成会执行的方法
        /// </summary>
        protected override void Loaded()
        {
            var action = new Action(this.InitData);
            action.BeginInvoke(ar => 
            {
                action.EndInvoke(ar);
            }, null);
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            var historyList = this.NoteService.GetHistoryList(this.CurrentNote.ID);

            this.InvokeOnUIDispatcher(() =>
            {
                foreach (var item in historyList)
                {
                    this.HistoryList.Add(item);
                }

                this.CurrentHistoryNote = this.HistoryList.LastOrDefault();
            });
        }

        #endregion
    }
}
