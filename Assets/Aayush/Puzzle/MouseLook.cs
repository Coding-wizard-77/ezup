using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Range(10f, 300f)]
    public float mouseSensitivity = 100f;

    [TextArea]
    public string description = "You look around.";

    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // vertical look (camera)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // horizontal look (player)
        if (playerBody != null)
            playerBody.localRotation *= Quaternion.Euler(0f, mouseX, 0f);
    }

    public string DescribeSelf() => description;
    public string DescribeSelf(string customDescription) => customDescription;
    public string DescribeSelf(GameObject obj) => obj != null ? obj.name : description;
}
