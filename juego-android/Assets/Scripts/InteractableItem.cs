using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public float minMoveSpeed = 1f; // Velocidad mínima de movimiento
    public float maxMoveSpeed = 3f; // Velocidad máxima de movimiento

    public float moveSpeed; // Velocidad actual del objeto

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
            GameManager.instance.CollectItem(); // Modifica el GameManager
            collision.GetComponent<PlayerController>().IncreaseGravity(); // Aumenta la gravedad
            Destroy(gameObject);
        }
    }
}