using UnityEngine;

public class ObjectDetacher : MonoBehaviour
{
    [SerializeField] private Transform[] objectsToDetach = null;

    public void Detach()
    {
        foreach (Transform detachment in objectsToDetach)
        {
            detachment.parent = null;
        }
    }
}