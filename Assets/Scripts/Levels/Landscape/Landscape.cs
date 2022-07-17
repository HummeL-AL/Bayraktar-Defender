using UnityEngine;

public class Landscape : MonoBehaviour
{
    private MeshFilter _filter = null;
    private MeshRenderer _renderer = null;

    

    private void Awake()
    {
        _filter = GetComponent<MeshFilter>();
        _renderer = GetComponent<MeshRenderer>();
    }
}
