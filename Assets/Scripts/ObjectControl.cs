using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControl : MonoBehaviour
{
    public static ObjectControl Instance;
    

    public bool isTouchingGameObject = false;
    public bool isPieceComplete = false;
    public bool isVibAvailable = true;

    Vector3 mouseStartPos;
    Vector3 mouseActualPos;
    Vector3 startLenght;
    Vector3 currentLenght;
    public Vector3 startRot;
    float aLenght;

    float axis;
    float startYAxis;
    float startZAxis;
    float maxTurnSpeed = 1.5f;
    float touchStartTime;
    float touchEndTime;

    float turnSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        startYAxis = gameObject.transform.localEulerAngles.y;
        startZAxis = gameObject.transform.localEulerAngles.z;


    }

    // Update is called once per frame
    void Update()
    {

        if (gameObject.tag == "yAxis")
        {
            axis = gameObject.transform.localEulerAngles.y;
        }
        if (gameObject.tag == "zAxis")
        {
            axis = gameObject.transform.localEulerAngles.z;
        }

        //CHECK İF PİECE COMPLETE


        if (gameObject.transform.parent.tag == "normal")
        {
            if (axis < 10 || axis > 350)
            {
                isPieceComplete = true;
            }
            if (axis > 10 && axis < 350)
            {
                isPieceComplete = false;
            }

        }
        

        if (isTouchingGameObject)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 165);
        }


        if (!isTouchingGameObject)
        {
            startLenght = currentLenght;
        }

        mouseActualPos = Input.mousePosition;
        currentLenght = mouseActualPos - Camera.main.WorldToScreenPoint(gameObject.transform.position);
        currentLenght.z = 0;

        if (isTouchingGameObject && !GameControl.Instance.isFinish &&!TouchArea.Instance.isTouch)
        {
            startLenght = mouseStartPos - Camera.main.WorldToScreenPoint(gameObject.transform.position);
            
            startLenght.z = 0;

            aLenght = Vector3.SignedAngle(startLenght,currentLenght,Vector3.forward);

            gameObject.transform.localEulerAngles = new Vector3(0, 0, startRot.z + aLenght);

        }
        

        //PLAY SOUND

        if (gameObject.transform.localEulerAngles.y > startYAxis + 10 || gameObject.transform.localEulerAngles.y < startYAxis - 10 || gameObject.transform.localEulerAngles.z > startZAxis + 10 || gameObject.transform.localEulerAngles.z < startZAxis - 10)
        {
            if (!AudioManager.Instance.audioSource.isPlaying)
            {
                AudioManager.Instance.audioSource.Play();
            }
 
            startYAxis = gameObject.transform.localEulerAngles.y;
            startZAxis = gameObject.transform.localEulerAngles.z;
        }
         
    }
    
    public void OnMouseDown()
    {
        Vibration.Vibrate(10);

        mouseStartPos = Input.mousePosition;

        startRot = transform.localEulerAngles;

        for (int i = 0; i < gameObject.transform.parent.childCount; i++)
        {
            gameObject.transform.parent.GetChild(i).GetComponent<ObjectControl>().isTouchingGameObject = false;
        }

        isTouchingGameObject = true;

        touchStartTime = Time.time;

        mouseStartPos = Input.mousePosition;

        

    }
    public void OnMouseDrag()
    {
        touchEndTime = Time.time;
        

    }
    public void OnMouseUp()
    {
        isTouchingGameObject = false;
        touchEndTime = 0;
        touchStartTime = 0;
    }


}
