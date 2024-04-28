using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Handheld_Hardware_Tools
{
    /// <summary>
    /// Interaction logic for SplashScreenStartUp.xaml
    /// </summary>
    public partial class SplashScreenStartUp : Window
    {
        public SplashScreenStartUp()
        {
            InitializeComponent();

            string picLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HH Logo.png");


            //
            image.ImageSource = new BitmapImage(new Uri(picLocation));
        }
    }
}
