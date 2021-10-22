using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchArea : MonoBehaviour
{
    public static TouchArea Instance;
    public Vector3 mouseStartPos;
    public Vector3 mouseActualPos;
    public bool isTouch = false;

    Vector3 startLenght;
    Vector3 currentLenght;
    float aLenght;
    public GameObject Puzzle;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTouch)
        {
            startLenght = currentLenght;
        }

        mouseActualPos = Input.mousePosition;
        currentLenght = mouseActualPos - Camera.main.WorldToScreenPoint(Puzzle.transform.position);
        currentLenght.z = 0;

        if (isTouch && !GameControl.Instance.isFinish)
        {
            startLenght = mouseStartPos - Camera.main.WorldToScreenPoint(Puzzle.transform.position);

            startLenght.z = 0;

            aLenght = Vector3.SignedAngle(startLenght, currentLenght, Vector3.forward);

            foreach (int i in GameControl.Instance.PiecesList)
            {

                Puzzle.transform.GetChild(i).transform.localEulerAngles = new Vector3(0, 0, Puzzle.transform.GetChild(i).GetComponent<ObjectControl>().startRot.z + aLenght);

            }


        }
      
    }

    public void OnMouseDown()
    {
        Vibration.Vibrate(10);

        isTouch = true;
        mouseStartPos = Input.mousePosition;
          foreach (int i in GameControl.Instance.PiecesList)
          {
            Puzzle.transform.GetChild(i).GetComponent<ObjectControl>().startRot = Puzzle.transform.GetChild(i).transform.localEulerAngles;
            Puzzle.transform.GetChild(i).GetComponent<ObjectControl>().isTouchingGameObject = true;

          }
    }
    public void OnMouseUp()
    {
        foreach (int i in GameControl.Instance.PiecesList)
        {
            Puzzle.transform.GetChild(i).GetComponent<ObjectControl>().isTouchingGameObject = false;

        }
        isTouch = false;
    }
}
