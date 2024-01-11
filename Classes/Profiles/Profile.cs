using Everything_Handhelds_Tool.Classes.Profiles.ProfileActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Everything_Handhelds_Tool.Classes.Profiles
{
    public class Profile
    {
        public string processExe;
        public ProfileActionList actionList;


        public virtual void ApplyProfile()
        {
            if (actionList != null)
            {
                if (actionList.Count > 0)
                {
                    PowerLineStatus status = new PowerLineStatus();
                    foreach(ProfileAction action in actionList)
                    {
                        action.ExecuteAction(status.ToString());
                    }
                }
            }
        }
    }
}
