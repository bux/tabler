using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MS.Internal.Annotations;
using tabler.Logic.Classes;
using tabler.Logic.Extensions;

namespace tabler.wpf.Container
{
    [DataContract]
    [Serializable] //for copy paste action
    public class Key_ExtendedWithChangeTracking
    {
        [DataMember]
        public Dictionary<string, ChangeTrackerString> Languages { get; set; } = new Dictionary<string, ChangeTrackerString>();
        [DataMember]
        public Dictionary<string, ChangeTrackerString> SystemValues { get; set; } = new Dictionary<string, ChangeTrackerString>();

        private const string IsComplete_PropertyName = "IsComplete";
        private const string Id_PropertyName = "Id";
        private const string Original_PropertyName = "Original";
        private const string PackageName_PropertyName = "PackageName";
        private const string ContainerName_PropertyName = "ContainerName";
        
        public Key_ExtendedWithChangeTracking()
        {

        }

        public Key_ExtendedWithChangeTracking(Key key)
        {
            SystemValues.Add(nameof(key.Id),new ChangeTrackerString( key.Id));
            SystemValues.Add(nameof(key.Original), new ChangeTrackerString(key.Original));
            SystemValues.Add(nameof(key.ContainerName), new ChangeTrackerString(key.ContainerName));
            SystemValues.Add(nameof(key.PackageName), new ChangeTrackerString(key.PackageName));
            SystemValues.Add(IsCompletePropertyName, new ChangeTrackerString("false"));

            Languages.Add(nameof(key.Chinese), new ChangeTrackerString(key.Chinese));
            Languages.Add(nameof(key.Chinesesimp), new ChangeTrackerString(key.Chinesesimp));
            Languages.Add(nameof(key.Czech), new ChangeTrackerString(key.Czech));
            Languages.Add(nameof(key.English), new ChangeTrackerString(key.English));
            Languages.Add(nameof(key.French), new ChangeTrackerString(key.French));
            Languages.Add(nameof(key.German), new ChangeTrackerString(key.German));
            Languages.Add(nameof(key.Hungarian), new ChangeTrackerString(key.Hungarian));
            Languages.Add(nameof(key.Italian), new ChangeTrackerString(key.Italian));
            Languages.Add(nameof(key.Japanese), new ChangeTrackerString(key.Japanese));
            Languages.Add(nameof(key.Korean), new ChangeTrackerString(key.Korean));
            Languages.Add(nameof(key.Polish), new ChangeTrackerString(key.Polish));
            Languages.Add(nameof(key.Portuguese), new ChangeTrackerString(key.Portuguese));
            Languages.Add(nameof(key.Russian), new ChangeTrackerString(key.Russian));
            Languages.Add(nameof(key.Spanish), new ChangeTrackerString(key.Spanish));
            Languages.Add(nameof(key.Turkish), new ChangeTrackerString(key.Turkish));
        }

        public void Update_IsCompleted(List<string> languagesToHave)
        {
            foreach (var language in languagesToHave)
            {
                if (string.IsNullOrEmpty(Languages[language].CurrentValue))
                {
                    SystemValues[IsCompletePropertyName].CurrentValue = "false";
                    return;
                };
            }

            SystemValues[IsCompletePropertyName].CurrentValue = "true";
        }

        public bool ContainsText(string value, bool searchOnlyInId, bool ignoreCase)
        {
            var comp = StringComparison.InvariantCulture;

            if (ignoreCase)
            {
                comp = StringComparison.InvariantCultureIgnoreCase;
            }

            if (SystemValues[Id_PropertyName].CurrentValue.ContainsEx(value, comp))
            {
                return true;
            }

            if (searchOnlyInId)
            {
                return false;
            }

            foreach (var language in Languages.Values)
            {
                if (language.CurrentValue.ContainsEx(value, comp))
                {
                    return true;
                }
            }
            

            return false;
        }

        internal XElement AsXElement(bool ignoreEmptyValues, List<string> languagesToWrite)
        {
            var res = new XElement("Key", new XAttribute("ID", SystemValues[Id_PropertyName].CurrentValue));

            foreach (var item in languagesToWrite)
            {
                AddIfNecessary(res, item, Languages[item].CurrentValue, ignoreEmptyValues);
            }

            return res;
        }

        private void AddIfNecessary(XElement parent, string nameOfKey, string valueOfKey, bool ignoreEmptyValues)
        {
            if (ignoreEmptyValues && string.IsNullOrEmpty(valueOfKey))
            {
                return;
            }

            parent.Add(new XElement(nameOfKey, valueOfKey));
        }
    }

    //todo something generic
    public class ChangeTrackerString : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string OriginalValue { get; set; }
        private string _currentValue;
        private bool _hasChanged;

        public ChangeTrackerString(string originalValue)
        {
            OriginalValue = originalValue;
            _currentValue = OriginalValue;
        }

        private void RaisePropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public bool HasChanged { get => _hasChanged; set => _hasChanged = value; }


        public string CurrentValue
        {
            get { return _currentValue; }
            set
            {
                if (OriginalValue != value)
                {

                    _currentValue = value;
                    RaisePropertyChanged(nameof(CurrentValue));

                    if (HasChanged == false)
                    {
                        HasChanged = true;
                        RaisePropertyChanged(nameof(HasChanged));
                    }

                }
                else
                {
                    if (HasChanged == true)
                    {
                        HasChanged = false;
                        RaisePropertyChanged(nameof(HasChanged));
                    }

                }
            }
        }

    }


}
