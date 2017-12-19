using System;

namespace Backup.Logic
{
    [Serializable]
    public enum GlacierResult
    {
        Unknown,
        DownloadRequested,
        InventoryRequested,
        Ready,
        Error,
        Completed,
        JobFailed,
        Incomplete,
        JobInProgress
    }
}
