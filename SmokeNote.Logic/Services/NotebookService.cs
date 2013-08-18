using SmokeNote.Logic.Helpers;
using SmokeNote.Logic.IService;
using SmokeNote.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SmokeNote.Logic.Services
{
    public class NotebookService : INotebookService
    {
        public bool ModifyNotebook(Notebook entity, ref string message)
        {
            bool isNew = false;

            if (entity.ID == default(Guid))
            {
                entity.ID = Guid.NewGuid();
                isNew = true;
                entity.CreateDate = DateTime.Now;
            }

            entity.ModifyDate = DateTime.Now;

            List<System.Data.SQLite.SQLiteParameter> paraList = new List<System.Data.SQLite.SQLiteParameter>();
            paraList.Add(SqliteHelper.CreateInParameter("@ID", entity.ID, DbType.String));
            paraList.Add(SqliteHelper.CreateInParameter("@Name", entity.Name, DbType.StringFixedLength, 100));

            //检查名称重复
            string sql = "SELECT COUNT(1) FROM [Notebook] WHERE [Name]=@Name AND [ID]!=@ID";
            int exists = Convert.ToInt32(SqliteHelper.ExecuteScalar(sql, paraList.ToArray()));
            if (exists > 0)
            {
                message = "已经存在相同名称的笔记本!请重新输入!";
                return false;
            }

            //添加或修改操作
            paraList.Add(SqliteHelper.CreateInParameter("@Type", (Byte)entity.Type, DbType.Byte));
            paraList.Add(SqliteHelper.CreateInParameter("@ModifyDate", entity.ModifyDate, DbType.DateTime));

            if (isNew)
            {
                sql = @"INSERT INTO [Notebook]([ID],[Name],[Type],[CreateDate],[ModifyDate])
                        VALUES(@ID,@Name,@Type,@CreateDate,@ModifyDate)";
                paraList.Add(SqliteHelper.CreateInParameter("@CreateDate", entity.CreateDate, DbType.DateTime));
            }
            else
            {
                sql = "UPDATE [Notebook] SET [Name]=@Name,[Type]=@Type,[ModifyDate]=@ModifyDate WHERE [ID]=@ID";
            }

            int result = SqliteHelper.ExecuteNonQuery(sql, paraList.ToArray());

            return result > 0;
        }

        public bool DeleteNotebook(Guid id, out int notes, ref string message)
        {
            var para = SqliteHelper.CreateInParameter("@ID", id, DbType.Guid);

            //删除日记历史记录
            string sql = @"DELETE FROM [NoteHistory] WHERE [NoteID] IN (SELECT [ID] FROM [Note] WHERE [NotebookID]=@ID);";
            SqliteHelper.ExecuteNonQuery(sql, para);

            //删除日记
            sql = "DELETE FROM [Note] WHERE [NotebookID]=@ID;";
            notes = SqliteHelper.ExecuteNonQuery(sql, para);

            //删除日记本
            sql = "DELETE FROM [Notebook] WHERE [ID] = @ID";
            int result = SqliteHelper.ExecuteNonQuery(sql, para);

            if (result == 0)
            {
                message = "笔记本不存在或已被删除";
            }

            return result > 0;
        }


        public List<Models.Notebook> GetNotebookList()
        {
            string sql = @"SELECT [ID],[Name],[Type],[IsDefault],[CreateDate],[ModifyDate],B.[Notes] FROM [Notebook] A
                LEFT JOIN 
                (SELECT [NotebookID],COUNT(1) AS [Notes] FROM [Note] GROUP BY [NotebookID]) AS B
                ON A.[ID]=B.[NotebookID]
                ORDER BY A.[Type];";
            var list = SqliteHelper.ExecuteList<Notebook>(sql);
            return list;
        }
    }
}
