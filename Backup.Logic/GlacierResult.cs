using System;

namespace Backup.Logic
{
    [Serializable]
    public enum GlacierResult
    {
        NoJob,
        JobRequested,
        Ready,
        Error,
        Completed,
        JobFailed,
        Incomplete,
        JobInProgress
    }
}
