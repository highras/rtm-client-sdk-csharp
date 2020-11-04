using System;
using System.Collections.Generic;
using com.fpnn.rtm;

namespace Rooms
{
    class Program
    {
        private static long roomId = 556677;

        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: RTMRooms <rtm-endpoint> <projectId> <uid> <token>");
                return;
            }

            //-- Testing only
            ManualInitForTesting();

            string rtmEndpoint = args[0];
            long projectId = Int64.Parse(args[1]);
            long uid = Int64.Parse(args[2]);
            string token = args[3];

            RTMClient client = LoginRTM(rtmEndpoint, projectId, uid, token);

            if (client == null)
                return;

            Console.WriteLine("======== enter room =========");
            EnterRoom(client, roomId);

            Console.WriteLine("======== get self rooms =========");
            GetSelfRooms(client);

            Console.WriteLine("======== leave room =========");
            LeaveRoom(client, roomId);

            Console.WriteLine("======== get self rooms =========");
            GetSelfRooms(client);


            Console.WriteLine("======== enter room =========");
            EnterRoom(client, roomId);

            Console.WriteLine("======== set room infos =========");

            SetRoomInfos(client, roomId, "This is public info", "This is private info");
            GetRoomInfos(client, roomId);

            Console.WriteLine("======== change room infos =========");

            SetRoomInfos(client, roomId, "", "This is private info");
            GetRoomInfos(client, roomId);

            Console.WriteLine("======== change room infos =========");

            SetRoomInfos(client, roomId, "This is public info", "");
            GetRoomInfos(client, roomId);

            Console.WriteLine("======== only change the private infos =========");

            SetRoomInfos(client, roomId, null, "balabala");
            GetRoomInfos(client, roomId);

            SetRoomInfos(client, roomId, "This is public info", "This is private info");
            client.Bye();

            Console.WriteLine("======== user relogin =========");

            client = LoginRTM(rtmEndpoint, projectId, uid, token);

            if (client == null)
                return;

            Console.WriteLine("======== enter room =========");
            EnterRoom(client, roomId);

            GetRoomInfos(client, roomId);

            GetRoomsPublicInfo(client, new HashSet<long>() { 556677, 778899, 445566, 334455, 1234 });

            Console.WriteLine("======== Test done =========");
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

        static void EnterRoom(RTMClient client, long roomId)
        {
            int errorCode = client.EnterRoom(roomId);
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Enter room {0} in sync failed.", roomId);
            else
                Console.WriteLine("Enter room {0} in sync successed.", roomId);
        }

        static void LeaveRoom(RTMClient client, long roomId)
        {
            int errorCode = client.LeaveRoom(roomId);
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Leave room {0} in sync failed.", roomId);
            else
                Console.WriteLine("Leave room {0} in sync successed.", roomId);
        }

        static void GetSelfRooms(RTMClient client)
        {
            int errorCode = client.GetUserRooms(out HashSet<long> rids);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Get user rooms in sync failed, error code is {0}.", errorCode);
            else
            {
                Console.WriteLine("Get user rooms in sync successed, current I am in {0} rooms.", rids.Count);
                foreach (long rid in rids)
                    Console.WriteLine("-- room id: " + rid);
            }
        }

        static void SetRoomInfos(RTMClient client, long roomId, string publicInfos, string privateInfos)
        {
            int errorCode = client.SetRoomInfo(roomId, publicInfos, privateInfos);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Set room infos in sync failed, error code is {0}.", errorCode);
            else
                Console.WriteLine("Set room infos in sync successed.");
        }

        static void GetRoomInfos(RTMClient client, long roomId)
        {
            int errorCode = client.GetRoomInfo(out string publicInfos, out string privateInfos, roomId);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Get room infos in sync failed, error code is {0}.", errorCode);
            else
            {
                Console.WriteLine("Get room infos in sync successed.");
                Console.WriteLine("Public info: {0}", publicInfos ?? "null");
                Console.WriteLine("Private info: {0}", privateInfos ?? "null");
            }
        }

        static void GetRoomsPublicInfo(RTMClient client, HashSet<long> roomIds)
        {
            int errorCode = client.GetRoomsPublicInfo(out Dictionary<string, string> publicInfos, roomIds);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Get rooms' info in sync failed, error code is {0}.", errorCode);
            else
            {
                Console.WriteLine("Get rooms' info in sync success");
                foreach (var kvp in publicInfos)
                    Console.WriteLine("-- room id: " + kvp.Key + " info: [" + kvp.Value + "]");
            }
        }
    }
}
