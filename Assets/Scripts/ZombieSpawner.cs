using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ZombieSpawner : MonoBehaviour
{
    public ZombieData[] datas;
    public Transform[] spawnPoints;

    private float spawnInterval = 2f;
    private float lastSpawnTime = 0f;
    private List<Zombie> zombies = new List<Zombie>();

    private void Update()
    {
        if (Time.time > lastSpawnTime + spawnInterval)
        {
            lastSpawnTime = Time.time;
            CreateZombie();
        }
    }

    private void CreateZombie()
    {
        var point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var data = datas[Random.Range(0, datas.Length)];
        var zombie = Instantiate(data.zombie, point.position, point.rotation);

        zombie.Setup(data);
        zombies.Add(zombie);

        zombie.OnDead.AddListener(() => zombies.Remove(zombie));
        // 좀비 죽었을 때 이벤트 추가할 것
        zombie.OnDead.AddListener(() => Destroy(zombie.gameObject, 5f));
    }
}
