using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WindowsInput;
using Shell32;
using System.Diagnostics.Eventing.Reader;
namespace Everything_Handhelds_Tool.Classes.Actions.ActionClass
{
    public class Toggle_Desktop : Action
    {
        public Toggle_Desktop()
        {
            actionName = "Toggle_Desktop";
            //arguments = new List<string>();
        }
        public override void OnActivate()
        {
            //Shell objects need to run on STA thread, thats why i need to use a new thread, otherwise it wont work

            if (1 == 1)
            {

                Thread thread = new Thread(ToggleDesktop);
                thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
                thread.Start();
                Debug.WriteLine("Invoking togg desk");
                //Application.Current.Dispatcher.BeginInvoke(() => ToggleDesktop());
            }
           
       


        }

        private async void ToggleDesktop()
        {
            var objShel = new Shell();
            // Show the desktop
            objShel.ToggleDesktop();
      
            objShel = null;
        }
       
    }
    
}
