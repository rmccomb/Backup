using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Backup.UWLogic
{
    [DataContract]
    public class InventoryTopic
    {
        [DataMember]
        public string TopicARN { get; set; }
        [DataMember]
        public string JobId { get; set; }
        [DataMember]
        public string QueueARN { get; set; }
        [DataMember]
        public string QueueUrl { get; internal set; }
    }
}
