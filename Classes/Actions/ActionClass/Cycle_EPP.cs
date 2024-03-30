using Everything_Handhelds_Tool.Classes.Profiles.ProfileActions.ProfileActionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.ApplicationModel.Store;

namespace Everything_Handhelds_Tool.Classes.Actions.ActionClass
{
    public class Cycle_EPP : Action
    {
        public Cycle_EPP()
        {
            actionName = "Cycle_EPP";
            parameters = new List<string>();
        }
        public override void OnActivate(string quickActionWheelParameter="")
        {
            if (parameters.Count > 0)
            {
                int intEPP = Powercfg_Management.Instance.ReadAndReturnEPP();
                
                if (quickActionWheelParameter == "")
                {
                    string strEPP = intEPP.ToString();

                    int index = parameters.IndexOf(strEPP);
                    //if the index is -1 that means it doesnt exist, start at the first tdp in the list
                    //if the index+1 = count that means we are at the end of the list and we need to go back to 0
                    if (index == -1 || index + 1 == parameters.Count)
                    {
                        index = 0;
                    }
                    else
                    {
                        index = index + 1;
                    }

                    int.TryParse(parameters[index], out intEPP);

                    Powercfg_Management.Instance.ChangeEPP(intEPP);

                    
                }
                else
                {//this is for quick action wheel only
                    quickActionWheelParameter = quickActionWheelParameter.Replace(" %", "");

                    if (int.TryParse(quickActionWheelParameter, out intEPP))
                    {
                        Powercfg_Management.Instance.ChangeEPP(intEPP);
                    }
                }

                if (displayNotification)
                {
                    //displaynotification
                    DisplayNotification(Application.Current.Resources["Action_" + actionName].ToString(), (intEPP).ToString() + " %");
                }
                

            }
        }
    }
}
