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
        public delegate dynamic CarpObjParser();

        public Dictionary<String, CarpObjParser> singlarObjParser = new Dictionary<String, CarpObjParser> {};
        
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
            singlarObjParser["System.Boolean"] = () =>
            {
                return this.ReadBoolean();
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
                return new Color(this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle());
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
            singlarObjParser["ItemClass"] = () =>
            {
                string retval = this.ReadString();
                this.ReadBoolean();
                return (retval);
            };
            singlarObjParser["ItemClass+Placement"] = () =>
            {
                return (ItemClass.Placement)(this.ReadInt32());
            };
            singlarObjParser["ItemClass+Availability"] = () =>
            {
                return (ItemClass.Availability)(this.ReadInt32());
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

            singlarObjParser["BuildingInfo+PlacementMode"] = () =>
            {
                return (BuildingInfo.PlacementMode)(this.ReadInt32());
            };
            singlarObjParser["BuildingInfo+ZoningMode"] = () =>
            {
                return (BuildingInfo.ZoningMode)(this.ReadInt32());
            };

            singlarObjParser["TerrainModify+Surface"] = () =>
            {
                return (TerrainModify.Surface)(this.ReadInt32());
            };

            singlarObjParser["BuildingInfo+Prop"] = () =>
            {
                Dictionary<string, dynamic> retVal = new Dictionary<string, dynamic>();
                retVal["enabled"] = this.ReadBoolean();
                retVal["m_position"] =singlarObjParser["UnityEngine.Vector3"]();
                retVal["m_angle"] = this.ReadSingle();
                retVal["m_probability"] = this.ReadInt32();
                retVal["m_fixedHeight"] = this.ReadBoolean();

                return (TerrainModify.Surface)(this.ReadInt32());
            };


            singlarObjParser["VehicleInfo+VehicleType"] = () =>
            {
                return (VehicleInfo.VehicleType)(this.ReadInt32());
            };

            singlarObjParser["SteamHelper+DLC_BitMask"] = () =>
            {
                return (SteamHelper.DLC_BitMask)(this.ReadInt32());
            };
            singlarObjParser["ColossalFramework.Packaging.Package+Asset"] = () =>
            {
                return this.ReadString();
            };
            singlarObjParser["CustomAssetMetaData+Type"] = () =>
            {
                return (CustomAssetMetaData.Type)(this.ReadInt32());
            };
            singlarObjParser["UnityEngine.Transform"] = () =>
            {
                Transform transform = new Transform();
                transform.position = new Vector3(this.ReadSingle(), this.ReadSingle(), this.ReadSingle());
                transform.rotation = new Vector4(this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle());
                transform.scale = new Vector3(this.ReadSingle(), this.ReadSingle(), this.ReadSingle());
                return transform;
            };
            singlarObjParser["UnityEngine.MeshFilter"] = () =>
            {
                return this.ReadString();
            };
            singlarObjParser["UnityEngine.MeshRenderer"] = () =>
            {
                int numEntries = this.ReadInt32();
                string[] retVal = new string[numEntries];
                for (int i = 0; i < numEntries; i++)
                {
                    retVal[i] = this.ReadString();
                }
                return retVal;
            };
            singlarObjParser["BuildingInfo"] = readGameInfo;
            singlarObjParser["PropInfo"] = readGameInfo;

        }

        public dynamic readGameInfo()
        {
            Dictionary<string, dynamic> retVal = new Dictionary<string, dynamic>();

            int numProperties = this.ReadInt32();
            for (int i = 0; i < numProperties; i++)
            {
                bool isNull = this.ReadBoolean();
                if (!isNull)
                {
                    string assemblyQualifiedName = this.ReadString();
                    string propertyType = assemblyQualifiedName.Split(new char[] { ',' })[0];
                    string propertyName = this.ReadString();
                    if (propertyType.Contains("[]"))
                    {
                        retVal[propertyName] = this.readUnityArray(propertyType);
                    }
                    else
                    {
                        retVal[propertyName] = this.readUnityObj(propertyType);
                    }

                }
            }

            return retVal;
        }

        public dynamic readUnityObj(string name)
        {
            if (singlarObjParser.ContainsKey(name))
            {
                return singlarObjParser[name]();
            }
            else
            {
                throw new KeyNotFoundException(String.Format("Type {0} cannot be parsed! Please file a bug report :(",name));
            }
        }

        public dynamic readUnityArray(string name)
        {
            string strippedName = name.Replace("]", "").Replace("[","");
            int numEntries = this.ReadInt32();
            dynamic[] tempArray = new dynamic[numEntries];
            if (singlarObjParser.ContainsKey(strippedName))
            {
                for(int i=0; i < numEntries; i++)
                {
                    tempArray[i] = singlarObjParser[strippedName]();
                }

                if(numEntries != 0)
                {
                    Type t = tempArray[0].GetType();
                    Array retVal = Array.CreateInstance(t, numEntries);
                    for (int i = 0; i < numEntries; i++)
                    {
                        retVal.SetValue(Convert.ChangeType(tempArray[i], t), i);
                    }

                    return retVal;
                }
                else
                {
                    return null;
                }
                
            }
            else
            {
                throw new KeyNotFoundException(String.Format("Type {0} cannot be parsed! Please file a bug report :(", name));
            }
        }

    }
}
