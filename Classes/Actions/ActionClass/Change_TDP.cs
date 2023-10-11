using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Everything_Handhelds_Tool.Classes.Actions.ActionClass
{
    public class Change_TDP : Action
    {
        public Change_TDP(List<string> newArguments)
        {
            actionName = "Change_TDP";
            arguments = newArguments;
        }
        public override void OnActivate()
        {
            Dispatcher.CurrentDispatcher.Invoke(() => ActionTask());
           
        }

        public void ActionTask()
        {
            if (arguments.Count > 0)
            {
                int changeTDP = 0;
                int currentTDP = TDP_Management.Instance.ReadAndReturnSustainedTDP();
                if (arguments[0].ToString().Contains("-"))
                {
                    int.TryParse(arguments[0].Substring(1, arguments[0].Length - 1), out changeTDP);
                    changeTDP = -changeTDP;
                }
                else
                {
                    int.TryParse(arguments[0], out changeTDP);
                }

                TDP_Management.Instance.ChangeSustainedBoostTDP(currentTDP + changeTDP, currentTDP + changeTDP);
            }
        }
    }
}
