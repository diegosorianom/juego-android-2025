using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 5f; // Fuerza del impuslo vertical
    public float cooldownTime = 0.5f; // Tiempo de espera entre saltos (ajustable desde el inspector)
    
    private Rigidbody2D rb;
    private float lastJumpTime; // Tiempo en el que ocurrió el último salto
    private Vector2 jumpDirection = new Vector2(1, 1).normalized; // Dirección diagonal inicial (hacia arriba y derecha)

    private void Start()
    {
        // Obtener el componente Rigidbody2D del cubo
        rb = GetComponent<Rigidbody2D>();
        lastJumpTime = cooldownTime; // Inicializar para permitir el primer salto inmediatamente
    }

    private void Update()
    {
        // Detectar si el jugador toca la pantalla y si ha pasado el tiempo de cooldown
        if (Input.GetMouseButtonDown(0) && Time.time - lastJumpTime >= cooldownTime) // 0 representa el clic izquierdo o el toque en movil
        {
            Jump();
        }
    }

    void Jump()
    {
        // Reiniciar la velocidad para evitar acumulación de fuerza
        rb.linearVelocity = Vector2.zero;

        // Aplicar un impulso vertical al cubo
        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);

        // Registrar el momento del último salto
        lastJumpTime = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Cambiar la dirección horizontal del impulso al chocar con un objeto
        jumpDirection.x *= -1; // Invertir la dirección horizontal
    }
}
