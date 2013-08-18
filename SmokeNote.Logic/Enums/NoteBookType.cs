using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmokeNote.Logic.Enums
{
    /// <summary>
    /// 笔记本类型枚举
    /// </summary>
    public enum NoteBookType : byte
    {
        /// <summary>
        /// 本地笔记本
        /// </summary>
        Local = 0,

        /// <summary>
        /// 同步笔记本
        /// </summary>
        Remote = 1
    }
}
