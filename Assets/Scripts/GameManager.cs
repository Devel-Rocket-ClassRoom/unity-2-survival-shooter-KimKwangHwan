using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int totalScore = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Slider hpBar;
    public Image hitImage;
    public Image gameOverImage;

    private float hitColorAlpha = 0.2f;
    private float blinkDuration = 0.05f;
    private int blinkCount = 2;

    private float fadeOutDuration = 2f;

    private Coroutine coHitBlink;
    private Coroutine coGameOver;

    private bool isPause;

    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
            {
                Time.timeScale = 1f;
                isPause = false;
            }
            else
            {
                Time.timeScale = 0f;
                isPause = true;
            }
        }
    }

    private void OnEnable()
    {
        scoreText.text = $"SCORE : 0";
        gameOverText.enabled = false;
        hpBar.value = 1;
        isPause = false;
    }

    public void hpBarUpdate(float value)
    {
        hpBar.value = value;
        if (coHitBlink != null)
        {
            StopCoroutine(coHitBlink);
            Color color = hitImage.color;
            color.a = 0f;
            hitImage.color = color;
            coHitBlink = null;
        }
        coHitBlink = StartCoroutine(CoHitBlink());
    }
    private IEnumerator CoHitBlink()
    {
        Color color = hitImage.color;

        for (int i = 0; i < blinkCount; i++)
        {
            color.a = hitColorAlpha;
            hitImage.color = color;
            yield return new WaitForSeconds(blinkDuration);

            color.a = 0f;
            hitImage.color = color;
            yield return new WaitForSeconds(blinkDuration);
        }
        coHitBlink = null;
    }
    public void GameOver()
    {
        if (coGameOver != null)
        {
            StopCoroutine(coGameOver);
            Color color = gameOverImage.color;
            color.a = 1f;
            gameOverImage.color = color;
            coGameOver = null;
        }
        coGameOver = StartCoroutine(CoGameOver());
    }

    public IEnumerator CoGameOver()
    {
        Color color = gameOverImage.color;
        float elapsed = 0f;
        color.a = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            
            color.a = Mathf.Lerp(0f, 1f, elapsed / fadeOutDuration);
            gameOverImage.color = color;
            yield return null;
        }
        color.a = 1f;
        gameOverImage.color = color;

        gameOverText.enabled = true;
        coGameOver = null;
    }
    public void AddScore(int amount)
    {
        totalScore += amount;
        scoreText.text = $"SCORE : {totalScore}";
    }
    public void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}
