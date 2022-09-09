using UnityEngine;

public class ObjectDetacher : MonoBehaviour
{
    [SerializeField] private Transform[] objectsToDetach = null;

    public void Detach()
    {
        foreach (Transform detachment in objectsToDetach)
        {
            if(detachment) detachment.parent = null;
        }
    }
}