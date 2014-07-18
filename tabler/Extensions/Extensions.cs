using OfficeOpenXml;

namespace tabler
{
    internal static class Extensions
    {
        public static bool IsEmpty(this ExcelRange cell)
        {
            return ((cell.Value == null) || string.IsNullOrEmpty(cell.Value.ToString()));
        }
    }
}