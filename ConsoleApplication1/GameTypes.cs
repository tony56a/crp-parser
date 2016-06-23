using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{

    public class Vector3
    {
        public float x;
        public float y;
        public float z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public class Vector2
    {
        public float x;
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class Vector4
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }

    public class Matrix4x4
    {
        public float[] entries = new float[16];
    }

    public class Boneweight
    {
        public int[] indicies = new int[4];
        public float[] weights = new float[4];
    }

    public class Mesh
    {
        public Matrix4x4[] bindPoses;
        public Boneweight[] boneWeights;
        public Vector4[] colors;
        public Vector3[] normals;
        public int subMeshCount;
        public Vector4[] tangents;
        public int[] triangles;
        public Vector2[] uv;
        public Vector3[] vertices;

    }

}
