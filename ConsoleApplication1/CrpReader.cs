using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class CrpReader : BinaryReader
    {
        delegate dynamic CarpObjParser();

        Dictionary<String, CarpObjParser> singlarObjParser = new Dictionary<String, CarpObjParser> {};
        
        public CrpReader(Stream stream) : base(stream) {
            singlarObjParser["System.DateTime"] = () => 
            {
                return DateTime.Parse(this.ReadString(), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            };
            singlarObjParser["UnityEngine.Vector2"] = () => 
            {
                return new Vector2(this.ReadSingle(), this.ReadSingle());
            };
            singlarObjParser["UnityEngine.Vector3"] = () => 
            {
                return new Vector3(this.ReadSingle(), this.ReadSingle(), this.ReadSingle());
            };
            singlarObjParser["UnityEngine.Vector4"] = () =>
            {
                return new Vector4(this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle());
            };
            singlarObjParser["System.String"] = () =>
            {
                return this.ReadString();
            };
            singlarObjParser["System.Single"] = () => 
            {
                return this.ReadSingle();
            };
            singlarObjParser["System.Int32"] = () =>
            {
                return this.ReadInt32();
            };
            singlarObjParser["CustomAssetMetaData"] = () =>
            {
                return (CustomAssetMetaData.Type)(this.ReadInt32());
            };
            singlarObjParser["ModInfo"] = () =>
            {
                ModInfo info = new ModInfo();
                info.modName = this.ReadString();
                info.modWorkshopID = this.ReadUInt64();
                return info;
            };
            singlarObjParser["UnityEngine.Color"] = () =>
            {
                return new Vector4(this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle());
            };
            singlarObjParser["UnityEngine.BoneWeight"] = () =>
            {
                Boneweight boneWeight = new Boneweight();
                for (int i = 0; i < 4; i++)
                {
                    boneWeight.indicies[i] = this.ReadInt32();
                    boneWeight.weights[i] = this.ReadSingle();
                }
                return boneWeight;
            };
            singlarObjParser["UnityEngine.Matrix4x4"] = () =>
            {
                Matrix4x4 matrix = new Matrix4x4();
                for (int i = 0; i < 16; i++)
                {
                    matrix.entries[i] = this.ReadSingle();
                }
                return matrix;
            };
            singlarObjParser["ItemClass+Level"] = () =>
            {
                return (ItemClass.Level)(this.ReadInt32());
            };
            singlarObjParser["ItemClass+Service"] = () =>
            {
                return (ItemClass.Service)(this.ReadInt32());
            };
            singlarObjParser["ItemClass+SubService"] = () =>
            {
                return (ItemClass.SubService)(this.ReadInt32());
            };
            singlarObjParser["VehicleInfo+VehicleType"] = () =>
            {
                return (VehicleInfo.VehicleType)(this.ReadInt32());
            };
            singlarObjParser["SteamHelper+DLC_BitMask"] = () =>
            {
                return (SteamHelper.DLC_BitMask)(this.ReadInt32());
            };
        }

        public dynamic readUnityObj(AssemblyName name)
        {
            if (singlarObjParser.ContainsKey(name.FullName))
            {
                return singlarObjParser[name.FullName];
            }
            else
            {
                throw new KeyNotFoundException(String.Format("Type {0} cannot be parsed! Please file a bug report :(",name.FullName));
            }
        }

        public dynamic readUnityArray(AssemblyName name)
        {
            string strippedName = name.FullName.Replace("]", "").Replace("[","");
            int numEntries = this.ReadInt32();
            dynamic[] retVal = new dynamic[numEntries];
            if (singlarObjParser.ContainsKey(strippedName))
            {
                for(int i=0; i < numEntries; i++)
                {
                    retVal[i] = singlarObjParser[strippedName];
                }
                return retVal;
            }
            else
            {
                throw new KeyNotFoundException(String.Format("Type {0} cannot be parsed! Please file a bug report :(", name.FullName));
            }
        }

    }
}
