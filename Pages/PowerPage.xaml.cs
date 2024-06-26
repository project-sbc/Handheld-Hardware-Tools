﻿
using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using Handheld_Hardware_Tools.Classes.Models;
using Handheld_Hardware_Tools.UserControls.HomePageUserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Handheld_Hardware_Tools.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class PowerPage : ControllerPage
    {
        
        public PowerPage()
        {
            //Move initilize components to sub routine and async it to make pages feel smoother
            Application.Current.Dispatcher.BeginInvoke(new Action(() => Initialize()));

        }

        private void Initialize()
        {
            InitializeComponent();
            virtualStackPanel = stackPanel;
            AddControlsToArray();
        }
        private void AddControlsToArray()
        {
            //There is a general function that searches the stack panel and adds to the list of ControllerUserControls. It makes sure
            //visibility isn't collapsed too
            userControls= General_Functions.SearchStackPanelReturnArray(stackPanel);
        }



    }
}
