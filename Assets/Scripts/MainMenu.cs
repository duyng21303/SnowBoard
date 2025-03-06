using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;  // Kéo Text (TMP) vào đây trong Inspector

    void Start()
    {
        // Lấy điểm đã lưu trong PlayerPrefs (mặc định là 0 nếu chưa có)
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        if(finalScore > 0)
        {
            scoreText.text = "Score: " + finalScore.ToString();
            PlayerPrefs.SetInt("FinalScore", 0);
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
