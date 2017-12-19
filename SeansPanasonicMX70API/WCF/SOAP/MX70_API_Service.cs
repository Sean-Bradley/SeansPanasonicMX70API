//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.ServiceModel;
//using System.ServiceModel.Channels;
//using System.ServiceModel.Dispatcher;
//using System.ServiceModel.Description;
//using System.Net.Sockets;
//using System.Net;
//using System.Xml;
//using System.Xml.Serialization;
//using System.IO;
//using System.ServiceModel.Web;
//using System.Threading;

//namespace MX70_API.WCF.SOAP
//{
//    [ServiceContract(Namespace = "MX70_API")]
//    public interface IMX70_API_Service
//    {
//        [OperationContract]
//        String Ping();

//        [OperationContract]
//        Boolean SendCommand(String Command);

//        [OperationContract]
//        Boolean SendCommands(String[] Commands);

//        [OperationContract]
//        Boolean SwitchCamera(Int32 Source, Int32 Speed);

//        [OperationContract]
//        Boolean AutoTakeToRow(Row Row, Int32 Source, Int32 Speed);
//    }

//    /// <summary>
//    /// TBA
//    /// </summary>
//    [ServiceBehavior(Namespace = "MX70_API")]
//    public class MX70_API_Service : IMX70_API_Service
//    {
//        public String Ping()
//        {
//            RemoteEndpointMessageProperty clientEndpoint = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
//            Console.WriteLine(DateTime.Now + " : " + clientEndpoint.Address + " -> called Ping");

//            return DateTime.Now.ToUniversalTime().ToString();
//        }

//        public Boolean SendCommand(String Command)
//        {
//            RemoteEndpointMessageProperty clientEndpoint = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
//            Console.WriteLine(DateTime.Now + " : " + clientEndpoint.Address + " -> called SendCommand : " + Command);

//            Boolean ret = false;
//            try
//            {
//                ret = Program.sendSerialData(Command);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                Logging.Log.LogIt(MX70_API.Logging.LogType.ApplicationError, "SendCommand : " + ex.Message);
//            }

//            return ret;
//        }

//        public Boolean SendCommands(String[] Commands)
//        {
//            RemoteEndpointMessageProperty clientEndpoint = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
//            Console.WriteLine(DateTime.Now + " : " + clientEndpoint.Address + " -> called SendCommands : " + Commands.Length);

//            try
//            {
//                foreach (String Command in Commands)
//                {
//                    Program.sendSerialData(Command);
//                    Thread.Sleep(100);
//                }
//                return true;
//            }
//            catch (Exception ex)
//            {
//                Logging.Log.LogIt(MX70_API.Logging.LogType.ApplicationError, "SendCommands : " + ex.Message);
//                return false;
//            }
//        }

//        public Boolean SwitchCamera(Int32 Source, Int32 Speed)
//        {
//            RemoteEndpointMessageProperty clientEndpoint = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
//            Console.WriteLine(DateTime.Now + " : " + clientEndpoint.Address + " -> called SwitchCamera : " + Source + " " + Speed);

//            Boolean ret = false;
//            try
//            {
//                if (Program.Row == Row.A)//switch to row B
//                {
//                    Program.Row = Row.B;
//                    if (Program.sendSerialData("VCP:B" + Source))
//                    {
//                        Thread.Sleep(50);
//                        Program.FadeToB(Speed);
//                        ret = true;
//                    }
//                }
//                else
//                {
//                    Program.Row = Row.A;
//                    if (Program.sendSerialData("VCP:A" + Source))
//                    {
//                        Thread.Sleep(50);
//                        Program.FadeToA(Speed);
//                        ret = true;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Logging.Log.LogIt(MX70_API.Logging.LogType.ApplicationError, "SwitchCamera : " + ex.Message);
//            }

//            return ret;
//        }

//        public Boolean AutoTakeToRow(Row Row, Int32 Source, Int32 Speed)
//        {
//            RemoteEndpointMessageProperty clientEndpoint = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
//            Console.WriteLine(DateTime.Now + " : " + clientEndpoint.Address + " -> called AutoTakeToRow : " + Row + " " + Source + " " + Speed);

//            Boolean ret = false;

//            try  //0,1
//            {
//                //if (Program.Row != Row)//only do it if we are not already in the row
//                //{
//                if (Row == Row.B)//switch to row B
//                {
//                    Program.Row = Row.B;
//                    if (Program.sendSerialData("VCP:B" + Source))
//                    {
//                        Thread.Sleep(50);
//                        Program.FadeToB(Speed);
//                        ret = true;
//                    }
//                }
//                else
//                {
//                    Program.Row = Row.A;
//                    if (Program.sendSerialData("VCP:A" + Source))
//                    {
//                        Thread.Sleep(50);
//                        Program.FadeToA(Speed);
//                        ret = true;
//                    }
//                }
//                //}
//            }
//            catch (Exception ex)
//            {
//                Logging.Log.LogIt(MX70_API.Logging.LogType.ApplicationError, "SwitchCamera : " + ex.Message);
//            }

//            return ret;
//        }
//    }
//}


