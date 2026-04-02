using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "Scriptable Objects/ZombieData")]
public class ZombieData : ScriptableObject
{
    public float startingHealth;
    public float damage;
    public float moveSpeed;

    public GameObject zombie;
}
