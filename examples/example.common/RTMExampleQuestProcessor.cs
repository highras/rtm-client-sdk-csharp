﻿using System;
using com.fpnn.rtm;
namespace example.common
{
    public class RTMExampleQuestProcessor: RTMQuestProcessor
    {
        public RTMExampleQuestProcessor()
        {
        }

        //----------------[ System Events ]-----------------//

        public override void SessionClosed(int ClosedByErrorCode)
        {
            lock (this)
                Console.WriteLine($"Session closed by error code: {ClosedByErrorCode}");
        }

        public override bool ReloginWillStart(int lastErrorCode, int retriedCount)
        {
            lock (this)
                Console.WriteLine($"Relogin will start. Last error code is {lastErrorCode}, total relogin count is {retriedCount}.");

            return true;
        }

        public override void ReloginCompleted(bool successful, bool retryAgain, int errorCode, int retriedCount)
        {
            lock (this)
            {
                if (successful)
                    Console.WriteLine("Relogin Completed. Relogin succeeded, total relogin count is " + retriedCount);
                else
                    Console.WriteLine("Relogin Completed. Relogin failed, error code: {0}, will{1} retry again. Total relogin count is {2}.",
                        errorCode, retryAgain ? "" : " not", retriedCount);
            }
        }

        public override void Kickout()
        {
            lock (this)
                Console.WriteLine("Received kickout.");
        }

        public override void KickoutRoom(long roomId)
        {
            lock (this)
                Console.WriteLine($"Kickout from room {roomId}");
        }

        //----------------[ Message Interfaces ]-----------------//

        //-- Messages
        public override void PushMessage(RTMMessage message)
        {
            lock (this)
            {
                if (message.binaryMessage == null)
                    Console.WriteLine($"Receive push message: from {message.fromUid}, " +
                        $"type: {message.messageType}, mid: {message.messageId}, " +
                        $"attrs: {message.attrs}, message: {message.stringMessage}");
                else
                    Console.WriteLine($"Receive push binary message: from {message.fromUid}, " +
                        $"type: {message.messageType}, mid: {message.messageId}, " +
                        $"attrs: {message.attrs}, message length: {message.binaryMessage.Length}");
            }
        }
        public override void PushGroupMessage(RTMMessage message)
        {
            lock (this)
            {
                if (message.binaryMessage == null)
                    Console.WriteLine($"Receive push group message: from {message.fromUid}, in group {message.toId}, " +
                        $"type: {message.messageType}, mid: {message.messageId}, " +
                        $"attrs: {message.attrs}, message: {message.stringMessage}");
                else
                    Console.WriteLine($"Receive push group binary message: from {message.fromUid}, in group {message.toId}, " +
                        $"type: {message.messageType}, mid: {message.messageId}, " +
                        $"attrs: {message.attrs}, message length: {message.binaryMessage.Length}");
            }
        }
        public override void PushRoomMessage(RTMMessage message)
        {
            lock (this)
            {
                if (message.binaryMessage == null)
                    Console.WriteLine($"Receive push room message: from {message.fromUid}, in room {message.toId}, " +
                        $"type: {message.messageType}, mid: {message.messageId}, " +
                        $"attrs: {message.attrs}, message: {message.stringMessage}");
                else
                    Console.WriteLine($"Receive push room binary message: from {message.fromUid}, in room {message.toId}, " +
                        $"type: {message.messageType}, mid: {message.messageId}, " +
                        $"attrs: {message.attrs}, message length: {message.binaryMessage.Length}");
            }
        }
        public override void PushBroadcastMessage(RTMMessage message)
        {
            lock (this)
            {
                if (message.binaryMessage == null)
                    Console.WriteLine($"Receive push broadcast message: from {message.fromUid}, " +
                        $"type: {message.messageType}, mid: {message.messageId}, " +
                        $"attrs: {message.attrs}, message: {message.stringMessage}");
                else
                    Console.WriteLine($"Receive push broadcast binary message: from {message.fromUid}, " +
                        $"type: {message.messageType}, mid: {message.messageId}, " +
                        $"attrs: {message.attrs}, message length: {message.binaryMessage.Length}");
            }
        }

