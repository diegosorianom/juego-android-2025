using UnityEngine;

public abstract class CollectableItem : MonoBehaviour
{
    public float moveSpeed = 2f; // Velocidad del objeto

    private void Update()
    {
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);

        // Destruir el objeto si sale de la pantalla
        if (transform.position.y < -Camera.main.orthographicSize -1 )
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                ApplyEffect(player);
            }
            Destroy(gameObject);
        }
    }

    // Método que será sobreescrito en los objetos especificos
    protected abstract void ApplyEffect(PlayerController player);
}
