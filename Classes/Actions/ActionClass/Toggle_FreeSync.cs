using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Handheld_Hardware_Tools.Classes.Actions.ActionClass
{
    public class Toggle_FreeSync : Action
    {
        public Toggle_FreeSync()
        {
            actionName = "Toggle_FreeSync";
       
        }

        public override bool UsableOnDevice()
        {
            try
            {
                if (ADLX_Management.HasFreeSyncSupport1() || ADLX_Management.HasFreeSyncSupport2())
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
            if (ADLX_Management.HasFreeSyncSupport1())
            {
                bool freeSyncEnabled = ADLX_Management.IsFreeSyncEnabled1();
                if (freeSyncEnabled)
                {
                    ADLX_Management.SetFreeSync1(0);
               
                }
                else
                {
                    ADLX_Management.SetFreeSync1(1);
                }
            }
            //if second monitor...
            if (ADLX_Management.HasFreeSyncSupport2())
            {
                bool freeSyncEnabled = ADLX_Management.IsFreeSyncEnabled2();
                if (freeSyncEnabled)
                {
                    ADLX_Management.SetFreeSync2(0);

                }
                else
                {
                    ADLX_Management.SetFreeSync2(1);
                }
            }
        }
    }
}
