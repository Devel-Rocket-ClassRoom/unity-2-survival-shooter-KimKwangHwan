using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody rb;
    private Animator animator;

    private float moveSpeed = 5f;
    private float rotateSpeed = 60f;
    private Plane ground;

    public readonly static int HashMoving = Animator.StringToHash("Moving");

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool("Moving", playerInput.UpDown != 0 || playerInput.leftRight != 0);
    }

    private void FixedUpdate()
    {
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(playerInput.MP);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 point = ray.GetPoint(distance);
            Vector3 lookDir = point - transform.position;
            lookDir.y = 0f;

            if (lookDir != Vector3.zero)
            {
                float angle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
                Quaternion targetRot = Quaternion.Euler(0f, angle, 0f);
                Quaternion smoothRot = Quaternion.Slerp(rb.rotation, targetRot, rotateSpeed * Time.fixedDeltaTime);
                rb.MoveRotation(smoothRot);
            }
        }

        Vector3 delta = new Vector3(playerInput.leftRight, 0, playerInput.UpDown);
        rb.MovePosition(transform.position + delta * moveSpeed * Time.deltaTime);
    }
}
