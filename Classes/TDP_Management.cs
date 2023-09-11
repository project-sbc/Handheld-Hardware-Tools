using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everything_Handhelds_Tool.Classes
{
    public class TDP_Management
    {
        private static TDP_Management _instance = null;
     

        private static readonly object lockObj = new object();

        private string appDir = AppDomain.CurrentDomain.BaseDirectory;

        private TDP_Management()
        {

        }

        public static TDP_Management Instance
        {
            get
            {
                if (_instance == null )
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new TDP_Management();
                        }
                    }
                }

              

                return _instance;
            }
        }

       
    }
}
