namespace tabler.Logic.Classes
{
    public class Settings
    {
        public Settings()
        {
            // defaults
            IndentationSettings = IndentationSettings.Spaces;
            TabSize = 4;
            Language = "de-DE";
            LastPathOfDataFiles = "";
            RemoveEmptyNodes = false;
        }

        public IndentationSettings IndentationSettings { get; set; }
        public int TabSize { get; set; }
        public string Language { get; set; }
        public string LastPathOfDataFiles { get; set; }
        public bool RemoveEmptyNodes { get; set; }



    }

    public enum IndentationSettings
    {
        Spaces = 0,
        Tabs = 1
    }
}
