using Handheld_Hardware_Tools.Classes;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;


namespace Handheld_Hardware_Tools.AppWindows.AyaNeoFlipDSApp
{
    /// <summary>
    /// Interaction logic for OSK.xaml
    /// </summary>
    public partial class AyaNeoFlipDSApp : Window
    {

        public AyaNeoFlipDSApp()
        {

            InitializeComponent();
           
            //Move initilize components to sub routine and async it to make pages feel smoother
            Application.Current.Dispatcher.BeginInvoke(new System.Action(() => Initialize()));

        }

        private void Initialize()
        {
           SetLocation();

            frame.Source = new Uri("Pages\\QWERTY_FlipDS.xaml", UriKind.RelativeOrAbsolute);

     
        }



        #region making the app non focusable

        //used in non focus app
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_NOACTIVATE = 0x08000000;

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);


            SetWindowAsNonFocusable();
        }


        private void SetWindowAsNonFocusable()
        {
            //set app as non focusable 
            var helper = new WindowInteropHelper(this);
            SetWindowLong(helper.Handle, GWL_EXSTYLE,
                GetWindowLong(helper.Handle, GWL_EXSTYLE) | WS_EX_NOACTIVATE);
        }




        #endregion

        public void UnloadPageAndHideWindow()
        {
            frame.Content = null;
            frame.Source = null;

        }

       

        private void SetLocation()
        {
           

        }

  
    }
}
