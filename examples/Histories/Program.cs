﻿using System;
using System.Collections.Generic;
using com.fpnn.rtm;

namespace Histories
{
    class Program
    {
        private static long peerUid = 12345678;
        private static long groupId = 223344;
        private static long roomId = 556677;

        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: RTMHistories <rtm-endpoint> <projectId> <uid> <token>");
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

            EnterRoom(client, roomId);

            int fetchCount = 100;
            Console.WriteLine("\n================[ Get P2P Message {0} items ]==================", fetchCount);
            GetP2PMessageInSync(client, peerUid, fetchCount);

            Console.WriteLine("\n================[ Get Group Message {0} items ]==================", fetchCount);
            GetGroupMessageInSync(client, groupId, fetchCount);

            Console.WriteLine("\n================[ Get Room Message {0} items ]==================", fetchCount);
            GetRoomMessageInSync(client, roomId, fetchCount);

            Console.WriteLine("\n================[ Get Broadcast Message {0} items ]==================", fetchCount);
            GetBroadcastMessageInSync(client, fetchCount);


            Console.WriteLine("\n================[ Get P2P Chat {0} items ]==================", fetchCount);
            GetP2PChatInSync(client, peerUid, fetchCount);

            Console.WriteLine("\n================[ Get Group Chat {0} items ]==================", fetchCount);
            GetGroupChatInSync(client, groupId, fetchCount);

            Console.WriteLine("\n================[ Get Room Chat {0} items ]==================", fetchCount);
            GetRoomChatInSync(client, roomId, fetchCount);

            Console.WriteLine("\n================[ Get Broadcast Chat {0} items ]==================", fetchCount);
            GetBroadcastChatInSync(client, fetchCount);
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

        //------------------------[ Desplay Histories Message ]-------------------------//
        static void DisplayHistoryMessages(List<HistoryMessage> messages)
        {
            foreach (HistoryMessage hm in messages)
            {
                if (hm.binaryMessage != null)
                    Console.WriteLine("-- Fetched: ID: {0}, from {1}, mtype {2}, mid {3}, binary message length {4}, attrs {5}, mtime {6}",
                        hm.id, hm.fromUid, hm.mtype, hm.mid, hm.binaryMessage.Length, hm.attrs, hm.mtime);
                else
                    Console.WriteLine("-- Fetched: ID: {0}, from {1}, mtype {2}, mid {3}, message {4}, attrs {5}, mtime {6}",
                        hm.id, hm.fromUid, hm.mtype, hm.mid, hm.stringMessage, hm.attrs, hm.mtime);
            }
        }

        //------------------------[ Message Histories Demo ]-------------------------//
        static void GetP2PMessageInSync(RTMClient client, long peerUid, int count)
        {
            long beginMsec = 0;
            long endMsec = 0;
            long lastId = 0;
            int fetchedCount = 0;

            while (count > 0)
            {
                int maxCount = (count > 20) ? 20 : count;
                count -= maxCount;

                int errorCode = client.GetP2PMessage(out HistoryMessageResult result, peerUid, true, maxCount, beginMsec, endMsec, lastId);
                if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                {
                    Console.WriteLine("Get P2P history message with user {0} in sync failed, current fetched {1} items, error Code {2}",
                        peerUid, fetchedCount, errorCode);
                    return;
                }

                fetchedCount += result.count;
                DisplayHistoryMessages(result.messages);

                beginMsec = result.beginMsec;
                endMsec = result.endMsec;
                lastId = result.lastId;
            }

            Console.WriteLine("Get P2P history message total fetched {0} items", fetchedCount);
        }

        static void GetGroupMessageInSync(RTMClient client, long groupId, int count)
        {
            long beginMsec = 0;
            long endMsec = 0;
            long lastId = 0;
            int fetchedCount = 0;

            while (count > 0)
            {
                int maxCount = (count > 20) ? 20 : count;
                count -= maxCount;

                int errorCode = client.GetGroupMessage(out HistoryMessageResult result, groupId, true, maxCount, beginMsec, endMsec, lastId);
                if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                {
                    Console.WriteLine("Get group history message in group {0} in sync failed, current fetched {1} items, error Code {2}",
                        groupId, fetchedCount, errorCode);
                    return;
                }

                fetchedCount += result.count;
                DisplayHistoryMessages(result.messages);

                beginMsec = result.beginMsec;
                endMsec = result.endMsec;
                lastId = result.lastId;
            }

            Console.WriteLine("Get group history message total fetched {0} items", fetchedCount);
        }

        static void GetRoomMessageInSync(RTMClient client, long roomId, int count)
        {
            long beginMsec = 0;
            long endMsec = 0;
            long lastId = 0;
            int fetchedCount = 0;

            while (count > 0)
            {
                int maxCount = (count > 20) ? 20 : count;
                count -= maxCount;

                int errorCode = client.GetRoomMessage(out HistoryMessageResult result, roomId, true, maxCount, beginMsec, endMsec, lastId);
                if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                {
                    Console.WriteLine("Get room history message in room {0} in sync failed, current fetched {1} items, error Code {2}",
                        roomId, fetchedCount, errorCode);
                    return;
                }

                fetchedCount += result.count;
                DisplayHistoryMessages(result.messages);

                beginMsec = result.beginMsec;
                endMsec = result.endMsec;
                lastId = result.lastId;
            }

            Console.WriteLine("Get room history message total fetched {0} items", fetchedCount);
        }

