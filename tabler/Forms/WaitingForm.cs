using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace tabler.Forms
{
    public partial class WaitingForm : Form
    {
        private static Thread _waitingThread;
        private static WaitingForm _waitingForm;
        private static Form _parentForm;

        public WaitingForm(Form parentForm)
        {
            InitializeComponent();

            var startX = (parentForm.Width - ClientSize.Width) / 2;
            var startY = (parentForm.Height - ClientSize.Height) / 2;

            Location = new Point(parentForm.Location.X + startX, parentForm.Location.Y + startY);
            Location = new Point(parentForm.Location.X + startX, parentForm.Location.Y + startY);

            progressBar1.MarqueeAnimationSpeed = 30;
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
        }

        public static void ShowForm(Form parentForm)
        {
            if (_waitingThread != null )
            {
                CloseForm();
            }
           
            _parentForm = parentForm;
            _waitingThread = new Thread(ThreadTask) {IsBackground = false};
            _waitingThread.Start();
            
        }

        private static void ThreadTask(object owner)
        {
            _waitingForm = new WaitingForm(_parentForm);
            _waitingForm.Owner = (Form) owner;
            Application.Run(_waitingForm);
        }

        public static void CloseDialogDown()
        {
            Application.ExitThread();
        }

        public static void CloseForm()
        {
            while (_waitingForm == null || !_waitingForm.IsHandleCreated)
            {
                Thread.Sleep(10);
            }

            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);

            var mi = new MethodInvoker(CloseDialogDown);
            _waitingForm.Invoke(mi);
        }
    }
}
