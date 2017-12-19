using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Security.Cryptography.X509Certificates;
using System.Timers;
using System.Configuration;
using System.IO.Ports;
using System.ServiceModel.Web;

namespace SeansPanasonicMX70API
{
    class Program
    {
        public static SerialPort SerialPort;

        public static String ComPort = ConfigurationSettings.AppSettings["ComPort"].ToString();
        public static Int32 BaudRate = 38400; //Convert.ToInt32(ConfigurationSettings.AppSettings["BaudRate"].ToString());

        public static WebServiceHost WebServiceHost;
        public static String APIURI = ConfigurationSettings.AppSettings["APIURI"].ToString();

        public static Int32 Source = 0;
        public static Row Row = Row.A;

        static void Main(string[] args)
        {
            Console.WriteLine("SeansPanasonicMX70API");
            Console.WriteLine("Copyright (C) 2014 Sean Bradley (seanwasere.com)");
            Console.WriteLine();
            Console.WriteLine("Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:");
            Console.WriteLine();
            Console.WriteLine("The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.");
            Console.WriteLine();
            Console.WriteLine("THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.");
            Console.WriteLine();
            Console.WriteLine();

            try
            {
                Console.WriteLine("Trying to connecting to " + ComPort);
                SerialPort = new SerialPort(ComPort, BaudRate, Parity.Odd, 8, StopBits.One);
                SerialPort.Handshake = Handshake.XOnXOff;
                SerialPort.WriteTimeout = 500;
                SerialPort.DataReceived += new SerialDataReceivedEventHandler(serialPortDataReceived);
                SerialPort.Open();
                Console.WriteLine(ComPort + " opened. BaudRate=" + BaudRate);
                try
                {
                    Uri baseAddress = new Uri(APIURI);

                    WebServiceHost = new WebServiceHost(typeof(WCF.WEBGETAPI), baseAddress);
                    WebServiceHost.Open();

                    Console.WriteLine("");
                    Console.WriteLine("The web service host is running and listening at '" + baseAddress + "'");
                    Console.WriteLine("");
                    Console.WriteLine("API functions are,");
                    Console.WriteLine("\tAutotake");
                    Console.WriteLine("\tSetRowSource");
                    Console.WriteLine("\tFadeToRow");
                    Console.WriteLine("");
                    Console.WriteLine("Examples,");
                    Console.WriteLine("");
                    Console.WriteLine("Autotake requires parameter 'speed' which can equal 1 to 100. 10 recommended.");
                    Console.WriteLine("\t" + baseAddress + "/do?command=Autotake&speed=1");
                    Console.WriteLine("\t" + baseAddress + "/do?command=Autotake&speed=10");
                    Console.WriteLine("\t" + baseAddress + "/do?command=Autotake&speed=100");

                    Console.WriteLine("");
                    Console.WriteLine("SetRowSource requires parameter 'row' which equals 'A' or 'B', and 'source' which equals 1 to 8");
                    Console.WriteLine("\t" + baseAddress + "/do?command=SetRowSource&Row=A&Source=1");
                    Console.WriteLine("\t" + baseAddress + "/do?command=SetRowSource&Row=B&Source=2");
                    Console.WriteLine("\t" + baseAddress + "/do?command=SetRowSource&Row=A&Source=3");
                    Console.WriteLine("\t" + baseAddress + "/do?command=SetRowSource&Row=B&Source=4");

                    Console.WriteLine("");
                    Console.WriteLine("FadeToRow requires parameter 'row' which equals 'A' or 'B', and 'speed' which equals 1 to 100. 10 recommended.");
                    Console.WriteLine("\t" + baseAddress + "/do?command=FadeToRow&row=A&Speed=1");
                    Console.WriteLine("\t" + baseAddress + "/do?command=FadeToRow&row=B&Speed=1");
                    Console.WriteLine("\t" + baseAddress + "/do?command=FadeToRow&row=A&Speed=10");
                    Console.WriteLine("\t" + baseAddress + "/do?command=FadeToRow&row=B&Speed=10");
                    Console.WriteLine("\t" + baseAddress + "/do?command=FadeToRow&row=A&Speed=100");
                    Console.WriteLine("\t" + baseAddress + "/do?command=FadeToRow&row=B&Speed=100");

                    Console.WriteLine("");
                    Console.WriteLine("seanwasere youtube");
                    Thread.Sleep(System.Threading.Timeout.Infinite);

                }
                catch (Exception ex2)
                {
                    Console.WriteLine("Not able to create web service host. Ensure you run this program as an administrator and that the APIURI in the config is valid: " + ex2.Message);
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MX70 not connected : " + ex.Message);
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }


        }

        static void serialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = SerialPort.ReadLine();

                int check = SerialPort.ReadByte();
                if (check == 6)  //ack
                {
                    //Console.WriteLine();
                    //Console.WriteLine(DateTime.Now + " : " + SerialPort.ReadExisting() + data);
                    Console.WriteLine(DateTime.Now + " : serialPortDataReceived : ack : " + data);
                }
                else if (check == 21) //nak
                {
                    String ErrorString = SerialPort.ReadExisting() + data;
                    //Console.WriteLine();
                    //Console.WriteLine("Error : " + ErrorString);
                    Console.WriteLine(DateTime.Now + " : serialPortDataReceived : nak : " + ErrorString);
                    //Logging.Log.LogIt(MX70_API.Logging.LogType.ApplicationError, ErrorString);
                }
                Console.WriteLine(data);
            }
            catch (Exception ex)
            {
                //Logging.Log.LogIt(MX70_API.Logging.LogType.ApplicationError, ex.Message);
                Console.WriteLine(DateTime.Now + " : serialPortDataReceived : " + ex.Message);
            };
        }

