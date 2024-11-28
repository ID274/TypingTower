using System.Collections;
using UnityEngine;

public class BoundaryDetector : MonoBehaviour
{
    private bool colliding = false;
    private Material material;
    [SerializeField] private Color collidingColor = Color.red;
    [SerializeField] private Color normalColor = Color.green;

    private Collider collidingBlock = null;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }
    private void FixedUpdate()
    {
        switch (colliding)
        {
            case true:
                material.color = collidingColor;
                break;
            case false:
                material.color = normalColor;
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Playing)
        {
            if (collidingBlock != null)
            {
                return;
            }
            if (other.CompareTag("Word Object"))
            {
                colliding = true;
                collidingBlock = other;
                StartCoroutine(CountDownTimer());
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
                UIManager.Instance.HideCountdown();
            }
        }
    }

    private IEnumerator CountDownTimer()
    {
        yield return new WaitForSeconds(0.5f);
        if (!colliding)
        {
            yield break;
        }
        int timeLeft = GameManager.Instance.lossCountdownTime;
        for (int i = 0; i < GameManager.Instance.lossCountdownTime; i++)
        {
            UIManager.Instance.DisplayCountdown(timeLeft.ToString());
            Debug.Log("Time left: " + (GameManager.Instance.lossCountdownTime - i) + " seconds");
            yield return new WaitForSecondsRealtime(1);
            if (colliding == false)
            {
                yield break;
            }
            timeLeft--;
        }
        if (colliding)
        {
            Debug.Log("You lose!");
            UIManager.Instance.DisplayCountdown("L");
            GameManager.Instance.GameOver();
        }
    }
}
