using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace SeansPanasonicMX70API.WCF
{
    [ServiceContract]
    public interface IWEBGETAPI
    {
        [OperationContract]
        [WebGet]
        Boolean Do(String Command, String Row, String Source, String Speed);
    }

    /// <summary>
    /// TBA
    /// </summary>
    [ServiceBehavior]
    public class WEBGETAPI : IWEBGETAPI
    {
        public Boolean Do(String Command, String Row, String Source, String Speed)
        {
            Boolean success = false;
            try
            {
                switch (Command.ToLower())
                {
                    case "setrowsource":
                        success = Program.SetRowSource(Row, Convert.ToInt32(Source));//, Convert.ToInt32(Speed));
                        break;
                    case "fadetorow":
                        success = Program.FadeToRow(Row, Convert.ToInt32(Speed));//, Convert.ToInt32(Speed));
                        break;                   
                    case "autotake":
                        success = Program.Autotake(Convert.ToInt32(Speed));
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("go : " + ex.Message);
                success = false;

            }
            return success; // "C:" + Channel.ToString() + " N:" + Note.ToString() + " V:" + Velocity.ToString();
        }


    }
}


//http://127.0.0.1:8080/SeansPanasonicMX70API/do?command=SetRowSource&Row=A&Source=1
//http://127.0.0.1:8080/SeansPanasonicMX70API/do?command=SetRowSource&Row=B&Source=1
//http://127.0.0.1:8080/SeansPanasonicMX70API/do?command=SetRowSource&Row=A&Source=2
//http://127.0.0.1:8080/SeansPanasonicMX70API/do?command=SetRowSource&Row=B&Source=2
//http://127.0.0.1:8080/SeansPanasonicMX70API/do?command=SetRowSource&Row=A&Source=3
//http://127.0.0.1:8080/SeansPanasonicMX70API/do?command=SetRowSource&Row=B&Source=3
//http://127.0.0.1:8080/SeansPanasonicMX70API/do?command=SetRowSource&Row=A&Source=4
//http://127.0.0.1:8080/SeansPanasonicMX70API/do?command=SetRowSource&Row=B&Source=4
//http://127.0.0.1:8080/SeansPanasonicMX70API/do?command=SetRowSource&Row=A&Source=5
//http://127.0.0.1:8080/SeansPanasonicMX70API/do?command=SetRowSource&Row=B&Source=5

//http://127.0.0.1:8080/SeansPanasonicMX70API/do?command=FadeToRow&row=A&Speed=1
//http://127.0.0.1:8080/SeansPanasonicMX70API/do?command=FadeToRow&row=A&Speed=10
//http://127.0.0.1:8080/SeansPanasonicMX70API/do?command=FadeToRow&row=A&Speed=50

//http://127.0.0.1:8080/SeansPanasonicMX70API/do?command=FadeToRow&row=B&Speed=1
//http://127.0.0.1:8080/SeansPanasonicMX70API/do?command=FadeToRow&row=B&Speed=10
//http://127.0.0.1:8080/SeansPanasonicMX70API/do?command=FadeToRow&row=B&Speed=50

//http://127.0.0.1:8080/SeansPanasonicMX70API/do?command=Autotake&speed=1
//http://127.0.0.1:8080/SeansPanasonicMX70API/do?command=Autotake&speed=10
//http://127.0.0.1:8080/SeansPanasonicMX70API/do?command=Autotake&speed=100