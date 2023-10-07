﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everything_Handhelds_Tool.Classes.Actions
{
    public class Change_TDP: Action
    {
        public Change_TDP() 
        {
            actionName = "Change_TDP";
            arguments = new List<string>();
        }
        public override void OnActivate()
        {
            if (arguments.Count > 0)
            {
                int changeTDP = 0;
                int currentTDP = TDP_Management.Instance.ReadAndReturnSustainedTDP();
                if (arguments[0].ToString().Contains("-"))
                {
                    Int32.TryParse(arguments[0].Substring(1, arguments[0].Length - 1), out changeTDP);
                    changeTDP = -changeTDP;
                }
                else
                {
                    Int32.TryParse(arguments[0], out changeTDP);
                }

                TDP_Management.Instance.ChangeSustainedTDP(currentTDP + changeTDP);
            }
        }
    }
}