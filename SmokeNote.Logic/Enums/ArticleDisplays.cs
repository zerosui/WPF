using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmokeNote.Logic.Enums
{
    /// <summary>
    /// 笔记显示类型枚举
    /// </summary>
    public enum ArticleDisplays : byte
    {
        /// <summary>
        /// 摘要
        /// </summary>
        Summary = 0,

        /// <summary>
        /// 缩略图
        /// </summary>
        Thumb = 1,

        /// <summary>
        /// 列表
        /// </summary>
        List = 2
    }
}
