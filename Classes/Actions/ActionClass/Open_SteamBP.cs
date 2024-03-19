
using System.Linq;
using System.Windows;
using Everything_Handhelds_Tool.AppWindows.OSK;
using Everything_Handhelds_Tool.UserControls.PowerPageUserControls;
namespace Everything_Handhelds_Tool.Classes.Actions.ActionClass
{
    public class Open_SteamBP : Action
    {
        public Open_SteamBP()
        {
            actionName = "Open_SteamBP";
            //arguments = new List<string>();
        }
        public override void OnActivate(string quickActionWheelParameter = "")
        {

            Steam_Management.Instance.openSteamBigPicture();
           
        }
      
    }
   
}
