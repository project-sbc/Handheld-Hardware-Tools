using Nefarius.Utilities.DeviceManagement.PnP;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.WiFiDirect;

namespace Handheld_Hardware_Tools.Classes.Wifi_AP
{
    public class WiFiDirectHotspotManager
    {
        private WiFiDirectAdvertisementPublisher publisher_;
        private WiFiDirectAdvertisement advertisement_;
        private WiFiDirectLegacySettings legacySettings_;
        private WiFiDirectConnectionListener connectionListener_;
        private readonly List<WiFiDirectDevice> connectedDevices_;

        private string ssid_;
        private string passphrase_;

        public event EventHandler<DeviceEventArgs> DeviceConnected;
        public event EventHandler<DeviceEventArgs> DeviceDisconnected;
        public event EventHandler<string> AdvertisementStarted;
        public event EventHandler<string> AdvertisementStopped;
        public event EventHandler<string> AdvertisementAborted;
        public event EventHandler<string> AsyncException;

        private void RaiseDeviceConnected(WiFiDirectDevice device, string message)
        {
            var handler = DeviceConnected;
            handler?.Invoke(this, new DeviceEventArgs(device, message));
        }

        private void RaiseDeviceDisconnected(WiFiDirectDevice device, string message)
        {
            var handler = DeviceDisconnected;
            handler?.Invoke(this, new DeviceEventArgs(device, message));
        }

        private void RaiseAdvertisementStarted(string message)
        {
            var handler = AdvertisementStarted;
            handler?.Invoke(this, message);
        }

        private void RaiseAdvertisementStopped(string message)
        {
            var handler = AdvertisementStopped;
            handler?.Invoke(this, message);
        }

        private void RaiseAdvertisementAborted(string message)
        {
            var handler = AdvertisementAborted;
            handler?.Invoke(this, message);
        }

        private void RaiseAsyncException(string message)
        {
            var handler = AsyncException;
            handler?.Invoke(this, message);
        }

        public WiFiDirectHotspotManager(string ssid, string passphrase) : this()
        {
            Ssid = ssid;
            Passphrase = passphrase;
        }

        public WiFiDirectHotspotManager()
        {
            SsidProvided = false;
            PassphraseProvided = false;
            connectedDevices_ = new List<WiFiDirectDevice>();
        }

        ~WiFiDirectHotspotManager()
        {
            publisher_?.Stop();
            Reset();
        }

        public List<WiFiDirectDevice> ConnectedDevices => connectedDevices_;
        public string Ssid
        {
            get => ssid_;
            set
            {
                ssid_ = value;
                SsidProvided = true;
            }
        }
        public bool SsidProvided { get; set; }

        public string Passphrase
        {
            get => passphrase_;
            set
            {
                passphrase_ = value;
                PassphraseProvided = true;
            }
        }

        public bool PassphraseProvided { get; set; }


        private void StartConnectionListener()
        {
            connectionListener_ = new WiFiDirectConnectionListener();
            connectionListener_.ConnectionRequested += ConnectionListenerOnConnectionRequested;
        }

        private async void ConnectionListenerOnConnectionRequested(WiFiDirectConnectionListener sender,
            WiFiDirectConnectionRequestedEventArgs args)
        {
            try
            {
                var connectionRequest = args.GetConnectionRequest();

                if (connectionRequest == null)
                {
                    throw new WlanHostedNetworkException(
                         "Call to ConnectionRequestedEventArgs.GetConnectionRequest() return a null result.");
                }

                var deviceInfo = connectionRequest.DeviceInformation;

                var wiFiDirectDevice = await WiFiDirectDevice.FromIdAsync(deviceInfo.Id);

                if (wiFiDirectDevice == null)
                {
                    throw new WlanHostedNetworkException($"Connection to {deviceInfo.Id} failed;");
                }

                if (wiFiDirectDevice.ConnectionStatus == WiFiDirectConnectionStatus.Connected)
                {
                    var endpointPairs = wiFiDirectDevice.GetConnectionEndpointPairs();
                    var connection = endpointPairs.First();
                    var remoteHostName = connection.RemoteHostName;
                    var remoteHostNameDisplay = remoteHostName.DisplayName;
                    RaiseDeviceConnected(wiFiDirectDevice, $"{remoteHostNameDisplay} connected.");
                    connectedDevices_.Add(wiFiDirectDevice);
                }
                wiFiDirectDevice.ConnectionStatusChanged += WfdDeviceOnConnectionStatusChanged;
            }
            catch (Exception ex)
            {
                throw new WlanHostedNetworkException(ex.Message, ex);
            }
        }


