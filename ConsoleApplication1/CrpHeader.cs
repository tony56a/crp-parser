using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class CrpHeader
    {
        public ushort formatVersion;
        public string packageName;
        public string authorName;
        public uint pkgVersion;
        public string mainAssetName;
        public int numAssets;
        public long contentBeginIndex;
        public List<CrpAssetInfoHeader> assets;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (packageName != null)
            {
                builder.AppendFormat("Package Name:{0}\n", packageName);
            }
            if (authorName != null)
            {
                builder.AppendFormat("Package Author:{0}\n", authorName);
            }
            builder.AppendFormat("Package Format Version:{0}\n", formatVersion);
            builder.AppendFormat("Package Version:{0}\n", pkgVersion);

            if (mainAssetName != null)
            {
                builder.AppendFormat("Main Asset Name:{0}\n", mainAssetName);
            }
            builder.AppendFormat("Content Begin Index:{0}\n", contentBeginIndex);
            builder.AppendFormat("# of Assets:{0}\n", numAssets);
            if (assets != null)
            {
                foreach(CrpAssetInfoHeader info in assets)
                {
                    builder.AppendLine();
                    builder.Append(info);
                }
            }
            return builder.ToString();
        }
    }
}
