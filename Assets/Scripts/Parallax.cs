using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class Parallax : MonoBehaviour
{
	private float startPos, startPosY, length;
	public GameObject cam;
	public float parallaxEffect;
	void Start()
	{
		startPos = transform.position.x;
		length = GetComponent<SpriteRenderer>().bounds.size.x;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		float distance = cam.transform.position.x * parallaxEffect;
		float movement = cam.transform.position.x * (1 - parallaxEffect);
		float distanceY = cam.transform.position.y;

		transform.position = new Vector3(startPos + distance, startPosY + distanceY, transform.position.z);

		if (movement > startPos + length)
		{
			startPos += length;
		}
		else if (movement < startPos - length)
		{
			startPos -= length;
		}
	}
}
