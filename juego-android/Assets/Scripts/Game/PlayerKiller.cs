using System.Collections;
using UnityEngine;

public class PlayerKiller : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el jugador toca un player killer, terminar la partida
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.GameOver();
            Destroy(collision.gameObject);
            StartCoroutine(RestartAfterDelay(10f)); // Llamamos a la corrutina con 10 segundos de espera
        }
    }

    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.instance.RestartGame();
    }
}