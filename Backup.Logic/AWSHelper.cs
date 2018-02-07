using Amazon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup.Logic
{
    [Serializable]
    public class AWSRegionEndPoint
    {
        public AWSRegionEndPoint(string systemName, string displayName)
        {
            this.SystemName = systemName;
            this.DisplayName = displayName;
        }
        public string SystemName { get; set; }
        public string DisplayName { get; set; }

        public override bool Equals(object obj)
        {
            var point = obj as AWSRegionEndPoint;
            return point != null &&
                   SystemName == point.SystemName;
        }

        public override int GetHashCode()
        {
            return -283799761 + EqualityComparer<string>.Default.GetHashCode(SystemName);
        }

        static public bool operator==(AWSRegionEndPoint a, AWSRegionEndPoint b)
        {
            if ((object)b == null)
                return false;

            return (object)a != null && (object)b != null 
                && a.SystemName == b.SystemName;
        }
        static public bool operator !=(AWSRegionEndPoint a, AWSRegionEndPoint b)
        {
            if ((object)a != null && (object)b == null)
                return true;

            return a.SystemName != b.SystemName;
        }
    }

    public class AWSHelper
    {
        public static AWSRegionEndPoint[] GetAllRegions()
        {
            var regions = RegionEndpoint.EnumerableAllRegions;
            var endpoints = regions.Select(r => new AWSRegionEndPoint(r.SystemName, r.DisplayName));
            return endpoints.ToArray();
        }

        public static string GetDefaultRegionSystemName()
        {
            return RegionEndpoint.APSoutheast2.SystemName;
        }

        public static AWSRegionEndPoint GetDefaultRegionEndPoint()
        {
            var endpoint =  RegionEndpoint.EnumerableAllRegions.First(n => n.SystemName == GetDefaultRegionSystemName());
            return new AWSRegionEndPoint(endpoint.SystemName, endpoint.DisplayName);
        }
    }
}
