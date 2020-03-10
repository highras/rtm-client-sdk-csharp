using System;
using System.Collections.Generic;
using System.Threading;
using com.fpnn.rtm;

namespace Messages
{
    class Program
    {
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

            int errorCode = client.Login(out bool ok, token, new Dictionary<string, string>() {
                { "attr1", "demo 123" },
                { "attr2", " demo 234" },
            });
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

        static void AddAttributesDemo(RTMClient client)
        {
            int errorCode = client.AddAttributes(new Dictionary<string, string>() {
                { "key1", "value1" },
                { "key2", "value2" }
            });

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Add attributes in sync failed.");
            else
                Console.WriteLine("Add attributes in sync success.");
        }

        static void GetAttributesDemo(RTMClient client)
        {
            int errorCode = client.GetAttributes(out List<Dictionary<string, string>> attributes);
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Console.WriteLine("Get attributes in sync failed. error code {0}", errorCode);
                return;
            }

            Console.WriteLine("Attributes has {0} dictory.", attributes.Count);
            int dictCount = 0;

            foreach (Dictionary<string, string> dict in attributes)
            {
                dictCount += 1;

                Console.WriteLine("Dictory {0} has {1} items.", dictCount, dict.Count);

                foreach (KeyValuePair<string, string> kvp in dict)
                    Console.WriteLine("Key {0}, value {1}", kvp.Key, kvp.Value);

                Console.WriteLine("");
            }
        }

        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: RTMSystem <rtm-endpoint> <projectId> <uid> <token>");
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

            AddAttributesDemo(client);
            GetAttributesDemo(client);

            client.Bye();
            Thread.Sleep(1000);
        }
    }
}
