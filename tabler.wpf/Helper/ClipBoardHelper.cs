using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using tabler.Logic.Classes;
using tabler.Logic.Helper;

namespace tabler.wpf.Helper
{
    public static class ClipBoardHelper
    {
        private static DataFormat GetDataFormatOfKey()
        {
            return DataFormats.GetDataFormat(typeof(List<Key>).FullName);
        }

        public static void AddKeyObjectsToClipboard(List<Key> obj)
        {
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                Logger.LogInternal($"To access the clipboard, the thread must be STA!");
                return;
            }

            try
            {
                var format = GetDataFormatOfKey();
                IDataObject dataObj = new DataObject();
                dataObj.SetData(format.Name, obj, false);
                Clipboard.SetDataObject(dataObj, false);
                Logger.LogGeneral($"Added to clipboard: {obj.Count} format: {format.Id} format.name:{format.Name}");
            }
            catch (Exception ex)
            {
                Logger.LogEx(ex);
            }
        }

        public static void AddKeyObjectToClipboard(Key obj)
        {
            AddKeyObjectsToClipboard(new List<Key> { obj });
        }

        public static List<Key> GetKeyObjectsFromClipboard()
        {
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                Logger.LogInternal($"To access the clipboard, the thread must be STA!");
                return null;
            }
            List<Key> doc = null;

            try
            {
                var format = GetDataFormatOfKey();
                var dataObj = Clipboard.GetDataObject();
                Logger.LogGeneral($"Get from clipboard: format: {format.Id} format.name:{format.Name} :{Thread.CurrentThread.GetApartmentState()}");
                if (dataObj != null && dataObj.GetDataPresent(format.Name))
                {
                    var o = dataObj.GetData(format.Name);
                    doc = o as List<Key>;
                    Logger.LogGeneral($"Get key from clipboard SUCCESSS ");
                }
                else
                {
                    Logger.LogGeneral($"Get key from clipboard failed :(");
                }

                return doc;
            }
            catch (Exception ex)
            {
                Logger.LogEx(ex);
                return null;
            }

        }

        public static Key GetKeyObjectFromClipboard()
        {
            return GetKeyObjectsFromClipboard().FirstOrDefault();

        }
    }
}
