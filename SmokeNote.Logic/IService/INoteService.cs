using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmokeNote.Logic.Models;

namespace SmokeNote.Logic.IService
{
    public interface INoteService
    {
        List<Note> GetNoteList();

        bool ModifyNote(Note note, ref string message);

        bool MoveNote(Guid noteID, Guid notebookID, ref string message);

        bool DeleteToRecycle(Guid noteID, ref string message);

        bool RestoreFromRecycle(Guid noteID, ref string message);

        bool DeleteNote(Guid noteID, ref string message);

        bool ClearRecycle(ref string message);

        Note GetNote(Guid id);

        List<Note> GetHistoryList(Guid noteID);
    }
}
