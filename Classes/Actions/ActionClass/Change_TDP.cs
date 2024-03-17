using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Everything_Handhelds_Tool.Classes.Actions.ActionClass
{
    public class Change_TDP : Action
    {
       
        public Change_TDP()
        {
            actionName = "Change_TDP";
            
        }
        public override void OnActivate(string quickActionWheelParameter = "")
        {
            ActionTask();
      
           
        }

        public void ActionTask()
        {
            if (parameters.Count > 0)
            {
                int changeTDP = 0;
                int currentTDP = TDP_Management.Instance.ReadAndReturnSustainedTDP();
                if (parameters[0].ToString().Contains("-"))
                {
                    int.TryParse(parameters[0].Substring(1, parameters[0].Length - 1), out changeTDP);
                    changeTDP = -changeTDP;
                }
                else
                {
                    int.TryParse(parameters[0], out changeTDP);
                }

                TDP_Management.Instance.ChangeSustainedBoostTDP(currentTDP + changeTDP, currentTDP + changeTDP);

                //displaynotification
                DisplayNotification(Application.Current.Resources["Action_" + actionName].ToString(), (currentTDP + changeTDP).ToString() + " W");

            }
        }
    }
}