        public static Boolean sendSerialData(String Data)
        {
            Boolean ret = false;
            try
            {
                if (SerialPort.IsOpen)
                {
                    lock (SerialPort)
                    {
                        byte[] command = new byte[Data.Length + 2];
                        command[0] = (byte)0x02;
                        for (Int32 i = 0; i < Data.Length; i++)
                        {
                            command[i + 1] = (byte)Data[i];
                        }
                        command[Data.Length + 1] = (byte)0x03;

                        SerialPort.Write(command, 0, command.Length);

                        //Console.WriteLine(DateTime.Now.ToString() + "-> " + Encoding.ASCII.GetString(command));

                        ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now + " : sendSerialData : " + ex.Message);
                //Logging.Log.LogIt(MX70_API.Logging.LogType.ApplicationError, ex.Message + Environment.NewLine + "Trying to send : " + Data);
            }
            return ret;
        }

        public static Boolean SetRowSource(String Row, Int32 Source)
        {
            Boolean ret = false;
            try
            {
                Console.WriteLine(DateTime.Now + " : SetRowSource : Row=" + Row + " Source=" + Source);
                   
                Program.sendSerialData("VCP:" + Row.ToUpper() + Source);
                ret = true;

            }
            catch (Exception ex)
            {
                //Logging.Log.LogIt(MX70_API.Logging.LogType.ApplicationError, "SwitchCamera : " + ex.Message);
                Console.WriteLine(DateTime.Now + " : SwitchCamera : " + ex.Message);
            }

            return ret;
        }


        //public static Boolean FadeToA(Int32 SleepTime)
        //{
        //    Boolean ret = false;
        //    try
        //    {
        //        if (SerialPort.IsOpen)
        //        {
        //            Program.sendSerialData("VMM:FF");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:EE");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:DD");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:CC");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:BB");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:AA");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:99");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:88");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:77");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:66");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:55");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:44");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:33");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:22");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:11");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:00");
        //            Thread.Sleep(SleepTime);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Logging.Log.LogIt(MX70_API.Logging.LogType.ApplicationError, "SwitchCamera : " + ex.Message);
        //        Console.WriteLine(DateTime.Now + " : FadeToA : " + ex.Message);
        //    }

        //    return ret;
        //}
        //public static Boolean FadeToB(Int32 SleepTime)
        //{
        //    Boolean ret = false;
        //    try
        //    {
        //        if (SerialPort.IsOpen)
        //        {
        //            Program.sendSerialData("VMM:00");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:11");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:22");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:33");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:44");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:55");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:66");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:77");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:88");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:99");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:AA");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:BB");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:CC");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:DD");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:EE");
        //            Thread.Sleep(SleepTime);
        //            Program.sendSerialData("VMM:FF");
        //            Thread.Sleep(SleepTime);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Logging.Log.LogIt(MX70_API.Logging.LogType.ApplicationError, "SwitchCamera : " + ex.Message);
        //        Console.WriteLine(DateTime.Now + " : FadeToB : " + ex.Message);
        //    }

        //    return ret;
        //}

        internal static Boolean FadeToRow(String Row, Int32 SleepTime)
        {
            Boolean ret = false;
            try
            {
                if (SerialPort.IsOpen)
                {
                    Console.WriteLine(DateTime.Now + " : FadeToRow : Row=" + Row + " Speed=" + SleepTime.ToString("000"));
                    if (Row.ToUpper() == "A")
                    {
                        Program.sendSerialData("VMM:FF");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:EE");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:DD");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:CC");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:BB");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:AA");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:99");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:88");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:77");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:66");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:55");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:44");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:33");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:22");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:11");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:00");
                        Thread.Sleep(SleepTime);

                    }
                    else if (Row.ToUpper() == "B")
                    {
                        Program.sendSerialData("VMM:00");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:11");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:22");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:33");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:44");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:55");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:66");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:77");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:88");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:99");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:AA");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:BB");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:CC");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:DD");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:EE");
                        Thread.Sleep(SleepTime);
                        Program.sendSerialData("VMM:FF");
                        Thread.Sleep(SleepTime);
                    }
                }
            }
            catch (Exception ex)
            {
                //Logging.Log.LogIt(MX70_API.Logging.LogType.ApplicationError, "SwitchCamera : " + ex.Message);
                Console.WriteLine(DateTime.Now + " : FadeToRow : " + ex.Message);
            }

            return ret;
        }

        internal static bool Autotake(Int32 Speed)
        {
            Boolean ret = false;
            try
            {
                if (SerialPort.IsOpen)
                {
                    Console.WriteLine(DateTime.Now + " : Autotake : " + Speed.ToString("000"));
                    Program.sendSerialData("VMA:" + Speed.ToString("000"));
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                //Logging.Log.LogIt(MX70_API.Logging.LogType.ApplicationError, "SwitchCamera : " + ex.Message);
                Console.WriteLine(DateTime.Now + " : Autotake : " + ex.Message);

            }

            return ret;
        }
    }
}



