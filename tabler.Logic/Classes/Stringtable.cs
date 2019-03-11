using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace tabler.Logic.Classes
{
    public class Stringtable
    {

        public string Name { get; set; }

        public FileInfo File { get; set; }

        public Project Project { get; set; }

        public bool HasChanges { get; set; }

        public bool FileHasBom { get; set; }

        public IEnumerable<Key> AllKeys => Project?.Packages?.SelectMany(p =>
        {
            var keys = new List<Key>();

            if (p.Containers.Any())
            {
                keys.AddRange(p.Containers.SelectMany(c => c.Keys));
            }

            keys.AddRange(p.Keys);

            return keys;
        });
        
    }
}
