using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera CameraObj;

    private GameObject FocusObj;

    void Start()
    {
        CameraObj = gameObject.GetComponent<Camera>();
        FocusObj = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        if(FocusObj != null)
        {
            var currentPos = CameraObj.transform.position;
            var newPos = new Vector3(FocusObj.transform.position.x, currentPos.y, FocusObj.transform.position.z);
            CameraObj.transform.position = newPos;
        }
    }

    public void SetCameraFocus(GameObject newFocus)
    {
        FocusObj = newFocus;
    }
}
