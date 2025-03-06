using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[SerializeField] private AudioSource sfxAudioSource;

	[SerializeField] private AudioClip coinClip;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
	}
	public void PlayCoinSound()
	{
		sfxAudioSource.PlayOneShot(coinClip);
	}
}