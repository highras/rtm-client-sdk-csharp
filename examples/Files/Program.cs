using System;
using System.Threading;
using com.fpnn.rtm;

namespace Files
{
    class Program
    {
        private static long peerUid = 12345678;
        private static long groupId = 223344;
        private static long roomId = 556677;

        private static string filename = "demo.bin";
        private static byte[] fileContent = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: RTMFiles <rtm-endpoint> <projectId> <uid> <token>");
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

            SendP2PFileInAsync(client, peerUid, MessageType.NormalFile);
            SendP2PFileInSync(client, peerUid, MessageType.NormalFile);

            SendGroupFileInAsync(client, groupId, MessageType.NormalFile);
            SendGroupFileInSync(client, groupId, MessageType.NormalFile);

            EnterRoom(client, roomId);

            SendRoomFileInAsync(client, roomId, MessageType.NormalFile);
            SendRoomFileInSync(client, roomId, MessageType.NormalFile);
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

        //--------------[ Send files Demo ]---------------------//
        static void SendP2PFileInAsync(RTMClient client, long peerUid, MessageType type)
        {
            bool status = client.SendFile((long mtime, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send file to user {0} in async successed, mtime is {1}.", peerUid, mtime);
                else
                    Console.WriteLine("Send text message to user {0} in async failed, errorCode is {1}.", peerUid, errorCode);
            }, peerUid, type, fileContent, filename);

            if (!status)
                Console.WriteLine("Perpare send file to user {0} in async failed.", peerUid);
            else
                Thread.Sleep(3000);     //-- Waiting callback desipay result info
        }

        static void SendP2PFileInSync(RTMClient client, long peerUid, MessageType type)
        {
            int errorCode = client.SendFile(out long mtime, peerUid, type, fileContent, filename);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send file to user {0} in sync successed, mtime is {1}.", peerUid, mtime);
            else
                Console.WriteLine("Send file to user {0} in sync failed, error code {1}.", peerUid, errorCode);
        }

        static void SendGroupFileInAsync(RTMClient client, long groupId, MessageType type)
        {
            bool status = client.SendGroupFile((long mtime, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send file to group {0} in async successed, mtime is {1}.", groupId, mtime);
                else
                    Console.WriteLine("Send file to group {0} in async failed, errorCode is {1}.", groupId, errorCode);
            }, groupId, type, fileContent, filename);

            if (!status)
                Console.WriteLine("Perpare send file to group {0} in async failed.", groupId);
            else
                Thread.Sleep(3000);     //-- Waiting callback desipay result info
        }

        static void SendGroupFileInSync(RTMClient client, long groupId, MessageType type)
        {
            int errorCode = client.SendGroupFile(out long mtime, groupId, type, fileContent, filename);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send file to group {0} in sync successed, mtime is {1}.", groupId, mtime);
            else
                Console.WriteLine("Send file to group {0} in sync failed, error code {1}.", groupId, errorCode);
        }

        static void SendRoomFileInAsync(RTMClient client, long roomId, MessageType type)
        {
            bool status = client.SendRoomFile((long mtime, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send file to room {0} in async successed, mtime is {1}.", roomId, mtime);
                else
                    Console.WriteLine("Send file to room {0} in async failed, errorCode is {1}.", roomId, errorCode);
            }, roomId, type, fileContent, filename);

            if (!status)
                Console.WriteLine("Perpare send file to room {0} in async failed.", roomId);
            else
                Thread.Sleep(3000);     //-- Waiting callback desipay result info
        }

        static void SendRoomFileInSync(RTMClient client, long roomId, MessageType type)
        {
            int errorCode = client.SendRoomFile(out long mtime, roomId, type, fileContent, filename);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send file to room {0} in sync successed, mtime is {1}.", roomId, mtime);
            else
                Console.WriteLine("Send file to room {0} in sync failed, error code {1}.", roomId, errorCode);
        }
    }
}
