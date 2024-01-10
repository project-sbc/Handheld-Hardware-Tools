using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everything_Handhelds_Tool.Classes.Profiles.ProfileActions
{
    public class SetTDP : ProfileAction
    {
        public override void ExecuteAction()
        {
            int tdp;
            if (int.TryParse(Value, out tdp))
            {
                TDP_Management.Instance.ChangeSustainedBoostTDP(tdp, tdp);
            }
            
        }
    }
}
