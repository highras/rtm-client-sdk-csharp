using System;

namespace example.common
{
    public class ErrorRecorder : com.fpnn.common.ErrorRecorder
    {
        public void RecordError(Exception e)
        {
            lock (this)
                Console.WriteLine("Exception: {0}", e);
        }
        public void RecordError(string message)
        {
            lock (this)
                Console.WriteLine("Error: {0}", message);
        }
        public void RecordError(string message, Exception e)
        {
            lock (this)
                Console.WriteLine("Error: {0}, exception: {1}", message, e);
        }
    }
}
