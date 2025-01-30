using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton para acceder al GameManager desde otros scripts
    public int objectsCollected = 0; // Contador de objetos recogidos

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    public void CollectItem()
    {
        objectsCollected++;
        Debug.Log("Objetos recogidos: " +  objectsCollected);
    }

    public void GameOver()
    {
        // Lógica para terminar la partida
        Debug.Log("Game Over!");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
