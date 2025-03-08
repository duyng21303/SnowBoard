using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	[SerializeField] private TextMeshProUGUI scoreText;
	public static ScoreController instance;
	public int score;

	void Start()
	{
		score = PlayerPrefs.GetInt("FinalScore", 0);
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