        static void GetBroadcastMessageInSync(RTMClient client, int count)
        {
            long beginMsec = 0;
            long endMsec = 0;
            long lastId = 0;
            int fetchedCount = 0;

            while (count > 0)
            {
                int maxCount = (count > 20) ? 20 : count;
                count -= maxCount;

                int errorCode = client.GetBroadcastMessage(out HistoryMessageResult result, true, maxCount, beginMsec, endMsec, lastId);
                if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                {
                    Console.WriteLine("Get broadcast history message in sync failed, current fetched {0} items, error Code {1}",
                        fetchedCount, errorCode);
                    return;
                }

                fetchedCount += result.count;
                DisplayHistoryMessages(result.messages);

                beginMsec = result.beginMsec;
                endMsec = result.endMsec;
                lastId = result.lastId;
            }

            Console.WriteLine("Get broadcast history message total fetched {0} items", fetchedCount);
        }

        //------------------------[ Chat Histories Demo ]-------------------------//
        static void GetP2PChatInSync(RTMClient client, long peerUid, int count)
        {
            long beginMsec = 0;
            long endMsec = 0;
            long lastId = 0;
            int fetchedCount = 0;

            while (count > 0)
            {
                int maxCount = (count > 20) ? 20 : count;
                count -= maxCount;

                int errorCode = client.GetP2PChat(out HistoryMessageResult result, peerUid, true, maxCount, beginMsec, endMsec, lastId);
                if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                {
                    Console.WriteLine("Get P2P history chat with user {0} in sync failed, current fetched {1} items, error Code {2}",
                        peerUid, fetchedCount, errorCode);
                    return;
                }

                fetchedCount += result.count;
                DisplayHistoryMessages(result.messages);

                beginMsec = result.beginMsec;
                endMsec = result.endMsec;
                lastId = result.lastId;
            }

            Console.WriteLine("Get P2P history chat total fetched {0} items", fetchedCount);
        }

        static void GetGroupChatInSync(RTMClient client, long groupId, int count)
        {
            long beginMsec = 0;
            long endMsec = 0;
            long lastId = 0;
            int fetchedCount = 0;

            while (count > 0)
            {
                int maxCount = (count > 20) ? 20 : count;
                count -= maxCount;

                int errorCode = client.GetGroupChat(out HistoryMessageResult result, groupId, true, maxCount, beginMsec, endMsec, lastId);
                if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                {
                    Console.WriteLine("Get group history chat in group {0} in sync failed, current fetched {1} items, error Code {2}",
                        groupId, fetchedCount, errorCode);
                    return;
                }

                fetchedCount += result.count;
                DisplayHistoryMessages(result.messages);

                beginMsec = result.beginMsec;
                endMsec = result.endMsec;
                lastId = result.lastId;
            }

            Console.WriteLine("Get group history chat total fetched {0} items", fetchedCount);
        }

        static void GetRoomChatInSync(RTMClient client, long roomId, int count)
        {
            long beginMsec = 0;
            long endMsec = 0;
            long lastId = 0;
            int fetchedCount = 0;

            while (count > 0)
            {
                int maxCount = (count > 20) ? 20 : count;
                count -= maxCount;

                int errorCode = client.GetRoomChat(out HistoryMessageResult result, roomId, true, maxCount, beginMsec, endMsec, lastId);
                if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                {
                    Console.WriteLine("Get room history chat in room {0} in sync failed, current fetched {1} items, error Code {2}",
                        roomId, fetchedCount, errorCode);
                    return;
                }

                fetchedCount += result.count;
                DisplayHistoryMessages(result.messages);

                beginMsec = result.beginMsec;
                endMsec = result.endMsec;
                lastId = result.lastId;
            }

            Console.WriteLine("Get room history chat total fetched {0} items", fetchedCount);
        }

        static void GetBroadcastChatInSync(RTMClient client, int count)
        {
            long beginMsec = 0;
            long endMsec = 0;
            long lastId = 0;
            int fetchedCount = 0;

            while (count > 0)
            {
                int maxCount = (count > 20) ? 20 : count;
                count -= maxCount;

                int errorCode = client.GetBroadcastChat(out HistoryMessageResult result, true, maxCount, beginMsec, endMsec, lastId);
                if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                {
                    Console.WriteLine("Get broadcast history chat in sync failed, current fetched {0} items, error Code {1}",
                        fetchedCount, errorCode);
                    return;
                }

                fetchedCount += result.count;
                DisplayHistoryMessages(result.messages);

                beginMsec = result.beginMsec;
                endMsec = result.endMsec;
                lastId = result.lastId;
            }

            Console.WriteLine("Get broadcast history chat total fetched {0} items", fetchedCount);
        }
    }
}
