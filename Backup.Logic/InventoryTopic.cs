using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup.Logic
{
    [Serializable]
    class InventoryTopic
    {
        public string TopicARN { get; set; }
        public string JobId { get; set; }
        public string QueueARN { get; set; }
        public string QueueUrl { get; internal set; }
    }
}
