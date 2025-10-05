using DG.Tweening;
using UnityEngine;

public class MoneySpawner : MonoBehaviour
{
    public GameObject prefab;
    public Transform pointA;
    public Transform pointB;
    public float minScale = 0.5f;    
    public float maxScale = 1.5f;       

    void Start()
    {
        SpawnRandom();
        DOVirtual.DelayedCall(1, SpawnRandom);
        DOVirtual.DelayedCall(2, SpawnRandom);
        DOVirtual.DelayedCall(3, SpawnRandom);
        DOVirtual.DelayedCall(4, SpawnRandom);
        DOVirtual.DelayedCall(5, SpawnRandom);

    }

    public void SpawnRandom()
    {
        if (prefab == null || pointA == null || pointB == null) {
            Debug.LogWarning("Missing references on RandomSpawner!");
            return;
        }

        float randomX = Random.Range(pointA.position.x, pointB.position.x);
        float y = pointA.position.y;
        float z = pointA.position.z;

        Vector3 spawnPosition = new Vector3(randomX, y, z);

        GameObject spawned = Instantiate(prefab, spawnPosition, Quaternion.identity);
        spawned.transform.SetParent(transform);
        spawned.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        float randomScale = Random.Range(minScale, maxScale);
        spawned.transform.localScale = Vector3.one * randomScale;
        spawned.transform.DOLocalMoveY(-2200, 5f).SetEase(Ease.Linear);
        Destroy(spawned, 5);
        DOVirtual.DelayedCall(Random.Range(1, 3), SpawnRandom);
    }
}
