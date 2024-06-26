﻿using Handheld_Hardware_Tools.Classes.Profiles.ProfileActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Handheld_Hardware_Tools.Classes.Profiles.ProfileActions.ProfileActionClass;

namespace Handheld_Hardware_Tools.Classes.Profiles
{
    public class Profile
    {
        public string processExe { get; set; } = "";
        public ProfileActionList actionList { get; set; } = new ProfileActionList() { new SetTDP() { ChangeWithPowerState = false, Value = "20" } };
        public string profileName { get; set; } = "";

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
