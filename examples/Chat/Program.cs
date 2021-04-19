using System;
using System.Collections.Generic;
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

            GetP2PUnreadInSync(client, new HashSet<long> { peerUid, peerUid+1, peerUid+2 }, new HashSet<byte>{ 30, 40, 50 });
            GetP2PUnreadInSyncPlus(client, new HashSet<long> { peerUid, peerUid + 1, peerUid + 2 }, new HashSet<byte> { 30, 40, 50 });
            GetGroupUnreadInSync(client, new HashSet<long> { groupId }, new HashSet<byte> { 30, 40, 50 });
            GetGroupUnreadInSyncPlus(client, new HashSet<long> { groupId }, new HashSet<byte> { 30, 40, 50 });

            Console.WriteLine("Wait 30 seonds for receiving server pushed Chat & Cmd if those are being demoed ...");
            Thread.Sleep(30 * 1000);

            TextAudit(client, "sdaada asdasd asdasd asdas dds");
            TextAudit(client, "ssds 他妈的， 去你妈逼，操你妈的");
            TextAudit(client, "sdaada fuck you mother dds");

            ImageAudit(client, "https://box.bdimg.com/static/fisp_static/common/img/searchbox/logo_news_276_88_1f9876a.png");
            AudioAudit(client, "https://opus-codec.org/static/examples/samples/speech_orig.wav");
            VideoAudit(client, "http://vfx.mtime.cn/Video/2019/02/04/mp4/190204084208765161.mp4");

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
            bool status = client.SendChat((long messageId, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send chat message to user {0} in sync successed, messageId is {1}.", peerUid, messageId);
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
            int errorCode = client.SendChat(out long messageId, peerUid, textMessage);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send chat message to user {0} in sync successed, messageId is {1}.", peerUid, messageId);
            else
                Console.WriteLine("Send chat message to user {0} in sync failed.", peerUid);
        }

        static void SendGroupChatInAsync(RTMClient client, long groupId)
        {
            bool status = client.SendGroupChat((long messageId, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send chat message to group {0} in sync successed, messageId is {1}.", groupId, messageId);
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
            int errorCode = client.SendGroupChat(out long messageId, groupId, textMessage);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send chat message to group {0} in sync successed, messageId is {1}.", groupId, messageId);
            else
                Console.WriteLine("Send chat message to group {0} in sync failed.", groupId);
        }

        static void SendRoomChatInAsync(RTMClient client, long roomId)
        {
            bool status = client.SendRoomChat((long messageId, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send chat message to room {0} in sync successed, messageId is {1}.", roomId, messageId);
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
            long messageId;
            int errorCode = client.SendRoomChat(out messageId, roomId, textMessage);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send chat message to room {0} in sync successed, messageId is {1}.", roomId, messageId);
            else
                Console.WriteLine("Send chat message to room {0} in sync failed.", roomId);
        }

        //------------------------[ Cmd Demo ]-------------------------//
        static void SendP2PCmdInAsync(RTMClient client, long peerUid)
        {
            bool status = client.SendCmd((long messageId, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send cmd message to user {0} in sync successed, messageId is {1}.", peerUid, messageId);
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
            int errorCode = client.SendCmd(out long messageId, peerUid, textMessage);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send cmd message to user {0} in sync successed, messageId is {1}.", peerUid, messageId);
            else
                Console.WriteLine("Send cmd message to user {0} in sync failed.", peerUid);
        }

        static void SendGroupCmdInAsync(RTMClient client, long groupId)
        {
            bool status = client.SendGroupCmd((long messageId, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send cmd message to group {0} in sync successed, messageId is {1}.", groupId, messageId);
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
            int errorCode = client.SendGroupCmd(out long messageId, groupId, textMessage);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send cmd message to group {0} in sync successed, messageId is {1}.", groupId, messageId);
            else
                Console.WriteLine("Send cmd message to group {0} in sync failed.", groupId);
        }

        static void SendRoomCmdInAsync(RTMClient client, long roomId)
        {
            bool status = client.SendRoomCmd((long messageId, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                    Console.WriteLine("Send cmd message to room {0} in sync successed, messageId is {1}.", roomId, messageId);
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
            long messageId;
            int errorCode = client.SendRoomCmd(out messageId, roomId, textMessage);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Send cmd message to room {0} in sync successed, messageId is {1}.", roomId, messageId);
            else
                Console.WriteLine("Send cmd message to room {0} in sync failed.", roomId);
        }

        //------------------------[ Get P2P & Group Unread ]-------------------------//
        static void GetP2PUnreadInSync(RTMClient client, HashSet<long> uids, HashSet<byte> mtypes)
        {
            Dictionary<long, int> unreadMap;
            int errorCode = client.GetP2PUnread(out unreadMap, uids);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Console.WriteLine("Fetch P2P unread in sync successful.");
                foreach (KeyValuePair<long, int> kvp in unreadMap)
                {
                    Console.WriteLine($" -- peer: {kvp.Key}, unread message {kvp.Value}");
                }
            }
            else
                Console.WriteLine($"Fetch P2P unread in sync failed. Error code {errorCode}");

            errorCode = client.GetP2PUnread(out unreadMap, uids, mtypes);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Console.WriteLine("Fetch P2P unread with mTypes in sync successful.");
                foreach (KeyValuePair<long, int> kvp in unreadMap)
                {
                    Console.WriteLine($" -- peer: {kvp.Key}, unread message {kvp.Value}");
                }
            }
            else
                Console.WriteLine($"Fetch P2P unread with mTypes in sync failed. Error code {errorCode}");
        }

        static void GetP2PUnreadInSyncPlus(RTMClient client, HashSet<long> uids, HashSet<byte> mtypes)
        {
            Dictionary<long, int> unreadMap;
            Dictionary<long, long> lastUnreadTimestampDictionary;
            int errorCode = client.GetP2PUnread(out unreadMap, out lastUnreadTimestampDictionary, uids);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Console.WriteLine("Fetch P2P unread in sync successful.");
                foreach (KeyValuePair<long, int> kvp in unreadMap)
                {
                    Console.WriteLine($" -- peer: {kvp.Key}, unread message {kvp.Value}");
                }
                foreach (KeyValuePair<long, long> kvp in lastUnreadTimestampDictionary)
                {
                    Console.WriteLine($" -- peer: {kvp.Key}, last unread message UTC in msec is {kvp.Value}");
                }
            }
            else
                Console.WriteLine($"Fetch P2P unread in sync failed. Error code {errorCode}");

            errorCode = client.GetP2PUnread(out unreadMap, out lastUnreadTimestampDictionary, uids, mtypes);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Console.WriteLine("Fetch P2P unread with mTypes in sync successful.");
                foreach (KeyValuePair<long, int> kvp in unreadMap)
                {
                    Console.WriteLine($" -- peer: {kvp.Key}, unread message {kvp.Value}");
                }
                foreach (KeyValuePair<long, long> kvp in lastUnreadTimestampDictionary)
                {
                    Console.WriteLine($" -- peer: {kvp.Key}, last unread message UTC in msec is {kvp.Value}");
                }
            }
            else
                Console.WriteLine($"Fetch P2P unread with mTypes in sync failed. Error code {errorCode}");
        }

        static void GetGroupUnreadInSync(RTMClient client, HashSet<long> groupIds, HashSet<byte> mtypes)
        {
            Dictionary<long, int> unreadMap;
            int errorCode = client.GetGroupUnread(out unreadMap, groupIds);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Console.WriteLine("Fetch group unread in sync successful.");
                foreach (KeyValuePair<long, int> kvp in unreadMap)
                {
                    Console.WriteLine($" -- group: {kvp.Key}, unread message {kvp.Value}");
                }
            }
            else
                Console.WriteLine($"Fetch group unread in sync failed. Error code {errorCode}");

            errorCode = client.GetGroupUnread(out unreadMap, groupIds, mtypes);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Console.WriteLine("Fetch group unread with mTypes in sync successful.");
                foreach (KeyValuePair<long, int> kvp in unreadMap)
                {
                    Console.WriteLine($" -- group: {kvp.Key}, unread message {kvp.Value}");
                }
            }
            else
                Console.WriteLine($"Fetch group unread with mTypes in sync failed. Error code {errorCode}");
        }

        static void GetGroupUnreadInSyncPlus(RTMClient client, HashSet<long> groupIds, HashSet<byte> mtypes)
        {
            Dictionary<long, int> unreadMap;
            Dictionary<long, long> lastUnreadTimestampDictionary;
            int errorCode = client.GetGroupUnread(out unreadMap, out lastUnreadTimestampDictionary, groupIds);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Console.WriteLine("Fetch group unread in sync successful.");
                foreach (KeyValuePair<long, int> kvp in unreadMap)
                {
                    Console.WriteLine($" -- group: {kvp.Key}, unread message {kvp.Value}");
                }
                foreach (KeyValuePair<long, long> kvp in lastUnreadTimestampDictionary)
                {
                    Console.WriteLine($" -- group: {kvp.Key}, last unread message UTC in msec is {kvp.Value}");
                }
            }
            else
                Console.WriteLine($"Fetch group unread in sync failed. Error code {errorCode}");

            errorCode = client.GetGroupUnread(out unreadMap, groupIds, mtypes);

            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Console.WriteLine("Fetch group unread with mTypes in sync successful.");
                foreach (KeyValuePair<long, int> kvp in unreadMap)
                {
                    Console.WriteLine($" -- group: {kvp.Key}, unread message {kvp.Value}");
                }
                foreach (KeyValuePair<long, long> kvp in lastUnreadTimestampDictionary)
                {
                    Console.WriteLine($" -- group: {kvp.Key}, last unread message UTC in msec is {kvp.Value}");
                }
            }
            else
                Console.WriteLine($"Fetch group unread with mTypes in sync failed. Error code {errorCode}");
        }

        //------------------------[ Text Image Audio Vedio Audit ]-------------------------//
        static void TextAudit(RTMClient client, string text)
        {
            int errorCode = client.TextCheck(out TextCheckResult result, text);
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("TextCheck in sync failed, error " + errorCode);
            else
            {
                Console.WriteLine("TextCheck in sync successed");
                Console.WriteLine("  -- result " + result.result);
                Console.WriteLine("  -- text " + result.text);

                if (result.tags != null)
                    Console.WriteLine("  -- tags.Count " + result.tags.Count);
                if (result.wlist != null)
                    Console.WriteLine("  -- wlist.Count " + result.wlist.Count);
            }
        }

        static void ImageAudit(RTMClient client, string url)
        {
            int errorCode = client.ImageCheck(out CheckResult result,url);
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("ImageCheck in sync failed, error " + errorCode);
            else
            {
                Console.WriteLine("ImageCheck in sync successed");
                Console.WriteLine("  -- result " + result.result);
                if (result.tags != null)
                    Console.WriteLine("  -- tags.Count " + result.tags.Count);
            }
        }

        static void AudioAudit(RTMClient client, string url)
        {
            int errorCode = client.AudioCheck(out CheckResult result, url, "zh-CN");
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("AudioCheck in sync failed, error " + errorCode);
            else
            {
                Console.WriteLine("AudioCheck in sync successed");
                Console.WriteLine("  -- result " + result.result);
                if (result.tags != null)
                    Console.WriteLine("  -- tags.Count " + result.tags.Count);
            }
        }

        static void VideoAudit(RTMClient client, string url)
        {
            int errorCode = client.VideoCheck(out CheckResult result, url, "testVideo");
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("VideoCheck in sync failed, error " + errorCode);
            else
            {
                Console.WriteLine("VideoCheck in sync successed");
                Console.WriteLine("  -- result " + result.result);
                if (result.tags != null)
                    Console.WriteLine("  -- tags.Count " + result.tags.Count);
            }
        }
    }
}
