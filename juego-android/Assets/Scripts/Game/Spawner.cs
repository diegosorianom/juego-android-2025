using UnityEngine;

public class Spawner : MonoBehaviour
{
    //public GameObject prefab; // Prefab del objeto a spawnear
    [Header("Prefabs objetos")]
    public GameObject normalPrefab;
    public GameObject positivePrefab;
    public GameObject negativePrefab;

    [Header("Ajustes de spawn")]
    public BoxCollider2D spawnArea; // Zona de spawn
    public float spawnOffset = 0.5f; // Cu�nto se reduce el �rea de spawn en los lados
    public float initialMinSpawnInterval = 2f; // Tiempo inicial m�nimo entre spawns
    public float initialMaxSpawnInterval = 4f; // Tiempo inicial m�ximo entre spawns
    public float minSpawnInterval = 0.5f; // L�mite inferior de spawn
    public float difficultyIncreaseRate = 0.05f; // Cu�nto se reduce el intervalo de spawn
    private float nextSpawnTime;
    private float currentMinSpawnInterval;
    private float currentMaxSpawnInterval;
    private enum SpawnState { Normal, Frenesi, Calma }
    private SpawnState currentState = SpawnState.Normal;
    private SpawnState previousState;

    [Header("Duraci�n de estados")]
    public float frenesiDuration = 5f; // Duraci�n del Fr�nesi
    public float calmaDuration = 10f; // Duraci�n del modo Calma
    public float frenesiCooldown = 20f; // Tiempo antes de que pueda ocurrir otro Fr�nesi
    private float frenesiTimer = 0f;
    private float calmaTimer = 0f;
    private float frenesiCooldownTimer;

    [Header("Ajustes de la partida")]
    private float timer;
    private float elapsedTime = 0f; // Tiempo transcurrido en la partida


    private void Start()
    {
        AdjustSpawnAreaToScreen(); // Ajustar automaticamente el �rea de spawn

        // Inicializar valores correctamente
        currentMinSpawnInterval = initialMinSpawnInterval;
        currentMaxSpawnInterval = initialMaxSpawnInterval;
        nextSpawnTime = Random.Range(currentMinSpawnInterval, currentMaxSpawnInterval);
        timer = nextSpawnTime;
        frenesiCooldownTimer = frenesiCooldown; // Se inicializa con cooldown para evitar Fr�nesi inmediato

        previousState = currentState; // Inicializar el estado anterior
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        // Control del estado del spawn
        switch (currentState)
        {
            case SpawnState.Normal:
                HandleNormalState();
                break;
            case SpawnState.Frenesi:
                HandleFrenesiState();
                break;
            case SpawnState.Calma:
                HandleCalmaState();
                break;
        }

        // Contar el tiempo para el pr�ximo spawn
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Spawn();
            timer = nextSpawnTime;
        }
    }

    private void HandleNormalState()
    {
        // Reducir los tiempos de spawn progresivamente hasta el m�nimo permitido
        currentMinSpawnInterval = Mathf.Max(initialMinSpawnInterval - (elapsedTime * difficultyIncreaseRate), minSpawnInterval);
        currentMaxSpawnInterval = Mathf.Max(initialMaxSpawnInterval - (elapsedTime * difficultyIncreaseRate), minSpawnInterval + 0.5f);
        nextSpawnTime = Random.Range(currentMinSpawnInterval, currentMaxSpawnInterval);

        // Reducir el cooldown del Fr�nesi solo si a�n no ha llegado a 0
        if (frenesiCooldownTimer > 0)
        {
            frenesiCooldownTimer -= Time.deltaTime;
        }
        else
        {
            // Solo entra en Fr�nesi si el cooldown ha pasado
            currentState = SpawnState.Frenesi;
            frenesiTimer = frenesiDuration;
        }
    }

    private void HandleFrenesiState()
    {
        // Establecer el tiempo de spawn m�nimo
        nextSpawnTime = minSpawnInterval;

        // Reducir el tiempo del Fr�nesi
        frenesiTimer -= Time.deltaTime;
        if (frenesiTimer <= 0)
        {
            currentState = SpawnState.Calma;
            calmaTimer = calmaDuration;
        }
    }

    private void HandleCalmaState()
    {
        // Establecer el spawn en su valor inicial m�nimo (descanso)
        nextSpawnTime = initialMinSpawnInterval;

        // Reducir el tiempo de la calma
        calmaTimer -= Time.deltaTime;
        if (calmaTimer <= 0)
        {
            currentState = SpawnState.Normal;
            frenesiCooldownTimer = frenesiCooldown; // Reiniciar cooldown
        }
    }

    void Spawn()
    {
        if (spawnArea == null)
        {
            Debug.LogError("El spawnArea no est� asignado en el Inspector.");
            return;
        }

        // Obtener los l�mites del collider
        Bounds bounds = spawnArea.bounds;

        // Generar una posici�n aleatoria dentro del collider en el eje X
        float spawnX = Random.Range(bounds.min.x, bounds.max.x);
        float spawnY = bounds.max.y; // Asegurar que aparezcan arriba

        Vector2 spawnPosition = new Vector2(spawnX, spawnY);

        // Instanciar el objeto recolectable
        Instantiate(GetRandomPrefab(), spawnPosition, Quaternion.identity);
    }

    private GameObject GetRandomPrefab()
    {
        float randomValue = Random.value * 100; // N�mero aleatorio entre 0 y 100

        if (randomValue < 70f)
        {
            return normalPrefab;
        }
        else if (randomValue < 85f)
        {
            return positivePrefab;
        }
        else 
        {
            return negativePrefab;
        }
    }

    private void AdjustSpawnAreaToScreen()
    {
        if (spawnArea == null)
        {
            Debug.LogError("El BoxCollider2D del SpawnArea no est� asignado en el Inspector.");
            return;
        }

        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("No se encontr� la c�mara principal.");
            return;
        }

        float screenHeight = mainCamera.orthographicSize * 2;
        float screenWidth = screenHeight * mainCamera.aspect; // Ancho basado en la relaci�n de aspecto

        // Ajustar el tama�o del BoxCollider2D para que coincida con la pantalla
        spawnArea.size = new Vector2(screenWidth - (spawnOffset * 2), spawnArea.size.y);
        spawnArea.offset = new Vector2(0, spawnArea.offset.y);

        Debug.Log($"SpawnArea ajustado: Ancho = {spawnArea.size.x}, Offset = {spawnArea.offset}");
    }
}
