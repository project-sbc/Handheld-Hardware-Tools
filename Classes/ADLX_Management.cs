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


        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool HasIntegerScalingSupport();
        //0 is disabled, 1 is enabled
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern int SetIntegerScaling(int key);
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool IsIntegerScalingEnabled();



        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool HasGPUScalingSupport();
        //0 is disabled, 1 is enabled
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern int SetGPUScaling(int key);
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool IsGPUScalingEnabled();


        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool HasScalingModeSupport();

        //Scaling Mode int to mode: 0 is preserve aspect ration, 1 is full panel, 2 is center
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern int SetScalingMode(int key);
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern int GetScalingMode();


        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool HasFreeSyncSupport();      
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern bool IsFreeSyncEnabled();
        //freesync: 0 is off, 1 is enabled
        [DllImport(CppFunctionsDLL4, CallingConvention = CallingConvention.Cdecl)] public static extern int SetFreeSync(int key);
        #endregion

    }
}
