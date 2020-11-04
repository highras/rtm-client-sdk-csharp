using System;
using System.Threading;
using com.fpnn.rtm;

namespace Messages
{
    class Program
    {
        private static long peerUid = 12345678;
        private static long groupId = 223344;
        private static long roomId = 556677;
        private static byte customMType = 60;

        private static string textMessage = "Hello, RTM!";
        private static byte[] binaryMessage = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: RTMMessages <rtm-endpoint> <projectId> <uid> <token>");
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

            SendP2PMessageInAsync(client, peerUid, customMType);
            SendP2PMessageInSync(client, peerUid, customMType);

            SendGroupMessageInAsync(client, groupId, customMType);
            SendGroupMessageInSync(client, groupId, customMType);

            if (EnterRoom(client, roomId))
            {
                SendRoomMessageInAsync(client, roomId, customMType);
                SendRoomMessageInSync(client, roomId, customMType);
            }

            Console.WriteLine("Wait 10 seonds for receiving server pushed messsage if those are being demoed ...");
            Thread.Sleep(10 * 1000);
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

        static void SendP2PMessageInAsync(RTMClient client, long peerUid, byte mtype)
        {
            bool status = client.SendMessage((long messageId, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send text message to user {0} in sync successed, messageId is {1}.", peerUid, messageId);
                else
                    Console.WriteLine("Send text message to user {0} in sync failed, errorCode is {1}.", peerUid, errorCode);
            }, peerUid, mtype, textMessage);

            if (!status)
                Console.WriteLine("Perpare send text message to user {0} in async failed.", peerUid);
            else
                Thread.Sleep(1000);     //-- Waiting callback desipay result info

            status = client.SendMessage((long messageId, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send binary message to user {0} in sync successed, messageId is {1}.", peerUid, messageId);
                else
                    Console.WriteLine("Send binary message to user {0} in sync failed, errorCode is {1}.", peerUid, errorCode);
            }, peerUid, mtype, binaryMessage);

            if (!status)
                Console.WriteLine("Perpare send binary message to user {0} in async failed.", peerUid);
            else
                Thread.Sleep(1000);     //-- Waiting callback desipay result info
        }

        static void SendP2PMessageInSync(RTMClient client, long peerUid, byte mtype)
        {
            long messageId;
            int errorCode = client.SendMessage(out messageId, peerUid, mtype, textMessage);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send text message to user {0} in sync successed, messageId is {1}.", peerUid, messageId);
            else
                Console.WriteLine("Send text message to user {0} in sync failed.", peerUid);

            errorCode = client.SendMessage(out messageId, peerUid, mtype, binaryMessage);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send binary message to user {0} in sync successed, messageId is {1}.", peerUid, messageId);
            else
                Console.WriteLine("Send binary message to user {0} in sync failed.", peerUid);
        }

        static void SendGroupMessageInAsync(RTMClient client, long groupId, byte mtype)
        {
            bool status = client.SendGroupMessage((long messageId, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send text message to group {0} in sync successed, messageId is {1}.", groupId, messageId);
                else
                    Console.WriteLine("Send text message to group {0} in sync failed, errorCode is {1}.", groupId, errorCode);
            }, groupId, mtype, textMessage);

            if (!status)
                Console.WriteLine("Perpare send text message to group {0} in async failed.", groupId);
            else
                Thread.Sleep(1000);     //-- Waiting callback desipay result info

            status = client.SendGroupMessage((long messageId, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send binary message to group {0} in sync successed, messageId is {1}.", groupId, messageId);
                else
                    Console.WriteLine("Send binary message to group {0} in sync failed, errorCode is {1}.", groupId, errorCode);
            }, groupId, mtype, binaryMessage);

            if (!status)
                Console.WriteLine("Perpare send binary message to group {0} in async failed.", groupId);
            else
                Thread.Sleep(1000);     //-- Waiting callback desipay result info
        }

        static void SendGroupMessageInSync(RTMClient client, long groupId, byte mtype)
        {
            long messageId;
            int errorCode = client.SendGroupMessage(out messageId, groupId, mtype, textMessage);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send text message to group {0} in sync successed, messageId is {1}.", groupId, messageId);
            else
                Console.WriteLine("Send text message to group {0} in sync failed.", groupId);

            errorCode = client.SendGroupMessage(out messageId, groupId, mtype, binaryMessage);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send binary message to group {0} in sync successed, messageId is {1}.", groupId, messageId);
            else
                Console.WriteLine("Send binary message to group {0} in sync failed.", groupId);
        }

        static void SendRoomMessageInAsync(RTMClient client, long roomId, byte mtype)
        {
            bool status = client.SendRoomMessage((long messageId, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send text message to room {0} in sync successed, messageId is {1}.", roomId, messageId);
                else
                    Console.WriteLine("Send text message to room {0} in sync failed, errorCode is {1}.", roomId, errorCode);
            }, roomId, mtype, textMessage);

            if (!status)
                Console.WriteLine("Perpare send text message to room {0} in async failed.", roomId);
            else
                Thread.Sleep(1000);     //-- Waiting callback desipay result info

            status = client.SendRoomMessage((long messageId, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send binary message to room {0} in sync successed, messageId is {1}.", roomId, messageId);
                else
                    Console.WriteLine("Send binary message to room {0} in sync failed, errorCode is {1}.", roomId, errorCode);
            }, roomId, mtype, binaryMessage);

            if (!status)
                Console.WriteLine("Perpare send binary message to room {0} in async failed.", roomId);
            else
                Thread.Sleep(1000);     //-- Waiting callback desipay result info
        }

        static void SendRoomMessageInSync(RTMClient client, long roomId, byte mtype)
        {
            long messageId;
            int errorCode = client.SendRoomMessage(out messageId, roomId, mtype, textMessage);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send text message to room {0} in sync successed, messageId is {1}.", roomId, messageId);
            else
                Console.WriteLine("Send text message to room {0} in sync failed.", roomId);

            errorCode = client.SendRoomMessage(out messageId, roomId, mtype, binaryMessage);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send binary message to room {0} in sync successed, messageId is {1}.", roomId, messageId);
            else
                Console.WriteLine("Send binary message to room {0} in sync failed.", roomId);
        }
    }
}
