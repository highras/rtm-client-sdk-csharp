using System;
namespace com.fpnn.rtm
{
    internal class MidGenerator
    {
        static private long count = 0;
        static private object interLocker = new object();

        static public long Gen()
        {
            lock (interLocker)
            {
                if (count == 0)
                    count = ClientEngine.GetCurrentMilliseconds() % 1000;

                return ++count;
            }
        }
    }
}
