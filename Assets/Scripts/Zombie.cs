using UnityEngine;

public class Zombie : LivingEntity
{
    private bool isSinking = false;

    public void Setup(ZombieData data)
    {

    }

    public void StartSinking()
    {
        isSinking = true;
        GetComponent<Collider>().enabled = false;
    }
}
