using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        string id;
        string username;
        string evnt;
        string[] IPadd;
        string[] licensefile;
        string lic;
        public static double MAX_CH = 0;
        public static double MAX_CHs = 0;
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);

            this.Resize += new EventHandler(TrayMinimizerForm_Resize);
            notifyIcon1.MouseDoubleClick += maxi;

         //   this.WindowState = FormWindowState.Minimized;

            ///Startup Code
            var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var s = Path.Combine(appDataDir, "ishida.ini");
            var l = Path.Combine(appDataDir, "lic.dat");
            try
            {
                IPadd = File.ReadAllLines(s);
            }
            catch
            {
                string[] lines = { "192.168.1.1", "192.168.1.2", "192.168.1.3", "192.168.1.4", "192.168.1.5", "192.168.1.6", "192.168.1.7", "192.168.1.8", "192.168.1.9", "192.168.1.10", "192.168.1.11", "192.168.1.12", "192.168.1.13", "192.168.1.14", "192.168.1.15", "192.168.1.16" };
                System.IO.File.WriteAllLines(s, lines);
                IPadd = File.ReadAllLines(s);

            }

            try
            {
                licensefile = File.ReadAllLines(l);
            }
            catch
            {
                string[] lines = { "Lkey-XXXXXXXXXXXX-XX-XX" };
                System.IO.File.WriteAllLines(l, lines);
                licensefile = File.ReadAllLines(l);
            }
            
            textBox1.Text = IPadd.GetValue(0).ToString();
            textBox2.Text = IPadd.GetValue(1).ToString();
            textBox3.Text = IPadd.GetValue(2).ToString();
            textBox4.Text = IPadd.GetValue(3).ToString();
            textBox5.Text = IPadd.GetValue(4).ToString();
            textBox6.Text = IPadd.GetValue(5).ToString();
            textBox7.Text = IPadd.GetValue(6).ToString();
            textBox8.Text = IPadd.GetValue(7).ToString();
            textBox9.Text = IPadd.GetValue(8).ToString();
            textBox10.Text = IPadd.GetValue(9).ToString();
            textBox11.Text = IPadd.GetValue(10).ToString();
            textBox12.Text = IPadd.GetValue(11).ToString();
            textBox13.Text = IPadd.GetValue(12).ToString();
            textBox14.Text = IPadd.GetValue(13).ToString();
            textBox15.Text = IPadd.GetValue(14).ToString();
            textBox16.Text = IPadd.GetValue(15).ToString();

         
          
                char[] delimiter = { '-' };
                string[] settings = licensefile.GetValue(0).ToString().Split(delimiter);
                string posd = MinifyA(settings[2]);
                string scaled = MinifyA(settings[3]);
                lic = MinifyA(settings[1]);
                int pos = Int32.Parse(posd);
                int scale = Int32.Parse(scaled);
                MAX_CH = (pos / 168);
                MAX_CHs = (scale / 164);


            label1.Text = "Translator ID : " + GetMACAddress().ToLower();
            label7.Text = "Translator Key  :" + licensefile.GetValue(0).ToString();


            string unkey = GetMACAddress().ToLower();
            string unkeyl = lic;

            if (unkey != unkeyl)
            {
                MAX_CH = 0;
                MAX_CHs = 0;
            }


            string channels = MAX_CH.ToString();
            string channelss = MAX_CHs.ToString();

            label8.Text = "Total POS CH   : " + channels;
            label9.Text = "Total Scale CH : " + channelss;




            //PNP
            Thread thdUDPServerPNP = new Thread(new ThreadStart(PNP));
            thdUDPServerPNP.IsBackground = true;
            thdUDPServerPNP.Start();
            //PNPB
        //    Thread thdUDPServerPNPb = new Thread(new ThreadStart(PNPb));
        //  thdUDPServerPNPb.IsBackground = true;
       //     thdUDPServerPNPb.Start();

            //Scales
            //1
           if (MAX_CHs >= 1)
           {
                Thread thdUDPServer1 = new Thread(new ThreadStart(scale1));
               thdUDPServer1.IsBackground = true;
                thdUDPServer1.Start();
           }
            //2
            if (MAX_CHs >= 2)
            {
                Thread thdUDPServer2 = new Thread(new ThreadStart(scale2));
                thdUDPServer2.IsBackground = true;
                thdUDPServer2.Start();
            }
            //3
            if (MAX_CHs >= 3)
            {
                Thread thdUDPServer3 = new Thread(new ThreadStart(scale3));
                thdUDPServer3.IsBackground = true;
                thdUDPServer3.Start();
            }
            //4
            if (MAX_CHs >= 4)
            {
                Thread thdUDPServer4 = new Thread(new ThreadStart(scale4));
                thdUDPServer4.IsBackground = true;
                thdUDPServer4.Start();
            }
            //5
            if (MAX_CHs >= 5)
            {
                Thread thdUDPServer5 = new Thread(new ThreadStart(scale5));
                thdUDPServer5.IsBackground = true;
                thdUDPServer5.Start();
            }
            //6
            if (MAX_CHs >= 6)
            {
                Thread thdUDPServer6 = new Thread(new ThreadStart(scale6));
                thdUDPServer6.IsBackground = true;
                thdUDPServer6.Start();
            }
            //7
            if (MAX_CHs >= 7)
            {
                Thread thdUDPServer7 = new Thread(new ThreadStart(scale7));
                thdUDPServer7.IsBackground = true;
                thdUDPServer7.Start();
            }
            //8
            if (MAX_CHs >= 8)
            {
                Thread thdUDPServer8 = new Thread(new ThreadStart(scale8));
                thdUDPServer8.IsBackground = true;
                thdUDPServer8.Start();
            }
            //9
            if (MAX_CHs >= 9)
            {
                Thread thdUDPServer9 = new Thread(new ThreadStart(scale9));
                thdUDPServer9.IsBackground = true;
                thdUDPServer9.Start();
            }
            //10
            if (MAX_CHs >= 10)
            {
                Thread thdUDPServer10 = new Thread(new ThreadStart(scale10));
                thdUDPServer10.IsBackground = true;
                thdUDPServer10.Start();
            }
            //11
            if (MAX_CHs >= 11)
            {
                Thread thdUDPServer11 = new Thread(new ThreadStart(scale11));
                thdUDPServer11.IsBackground = true;
                thdUDPServer11.Start();
            }
            //12
            if (MAX_CHs >= 12)
            {
                Thread thdUDPServer12 = new Thread(new ThreadStart(scale12));
                thdUDPServer12.IsBackground = true;
                thdUDPServer12.Start();
            }
            //13
            if (MAX_CHs >= 13)
            {
                Thread thdUDPServer13 = new Thread(new ThreadStart(scale13));
                thdUDPServer13.IsBackground = true;
                thdUDPServer13.Start();
            }
            //14
            if (MAX_CHs >= 14)
            {
                Thread thdUDPServer14 = new Thread(new ThreadStart(scale14));
                thdUDPServer14.IsBackground = true;
                thdUDPServer14.Start();
            }
            //15
            if (MAX_CHs >= 15)
            {
                Thread thdUDPServer15 = new Thread(new ThreadStart(scale15));
                thdUDPServer15.IsBackground = true;
                thdUDPServer15.Start();
            }
            //16
            if (MAX_CHs >= 16)
            {
                Thread thdUDPServer16 = new Thread(new ThreadStart(scale16));
                thdUDPServer16.IsBackground = true;
                thdUDPServer16.Start();
            }
            //ishida
            Thread thdUDPServerishida = new Thread(new ThreadStart(isida));
            thdUDPServerishida.IsBackground = true;
            thdUDPServerishida.Start();


        }


        public void PNP()
        {


            UdpClient udpClient = new UdpClient(4050);
            udpClient.JoinMulticastGroup(IPAddress.Parse("230.0.0.1"));
            XmlDocument xml = new XmlDocument();
            string Ipaddress = "127.0.0.1";
          //  IPAddress ip = IPAddress.Parse("224.5.6.7");

            while (true)
            {
                try
                {
                    string returnData = " ";


                    IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);

                    returnData = Encoding.ASCII.GetString(receiveBytes, 0, receiveBytes.Length);
                    //  returnData = Encoding.ASCII.GetString(receiveBytes);


                    // richTextBox1.Invoke(new Action(() => richTextBox1.Text = RemoteIpEndPoint.Address.ToString() + returnData + "\n"));
                    if (returnData.Contains("Lkey-"))
                    {
                        richTextBox1.Invoke(new Action(() => richTextBox1.Text = returnData));
                         var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                          var l = Path.Combine(appDataDir, "lic.dat");
                          string[] lines = { returnData };
                         System.IO.File.WriteAllLines(l, lines);
                       
                    }



                    xml.LoadXml(returnData); // suppose that myXmlString contains "<Names>...</Names>"
                  

                    //LOGON.1
                    if (returnData.Contains("type=\"LOGON\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value;
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];


                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode +" Seq# " +sequenceNumber + evnt;
                            id = " Till# " + till + username + " UserID " + operatorCode + evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }





                        if (MAX_CH >= port)
                        {
                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }
                    }

                    //LOGON INVALID.2
                    if (returnData.Contains("type=\"LOGON INVALID\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value;
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];


                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }





                        if (MAX_CH >= port)
                        {
                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }

                    }



                    //SESSION OVERRIDE.3
                    if (returnData.Contains("type=\"SESSION OVERRIDE\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("authorisorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " AuthorisorUser " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value;
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode authorisorCodei = xn["authorisorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];


                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            // string authorisorCode = authorisorCodei.InnerText;
                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username +  " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }


                    }

                    //LOGOFF.4
                    if (returnData.Contains("type=\"LOGOFF\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value;
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];


                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            id = " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }

                    }


                    //OPEN CASH DRAWER.5
                    if (returnData.Contains("type=\"OPEN CASH DRAWER\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string cashdstatus = eventi.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + cashdstatus;
                            id = evnt + " CashDrawerstatus " + cashdstatus;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }


                    }


                    //REMOVE CASH DRAWER.6
                    if (returnData.Contains("type=\"REMOVE CASH DRAWER\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string cashdstatus = eventi.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + cashdstatus;
                            id = evnt + " CashDrawerstatus " + cashdstatus;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }





                    }

                    //SECURE.7
                    if (returnData.Contains("type=\"SECURE\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }





                    }

                    //AUTHORISATION SUCCESSFUL.8
                    if (returnData.Contains("type=\"AUTHORISATION SUCCESSFUL\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string accessPointToken = " " + eventi.ChildNodes[0].InnerXml + " ";

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + accessPointToken ;
                            id = evnt + " " + accessPointToken;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }



                    }


                    //SECURE UNLOCK.9
                    if (returnData.Contains("type=\"SECURE UNLOCK\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }


                    }

                    //AUTHORISATION FAILED.10
                    if (returnData.Contains("type=\"AUTHORISATION FAILED\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string accessPointToken = " " + eventi.ChildNodes[0].InnerXml + " ";

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + accessPointToken;
                            id = evnt + " " + accessPointToken;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }




                    }

                    //ITEM SALE.11
                    if (returnData.Contains("type=\"ITEM SALE\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";


                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];

                            XmlNode eventi = xn["event"];
                            // XmlNode descriptioni = xn["description"];
                            // XmlNode quantityi = xn["quantity"];
                            // XmlNode plui = xn["plu"];
                            // XmlNode skui = xn["sku"];
                            // XmlNode unitPricei = xn["unitPrice"];



                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;




                            string amount = eventi.ChildNodes[0].InnerXml;
                            string description = eventi.ChildNodes[2].InnerXml;
                            string quantity = eventi.ChildNodes[3].InnerXml;
                            string plu = eventi.ChildNodes[4].InnerXml;
                            string unitPrice = eventi.ChildNodes[6].InnerXml;
                            string sku = eventi.ChildNodes[5].InnerXml;





                            //output
                            //   id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt
                            //       + " Description " + description + " SKU " + sku + " PLU " + plu + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount;
                            id = description + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount;
                            port = Int32.Parse(tilli.InnerText);

                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }






                    }


                    //TENDER.12
                    if (returnData.Contains("type=\"TENDER\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string amount = eventi.ChildNodes[0].InnerXml;
                            string tendername = eventi.ChildNodes[1].InnerXml;

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount + " Tendername " + tendername;
                            id = " Amount R" + amount + " Tendername " + tendername;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }




                    }

                    //TRANSACTION TOTAL.13
                    if (returnData.Contains("type=\"TRANSACTION TOTAL\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string amount = eventi.ChildNodes[0].InnerXml;
                            ;

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //  id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount;
                            id = " Total R" + amount;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }




                    }

                    //CHANGE.14
                    if (returnData.Contains("type=\"CHANGE\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string amount = eventi.ChildNodes[0].InnerXml;
                            string tenderName = eventi.ChildNodes[1].InnerXml;
                            ;

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount + " Tendername: " + tenderName;
                            id = " Change R" + amount;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }





                    }


                    //TRANSACTION COMPLETE.15
                    if (returnData.Contains("type=\"TRANSACTION COMPLETE\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string amount = eventi.ChildNodes[0].InnerXml;


                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //  id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount;
                            id = " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + evnt + " Amount R" + amount;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }

                    }


                    //ITEM UNKNOWN.16
                    if (returnData.Contains("type=\"ITEM UNKNOWN\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];

                            XmlNode eventi = xn["event"];



                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;

                            string amount = eventi.ChildNodes[0].InnerXml;
                            string description = eventi.ChildNodes[2].InnerXml;
                            string quantity = eventi.ChildNodes[3].InnerXml;
                            string plu = eventi.ChildNodes[4].InnerXml;
                            string unitPrice = eventi.ChildNodes[5].InnerXml;




                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt
                            //   + " Description " + description  + " PLU " + plu + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount;
                            id = description + " PLU " + plu + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }







                    }

                    //ACCOUNT PAYMENT.17
                    if (returnData.Contains("type=\"ACCOUNT PAYMENT\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string amount = eventi.ChildNodes[0].InnerXml;


                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //  id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount;
                            id = " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + evnt + " Amount R" + amount;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }


                    }

                    //DEPARTMENT SALE.18
                    if (returnData.Contains("event type=\"DEPARTMENT SALE\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";


                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];

                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;

                            string amount = eventi.ChildNodes[0].InnerXml;
                            string description = eventi.ChildNodes[2].InnerXml;
                            string quantity = eventi.ChildNodes[3].InnerXml;
                            string unitPrice = eventi.ChildNodes[4].InnerXml;

                            //output
                            //    id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt
                            //      + " Description " + description  + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount;
                            id = " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + evnt
                         + description + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount;
                            port = Int32.Parse(tilli.InnerText);

                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }







                    }
                    //TRANSACTION RECALL.19
                    if (returnData.Contains("type=\"TRANSACTION RECALL\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string amount = eventi.ChildNodes[0].InnerXml;


                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount;
                            id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + evnt + " Amount R" + amount;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }




                    }
                    //ITEM REFUND.20
                    if (returnData.Contains("type=\"ITEM REFUND\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";


                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];

                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;

                            string amount = eventi.ChildNodes[0].InnerXml;
                            string description = eventi.ChildNodes[2].InnerXml;
                            string quantity = eventi.ChildNodes[3].InnerXml;
                            string sku = eventi.ChildNodes[4].InnerXml;
                            string unitPrice = eventi.ChildNodes[5].InnerXml;

                            //output
                            //    id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt
                            //    + " Description " + description + " Sku " + sku + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount;
                            id = evnt + description + " Sku " + sku + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount;

                            port = Int32.Parse(tilli.InnerText);

                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }







                    }

                    //ITEM VOID.21
                    if (returnData.Contains("type=\"ITEM VOID\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";


                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];

                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;

                            string amount = eventi.ChildNodes[0].InnerXml;
                            string description = eventi.ChildNodes[2].InnerXml;
                            string quantity = eventi.ChildNodes[3].InnerXml;
                            string plu = eventi.ChildNodes[4].InnerXml;
                            string sku = eventi.ChildNodes[6].InnerXml;
                            string reason = eventi.ChildNodes[5].InnerXml;
                            string unitPrice = eventi.ChildNodes[7].InnerXml;

                            //output
                            //   id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt
                            //         + " Description " + description + " Sku " + sku + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount + " Reason " + reason;
                            id = evnt + description + " Sku " + sku + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount + " Reason " + reason;

                            port = Int32.Parse(tilli.InnerText);

                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }







                    }
                    //ITEM PRICE OVERRIDE.22
                    if (returnData.Contains("type=\"ITEM PRICE OVERRIDE\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";


                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];

                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;

                            string amount = eventi.ChildNodes[0].InnerXml;
                            string description = eventi.ChildNodes[2].InnerXml;
                            string quantity = eventi.ChildNodes[5].InnerXml;
                            string plu = eventi.ChildNodes[6].InnerXml;
                            string sku = eventi.ChildNodes[8].InnerXml;
                            string originalAmount = eventi.ChildNodes[3].InnerXml;
                            string originalUnitPrice = eventi.ChildNodes[4].InnerXml;
                            string reason = eventi.ChildNodes[7].InnerXml;
                            string unitPrice = eventi.ChildNodes[9].InnerXml;

                            //output
                            //  id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt
                            //     + " Description " + description + " Sku " + sku + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount + " Reason " + reason + " OriginalUnitPrice R" + originalUnitPrice + " OriginalAmount R" + originalAmount;
                            id = evnt + description + " Sku " + sku + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount + " Reason " + reason + " OriginalUnitPrice R" + originalUnitPrice + " OriginalAmount R" + originalAmount;

                            port = Int32.Parse(tilli.InnerText);

                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }





                    }

                    //ITEM DISCOUNT.23
                    if (returnData.Contains("type=\"ITEM DISCOUNT\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";


                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];

                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;

                            string discountValue = eventi.ChildNodes[1].InnerXml;
                            string description = eventi.ChildNodes[2].InnerXml;

                            string plu = eventi.ChildNodes[3].InnerXml;
                            string sku = eventi.ChildNodes[5].InnerXml;

                            string reason = eventi.ChildNodes[4].InnerXml;


                            //output
                            //   id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt
                            //  + " Description " + description + " Sku " + sku +  "Discount R" + discountValue + " Reason " + reason ;
                            id = evnt + description + "Discount R" + discountValue + " Reason " + reason;
                            port = Int32.Parse(tilli.InnerText);

                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();


                        }


                    }
                    //TRANSACTION LOYALTY.24
                    if (returnData.Contains("type=\"TRANSACTION LOYALTY\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string discription = eventi.ChildNodes[0].InnerXml;


                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //   id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " " + discription;
                            id = evnt + " " + discription;
                            port = Int32.Parse(tilli.InnerText);
                        }


                        if (MAX_CH >= port)
                        {



                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }


                    }

                    //TRANSACTION DISCOUNT.25
                    if (returnData.Contains("type=\"TRANSACTION DISCOUNT\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string discountValue = eventi.ChildNodes[0].InnerXml;
                            string reason = eventi.ChildNodes[0].InnerXml;

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " " + " DiscountValue R" + discountValue + " Reason " + reason;
                            id = evnt + " " + " DiscountValue R" + discountValue + " Reason " + reason;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }

                    }

                    //TRANSACTION SUSPEND.26
                    if (returnData.Contains("type=\"TRANSACTION SUSPEND\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string amount = eventi.ChildNodes[0].InnerXml;


                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount;
                            id = evnt + " Amount R" + amount;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }


                    }

                    //TRANSACTION VOID.27
                    if (returnData.Contains("type=\"TRANSACTION VOID\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string amount = eventi.ChildNodes[0].InnerXml;
                            string reason = eventi.ChildNodes[0].InnerXml;

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //  id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount + " Reason " + reason;
                            id = " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + evnt + " Amount R" + amount + " Reason " + reason;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();


                        }


                    }

                    //TRANSACTION SEARCH.28
                    if (returnData.Contains("type=\"TRANSACTION SEARCH\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;



                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }

                    }

                    //TRANSACTION REPRINT.29
                    if (returnData.Contains("type=\"TRANSACTION REPRINT\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;



                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //  id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }

                    }
                    //ENQUIRY PRICE.30
                    if (returnData.Contains("type=\"ENQUIRY PRICE\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sku = eventi.ChildNodes[0].InnerXml;


                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //   id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " SKU " + sku;
                            id = evnt + " SKU " + sku;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }


                    }
                    //ENQUIRY GIFT CARD BALANCE.31
                    if (returnData.Contains("type=\"ENQUIRY GIFT CARD BALANCE\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;



                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //  id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }

                    }
                    //MONEY TRANSFER DEPOSIT.32
                    if (returnData.Contains("type=\"MONEY TRANSFER DEPOSIT\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string amount = eventi.ChildNodes[0].InnerXml;


                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //   id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount;
                            id = evnt + " Amount R" + amount;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }


                    }
                    //MONEY TRANSFER WITHDRAWAL.33
                    if (returnData.Contains("type=\"MONEY TRANSFER WITHDRAWAL\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string amount = eventi.ChildNodes[0].InnerXml;


                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount;
                            id = evnt + " Amount R" + amount;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }


                    }

                    //MANAGER MENU.34
                    if (returnData.Contains("type=\"MANAGER MENU\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;



                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }


                        if (MAX_CH >= port)
                        {



                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }


                    }


                    //PASSWORD FORCE.35
                    if (returnData.Contains("type=\"PASSWORD FORCE\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;



                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }

                    }
                    //VAS FAILED.36
                    if (returnData.Contains("type=\"VAS FAILED\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;



                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }


                        if (MAX_CH >= port)
                        {



                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }

                    }

                    //TENDER VOID.37
                    if (returnData.Contains("type=\"TENDER VOID\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string amount = eventi.ChildNodes[0].InnerXml;
                            string tenderName = eventi.ChildNodes[1].InnerXml;

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R:" + amount + " TenderName " + tenderName;
                            id = evnt + " Amount R:" + amount + " TenderName " + tenderName;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (40000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }
                    }












































                    XmlNodeList xnListtype = xml.SelectNodes("/POSEventMessage/event[@type='AUTHORISATION SUCCESSFUL']");

                    foreach (XmlNode xn in xnListtype)
                    {
                        Console.WriteLine(xn.InnerText);
                    }




                  






                }
                catch
                {

                }

            }
            
        }

        public void PNPb()
        {


            UdpClient udpClient = new UdpClient(4051);
            udpClient.JoinMulticastGroup(IPAddress.Parse("230.0.0.1"));
            XmlDocument xml = new XmlDocument();
            string Ipaddress = "127.0.0.1";


            while (true)
            {
                try
                {
                    string returnData = " ";


                    IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);

                    returnData = Encoding.ASCII.GetString(receiveBytes, 0, receiveBytes.Length);
                    //  returnData = Encoding.ASCII.GetString(receiveBytes);


                    // richTextBox1.Invoke(new Action(() => richTextBox1.Text = RemoteIpEndPoint.Address.ToString() + returnData + "\n"));
                    if (returnData.Contains("Lkey-"))
                    {
                        richTextBox1.Invoke(new Action(() => richTextBox1.Text = returnData));
                        var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        var l = Path.Combine(appDataDir, "lic.dat");
                        string[] lines = { returnData };
                        System.IO.File.WriteAllLines(l, lines);

                    }



                    xml.LoadXml(returnData); // suppose that myXmlString contains "<Names>...</Names>"


                    //LOGON.1
                    if (returnData.Contains("type=\"LOGON\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value;
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];


                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode +" Seq# " +sequenceNumber + evnt;
                            id = " Till# " + till + username + " UserID " + operatorCode + evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }





                        if (MAX_CH >= port)
                        {
                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }
                    }

                    //LOGON INVALID.2
                    if (returnData.Contains("type=\"LOGON INVALID\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value;
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];


                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }





                        if (MAX_CH >= port)
                        {
                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }

                    }



                    //SESSION OVERRIDE.3
                    if (returnData.Contains("type=\"SESSION OVERRIDE\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("authorisorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " AuthorisorUser " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value;
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode authorisorCodei = xn["authorisorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];


                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            // string authorisorCode = authorisorCodei.InnerText;
                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username +  " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }


                    }

                    //LOGOFF.4
                    if (returnData.Contains("type=\"LOGOFF\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value;
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];


                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            id = " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }

                    }


                    //OPEN CASH DRAWER.5
                    if (returnData.Contains("type=\"OPEN CASH DRAWER\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string cashdstatus = eventi.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + cashdstatus;
                            id = evnt + " CashDrawerstatus " + cashdstatus;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }


                    }


                    //REMOVE CASH DRAWER.6
                    if (returnData.Contains("type=\"REMOVE CASH DRAWER\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string cashdstatus = eventi.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + cashdstatus;
                            id = evnt + " CashDrawerstatus " + cashdstatus;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }





                    }

                    //SECURE.7
                    if (returnData.Contains("type=\"SECURE\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }





                    }

                    //AUTHORISATION SUCCESSFUL.8
                    if (returnData.Contains("type=\"AUTHORISATION SUCCESSFUL\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string accessPointToken = " " + eventi.ChildNodes[0].InnerXml + " ";

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + accessPointToken ;
                            id = evnt + " " + accessPointToken;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }



                    }


                    //SECURE UNLOCK.9
                    if (returnData.Contains("type=\"SECURE UNLOCK\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }


                    }

                    //AUTHORISATION FAILED.10
                    if (returnData.Contains("type=\"AUTHORISATION FAILED\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string accessPointToken = " " + eventi.ChildNodes[0].InnerXml + " ";

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + accessPointToken;
                            id = evnt + " " + accessPointToken;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }




                    }

                    //ITEM SALE.11
                    if (returnData.Contains("type=\"ITEM SALE\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";


                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];

                            XmlNode eventi = xn["event"];
                            // XmlNode descriptioni = xn["description"];
                            // XmlNode quantityi = xn["quantity"];
                            // XmlNode plui = xn["plu"];
                            // XmlNode skui = xn["sku"];
                            // XmlNode unitPricei = xn["unitPrice"];



                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;




                            string amount = eventi.ChildNodes[0].InnerXml;
                            string description = eventi.ChildNodes[2].InnerXml;
                            string quantity = eventi.ChildNodes[3].InnerXml;
                            string plu = eventi.ChildNodes[4].InnerXml;
                            string unitPrice = eventi.ChildNodes[6].InnerXml;
                            string sku = eventi.ChildNodes[5].InnerXml;





                            //output
                            //   id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt
                            //       + " Description " + description + " SKU " + sku + " PLU " + plu + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount;
                            id = description + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount;
                            port = Int32.Parse(tilli.InnerText);

                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }






                    }


                    //TENDER.12
                    if (returnData.Contains("type=\"TENDER\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string amount = eventi.ChildNodes[0].InnerXml;
                            string tendername = eventi.ChildNodes[1].InnerXml;

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount + " Tendername " + tendername;
                            id = " Amount R" + amount + " Tendername " + tendername;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }




                    }

                    //TRANSACTION TOTAL.13
                    if (returnData.Contains("type=\"TRANSACTION TOTAL\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string amount = eventi.ChildNodes[0].InnerXml;
                            ;

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //  id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount;
                            id = " Total R" + amount;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }




                    }

                    //CHANGE.14
                    if (returnData.Contains("type=\"CHANGE\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string amount = eventi.ChildNodes[0].InnerXml;
                            string tenderName = eventi.ChildNodes[1].InnerXml;
                            ;

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount + " Tendername: " + tenderName;
                            id = " Change R" + amount;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }





                    }


                    //TRANSACTION COMPLETE.15
                    if (returnData.Contains("type=\"TRANSACTION COMPLETE\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string amount = eventi.ChildNodes[0].InnerXml;


                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //  id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount;
                            id = " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + evnt + " Amount R" + amount;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }

                    }


                    //ITEM UNKNOWN.16
                    if (returnData.Contains("type=\"ITEM UNKNOWN\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];

                            XmlNode eventi = xn["event"];



                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;

                            string amount = eventi.ChildNodes[0].InnerXml;
                            string description = eventi.ChildNodes[2].InnerXml;
                            string quantity = eventi.ChildNodes[3].InnerXml;
                            string plu = eventi.ChildNodes[4].InnerXml;
                            string unitPrice = eventi.ChildNodes[5].InnerXml;




                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt
                            //   + " Description " + description  + " PLU " + plu + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount;
                            id = description + " PLU " + plu + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }







                    }

                    //ACCOUNT PAYMENT.17
                    if (returnData.Contains("type=\"ACCOUNT PAYMENT\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string amount = eventi.ChildNodes[0].InnerXml;


                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //  id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount;
                            id = " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + evnt + " Amount R" + amount;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }


                    }

                    //DEPARTMENT SALE.18
                    if (returnData.Contains("event type=\"DEPARTMENT SALE\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";


                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];

                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;

                            string amount = eventi.ChildNodes[0].InnerXml;
                            string description = eventi.ChildNodes[2].InnerXml;
                            string quantity = eventi.ChildNodes[3].InnerXml;
                            string unitPrice = eventi.ChildNodes[4].InnerXml;

                            //output
                            //    id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt
                            //      + " Description " + description  + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount;
                            id = " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + evnt
                         + description + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount;
                            port = Int32.Parse(tilli.InnerText);

                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }







                    }
                    //TRANSACTION RECALL.19
                    if (returnData.Contains("type=\"TRANSACTION RECALL\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string amount = eventi.ChildNodes[0].InnerXml;


                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount;
                            id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + evnt + " Amount R" + amount;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }




                    }
                    //ITEM REFUND.20
                    if (returnData.Contains("type=\"ITEM REFUND\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";


                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];

                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;

                            string amount = eventi.ChildNodes[0].InnerXml;
                            string description = eventi.ChildNodes[2].InnerXml;
                            string quantity = eventi.ChildNodes[3].InnerXml;
                            string sku = eventi.ChildNodes[4].InnerXml;
                            string unitPrice = eventi.ChildNodes[5].InnerXml;

                            //output
                            //    id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt
                            //    + " Description " + description + " Sku " + sku + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount;
                            id = evnt + description + " Sku " + sku + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount;

                            port = Int32.Parse(tilli.InnerText);

                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }







                    }

                    //ITEM VOID.21
                    if (returnData.Contains("type=\"ITEM VOID\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";


                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];

                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;

                            string amount = eventi.ChildNodes[0].InnerXml;
                            string description = eventi.ChildNodes[2].InnerXml;
                            string quantity = eventi.ChildNodes[3].InnerXml;
                            string plu = eventi.ChildNodes[4].InnerXml;
                            string sku = eventi.ChildNodes[6].InnerXml;
                            string reason = eventi.ChildNodes[5].InnerXml;
                            string unitPrice = eventi.ChildNodes[7].InnerXml;

                            //output
                            //   id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt
                            //         + " Description " + description + " Sku " + sku + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount + " Reason " + reason;
                            id = evnt + description + " Sku " + sku + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount + " Reason " + reason;

                            port = Int32.Parse(tilli.InnerText);

                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }







                    }
                    //ITEM PRICE OVERRIDE.22
                    if (returnData.Contains("type=\"ITEM PRICE OVERRIDE\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";


                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];

                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;

                            string amount = eventi.ChildNodes[0].InnerXml;
                            string description = eventi.ChildNodes[2].InnerXml;
                            string quantity = eventi.ChildNodes[5].InnerXml;
                            string plu = eventi.ChildNodes[6].InnerXml;
                            string sku = eventi.ChildNodes[8].InnerXml;
                            string originalAmount = eventi.ChildNodes[3].InnerXml;
                            string originalUnitPrice = eventi.ChildNodes[4].InnerXml;
                            string reason = eventi.ChildNodes[7].InnerXml;
                            string unitPrice = eventi.ChildNodes[9].InnerXml;

                            //output
                            //  id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt
                            //     + " Description " + description + " Sku " + sku + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount + " Reason " + reason + " OriginalUnitPrice R" + originalUnitPrice + " OriginalAmount R" + originalAmount;
                            id = evnt + description + " Sku " + sku + " Unit Price R" + unitPrice + " QTY " + quantity + " R" + amount + " Reason " + reason + " OriginalUnitPrice R" + originalUnitPrice + " OriginalAmount R" + originalAmount;

                            port = Int32.Parse(tilli.InnerText);

                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }





                    }

                    //ITEM DISCOUNT.23
                    if (returnData.Contains("type=\"ITEM DISCOUNT\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";


                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];

                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sequenceNumber = sequenceNumberi.InnerText;

                            string discountValue = eventi.ChildNodes[1].InnerXml;
                            string description = eventi.ChildNodes[2].InnerXml;

                            string plu = eventi.ChildNodes[3].InnerXml;
                            string sku = eventi.ChildNodes[5].InnerXml;

                            string reason = eventi.ChildNodes[4].InnerXml;


                            //output
                            //   id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt
                            //  + " Description " + description + " Sku " + sku +  "Discount R" + discountValue + " Reason " + reason ;
                            id = evnt + description + "Discount R" + discountValue + " Reason " + reason;
                            port = Int32.Parse(tilli.InnerText);

                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();


                        }


                    }
                    //TRANSACTION LOYALTY.24
                    if (returnData.Contains("type=\"TRANSACTION LOYALTY\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string discription = eventi.ChildNodes[0].InnerXml;


                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //   id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " " + discription;
                            id = evnt + " " + discription;
                            port = Int32.Parse(tilli.InnerText);
                        }


                        if (MAX_CH >= port)
                        {



                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }


                    }

                    //TRANSACTION DISCOUNT.25
                    if (returnData.Contains("type=\"TRANSACTION DISCOUNT\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string discountValue = eventi.ChildNodes[0].InnerXml;
                            string reason = eventi.ChildNodes[0].InnerXml;

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " " + " DiscountValue R" + discountValue + " Reason " + reason;
                            id = evnt + " " + " DiscountValue R" + discountValue + " Reason " + reason;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }

                    }

                    //TRANSACTION SUSPEND.26
                    if (returnData.Contains("type=\"TRANSACTION SUSPEND\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string amount = eventi.ChildNodes[0].InnerXml;


                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount;
                            id = evnt + " Amount R" + amount;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }


                    }

                    //TRANSACTION VOID.27
                    if (returnData.Contains("type=\"TRANSACTION VOID\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;

                            string amount = eventi.ChildNodes[0].InnerXml;
                            string reason = eventi.ChildNodes[0].InnerXml;

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //  id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount + " Reason " + reason;
                            id = " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + evnt + " Amount R" + amount + " Reason " + reason;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();


                        }


                    }

                    //TRANSACTION SEARCH.28
                    if (returnData.Contains("type=\"TRANSACTION SEARCH\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;



                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }

                    }

                    //TRANSACTION REPRINT.29
                    if (returnData.Contains("type=\"TRANSACTION REPRINT\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;



                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //  id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }

                    }
                    //ENQUIRY PRICE.30
                    if (returnData.Contains("type=\"ENQUIRY PRICE\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string sku = eventi.ChildNodes[0].InnerXml;


                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //   id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " SKU " + sku;
                            id = evnt + " SKU " + sku;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }


                    }
                    //ENQUIRY GIFT CARD BALANCE.31
                    if (returnData.Contains("type=\"ENQUIRY GIFT CARD BALANCE\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;



                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //  id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }




                        if (MAX_CH >= port)
                        {

                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }

                    }
                    //MONEY TRANSFER DEPOSIT.32
                    if (returnData.Contains("type=\"MONEY TRANSFER DEPOSIT\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string amount = eventi.ChildNodes[0].InnerXml;


                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //   id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount;
                            id = evnt + " Amount R" + amount;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }


                    }
                    //MONEY TRANSFER WITHDRAWAL.33
                    if (returnData.Contains("type=\"MONEY TRANSFER WITHDRAWAL\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string amount = eventi.ChildNodes[0].InnerXml;


                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R" + amount;
                            id = evnt + " Amount R" + amount;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }


                    }

                    //MANAGER MENU.34
                    if (returnData.Contains("type=\"MANAGER MENU\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;



                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }


                        if (MAX_CH >= port)
                        {



                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }


                    }


                    //PASSWORD FORCE.35
                    if (returnData.Contains("type=\"PASSWORD FORCE\""))
                    {

                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;



                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }

                    }
                    //VAS FAILED.36
                    if (returnData.Contains("type=\"VAS FAILED\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;



                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            //id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt;
                            id = evnt;
                            port = Int32.Parse(tilli.InnerText);
                        }


                        if (MAX_CH >= port)
                        {



                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();
                        }

                    }

                    //TENDER VOID.37
                    if (returnData.Contains("type=\"TENDER VOID\""))
                    {
                        id = "";
                        username = "";
                        evnt = "";
                        Int32 port = 0;

                        XmlNodeList elemList = xml.GetElementsByTagName("operatorCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            username = " UserName " + elemList[i].Attributes["name"].Value;
                        }


                        XmlNodeList evnts = xml.GetElementsByTagName("event");
                        for (int j = 0; j < evnts.Count; j++)
                        {
                            evnt = " " + evnts[j].Attributes["type"].Value + " ";
                        }

                        XmlNodeList xnList = xml.SelectNodes("/POSEventMessage");
                        foreach (XmlNode xn in xnList)
                        {
                            //list
                            XmlNode storeCodei = xn["storeCode"];
                            XmlNode tilli = xn["till"];
                            XmlNode txnNumberi = xn["txnNumber"];
                            XmlNode operatorCodei = xn["operatorCode"];
                            XmlNode sequenceNumberi = xn["sequenceNumber"];
                            XmlNode eventi = xn["event"];

                            //string
                            string storeCode = storeCodei.InnerText;
                            string till = tilli.InnerText;
                            string txNumber = txnNumberi.InnerText;
                            string operatorCode = operatorCodei.ChildNodes[0].InnerXml;
                            string amount = eventi.ChildNodes[0].InnerXml;
                            string tenderName = eventi.ChildNodes[1].InnerXml;

                            string sequenceNumber = sequenceNumberi.InnerText;
                            //output
                            // id = " StoreCode# " + storeCode + " Till# " + till + " Tx# " + txNumber + username + " UserID " + operatorCode + " Seq# " + sequenceNumber + evnt + " Amount R:" + amount + " TenderName " + tenderName;
                            id = evnt + " Amount R:" + amount + " TenderName " + tenderName;
                            port = Int32.Parse(tilli.InnerText);
                        }



                        if (MAX_CH >= port)
                        {


                            richTextBox1.Invoke(new Action(() => richTextBox1.Text = id + "\n"));

                            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                            IPAddress send_to_address = IPAddress.Parse(Ipaddress);
                            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, (43000 + port));
                            byte[] send_buffer = Encoding.ASCII.GetBytes(id + Environment.NewLine);

                            sending_socket.SendTo(send_buffer, sending_end_point);
                            sending_socket.Dispose();

                        }
                    }












































                    XmlNodeList xnListtype = xml.SelectNodes("/POSEventMessage/event[@type='AUTHORISATION SUCCESSFUL']");

                    foreach (XmlNode xn in xnListtype)
                    {
                        Console.WriteLine(xn.InnerText);
                    }











                }
                catch
                {

                }

            }

        }



        public void scale1()
        {

           

            string CCTVServer = "127.0.0.1";
            char ch = '\x0011';
            string com = "";


            UdpClient udpClient = new UdpClient(41001);

            while (true)
            {
                string returnData = " ";
               
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                returnData = Encoding.ASCII.GetString(receiveBytes);
          

               // label3.Invoke(new Action(() => label3.Text = "Data From Scale 1 : " + returnData));


                if (returnData.Contains(ch))
                {

                    try
                    {
                        richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                        char[] delimiterChars = { '\x001C' };
                        string[] words = returnData.Split(delimiterChars);
                        string line = "----------------------------------------";

                        string weight = words[5].Remove(words[5].Length - 2);
                        string plu = words[0].Remove(0, 1);

                        string filtered = "PLU CODE : " + plu + "\n" + "Item : " + words[1] + "\n" + "R/Kg             : " + words[2] + "\n" + "Total Weight : " + weight + "\n" + "Tare Value    : " + words[4] + "\n" + "Price              : R" + words[3] + "\n" + line + "\n";

                        richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 1" + "\n" + filtered));

                        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                        IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 42001);
                        byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                        sending_socket.SendTo(send_bufferw, sending_end_point);
                        sending_socket.Dispose();
                      
                    }
                    catch
                    {
                    }
                 


                }
///
            if(checkBox1.Checked)
                {
                    try
                    {
                      

                        char[] delimiterChars = { '\x001C' };
                        string[] words = returnData.Split(delimiterChars);
                        string line = "----------------------------------------";

                        string weight = words[5].Remove(words[5].Length - 2);
                        string plu = words[0].Remove(0, 1);
                        decimal d = decimal.Parse(weight);
                        decimal p = decimal.Parse(plu);
                      
                        if (p > 00001M & d > 0.070M & com != returnData)
                        { 
                            
                                richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));
                                string filtered = "PLU CODE : " + plu + "\n" + "Item : " + words[1] + "\n" + "R/Kg             : " + words[2] + "\n" + "Total Weight : " + weight + "\n" + "Tare Value    : " + words[4] + "\n" + "Price              : R" + words[3] + "\n" + line + "\n";

                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 1" + "\n" + filtered));

                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 42001);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);
                                
                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            com = returnData;
                            
                       }
                       
                    }
                    catch
                    {
                    }

                  

                }

            }

            
        }

        public void scale2()
        {



            string CCTVServer = "127.0.0.1";
            char ch = '\x0011';



            UdpClient udpClient = new UdpClient(41002);

            while (true)
            {
                string returnData = " ";

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                returnData = Encoding.ASCII.GetString(receiveBytes);


            //    label3.Invoke(new Action(() => label3.Text = "Data From Scale 2 : " + returnData));


                if (returnData.Contains(ch))
                {

                    try
                    {
                        richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                        char[] delimiterChars = { '\x001C' };
                        string[] words = returnData.Split(delimiterChars);
                        string line = "----------------------------------------";

                        string weight = words[5].Remove(words[5].Length - 2);
                        string plu = words[0].Remove(0, 1);

                        string filtered = "PLU CODE : " + plu + "\n" + "Item : " + words[1] + "\n" + "R/Kg             : " + words[2] + "\n" + "Total Weight : " + weight + "\n" + "Tare Value    : " + words[4] + "\n" + "Price              : R" + words[3] + "\n" + line + "\n";

                        richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 2" + "\n" + filtered));

                        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                        IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 42002);
                        byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                        sending_socket.SendTo(send_bufferw, sending_end_point);
                    
                        sending_socket.Dispose();
                    }
                    catch
                    {
                    }
                    

                }




            }
        }
        public void scale3()
        {



            string CCTVServer = "127.0.0.1";
            char ch = '\x0011';



            UdpClient udpClient = new UdpClient(41003);

            while (true)
            {
                string returnData = " ";

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                returnData = Encoding.ASCII.GetString(receiveBytes);


           //     label3.Invoke(new Action(() => label3.Text = "Data From Scale 3 : " + returnData));


                if (returnData.Contains(ch))
                {

                    try
                    {
                        richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                        char[] delimiterChars = { '\x001C' };
                        string[] words = returnData.Split(delimiterChars);
                        string line = "----------------------------------------";

                        string weight = words[5].Remove(words[5].Length - 2);
                        string plu = words[0].Remove(0, 1);

                        string filtered = "PLU CODE : " + plu + "\n" + "Item : " + words[1] + "\n" + "R/Kg             : " + words[2] + "\n" + "Total Weight : " + weight + "\n" + "Tare Value    : " + words[4] + "\n" + "Price              : R" + words[3] + "\n" + line + "\n";

                        richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 3" + "\n" + filtered));

                        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                        IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 42003);
                        byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                        sending_socket.SendTo(send_bufferw, sending_end_point);
                        sending_socket.Dispose();
                    }
                    catch
                    {
                    }


                }




            }
        }

        public void scale4()
        {



            string CCTVServer = "127.0.0.1";
            char ch = '\x0011';



            UdpClient udpClient = new UdpClient(41004);

            while (true)
            {
                string returnData = " ";

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                returnData = Encoding.ASCII.GetString(receiveBytes);


           //     label3.Invoke(new Action(() => label3.Text = "Data From Scale 4 : " + returnData));


                if (returnData.Contains(ch))
                {

                    try
                    {
                        richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                        char[] delimiterChars = { '\x001C' };
                        string[] words = returnData.Split(delimiterChars);
                        string line = "----------------------------------------";

                        string weight = words[5].Remove(words[5].Length - 2);
                        string plu = words[0].Remove(0, 1);

                        string filtered = "PLU CODE : " + plu + "\n" + "Item : " + words[1] + "\n" + "R/Kg             : " + words[2] + "\n" + "Total Weight : " + weight + "\n" + "Tare Value    : " + words[4] + "\n" + "Price              : R" + words[3] + "\n" + line + "\n";

                        richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 4" + "\n" + filtered));

                        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                        IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 42004);
                        byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                        sending_socket.SendTo(send_bufferw, sending_end_point);
                        sending_socket.Dispose();
                    }
                    catch
                    {
                    }


                }




            }
        }

        public void scale5()
        {



            string CCTVServer = "127.0.0.1";
            char ch = '\x0011';



            UdpClient udpClient = new UdpClient(41005);

            while (true)
            {
                string returnData = " ";

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                returnData = Encoding.ASCII.GetString(receiveBytes);


          //      label3.Invoke(new Action(() => label3.Text = "Data From Scale 5 : " + returnData));


                if (returnData.Contains(ch))
                {

                    try
                    {
                        richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                        char[] delimiterChars = { '\x001C' };
                        string[] words = returnData.Split(delimiterChars);
                        string line = "----------------------------------------";

                        string weight = words[5].Remove(words[5].Length - 2);
                        string plu = words[0].Remove(0, 1);

                        string filtered = "PLU CODE : " + plu + "\n" + "Item : " + words[1] + "\n" + "R/Kg             : " + words[2] + "\n" + "Total Weight : " + weight + "\n" + "Tare Value    : " + words[4] + "\n" + "Price              : R" + words[3] + "\n" + line + "\n";

                        richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 5" + "\n" + filtered));

                        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                        IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 42005);
                        byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                        sending_socket.SendTo(send_bufferw, sending_end_point);
                        sending_socket.Dispose();
                    }
                    catch
                    {
                    }


                }




            }
        }

        public void scale6()
        {



            string CCTVServer = "127.0.0.1";
            char ch = '\x0011';



            UdpClient udpClient = new UdpClient(41006);

            while (true)
            {
                string returnData = " ";

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                returnData = Encoding.ASCII.GetString(receiveBytes);


                label3.Invoke(new Action(() => label3.Text = "Data From Scale 6 : " + returnData));


                if (returnData.Contains(ch))
                {

                    try
                    {
                        richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                        char[] delimiterChars = { '\x001C' };
                        string[] words = returnData.Split(delimiterChars);
                        string line = "----------------------------------------";

                        string weight = words[5].Remove(words[5].Length - 2);
                        string plu = words[0].Remove(0, 1);

                        string filtered = "PLU CODE : " + plu + "\n" + "Item : " + words[1] + "\n" + "R/Kg             : " + words[2] + "\n" + "Total Weight : " + weight + "\n" + "Tare Value    : " + words[4] + "\n" + "Price              : R" + words[3] + "\n" + line + "\n";

                        richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 6" + "\n" + filtered));

                        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                        IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 42006);
                        byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                        sending_socket.SendTo(send_bufferw, sending_end_point);
                        sending_socket.Dispose();
                    }
                    catch
                    {
                    }


                }




            }
        }

        public void scale7()
        {



            string CCTVServer = "127.0.0.1";
            char ch = '\x0011';



            UdpClient udpClient = new UdpClient(41007);

            while (true)
            {
                string returnData = " ";

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                returnData = Encoding.ASCII.GetString(receiveBytes);


         //       label3.Invoke(new Action(() => label3.Text = "Data From Scale 7 : " + returnData));


                if (returnData.Contains(ch))
                {

                    try
                    {
                        richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                        char[] delimiterChars = { '\x001C' };
                        string[] words = returnData.Split(delimiterChars);
                        string line = "----------------------------------------";

                        string weight = words[5].Remove(words[5].Length - 2);
                        string plu = words[0].Remove(0, 1);

                        string filtered = "PLU CODE : " + plu + "\n" + "Item : " + words[1] + "\n" + "R/Kg             : " + words[2] + "\n" + "Total Weight : " + weight + "\n" + "Tare Value    : " + words[4] + "\n" + "Price              : R" + words[3] + "\n" + line + "\n";

                        richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 7" + "\n" + filtered));

                        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                        IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 42007);
                        byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                        sending_socket.SendTo(send_bufferw, sending_end_point);
                        sending_socket.Dispose();
                    }
                    catch
                    {
                    }


                }




            }
        }

        public void scale8()
        {



            string CCTVServer = "127.0.0.1";
            char ch = '\x0011';



            UdpClient udpClient = new UdpClient(41008);

            while (true)
            {
                string returnData = " ";

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                returnData = Encoding.ASCII.GetString(receiveBytes);


          //      label3.Invoke(new Action(() => label3.Text = "Data From Scale 8 : " + returnData));


                if (returnData.Contains(ch))
                {

                    try
                    {
                        richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                        char[] delimiterChars = { '\x001C' };
                        string[] words = returnData.Split(delimiterChars);
                        string line = "----------------------------------------";

                        string weight = words[5].Remove(words[5].Length - 2);
                        string plu = words[0].Remove(0, 1);

                        string filtered = "PLU CODE : " + plu + "\n" + "Item : " + words[1] + "\n" + "R/Kg             : " + words[2] + "\n" + "Total Weight : " + weight + "\n" + "Tare Value    : " + words[4] + "\n" + "Price              : R" + words[3] + "\n" + line + "\n";

                        richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 8" + "\n" + filtered));

                        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                        IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 42008);
                        byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                        sending_socket.SendTo(send_bufferw, sending_end_point);
                        sending_socket.Dispose();
                    }
                    catch
                    {
                    }


                }




            }
        }

        public void scale9()
        {



            string CCTVServer = "127.0.0.1";
            char ch = '\x0011';



            UdpClient udpClient = new UdpClient(41009);

            while (true)
            {
                string returnData = " ";

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                returnData = Encoding.ASCII.GetString(receiveBytes);


          //      label3.Invoke(new Action(() => label3.Text = "Data From Scale 9 : " + returnData));


                if (returnData.Contains(ch))
                {

                    try
                    {
                        richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                        char[] delimiterChars = { '\x001C' };
                        string[] words = returnData.Split(delimiterChars);
                        string line = "----------------------------------------";

                        string weight = words[5].Remove(words[5].Length - 2);
                        string plu = words[0].Remove(0, 1);

                        string filtered = "PLU CODE : " + plu + "\n" + "Item : " + words[1] + "\n" + "R/Kg             : " + words[2] + "\n" + "Total Weight : " + weight + "\n" + "Tare Value    : " + words[4] + "\n" + "Price              : R" + words[3] + "\n" + line + "\n";

                        richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 9" + "\n" + filtered));

                        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                        IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 42009);
                        byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                        sending_socket.SendTo(send_bufferw, sending_end_point);
                        sending_socket.Dispose();
                    }
                    catch
                    {
                    }


                }




            }
        }

        public void scale10()
        {



            string CCTVServer = "127.0.0.1";
            char ch = '\x0011';



            UdpClient udpClient = new UdpClient(41010);

            while (true)
            {
                string returnData = " ";

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                returnData = Encoding.ASCII.GetString(receiveBytes);


         //       label3.Invoke(new Action(() => label3.Text = "Data From Scale 10 : " + returnData));


                if (returnData.Contains(ch))
                {

                    try
                    {
                        richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                        char[] delimiterChars = { '\x001C' };
                        string[] words = returnData.Split(delimiterChars);
                        string line = "----------------------------------------";

                        string weight = words[5].Remove(words[5].Length - 2);
                        string plu = words[0].Remove(0, 1);

                        string filtered = "PLU CODE : " + plu + "\n" + "Item : " + words[1] + "\n" + "R/Kg             : " + words[2] + "\n" + "Total Weight : " + weight + "\n" + "Tare Value    : " + words[4] + "\n" + "Price              : R" + words[3] + "\n" + line + "\n";

                        richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 10" + "\n" + filtered));

                        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                        IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 42010);
                        byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                        sending_socket.SendTo(send_bufferw, sending_end_point);
                        sending_socket.Dispose();
                    }
                    catch
                    {
                    }


                }




            }
        }

        public void scale11()
        {



            string CCTVServer = "127.0.0.1";
            char ch = '\x0011';



            UdpClient udpClient = new UdpClient(41011);

            while (true)
            {
                string returnData = " ";

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                returnData = Encoding.ASCII.GetString(receiveBytes);


         //       label3.Invoke(new Action(() => label3.Text = "Data From Scale 11 : " + returnData));


                if (returnData.Contains(ch))
                {

                    try
                    {
                        richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                        char[] delimiterChars = { '\x001C' };
                        string[] words = returnData.Split(delimiterChars);
                        string line = "----------------------------------------";

                        string weight = words[5].Remove(words[5].Length - 2);
                        string plu = words[0].Remove(0, 1);

                        string filtered = "PLU CODE : " + plu + "\n" + "Item : " + words[1] + "\n" + "R/Kg             : " + words[2] + "\n" + "Total Weight : " + weight + "\n" + "Tare Value    : " + words[4] + "\n" + "Price              : R" + words[3] + "\n" + line + "\n";

                        richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 11" + "\n" + filtered));

                        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                        IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 42011);
                        byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                        sending_socket.SendTo(send_bufferw, sending_end_point);
                        sending_socket.Dispose();
                    }
                    catch
                    {
                    }


                }




            }
        }

        public void scale12()
        {



            string CCTVServer = "127.0.0.1";
            char ch = '\x0011';



            UdpClient udpClient = new UdpClient(41012);

            while (true)
            {
                string returnData = " ";

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                returnData = Encoding.ASCII.GetString(receiveBytes);


      //          label3.Invoke(new Action(() => label3.Text = "Data From Scale 12 : " + returnData));


                if (returnData.Contains(ch))
                {

                    try
                    {
                        richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                        char[] delimiterChars = { '\x001C' };
                        string[] words = returnData.Split(delimiterChars);
                        string line = "----------------------------------------";

                        string weight = words[5].Remove(words[5].Length - 2);
                        string plu = words[0].Remove(0, 1);

                        string filtered = "PLU CODE : " + plu + "\n" + "Item : " + words[1] + "\n" + "R/Kg             : " + words[2] + "\n" + "Total Weight : " + weight + "\n" + "Tare Value    : " + words[4] + "\n" + "Price              : R" + words[3] + "\n" + line + "\n";

                        richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 12" + "\n" + filtered));

                        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                        IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 42012);
                        byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                        sending_socket.SendTo(send_bufferw, sending_end_point);
                        sending_socket.Dispose();
                    }
                    catch
                    {
                    }


                }




            }
        }

        public void scale13()
        {



            string CCTVServer = "127.0.0.1";
            char ch = '\x0011';



            UdpClient udpClient = new UdpClient(41013);

            while (true)
            {
                string returnData = " ";

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                returnData = Encoding.ASCII.GetString(receiveBytes);


         //       label3.Invoke(new Action(() => label3.Text = "Data From Scale 13 : " + returnData));


                if (returnData.Contains(ch))
                {

                    try
                    {
                        richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                        char[] delimiterChars = { '\x001C' };
                        string[] words = returnData.Split(delimiterChars);
                        string line = "----------------------------------------";

                        string weight = words[5].Remove(words[5].Length - 2);
                        string plu = words[0].Remove(0, 1);

                        string filtered = "PLU CODE : " + plu + "\n" + "Item : " + words[1] + "\n" + "R/Kg             : " + words[2] + "\n" + "Total Weight : " + weight + "\n" + "Tare Value    : " + words[4] + "\n" + "Price              : R" + words[3] + "\n" + line + "\n";

                        richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 13" + "\n" + filtered));

                        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                        IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 42013);
                        byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                        sending_socket.SendTo(send_bufferw, sending_end_point);
                        sending_socket.Dispose();
                    }
                    catch
                    {
                    }


                }




            }
        }

        public void scale14()
        {



            string CCTVServer = "127.0.0.1";
            char ch = '\x0011';



            UdpClient udpClient = new UdpClient(41014);

            while (true)
            {
                string returnData = " ";

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                returnData = Encoding.ASCII.GetString(receiveBytes);


       //         label3.Invoke(new Action(() => label3.Text = "Data From Scale 14: " + returnData));


                if (returnData.Contains(ch))
                {

                    try
                    {
                        richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                        char[] delimiterChars = { '\x001C' };
                        string[] words = returnData.Split(delimiterChars);
                        string line = "----------------------------------------";

                        string weight = words[5].Remove(words[5].Length - 2);
                        string plu = words[0].Remove(0, 1);

                        string filtered = "PLU CODE : " + plu + "\n" + "Item : " + words[1] + "\n" + "R/Kg             : " + words[2] + "\n" + "Total Weight : " + weight + "\n" + "Tare Value    : " + words[4] + "\n" + "Price              : R" + words[3] + "\n" + line + "\n";

                        richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 14" + "\n" + filtered));

                        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                        IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 42014);
                        byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                        sending_socket.SendTo(send_bufferw, sending_end_point);
                        sending_socket.Dispose();
                    }
                    catch
                    {
                    }


                }




            }
        }

        public void scale15()
        {



            string CCTVServer = "127.0.0.1";
            char ch = '\x0011';



            UdpClient udpClient = new UdpClient(41015);

            while (true)
            {
                string returnData = " ";

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                returnData = Encoding.ASCII.GetString(receiveBytes);


          //      label3.Invoke(new Action(() => label3.Text = "Data From Scale 15 : " + returnData));


                if (returnData.Contains(ch))
                {

                    try
                    {
                        richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                        char[] delimiterChars = { '\x001C' };
                        string[] words = returnData.Split(delimiterChars);
                        string line = "----------------------------------------";

                        string weight = words[5].Remove(words[5].Length - 2);
                        string plu = words[0].Remove(0, 1);

                        string filtered = "PLU CODE : " + plu + "\n" + "Item : " + words[1] + "\n" + "R/Kg             : " + words[2] + "\n" + "Total Weight : " + weight + "\n" + "Tare Value    : " + words[4] + "\n" + "Price              : R" + words[3] + "\n" + line + "\n";

                        richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 15" + "\n" + filtered));

                        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                        IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 42015);
                        byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                        sending_socket.SendTo(send_bufferw, sending_end_point);
                        sending_socket.Dispose();
                    }
                    catch
                    {
                    }


                }




            }
        }

        public void scale16()
        {



            string CCTVServer = "127.0.0.1";
            char ch = '\x0011';



            UdpClient udpClient = new UdpClient(41016);

            while (true)
            {
                string returnData = " ";

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                returnData = Encoding.ASCII.GetString(receiveBytes);


          //      label3.Invoke(new Action(() => label3.Text = "Data From Scale 16 : " + returnData));


                if (returnData.Contains(ch))
                {

                    try
                    {
                        richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                        char[] delimiterChars = { '\x001C' };
                        string[] words = returnData.Split(delimiterChars);
                        string line = "----------------------------------------";

                        string weight = words[5].Remove(words[5].Length - 2);
                        string plu = words[0].Remove(0, 1);

                        string filtered = "PLU CODE : " + plu + "\n" + "Item : " + words[1] + "\n" + "R/Kg             : " + words[2] + "\n" + "Total Weight : " + weight + "\n" + "Tare Value    : " + words[4] + "\n" + "Price              : R" + words[3] + "\n" + line + "\n";

                        richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 16" + "\n" + filtered));

                        Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                        IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 42016);
                        byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                        sending_socket.SendTo(send_bufferw, sending_end_point);
                        sending_socket.Dispose();
                    }
                    catch
                    {
                    }


                }




            }
        }

        public void isida()
        {


            string ishidaport = "50022";
            Int32 ishidaporti = Int32.Parse(ishidaport);



            UdpClient udpClient = new UdpClient(ishidaporti);

            while (true)
            {
                string returnData = " ";

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                returnData = Encoding.ASCII.GetString(receiveBytes);
                var Firstchars = new byte[] { 0x02 };
                string firstchars = Encoding.ASCII.GetString(Firstchars);
                string firstcharp = returnData.Substring(0, 1);
                string isidalog = RemoteIpEndPoint.Address.ToString() + returnData;
                richTextBox3.Invoke(new Action(() => richTextBox3.Text = isidalog));
                //label10.Invoke(new Action(() => label10.Text = "Scale 10 Data : " + returnData));

                string ishidaipa = textBox1.Text;
                string ishidaipb = textBox2.Text;
                string ishidaipc = textBox3.Text;
                string ishidaipd = textBox4.Text;
                string ishidaipe = textBox5.Text;

                string ishidaipf = textBox6.Text;
                string ishidaipg = textBox7.Text;
                string ishidaiph = textBox8.Text;
                string ishidaipi = textBox9.Text;
                string ishidaipj = textBox10.Text;

                string ishidaipk = textBox11.Text;
                string ishidaipl = textBox12.Text;
                string ishidaipm = textBox13.Text;
                string ishidaipn = textBox14.Text;
                string ishidaipo = textBox15.Text;
                string ishidaipp = textBox16.Text;

                //Scale 1
                if (firstchars == firstcharp)
                {
                    if (isidalog.Contains(ishidaipa))
                    {

                        try
                        {
                            
                            Int32 scaleSPn = 42001;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));


                            char[] delimiterChars = { '\x000A' };
                            string[] words = returnData.Split(delimiterChars);
                            string line = "----------------------------------------";

                            string weights = words[6].TrimStart(new Char[] { '0' });
                            string prices = words[7].TrimStart(new Char[] { '0' });
                            string rpkgs = words[5].TrimStart(new Char[] { '0' });

                            double w = Int32.Parse(weights);
                            double p = Int32.Parse(prices);
                            double rp = Int32.Parse(rpkgs);

                            double wc = (w / 1000);
                            double pc = (p / 100);
                            double rpc = (rp / 100);

                            string weight = wc.ToString();
                            string price = pc.ToString();
                            string rpkg = rpc.ToString();

                            string filtered = "PLU CODE : " + words[2] + "\n" + "Item : " + words[3] + "\n" + "Total Weight : " + weight + "\n" + "R/Kg             : " + rpkg + "\n" + "Tare Value     : " + words[4] + "\n" + "Price              : R" + price + "\n" + line + "\n";

                            if (MAX_CHs >= 1)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 1" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }


                    //Scale 2
                    if (isidalog.Contains(ishidaipb))
                    {

                        try
                        {

                            Int32 scaleSPn = 42002;

                            string CCTVServer = "127.0.0.1";
                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));


                            char[] delimiterChars = { '\x000A' };
                            string[] words = returnData.Split(delimiterChars);
                            string line = "----------------------------------------";

                            string weights = words[6].TrimStart(new Char[] { '0' });
                            string prices = words[7].TrimStart(new Char[] { '0' });
                            string rpkgs = words[5].TrimStart(new Char[] { '0' });

                            double w = Int32.Parse(weights);
                            double p = Int32.Parse(prices);
                            double rp = Int32.Parse(rpkgs);

                            double wc = (w / 1000);
                            double pc = (p / 100);
                            double rpc = (rp / 100);

                            string weight = wc.ToString();
                            string price = pc.ToString();
                            string rpkg = rpc.ToString();

                            string filtered = "PLU CODE : " + words[2] + "\n" + "Item : " + words[3] + "\n" + "Total Weight : " + weight + "\n" + "R/Kg             : " + rpkg + "\n" + "Tare Value     : " + words[4] + "\n" + "Price              : R" + price + "\n" + line + "\n";
                            if (MAX_CHs >= 2)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 2" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }
                    }


                    //Scale 3
                    if (isidalog.Contains(ishidaipc))
                    {

                        try
                        {
                            Int32 scaleSPn = 42003;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));


                            char[] delimiterChars = { '\x000A' };
                            string[] words = returnData.Split(delimiterChars);
                            string line = "----------------------------------------";

                            string weights = words[6].TrimStart(new Char[] { '0' });
                            string prices = words[7].TrimStart(new Char[] { '0' });
                            string rpkgs = words[5].TrimStart(new Char[] { '0' });

                            double w = Int32.Parse(weights);
                            double p = Int32.Parse(prices);
                            double rp = Int32.Parse(rpkgs);

                            double wc = (w / 1000);
                            double pc = (p / 100);
                            double rpc = (rp / 100);

                            string weight = wc.ToString();
                            string price = pc.ToString();
                            string rpkg = rpc.ToString();

                            string filtered = "PLU CODE : " + words[2] + "\n" + "Item : " + words[3] + "\n" + "Total Weight : " + weight + "\n" + "R/Kg             : " + rpkg + "\n" + "Tare Value     : " + words[4] + "\n" + "Price              : R" + price + "\n" + line + "\n";
                            if (MAX_CHs >= 3)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 3" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }

                    //Scale 4
                    if (isidalog.Contains(ishidaipd))

                    {
                        try
                        {
                            Int32 scaleSPn = 42004;

                            string CCTVServer = "127.0.0.1";

                          

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));


                            char[] delimiterChars = { '\x000A' };
                            string[] words = returnData.Split(delimiterChars);
                            string line = "----------------------------------------";

                            string weights = words[6].TrimStart(new Char[] { '0' });
                            string prices = words[7].TrimStart(new Char[] { '0' });
                            string rpkgs = words[5].TrimStart(new Char[] { '0' });

                            double w = Int32.Parse(weights);
                            double p = Int32.Parse(prices);
                            double rp = Int32.Parse(rpkgs);

                            double wc = (w / 1000);
                            double pc = (p / 100);
                            double rpc = (rp / 100);

                            string weight = wc.ToString();
                            string price = pc.ToString();
                            string rpkg = rpc.ToString();

                            string filtered = "PLU CODE : " + words[2] + "\n" + "Item : " + words[3] + "\n" + "Total Weight : " + weight + "\n" + "R/Kg             : " + rpkg + "\n" + "Tare Value     : " + words[4] + "\n" + "Price              : R" + price + "\n" + line + "\n";
                            if (MAX_CHs >= 4)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 4" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }

                    //Scale 5
                    if (isidalog.Contains(ishidaipe))
                    {

                        try
                        {
                            Int32 scaleSPn = 42005;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));


                            char[] delimiterChars = { '\x000A' };
                            string[] words = returnData.Split(delimiterChars);
                            string line = "----------------------------------------";

                            string weights = words[6].TrimStart(new Char[] { '0' });
                            string prices = words[7].TrimStart(new Char[] { '0' });
                            string rpkgs = words[5].TrimStart(new Char[] { '0' });

                            double w = Int32.Parse(weights);
                            double p = Int32.Parse(prices);
                            double rp = Int32.Parse(rpkgs);

                            double wc = (w / 1000);
                            double pc = (p / 100);
                            double rpc = (rp / 100);

                            string weight = wc.ToString();
                            string price = pc.ToString();
                            string rpkg = rpc.ToString();

                            string filtered = "PLU CODE : " + words[2] + "\n" + "Item : " + words[3] + "\n" + "Total Weight : " + weight + "\n" + "R/Kg             : " + rpkg + "\n" + "Tare Value     : " + words[4] + "\n" + "Price              : R" + price + "\n" + line + "\n";
                            if (MAX_CHs >= 5)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 5 " + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }


                    //Scale 6
                    if (isidalog.Contains(ishidaipf))
                    {

                        try
                        {
                            Int32 scaleSPn = 42006;

                            string CCTVServer = "127.0.0.1";
                           

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));


                            char[] delimiterChars = { '\x000A' };
                            string[] words = returnData.Split(delimiterChars);
                            string line = "----------------------------------------";

                            string weights = words[6].TrimStart(new Char[] { '0' });
                            string prices = words[7].TrimStart(new Char[] { '0' });
                            string rpkgs = words[5].TrimStart(new Char[] { '0' });

                            double w = Int32.Parse(weights);
                            double p = Int32.Parse(prices);
                            double rp = Int32.Parse(rpkgs);

                            double wc = (w / 1000);
                            double pc = (p / 100);
                            double rpc = (rp / 100);

                            string weight = wc.ToString();
                            string price = pc.ToString();
                            string rpkg = rpc.ToString();

                            string filtered = "PLU CODE : " + words[2] + "\n" + "Item : " + words[3] + "\n" + "Total Weight : " + weight + "\n" + "R/Kg             : " + rpkg + "\n" + "Tare Value     : " + words[4] + "\n" + "Price              : R" + price + "\n" + line + "\n";
                            if (MAX_CHs >= 6)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 6 " + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }

                    //Scale 7
                    if (isidalog.Contains(ishidaipg))
                    {
                        try
                        {
                            Int32 scaleSPn = 42007;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));


                            char[] delimiterChars = { '\x000A' };
                            string[] words = returnData.Split(delimiterChars);
                            string line = "----------------------------------------";

                            string weights = words[6].TrimStart(new Char[] { '0' });
                            string prices = words[7].TrimStart(new Char[] { '0' });
                            string rpkgs = words[5].TrimStart(new Char[] { '0' });

                            double w = Int32.Parse(weights);
                            double p = Int32.Parse(prices);
                            double rp = Int32.Parse(rpkgs);

                            double wc = (w / 1000);
                            double pc = (p / 100);
                            double rpc = (rp / 100);

                            string weight = wc.ToString();
                            string price = pc.ToString();
                            string rpkg = rpc.ToString();

                            string filtered = "PLU CODE : " + words[2] + "\n" + "Item : " + words[3] + "\n" + "Total Weight : " + weight + "\n" + "R/Kg             : " + rpkg + "\n" + "Tare Value     : " + words[4] + "\n" + "Price              : R" + price + "\n" + line + "\n";
                            if (MAX_CHs >= 7)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 7" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }

                    //Scale 8
                    if (isidalog.Contains(ishidaiph))
                    {

                        try

                        {
                            Int32 scaleSPn = 42008;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));


                            char[] delimiterChars = { '\x000A' };
                            string[] words = returnData.Split(delimiterChars);
                            string line = "----------------------------------------";

                            string weights = words[6].TrimStart(new Char[] { '0' });
                            string prices = words[7].TrimStart(new Char[] { '0' });
                            string rpkgs = words[5].TrimStart(new Char[] { '0' });

                            double w = Int32.Parse(weights);
                            double p = Int32.Parse(prices);
                            double rp = Int32.Parse(rpkgs);

                            double wc = (w / 1000);
                            double pc = (p / 100);
                            double rpc = (rp / 100);

                            string weight = wc.ToString();
                            string price = pc.ToString();
                            string rpkg = rpc.ToString();

                            string filtered = "PLU CODE : " + words[2] + "\n" + "Item : " + words[3] + "\n" + "Total Weight : " + weight + "\n" + "R/Kg             : " + rpkg + "\n" + "Tare Value     : " + words[4] + "\n" + "Price              : R" + price + "\n" + line + "\n";
                            if (MAX_CHs >= 8)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 8" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }


                    //Scale 9
                    if (isidalog.Contains(ishidaipi))
                    {

                        try
                        {
                            Int32 scaleSPn = 42009;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));


                            char[] delimiterChars = { '\x000A' };
                            string[] words = returnData.Split(delimiterChars);
                            string line = "----------------------------------------";

                            string weights = words[6].TrimStart(new Char[] { '0' });
                            string prices = words[7].TrimStart(new Char[] { '0' });
                            string rpkgs = words[5].TrimStart(new Char[] { '0' });

                            double w = Int32.Parse(weights);
                            double p = Int32.Parse(prices);
                            double rp = Int32.Parse(rpkgs);

                            double wc = (w / 1000);
                            double pc = (p / 100);
                            double rpc = (rp / 100);

                            string weight = wc.ToString();
                            string price = pc.ToString();
                            string rpkg = rpc.ToString();

                            string filtered = "PLU CODE : " + words[2] + "\n" + "Item : " + words[3] + "\n" + "Total Weight : " + weight + "\n" + "R/Kg             : " + rpkg + "\n" + "Tare Value     : " + words[4] + "\n" + "Price              : R" + price + "\n" + line + "\n";
                            if (MAX_CHs >= 9)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 9 " + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }

                    //Scale 10
                    if (isidalog.Contains(ishidaipj))
                    {

                        try
                        {
                            Int32 scaleSPn = 42010;

                            string CCTVServer = "127.0.0.1";
                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));


                            char[] delimiterChars = { '\x000A' };
                            string[] words = returnData.Split(delimiterChars);
                            string line = "----------------------------------------";

                            string weights = words[6].TrimStart(new Char[] { '0' });
                            string prices = words[7].TrimStart(new Char[] { '0' });
                            string rpkgs = words[5].TrimStart(new Char[] { '0' });

                            double w = Int32.Parse(weights);
                            double p = Int32.Parse(prices);
                            double rp = Int32.Parse(rpkgs);

                            double wc = (w / 1000);
                            double pc = (p / 100);
                            double rpc = (rp / 100);

                            string weight = wc.ToString();
                            string price = pc.ToString();
                            string rpkg = rpc.ToString();

                            string filtered = "PLU CODE : " + words[2] + "\n" + "Item : " + words[3] + "\n" + "Total Weight : " + weight + "\n" + "R/Kg             : " + rpkg + "\n" + "Tare Value     : " + words[4] + "\n" + "Price              : R" + price + "\n" + line + "\n";
                            if (MAX_CHs >= 10)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 10" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                }
                //Scale 11
                if (firstchars == firstcharp)
                {
                    if (isidalog.Contains(ishidaipk))
                    {

                        try
                        {

                            Int32 scaleSPn = 42011;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));


                            char[] delimiterChars = { '\x000A' };
                            string[] words = returnData.Split(delimiterChars);
                            string line = "----------------------------------------";

                            string weights = words[6].TrimStart(new Char[] { '0' });
                            string prices = words[7].TrimStart(new Char[] { '0' });
                            string rpkgs = words[5].TrimStart(new Char[] { '0' });

                            double w = Int32.Parse(weights);
                            double p = Int32.Parse(prices);
                            double rp = Int32.Parse(rpkgs);

                            double wc = (w / 1000);
                            double pc = (p / 100);
                            double rpc = (rp / 100);

                            string weight = wc.ToString();
                            string price = pc.ToString();
                            string rpkg = rpc.ToString();

                            string filtered = "PLU CODE : " + words[2] + "\n" + "Item : " + words[3] + "\n" + "Total Weight : " + weight + "\n" + "R/Kg             : " + rpkg + "\n" + "Tare Value     : " + words[4] + "\n" + "Price              : R" + price + "\n" + line + "\n";
                            if (MAX_CHs >= 11)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 11" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                }
                //Scale 12
                if (firstchars == firstcharp)
                {
                    if (isidalog.Contains(ishidaipl))
                    {

                        try
                        {

                            Int32 scaleSPn = 42012;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));


                            char[] delimiterChars = { '\x000A' };
                            string[] words = returnData.Split(delimiterChars);
                            string line = "----------------------------------------";

                            string weights = words[6].TrimStart(new Char[] { '0' });
                            string prices = words[7].TrimStart(new Char[] { '0' });
                            string rpkgs = words[5].TrimStart(new Char[] { '0' });

                            double w = Int32.Parse(weights);
                            double p = Int32.Parse(prices);
                            double rp = Int32.Parse(rpkgs);

                            double wc = (w / 1000);
                            double pc = (p / 100);
                            double rpc = (rp / 100);

                            string weight = wc.ToString();
                            string price = pc.ToString();
                            string rpkg = rpc.ToString();

                            string filtered = "PLU CODE : " + words[2] + "\n" + "Item : " + words[3] + "\n" + "Total Weight : " + weight + "\n" + "R/Kg             : " + rpkg + "\n" + "Tare Value     : " + words[4] + "\n" + "Price              : R" + price + "\n" + line + "\n";
                            if (MAX_CHs >= 12)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 12" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                               
                            }
                        }
                        catch
                        {
                        }

                    }

                }
                //Scale 13
                if (firstchars == firstcharp)
                {
                    if (isidalog.Contains(ishidaipm))
                    {

                        try
                        {

                            Int32 scaleSPn = 42013;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));


                            char[] delimiterChars = { '\x000A' };
                            string[] words = returnData.Split(delimiterChars);
                            string line = "----------------------------------------";

                            string weights = words[6].TrimStart(new Char[] { '0' });
                            string prices = words[7].TrimStart(new Char[] { '0' });
                            string rpkgs = words[5].TrimStart(new Char[] { '0' });

                            double w = Int32.Parse(weights);
                            double p = Int32.Parse(prices);
                            double rp = Int32.Parse(rpkgs);

                            double wc = (w / 1000);
                            double pc = (p / 100);
                            double rpc = (rp / 100);

                            string weight = wc.ToString();
                            string price = pc.ToString();
                            string rpkg = rpc.ToString();

                            string filtered = "PLU CODE : " + words[2] + "\n" + "Item : " + words[3] + "\n" + "Total Weight : " + weight + "\n" + "R/Kg             : " + rpkg + "\n" + "Tare Value     : " + words[4] + "\n" + "Price              : R" + price + "\n" + line + "\n";
                            if (MAX_CHs >= 13)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 13" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }

                }
                //Scale 14
                if (firstchars == firstcharp)
                {
                    if (isidalog.Contains(ishidaipn))
                    {

                        try
                        {

                            Int32 scaleSPn = 42014;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));


                            char[] delimiterChars = { '\x000A' };
                            string[] words = returnData.Split(delimiterChars);
                            string line = "----------------------------------------";

                            string weights = words[6].TrimStart(new Char[] { '0' });
                            string prices = words[7].TrimStart(new Char[] { '0' });
                            string rpkgs = words[5].TrimStart(new Char[] { '0' });

                            double w = Int32.Parse(weights);
                            double p = Int32.Parse(prices);
                            double rp = Int32.Parse(rpkgs);

                            double wc = (w / 1000);
                            double pc = (p / 100);
                            double rpc = (rp / 100);

                            string weight = wc.ToString();
                            string price = pc.ToString();
                            string rpkg = rpc.ToString();

                            string filtered = "PLU CODE : " + words[2] + "\n" + "Item : " + words[3] + "\n" + "Total Weight : " + weight + "\n" + "R/Kg             : " + rpkg + "\n" + "Tare Value     : " + words[4] + "\n" + "Price              : R" + price + "\n" + line + "\n";
                            if (MAX_CHs >= 14)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 14" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }

                }
                //Scale 15
                if (firstchars == firstcharp)
                {
                    if (isidalog.Contains(ishidaipo))
                    {

                        try
                        {

                            Int32 scaleSPn = 42015;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));


                            char[] delimiterChars = { '\x000A' };
                            string[] words = returnData.Split(delimiterChars);
                            string line = "----------------------------------------";

                            string weights = words[6].TrimStart(new Char[] { '0' });
                            string prices = words[7].TrimStart(new Char[] { '0' });
                            string rpkgs = words[5].TrimStart(new Char[] { '0' });

                            double w = Int32.Parse(weights);
                            double p = Int32.Parse(prices);
                            double rp = Int32.Parse(rpkgs);

                            double wc = (w / 1000);
                            double pc = (p / 100);
                            double rpc = (rp / 100);

                            string weight = wc.ToString();
                            string price = pc.ToString();
                            string rpkg = rpc.ToString();

                            string filtered = "PLU CODE : " + words[2] + "\n" + "Item : " + words[3] + "\n" + "Total Weight : " + weight + "\n" + "R/Kg             : " + rpkg + "\n" + "Tare Value     : " + words[4] + "\n" + "Price              : R" + price + "\n" + line + "\n";
                            if (MAX_CHs >= 15)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 15" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                }
                //Scale 16
                if (firstchars == firstcharp)
                {
                    if (isidalog.Contains(ishidaipa))
                    {

                        try
                        {

                            Int32 scaleSPn = 42016;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));


                            char[] delimiterChars = { '\x000A' };
                            string[] words = returnData.Split(delimiterChars);
                            string line = "----------------------------------------";

                            string weights = words[6].TrimStart(new Char[] { '0' });
                            string prices = words[7].TrimStart(new Char[] { '0' });
                            string rpkgs = words[5].TrimStart(new Char[] { '0' });

                            double w = Int32.Parse(weights);
                            double p = Int32.Parse(prices);
                            double rp = Int32.Parse(rpkgs);

                            double wc = (w / 1000);
                            double pc = (p / 100);
                            double rpc = (rp / 100);

                            string weight = wc.ToString();
                            string price = pc.ToString();
                            string rpkg = rpc.ToString();

                            string filtered = "PLU CODE : " + words[2] + "\n" + "Item : " + words[3] + "\n" + "Total Weight : " + weight + "\n" + "R/Kg             : " + rpkg + "\n" + "Tare Value     : " + words[4] + "\n" + "Price              : R" + price + "\n" + line + "\n";
                            if (MAX_CHs >= 16)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 16" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                }

                //code for BC4000
                else
                {

                    //Scale 1

                    if (isidalog.Contains(ishidaipa))
                    {

                        try
                        {
                            Int32 scaleSPn = 42001;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                            string hexdata = GetHexStringFrom(receiveBytes);
                            string lasdata = hexdata.Substring(hexdata.Length - 47);
                            string v = lasdata.Replace("-", "");

                            string priceh = v.Substring(24, 8).TrimStart(new Char[] { '0' });
                            string priceF = priceh.Insert(priceh.Length - 2, ".");
                            string tare = v.Substring(4, 4);
                            string tareF = tare.Insert(tare.Length - 3, ",");
                            string Randskg = v.Substring(8, 8).TrimStart(new Char[] { '0' });
                            string RandskgF = Randskg.Insert(Randskg.Length - 2, ".");
                            string mass = v.Substring(16, 8).TrimStart(new Char[] { '0' });
                            string line = "----------------------------------------";

                            returnData = Encoding.Default.GetString(receiveBytes);

                            string desc = returnData.Substring(returnData.Length - 46, 30);



                            string filtered = "Item : " + desc + "\n" + "Total Weight : " + mass + "g" + "\n" + "R/Kg             : " + RandskgF + "\n" + "Tare Value     : " + tareF + "\n" + "Price              : R" + priceF + "\n" + line + "\n";
                            if (MAX_CHs >= 1)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 1" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                    //-----------------------------------------------------------------------------------------
                    //Scale 2

                    if (isidalog.Contains(ishidaipb))
                    {

                        try
                        {
                            Int32 scaleSPn = 42002;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                            string hexdata = GetHexStringFrom(receiveBytes);
                            string lasdata = hexdata.Substring(hexdata.Length - 47);
                            string v = lasdata.Replace("-", "");

                            string priceh = v.Substring(24, 8).TrimStart(new Char[] { '0' });
                            string priceF = priceh.Insert(priceh.Length - 2, ".");
                            string tare = v.Substring(4, 4);
                            string tareF = tare.Insert(tare.Length - 3, ",");
                            string Randskg = v.Substring(8, 8).TrimStart(new Char[] { '0' });
                            string RandskgF = Randskg.Insert(Randskg.Length - 2, ".");
                            string mass = v.Substring(16, 8).TrimStart(new Char[] { '0' });
                            string line = "----------------------------------------";

                            returnData = Encoding.Default.GetString(receiveBytes);

                            string desc = returnData.Substring(returnData.Length - 46, 30);



                            string filtered = "Item : " + desc + "\n" + "Total Weight : " + mass + "g" + "\n" + "R/Kg             : " + RandskgF + "\n" + "Tare Value     : " + tareF + "\n" + "Price              : R" + priceF + "\n" + line + "\n";
                            if (MAX_CHs >= 2)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 2" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                    //-----------------------------------------------------------------------------------------
                    //Scale 3

                    if (isidalog.Contains(ishidaipc))
                    {

                        try
                        {
                            Int32 scaleSPn = 42003;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                            string hexdata = GetHexStringFrom(receiveBytes);
                            string lasdata = hexdata.Substring(hexdata.Length - 47);
                            string v = lasdata.Replace("-", "");

                            string priceh = v.Substring(24, 8).TrimStart(new Char[] { '0' });
                            string priceF = priceh.Insert(priceh.Length - 2, ".");
                            string tare = v.Substring(4, 4);
                            string tareF = tare.Insert(tare.Length - 3, ",");
                            string Randskg = v.Substring(8, 8).TrimStart(new Char[] { '0' });
                            string RandskgF = Randskg.Insert(Randskg.Length - 2, ".");
                            string mass = v.Substring(16, 8).TrimStart(new Char[] { '0' });
                            string line = "----------------------------------------";

                            returnData = Encoding.Default.GetString(receiveBytes);

                            string desc = returnData.Substring(returnData.Length - 46, 30);



                            string filtered = "Item : " + desc + "\n" + "Total Weight : " + mass + "g" + "\n" + "R/Kg             : " + RandskgF + "\n" + "Tare Value     : " + tareF + "\n" + "Price              : R" + priceF + "\n" + line + "\n";
                            if (MAX_CHs >= 3)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 3 " + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                    //-----------------------------------------------------------------------------------------
                    //Scale 4

                    if (isidalog.Contains(ishidaipd))
                    {

                        try
                        {
                            Int32 scaleSPn = 42004;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                            string hexdata = GetHexStringFrom(receiveBytes);
                            string lasdata = hexdata.Substring(hexdata.Length - 47);
                            string v = lasdata.Replace("-", "");

                            string priceh = v.Substring(24, 8).TrimStart(new Char[] { '0' });
                            string priceF = priceh.Insert(priceh.Length - 2, ".");
                            string tare = v.Substring(4, 4);
                            string tareF = tare.Insert(tare.Length - 3, ",");
                            string Randskg = v.Substring(8, 8).TrimStart(new Char[] { '0' });
                            string RandskgF = Randskg.Insert(Randskg.Length - 2, ".");
                            string mass = v.Substring(16, 8).TrimStart(new Char[] { '0' });
                            string line = "----------------------------------------";

                            returnData = Encoding.Default.GetString(receiveBytes);

                            string desc = returnData.Substring(returnData.Length - 46, 30);



                            string filtered = "Item : " + desc + "\n" + "Total Weight : " + mass + "g" + "\n" + "R/Kg             : " + RandskgF + "\n" + "Tare Value     : " + tareF + "\n" + "Price              : R" + priceF + "\n" + line + "\n";
                            if (MAX_CHs >= 4)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 4" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                    //-----------------------------------------------------------------------------------------
                    //Scale 5

                    if (isidalog.Contains(ishidaipe))
                    {

                        try
                        {
                            Int32 scaleSPn = 42005;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                            string hexdata = GetHexStringFrom(receiveBytes);
                            string lasdata = hexdata.Substring(hexdata.Length - 47);
                            string v = lasdata.Replace("-", "");

                            string priceh = v.Substring(24, 8).TrimStart(new Char[] { '0' });
                            string priceF = priceh.Insert(priceh.Length - 2, ".");
                            string tare = v.Substring(4, 4);
                            string tareF = tare.Insert(tare.Length - 3, ",");
                            string Randskg = v.Substring(8, 8).TrimStart(new Char[] { '0' });
                            string RandskgF = Randskg.Insert(Randskg.Length - 2, ".");
                            string mass = v.Substring(16, 8).TrimStart(new Char[] { '0' });
                            string line = "----------------------------------------";

                            returnData = Encoding.Default.GetString(receiveBytes);

                            string desc = returnData.Substring(returnData.Length - 46, 30);



                            string filtered = "Item : " + desc + "\n" + "Total Weight : " + mass + "g" + "\n" + "R/Kg             : " + RandskgF + "\n" + "Tare Value     : " + tareF + "\n" + "Price              : R" + priceF + "\n" + line + "\n";
                            if (MAX_CHs >= 5)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 5 " + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                    //-----------------------------------------------------------------------------------------
                    //Scale 6

                    if (isidalog.Contains(ishidaipf))
                    {

                        try
                        {
                            Int32 scaleSPn = 42006;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                            string hexdata = GetHexStringFrom(receiveBytes);
                            string lasdata = hexdata.Substring(hexdata.Length - 47);
                            string v = lasdata.Replace("-", "");

                            string priceh = v.Substring(24, 8).TrimStart(new Char[] { '0' });
                            string priceF = priceh.Insert(priceh.Length - 2, ".");
                            string tare = v.Substring(4, 4);
                            string tareF = tare.Insert(tare.Length - 3, ",");
                            string Randskg = v.Substring(8, 8).TrimStart(new Char[] { '0' });
                            string RandskgF = Randskg.Insert(Randskg.Length - 2, ".");
                            string mass = v.Substring(16, 8).TrimStart(new Char[] { '0' });
                            string line = "----------------------------------------";

                            returnData = Encoding.Default.GetString(receiveBytes);

                            string desc = returnData.Substring(returnData.Length - 46, 30);



                            string filtered = "Item : " + desc + "\n" + "Total Weight : " + mass + "g" + "\n" + "R/Kg             : " + RandskgF + "\n" + "Tare Value     : " + tareF + "\n" + "Price              : R" + priceF + "\n" + line + "\n";
                            if (MAX_CHs >= 6)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 6" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                    //-----------------------------------------------------------------------------------------
                    //Scale 7

                    if (isidalog.Contains(ishidaipg))
                    {

                        try
                        {
                            Int32 scaleSPn = 42007;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                            string hexdata = GetHexStringFrom(receiveBytes);
                            string lasdata = hexdata.Substring(hexdata.Length - 47);
                            string v = lasdata.Replace("-", "");

                            string priceh = v.Substring(24, 8).TrimStart(new Char[] { '0' });
                            string priceF = priceh.Insert(priceh.Length - 2, ".");
                            string tare = v.Substring(4, 4);
                            string tareF = tare.Insert(tare.Length - 3, ",");
                            string Randskg = v.Substring(8, 8).TrimStart(new Char[] { '0' });
                            string RandskgF = Randskg.Insert(Randskg.Length - 2, ".");
                            string mass = v.Substring(16, 8).TrimStart(new Char[] { '0' });
                            string line = "----------------------------------------";

                            returnData = Encoding.Default.GetString(receiveBytes);

                            string desc = returnData.Substring(returnData.Length - 46, 30);



                            string filtered = "Item : " + desc + "\n" + "Total Weight : " + mass + "g" + "\n" + "R/Kg             : " + RandskgF + "\n" + "Tare Value     : " + tareF + "\n" + "Price              : R" + priceF + "\n" + line + "\n";
                            if (MAX_CHs >= 7)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 7 " + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                    //-----------------------------------------------------------------------------------------
                    //Scale 8

                    if (isidalog.Contains(ishidaiph))
                    {

                        try
                        {
                            Int32 scaleSPn = 42008;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                            string hexdata = GetHexStringFrom(receiveBytes);
                            string lasdata = hexdata.Substring(hexdata.Length - 47);
                            string v = lasdata.Replace("-", "");

                            string priceh = v.Substring(24, 8).TrimStart(new Char[] { '0' });
                            string priceF = priceh.Insert(priceh.Length - 2, ".");
                            string tare = v.Substring(4, 4);
                            string tareF = tare.Insert(tare.Length - 3, ",");
                            string Randskg = v.Substring(8, 8).TrimStart(new Char[] { '0' });
                            string RandskgF = Randskg.Insert(Randskg.Length - 2, ".");
                            string mass = v.Substring(16, 8).TrimStart(new Char[] { '0' });
                            string line = "----------------------------------------";

                            returnData = Encoding.Default.GetString(receiveBytes);

                            string desc = returnData.Substring(returnData.Length - 46, 30);



                            string filtered = "Item : " + desc + "\n" + "Total Weight : " + mass + "g" + "\n" + "R/Kg             : " + RandskgF + "\n" + "Tare Value     : " + tareF + "\n" + "Price              : R" + priceF + "\n" + line + "\n";
                            if (MAX_CHs >= 8)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 8 " + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                    //-----------------------------------------------------------------------------------------
                    //Scale 9

                    if (isidalog.Contains(ishidaipi))
                    {

                        try
                        {
                            Int32 scaleSPn = 42009;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                            string hexdata = GetHexStringFrom(receiveBytes);
                            string lasdata = hexdata.Substring(hexdata.Length - 47);
                            string v = lasdata.Replace("-", "");

                            string priceh = v.Substring(24, 8).TrimStart(new Char[] { '0' });
                            string priceF = priceh.Insert(priceh.Length - 2, ".");
                            string tare = v.Substring(4, 4);
                            string tareF = tare.Insert(tare.Length - 3, ",");
                            string Randskg = v.Substring(8, 8).TrimStart(new Char[] { '0' });
                            string RandskgF = Randskg.Insert(Randskg.Length - 2, ".");
                            string mass = v.Substring(16, 8).TrimStart(new Char[] { '0' });
                            string line = "----------------------------------------";

                            returnData = Encoding.Default.GetString(receiveBytes);

                            string desc = returnData.Substring(returnData.Length - 46, 30);



                            string filtered = "Item : " + desc + "\n" + "Total Weight : " + mass + "g" + "\n" + "R/Kg             : " + RandskgF + "\n" + "Tare Value     : " + tareF + "\n" + "Price              : R" + priceF + "\n" + line + "\n";
                            if (MAX_CHs >= 9)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 9" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                    //-----------------------------------------------------------------------------------------
                    //Scale 10

                    if (isidalog.Contains(ishidaipj))
                    {

                        try
                        {
                            Int32 scaleSPn = 42010;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                            string hexdata = GetHexStringFrom(receiveBytes);
                            string lasdata = hexdata.Substring(hexdata.Length - 47);
                            string v = lasdata.Replace("-", "");

                            string priceh = v.Substring(24, 8).TrimStart(new Char[] { '0' });
                            string priceF = priceh.Insert(priceh.Length - 2, ".");
                            string tare = v.Substring(4, 4);
                            string tareF = tare.Insert(tare.Length - 3, ",");
                            string Randskg = v.Substring(8, 8).TrimStart(new Char[] { '0' });
                            string RandskgF = Randskg.Insert(Randskg.Length - 2, ".");
                            string mass = v.Substring(16, 8).TrimStart(new Char[] { '0' });
                            string line = "----------------------------------------";

                            returnData = Encoding.Default.GetString(receiveBytes);

                            string desc = returnData.Substring(returnData.Length - 46, 30);



                            string filtered = "Item : " + desc + "\n" + "Total Weight : " + mass + "g" + "\n" + "R/Kg             : " + RandskgF + "\n" + "Tare Value     : " + tareF + "\n" + "Price              : R" + priceF + "\n" + line + "\n";
                            if (MAX_CHs >= 10)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 10" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                    //-----------------------------------------------------------------------------------------

                    //Scale 11

                    if (isidalog.Contains(ishidaipk))
                    {

                        try
                        {
                            Int32 scaleSPn = 42011;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                            string hexdata = GetHexStringFrom(receiveBytes);
                            string lasdata = hexdata.Substring(hexdata.Length - 47);
                            string v = lasdata.Replace("-", "");

                            string priceh = v.Substring(24, 8).TrimStart(new Char[] { '0' });
                            string priceF = priceh.Insert(priceh.Length - 2, ".");
                            string tare = v.Substring(4, 4);
                            string tareF = tare.Insert(tare.Length - 3, ",");
                            string Randskg = v.Substring(8, 8).TrimStart(new Char[] { '0' });
                            string RandskgF = Randskg.Insert(Randskg.Length - 2, ".");
                            string mass = v.Substring(16, 8).TrimStart(new Char[] { '0' });
                            string line = "----------------------------------------";

                            returnData = Encoding.Default.GetString(receiveBytes);

                            string desc = returnData.Substring(returnData.Length - 46, 30);



                            string filtered = "Item : " + desc + "\n" + "Total Weight : " + mass + "g" + "\n" + "R/Kg             : " + RandskgF + "\n" + "Tare Value     : " + tareF + "\n" + "Price              : R" + priceF + "\n" + line + "\n";
                            if (MAX_CHs >= 11)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 11" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                    //-----------------------------------------------------------------------------------------
                    //Scale 12

                    if (isidalog.Contains(ishidaipl))
                    {

                        try
                        {
                            Int32 scaleSPn = 42012;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                            string hexdata = GetHexStringFrom(receiveBytes);
                            string lasdata = hexdata.Substring(hexdata.Length - 47);
                            string v = lasdata.Replace("-", "");

                            string priceh = v.Substring(24, 8).TrimStart(new Char[] { '0' });
                            string priceF = priceh.Insert(priceh.Length - 2, ".");
                            string tare = v.Substring(4, 4);
                            string tareF = tare.Insert(tare.Length - 3, ",");
                            string Randskg = v.Substring(8, 8).TrimStart(new Char[] { '0' });
                            string RandskgF = Randskg.Insert(Randskg.Length - 2, ".");
                            string mass = v.Substring(16, 8).TrimStart(new Char[] { '0' });
                            string line = "----------------------------------------";

                            returnData = Encoding.Default.GetString(receiveBytes);

                            string desc = returnData.Substring(returnData.Length - 46, 30);



                            string filtered = "Item : " + desc + "\n" + "Total Weight : " + mass + "g" + "\n" + "R/Kg             : " + RandskgF + "\n" + "Tare Value     : " + tareF + "\n" + "Price              : R" + priceF + "\n" + line + "\n";
                            if (MAX_CHs >= 12)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 12" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                    //-----------------------------------------------------------------------------------------
                    //Scale 13

                    if (isidalog.Contains(ishidaipm))
                    {

                        try
                        {
                            Int32 scaleSPn = 42013;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                            string hexdata = GetHexStringFrom(receiveBytes);
                            string lasdata = hexdata.Substring(hexdata.Length - 47);
                            string v = lasdata.Replace("-", "");

                            string priceh = v.Substring(24, 8).TrimStart(new Char[] { '0' });
                            string priceF = priceh.Insert(priceh.Length - 2, ".");
                            string tare = v.Substring(4, 4);
                            string tareF = tare.Insert(tare.Length - 3, ",");
                            string Randskg = v.Substring(8, 8).TrimStart(new Char[] { '0' });
                            string RandskgF = Randskg.Insert(Randskg.Length - 2, ".");
                            string mass = v.Substring(16, 8).TrimStart(new Char[] { '0' });
                            string line = "----------------------------------------";

                            returnData = Encoding.Default.GetString(receiveBytes);

                            string desc = returnData.Substring(returnData.Length - 46, 30);



                            string filtered = "Item : " + desc + "\n" + "Total Weight : " + mass + "g" + "\n" + "R/Kg             : " + RandskgF + "\n" + "Tare Value     : " + tareF + "\n" + "Price              : R" + priceF + "\n" + line + "\n";
                            if (MAX_CHs >= 13)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 13" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                    //-----------------------------------------------------------------------------------------
                    //Scale 14

                    if (isidalog.Contains(ishidaipn))
                    {

                        try
                        {
                            Int32 scaleSPn = 42014;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                            string hexdata = GetHexStringFrom(receiveBytes);
                            string lasdata = hexdata.Substring(hexdata.Length - 47);
                            string v = lasdata.Replace("-", "");

                            string priceh = v.Substring(24, 8).TrimStart(new Char[] { '0' });
                            string priceF = priceh.Insert(priceh.Length - 2, ".");
                            string tare = v.Substring(4, 4);
                            string tareF = tare.Insert(tare.Length - 3, ",");
                            string Randskg = v.Substring(8, 8).TrimStart(new Char[] { '0' });
                            string RandskgF = Randskg.Insert(Randskg.Length - 2, ".");
                            string mass = v.Substring(16, 8).TrimStart(new Char[] { '0' });
                            string line = "----------------------------------------";

                            returnData = Encoding.Default.GetString(receiveBytes);

                            string desc = returnData.Substring(returnData.Length - 46, 30);



                            string filtered = "Item : " + desc + "\n" + "Total Weight : " + mass + "g" + "\n" + "R/Kg             : " + RandskgF + "\n" + "Tare Value     : " + tareF + "\n" + "Price              : R" + priceF + "\n" + line + "\n";
                            if (MAX_CHs >= 14)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 14" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                    //-----------------------------------------------------------------------------------------
                    //Scale 15

                    if (isidalog.Contains(ishidaipo))
                    {

                        try
                        {
                            Int32 scaleSPn = 42015;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                            string hexdata = GetHexStringFrom(receiveBytes);
                            string lasdata = hexdata.Substring(hexdata.Length - 47);
                            string v = lasdata.Replace("-", "");

                            string priceh = v.Substring(24, 8).TrimStart(new Char[] { '0' });
                            string priceF = priceh.Insert(priceh.Length - 2, ".");
                            string tare = v.Substring(4, 4);
                            string tareF = tare.Insert(tare.Length - 3, ",");
                            string Randskg = v.Substring(8, 8).TrimStart(new Char[] { '0' });
                            string RandskgF = Randskg.Insert(Randskg.Length - 2, ".");
                            string mass = v.Substring(16, 8).TrimStart(new Char[] { '0' });
                            string line = "----------------------------------------";

                            returnData = Encoding.Default.GetString(receiveBytes);

                            string desc = returnData.Substring(returnData.Length - 46, 30);



                            string filtered = "Item : " + desc + "\n" + "Total Weight : " + mass + "g" + "\n" + "R/Kg             : " + RandskgF + "\n" + "Tare Value     : " + tareF + "\n" + "Price              : R" + priceF + "\n" + line + "\n";
                            if (MAX_CHs >= 15)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 15" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                    //-----------------------------------------------------------------------------------------
                    //Scale 16

                    if (isidalog.Contains(ishidaipp))
                    {

                        try
                        {
                            Int32 scaleSPn = 42016;

                            string CCTVServer = "127.0.0.1";

                            richTextBox2.Invoke(new Action(() => richTextBox2.Clear()));

                            string hexdata = GetHexStringFrom(receiveBytes);
                            string lasdata = hexdata.Substring(hexdata.Length - 47);
                            string v = lasdata.Replace("-", "");

                            string priceh = v.Substring(24, 8).TrimStart(new Char[] { '0' });
                            string priceF = priceh.Insert(priceh.Length - 2, ".");
                            string tare = v.Substring(4, 4);
                            string tareF = tare.Insert(tare.Length - 3, ",");
                            string Randskg = v.Substring(8, 8).TrimStart(new Char[] { '0' });
                            string RandskgF = Randskg.Insert(Randskg.Length - 2, ".");
                            string mass = v.Substring(16, 8).TrimStart(new Char[] { '0' });
                            string line = "----------------------------------------";

                            returnData = Encoding.Default.GetString(receiveBytes);

                            string desc = returnData.Substring(returnData.Length - 46, 30);



                            string filtered = "Item : " + desc + "\n" + "Total Weight : " + mass + "g" + "\n" + "R/Kg             : " + RandskgF + "\n" + "Tare Value     : " + tareF + "\n" + "Price              : R" + priceF + "\n" + line + "\n";
                            if (MAX_CHs >= 16)
                            {
                                richTextBox2.Invoke(new Action(() => richTextBox2.Text = "Scale 16" + "\n" + filtered));


                                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                                IPAddress send_to_address = IPAddress.Parse(CCTVServer);
                                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, scaleSPn);
                                byte[] send_bufferw = Encoding.ASCII.GetBytes(filtered);

                                sending_socket.SendTo(send_bufferw, sending_end_point);
                                sending_socket.Dispose();
                            }
                        }
                        catch
                        {
                        }

                    }
                    //-----------------------------------------------------------------------------------------


                }


            }



        }

        public static string GetHexStringFrom(byte[] byteArray)
        {
            return BitConverter.ToString(byteArray); //To convert the whole array
        }

        static string MinifyA(string p)
        {

            p = p.Replace("D", "a");
            p = p.Replace("M", "b");
            p = p.Replace("Y", "c");
            p = p.Replace("O", "d");
            p = p.Replace("S", "e");
            p = p.Replace("I", "f");
            p = p.Replace("X", "0");
            p = p.Replace("B", "1");
            p = p.Replace("R", "2");
            p = p.Replace("T", "3");
            p = p.Replace("W", "4");
            p = p.Replace("U", "5");
            p = p.Replace("N", "6");
            p = p.Replace("P", "7");
            p = p.Replace("Z", "8");
            p = p.Replace("G", "9");
            p = p.Replace("H", "10");




            return p;

        }



        private void button1_Click(object sender, EventArgs e)
        {

     

        }


        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                    
                }
            }
            return sMacAddress;
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {



             
            var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var s = Path.Combine(appDataDir, "ishida.ini");
            string[] lines = { textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox9.Text, textBox10.Text, textBox11.Text , textBox12.Text, textBox13.Text, textBox14.Text, textBox15.Text, textBox16.Text };
            System.IO.File.WriteAllLines(s, lines);


         
        }

        private void TrayMinimizerForm_Resize(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "Ultech PNP Translator Minimized";
            notifyIcon1.BalloonTipText = "Dubble Click on icon to show";

            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void maxi(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            DialogResult dr = MessageBox.Show("Are you sure you want to exit application?" + "\n" + "Please Enter Password in logout!",

                                               "Exit", MessageBoxButtons.YesNo);

            if (dr == DialogResult.Yes)
            {
                if (maskedTextBox1.Text == "logmeout")
                {
                    // closeopenlinks();
                }
                else
                {
                    e.Cancel = true;

                }
            }
            if (dr == DialogResult.No)
            {

                e.Cancel = true;

            }
        }


    }
}
