using UnityEngine;
using System.Collections;

public class LimbSpring : MonoBehaviour {
    //public variables open to change by users
    [Header("Spring Coefficient")]
    [SerializeField]
    float kineticCoefficient;
    [SerializeField]
    float dampingCoefficient;
    [SerializeField]
    float proportionToOriginalLength;
    [SerializeField]
    float FramePerSecond;

    //spring motion attributes
    private float timer, originLength, currentLength;
    private float currentStretch = 0, speed = 0;

    //*This balances the fact that mouse moving distance
    //  is not the same as the distance in game scene
    //*I think I have fixed this, just put it one in cese
    private float offset = 1;

    //gameobject attributs
    private Rigidbody body;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody>();

        //set up motion rate of spring system
        FramePerSecond = 1.0f / FramePerSecond;
        timer = FramePerSecond;

        //get the original length by unprojection
        //*this now is mapped to mouse moving distance
        Vector3 temp1 = Camera.main.WorldToScreenPoint(transform.localPosition + GetComponent<Renderer>().bounds.size.y * Vector3.up);
        Vector3 temp2 = Camera.main.WorldToScreenPoint(transform.localPosition - GetComponent<Renderer>().bounds.size.y * Vector3.up);
        originLength = Vector3.Magnitude(temp1 - temp2); Debug.Log(gameObject.name+" Spring length:"+originLength);
        currentLength = originLength;
    }

    // Update is called once per frame
    void Update () {
        //test input functions
        //*Manually change its length
        if (Input.GetKeyDown(KeyCode.A))
            currentLength = proportionToOriginalLength * currentLength;
        //*Manually to restore its length to stop
        if (Input.GetKeyDown(KeyCode.S))
            currentLength = originLength;
        bounce();
    }

    //use explicit method to derive the motion of spring
    //use length & speed variables to simulate physics
    void bounce()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {   
            //F=-k(l-l'), if l=l', there is no motion
            if(Mathf.Abs(currentLength- originLength)>0.001)
            {
                // y(n+1)=y(n)+h*v(n)
                currentLength = currentLength + speed;

                // v(n+1)=v(n)+h*a(n)       F=-k(l-l')-DampingForce
                speed = speed + (-kineticCoefficient * (currentLength - originLength) - dampingCoefficient * speed);

                transform.localScale = new Vector3(1, currentLength / originLength, 1);
                currentStretch = (currentLength - originLength) / originLength;
                transform.localPosition = transform.localPosition + 0.5f * ((currentLength - originLength) / originLength) * Vector3.up;
            }
            timer = FramePerSecond;
        }
    }

    //bounce bool checks if it applies bouncing
    //since we only apply motion after releasing mouse
    public void stretch(float dragDistance,bool bounce)
    {
        currentStretch = 0; currentLength = originLength;

        dragDistance = (dragDistance-originLength) /offset;
        currentStretch = dragDistance / originLength;
        if (bounce) {
            currentLength = originLength + dragDistance;
            transform.localScale = new Vector3(1, 1 + currentStretch, 1);
            transform.localPosition = transform.localPosition + 0.5f * ((currentLength - originLength) / originLength) * Vector3.up;
        }
        else
        {   
            transform.localScale = new Vector3(1, 1+ currentStretch, 1);
        }
    }
}
