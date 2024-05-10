using System.Runtime.ExceptionServices;

namespace Cybercraft.Common
{
    public static class Exception
    {
        public static void Rethrow(System.Exception ex)
        {
            ExceptionDispatchInfo.Capture(ex).Throw();
        }
    }
}
