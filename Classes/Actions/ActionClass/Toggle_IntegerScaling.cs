using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Handheld_Hardware_Tools.Classes.Actions.ActionClass
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
                if (ADLX_Management.HasIntegerScalingSupport1() || ADLX_Management.HasIntegerScalingSupport2())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public override void OnActivate(string quickActionWheelParameter = "")
        {
            //if first monitor
            if (ADLX_Management.HasIntegerScalingSupport1())
            {
                bool intScalingEnabled = ADLX_Management.IsIntegerScalingEnabled1();
                if (intScalingEnabled)
                {
                    ADLX_Management.SetIntegerScaling1(0);
                    ADLX_Management.SetScalingMode1(0);
                }
                else
                {
                    ADLX_Management.SetScalingMode1(1);
                    ADLX_Management.SetIntegerScaling1(1);
                }
            }
            //if second monitor
            if (ADLX_Management.HasIntegerScalingSupport2())
            {
                bool intScalingEnabled = ADLX_Management.IsIntegerScalingEnabled2();
                if (intScalingEnabled)
                {
                    ADLX_Management.SetIntegerScaling2(0);
                    ADLX_Management.SetScalingMode2(0);
                }
                else
                {
                    ADLX_Management.SetScalingMode2(1);
                    ADLX_Management.SetIntegerScaling2(1);
                }
            }
        }
    }
}
