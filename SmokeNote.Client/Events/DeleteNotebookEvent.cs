using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmokeNote.Client.Events
{
    public class DeleteNotebookEvent
    {
        public DeleteNotebookEvent(Guid notebookID)
        {
            this.NotebookID = notebookID;
        }

        public Guid NotebookID { get; set; }
    }
}
