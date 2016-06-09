using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using InTheHand;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Ports;
using InTheHand.Net.Sockets;
using System.IO;



namespace Bluetooth_tutorial
{
    public partial class Form1 : Form
    {
        #region Variables
        List<string> items; //found devices
        Stream mStream;
        bool ready; //if there is something in message to send
        Guid mUUID;
        bool serverStarted;
        string myPin;
        BluetoothDeviceInfo[] devices;
        byte[] received;
        int i;
        BluetoothDeviceInfo deviceInfo;
        #endregion

        #region Constructor
        public Form1()
        {
            InitializeComponent();
            items = new List<string>();
            mUUID = new Guid("00001101-0000-1000-8000-00805F9B34FB");
            ready = false;
            serverStarted = false;
            myPin = "1995";
            received = new byte[1024];
            i = 0;
            for (int i = 0; i < received.Length; i++)
            {
                received[i] = 0;
            }
        }
        #endregion

        #region Send and read message
        void send1(ref Stream mStream, byte[] mes)
        {
            try
            {
                if (ready == true && (mes != null))
                {
                    mStream.Write(mes, 0, mes.Length);
                    updateUI(System.Environment.NewLine + "Send: " + Encoding.ASCII.GetString(mes));
                    ready = false;
                    mes = null;
                }
            }
            catch (IOException exception)
            {

                updateUI("Client has disconnected (send method)");
            }
        }
        void read1(ref Stream mStream)
        {
            try
            {
                if (ready == false /*&& (message == null)*/)
                {
                    //handle server connection
                    int x = mStream.ReadByte();
                    received[i] = Convert.ToByte(x);
                    i++;
                    if(x==33)
                    {
                        updateUI(System.Environment.NewLine + "Received: "+ Encoding.ASCII.GetString(received));
                        for (int j = 0; j <= received.Length; j++)
                        {
                            received[j] = 0;
                            if(j==i)
                            {
                                i = 0;
                                break;
                            }
                        }
                    }
                }
            }
            catch (IOException exception)
            {
                updateUI("Client has disconnected! (read method)");
                return;
            }
        }
        #endregion

        #region Server connection
        void connectAsServer()
        {
            Thread bluetoothServerThread = new Thread(new ThreadStart(ServerConnectThread));
            bluetoothServerThread.Start();
        }

        public void ServerConnectThread()
        {
            serverStarted = true;
            updateUI("Server started waiting for a client");
            BluetoothListener blueListener = new BluetoothListener(mUUID);
            blueListener.Start();
            BluetoothClient conn = blueListener.AcceptBluetoothClient();
            updateUI("Client has connected");

            mStream = conn.GetStream();

            while (true)
            {
                read1(ref mStream);
            }
        }
        #endregion

        #region Update output
        private void updateUI(string message)
        {
            Func<int> del = delegate ()
            {
                tbOutput.AppendText(message + System.Environment.NewLine);
                return 0;
            };
            Invoke(del);
        }

        private void updateDeviceList()
        {
            Func<int> del = delegate ()
            {
                listBox1.DataSource = items;
                return 0;
            };
            Invoke(del);
        }

        #endregion

        private void bGo_Click(object sender, EventArgs e)
        {
            if(serverStarted)
            {
                updateUI("Server already started you silly sausage!");
                return;
            }
            if(rbClient.Checked)
            {
                startScan();
            }
            else
            {
                connectAsServer();
            }
        }

        private void startScan()
        {
            listBox1.DataSource = null;
            listBox1.Items.Clear();
            items.Clear();
            Thread bluetoothScanThread = new Thread(new ThreadStart(scan));
            bluetoothScanThread.Start();
        }

        private void scan()
        {
            updateUI("Starting scan for devices...");
            BluetoothClient client = new BluetoothClient();
            devices = client.DiscoverDevicesInRange();
            updateUI("Scan complete.");
            updateUI(devices.Length.ToString() + " devices discovered.");
            foreach (BluetoothDeviceInfo item in devices)
            {
                items.Add(item.DeviceName);
            }

            updateDeviceList();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            deviceInfo = devices.ElementAt(listBox1.SelectedIndex);
            updateUI(deviceInfo.DeviceName + " was selected, attempting connect");

            if(pairDevice())
            {
                updateUI("Device was paired.");
                updateUI("Starting thread connection...");
                Thread bluetoothClientThread = new Thread(new ThreadStart(ClientConnectThread));
                bluetoothClientThread.Start();
            }
            else
            {
                updateUI("Pair failed");
            }
        }

        #region Client connection
        private void ClientConnectThread()
        {
            BluetoothClient client = new BluetoothClient();
            
            updateUI("Attempting to connect...");
            client.BeginConnect(deviceInfo.DeviceAddress, mUUID, this.BluetoothClientConnectCallback, client);
        }

