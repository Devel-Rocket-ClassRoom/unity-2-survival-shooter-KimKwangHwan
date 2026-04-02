using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    private PlayerInput playerInput;
    public Gun gun;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;
        if (playerInput.Shot)
        {
            gun.Fire();
        }
    }
}
