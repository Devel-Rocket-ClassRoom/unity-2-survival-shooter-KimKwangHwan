using UnityEngine;
using UnityEngine.InputSystem.HID;

public interface IDamageable
{
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}
