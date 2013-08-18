using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmokeNote.Logic.IService
{
    public interface INotebookService
    {
        /// <summary>
        /// 修改笔记本
        /// 包括修改和添加
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool ModifyNotebook(Models.Notebook entity, ref string message);

        /// <summary>
        /// 删除笔记本
        /// </summary>
        /// <param name="id"></param>
        /// <param name="notes"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool DeleteNotebook(Guid id, out int notes, ref string message);

        /// <summary>
        /// 得到笔记本列表
        /// </summary>
        /// <returns></returns>
        List<Models.Notebook> GetNotebookList();
    }
}
