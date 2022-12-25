using UnityEngine;

public class FaceToCamera : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }
}
