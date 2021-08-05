using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication1
{
    public class CrpHeader
    {
        public System.UInt16 formatVersion;
        public System.String packageName;
        public System.String authorName;
        public System.UInt32 pkgVersion;
        public System.String mainAssetName;
        public System.Int32 numAssets;
        public System.Int64 contentBeginIndex;
        public System.Boolean isLut;
        public List<CrpAssetInfoHeader> assets;

        public override System.String ToString()
        {
            var builder = new StringBuilder();
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
                foreach (var info in assets)
                {
                    builder.AppendLine();
                    builder.Append(info);
                }
            }
            return builder.ToString();
        }
    }
}
