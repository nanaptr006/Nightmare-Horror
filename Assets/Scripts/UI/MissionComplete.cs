using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionComplete : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TMP_Text resultText;

    private void Awake()
    {
        if (panelRoot == null)
            panelRoot = gameObject;

        RectTransform panelTransform = panelRoot.GetComponent<RectTransform>();
        if (panelTransform != null)
        {
            panelTransform.anchorMin = new Vector2(0.5f, 0.5f);
            panelTransform.anchorMax = new Vector2(0.5f, 0.5f);
            panelTransform.pivot = new Vector2(0.5f, 0.5f);
            panelTransform.anchoredPosition = Vector2.zero;
        }

        panelRoot.SetActive(false);
    }

    public void Show(int killedCount)
    {
        if (resultText != null)
            resultText.text = $"Zombies killed: {killedCount}";

        panelRoot.SetActive(true);
    }

    public void RestartGame()
    {
        KillCount.ResetCount();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
