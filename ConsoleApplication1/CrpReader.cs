using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ConsoleApplication1
{
    public class CrpReader : BinaryReader
    {
        public delegate dynamic CarpObjParser();

        public Dictionary<String, CarpObjParser> singlarObjParser = new() { };

        public CrpReader(Stream stream) : base(stream)
        {
            singlarObjParser["System.DateTime"] = () =>
            {
                return DateTime.Parse(ReadString(), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            };
            singlarObjParser["UnityEngine.Vector2"] = () =>
            {
                return new Vector2(ReadSingle(), ReadSingle());
            };
            singlarObjParser["UnityEngine.Vector3"] = () =>
            {
                return new Vector3(ReadSingle(), ReadSingle(), ReadSingle());
            };
            singlarObjParser["UnityEngine.Vector4"] = () =>
            {
                return new Vector4(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
            };
            singlarObjParser["System.String"] = () =>
            {
                return ReadString();
            };
            singlarObjParser["System.Single"] = () =>
            {
                return ReadSingle();
            };
            singlarObjParser["System.Int32"] = () =>
            {
                return ReadInt32();
            };
            singlarObjParser["System.Boolean"] = () =>
            {
                return ReadBoolean();
            };
            singlarObjParser["CustomAssetMetaData"] = () =>
            {
                return (CustomAssetMetaData.Type)(ReadInt32());
            };
            singlarObjParser["ModInfo"] = () =>
            {
                var info = new ModInfo();
                info.modName = ReadString();
                info.modWorkshopID = ReadUInt64();
                return info;
            };
            singlarObjParser["UnityEngine.Color"] = () =>
            {
                return new Color(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
            };
            singlarObjParser["UnityEngine.BoneWeight"] = () =>
            {
                var boneWeight = new Boneweight();
                for (var i = 0; i < 4; i++)
                {
                    boneWeight.indicies[i] = ReadInt32();
                    boneWeight.weights[i] = ReadSingle();
                }
                return boneWeight;
            };
            singlarObjParser["UnityEngine.Matrix4x4"] = () =>
            {
                var matrix = new Matrix4x4();
                for (var i = 0; i < 16; i++)
                {
                    matrix.entries[i] = ReadSingle();
                }
                return matrix;
            };
            singlarObjParser["ItemClass"] = () =>
            {
                var retval = ReadString();
                ReadBoolean();
                return (retval);
            };
            singlarObjParser["ItemClass+Placement"] = () =>
            {
                return (ItemClass.Placement)(ReadInt32());
            };
            singlarObjParser["ItemClass+Availability"] = () =>
            {
                return (ItemClass.Availability)(ReadInt32());
            };
            singlarObjParser["ItemClass+Level"] = () =>
            {
                return (ItemClass.Level)(ReadInt32());
            };
            singlarObjParser["ItemClass+Service"] = () =>
            {
                return (ItemClass.Service)(ReadInt32());
            };
            singlarObjParser["ItemClass+SubService"] = () =>
            {
                return (ItemClass.SubService)(ReadInt32());
            };

            singlarObjParser["BuildingInfo+PlacementMode"] = () =>
            {
                return (BuildingInfo.PlacementMode)(ReadInt32());
            };
            singlarObjParser["BuildingInfo+ZoningMode"] = () =>
            {
                return (BuildingInfo.ZoningMode)(ReadInt32());
            };

            singlarObjParser["TerrainModify+Surface"] = () =>
            {
                return (TerrainModify.Surface)(ReadInt32());
            };
            singlarObjParser["VehicleInfo+MeshInfo"] = () =>
            {

                var retVal = new Dictionary<String, dynamic>();
                retVal["checksum"] = ReadString();
                retVal["m_parkedFlagsForbidden"] = (VehicleParked.Flags)ReadInt32();
                retVal["m_parkedFlagsRequired"] = (VehicleParked.Flags)ReadInt32();
                retVal["m_vehicleFlagsForbidden"] = (Vehicle.Flags)ReadInt32();
                retVal["m_vehicleFlagsRequired"] = (Vehicle.Flags)ReadInt32();
                return retVal;
            };
            singlarObjParser["BuildingInfo+Prop"] = () =>
            {
                var retVal = new Dictionary<String, dynamic>();
                retVal["m_prop"] = ReadString();
                retVal["m_tree"] = ReadString();
                retVal["m_position"] = singlarObjParser["UnityEngine.Vector3"]();
                retVal["m_angle"] = ReadSingle();
                retVal["m_probability"] = ReadInt32();
                retVal["m_fixedHeight"] = ReadBoolean();
                return retVal;
            };
            singlarObjParser["BuildingInfo+MeshInfo"] = () =>
            {

                var retVal = new Dictionary<String, dynamic>();
                retVal["m_subInfo"] = ReadString();
                retVal["m_flagsForbidden"] = (Building.Flags)(ReadInt32());
                retVal["m_flagsRequired"] = (Building.Flags)(ReadInt32());
                retVal["m_position"] = singlarObjParser["UnityEngine.Vector3"]();
                retVal["m_angle"] = ReadSingle();
                return retVal;
            };
            singlarObjParser["BuildingInfo+PathInfo"] = () =>
            {
                var retVal = new Dictionary<String, dynamic>();
                retVal["m_netInfo"] = ReadString();
                retVal["m_nodes"] = readUnityArray("UnityEngine.Vector3[]");
                retVal["m_curveTargets"] = readUnityArray("UnityEngine.Vector3[]");
                retVal["m_invertSegments"] = ReadBoolean();
                retVal["m_maxSnapDistance"] = ReadSingle();
                return retVal;
            };
            singlarObjParser["BuildingInfo+SubInfo"] = () =>
            {
                var retVal = new Dictionary<String, dynamic>();
                retVal["m_buildingInfo"] = ReadString();
                retVal["m_position"] = singlarObjParser["UnityEngine.Vector3"]();
                retVal["m_angle"] = ReadSingle();
                retVal["m_fixedHeight"] = ReadBoolean();
                return retVal;
            };
            singlarObjParser["PropInfo+DoorType"] = () =>
            {
                return (PropInfo.DoorType)ReadInt32();
            };
            singlarObjParser["PropInfo+Variation"] = () =>
            {
                var retVal = new Dictionary<String, dynamic>();
                retVal["m_prop"] = ReadString();
                retVal["m_probability"] = ReadInt32();
                return retVal;
            };
            singlarObjParser["PropInfo+ParkingSpace"] = () =>
            {
                var retVal = new Dictionary<String, dynamic>();
                retVal["m_position"] = singlarObjParser["UnityEngine.Vector3"]();
                retVal["m_angle"] = singlarObjParser["UnityEngine.Vector3"]();
                retVal["m_size"] = singlarObjParser["UnityEngine.Vector3"]();
                return retVal;
            };
            singlarObjParser["PropInfo+Effect"] = () =>
            {
                var retVal = new Dictionary<String, dynamic>();
                retVal["m_effect"] = ReadString();
                retVal["m_position"] = singlarObjParser["UnityEngine.Vector3"]();
                retVal["m_direction"] = singlarObjParser["UnityEngine.Vector3"]();
                return retVal;
            };
            singlarObjParser["DepotAI+SpawnPoint"] = () =>
            {
                var retVal = new Dictionary<String, dynamic>();
                retVal["m_position"] = singlarObjParser["UnityEngine.Vector3"]();
                retVal["m_target"] = singlarObjParser["UnityEngine.Vector3"]();
                return retVal;
            };
            singlarObjParser["PropInfo+SpecialPlace"] = () =>
            {
                var retVal = new Dictionary<String, dynamic>();
                retVal["m_specialFlags"] = ReadInt32();
                retVal["m_position"] = singlarObjParser["UnityEngine.Vector3"]();
                retVal["m_direction"] = singlarObjParser["UnityEngine.Vector3"]();
                return retVal;
            };

            singlarObjParser["TransportInfo"] = () =>
            {
                var retVal = new Dictionary<String, dynamic>();
                retVal["m_transportInfo"] = ReadString();
                return retVal;
            };
            singlarObjParser["ItemClass"] = () =>
            {
                var retVal = new Dictionary<String, dynamic>();
                retVal["m_itemClass"] = ReadString();
                return retVal;
            };
            singlarObjParser["MessageInfo"] = () =>
            {
                var retVal = new Dictionary<String, dynamic>();
                retVal["firstId1"] = ReadString();
                retVal["firstId2"] = ReadString();
                retVal["repeatId1"] = ReadString();
                retVal["repeatId2"] = ReadString();
                return retVal;
            };
            singlarObjParser["VehicleParked+Flags"] = () =>
            {
                return (VehicleParked.Flags)(ReadInt32());
            };
            singlarObjParser["Vehicle+Flags"] = () =>
            {
                return (Vehicle.Flags)(ReadInt32());
            };
            singlarObjParser["Building+Flags"] = () =>
            {
                return (Building.Flags)(ReadInt32());
            };
            singlarObjParser["VehicleInfo+VehicleType"] = () =>
            {
                return (VehicleInfo.VehicleType)(ReadInt32());
            };

            singlarObjParser["VehicleInfo+Effect"] = () =>
            {
                var retVal = new Dictionary<String, dynamic>();
                retVal["m_effect"] = ReadString();
                retVal["m_parkedFlagsForbidden"] = (VehicleParked.Flags)ReadInt32();
                retVal["m_parkedFlagsRequired"] = (VehicleParked.Flags)ReadInt32();
                retVal["m_vehicleFlagsForbidden"] = (Vehicle.Flags)ReadInt32();
                retVal["m_vehicleFlagsRequired"] = (Vehicle.Flags)ReadInt32();
                return retVal;
            };
            singlarObjParser["VehicleInfo+VehicleDoor"] = () =>
            {
                var retVal = new Dictionary<String, dynamic>();
                retVal["m_name"] = ReadString();
                retVal["m_position"] = singlarObjParser["UnityEngine.Vector3"]();
                return retVal;
            };
            singlarObjParser["VehicleInfo+VehicleTrailer"] = () =>
            {
                var retVal = new Dictionary<String, dynamic>();
                retVal["m_type"] = ReadInt32();
                retVal["m_probability"] = ReadInt32();
                retVal["m_invertprobability"] = ReadInt32();
                return retVal;
            };
            singlarObjParser["SteamHelper+DLC_BitMask"] = () =>
            {
                return (SteamHelper.DLC_BitMask)(ReadInt32());
            };
            singlarObjParser["ColossalFramework.Packaging.Package+Asset"] = () =>
            {
                return ReadString();
            };
            singlarObjParser["CustomAssetMetaData+Type"] = () =>
            {
                return (CustomAssetMetaData.Type)(ReadInt32());
            };
            singlarObjParser["UnityEngine.Transform"] = () =>
            {
                var transform = new Transform();
                transform.position = new Vector3(ReadSingle(), ReadSingle(), ReadSingle());
                transform.rotation = new Vector4(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
                transform.scale = new Vector3(ReadSingle(), ReadSingle(), ReadSingle());
                return transform;
            };
            singlarObjParser["UnityEngine.MeshFilter"] = () =>
            {
                return ReadString();
            };
            singlarObjParser["UnityEngine.MeshRenderer"] = () =>
            {
                var numEntries = ReadInt32();
                var retVal = new String[numEntries];
                for (var i = 0; i < numEntries; i++)
                {
                    retVal[i] = ReadString();
                }
                return retVal;
            };

            singlarObjParser["VehicleInfo"] = readGameInfo;
            singlarObjParser["BuildingInfo"] = readGameInfo;
            singlarObjParser["PropInfo"] = readGameInfo;
            singlarObjParser["BuildingInfoGen"] = () =>
            {
                return ReadString();
            };
            singlarObjParser["PropInfoGen"] = () =>
            {
                return ReadString();
            };
            singlarObjParser["VehicleInfoGen"] = () =>
            {
                return ReadString();
            };
            singlarObjParser["UnityEngine.GameObject"] = () =>
            {
                var retVal = new Dictionary<String, dynamic>();

                retVal["tag"] = ReadString();
                retVal["enabled"] = ReadBoolean();

                return retVal;
            };
            singlarObjParser["ResidentialBuildingAI"] = readGameInfo;
            singlarObjParser["CommercialBuildingAI"] = readGameInfo;
            singlarObjParser["OfficeBuildingAI"] = readGameInfo;
            singlarObjParser["IndustrialBuildingAI"] = readGameInfo;
            singlarObjParser["IndustrialExtractorAI"] = readGameInfo;
            singlarObjParser["IndustrialBuildingAI"] = readGameInfo;
            singlarObjParser["LivestockExtractorAI"] = readGameInfo;

        }

        public dynamic readGameInfo()
        {
            var retVal = new Dictionary<String, dynamic>();

            var numProperties = ReadInt32();
            for (var i = 0; i < numProperties; i++)
            {
                var isNull = ReadBoolean();
                if (!isNull)
                {
                    var assemblyQualifiedName = ReadString();
                    var propertyType = assemblyQualifiedName.Split(new Char[] { ',' })[0];
                    var propertyName = ReadString();

                    try
                    {
                        if (propertyType.Contains("[]"))
                        {
                            retVal[propertyName] = readUnityArray(propertyType);
                        }
                        else
                        {
                            retVal[propertyName] = readUnityObj(propertyType);
                        }
                    }
                    catch (Exception e)
                    {
                        retVal["Error"] = e.Message;
                        return retVal;
                    }
                }
            }

            return retVal;
        }

        public dynamic readUnityObj(String name)
        {
            if (singlarObjParser.ContainsKey(name))
            {
                return singlarObjParser[name]();
            }
            else
            {
                throw new KeyNotFoundException(String.Format("Type {0} cannot be parsed! Please file a bug report :(", name));
            }
        }

        public dynamic readUnityArray(String name)
        {
            var strippedName = name.Replace("]", "").Replace("[", "");
            var numEntries = ReadInt32();
            var tempArray = new dynamic[numEntries];
            if (singlarObjParser.ContainsKey(strippedName))
            {
                for (var i = 0; i < numEntries; i++)
                {
                    tempArray[i] = singlarObjParser[strippedName]();
                }

                if (numEntries != 0)
                {
                    Type t = tempArray[0].GetType();
                    var retVal = Array.CreateInstance(t, numEntries);
                    for (var i = 0; i < numEntries; i++)
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
