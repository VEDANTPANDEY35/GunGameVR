using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 150;
    public int currentHealth;

    [Header("Respawn")]
    public Transform[] spawnPoints;

    [Header("References")]
    public GameObject locomotionObject;
    public GameObject xrSimulator;
    public GameObject gameOverUI;

    CharacterController controller;
    bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        controller = GetComponent<CharacterController>();

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        currentHealth = 0;

        Debug.Log("Player Died");

        if (locomotionObject != null)
            locomotionObject.SetActive(false);

        if (xrSimulator != null)
            xrSimulator.SetActive(false);

        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        // 🟢 Freeze game
        Time.timeScale = 0f;
    }

    public void Respawn()
    {
        Debug.Log("Respawning");

        // 🟢 Resume game
        Time.timeScale = 1f;

        currentHealth = maxHealth;
        isDead = false;

        if (spawnPoints.Length > 0)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

            controller.enabled = false;

            transform.position = spawn.position;
            transform.rotation = spawn.rotation;

            controller.enabled = true;
        }

        if (locomotionObject != null)
            locomotionObject.SetActive(true);

        if (xrSimulator != null)
            xrSimulator.SetActive(true);

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }
}