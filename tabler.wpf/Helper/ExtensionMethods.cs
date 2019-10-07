using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Polenter.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace tabler.wpf.Helper
{
    public static class ExtensionMethods
    {
        #region Rounding numbers

        public static Double SafeRound(this Double value, int decimalPlaces)
        {
            return Math.Round(value, decimalPlaces);
        }

        public static Double SafeRound(this Double value)
        {
            return Math.Round(value, 6);
        }

        public static float SafeRound(this float value)
        {
            return (float)Math.Round(value, 6);
        }

        public static float SafeRound(this float value, int decimalPlaces)
        {
            return (float)Math.Round(value, decimalPlaces);
        }

        public static decimal SafeRound(this decimal value)
        {
            return Math.Round(value, 6);
        }

        public static decimal SafeRound(this decimal value, int decimalPlaces)
        {
           return Math.Round(value, decimalPlaces);
        }

        #endregion
        public static String ToInvariantString(this Double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static String ToInvariantString(this float value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static string GetDateTimeString_yyyyMMdd(this DateTime timestamp)
        {
            return timestamp.ToString("yyyy.MM.dd");
        }

        public static string GetDateTimeString_yyyyMMddHH(this DateTime timestamp)
        {
            return timestamp.ToString("yyyy.MM.dd HH");
        }

        public static string GetDateTimeString_yyyyMM(this DateTime timestamp)
        {
            return timestamp.ToString("yyyy.MM");
        }

        public static string GetDateTimeString_yyyyMMddhhmmssFtpReady(this DateTime timestamp)
        {
            return timestamp.ToString("yyyy_MM_dd_HH-mm-ss");
        }


        public static string GetDateTimeString_ddMMyyyyhhmmss(this DateTime timestamp)
        {
            // 2016-05-19 15:16:17
            return timestamp.ToString("dd-MM-yyyy HH:mm:ss");
        }

        public static string GetDateTimeString_yyyyMMddhhmmssFileName(this DateTime timestamp)
        {
            return timestamp.ToString("yyyy.MM.dd_HH-mm-ss");
        }

        public static string GetDateTimeString_yyyyMMddhhmmssms(this DateTime timestamp)
        {
            return timestamp.ToString("yyyy.MM.dd HH:mm:ss:ffff");
        }
        public static string GetDateTimeString_yyyyMMddhhmmss(this DateTime timestamp)
        {
            return timestamp.ToString("yyyy.MM.dd HH:mm:ss");
        }

        public static double ToDouble(this TextBox source, double defaultValue = 0)
        {
            double result = defaultValue;

            if (double.TryParse(source.Text, out result) == false)
            {
                result = defaultValue;
            }

            return result;
        }

        public static int ToInt(this TextBox source, int defaultValue = 0)
        {
            int result = defaultValue;

            if (int.TryParse(source.Text, out result) == false)
            {
                result = defaultValue;
            }

            return result;
        }
        public static byte ToByte(this TextBox source, byte defaultValue = 0)
        {
            byte result = defaultValue;

            if (byte.TryParse(source.Text, out result) == false)
            {
                result = defaultValue;
            }

            return result;
        }
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            using (var enumerator = source.GetEnumerator())
                while (enumerator.MoveNext())
                    yield return YieldBatchElements(enumerator, batchSize - 1);
        }

        private static IEnumerable<T> YieldBatchElements<T>(
            IEnumerator<T> source, int batchSize)
        {
            yield return source.Current;
            for (int i = 0; i < batchSize && source.MoveNext(); i++)
                yield return source.Current;
        }


        public static bool ExistsInString(this String value,
                             List<string> searchValues,
                             List<string> searchValuesNegative,
                             bool doOr,
                             bool doOrNegative,
                             ref List<String> valuesThatMatched)
        {

            var validRow = false;

            if (searchValues == null)
            {
                searchValues = new List<string>();
            }
            if (searchValuesNegative == null)
            {
                searchValuesNegative = new List<string>();
            }

            //search positive
            if (searchValues.Any() == false)
            {
                validRow = true;
            }
            else
            {
                if (doOr)
                {
                    //OR

                    foreach (string keyword in searchValues)
                    {
                        string temp = keyword.ToUpperInvariant();

                        if (value.Contains(temp))
                        {
                            valuesThatMatched.Add(temp);
                            validRow = true;
                            //dont break here, beacuse maybe multiple filter are valid
                            //break;
                        }
                    }



                }
                else
                {

                    //AND
                    foreach (string keyword in searchValues)
                    {
                        string temp = keyword.ToUpperInvariant();

                        validRow = true;

                        if (value.Contains(temp) == false)
                        {
                            validRow = false;
                            break;
                        }
                        else
                        {
                            valuesThatMatched.Add(temp);
                        }
                    }


                }
            }


            if (validRow == false)
            {
                return false;
            }

            //SEARCH NEGATIVE
            if (searchValuesNegative.Any() == false)
            {
                validRow = true;
            }
            else
            {
                if (doOrNegative)
                {
                    //or neagtive
                    if (value.ContainsAny(searchValuesNegative))
                    {
                        validRow = false;
                    }
                }
                else
                {
                    //and negative
                    if (value.ContainsAll(searchValuesNegative))
                    {
                        validRow = false;
                    }
                }

            }

            return validRow;
        }

        public static DateTime GetPreviousWorkDay(this DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return date.AddDays(-2);
                case DayOfWeek.Monday:
                    return date.AddDays(-3);
                default:
                    return date.AddDays(-1);
            }
        }


        public static DateTime GetLastWeekday(this DateTime date, DayOfWeek weekDay)
        {
            DateTime lastDay = date.AddDays(-1);
            while (lastDay.DayOfWeek != weekDay)
            {
                lastDay = lastDay.AddDays(-1);
            }
            return lastDay;
        }

        public static DateTime GetNextWeekday(this DateTime date, DayOfWeek weekDay)
        {
            DateTime lastDay = date.AddDays(1);
            while (lastDay.DayOfWeek != weekDay)
            {
                lastDay = lastDay.AddDays(1);
            }
            return lastDay;
        }



        public static List<T> GetSelectedBoundItems<T>(this System.Windows.Controls.DataGrid dgv)
        {
            List<T> result = null;

            dgv.Dispatcher.Invoke(() =>
            {
                //check full rows
                result = dgv.SelectedItems.OfType<System.Windows.Controls.DataGridRow>()
                                     .Select(x => x.Item)
                                     .OfType<T>().Distinct().ToList();


                if (result.Any() == false)
                {
                    //check in cells
                    result = dgv.SelectedCells.OfType<DataGridCellInfo>()
                                     .Select(x => x.Item)
                                     .OfType<T>().Distinct().ToList();
                }
            });

            return result;
        }


        public static string Compress(this string uncompressedString)
        {
            var compressedStream = new MemoryStream();
            var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString));

            using (var compressorStream = new DeflateStream(compressedStream, CompressionMode.Compress, true))
            {
                uncompressedStream.CopyTo(compressorStream);
            }

            return Convert.ToBase64String(compressedStream.ToArray());
        }

        /// <summary>
        /// Decompresses a deflate compressed, Base64 encoded string and returns an uncompressed string.
        /// </summary>
        /// <param name="compressedString">String to decompress.</param>
        public static string Decompress(this string compressedString)
        {
            var decompressedStream = new MemoryStream();
            var compressedStream = new MemoryStream(Convert.FromBase64String(compressedString));

            using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
            {
                decompressorStream.CopyTo(decompressedStream);
            }

            return Encoding.UTF8.GetString(decompressedStream.ToArray());
        }

        public static byte[] SerializeAndCompress(this object obj)
        {
            var ser = new SharpSerializer(new SharpSerializerBinarySettings()
            {
                IncludeAssemblyVersionInTypeName = false,
                IncludePublicKeyTokenInTypeName = false,
                Mode = BinarySerializationMode.SizeOptimized
            });

            using (MemoryStream ms = new MemoryStream())
            {
                ser.Serialize(obj, ms);
                return ms.ToArray();
            }
        }


        /// <summary>
        /// zipLevel low:0 - high:9 !
        /// </summary>
        /// <param name="memStreamIn"></param>
        /// <param name="zipEntryName"></param>
        /// <param name="zipLevel"></param>
        /// <returns></returns>
        public static MemoryStream ZipMemoryStream(this FileStream memStreamIn, string zipEntryName, int zipLevel = 3)
        {

            MemoryStream outputMemStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

            zipStream.SetLevel(zipLevel); //0-9, 9 being the highest level of compression

            ZipEntry newEntry = new ZipEntry(zipEntryName);
            newEntry.DateTime = DateTime.Now;

            zipStream.PutNextEntry(newEntry);

            StreamUtils.Copy(memStreamIn, zipStream, new byte[4096]);
            zipStream.CloseEntry();

            zipStream.IsStreamOwner = false;    // False stops the Close also Closing the underlying stream.
            zipStream.Close();          // Must finish the ZipOutputStream before using outputMemStream.

            outputMemStream.Position = 0;
            return outputMemStream;

            // Alternative outputs:
            // ToArray is the cleaner and easiest to use correctly with the penalty of duplicating allocated memory.
            //byte[] byteArrayOut = outputMemStream.ToArray();
            //return byteArrayOut;
            //// GetBuffer returns a raw buffer raw and so you need to account for the true length yourself.
            //byte[] byteArrayOut = outputMemStream.GetBuffer();
            //long len = outputMemStream.Length;
        }
        public static MemoryStream ZipMemoryStream(this List<FileInfo> files, int zipLevel = 3)
        {

            MemoryStream outputMemStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

            zipStream.SetLevel(zipLevel); //0-9, 9 being the highest level of compression

            foreach (var fileInfo in files)
            {
                ZipEntry newEntry = new ZipEntry(fileInfo.Name);
                newEntry.DateTime = fileInfo.LastWriteTime.ToUniversalTime();

                zipStream.PutNextEntry(newEntry);

                StreamUtils.Copy(fileInfo.OpenRead(), zipStream, new byte[4096]);
                zipStream.CloseEntry();

            }

            zipStream.IsStreamOwner = false;    // False stops the Close also Closing the underlying stream.
            zipStream.Close();          // Must finish the ZipOutputStream before using outputMemStream.

            outputMemStream.Position = 0;
            return outputMemStream;

            // Alternative outputs:
            // ToArray is the cleaner and easiest to use correctly with the penalty of duplicating allocated memory.
            //byte[] byteArrayOut = outputMemStream.ToArray();
            //return byteArrayOut;
            //// GetBuffer returns a raw buffer raw and so you need to account for the true length yourself.
            //byte[] byteArrayOut = outputMemStream.GetBuffer();
            //long len = outputMemStream.Length;
        }
        /// <summary>
        /// zipLevel low:0 - high:9 !
        /// </summary>
        /// <param name="memStreamIn"></param>
        /// <param name="zipEntryName"></param>
        /// <param name="zipLevel"></param>
        /// <returns></returns>
        public static byte[] ZipMemoryStream(this MemoryStream memStreamIn, string zipEntryName, int zipLevel)
        {

            MemoryStream outputMemStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

            zipStream.SetLevel(zipLevel); //0-9, 9 being the highest level of compression

            ZipEntry newEntry = new ZipEntry(zipEntryName);
            newEntry.DateTime = DateTime.Now;

            zipStream.PutNextEntry(newEntry);

            StreamUtils.Copy(memStreamIn, zipStream, new byte[4096]);
            zipStream.CloseEntry();

            zipStream.IsStreamOwner = false;    // False stops the Close also Closing the underlying stream.
            zipStream.Close();          // Must finish the ZipOutputStream before using outputMemStream.

            outputMemStream.Position = 0;
            //return outputMemStream;

            // Alternative outputs:
            // ToArray is the cleaner and easiest to use correctly with the penalty of duplicating allocated memory.
            byte[] byteArrayOut = outputMemStream.ToArray();
            return byteArrayOut;
            //// GetBuffer returns a raw buffer raw and so you need to account for the true length yourself.
            //byte[] byteArrayOut = outputMemStream.GetBuffer();
            //long len = outputMemStream.Length;
        }
        public static List<FileInfo> UnzipFromStream(this Stream zipStream, string outFolder, IProgressBarControl progress = null)
        {
            ZipInputStream zipInputStream = new ZipInputStream(zipStream);
            ZipEntry zipEntry = zipInputStream.GetNextEntry();

            var lstFiles = new List<FileInfo>();

            while (zipEntry != null)
            {
                progress?.Increase(1);
                progress?.SetOperationName($"Extracting file: {zipEntry.Name}");
                String entryFileName = zipEntry.Name;
                // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                // Optionally match entrynames against a selection list here to skip as desired.
                // The unpacked length is available in the zipEntry.Size property.

                byte[] buffer = new byte[4096];     // 4K is optimum

                // Manipulate the output filename here as desired.
                String fullZipToPath = Path.Combine(outFolder, entryFileName);
                string directoryName = Path.GetDirectoryName(fullZipToPath);
                if (directoryName.Length > 0)
                    Directory.CreateDirectory(directoryName);

                // Skip directory entry
                string fileName = Path.GetFileName(fullZipToPath);
                if (fileName.Length == 0)
                {
                    zipEntry = zipInputStream.GetNextEntry();
                    continue;
                }

                // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                // of the file, but does not waste memory.
                // The "using" will close the stream even if an exception occurs.
                using (FileStream streamWriter = File.Create(fullZipToPath))
                {
                    StreamUtils.Copy(zipInputStream, streamWriter, buffer);
                }

                var fi = new FileInfo(fullZipToPath);
                if (fi.Exists)
                {
                    File.SetLastWriteTimeUtc(fullZipToPath, zipEntry.DateTime);
                    lstFiles.Add(fi);
                }

                zipEntry = zipInputStream.GetNextEntry();
            }
            return lstFiles;
        }
        public static T DecompressAndDeserialize<T>(this byte[] data)
        {
            var ser = new SharpSerializer(new SharpSerializerBinarySettings()
            {
                IncludeAssemblyVersionInTypeName = false,
                IncludePublicKeyTokenInTypeName = false,
                Mode = BinarySerializationMode.SizeOptimized
            });

            using (MemoryStream ms = new MemoryStream(data))
            {
                return (T)ser.Deserialize(ms);

            }

        }


        /// <summary>
        /// Determines whether the specified value contains all.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="lstValues">The LST values.</param>
        public static Boolean ContainsAll(this string value, List<string> lstValues)
        {
            foreach (var lstValue in lstValues)
            {

                if (value.Contains(lstValue) == false)
                {
                    return false;
                }

            }

            return true;

        }

        public static SortableBindingList<T> ToSortableBindingList<T>(this IEnumerable<T> values)
        {

            var newList = new SortableBindingList<T>();
            newList.AddRange(values);

            return newList;

        }

        public static Boolean ContainsAny(this string value, List<string> lstValues)
        {
            foreach (var lstValue in lstValues)
            {
                if (value.Contains(lstValue) == true)
                {
                    return true;
                }
            }
            return false;
        }


        public static void ShowInNewWindow(this Control control, bool isModal, Control parent, string title)
        {
            System.Windows.Window parentWindow = null;
            if (parent != null)
            {
                parentWindow = System.Windows.Window.GetWindow(parent);
            }

            control.ShowInNewWindow(isModal, parentWindow, title);
        }
        public static void ShowInNewWindow(this Control control, bool isModal, Window parent, string title)
        {
            var win = new Window();
            win.Title = title;
            if (parent != null)
            {

                win.Owner = parent;
            }

            win.Content = control;

            if (isModal)
            {
                win.ShowDialog();
            }
            else
            {
                win.Show();
            }


        }

    }
}
