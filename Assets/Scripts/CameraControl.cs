using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject Puzzle;
    Vector2 fingersStartPos;
    Vector2 a;
    Vector3 cameraStartPos;

    // Start is called before the first frame update
    void Start()
    {
        cameraStartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(Puzzle.transform);

        if (Input.touchCount != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                fingersStartPos = Input.touches[0].position;
            }
        }

        if (Input.touchCount > 1)
        {
            a = (Input.touches[0].position - fingersStartPos)/10;

            gameObject.transform.Translate(a.x *Time.deltaTime, a.y*Time.deltaTime, 0,Space.World);

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -4.1f, 4.1f), Mathf.Clamp(transform.position.y, -4.1f, 4.1f), transform.position.z);

            if ( Mathf.Abs( transform.position.x) == 4.1f)
            {
                fingersStartPos.x = Input.touches[0].position.x;
            }

            if (Mathf.Abs(transform.position.y) == 4.1f)
            {
                fingersStartPos.y = Input.touches[0].position.y;
            }

        }

        if(Input.touchCount < 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, cameraStartPos, 10f * Time.deltaTime);
        }

        gameObject.transform.LookAt(Puzzle.transform);

    }

}
