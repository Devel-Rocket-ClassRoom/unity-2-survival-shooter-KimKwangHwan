using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public Vector3 offset = new Vector3(0f, 10f, -5f);
    private Quaternion originRotation;

    private void Awake()
    {
        originRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        Vector3 desiredPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, speed * Time.deltaTime);
        transform.LookAt(player);

        transform.rotation = originRotation;
    }
}
