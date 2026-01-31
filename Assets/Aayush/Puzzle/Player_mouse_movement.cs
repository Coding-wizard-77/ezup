using UnityEngine;

public class Player_mouse_movement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.forward * v + transform.right * h;
        Vector3 targetPos = rb.position + move * speed * Time.fixedDeltaTime;

        // Preserve Y controlled by physics
        targetPos.y = rb.position.y;

        rb.MovePosition(targetPos);
    }

}
