using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace tabler.wpf.Controls
{
    /// <summary>
    /// Interaction logic for Helper_LabelTextBox.xaml
    /// </summary>
    public partial class Helper_LabelTextBox : UserControl
    {
        public Helper_LabelTextBox()
        {
            InitializeComponent();
           
            TextBox.MouseLeftButtonUp += (sender, args) =>
            {
                if (DoClearOnClickIn || DoClearOnClickInOnce)
                {
                    TextBox.Text = string.Empty;
                    DoClearOnClickInOnce = false;
                }
            };
        }

        public static readonly DependencyProperty DoClearOnClickInOnceProperty = DependencyProperty.Register(
            "DoClearOnClickInOnce", typeof (bool), typeof (Helper_LabelTextBox), new PropertyMetadata(default(bool)));

        public bool DoClearOnClickInOnce
        {
            get { return (bool) GetValue(DoClearOnClickInOnceProperty); }
            set { SetValue(DoClearOnClickInOnceProperty, value); }
        }

        public static readonly DependencyProperty DoClearOnClickInProperty = DependencyProperty.Register(
            "DoClearOnClickIn", typeof (bool), typeof (Helper_LabelTextBox), new PropertyMetadata(default(bool)));

        public bool DoClearOnClickIn
        {
            get { return (bool) GetValue(DoClearOnClickInProperty); }
            set { SetValue(DoClearOnClickInProperty, value); }
        }
        public static readonly DependencyProperty IsReadonlyProperty = DependencyProperty.Register("IsReadonly", typeof(bool), typeof(Helper_LabelTextBox), new PropertyMetadata(default(bool)));

        public bool IsReadonly
        {
            get { return (bool)GetValue(IsReadonlyProperty); }
            set { SetValue(IsReadonlyProperty, value);}
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(Helper_LabelTextBox), new PropertyMetadata(default(string)));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(Helper_LabelTextBox), new PropertyMetadata(default(string)));



        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        //public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        //    "Description", typeof (string), typeof (Helper_LabelTextBox), new PropertyMetadata(default(string)));

        //public string Description
        //{
        //    get { return (string) GetValue(DescriptionProperty); }
        //    set { SetValue(DescriptionProperty, value); }
        //}


        //public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        //    "Value", typeof (string), typeof (Helper_LabelTextBox), new PropertyMetadata(default(string)));

        //public string Value
        //{
        //    get { return (string) GetValue(ValueProperty); }
        //    set { SetValue(ValueProperty, value); }
        //}

        private void keyPressedinTextbox(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EnterPressed?.Invoke(Value);
            }
        }

        public delegate void EnterPressedDelegate(string value);
        public event EnterPressedDelegate EnterPressed;

        public delegate void CharactersEnteredCountDelegate(int value);
        public event CharactersEnteredCountDelegate CharactersEnteredCount;

        public delegate void ThreeOrMoreCharactersEnteredDelegate(string value);
        public event ThreeOrMoreCharactersEnteredDelegate ThreeOrMoreCharactersEntered;


        public TextBox TextBox => TbValue;
        public Label Label => LblDescription;

        private void TbValue_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Value = TextBox.Text;
            var value = TextBox.Text;

            if (string.IsNullOrEmpty(value))
            {
                value = String.Empty;
            }

            CharactersEnteredCount?.Invoke(value.Length);

            if (value.Length >= 3)
            {
                ThreeOrMoreCharactersEntered?.Invoke(value);
            }

        }
    }
}
