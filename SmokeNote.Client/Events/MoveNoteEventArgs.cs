using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmokeNote.Logic.Models;

namespace SmokeNote.Client.Events
{
    public class MoveNoteEventArgs
    {
        public MoveNoteEventArgs(Note note, Guid oldNotebookID, Guid newNotebookID, bool isDelete)
        {
            this.Note = note;
            this.OldNotebookID = oldNotebookID;
            this.NewNotebookID = newNotebookID;
            this.IsDelete = isDelete;
        }

        /// <summary>
        /// 笔记本实体
        /// </summary>
        public Note Note { get; set; }

        /// <summary>
        /// 旧的笔记本ID
        /// </summary>
        public Guid OldNotebookID { get; set; }

        /// <summary>
        /// 新的笔记本ID
        /// </summary>
        public Guid NewNotebookID { get; set; }
        
        /// <summary>
        /// 是否是删除状态
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
