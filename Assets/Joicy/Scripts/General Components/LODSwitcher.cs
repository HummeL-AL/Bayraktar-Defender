using System.Collections.Generic;
using UnityEngine;

public class LODSwitcher : MonoBehaviour
{
    private LODGroup currentLODGroup = null;
    private LODGroup[] LODGroups = null;

    public void SwitchLOD(LODGroup targetGroup)
    {
        foreach(LODGroup group in LODGroups)
        {
            if(group == targetGroup)
            {
                currentLODGroup.SetLODs(group.GetLODs());
                EnableLOD(group);
            }
            else
            {
                DisableLOD(group);
            }
        }
    }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        currentLODGroup = GetComponent<LODGroup>();

        UpdateLODs();
        SwitchLOD(LODGroups[0]);
    }

    private void UpdateLODs()
    {
        List<LODGroup> groups = new List<LODGroup>();
        foreach(Transform child in transform)
        {
            LODGroup group = child.GetComponent<LODGroup>();
            if(group)
            {
                groups.Add(group);
            }
        }

        LODGroups = groups.ToArray();
    }

    private void EnableLOD(LODGroup group)
    {
        foreach (LOD lod in group.GetLODs())
        {
            foreach (Renderer renderer in lod.renderers)
            {
                renderer.enabled = true;
            }
        }
    }

    private void DisableLOD(LODGroup group)
    {
        foreach(LOD lod in group.GetLODs())
        {
            foreach(Renderer renderer in lod.renderers)
            {
                renderer.enabled = false;
            }
        }
    }
}