        private void WfdDeviceOnConnectionStatusChanged(WiFiDirectDevice wiFiDirectDevice, object args)
        {
            var endpointPairs = wiFiDirectDevice.GetConnectionEndpointPairs();
            var connection = endpointPairs.First();
            var remoteHostName = connection.RemoteHostName;
            var remoteHostNameDisplay = remoteHostName.DisplayName;
            var status = wiFiDirectDevice.ConnectionStatus;
            if (status == WiFiDirectConnectionStatus.Connected)
            {
                RaiseDeviceConnected(wiFiDirectDevice, $"{remoteHostNameDisplay} connected");
            }
            else
            {
                if (connectedDevices_.Contains(wiFiDirectDevice))
                {
                    connectedDevices_.Remove(wiFiDirectDevice);
                }
                RaiseDeviceDisconnected(wiFiDirectDevice, $"{remoteHostNameDisplay} disconnected.");
            }
        }


        public void Start()
        {
            Reset();

            publisher_ = new WiFiDirectAdvertisementPublisher();
            publisher_.StatusChanged += PublisherOnStatusChanged;

            advertisement_ = publisher_.Advertisement;
            advertisement_.IsAutonomousGroupOwnerEnabled = true;

            legacySettings_ = advertisement_.LegacySettings;
            legacySettings_.IsEnabled = true;

            if (SsidProvided)
            {
                legacySettings_.Ssid = Ssid;
            }
            else
            {
                ssid_ = legacySettings_.Ssid;
            }

            var passwordCredentials = legacySettings_.Passphrase;
            if (PassphraseProvided)
            {
                passwordCredentials.Password = Passphrase;
            }
            else
            {
                passphrase_ = passwordCredentials.Password;
            }
           
            publisher_.Start();
        }

        public Windows.Devices.WiFiDirect.WiFiDirectAdvertisementPublisherStatus Status()
        {
            return publisher_.Status;
        }

        public void Stop()
        {
            publisher_?.Stop();
        }

        private void PublisherOnStatusChanged(WiFiDirectAdvertisementPublisher sender, WiFiDirectAdvertisementPublisherStatusChangedEventArgs args)
        {
            try
            {
                var status = args.Status;

                switch (status)
                {
                    case WiFiDirectAdvertisementPublisherStatus.Started:
                        {
                            StartConnectionListener();
                            RaiseAdvertisementStarted("Advertisement started.");
                            break;
                        }
                    case WiFiDirectAdvertisementPublisherStatus.Aborted:
                        {
                            var error = args.Error;

                            string message;

                            switch (error)
                            {
                                case WiFiDirectError.RadioNotAvailable:
                                    message = "Advertisement aborted, Wi-Fi radio is turned off";
                                    break;

                                case WiFiDirectError.ResourceInUse:
                                    message = "Advertisement aborted, Resource In Use";
                                    break;

                                default:
                                    message = "Advertisement aborted, unknown reason";
                                    break;
                            }

                            RaiseAdvertisementAborted(message);
                            break;
                        }
                    case WiFiDirectAdvertisementPublisherStatus.Stopped:
                        {
                            RaiseAdvertisementStopped("Advertisement stopped");
                            break;
                        }
                }
            }
            catch (WlanHostedNetworkException ex)
            {
                RaiseAsyncException(ex.Message);
            }
        }


        private void Reset()
        {
            if (connectionListener_ != null)
            {
                connectionListener_.ConnectionRequested -= ConnectionListenerOnConnectionRequested;
            }

            if (publisher_ != null)
            {
                publisher_.StatusChanged -= PublisherOnStatusChanged;
            }

            legacySettings_ = null;
            advertisement_ = null;
            publisher_ = null;

            if (connectionListener_ != null)
            {
                connectionListener_.ConnectionRequested += ConnectionListenerOnConnectionRequested;
                connectionListener_ = null;
            }

            connectedDevices_.Clear();
        }
    }

    public class WlanHostedNetworkException : Exception
    {

        public WlanHostedNetworkException(string message)
            : base(message)
        {

        }

        public WlanHostedNetworkException(string message, Exception ex)
            : base(message, ex)
        {

        }
    }
    public class DeviceEventArgs : EventArgs
    {
        public DeviceEventArgs(WiFiDirectDevice device, string message)
        {
            Device = device;
            Message = message;
        }
        public WiFiDirectDevice Device { get; }

        public string Message { get; }
    }
}

