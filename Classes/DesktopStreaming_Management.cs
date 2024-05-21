using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Handheld_Hardware_Tools.Classes
{
    internal class DesktopStreaming_Management
    {
        private static DesktopStreaming_Management _instance = null;
        private static readonly object lockObj = new object();
        private DesktopStreaming_Management()
        {
        }
        public static DesktopStreaming_Management Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new DesktopStreaming_Management();
                        }
                    }
                }
                return _instance;
            }
        }


        private Socket serverSocket;
        private Socket[] clientSockets;

        public void StartStreamingServer(int port = 4172)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Udp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
           
        }

    }
  
    

  

}
