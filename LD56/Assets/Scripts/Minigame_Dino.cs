using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame_Dino : MonoBehaviour
{
    [SerializeField] private GameObject player;  // Now a GameObject, not an Image
    private Rigidbody2D playerRb;

    [SerializeField] private GameObject[] obstacles;
    [SerializeField] private GameObject obstacleSpawner;
    private float obstacleSpeed = 500f;  // Adjusted for world space movement

    private float originalSpawnRate = 3f;
    private float minimumSpawnRate = 1f;

    private const float TIMER_SECONDS = 20f;
    private float time = 0f;

    public float thrust = 500.0f;

    public void Start() {
        EventManager.TriggerEvent("ChangeMusic", (int)Audio_MusicArea.MINIGAME);
        playerRb = player.GetComponent<Rigidbody2D>();
        StartCoroutine(SpawnObstacle());
        StartCoroutine(UpdateTimer());
    }

    public void Update() {
        // Player jump logic
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Jump");
            playerRb.AddForce(new Vector2(0, thrust), ForceMode2D.Impulse);
        }

        // Move obstacles and detect collision
        foreach (Transform child in obstacleSpawner.transform) {
            Rigidbody2D rb = child.GetComponent<Rigidbody2D>();
            BoxCollider2D obstacleCollider = child.GetComponent<BoxCollider2D>();

            // Move the obstacle to the left
            rb.velocity = new Vector2(-obstacleSpeed, rb.velocity.y);

            if (child.position.x < -1000) {
                Destroy(child.gameObject);
            }

            // Check if obstacle is touching player
            if (obstacleCollider.IsTouching(player.GetComponent<BoxCollider2D>())) {
                LostAttempt();
                ResetAttempt();
            }
        }
    }

    // Spawn obstacles at regular intervals
    private IEnumerator SpawnObstacle() {
        while (true) {
            int randomIndex = Random.Range(0, obstacles.Length);
            GameObject chosenObstacle = obstacles[randomIndex];

            GameObject newObstacle = Instantiate(chosenObstacle, obstacleSpawner.transform.position, Quaternion.identity);
            newObstacle.transform.SetParent(obstacleSpawner.transform);

            float currentSpawnRate = Mathf.Lerp(originalSpawnRate, minimumSpawnRate, time / TIMER_SECONDS);
            yield return new WaitForSeconds(currentSpawnRate);  // Adjust time between obstacle spawns
        }
    }

    private IEnumerator UpdateTimer()
    {
        const float frequency = 1.0f;
        while (time < TIMER_SECONDS)
        {
            yield return new WaitForSeconds(frequency);
            time += frequency;
        }
    }

    private void ResetAttempt() {
        foreach (Transform child in obstacleSpawner.transform) {
            Destroy(child.gameObject);
            time = 0f;
        }
    }

    private void LostAttempt() {
        EventManager.TriggerEvent("LoseMinigameAttempt");
    }

    private void Win() {
        EventManager.TriggerEvent("WinMinigame");
    }
}
