using UnityEngine;

[ExecuteInEditMode]
public class LandscapeGenerator : MonoBehaviour
{
    [SerializeField] private Mesh _originalMesh = null;

    [SerializeField] private Texture2D _landscapeTexture = null;
    [SerializeField] private float _landscapeHeight = 0f;

    public void SetMesh(Mesh mesh)
    {
        GetComponent<MeshFilter>().mesh = mesh;
        
        MeshCollider collider = GetComponent<MeshCollider>();
        if(collider != null)
        {
            collider.sharedMesh = mesh;
        }
    }

    public Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = CalculateVertices(_originalMesh);
        mesh.uv = _originalMesh.uv;
        mesh.triangles = _originalMesh.triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private Vector3[] CalculateVertices(Mesh mesh)
    {
        Vector3[] verts = new Vector3[mesh.vertices.Length];


        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            Vector2 vertexPos = mesh.uv[i];
            vertexPos.x *= _landscapeTexture.width;
            vertexPos.y *= _landscapeTexture.height;

            float heightPercent = _landscapeTexture.GetPixel((int)vertexPos.x, (int)vertexPos.y).r;
            verts[i] = mesh.vertices[i] + heightPercent * _landscapeHeight * Vector3.up;
        }

        return verts;
    }
}
