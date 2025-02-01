using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public enum ItemType { Normal, Positive, Negative }
    public ItemType itemType; // Tipo de objeto

    public float minMoveSpeed = 1f; // Velocidad mínima de movimiento
    public float maxMoveSpeed = 3f; // Velocidad máxima de movimiento
    public float moveSpeed; // Velocidad actual del objeto

    public float jumpBoostDuration = 5f; // Duración del aumento del salto
    public float cooldownIncreaseDuration = 5f; // Duración del aumento del cooldown
    public float jumpReductionDuration = 5f; // Duración para reducción del salto

    private void Start()
    {
        // Asignar una velocidad aleatoria al objeto
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
    }

    private void Update()
    {
        // Mover el objeto hacia abajo
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);

        // Destruir el objeto si sale de la pantalla
        if (transform.position.y < -Camera.main.orthographicSize - 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el jugador toca el objeto, desaparecer
        if (collision.CompareTag("Player"))
        {
            //GameManager.instance.CollectItem(); // Modifica el GameManager
            //collision.GetComponent<PlayerController>().IncreaseGravity(); // Aumenta la gravedad
            //Destroy(gameObject);

            PlayerController player = collision.GetComponent<PlayerController>();

            switch(itemType)
            {
                case ItemType.Normal:
                    Debug.Log("Objeto Normal recogido: Aumenta la gravedad.");
                    player.IncreaseGravity();
                    break;

                case ItemType.Positive:
                    ApplyPositiveEffect(player);
                    break;

                case ItemType.Negative:
                    ApplyNegativeEffect(player);
                    break;
            }

            Destroy(gameObject);
        }
    }

    private void ApplyPositiveEffect(PlayerController player)
    {
        int randomEffect = Random.Range(0, 2); // 0 = reducir gravedad, 1 = aumentar salto 
        if (randomEffect == 0)
        {
            Debug.Log("Objeto Positivo recogido: Disminuye la gravedad.");
            player.DecreaseGravity();
        }
        else
        {
            Debug.Log($"Objeto Positivo recogido: Aumenta la fuerza de salto por {jumpBoostDuration} segundos.");
            player.BoostJumpForce(jumpBoostDuration);
        }
    }

    private void ApplyNegativeEffect(PlayerController player)
    {
        int randomEffect = Random.Range(0, 3); // 0 = aumentar gravedad, 1 = aumentar cooldown, 2 = reduce salto
        if (randomEffect == 0)
        {
            Debug.Log("Objeto Negativo recogido: Aumenta el doble la gravedad.");
            player.IncreaseGravity(true); // Parámetro true indica que es un aumento doble
        }
        else if (randomEffect == 1)
        {
            Debug.Log($"Objeto Negativo recogido: Aumenta el cooldown de salto por {cooldownIncreaseDuration} segundos.");
            player.IncreaseCooldown(cooldownIncreaseDuration);
        }
        else
        {
            Debug.Log($"Objeto Negativo recogido: Reduce la fuerza de salto por {jumpReductionDuration} segundos.");
            player.ReduceJumpForce(jumpReductionDuration);
        }
    }
}