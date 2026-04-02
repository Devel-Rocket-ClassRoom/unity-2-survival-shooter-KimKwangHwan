using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public readonly static string Vertical = "Vertical";
    public readonly static string Horizontal = "Horizontal";
    public readonly static string Fire = "Fire1";

    public float UpDown { get; private set; }
    public float leftRight { get; private set; }
    public bool Shot { get; private set; }
    public Vector3 MP { get; private set; }

    void Update()
    {
        UpDown = Input.GetAxis(Vertical);
        leftRight = Input.GetAxis(Horizontal);
        Shot = Input.GetButton(Fire);
        MP = Input.mousePosition;
    }
}
