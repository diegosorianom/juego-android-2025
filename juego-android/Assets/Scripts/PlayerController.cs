using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 5f; // Fuerza del impuslo vertical
    public float cooldownTime = 0.5f; // Tiempo de espera entre saltos (ajustable desde el inspector)
    public float gravityIncrease = 0.2f; // Aumento de la gravedad por objeto recogido
    public float gravityDecrease = 0.01f; // Cantidad de gravedad que disminuye por cada salto
    public float minGravityScale = 1f; // L�mite m�nimo de gravedad
    public float maxGravityScale = 5f; // Valor m�ximo de gravedad si se usa el l�mite
    public bool useGravityLimit = false; // SI es true, ahy un m�ximo de gravedad     

    private Rigidbody2D rb;
    private float lastJumpTime; // Tiempo en el que ocurri� el �ltimo salto
    private Vector2 jumpDirection = new Vector2(1, 1).normalized; // Direcci�n diagonal inicial (hacia arriba y derecha)

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
        // Reiniciar la velocidad para evitar acumulaci�n de fuerza
        rb.linearVelocity = Vector2.zero;

        // Aplicar un impulso vertical al cubo
        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);

        // Reducir la gravedad, asegurando que no baje del m�nimo permitido
        rb.gravityScale = Mathf.Max(rb.gravityScale - gravityDecrease, minGravityScale);

        // Registrar el momento del �ltimo salto
        lastJumpTime = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Cambiar la direcci�n horizontal del impulso al chocar con un objeto
        jumpDirection.x *= -1; // Invertir la direcci�n horizontal
    }

    public void IncreaseGravity()
    {
        if (useGravityLimit)
        {
            // Solo aumenta si no ha alcanzo el m�ximo
            if (rb.gravityScale < maxGravityScale)
            {
                rb.gravityScale += gravityIncrease;
            }
        }
        else
        {
            // Si no hay limite, sigue aumentando
            rb.gravityScale += gravityIncrease;
        }
    }
}