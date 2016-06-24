using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class Consts
    {
        public static readonly string MAGICSTR = "CRAP";

        public enum AssetTypeMapping {
            unknown,
            obj,
            material,
            texture,
            staticMesh,
            text = 50,
            assembly = 51,
            data = 52,
            package = 53,
            locale = 80,
            user = 100,
            userSaveGame = 101,
            userCustomMap = 102,
            userMetaData = 103,
            userLut = 104,
            userDistrictStyle = 105,
            userMapTheme = 106,
            userMapThemeMap = 107,
        }

    }
}
