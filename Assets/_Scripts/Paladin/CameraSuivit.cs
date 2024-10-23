using UnityEngine;

public class CameraSuivit : MonoBehaviour
{
    public GameObject objCamera;
    public Vector3 posCamera;

    // Update is called once per frame
    void Update()
    {
        objCamera.transform.position = transform.position + posCamera;
        objCamera.transform.LookAt(transform.position);
    }
}
