using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmokeNote.Client.Consts
{
    public class DisplayFieldListItem
    {
        private string id = string.Empty;
        private string name = string.Empty;
        public DisplayFieldListItem(string sid, string sname)
        {
            id = sid;
            name = sname;
        }
        public override string ToString()
        {
            return this.name;
        }
        public string ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
    }
}
