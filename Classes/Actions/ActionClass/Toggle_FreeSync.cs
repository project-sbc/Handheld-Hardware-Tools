using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Everything_Handhelds_Tool.Classes.Actions.ActionClass
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
                return ADLX_Management.HasFreeSyncSupport();
            }
            catch
            {
                return false;
            }
        }

        public override void OnActivate(string quickActionWheelParameter = "")
        {
            //calls main window toggle window
            if (ADLX_Management.HasFreeSyncSupport())
            {
                bool freeSyncEnabled = ADLX_Management.IsFreeSyncEnabled();
                if (freeSyncEnabled)
                {
                    ADLX_Management.SetFreeSync(0);
               
                }
                else
                {
                    ADLX_Management.SetFreeSync(1);
                }
            }
      
        }
    }
}
