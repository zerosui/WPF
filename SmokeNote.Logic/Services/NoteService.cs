using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmokeNote.Logic.IService;
using SmokeNote.Logic.Helpers;
using SmokeNote.Logic.Models;
using System.Data;

namespace SmokeNote.Logic.Services
{
    public class NoteService : INoteService
    {
        public List<Note> GetNoteList()
        {
            string sql = "SELECT [ID],[NotebookID],[Title],[Author],[From],[Content],[Tags],[CreateDate],[ModifyDate],[IsDelete] FROM [Note] ORDER BY [ModifyDate] DESC";
            return SqliteHelper.ExecuteList<Note>(sql);
        }

        public bool ModifyNote(Note note, ref string message)
        {
            //验证
            if (string.IsNullOrWhiteSpace(note.Title))
            {
                message = "笔记标题不能为空!";
                return false;
            }

            note.ModifyDate = DateTime.Now;

            bool isNew = false;
            if (note.ID == default(Guid)) 
            {
                isNew = true;
                note.ID = Guid.NewGuid();
                note.CreateDate = DateTime.Now;
            }

            //添加或修改日记
            string sql = null;

            List<System.Data.SQLite.SQLiteParameter> paraList = new List<System.Data.SQLite.SQLiteParameter>();
            paraList.Add(SqliteHelper.CreateInParameter("@ID", note.ID, DbType.Guid));
            paraList.Add(SqliteHelper.CreateInParameter("@NotebookID", note.NotebookID, DbType.Guid));
            paraList.Add(SqliteHelper.CreateInParameter("@Title", note.Title, DbType.StringFixedLength, 200));
            paraList.Add(SqliteHelper.CreateInParameter("@Author", note.Author, DbType.StringFixedLength, 100));
            paraList.Add(SqliteHelper.CreateInParameter("@From", note.From, DbType.StringFixedLength, 100));
            paraList.Add(SqliteHelper.CreateInParameter("@Tags", note.Tags, DbType.StringFixedLength, 500));
            paraList.Add(SqliteHelper.CreateInParameter("@Content", note.Content, DbType.String));
            paraList.Add(SqliteHelper.CreateInParameter("@ModifyDate", note.ModifyDate, DbType.DateTime));

            if (isNew)
            {
                sql = @"INSERT INTO [Note]([ID],[NotebookID],[Title],[Author],[From],[Tags],[Content],[CreateDate],[ModifyDate])
                        VALUES(@ID,@NotebookID,@Title,@Author,@From,@Tags,@Content,@CreateDate,@ModifyDate);";
                paraList.Add(SqliteHelper.CreateInParameter("@CreateDate", note.CreateDate, DbType.DateTime));
            }
            else
            {
                sql = @"UPDATE [Note] SET [NotebookID]=@NotebookID,[Title]=@Title,[Author]=@Author,[From]=@From,[Tags]=@Tags,
                        [Content]=@Content,[ModifyDate]=@ModifyDate WHERE [ID]=@ID;";
            }

            int result = SqliteHelper.ExecuteNonQuery(sql, paraList.ToArray());

            //修改笔记时才添加历史记录
            if (result > 0 && !isNew)
            {                
                sql = @"INSERT INTO [NoteHistory]([NoteID],[NotebookID],[Title],[Author],[From],[Tags],[Content],[ModifyDate])
                        VALUES(@ID,@NotebookID,@Title,@Author,@From,@Tags,@Content,@ModifyDate);";
                SqliteHelper.ExecuteNonQuery(sql, paraList.ToArray());
            }

            return result > 0;
        }

        /// <summary>
        /// 移动笔记
        /// </summary>
        /// <param name="noteID"></param>
        /// <param name="notebookID"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool MoveNote(Guid noteID, Guid notebookID, ref string message)
        {
            string sql = "UPDATE [NOTE] SET [NotebookID]=@NotebookID,[IsDelete]=0 WHERE [ID]=@ID;";
            List<System.Data.SQLite.SQLiteParameter> paraList = new List<System.Data.SQLite.SQLiteParameter>();
            paraList.Add(SqliteHelper.CreateInParameter("@ID", noteID, DbType.Guid));
            paraList.Add(SqliteHelper.CreateInParameter("@NotebookID", notebookID, DbType.Guid));
            return SqliteHelper.ExecuteNonQuery(sql, paraList.ToArray()) > 0;
        }

        /// <summary>
        /// 删除笔记到回收站
        /// </summary>
        /// <param name="noteID"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool DeleteToRecycle(Guid noteID, ref string message)
        {
            string sql = "UPDATE [NOTE] SET [IsDelete]=1 WHERE [ID]=@ID;";
            var para = SqliteHelper.CreateInParameter("@ID", noteID, DbType.Guid);
            var result = SqliteHelper.ExecuteNonQuery(sql, para);
            return result > 0;
        }

        /// <summary>
        /// 彻底删除笔记
        /// </summary>
        /// <param name="noteID"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool DeleteNote(Guid noteID, ref string message)
        {
            string sql = @"DELETE FROM [NoteHistory] WHERE [NoteID]=@ID; 
                            DELETE FROM [Note] WHERE [ID]=@ID;";
            var para = SqliteHelper.CreateInParameter("@ID", noteID, DbType.Guid);
            var result = SqliteHelper.ExecuteNonQuery(sql, para);
            return result > 0;
        }

        /// <summary>
        /// 得到日记本实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Note GetNote(Guid id)
        {
            string sql = "SELECT [ID],[NotebookID],[Title],[Author],[From],[Content],[Tags],[CreateDate],[ModifyDate],[IsDelete] FROM [Note] WHERE [ID]=@ID";
            var para = SqliteHelper.CreateInParameter("@ID", id, DbType.Guid);
            return SqliteHelper.ExecuteItem<Note>(sql, para);
        }

        /// <summary>
        /// 得到历史记录列表
        /// </summary>
        /// <param name="noteID"></param>
        /// <returns></returns>
        public List<Note> GetHistoryList(Guid noteID)
        {
            string sql = "SELECT [NoteID] AS [ID],[NotebookID],[Title],[Author],[From],[Content],[Tags],[ModifyDate] FROM [NoteHistory] WHERE [NoteID]=@ID ORDER BY [ID] ASC";
            var para = SqliteHelper.CreateInParameter("@ID", noteID, DbType.Guid);
            return SqliteHelper.ExecuteList<Note>(sql, para);
        }

        /// <summary>
        /// 从回收站还原
        /// </summary>
        /// <param name="noteID"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool RestoreFromRecycle(Guid noteID, ref string message)
        {
            string sql = "UPDATE [NOTE] SET [IsDelete]=0 WHERE [ID]=@ID;";
            var para = SqliteHelper.CreateInParameter("@ID", noteID, DbType.Guid);
            var result = SqliteHelper.ExecuteNonQuery(sql, para);
            return result > 0;
        }

        /// <summary>
        /// 清空回收站
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ClearRecycle(ref string message)
        {
            string sql = "DELETE FROM [Note] WHERE [IsDelete]=1";
            var result = SqliteHelper.ExecuteNonQuery(sql);
            return result > 0;
        }
    }
}
