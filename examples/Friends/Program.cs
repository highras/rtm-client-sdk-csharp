using System;
using System.Collections.Generic;
using com.fpnn.rtm;

namespace Friends
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: RTMFriends <rtm-endpoint> <projectId> <uid> <token>");
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

            AddFriends(client, new HashSet<long>() { 123456, 234567, 345678, 456789 });

            GetFriends(client);

            DeleteFriends(client, new HashSet<long>() { 234567, 345678 });

            System.Threading.Thread.Sleep(2000);   //-- Wait for server sync action.

            GetFriends(client);
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

        static void AddFriends(RTMClient client, HashSet<long> uids)
        {
            int errorCode = client.AddFriends(uids);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Add friends in sync failed, error code is {0}.", errorCode);
            else
                Console.WriteLine("Add friends in sync success");
        }

        static void DeleteFriends(RTMClient client, HashSet<long> uids)
        {
            int errorCode = client.DeleteFriends(uids);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Delete friends in sync failed, error code is {0}.", errorCode);
            else
                Console.WriteLine("Delete friends in sync success");
        }

        static void GetFriends(RTMClient client)
        {
            int errorCode = client.GetFriends(out HashSet<long> uids);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Get friends in sync failed, error code is {0}.", errorCode);
            else
            {
                Console.WriteLine("Get friends in sync success");
                foreach (long uid in uids)
                    Console.WriteLine("-- Friend uid: " + uid);
            }
        }
    }
}