        //-- Chat
        public override void PushChat(RTMMessage message)
        {
            lock (this)
            {
                Console.WriteLine($"Receive push chat: from {message.fromUid}, " +
                    $"mid: {message.messageId}, attrs: {message.attrs}, " +
                    $"from language {message.translatedInfo.sourceLanguage} " +
                    $"content '{message.translatedInfo.sourceText}' to " +
                    $"language {message.translatedInfo.targetLanguage} " +
                    $"content '{message.translatedInfo.targetText}'.");
            }
        }
        public override void PushGroupChat(RTMMessage message)
        {
            lock (this)
            {
                Console.WriteLine($"Receive push group chat: from {message.fromUid}, " +
                    $"in group {message.toId}, mid: {message.messageId}, attrs: {message.attrs}, " +
                    $"from language {message.translatedInfo.sourceLanguage} " +
                    $"content '{message.translatedInfo.sourceText}' to " +
                    $"language {message.translatedInfo.targetLanguage} " +
                    $"content '{message.translatedInfo.targetText}'.");
            }
        }
        public override void PushRoomChat(RTMMessage message)
        {
            lock (this)
            {
                Console.WriteLine($"Receive push room chat: from {message.fromUid}, " +
                    $"in room {message.toId}, mid: {message.messageId}, attrs: {message.attrs}, " +
                    $"from language {message.translatedInfo.sourceLanguage} " +
                    $"content '{message.translatedInfo.sourceText}' to " +
                    $"language {message.translatedInfo.targetLanguage} " +
                    $"content '{message.translatedInfo.targetText}'.");
            }
        }
        public override void PushBroadcastChat(RTMMessage message)
        {
            lock (this)
            {
                Console.WriteLine($"Receive push broadcast chat: from {message.fromUid}, " +
                    $"mid: {message.messageId}, attrs: {message.attrs}, " +
                    $"from language {message.translatedInfo.sourceLanguage} " +
                    $"content '{message.translatedInfo.sourceText}' to " +
                    $"language {message.translatedInfo.targetLanguage} " +
                    $"content '{message.translatedInfo.targetText}'.");
            }
        }

        //-- Cmd
        public override void PushCmd(RTMMessage message)
        {
            lock (this)
            {
                Console.WriteLine($"Receive push cmd: from {message.fromUid}, " +
                    $"mid: {message.messageId}, attrs: {message.attrs}, " +
                    $"message {message.stringMessage}.");
            }
        }
        public override void PushGroupCmd(RTMMessage message)
        {
            lock (this)
            {
                Console.WriteLine($"Receive push group cmd: from {message.fromUid}, " +
                    $"in group {message.toId}, mid: {message.messageId}, attrs: {message.attrs}, " +
                    $"message {message.stringMessage}.");
            }
        }
        public override void PushRoomCmd(RTMMessage message)
        {
            lock (this)
            {
                Console.WriteLine($"Receive push room cmd: from {message.fromUid}, " +
                    $"in room {message.toId}, mid: {message.messageId}, attrs: {message.attrs}, " +
                    $"message {message.stringMessage}.");
            }
        }
        public override void PushBroadcastCmd(RTMMessage message)
        {
            lock (this)
            {
                Console.WriteLine($"Receive push broadcast cmd: from {message.fromUid}, " +
                    $"mid: {message.messageId}, attrs: {message.attrs}, " +
                    $"message {message.stringMessage}.");
            }
        }

        //-- Files
        public override void PushFile(RTMMessage message)
        {
            lock (this)
            {
                Console.WriteLine($"Receive push file: from {message.fromUid}, " +
                    $"type: {message.messageType}, mid: {message.messageId}," +
                    $"attrs: {message.attrs}, url: {message.fileInfo.url}, size: {message.fileInfo.size}.");

                if (message.fileInfo.isRTMAudio)
                    Console.WriteLine($" -- [RTM Audio] language: {message.fileInfo.language}, duration: {message.fileInfo.duration}");
            }
        }
        public override void PushGroupFile(RTMMessage message)
        {
            lock (this)
            {
                Console.WriteLine($"Receive push group file: from {message.fromUid}, in group {message.toId}, " +
                    $"type: {message.messageType}, mid: {message.messageId}," +
                    $"attrs: {message.attrs}, url: {message.fileInfo.url}, size: {message.fileInfo.size}.");

                if (message.fileInfo.isRTMAudio)
                    Console.WriteLine($" -- [RTM Audio] language: {message.fileInfo.language}, duration: {message.fileInfo.duration}");
            }
        }
        public override void PushRoomFile(RTMMessage message)
        {
            lock (this)
            {
                Console.WriteLine($"Receive push room file: from {message.fromUid}, in room {message.toId}, " +
                    $"type: {message.messageType}, mid: {message.messageId}," +
                    $"attrs: {message.attrs}, url: {message.fileInfo.url}, size: {message.fileInfo.size}.");

                if (message.fileInfo.isRTMAudio)
                    Console.WriteLine($" -- [RTM Audio] language: {message.fileInfo.language}, duration: {message.fileInfo.duration}");
            }
        }
        public override void PushBroadcastFile(RTMMessage message)
        {
            lock (this)
            {
                Console.WriteLine($"Receive push broadcast file: from {message.fromUid}, " +
                    $"type: {message.messageType}, mid: {message.messageId}," +
                    $"attrs: {message.attrs}, url: {message.fileInfo.url}, size: {message.fileInfo.size}.");

                if (message.fileInfo.isRTMAudio)
                    Console.WriteLine($" -- [RTM Audio] language: {message.fileInfo.language}, duration: {message.fileInfo.duration}");
            }
        }
    }
}
