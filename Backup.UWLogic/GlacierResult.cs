using System;
using System.Runtime.Serialization;

namespace Backup.UWLogic
{
    [DataContract]
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
