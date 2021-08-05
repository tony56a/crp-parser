using System.Text;

namespace ConsoleApplication1
{
    public class CrpAssetInfoHeader
    {
        public System.String assetName;
        public System.String assetChecksum;
        public Consts.AssetTypeMapping assetType;
        public System.Int64 assetOffsetBegin;
        public System.Int64 assetSize;

        public override System.String ToString()
        {

            var builder = new StringBuilder();
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
