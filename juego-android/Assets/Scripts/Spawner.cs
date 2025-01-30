using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab; // Prefab del objeto a spawnear
    public float minSpawnInterval = 1f; // Tiempo mínimo entre spawns
    public float maxSpawnInterval = 3f; // Tiempo maximo entre spawns
    public float spawnXRange = 2f; // Rango en el eje X para spanwear

    private float timer;
    private float nextSpawnTime;

    private void Start()
    {
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
        timer = nextSpawnTime;
    }

    private void Update()
    {
        // Contar el tiempo para spawnear
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Spawn();
            nextSpawnTime = Random.Range(minSpawnInterval , maxSpawnInterval);
            timer = nextSpawnTime; // Reiniciar el timer
        }
    }

    void Spawn()
    {
        // Calcular una posición aleatoria en x 
        float SpawnX = Random.Range(-spawnXRange, spawnXRange);

        // Calcular la posición de spawn (arriba de la pantalla)
        Vector2 spawnPosition = new Vector2(SpawnX, Camera.main.orthographicSize + 1);

        // Instanciar el objeto recolectable
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}
