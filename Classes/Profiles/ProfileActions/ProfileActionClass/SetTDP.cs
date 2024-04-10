using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Handheld_Hardware_Tools.Classes.Profiles.ProfileActions.ProfileActionClass
{

    public class SetTDP : ProfileAction
    {
        public override void ExecuteAction(string powerStatus)
        {
            int tdp;
            string sendValue = Value;

            //if the bool Changewithpowerstate is true we need to determine if its value or OnbatteryValue, otherwise we can just do the
            //tryparse with value
            if (ChangeWithPowerState)
            {
                //to keep the logic streamlined we ONLY need to change sendValue when both the bool ChangeWithPowerState AND the power state is offline 
                PowerLineStatus status = new PowerLineStatus();
                if (status.ToString() == "Offline")
                {
                    sendValue = OnBatteryValue;
                }
            }
            //the sendValue parameter is generic so we can set it based on the criteria above, rather than have multiple if statements with tryparse
            if (int.TryParse(sendValue, out tdp))
            {
                TDP_Management.Instance.ChangeSustainedBoostTDP(tdp, tdp);
            }

        }
    }
}
