using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed;
    private Transform Cam, target;
    private int camPos;
    private bool moving, rotating, back;
    // Start is called before the first frame update
    void Start()
    {
        camPos = 0;
        if (transform.GetChild(0).childCount > 0) { Cam = transform.GetChild(0).GetChild(0); } else { Cam = null; }
    }

    // Update is called once per frame
    void Update()
    {
        if (Cam == null) { return; }
        if (Input.GetKey("a")) { transform.Rotate(0, speed * Time.deltaTime, 0); }
        if (Input.GetKey("d")) { transform.Rotate(0, -speed * Time.deltaTime, 0); }
        if (Input.GetKeyDown("q"))
        {
            back = true;
        }
        if (back)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.parent.rotation, speed * Time.deltaTime * 10);
            if (Quaternion.Angle(transform.rotation, transform.parent.rotation) < 0.01f)
            {
                back = false;
                transform.rotation = transform.parent.rotation;
            }
        }
        if (Input.GetKeyDown("w"))
        {
            if (camPos < transform.childCount - 1) 
            { 
                camPos++;
            }
            else
            {
                camPos = 0;
            }
            moving = true;
            rotating = true;
            target = transform.GetChild(camPos);
        }
        if (Input.GetKeyDown("s"))
        {
            if (camPos > 0) 
            {
                camPos--;
            }
            else
            {
                camPos = transform.childCount - 1;
            }
            moving = true;
            rotating = true;
            target = transform.GetChild(camPos);
        }

        if (moving)
        {
            Cam.position = Vector3.MoveTowards(Cam.position, target.position, speed * Time.deltaTime);
            if (Vector3.Distance(Cam.position, target.position) < 0.01f)
            {
                moving = false;
                Cam.parent = target;
                Cam.position = target.position;
            }
        }

        if (rotating)
        {
            Cam.rotation = Quaternion.RotateTowards(Cam.rotation, target.rotation, speed * Time.deltaTime * 3);
            if (Quaternion.Angle(Cam.rotation, target.rotation) < 0.01f)
            {
                rotating = false;
                Cam.rotation = target.rotation;
            }
        }
    }

    public void Reset()
    {
        Cam.position = transform.GetChild(0).position; 
        Cam.rotation = transform.GetChild(0).rotation;
        Cam.parent = transform.GetChild(0);
        camPos = 0;
        back = false;
        rotating = false;
        moving = false;
        transform.rotation = transform.parent.rotation;
    }
}
