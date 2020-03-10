using System;
using com.fpnn.rtm;

namespace Data
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: RTMData <rtm-endpoint> <projectId> <uid> <token>");
                return;
            }

            //-- Testing only
            // ManualInitForTesting();

            string rtmEndpoint = args[0];
            long pid = Int64.Parse(args[1]);
            long uid = Int64.Parse(args[2]);
            string token = args[3];

            RTMClient client = LoginRTM(rtmEndpoint, pid, uid, token);

            if (client == null)
                return;

            Console.WriteLine("=========== Begin set user data ===========");

            SetData(client, "key 1", "value 1");
            SetData(client, "key 2", "value 2");

            Console.WriteLine("=========== Begin get user data ===========");

            GetData(client, "key 1");
            GetData(client, "key 2");

            Console.WriteLine("=========== Begin delete one of user data ===========");

            DeleteData(client, "key 2");

            Console.WriteLine("=========== Begin get user data after delete action ===========");

            GetData(client, "key 1");
            GetData(client, "key 2");

            Console.WriteLine("=========== User logout ===========");

            client.Bye();

            Console.WriteLine("=========== User relogin ===========");

            client = LoginRTM(rtmEndpoint, pid, uid, token);

            if (client == null)
                return;

            Console.WriteLine("=========== Begin get user data after relogin ===========");

            GetData(client, "key 1");
            GetData(client, "key 2");
        }

        static void ManualInitForTesting()
        {
            RTMConfig config = new RTMConfig()
            {
                defaultErrorRecorder = new example.common.ErrorRecorder()
            };
            RTMControlCenter.Init(config);
        }

        static RTMClient LoginRTM(string rtmEndpoint, long pid, long uid, string token)
        {
            RTMClient client = new RTMClient(rtmEndpoint, pid, uid, new example.common.RTMExampleQuestProcessor());

            int errorCode = client.Login(out bool ok, token);
            if (ok)
            {
                Console.WriteLine("RTM login success.");
                return client;
            }
            else
            {
                Console.WriteLine("RTM login failed, error code: {0}", errorCode);
                return null;
            }
        }

        static void SetData(RTMClient client, string key, string value)
        {
            int errorCode = client.DataSet(key, value);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Set user data with key {0} in sync failed.", key);
            else
                Console.WriteLine("Set user data with key {0} in sync success.", key);
        }

        static void GetData(RTMClient client, string key)
        {
            int errorCode = client.DataGet(out string value, key);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Get user data with key {0} in sync failed, error code is {1}.", key, errorCode);
            else
                Console.WriteLine("Get user data with key {0} in sync success, value is {1}", key, value ?? "null");
        }

        static void DeleteData(RTMClient client, string key)
        {
            int errorCode = client.DataDelete(key);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Delete user data with key {0} in sync failed.", key);
            else
                Console.WriteLine("Delete user data with key {0} in sync success.", key);
        }
    }
}
