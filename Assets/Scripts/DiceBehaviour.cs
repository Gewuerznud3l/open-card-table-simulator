using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceBehaviour : MonoBehaviour
{
    public List<int> rotations;
    public float speed = 500;
    private bool rotating;
    private int state, target;

    // Start is called before the first frame update
    void Start()
    {
        rotations = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rotations.Count > 0)
        {
            if (!rotating)
            {
                target = rotations[0];
                rotations.RemoveAt(0);
                rotating = true;
                state = 0;
            }
        }
        if (rotating)
        {
            switch (target)
            {
                case 0: transform.Rotate(9, 0, 0); break;
                case 1: transform.Rotate(0, 9, 0); break;
                case 2: transform.Rotate(0, 0, 9); break;
                case 3: transform.Rotate(0, 0, -9); break;
                case 4: transform.Rotate(0, -9, 0); break;
                case 5: transform.Rotate(-9, 0, 0); break;
            }
            state++;
        }
        if (state == 10) { rotating = false; state = 0; }
        
    }

    public void Rotate(int rot)
    {
        rotations.Add(rot);
    }
}
