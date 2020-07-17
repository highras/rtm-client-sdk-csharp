using System;
using System.Threading;
using com.fpnn.rtm;

namespace Chat
{
    class Program
    {
        private static long peerUid = 12345678;
        private static long groupId = 223344;
        private static long roomId = 556677;

        private static string textMessage = "Hello, RTM!";

        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: RTChats <rtm-endpoint> <projectId> <uid> <token>");
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

            SendP2PChatInAsync(client, peerUid);
            SendP2PChatInSync(client, peerUid);

            SendP2PCmdInAsync(client, peerUid);
            SendP2PCmdInSync(client, peerUid);

            SendGroupChatInAsync(client, groupId);
            SendGroupChatInSync(client, groupId);

            SendGroupCmdInAsync(client, groupId);
            SendGroupCmdInSync(client, groupId);

            if (EnterRoom(client, roomId))
            {
                SendRoomChatInAsync(client, roomId);
                SendRoomChatInSync(client, roomId);

                SendRoomCmdInAsync(client, roomId);
                SendRoomCmdInSync(client, roomId);
            }

            Console.WriteLine("Wait 30 seonds for receiving server pushed Chat & Cmd if those are being demoed ...");
            Thread.Sleep(30 * 1000);
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

        static bool EnterRoom(RTMClient client, long roomId)
        {
            int errorCode = client.EnterRoom(roomId);
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Console.WriteLine("Enter room {0} in sync failed.", roomId);
                return false;
            }
            else
                return true;
        }

        //------------------------[ Chat Demo ]-------------------------//
        static void SendP2PChatInAsync(RTMClient client, long peerUid)
        {
            bool status = client.SendChat((long mtime, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send chat message to user {0} in sync successed, mtime is {1}.", peerUid, mtime);
                else
                    Console.WriteLine("Send chat message to user {0} in sync failed, errorCode is {1}.", peerUid, errorCode);
            }, peerUid, textMessage);

            if (!status)
                Console.WriteLine("Perpare send chat message to user {0} in async failed.", peerUid);
            else
                Thread.Sleep(1000);     //-- Waiting callback desipay result info
        }

        static void SendP2PChatInSync(RTMClient client, long peerUid)
        {
            int errorCode = client.SendChat(out long mtime, peerUid, textMessage);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send chat message to user {0} in sync successed, mtime is {1}.", peerUid, mtime);
            else
                Console.WriteLine("Send chat message to user {0} in sync failed.", peerUid);
        }

        static void SendGroupChatInAsync(RTMClient client, long groupId)
        {
            bool status = client.SendGroupChat((long mtime, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send chat message to group {0} in sync successed, mtime is {1}.", groupId, mtime);
                else
                    Console.WriteLine("Send chat message to group {0} in sync failed, errorCode is {1}.", groupId, errorCode);
            }, groupId, textMessage);

            if (!status)
                Console.WriteLine("Perpare send chat message to group {0} in async failed.", groupId);
            else
                Thread.Sleep(1000);     //-- Waiting callback desipay result info
        }

        static void SendGroupChatInSync(RTMClient client, long groupId)
        {
            int errorCode = client.SendGroupChat(out long mtime, groupId, textMessage);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send chat message to group {0} in sync successed, mtime is {1}.", groupId, mtime);
            else
                Console.WriteLine("Send chat message to group {0} in sync failed.", groupId);
        }

        static void SendRoomChatInAsync(RTMClient client, long roomId)
        {
            bool status = client.SendRoomChat((long mtime, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send chat message to room {0} in sync successed, mtime is {1}.", roomId, mtime);
                else
                    Console.WriteLine("Send chat message to room {0} in sync failed, errorCode is {1}.", roomId, errorCode);
            }, roomId, textMessage);

            if (!status)
                Console.WriteLine("Perpare send chat message to room {0} in async failed.", roomId);
            else
                Thread.Sleep(1000);     //-- Waiting callback desipay result info
        }

        static void SendRoomChatInSync(RTMClient client, long roomId)
        {
            long mtime;
            int errorCode = client.SendRoomChat(out mtime, roomId, textMessage);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send chat message to room {0} in sync successed, mtime is {1}.", roomId, mtime);
            else
                Console.WriteLine("Send chat message to room {0} in sync failed.", roomId);
        }

        //------------------------[ Cmd Demo ]-------------------------//
        static void SendP2PCmdInAsync(RTMClient client, long peerUid)
        {
            bool status = client.SendCmd((long mtime, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send cmd message to user {0} in sync successed, mtime is {1}.", peerUid, mtime);
                else
                    Console.WriteLine("Send cmd message to user {0} in sync failed, errorCode is {1}.", peerUid, errorCode);
            }, peerUid, textMessage);

            if (!status)
                Console.WriteLine("Perpare send cmd message to user {0} in async failed.", peerUid);
            else
                Thread.Sleep(1000);     //-- Waiting callback desipay result info
        }

        static void SendP2PCmdInSync(RTMClient client, long peerUid)
        {
            int errorCode = client.SendCmd(out long mtime, peerUid, textMessage);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send cmd message to user {0} in sync successed, mtime is {1}.", peerUid, mtime);
            else
                Console.WriteLine("Send cmd message to user {0} in sync failed.", peerUid);
        }

        static void SendGroupCmdInAsync(RTMClient client, long groupId)
        {
            bool status = client.SendGroupCmd((long mtime, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send cmd message to group {0} in sync successed, mtime is {1}.", groupId, mtime);
                else
                    Console.WriteLine("Send cmd message to group {0} in sync failed, errorCode is {1}.", groupId, errorCode);
            }, groupId, textMessage);

            if (!status)
                Console.WriteLine("Perpare send cmd message to group {0} in async failed.", groupId);
            else
                Thread.Sleep(1000);     //-- Waiting callback desipay result info
        }

        static void SendGroupCmdInSync(RTMClient client, long groupId)
        {
            int errorCode = client.SendGroupCmd(out long mtime, groupId, textMessage);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send cmd message to group {0} in sync successed, mtime is {1}.", groupId, mtime);
            else
                Console.WriteLine("Send cmd message to group {0} in sync failed.", groupId);
        }

        static void SendRoomCmdInAsync(RTMClient client, long roomId)
        {
            bool status = client.SendRoomCmd((long mtime, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send cmd message to room {0} in sync successed, mtime is {1}.", roomId, mtime);
                else
                    Console.WriteLine("Send cmd message to room {0} in sync failed, errorCode is {1}.", roomId, errorCode);
            }, roomId, textMessage);

            if (!status)
                Console.WriteLine("Perpare cmd chat message to room {0} in async failed.", roomId);
            else
                Thread.Sleep(1000);     //-- Waiting callback desipay result info
        }

        static void SendRoomCmdInSync(RTMClient client, long roomId)
        {
            long mtime;
            int errorCode = client.SendRoomCmd(out mtime, roomId, textMessage);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send cmd message to room {0} in sync successed, mtime is {1}.", roomId, mtime);
            else
                Console.WriteLine("Send cmd message to room {0} in sync failed.", roomId);
        }
    }
}
