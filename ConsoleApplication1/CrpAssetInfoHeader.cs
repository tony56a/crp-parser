using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class CrpAssetInfoHeader
    {
        public string assetName;
        public string assetChecksum;
        public Consts.AssetTypeMapping assetType;
        public long assetOffsetBegin;
        public long assetSize;

        public override string ToString()
        {

            StringBuilder builder = new StringBuilder();
            if (assetName != null)
            {
                builder.AppendFormat("Asset Name:{0}\n", assetName);
            }
            if (assetChecksum != null)
            {
                builder.AppendFormat("Asset Checksum:{0}\n", assetChecksum);
            }
            builder.AppendFormat("Asset Type:{0}\n", assetType.ToString());
            builder.AppendFormat("Asset Offset Begin:{0}\n", assetOffsetBegin);
            builder.AppendFormat("Asset Size:{0}\n", assetSize);

            return builder.ToString();
        }        
    }
}
