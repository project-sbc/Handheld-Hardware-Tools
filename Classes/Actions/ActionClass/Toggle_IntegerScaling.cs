using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Everything_Handhelds_Tool.Classes.Actions.ActionClass
{
    public class Toggle_IntegerScaling: Action
    {
        public Toggle_IntegerScaling()
        {
            actionName = "Toggle_IntegerScaling";
       
        }

        public override bool UsableOnDevice()
        {
            try
            {
                return ADLX_Management.HasIntegerScalingSupport();
            }
            catch
            {
                return false;
            }
        }

        public override void OnActivate()
        {
            //calls main window toggle window
            if (ADLX_Management.HasIntegerScalingSupport())
            {
                bool intScalingEnabled = ADLX_Management.IsIntegerScalingEnabled();
                if (intScalingEnabled)
                {
                    ADLX_Management.SetIntegerScaling(0);
                    ADLX_Management.SetScalingMode(0);
                }
                else
                {
                    ADLX_Management.SetScalingMode(1);
                    ADLX_Management.SetIntegerScaling(1);
                }
            }
      
        }
    }
}
