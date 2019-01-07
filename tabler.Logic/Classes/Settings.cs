namespace tabler.Logic.Classes
{
    public class Settings
    {
        public Settings()
        {
            // defaults
            IndentationSettings = IndentationSettings.Spaces;
            TabSize = 4;
        }

        public IndentationSettings IndentationSettings { get; set; }

        public int TabSize { get; set; }
    }

    public enum IndentationSettings
    {
        Spaces = 0,
        Tabs = 1
    }
}
