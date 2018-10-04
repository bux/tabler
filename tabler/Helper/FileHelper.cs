using System.IO;
using System.Linq;
using System.Text;

namespace tabler
{
    public static class FileHelper
    {
        public static bool FileHasBom(Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                var bytes = ms.ToArray();

                // be nice and reset position
                stream.Position = 0;

                var enc = new UTF8Encoding(true);
                var preamble = enc.GetPreamble();
                return preamble.Where((p, i) => p == bytes[i]).Any();
            }
        }
    }
}
