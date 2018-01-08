using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Backup.Logic
{
    [DataContract]
    public class Archive
    {
        [DataMember]
        public string ArchiveId { get; set; }
        [DataMember]
        public string ArchiveDescription { get; set; }
        [DataMember]
        public string CreationDate { get; set; }
        [DataMember]
        public int Size { get; set; }
        [DataMember]
        public string SHA256TreeHash { get; set; }
    }

    /// <summary>
    /// AWS Glacier Inventory (JSON)
    /// </summary>
    [DataContract]
    public class Inventory
    {
        [DataMember]
        public string InventoryDate { get; set; }
        [DataMember]
        public Archive[] ArchiveList { get; set; }
    }

    [Serializable]
    public class ArchiveModel
    {
        public string Description { get; set; }
        public int Size { get; set; }
        public string ArchiveId { get; set; }
        public GlacierResult GlacierJobStatus { get; set; }
        public string ArchiveTopicFilePath { get; set; }
    }

    [Serializable]
    public class ArchiveModelList : List<ArchiveModel>
    {
        public DateTime InventoryDate { get; set; }
    }

}
