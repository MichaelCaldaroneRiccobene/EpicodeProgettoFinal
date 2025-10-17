using UnityEngine;

public class Object_Look_Camera : MonoBehaviour
{
    private void Update()
    {
        if (Camera.main != null)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
