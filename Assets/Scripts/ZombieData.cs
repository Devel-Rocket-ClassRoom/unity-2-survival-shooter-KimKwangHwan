using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "Scriptable Objects/ZombieData")]
public class ZombieData : ScriptableObject
{
    public float startingHealth;
    public float damage;
    public float moveSpeed;
    public float attackInterval;
    public int score;

    public AudioClip hurtClip;
    public AudioClip deathClip;

    public Zombie zombie;
}
