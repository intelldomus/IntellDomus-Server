using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows;
using System.Threading;
using System.IO;

namespace IntellDomus___Server
{
    class ConnecModule
    {
        TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 9999); //Definizione Server SOcket
        TcpClient clientSocket = default(TcpClient); //Definizione client Socket

        public ConnecModule()
        {
            listener.Start(); //Sart Server

            showLog(" Server Avviato on " + System.Net.IPAddress.Loopback + ":9999"); //Log

            clientSocket = listener.AcceptTcpClient();
            showLog(" Client " + clientSocket.Client.AddressFamily.ToString() + " Connesso");

            methodClient mClient = new methodClient();
            mClient.startClient(clientSocket);

            //Avvio server Thread
            /*Thread serverThread = new Thread(new ThreadStart(this.ServerTask));
            serverThread.IsBackground = true;
            serverThread.Start();*/

            //Chiusura
        }

        public void ServerTask()
        {
            clientSocket = listener.AcceptTcpClient();


            showLog(" Client " + clientSocket.Client.AddressFamily.ToString() + " Connesso");

            methodClient mClient = new methodClient();
            mClient.startClient(clientSocket);

            //Chiusura
           /* clientSocket.Close();
            showLog("Connessione con il client chiusa"); //Log
            serverSocket.Stop();
            showLog("Server si è arrestato"); //Log*/
            //Thread.Sleep(5000);
        }

        public class methodClient  {
            TcpClient clientSocket;
            public void startClient(TcpClient inClientSocket)
            {
                this.clientSocket = inClientSocket;
                                
                //Avvio funzione del client con il thread
                Thread ctThread = new Thread(new ThreadStart(this.clientFunction));
                ctThread.IsBackground = true;
                ctThread.Start();
            }

            public void clientFunction()
            {
                while (true)
                {
                    if (SocketConnected(clientSocket))
                    {
                        sendTemperatura();
                    }
                    else
                        Thread.CurrentThread.Abort();

                    Thread.Sleep(5000);
                }
            }

            public void sendVideo(System.Drawing.Bitmap bitmap)
            {
                
            }

            public void sendTemperatura()
            { //Method invio temperatura
                try
                {
                    StreamWriter writer = new StreamWriter(clientSocket.GetStream());
                    StreamReader reader = new StreamReader(clientSocket.GetStream());

                    Random rnd = new Random();


                    String str = rnd.Next(-20, 100).ToString();

                    showLog("Temperatura: " + str + "°C");


                    writer.Write(str); //Invio Valori             
                    reader.ReadLine(); //Ricezioni valori

                    writer.Close();
                    reader.Close();
                }
                catch (ObjectDisposedException e) { }
            }

            public bool SocketConnected(TcpClient s)
            {  //Controllo se la connessione con il client è ancora attiva
                //bool part1 = s.Client.Poll(1000, SelectMode.SelectRead);
                bool part2 = (s.Available == 0);
                if (part2)
                {
                    showLog("Client disconnesso");
                    return false;
                }
                else
                    return true;
            }

            public void showLog(String log)
            { //Scrittura log in LogBox
                variableGlobal.logBox2.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,
                                    new Action(() => variableGlobal.logBox2.AppendText(DateTime.Now.ToString() + " " + log + "\n")));
            }
        }
        public void showLog(String log)
        { //Scrittura log in LogBox
            variableGlobal.logBox2.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,
                                new Action(() => variableGlobal.logBox2.AppendText(DateTime.Now.ToString() + " " + log + "\n")));
        }
    }
}
