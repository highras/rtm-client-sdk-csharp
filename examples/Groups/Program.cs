using System;
using System.Collections.Generic;
using com.fpnn.rtm;

namespace Groups
{
    class Program
    {
        private static long groupId = 223344;

        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: RTMGroups <rtm-endpoint> <projectId> <uid> <token>");
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

            Console.WriteLine("======== get group members =========");
            GetGroupMembers(client, groupId);

            Console.WriteLine("======== add group members =========");
            AddGroupMembers(client, groupId, new HashSet<long>() { 99688868, 99688878, 99688888 });

            Console.WriteLine("======== get group members =========");
            GetGroupMembers(client, groupId);

            Console.WriteLine("======== delete group members =========");
            DeleteGroupMembers(client, groupId, new HashSet<long>() { 99688878 });

            Console.WriteLine("======== get group members =========");
            GetGroupMembers(client, groupId);
            GetGroupMembersAndOnlineMembers(client, groupId);

            Console.WriteLine("======== get group member count =========");
            GetGroupMemberCount(client, groupId);
            GetGroupMemberCountWithOnlineMemberCount(client, groupId);


            Console.WriteLine("======== get self groups =========");
            GetSelfGroups(client);


            Console.WriteLine("======== set group infos =========");

            SetGroupInfos(client, groupId, "This is public info", "This is private info");
            GetGroupInfos(client, groupId);

            Console.WriteLine("======== change group infos =========");

            SetGroupInfos(client, groupId, "", "This is private info");
            GetGroupInfos(client, groupId);

            Console.WriteLine("======== change group infos =========");

            SetGroupInfos(client, groupId, "This is public info", "");
            GetGroupInfos(client, groupId);

            Console.WriteLine("======== only change the private infos =========");

            SetGroupInfos(client, groupId, null, "balabala");
            GetGroupInfos(client, groupId);

            SetGroupInfos(client, groupId, "This is public info", "This is private info");
            client.Bye();

            Console.WriteLine("======== user relogin =========");

            client = LoginRTM(rtmEndpoint, projectId, uid, token);

            if (client == null)
                return;

            GetGroupInfos(client, groupId);

            GetGroupsPublicInfo(client, new HashSet<long>() { 223344, 334455, 445566, 667788, 778899 });

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

        static void AddGroupMembers(RTMClient client, long groupId, HashSet<long> uids)
        {
            int errorCode = client.AddGroupMembers(groupId, uids);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Add group members in sync failed, error code is {0}.", errorCode);
            else
                Console.WriteLine("Add group members in sync successed.");
        }

        static void DeleteGroupMembers(RTMClient client, long groupId, HashSet<long> uids)
        {
            int errorCode = client.DeleteGroupMembers(groupId, uids);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Delete group members in sync failed, error code is {0}.", errorCode);
            else
                Console.WriteLine("Delete group members in sync successed.");
        }

        static void GetGroupMembers(RTMClient client, long groupId)
        {
            int errorCode = client.GetGroupMembers(out HashSet<long> uids, groupId);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Get group members in sync failed, error code is {0}.", errorCode);
            else
            {
                Console.WriteLine("Get group members in sync successed, current has {0} members.", uids.Count);
                foreach (long uid in uids)
                    Console.WriteLine("-- member uid: " + uid);
            }
        }

        static void GetGroupMembersAndOnlineMembers(RTMClient client, long groupId)
        {
            int errorCode = client.GetGroupMembers(out HashSet<long> allUids, out HashSet<long> onlineUids, groupId);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Get group members with online members in sync failed, error code is {0}.", errorCode);
            else
            {
                Console.WriteLine($"Get group members with online members in sync successed, current has {allUids.Count} members, {onlineUids.Count} are online.");
                foreach (long uid in allUids)
                    Console.WriteLine("-- [ALL] member uid: " + uid);
                foreach (long uid in onlineUids)
                    Console.WriteLine("-- [Online] member uid: " + uid);
            }

            bool status = client.GetGroupMembers((HashSet<long> allUids, HashSet<long> onlineUids, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                {
                    Console.WriteLine($"Get group members with online members in async success, current has {allUids.Count} members, {onlineUids.Count} are online.");
                    foreach (long uid in allUids)
                        Console.WriteLine("-- [ALL] member uid: " + uid);
                    foreach (long uid in onlineUids)
                        Console.WriteLine("-- [Online] member uid: " + uid);
                }
                else
                    Console.WriteLine($"Get group members with online members in async failed, error code is {errorCode}.");
            }, groupId);
            if (!status)
                Console.WriteLine("Launch group members with online members in async failed.");

            System.Threading.Thread.Sleep(3 * 1000);
        }

