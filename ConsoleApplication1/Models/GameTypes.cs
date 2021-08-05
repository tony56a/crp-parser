using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication1
{
    public class Vector3
    {
        public System.Single x;
        public System.Single y;
        public System.Single z;

        public Vector3(System.Single x, System.Single y, System.Single z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override System.String ToString() => $"X:{x} Y:{x} Z:{x}\n";
    }

    public class Vector2
    {
        public System.Single x;
        public System.Single y;

        public Vector2(System.Single x, System.Single y)
        {
            this.x = x;
            this.y = y;
        }

        public override System.String ToString() => $"X:{x} Y:{x}\n";
    }

    public class Vector4
    {
        public System.Single x;
        public System.Single y;
        public System.Single z;
        public System.Single w;

        public Vector4(System.Single x, System.Single y, System.Single z, System.Single w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }

    public class Color
    {
        public System.Single r;
        public System.Single g;
        public System.Single b;
        public System.Single a;

        public Color(System.Single r, System.Single g, System.Single b, System.Single a)
        {
            this.r = r;
            this.b = g;
            this.g = b;
            this.a = a;
        }
    }

    public class Matrix4x4
    {
        public System.Single[] entries = new System.Single[16];
    }

    public class Boneweight
    {
        public System.Int32[] indicies = new System.Int32[4];
        public System.Single[] weights = new System.Single[4];
    }

    public class Transform
    {
        public Vector3 position;
        public Vector4 rotation;
        public Vector3 scale;
    }

    public class Mesh
    {
        public Matrix4x4[] bindPoses;
        public Boneweight[] boneWeights;
        public Color[] colors;
        public Vector3[] normals;
        public System.Int32 subMeshCount;
        public Vector4[] tangents;
        public List<System.Int32> triangles = new();
        public Vector2[] uv;
        public Vector3[] vertices;

        //Taken from:http://wiki.unity3d.com/index.php?title=ObjExporter
        public System.String ExportObj()
        {
            var sb = new StringBuilder();
            if (vertices != null)
            {
                foreach (var v in vertices)
                {
                    sb.Append($"v {v.x:0.000000000} {v.y:0.000000000} {v.z:0.000000000}\n");
                }
            }

            sb.Append('\n');
            if (normals != null)
            {
                foreach (var v in normals)
                {
                    sb.Append($"vn {v.x:0.000000000} {v.y:0.000000000} {v.z:0.000000000}\n");
                }
            }

            sb.Append('\n');
            if (uv != null)
            {
                foreach (var v in uv)
                {
                    sb.Append($"vt {v.x:0.000000000} {v.y:0.000000000}\n");
                }
            }

            if (subMeshCount != 0)
            {
                for (var i = 0; i < subMeshCount; i++)
                {
                    sb.Append($"g {i}");
                    sb.Append('\n');

                    for (var j = 0; j < triangles.Count; j += 3)
                    {
                        sb.Append($"f {triangles[j] + 1}/{triangles[j] + 1}/{triangles[j] + 1} {triangles[j + 1] + 1}/{triangles[j + 1] + 1}/{triangles[j + 1] + 1} {triangles[j + 2] + 1}/{triangles[j + 2] + 1}/{triangles[j + 2] + 1}\n");
                    }
                }
            }

            return sb.ToString();
        }
    }

    public class MaterialStub
    {
        public System.String shaderName;
        public System.Int32 numProperties;
        public Dictionary<System.String, Color> colors = new();
        public Dictionary<System.String, Vector4> vectors = new();
        public Dictionary<System.String, System.Single> floats = new();
        public Dictionary<System.String, System.String> textures = new();

    }

}
