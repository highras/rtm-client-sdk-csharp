using System;
using System.Threading;
using com.fpnn.rtm;

namespace Messages
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: RTMLogin <rtm-endpoint> <projectId> <uid> <token>");
                return;
            }

            //-- Testing only
            // ManualInitForTesting();

            string rtmEndpoint = args[0];
            long pid = Int64.Parse(args[1]);
            long uid = Int64.Parse(args[2]);
            string token = args[3];

            SyncLoginDemo(rtmEndpoint, pid, uid, token);
            AsyncLoginDemo(rtmEndpoint, pid, uid, token);
        }

        static void ManualInitForTesting()
        {
            RTMConfig config = new RTMConfig()
            {
                defaultErrorRecorder = new example.common.ErrorRecorder()
            };
            RTMControlCenter.Init(config);
        }

        static void SyncLoginDemo(string rtmEndpoint, long pid, long uid, string token)
        {
            RTMClient client = new RTMClient(rtmEndpoint, pid, uid, new example.common.RTMExampleQuestProcessor());

            int errorCode = client.Login(out bool ok, token);
            Console.WriteLine("Login Result: OK: {0}, code: {1}", ok, errorCode);

            Thread.Sleep(2000);

            client.Close();
            Console.WriteLine("closed");
            Thread.Sleep(1500);
        }

        static void AsyncLoginDemo(string rtmEndpoint, long pid, long uid, string token)
        {
            RTMClient client = new RTMClient(rtmEndpoint, pid, uid, new example.common.RTMExampleQuestProcessor());
            bool status = client.Login((long pid, long uid, bool authStatus, int errorCode) => {
                Console.WriteLine("Async login {0}. pid {1}, uid {2}, code : {3}", authStatus, pid, uid, errorCode);
            }, token);
            if (!status)
            {
                Console.WriteLine("Async login starting failed.");
                return;
            }

            Console.WriteLine("Waiting 3 seconds for login, then, close the session.");
            Thread.Sleep(3000);

            client.Close();
            Console.WriteLine("closed");
            Thread.Sleep(1500);
        }
    }
}
