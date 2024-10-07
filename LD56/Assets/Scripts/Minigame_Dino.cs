using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Dino : MonoBehaviour
{
    [SerializeField] private GameObject player;  // Now a GameObject, not an Image
    private Rigidbody2D playerRb;

    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject obstacleSpawner;
    private float obstacleSpeed = 500f;  // Adjusted for world space movement

    private float originalSpawnRate = 3f;
    private float minimumSpawnRate = 1f;

    private const float TIMER_SECONDS = 20f;
    private float time = 0f;

    public float thrust = 75.0f;

    private bool isPaused = false;

    public void Start() {
        EventManager.TriggerEvent("ChangeMusic", (int)Audio_MusicArea.MINIGAME);
        playerRb = player.GetComponent<Rigidbody2D>();
        StartCoroutine("SpawnObstacle");
        StartCoroutine("UpdateTimer");
    }

    public void Update() {
        if (!isPaused){
            // Player jump logic
            if (Input.GetKeyDown(KeyCode.Space)) {
                playerRb.velocity = new Vector2(0, 0);
                playerRb.AddForce(new Vector2(0, thrust), ForceMode2D.Impulse);
            }

            // Move obstacles and detect collision
            foreach (Transform child in obstacleSpawner.transform) {
                Rigidbody2D rb = child.GetComponent<Rigidbody2D>();

                // Move the obstacle to the left
                rb.velocity = new Vector2(-obstacleSpeed, rb.velocity.y);

                if (child.position.x < -1000) {
                    Destroy(child.gameObject);
                }

                // Check if obstacle is touching player
                foreach (Transform collider in child) {
                    if (collider.GetComponent<BoxCollider2D>().IsTouching(player.GetComponent<BoxCollider2D>())) {
                        LostAttempt();
                        PauseGame();
                    }
                }
            }
        }
    }

    // Spawn obstacles at regular intervals
    private IEnumerator SpawnObstacle() {
        while (true) {
            GameObject newObstacle = Instantiate(obstaclePrefab, obstacleSpawner.transform.position + new Vector3(0, Random.Range(-200, 200), 0), Quaternion.identity);
            newObstacle.transform.SetParent(obstacleSpawner.transform);

            float currentSpawnRate = Mathf.Lerp(originalSpawnRate, minimumSpawnRate, time / TIMER_SECONDS);
            yield return new WaitForSeconds(currentSpawnRate);  // Adjust time between obstacle spawns
        }
    }

    private IEnumerator UpdateTimer()
    {
        const float frequency = 0.5f;
        while (time < TIMER_SECONDS)
        {
            time += frequency;
            yield return new WaitForSeconds(frequency);
        }
        PauseGame();
    }

    public void ResetAttempt() {
        isPaused = false;
        obstacleSpeed = 500f;
        foreach (Transform child in obstacleSpawner.transform) {
            Destroy(child.gameObject);
            time = 0f;
        }
        playerRb.isKinematic = false;
        StartCoroutine("SpawnObstacle");
        StartCoroutine("UpdateTimer");
    }

    public void PauseGame() {
        StopCoroutine("SpawnObstacle");
        StopCoroutine("UpdateTimer");
        isPaused = true;
        obstacleSpeed = 0f;
        foreach (Transform child in obstacleSpawner.transform) {
            Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
            Debug.Log("Pasued Game");
            rb.velocity = new Vector2(0, 0);
        }
        playerRb.velocity = new Vector2(0, 0);
        playerRb.isKinematic = true;
    }

    private void LostAttempt() {
        EventManager.TriggerEvent("LoseMinigameAttempt");
    }

    private void Win() {
        EventManager.TriggerEvent("WinMinigame");
    }
}
