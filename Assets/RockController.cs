using UnityEngine;
using UnityEngine.SceneManagement;

public class RockController : MonoBehaviour
{
    [SerializeField] float loadDelay = 0.5f;        
    [SerializeField] AudioClip crushSFX;            
	Rigidbody2D rb2d;
    private bool scored = false;
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            FindAnyObjectByType<PlayerController>().DisableControls();
            GetComponent<AudioSource>().PlayOneShot(crushSFX);
            Invoke("ReloadScene", loadDelay);
            var crush = collision.gameObject.GetComponent<CrushDetector>();
            crush.GetEffect();
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
