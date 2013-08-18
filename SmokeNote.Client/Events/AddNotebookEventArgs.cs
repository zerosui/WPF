using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmokeNote.Client.Events
{
    public class AddNotebookEventArgs
    {
        public AddNotebookEventArgs(ViewModels.NotebookViewModel notebookViewModel)
        {
            this.NotebookViewModel = notebookViewModel;
        }

        public ViewModels.NotebookViewModel NotebookViewModel { get; private set; }
    }
}
