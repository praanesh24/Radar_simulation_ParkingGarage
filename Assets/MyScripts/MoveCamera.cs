using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
	public bool theTrigger = false;
	public Vector3 step = new Vector3(0.0f,0.0f,-0.01f);

	int counter = 0;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 cpos = this.transform.position;
		if (counter <= 500) {
			if (cpos.z < 5 && theTrigger)
			{
				this.transform.position += step;
				//theTrigger = false;
				//counter = 0;
			}
		}
		if (counter > 500) { 
				  if (cpos.z < 5 && theTrigger)
				{
				this.transform.position -= step;
				//theTrigger = false;

			}
		}
		if (counter ==1000)
			counter = 0;
		//else
        //{
			counter = counter + 1;
        //}
		/*else if(cpos.z <= 0)
		{
			//UnityEditor.EditorApplication.isPlaying = false;
		}*/
    }
}