////☻AFD:A1111♥           //audio level  A/B 0000 - FFFF
////☻VAS:001♥
////☻VAS:A0001♥
////☻VAS:N01111111♥
////☻VCC:A0001♥
////☻VCG:A0001♥
////☻VCY:A0001♥
////☻VCP:A2♥              //video souce A/B 1-8  
////☻VDA:N0111111♥
////☻VDK:001♥             //toggles dsk
////☻VDK:A0001♥
////☻VDK:N0111111♥
////☻VDL:001♥             //DSK set K Level 0-255  VDL:00-VDK:FF
////☻VDL:A0001♥
////☻VDS:001♥             //DSK Slice and Slope VDS:000 - VDS:FFF  1st 2 figits = Slice, 3rd gigit = slope. All 000 = full brightness 
////☻VDS:A0001♥
////☻VDZ:000♥
////☻VEB:100♥
////☻VFA:001♥
////☻VFM:001♥
////☻VFM:A0001♥
////☻VFN:100♥
////☻VKC:011111111111111111111♥
////☻VKL:001♥
////☻VKL:A0001♥           //☻VKL:011111111111111111111♥
////☻VKS:A0001♥           //☻VKS:011111111111♥
////☻VKO:000♥
////☻VKR:011111111111111111111♥
////☻VKS:001♥
////☻VKT:100♥
////☻VMA:001♥ //autotake  //☻VMA:011111111111♥
////☻VMM:001♥ //lever     //☻VMM:011111111111♥
////☻VMM:A0001♥
////☻VMP:011111111111♥
////☻VMW:011111111111♥
////☻VSD:A0001♥           //☻VSD:011111111111♥
////☻VWN:011111111111♥    //changes pattern, pattern id must exist otherwise it goes to nmext highest

