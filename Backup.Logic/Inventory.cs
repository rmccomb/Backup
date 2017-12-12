using System;
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
    //\"InventoryRetrievalParameters\"
    //    :{\"EndDate\":null,
    //      \"Format\":\"JSON\",
    //      \"Limit\":null,
    //      \"Marker\":null,
    //      \"StartDate\":null
    //    },
    //    \"InventorySizeInBytes\":811,
    //    \"JobDescription\":\"This job is to download a vault inventory.\",
    //    \"JobId\":\"gz4vCTUTRazp_2II86GPgWY-FEneRCEuRq8hBhueN2Dtxoaa17Om-c1wJ8egVAfdwN5kgOyEdR6_y_mRE3AROz4fzQMf\",
    //    \"RetrievalByteRange\":null,
    //    \"SHA256TreeHash\":null,
    //    \"SNSTopic\":\"arn:aws:sns:ap-southeast-2:019910574325:GlacierDownload-636484317871706400\",
    //    \"StatusCode\":\"Succeeded\",
    //    \"StatusMessage\":\"Succeeded\",
    //    \"Tier\":null,
    //    \"VaultARN\":\"arn:aws:glacier:ap-southeast-2:019910574325:vaults/rmccomb-backup\"
    //    }",

    [DataContract]
    public class Inventory
    {
        [DataMember]
        public string InventoryDate { get; set; }
        [DataMember]
        public Archive[] ArchiveList { get; set; }
    }

    public class InventoryResult
    {
        public string Result { get; set; }
        public Inventory Inventory { get; set; }

    }
}
