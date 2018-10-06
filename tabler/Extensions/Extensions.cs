using OfficeOpenXml;

namespace tabler
{
    internal static class Extensions
    {
        public static bool IsEmpty(this ExcelRange cell)
        {
            return string.IsNullOrEmpty(cell.Value?.ToString());
        }
    }
}
