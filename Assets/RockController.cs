using UnityEngine;
using UnityEngine.SceneManagement;

public class RockController : MonoBehaviour
{
    [SerializeField] float loadDelay = 0.5f;        
    [SerializeField] AudioClip crushSFX;            
	Rigidbody2D rb2d;
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            FindAnyObjectByType<PlayerController>().DisableControls();
            GetComponent<AudioSource>().PlayOneShot(crushSFX);
            var crush = collision.gameObject.GetComponent<CrushDetector>();
            crush.GetEffect();
            PlayerPrefs.SetInt("FinalScore", crush.GetScore());
            Invoke("ReloadScene", loadDelay);

        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
