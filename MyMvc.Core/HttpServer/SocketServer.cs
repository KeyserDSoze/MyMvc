using MyMvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MyMvc.Core
{
    public class AsynchronousSocketListener : IHttpListener
    {
        // Thread signal.  
        private static readonly ManualResetEvent AllDone = new ManualResetEvent(false);
        private readonly IDIMvc DIMvc;
        public AsynchronousSocketListener(IDIMvc dIMvc)
            => this.DIMvc = dIMvc;
        public void StartListening()
        {
            // Establish the local endpoint for the socket.  
            // The DNS name of the computer  
            // running the listener is "host.contoso.com".  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[2];
            int port = 11000;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // Bind the socket to the local endpoint and listen for incoming connections.  
            listener.Bind(localEndPoint);
            listener.Listen(100);
            Console.WriteLine($"Listening on {ipAddress}:{port}");
            while (true)
            {
                // Set the event to nonsignaled state.  
                AllDone.Reset();
                // Start an asynchronous socket to listen for connections.  
                listener.BeginAccept(
                    new AsyncCallback(AcceptCallback),
                    listener);
                // Wait until a connection is made before continuing.  
                AllDone.WaitOne();
            }
        }
        private void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            AllDone.Set();
            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            StateObject state = new StateObject
            {
                workSocket = handler
            };
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }
        private void ReadCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.   
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                state.Data.AddRange(state.Buffer);
                // Check for end-of-file tag. If it is not there, read   
                // more data.  
                if (state.Buffer[^1] == StateObject.Zero)
                {
                    // All the data has been read from the   
                    // Echo the data back to the client.  
                    state.Data.RemoveAll(x => x == StateObject.Zero);
                    this.Send(handler, state.Data.ToArray());
                }
                else
                {
                    // Not all data received. Get more.  
                    handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
        }
        private void Send(Socket handler, byte[] byteData)
        {
            // Begin sending the data to the remote device.
            // Do something with bytesData
            HttpContext httpContext = new HttpContext(new HttpRequest(byteData), new HttpResponse());
            this.DIMvc.ApplicationStarter.Next(httpContext);
            byte[] responseAsByte = httpContext.Response.Fetch();
            handler.BeginSend(responseAsByte, 0, responseAsByte.Length, 0, new AsyncCallback(SendCallback), handler);
        }
#pragma warning disable CA1031
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;
                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        ///  State object for reading client data asynchronously
        /// </summary>
        private class StateObject
        {
            // Client  socket.  
            public Socket workSocket = null;
            // Size of receive buffer.  
            public const int BufferSize = 1024;
            // Receive buffer.  
            public byte[] Buffer = new byte[BufferSize];
            public List<byte> Data = new List<byte>();
            public const byte Zero = (byte)'\0';
        }
    }
}