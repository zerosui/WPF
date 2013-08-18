using SmokeNote.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmokeNote.Client.Events
{
    public class RestoreNoteEvent
    {
        public RestoreNoteEvent(Note note)
        {
            this.Note = note;
        }

        public Note Note { get; set; }
    }
}
