using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 5f; // Fuerza del impuslo vertical
    public float cooldownTime = 0.5f; // Tiempo de espera entre saltos (ajustable desde el inspector)
    public float gravityIncrease = 0.2f; // Aumento de la gravedad por objeto recogido
    public float gravityDecrease = 0.01f; // Cantidad de gravedad que disminuye por cada salto
    public float minGravityScale = 1f; // Límite mínimo de gravedad
    public float maxGravityScale = 5f; // Valor máximo de gravedad si se usa el límite
    public bool useGravityLimit = false; // SI es true, ahy un máximo de gravedad     
    public float cooldownTimeAdd = 1f; // Tiempo que se sumara al cooldown

    private Rigidbody2D rb;
    private float lastJumpTime; // Tiempo en el que ocurrió el último salto
    private Vector2 jumpDirection = new Vector2(1, 1).normalized; // Dirección diagonal inicial (hacia arriba y derecha)

    private float defaultJumpForce;
    private float defaultCooldownTime;

    private void Start()
    {
        // Obtener el componente Rigidbody2D del cubo
        rb = GetComponent<Rigidbody2D>();
        lastJumpTime = cooldownTime; // Inicializar para permitir el primer salto inmediatamente
        defaultJumpForce = jumpForce; ;
        defaultCooldownTime = cooldownTime;
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

        // Reducir la gravedad, asegurando que no baje del mínimo permitido
        rb.gravityScale = Mathf.Max(rb.gravityScale - gravityDecrease, minGravityScale);

        // Registrar el momento del último salto
        lastJumpTime = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Cambiar la dirección horizontal del impulso al chocar con un objeto
        jumpDirection.x *= -1; // Invertir la dirección horizontal
    }

    public void IncreaseGravity(bool doubleEffect = false)
    {
        //if (useGravityLimit)
        //{
        //    // Solo aumenta si no ha alcanzo el máximo
        //    if (rb.gravityScale < maxGravityScale)
        //    {
        //        rb.gravityScale += gravityIncrease;
        //    }
        //}
        //else
        //{
        //    // Si no hay limite, sigue aumentando
        //    rb.gravityScale += gravityIncrease;
        //}

        float increaseAmount = doubleEffect ? gravityIncrease * 2 : gravityIncrease;

        if (useGravityLimit)
        {
            rb.gravityScale = Mathf.Min(rb.gravityScale + increaseAmount, maxGravityScale);
        }
        else
        {
            rb.gravityScale += increaseAmount;
        }

        Debug.Log($"Gravedad aumentada: {rb.gravityScale}");
    }

    public void DecreaseGravity()
    {
        rb.gravityScale = Mathf.Max(rb.gravityScale - gravityDecrease, minGravityScale);
        Debug.Log($"Gravedad reducida: {rb.gravityScale}");
    }

    public void BoostJumpForce(float duration)
    {
        StartCoroutine(BoostJumpCoroutine(duration));
    }

    private IEnumerator BoostJumpCoroutine(float duration)
    {
        jumpForce *= 1.5f; // Aumento un 50%
        Debug.Log($"Aumento de fuerza de salto activado. Nueva fuerza: {jumpForce}");
        yield return new WaitForSeconds(duration);
        jumpForce = defaultJumpForce;
        Debug.Log("Aumento de fuerza de salto finalizado.");
    }

    public void IncreaseCooldown(float duration)
    {
        StartCoroutine(IncreaseCooldownCoroutine(duration));
    }

    private IEnumerator IncreaseCooldownCoroutine(float duration)
    {
        cooldownTime += cooldownTimeAdd;
        Debug.Log($"Cooldown de salto aumentado. Nuevo cooldown: {cooldownTime}");
        yield return new WaitForSeconds(duration);
        cooldownTime = defaultCooldownTime;
        Debug.Log("Cooldown de salto vuelto a la normalidad.");
    }

    public void ReduceJumpForce(float duration)
    {
        StartCoroutine(ReduceJumpCoroutine(duration));
    }

    private IEnumerator ReduceJumpCoroutine(float duration)
    {
        jumpForce *= 0.7f; // Reduce la fuerza de salto al 70%
        Debug.Log($"Fuerza de salto reducida temporalmente. Nueva fuerza: {jumpForce}");
        yield return new WaitForSeconds(duration);
        jumpForce = defaultJumpForce;
        Debug.Log("Fuerza de salto restaurada.");
    }
}