        static void GetGroupMemberCount(RTMClient client, long groupId)
        {
            int errorCode = client.GetGroupCount(out int memberCount, groupId);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine($"Get group members count in sync failed, error code is {errorCode}.");
            else
                Console.WriteLine($"Get group members count in sync success, total {memberCount}");

            bool status = client.GetGroupCount((int memberCount, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                {
                    Console.WriteLine($"Get group members count in async success, total {memberCount}");
                }
                else
                    Console.WriteLine($"Get group members count in async failed, error code is {errorCode}.");
            }, groupId);
            if (!status)
                Console.WriteLine("Launch group members count in async failed.");

            System.Threading.Thread.Sleep(3 * 1000);
        }

        static void GetGroupMemberCountWithOnlineMemberCount(RTMClient client, long groupId)
        {
            int errorCode = client.GetGroupCount(out int memberCount, out int onlineCount, groupId);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine($"Get group members count with online member count in sync failed, error code is {errorCode}.");
            else
                Console.WriteLine($"Get group members count with online member count in sync success, total {memberCount}, online {onlineCount}");

            bool status = client.GetGroupCount((int memberCount, int onlineCount, int errorCode) => {
                if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                {
                    Console.WriteLine($"Get group members count with online member count in async success, total {memberCount}, online {onlineCount}");
                }
                else
                    Console.WriteLine($"Get group members count with online member count in async failed, error code is {errorCode}.");
            }, groupId);
            if (!status)
                Console.WriteLine("Launch group members count with online member count in async failed.");

            System.Threading.Thread.Sleep(3 * 1000);
        }

        static void GetSelfGroups(RTMClient client)
        {
            int errorCode = client.GetUserGroups(out HashSet<long> gids);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Get user groups in sync failed, error code is {0}.", errorCode);
            else
            {
                Console.WriteLine("Get user groups in sync successed, current I am in {0} groups.", gids.Count);
                foreach (long gid in gids)
                    Console.WriteLine("-- group id: " + gid);
            }
        }

        static void SetGroupInfos(RTMClient client, long groupId, string publicInfos, string privateInfos)
        {
            int errorCode = client.SetGroupInfo(groupId, publicInfos, privateInfos);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Set group infos in sync failed, error code is {0}.", errorCode);
            else
                Console.WriteLine("Set group infos in sync successed.");
        }

        static void GetGroupInfos(RTMClient client, long groupId)
        {
            int errorCode = client.GetGroupInfo(out string publicInfos, out string privateInfos, groupId);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Get group infos in sync failed, error code is {0}.", errorCode);
            else
            {
                Console.WriteLine("Get group infos in sync successed.");
                Console.WriteLine("Public info: {0}", publicInfos ?? "null");
                Console.WriteLine("Private info: {0}", privateInfos ?? "null");
            }
        }

        static void GetGroupsPublicInfo(RTMClient client, HashSet<long> groupIds)
        {
            int errorCode = client.GetGroupsPublicInfo(out Dictionary<long, string> publicInfos, groupIds);

            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Console.WriteLine("Get groups' info in sync failed, error code is {0}.", errorCode);
            else
            {
                Console.WriteLine("Get groups' info in sync success");
                foreach (var kvp in publicInfos)
                    Console.WriteLine("-- group id: " + kvp.Key + " info: [" + kvp.Value + "]");
            }
        }
    }
}
