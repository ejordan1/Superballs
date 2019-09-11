using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class Gatling : MonoBehaviour
    
    //I think the good way to do this is to have in the main script something like "get next gatling pos" and then
    //it handles the firing. that way you could fire any shape from it and it would just be like a thing.
  //this code is already bad but then with the circle gatler it gets terrible. the whole parent system wont work
    //or gatling gun should just have ammo... yeah this is yikes
    {
        private bool firing;
        public BouncyShoot bouncyShootRef;
      private bool circleFiring;
       private float circleTheta;


        void Start()
        {
            //StartCoroutine(gatler());
            //StartCoroutine(gatlerCircle());
        }

        public IEnumerator gatler(float startPos = 0, bool direction = true)
        {
             float ballNumberInCircle = 20;
            
             float sizeUpBy = .03f;
             float originalSize = 4f;
            float step;
             float currentSize;
             float theta = 2 * Mathf.PI * startPos;
            
            //it will relate to the step
            
            currentSize = originalSize;
           
            while (firing)
            {
                
                if (firing)
                {
                    ballNumberInCircle = currentSize * 6.66f;
                    step =  2 * Mathf.PI / ballNumberInCircle;
                    Vector3 vect;
                    if (direction) // equals true
                    {
                        vect  = new Vector3(0 - currentSize * Mathf.Cos(theta),
                            0 - currentSize * Mathf.Sin(theta), 0);
                    }
                    else
                    {
                        vect  = new Vector3(0 + currentSize * Mathf.Cos(theta),
                            0 - currentSize * Mathf.Sin(theta), 0);
                    }
                    bouncyShootRef.ballFire(bouncyShootRef.gameObject, vect, Color.black);
                    theta += step;
                    currentSize += sizeUpBy;
                   
                    
                }
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }
        
        //this is garbage
        public IEnumerator gatlerCircle()
        {
            float circleStep = 2 * Mathf.PI / 10;
            float individualCircleStep = 2 * Mathf.PI / 55;
            while (circleFiring)
            {
                if (circleFiring)
                {
                    
                    Vector3 vect = new Vector3(0 - 2f * Mathf.Cos(circleTheta), 0 - 2f * Mathf.Sin(circleTheta), 0);
                    for (float t = 0; t < 2 * Mathf.PI; t += individualCircleStep)
                    {
                        float x = 0 - 1.2f * Mathf.Cos(t);
                        float y = 0 - 1.2f * Mathf.Sin(t);
                        Vector3 fVect = new Vector3(x + vect.x, y + vect.y, 0);
                        bouncyShootRef.ballFire(bouncyShootRef.gameObject,
                            fVect * Static.ballSeperatness, Color.clear);
                        
                    }
                    circleTheta += circleStep;
                    
                }
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        public void firingOn(float startPos = 0, bool direction = true)
        {
            firing = true;
                StartCoroutine(gatler(startPos, direction));
            
        }
        
        public void firingOff()
        {
            firing = false;
            
           
        }

        
        public void firingCircleToggle()
        {
            circleFiring = !circleFiring;
           
            if (circleFiring)
            {
                StartCoroutine(gatlerCircle());
            }
        }


    }
}


/*
for (float theta = 0; theta < 2 * Mathf.PI; theta += step)
		{
			float x = 0 - size * Mathf.Cos(theta);
			float y = 0 - size * Mathf.Sin(theta);
			BallClass ballS = bouncyShootRef.ballFire(bouncyShootRef.gameObject,
				new Vector3(x, y, 0) * Static.ballSeperatness, Color.clear);
			ballS.ball.transform.parent = modelParent.transform;
			ballStructs.Add(ballS);
		}
*/