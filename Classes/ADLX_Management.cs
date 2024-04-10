using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Everything_Handhelds_Tool.Classes
{
    public class ADLX_Management
    {

        private static ADLX_Management _instance = null;
        private static readonly object lockObj = new object();
        private ADLX_Management()
        {
        }
        public static ADLX_Management Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new ADLX_Management();
                        }
                    }
                }
                return _instance;
            }
        }


        #region constants and dll references
        public const string CppFunctionsDLL = @"Resources\AMD\ADLX\ADLX_PerformanceMetrics.dll";
        public const string CppFunctionsDLL2 = @"Resources\AMD\ADLX\ADLX_AutoTuning.dll";
        public const string CppFunctionsDLL3 = @"Resources\AMD\ADLX\ADLX_3DSettings.dll";
        public const string CppFunctionsDLL4 = @"Resources\AMD\ADLX\ADLX_DisplaySettings.dll";

        [DllImport(CppFunctionsDLL, CallingConvention = CallingConvention.Cdecl)] public static extern int GetFPSData();

        [DllImport(CppFunctionsDLL, CallingConvention = CallingConvention.Cdecl)] public static extern int GetGPUMetrics(int GPU, int Sensor);

        [DllImport(CppFunctionsDLL2, CallingConvention = CallingConvention.Cdecl)] public static extern int SetAutoTuning(int GPU, int num);

        [DllImport(CppFunctionsDLL2, CallingConvention = CallingConvention.Cdecl)] public static extern int GetAutoTuning(int GPU);

        [DllImport(CppFunctionsDLL2, CallingConvention = CallingConvention.Cdecl)] public static extern int GetFactoryStatus(int GPU);

        [DllImport(CppFunctionsDLL3, CallingConvention = CallingConvention.Cdecl)] public static extern int SetFPSLimit(int GPU, bool isEnabled, int FPS);
        [DllImport(CppFunctionsDLL3, CallingConvention = CallingConvention.Cdecl)] public static extern int SetRSR(bool isEnabled);
        [DllImport(CppFunctionsDLL3, CallingConvention = CallingConvention.Cdecl)] public static extern int GetRSRState();

        [DllImport(CppFunctionsDLL3, CallingConvention = CallingConvention.Cdecl)] public static extern bool SetRSRSharpness(int sharpness);
        [DllImport(CppFunctionsDLL3, CallingConvention = CallingConvention.Cdecl)] public static extern int GetRSRSharpness();


        //if function ends 1, its for the first display! if ends in 2, second display

        

        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool HasIntegerScalingSupport1();
        //0 is disabled, 1 is enabled
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern int SetIntegerScaling1(int key);
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool IsIntegerScalingEnabled1();


        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool HasIntegerScalingSupport2();
        //0 is disabled, 1 is enabled
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern int SetIntegerScaling2(int key);
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool IsIntegerScalingEnabled2();

        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool HasGPUScalingSupport1();
        //0 is disabled, 1 is enabled
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern int SetGPUScaling1(int key);
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool IsGPUScalingEnabled1();

        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool HasGPUScalingSupport2();
        //0 is disabled, 1 is enabled
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern int SetGPUScaling2(int key);
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool IsGPUScalingEnabled2();
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool HasScalingModeSupport2();

        //Scaling Mode int to mode: 0 is preserve aspect ration, 1 is full panel, 2 is center
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern int SetScalingMode1(int key);
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern int GetScalingMode1();
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern int SetScalingMode2(int key);
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern int GetScalingMode2();

        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool HasFreeSyncSupport1();      
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool IsFreeSyncEnabled1();
        //freesync: 0 is off, 1 is enabled
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern int SetFreeSync1(int key);

        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool HasFreeSyncSupport2();
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool IsFreeSyncEnabled2();
        //freesync: 0 is off, 1 is enabled
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern int SetFreeSync2(int key);
        #endregion

    }
}
