using System;
using System.Collections.Generic;
using com.fpnn.rtm;

namespace Users
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: RTMUsers <rtm-endpoint> <projectId> <uid> <token>");
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

            GetOnlineUsers(client, new HashSet<long>() { 99688848, 123456, 234567, 345678, 456789 });

            SetUserInfos(client, "This is public info", "This is private info");
            GetUserInfos(client);

            Console.WriteLine("======== =========");

            SetUserInfos(client, "", "This is private info");
            GetUserInfos(client);

            Console.WriteLine("======== =========");

            SetUserInfos(client, "This is public info", "");
            GetUserInfos(client);

            Console.WriteLine("======== only change the private infos =========");

            SetUserInfos(client, null, "balabala");
            GetUserInfos(client);

            SetUserInfos(client, "This is public info", "This is private info");
            client.Bye();

            Console.WriteLine("======== user relogin =========");

            client = LoginRTM(rtmEndpoint, projectId, uid, token);

            if (client == null)
                return;

            GetUserInfos(client);
        }

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

        static void GetOnlineUsers(RTMClient client, HashSet<long> willCheckedUids)
        {
            int errorCode = client.GetOnlineUsers(out HashSet<long> onlineUids, willCheckedUids);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Get online users in sync failed, error code is {0}.", errorCode);
            else
            {
                Console.WriteLine("Get online users in sync success");
                Console.WriteLine("Only {0} user(s) online in total {1} checked users", onlineUids.Count, willCheckedUids.Count);
                foreach (long uid in onlineUids)
                    Console.WriteLine("-- online uid: " + uid);
            }
        }

        static void SetUserInfos(RTMClient client, string publicInfos, string privateInfos)
        {
            int errorCode = client.SetUserInfo(publicInfos, privateInfos);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Set user infos in sync failed, error code is {0}.", errorCode);
            else
                Console.WriteLine("Set user infos in sync successed.");
        }

        static void GetUserInfos(RTMClient client)
        {
            int errorCode = client.GetUserInfo(out string publicInfos, out string privateInfos);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Get user infos in sync failed, error code is {0}.", errorCode);
            else
            {
                Console.WriteLine("Get user infos in sync successed.");
                Console.WriteLine("Public info: {0}", publicInfos ?? "null");
                Console.WriteLine("Private info: {0}", privateInfos ?? "null");
            }
        }
    }
}
