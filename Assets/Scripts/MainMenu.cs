using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI scoreText;  // Kéo Text (TMP) vào đây trong Inspector
	[SerializeField] private TextMeshProUGUI highestScoreText;

	void Start()
	{
		// Lấy điểm đã lưu trong PlayerPrefs (mặc định là 0 nếu chưa có)
		int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
		int highesScore = PlayerPrefs.GetInt("HighestScore", 0);

		if (finalScore > 0)
		{
			scoreText.text = "Score: " + finalScore.ToString();
			if (finalScore > highesScore)
			{
				PlayerPrefs.SetInt("HighestScore", finalScore);
				highesScore = finalScore;
			}
			PlayerPrefs.SetInt("FinalScore", 0);
		}
		if (highesScore > 0)
		{
			highestScoreText.text = "Highest Score: " + highesScore.ToString();
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
