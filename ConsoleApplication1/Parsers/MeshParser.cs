using System;
using System.IO;

namespace ConsoleApplication1.Parsers
{
    class MeshParser
    {
        public static Mesh ParseMesh(CrpReader reader, Boolean saveFile, String saveFileName, Int64 fileSize, Boolean verbose)
        {
            var fileContentBegin = reader.BaseStream.Position;

            var retVal = new Mesh
            {
                vertices = reader.readUnityArray("UnityEngine.Vector3"),
                colors = reader.readUnityArray("UnityEngine.Color"),
                uv = reader.readUnityArray("UnityEngine.Vector2"),
                normals = reader.readUnityArray("UnityEngine.Vector3"),
                tangents = reader.readUnityArray("UnityEngine.Vector4"),
                boneWeights = reader.readUnityArray("UnityEngine.BoneWeight"),
                bindPoses = reader.readUnityArray("UnityEngine.Matrix4x4"),
                subMeshCount = reader.ReadInt32()
            };
            for (var i = 0; i < retVal.subMeshCount; i++)
            {
                Int32[] triangles = reader.readUnityArray("System.Int32");
                retVal.triangles.AddRange(triangles);
            }

            if ((reader.BaseStream.Position - fileContentBegin) != fileSize)
            {
                var bytesToRead = (Int32)(fileSize - (reader.BaseStream.Position - fileContentBegin));
                reader.ReadBytes(bytesToRead);
            }
            var fileName = saveFileName + ".obj";
            if (verbose)
            {
                Console.WriteLine("Read {0} bytes into image file {1}", (reader.BaseStream.Position - fileContentBegin), fileName);
            }
            if (saveFile)
            {
                using (var stream = new FileStream(fileName, FileMode.Create))
                {
                    var file = new StreamWriter(stream);
                    file.Write(retVal.ExportObj());
                }
            }
            return retVal;
        }

    }
}
