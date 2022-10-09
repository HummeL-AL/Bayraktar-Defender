using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MeshCombiner
{
    public Mesh CombineMeshes(Mesh[] meshes, Matrix4x4[] transforms)
    {
        CombineInstance[] combineInstances = new CombineInstance[meshes.Length];
        for(int i = 0; i < meshes.Length; i++)
        {
            CombineInstance instance = new CombineInstance();
            instance.mesh = meshes[i];
            instance.transform = transforms[i];

            combineInstances[i] = instance;
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.indexFormat = IndexFormat.UInt32;
        combinedMesh.CombineMeshes(combineInstances, true, true);

        //combinedMesh.indexFormat = IndexFormat.UInt32;

        //List<Vector3> vertices = new List<Vector3>();
        //List<Vector3> normals = new List<Vector3>();
        //List<int> triangles = new List<int>();

        //for (int i = 0; i < meshes.Length; i++)
        //{
        //    Mesh mesh = meshes[i];
        //    int objectVertexCount = vertices.Count;

        //    foreach (Vector3 vertex in mesh.vertices)
        //    {
        //        vertices.Add(positions[i] + vertex);
        //    }

        //    foreach (Vector3 normal in mesh.normals)
        //    {
        //        normals.Add(normal);
        //    }

        //    foreach (int vertexIndex in mesh.triangles)
        //    {
        //        triangles.Add(vertexIndex + objectVertexCount);
        //    }
        //}

        //combinedMesh.vertices = vertices.ToArray();
        //combinedMesh.normals = normals.ToArray();
        //combinedMesh.triangles = triangles.ToArray();

        return combinedMesh;
    }
}
