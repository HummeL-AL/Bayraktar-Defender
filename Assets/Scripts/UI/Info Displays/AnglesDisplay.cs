using UnityEngine;
using TMPro;

public class AnglesDisplay : MonoBehaviour
{
    [SerializeField] private Transform _object = null;

    private TMP_Text display = null;

    private void Awake()
    {
        display = GetComponent<TMP_Text>();
    }

    public void UpdateValue()
    {
        Vector3 rotation = _object.localEulerAngles;
        string xRotation = string.Format("{0:0}", rotation.x);
        string yRotation = string.Format("{0:0}", rotation.y);
        display.text = $"ANG:{xRotation}°{yRotation}°";
    }
}
