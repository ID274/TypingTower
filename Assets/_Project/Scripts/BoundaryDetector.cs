using UnityEngine;

public class BoundaryDetector : MonoBehaviour
{
    private bool colliding = false;
    private Material material;
    [SerializeField] private Color collidingColor = Color.red;
    [SerializeField] private Color normalColor = Color.green;

    private Collider collidingBlock = null;

    private float timer = 0f;
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
        int timeLeft = GameManager.Instance.lossCountdownTime;
        countdownTimer += Time.deltaTime;
        if (countdownTimer < timeLeft)
        {
            int timeRemaining = Mathf.CeilToInt(timeLeft - countdownTimer); // Use Mathf.CeilToInt for consistent seconds display
            if (timeRemaining < timeLeft)
            {
                UIManager.Instance.DisplayCountdown(timeRemaining.ToString());
            }
            //Debug.Log("Time left: " + timeRemaining + " seconds");
        }
        else
        {
            Debug.Log($"You lose! {timeLeft}");
            // display UI manager score panel
            GameManager.Instance.GameOver();
            StopCountdown();
        }
    }
}
