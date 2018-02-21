using System;
using System.Windows;
using System.Windows.Forms;

namespace PhotoshopBeepFix
{
    public partial class MainWindow : Window
    {
        private const string PhotoshopWndClassName = "Photoshop";

        private KeyboardHookController _keyboardController;
        private IntPtr photoshopHandle = IntPtr.Zero;
        private bool altDebounce = false;
        private Context _context;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = _context = new Context();
            _keyboardController = new KeyboardHookController(OnGlobalKeyDown, OnGlobalKeyUp);

            FocusOnPhotoshop();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _keyboardController.Dispose();
        }

        private void FocusOnPhotoshop()
        {
            photoshopHandle = WindowHandler.FindWindow(PhotoshopWndClassName, null);
            WindowHandler.SetForegroundWindow(photoshopHandle);
        }

        private void OnGlobalKeyDown(Keys key) { }

        private void OnGlobalKeyUp(Keys key)
        {
            HideAltMenusInPhotoshop(key);
        }

        private void HideAltMenusInPhotoshop(Keys key)
        {
            if (key != Keys.LMenu || altDebounce)
                return;

            photoshopHandle = WindowHandler.FindWindow(PhotoshopWndClassName, null);

            if (WindowHandler.IsWindowInFocus(photoshopHandle))
            {
                altDebounce = true;
                SendKeys.SendWait("^%");
                altDebounce = false;
            }
        }
    }
}
