using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Backup.UWLogic
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

    [DataContract]
    public class ArchiveModel
    {
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int Size { get; set; }
        [DataMember]
        public string ArchiveId { get; set; }
        [DataMember]
        public GlacierResult GlacierJobStatus { get; set; }
        [DataMember]
        public string ArchiveTopicFilePath { get; set; }
    }

    [DataContract]
    public class ArchiveModelList : List<ArchiveModel>
    {
        [DataMember]
        public DateTime InventoryDate { get; set; }
    }

}
