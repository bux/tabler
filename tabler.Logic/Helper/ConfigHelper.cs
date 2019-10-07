using Polenter.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using tabler.Logic.Classes;

namespace tabler.Logic.Helper
{
    public static class ConfigHelper
    {
        private static readonly FileInfo _fiConfig = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config\config.xml"));

        public static Settings CurrentSettings { get; set; }

        static ConfigHelper()
        {
            //create dir
            if (_fiConfig.Directory != null && _fiConfig.Directory.Exists == false)
            {
                _fiConfig.Directory.Create();
            }
            if (!_fiConfig.Exists)
            {
                CurrentSettings = new Settings();
                SaveSettings();
            }
            else
            {
                LoadSettings();
            }
        }
        public static void SaveSettings()
        {
            if (CurrentSettings == null)
            {
                return;
            }
            var serializer = new SharpSerializer();
            serializer.Serialize(CurrentSettings, _fiConfig.FullName);
        }

        public static void LoadSettings()
        {
            try
            {
                var serializer = new SharpSerializer();
                CurrentSettings = (Settings)serializer.Deserialize(_fiConfig.FullName);
            }
            catch (Exception ex)
            {
                Logger.LogEx(ex);
            }
          
         }
    }
}
