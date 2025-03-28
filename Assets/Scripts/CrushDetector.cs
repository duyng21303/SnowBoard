// ▼ The "using" Keyword 
//      → defines the "Namespace" Directive 
//      → that "Contains" a "Class Used" in the "Code" ▼
using UnityEngine;
using UnityEngine.SceneManagement; // ◄◄ "SceneManagement" Namespace ◄◄


public class CrushDetector : MonoBehaviour
{
    // ▼ "SerializeField" Attribute
    //      → to Dysplay the "Variable" 
    //      → in the "Inspector" Window ▼
    [SerializeField] float loadDelay = 0.5f;  // ◄◄ "1/2 Second" Delay ◄◄
    [SerializeField] ParticleSystem crushEffect;
    [SerializeField] AudioClip crushSFX;

    private int finalScore; // Biến lưu điểm của player khi chết
    // ▼ "Set Variable" 
    //      → to "Avoid Double Sounds & Particles Effect" 
    //      → when "Player Hits" the "Ground" ▼
    bool hasCrushed = false;

	public void Start()
	{
        finalScore = PlayerPrefs.GetInt("FinalScore", 0);
	}
	public int GetScore()
    {
        return finalScore;
    }

    // Phương thức để nhận điểm từ PlayerController
    public void SetScore(int score)
    {
        finalScore += score;
		PlayerPrefs.SetInt("FinalScore", score);
	}
	// ▬ "On Tregger Enter 2D()" Method 
	//       → with a "Delay" of "2 Seconds"
	//      → to "Call" the "ReloadScene()" Method ▬
	void OnTriggerEnter2D(Collider2D other) 
   {
        // ▼ "If" the "Player Hits" the "Ground" & "has Crushed" is "False" ▼
        if (other.tag == "Ground" && !hasCrushed)
        {

            // ▼ "Set Variable" 
            //      → to "Avoid Double Sounds & Particles Effect" 
            //      → when "Player Hits" the "Ground" ▼
            hasCrushed = true;

            // ▼ "Call" the "DisableControls()" Method 
            //      → to "Disable" the "Input" of the "Player" ▼
            FindAnyObjectByType<PlayerController>().DisableControls();

            // ▼ "Acccessing" the "Play()" Method 
            //     of the "Crush Effect" Particle System ▼
            crushEffect.Play();

            // ▼ "Getting" the "Audio Source Component" 
            //     → and "Play It" when the "Player Hits" the "Ground" ▼
            GetComponent<AudioSource>().PlayOneShot(crushSFX);

            // ▼ "Create" a "Delay" of "1 Seconds" 
            //      → to "Call" the "ReloadScene()" Method ▼
            Invoke("ReloadScene", loadDelay);
        }
   }

    public void GetEffect()
    {
        crushEffect.Play();
    }

    // ▬ "ReloadScene()" Method 
    //       → to "Reload" the "Level 1" Scene
    //       → when he "Player Hits" the "Ground" ▬
    void ReloadScene()
   {
        // ▼ "LoadScene()" Built-In Method
        //      → from the "SceneManager" Class 
        //      → which will "Load" our "Level 1" Scene, 
        //      → with "Index 0" ▼

        // Lưu điểm vào PlayerPrefs với key "FinalScore"
        PlayerPrefs.SetInt("FinalScore", finalScore);
        SceneManager.LoadScene(0);
   }
}
