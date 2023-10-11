using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everything_Handhelds_Tool.Classes.Actions.ActionClass
{
    public class Cycle_TDP : Action
    {
        public Cycle_TDP()
        {
            actionName = "Cycle_TDP";
            arguments = new List<string>();
        }
        public override void OnActivate()
        {
            if (arguments.Count > 0)
            {
                int intTDP = TDP_Management.Instance.ReadAndReturnSustainedTDP();
                string strTDP = intTDP.ToString();

                int index = arguments.IndexOf(strTDP);
                //if the index is -1 that means it doesnt exist, start at the first tdp in the list
                //if the index+1 = count that means we are at the end of the list and we need to go back to 0
                if (index == -1 || index + 1 == arguments.Count)
                {
                    index = 0;
                }
                else
                {
                    index = index + 1;
                }

                int.TryParse(arguments[index], out intTDP);

                TDP_Management.Instance.ChangeSustainedBoostTDP(intTDP, intTDP);
            }
        }
    }
}
