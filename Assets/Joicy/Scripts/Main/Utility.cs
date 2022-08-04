using UnityEngine;

public static class Utility
{
    public static Vector2 FindPointOnCircle(float radius)
    {
        float X = Random.Range(-radius, radius);
        float Z = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(X, 2));
        
        if(Random.Range(0,2) == 0)
        {
            Z *= -1;
        }
        return new Vector2(X, Z);
    }

    public static Vector2 FindPointInCircle(float radius)
    {
        float X = Random.Range(-radius, radius);
        float maxZ = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(X, 2));
        float Z = Random.Range(-maxZ, maxZ);

        return new Vector2(X, Z);
    }

    public static Vector3 GetMousePoint()
    {
        Vector3 targetPosition = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            targetPosition = raycastHit.point;
        }

        return targetPosition;
    }

    public static Vector3 GetCameraLookingPoint()
    {
        Vector3 targetPosition = Vector3.zero;

        Transform camera = Camera.main.transform;
        Ray ray = new Ray(camera.position, camera.forward);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            targetPosition = raycastHit.point;
        }
        return targetPosition;
    }
}
