using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	[SerializeField] private TextMeshProUGUI scoreText;
	public static ScoreController instance;
	public int score = 0;

	void Start()
	{
		UpdateScore();
	}

	private void Update()
	{

	}

	public void AddScore(int points)
	{
		score += points;
		UpdateScore();
	}
	public void UpdateScore()
	{
		scoreText.text = score.ToString();
	}
}

