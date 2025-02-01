using UnityEngine;

public class WallManager : MonoBehaviour
{
    public Transform leftWall;
    public Transform rightWall;
    public float wallThickness = 0.5f; // Grosor total de la pared
    public float wallOffset = 0.25f; // Cu�nto de la pared se ver� dentro de la pantalla

    void Start()
    {
        AdjustWallsToScreen();
    }

    void AdjustWallsToScreen()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("No se encontr� la c�mara principal.");
            return;
        }

        float screenHeight = mainCamera.orthographicSize * 2;
        float screenWidth = screenHeight * mainCamera.aspect;

        // Ajustamos las posiciones para que la mitad de la pared quede dentro de la pantalla
        leftWall.position = new Vector2(-screenWidth / 2 - (wallThickness / 2) + wallOffset, 0);
        rightWall.position = new Vector2(screenWidth / 2 + (wallThickness / 2) - wallOffset, 0);

        // Ajustamos el tama�o de las paredes
        leftWall.localScale = new Vector3(wallThickness, screenHeight, 1);
        rightWall.localScale = new Vector3(wallThickness, screenHeight, 1);
    }
}
