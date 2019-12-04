namespace tabler.Logic.Helper
{
    public delegate void CurrentProgressChanged(int progress, string currentAction);
    public interface IProgress
    {
        int CurrentProgressValue { get; set; }
        event CurrentProgressChanged ProgressChangedEvent;
        string CurrentAction { get; set; }
    }

    public  class ProgressHelper : IProgress
    {
        public int CurrentProgressValue { get; set; }
        public string CurrentAction { get; set; }

        public event CurrentProgressChanged ProgressChangedEvent;

        public void FireProgressChangedEvent(int progress, string currentAction)
        {
            CurrentProgressValue = progress;
            CurrentAction = currentAction;
            ProgressChangedEvent?.Invoke(progress, currentAction);
        }
        public void FireProgressNameChangedEvent(string currentAction)
        {
            FireProgressChangedEvent(CurrentProgressValue, currentAction);
        }
        public void FireProgressChangedEvent(int progress)
        {
            FireProgressChangedEvent(progress, CurrentAction);
        }

    }

}