        void BluetoothClientConnectCallback(IAsyncResult result)
        {
            BluetoothClient client = (BluetoothClient)result.AsyncState;
            client.EndConnect(result);
            updateUI("Connected");
            mStream = client.GetStream();

            while (true)
            {
                read1(ref mStream);
            }
        }
        #endregion

        private bool pairDevice()
        {
            if (!deviceInfo.Authenticated)
            {
                if (!BluetoothSecurity.PairRequest(deviceInfo.DeviceAddress, myPin))
                {
                    return false;
                }
            }
            return true;
        }

        #region Cannot be accessible from GUI
        private void tbText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13) //enter was pressed
            {
                if (mStream!=null)
                {
                    byte[] message;
                    message = Encoding.ASCII.GetBytes(tbText.Text);

                    ready = true;
                    send1(ref mStream,message);
                }
                else
                {
                    MessageBox.Show("Device is not connected. Cannot send a message");
                }
                tbText.Clear();
            }
        }
        #endregion

        #region I wonder what this buttons do?
        private void button1_Click(object sender, EventArgs e)
        {
            if (mStream != null)
            {
                byte[] collapse_message = new byte[9];
                collapse_message[0] = 2;
                collapse_message[1] = 122;
                collapse_message[2] = 119;
                collapse_message[3] = 105;
                collapse_message[4] = 110;
                collapse_message[5] = 30;
                collapse_message[6] = 228;
                collapse_message[7] = 228;
                collapse_message[8] = 3;

                ready = true;
                send1(ref mStream, collapse_message);
            }
            else
            {
                MessageBox.Show("Device is not connected. Cannot send a message!", "Error Detected in \"Collapse button\"");
            }
        }

        private void bt_ClearConsole_Click(object sender, EventArgs e)
        {
            tbOutput.Clear();
        }

        private void bt_Expand_Click(object sender, EventArgs e)
        {
            if (mStream != null)
            {
                byte[] expand_message = new byte[12];
                expand_message[0] = 2;
                expand_message[1] = 114;
                expand_message[2] = 111;
                expand_message[3] = 122;
                expand_message[4] = 119;
                expand_message[5] = 105;
                expand_message[6] = 110;
                expand_message[7] = 30;
                expand_message[8] = 227;
                expand_message[9] = 227;
                expand_message[10] = 227;
                expand_message[11] = 3;

                ready = true;
                send1(ref mStream, expand_message);
            }
            else
            {
                MessageBox.Show("Device is not connected. Cannot send a message!", "Error Detected in \"Expand button\"");
            }
        }

        private void bt_Stop_Click(object sender, EventArgs e)
        {
            if (mStream != null)
            {
                byte[] stop_message = new byte[9];
                stop_message[0] = 2;
                stop_message[1] = 115;
                stop_message[2] = 116;
                stop_message[3] = 111;
                stop_message[4] = 112;
                stop_message[5] = 30;
                stop_message[6] = 227;
                stop_message[7] = 227;
                stop_message[8] = 3;

                ready = true;
                send1(ref mStream, stop_message);
            }
            else
            {
                MessageBox.Show("Device is not connected. Cannot send a message!", "Error Detected in \"Stop button\"");
            }
        }

        private void bt_Auto_Click(object sender, EventArgs e)
        {
            if (mStream != null)
            {
                byte[] auto_message = new byte[10];
                auto_message[0] = 2;
                auto_message[1] = 97;
                auto_message[2] = 117;
                auto_message[3] = 116;
                auto_message[4] = 111;
                auto_message[5] = 30;
                auto_message[6] = 147;
                auto_message[7] = 147;
                auto_message[8] = 147;
                auto_message[9] = 3;

                ready = true;
                send1(ref mStream, auto_message);
            }
            else
            {
                MessageBox.Show("Device is not connected. Cannot send a message!", "Error Detected in \"Auto button\"");
            }
        }
        #endregion

        private void rbServer_CheckedChanged(object sender, EventArgs e)
        {
            if (rbServer.Checked == true)
            {
                MessageBox.Show("This function is hidden now", "Server CheckBox");
                rbServer.Checked = false;
                rbClient.Checked = true;
            }
        }

        private void rbClient_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Click button \"Go!\" to scan for devices","Message from \"Client\" CheckBox");
        }

        private void label2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Double click on device's name to connect","Message from \"Devices\" Label");
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Click \"Go!\" button to start scan.\nDouble click on device's name to connect.\nUse buttons below \"Console\" to control your blind.", "How to use \"Control your blinds\" program");
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yes, this is a console.\nProgram print output data here","Console message");
        }
    }
}
