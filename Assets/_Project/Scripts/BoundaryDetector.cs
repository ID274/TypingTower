using UnityEngine;

public class BoundaryDetector : MonoBehaviour
{
    private bool colliding = false;
    private Material material;
    [SerializeField] private Color collidingColor = Color.red;
    [SerializeField] private Color normalColor = Color.green;

    private Collider collidingBlock = null;

    private float countdownTimer = 0f;
    private bool isCountdownActive = false;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    private void FixedUpdate()
    {
        // Update the material color based on the colliding state
        material.color = colliding ? collidingColor : normalColor;
    }

    private void Update()
    {
        if (isCountdownActive)
        {
            UpdateCountdownTimer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Playing)
        {
            if (collidingBlock != null) return;

            if (other.CompareTag("Word Object"))
            {
                colliding = true;
                collidingBlock = other;
                StartCountdown();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Playing)
        {
            if (other.CompareTag("Word Object") && other == collidingBlock)
            {
                collidingBlock = null;
                colliding = false;
                StopCountdown();
            }
        }
    }

    private void StartCountdown()
    {
        isCountdownActive = true;
        countdownTimer = 0f; // Reset the countdown timer
    }

    private void StopCountdown()
    {
        isCountdownActive = false;
        countdownTimer = 0f; // Reset the countdown timer
        UIManager.Instance.HideCountdown();
    }

    private void UpdateCountdownTimer()
    {
        // Fetch the total countdown duration from GameManager
        float countdownTime = GameManager.Instance.lossCountdownTime;

        // Increment elapsed time using real-time delta (ignores time scale changes)
        if (Time.deltaTime != 0)
        {
            countdownTimer += Time.unscaledDeltaTime;
        }

        // Calculate remaining time
        float timeRemaining = countdownTime - countdownTimer;

        if (timeRemaining > 0 && colliding)
        {
            // Display the remaining time as whole seconds
            UIManager.Instance.DisplayCountdown(Mathf.CeilToInt(timeRemaining));
        }
        else if (colliding)
        {
            Debug.Log("You lose!");
            // Trigger game over logic and stop the countdown
            GameManager.Instance.GameOver();
            StopCountdown();
        }
        else
        {
            StopCountdown();
        }
    }

}
