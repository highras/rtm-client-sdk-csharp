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

        static RTMClient LoginRTM(string rtmEndpoint, long projectId, long uid, string token)
        {
            RTMClient client = new RTMClient(rtmEndpoint, projectId, uid, new example.common.RTMExampleQuestProcessor());

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
            int errorCode = client.GetAttributes(out Dictionary<string, string> attributes);
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Console.WriteLine("Get attributes in sync failed. error code {0}", errorCode);
                return;
            }

            Console.WriteLine("Attributes has {0} items.", attributes.Count);

            foreach (KeyValuePair<string, string> kvp in attributes)
                Console.WriteLine("Key {0}, value {1}", kvp.Key, kvp.Value);
        }

        static void AddDevicePushOption(RTMClient client, MessageCategory messageCategory, long targetId, HashSet<byte> mTypes = null)
        {
            Console.WriteLine($"===== [ AddDevicePushOption ] =======");

            int errorCode = client.AddDevicePushOption(messageCategory, targetId, mTypes);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Add device push option in sync failed.");
            else
                Console.WriteLine("Add device push option in sync success.");
        }

        static void RemoveDevicePushOption(RTMClient client, MessageCategory messageCategory, long targetId, HashSet<byte> mTypes = null)
        {
            Console.WriteLine($"===== [ RemoveDevicePushOption ] =======");

            int errorCode = client.RemoveDevicePushOption(messageCategory, targetId, mTypes);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Remove device push option in sync failed.");
            else
                Console.WriteLine("Remove device push option in sync success.");
        }

        static void PrintDevicePushOption(string categroy, Dictionary<long, HashSet<byte>> optionDictionary)
        {
            Console.WriteLine($"===== {categroy} has {optionDictionary.Count} items. =======");
            foreach (KeyValuePair<long, HashSet<byte>> kvp in optionDictionary)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("ID: ").Append(kvp.Key).Append(", count: ").Append(kvp.Value.Count);
                if (kvp.Value.Count > 0)
                {
                    sb.Append(": {");
                    foreach (byte mType in kvp.Value)
                        sb.Append($" {mType},");

                    sb.Append("}");
                }

                Console.WriteLine(sb);
            }
        }

        static void GetDevicePushOption(RTMClient client)
        {
            Console.WriteLine($"===== [ GetDevicePushOption ] =======");

            int errorCode = client.GetDevicePushOption(
                out Dictionary<long, HashSet<byte>> p2pDictionary, out Dictionary<long, HashSet<byte>> groupDictionary);
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Console.WriteLine($"Get device push option in sync failed. error code {errorCode}");
                return;
            }

            PrintDevicePushOption("P2P", p2pDictionary);
            PrintDevicePushOption("Group", groupDictionary);
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
            long projectId = Int64.Parse(args[1]);
            long uid = Int64.Parse(args[2]);
            string token = args[3];

            RTMClient client = LoginRTM(rtmEndpoint, projectId, uid, token);

            if (client == null)
                return;

            AddAttributesDemo(client);
            GetAttributesDemo(client);

            GetDevicePushOption(client);
            AddDevicePushOption(client, MessageCategory.P2PMessage, 12345);
            AddDevicePushOption(client, MessageCategory.GroupMessage, 223344, new HashSet<byte>());

            AddDevicePushOption(client, MessageCategory.P2PMessage, 34567, null);
            AddDevicePushOption(client, MessageCategory.GroupMessage, 445566, new HashSet<byte>() { 23, 35, 56,67 ,78, 89 });

            GetDevicePushOption(client);

            RemoveDevicePushOption(client, MessageCategory.GroupMessage, 223344, new HashSet<byte>() { 23, 35, 56, 67, 78, 89 });
            RemoveDevicePushOption(client, MessageCategory.GroupMessage, 445566, new HashSet<byte>());
            
            GetDevicePushOption(client);

            RemoveDevicePushOption(client, MessageCategory.P2PMessage, 12345);
            RemoveDevicePushOption(client, MessageCategory.P2PMessage, 34567);
            RemoveDevicePushOption(client, MessageCategory.GroupMessage, 223344);
            RemoveDevicePushOption(client, MessageCategory.GroupMessage, 445566, new HashSet<byte>() { 23, 35, 56, 67, 78, 89 });

            GetDevicePushOption(client);

            client.Bye();
            Thread.Sleep(1000);
        }
    }
}
