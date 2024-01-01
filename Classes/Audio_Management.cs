using Everything_Handhelds_Tool.Classes.Devices;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Everything_Handhelds_Tool.Classes
{
    public class Audio_Management
    {
        private static Audio_Management _instance = null;
        private static readonly object lockObj = new object();
        private Audio_Management()
        {
        }
        public static Audio_Management Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Audio_Management();
                        }
                    }
                }
                return _instance;
            }
        }

        public int volume = -1;
        public bool volumeMuted = true;
        private AudioSwitcher.AudioApi.CoreAudio.CoreAudioController audioController;

        public int GetAudioVolume()
        {
            UpdateAudioController();

            var device = audioController.DefaultPlaybackDevice;

            return (int)Math.Round(device.Volume, 0);
        }
        private void UpdateAudioController()
        {
            Task<AudioSwitcher.AudioApi.CoreAudio.CoreAudioController> task1 = Task<AudioSwitcher.AudioApi.CoreAudio.CoreAudioController>.Factory.StartNew(() => new AudioSwitcher.AudioApi.CoreAudio.CoreAudioController());

            audioController = task1.Result;
        }

        public string CurrentPlaybackDevice()
        {
            UpdateAudioController();

            string currentDevice = audioController.DefaultPlaybackDevice.FullName;

      
            return currentDevice;
        }
        public string CurrentInputDevice()
        {
            UpdateAudioController();

            string currentDevice = audioController.DefaultCaptureDevice.FullName;

            return currentDevice;
        }
        public List<string> AvailablePlaybackDevices()
        {
            List<string> availableDevices = new List<string>();

            UpdateAudioController();

            foreach (var device in audioController.GetPlaybackDevices(AudioSwitcher.AudioApi.DeviceState.Active))
            {
                availableDevices.Add(device.FullName);
            }

            return availableDevices;
        }

        public List<string> AvailableInputDevices()
        {
            List<string> availableDevices = new List<string>();

            UpdateAudioController();

            foreach (var device in audioController.GetCaptureDevices(AudioSwitcher.AudioApi.DeviceState.Active))
            {
                availableDevices.Add(device.FullName);
            }

            return availableDevices;
        }

        
        public void SetInputDevice(string deviceName)
        {
            UpdateAudioController();

            foreach (var device in audioController.GetCaptureDevices(AudioSwitcher.AudioApi.DeviceState.Active))
            {
                if (device.FullName == deviceName)
                {
                    audioController.DefaultCaptureDevice = device;
                    break;
                }
            }

        }

        public bool GetMicrophoneMute()
        {
            bool mute = false;

            UpdateAudioController();

            var device = audioController.DefaultCaptureDevice;

            mute = device.IsMuted;

            return mute;
        }

        public void SetPlaybackDevice(string deviceName)
        {

            UpdateAudioController();

            foreach (var device in audioController.GetPlaybackDevices(AudioSwitcher.AudioApi.DeviceState.Active))
            {
                if (device.FullName == deviceName)
                {
                    audioController.DefaultPlaybackDevice = device;
                    break;
                }
            }

        }

    }

   
}
