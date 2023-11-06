using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;  // Reference to the player GameObject
    private Camera mainCamera;  // Reference to the main camera
    public ScreenShake ss;
    private float ssTimer = 1f;
    public AudioSource spikeSFX;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Convert the player's world position to viewport position
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(player.transform.position);

        // Check if the player is off-screen
        if (viewportPosition.x < -1 || viewportPosition.x > 2 || viewportPosition.y < 0)
        {
            // Trigger respawn
            player.GetComponent<Respawn>().SpawnCharacter();
            if (ssTimer <= 0)
            {
                StartCoroutine(CameraPulse(.5f, 2f));
                spikeSFX.Play();
                ssTimer = 1f;
            }
            
        }
        ssTimer -= Time.deltaTime;
    }

    private IEnumerator CameraPulse(float duration, float magnitude)
    {
        float elapsed = 0.0f;
        float originalSize = mainCamera.orthographicSize;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percentComplete = elapsed / duration;

            // Calculate an offset using a sine wave that goes from -1 to +1
            float offset = Mathf.Sin(percentComplete * Mathf.PI * 2) * magnitude;

            // Set the camera's orthographic size to the original value + offset
            mainCamera.orthographicSize = originalSize + offset;

            yield return null;
        }

        // Reset the camera's orthographic size to its original value
        mainCamera.orthographicSize = originalSize;
    }
}
