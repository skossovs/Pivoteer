using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.MVVM
{
    public class Entry
    {
        public int Key { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }

    public class Group
    {
        public Group()
        {
            SubGroups = new List<Group>();
            Entries   = new List<Entry>();
        }
        public int Key { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public IList<Group> SubGroups { get; set; }
        public IList<Entry> Entries { get; set; }
        public IList<object> Items
        {
            get
            {
                IList<object> childNodes = new List<object>();
                foreach (var group in this.SubGroups)
                    childNodes.Add(group);
                foreach (var entry in this.Entries)
                    childNodes.Add(entry);

                return childNodes;
            }
        }
    }
}
