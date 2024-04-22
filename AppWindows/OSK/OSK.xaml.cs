using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;


namespace Handheld_Hardware_Tools.AppWindows.OSK
{
    /// <summary>
    /// Interaction logic for OSK.xaml
    /// </summary>
    public partial class OSK : Window
    {

        public OSK()
        {
            
            InitializeComponent();
           
            //Move initilize components to sub routine and async it to make pages feel smoother
            Dispatcher.BeginInvoke(new System.Action(() => Initialize()));

        }

        private void Initialize()
        {
            SetLocation();

            frame.Source = new Uri("Keyboards\\QWERTY.xaml", UriKind.RelativeOrAbsolute);

     
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

        public void UpdateOutlinePreviewText(string text)
        {
            if (text != null)
            {
                if (text.Length > 1)
                {
                    switch (text)
                    {
                        case "ENTER":
                            outlineTextblock.Text = "";
                            break;
                        case "BACKSPACE":
                            if (outlineTextblock.Text != null)
                            {
                                if (outlineTextblock.Text.Length > 0)
                                {
                                    outlineTextblock.Text = outlineTextblock.Text.Substring(0, outlineTextblock.Text.Length - 1);
                                }
                            }
                            break;
                        case "SPACE":
                            outlineTextblock.Text = outlineTextblock.Text + " ";
                            break;
                        case "LBracket":
                            outlineTextblock.Text = outlineTextblock.Text + "[";
                            break;
                        case "RBracket":
                            outlineTextblock.Text = outlineTextblock.Text + "]";
                            break;
                        case "Slash":
                            outlineTextblock.Text = outlineTextblock.Text + "\\";
                            break;
                    }
                }
                else
                {
                    outlineTextblock.Text = outlineTextblock.Text + text;
                }

                if (outlineTextblock.ActualWidth > frame.ActualWidth * 0.4 && outlineTextblock.Text != null)
                {
                    if (outlineTextblock.Text.Length > 0)
                    {
                        outlineTextblock.Text = outlineTextblock.Text.Substring(1, outlineTextblock.Text.Length - 1);
                    }

                }
            }
           
        }

        public void ToggleOutlineTextBlock()
        {
            if (outlineTextblock.Visibility == Visibility.Visible)
            {
                outlineTextblock.Text = "";
                outlineTextblock.Visibility = Visibility.Collapsed;
            }
            else
            {
                outlineTextblock.Visibility = Visibility.Visible;
            }
        }

        private void SetLocation()
        {
            this.Width = SystemParameters.FullPrimaryScreenWidth;
            this.Height = Math.Round(SystemParameters.PrimaryScreenHeight * 0.5, 0);
            this.Top = SystemParameters.PrimaryScreenHeight - Math.Round(SystemParameters.PrimaryScreenHeight * 0.5, 0);


        }

  
    }
}
