// ▼ The "using" Keyword 
//      → defines the "Namespace" Directive 
//      → that "Contains" a "Class Used" in the "Code" ▼
using UnityEngine;
using UnityEngine.SceneManagement; // ◄◄ "SceneManagement" Namespace ◄◄


public class FinishLine : MonoBehaviour
{
    // ▼ "SerializeField" Attribute
    //      → to Dysplay the "Variable" 
    //      → in the "Inspector" Window ▼
    [SerializeField] float loadDelay = 1f;  // ◄◄ "1 Second" Delay ◄◄
    [SerializeField] ParticleSystem finishEffect;



   // ▬ "On Tregger Enter 2D()" Method 
   //       → with a "Delay" of "2 Seconds"
   //       → to "Call" the "ReloadScene()" Method ▬
   void OnTriggerEnter2D(Collider2D other) 
   {
        // ▼ "If" there is the "Player" ▼
        if (other.tag == "Player")
        {
            
            // ▼ "Acccessing" the "Play()" Method 
            //     of the "Finish Effect" Particle System ▼
            finishEffect.Play();
                      
            // ▼ "Getting" the "Audio Source Component" 
            //     → and "Play It" when the "Player Reaches" the "Finish Line" ▼
            GetComponent<AudioSource>().Play(); 

            // ▼ "Create" a "Delay" of "1 Seconds" 
            //      → to "Call" the "ReloadScene()" Method ▼
            Invoke("ReloadScene", loadDelay);            
        }
   }



   // ▬ "ReloadScene()" Method 
   //       → to "Reload" the "Level 1" Scene
   //       → when he "Player Finishes" the "Level 1" of the "Game" ▬
   void ReloadScene()
   {
        // ▼ "LoadScene()" Built-In Method
        //      → from the "SceneManager" Class 
        //      → which will "Load" our "Level 1" Scene, 
        //      → with "Index 0" ▼
        SceneManager.LoadScene(0);
   }
}
