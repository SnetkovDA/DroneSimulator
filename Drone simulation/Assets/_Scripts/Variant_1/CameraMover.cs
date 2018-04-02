using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public System.Single speed = 20f;
    public System.Single scrollSpeed = 2f;
    public System.Single borderThickness = 10f;
    public Vector3 moveLimit;

    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        Vector3 currPos = transform.position;

        if (Input.GetKey(KeyCode.UpArrow) /*|| Input.mousePosition.y >= (Screen.height - borderThickness)*/)
            currPos.z += speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.DownArrow) /*|| Input.mousePosition.y <= (borderThickness)*/)
            currPos.z -= speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow) /*|| Input.mousePosition.x <= (borderThickness)*/)
            currPos.x -= speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow) /*|| Input.mousePosition.x >= (Screen.width - borderThickness)*/)
            currPos.x += speed * Time.deltaTime;
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cam.orthographicSize -= scroll * scrollSpeed * Time.deltaTime;
        currPos.x = Mathf.Clamp(currPos.x, -moveLimit.x, moveLimit.x);
        currPos.z = Mathf.Clamp(currPos.z, -moveLimit.z, moveLimit.z);
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 10, moveLimit.y);
        transform.position = currPos;
    }
}
