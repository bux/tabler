using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tabler
{
   public class ModInfoContainer
    {

        public string Name { get; set; }
        public FileInfo FileInfoStringTable { get; set; }

        //Values(ID)(LANGUAGE)
        public Dictionary<string, Dictionary<string, string>> Values = new Dictionary<string,Dictionary<string,string>> ();

    }
}
