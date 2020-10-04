using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    private bool isRewinding = false;

    Stack<Vector3> positions;

    void Start()
    {
        positions = new Stack<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if(isRewinding)
        {
            RewindTimeBody();
        }
        else
        {
            Record();
        }
    }

    void RewindTimeBody()
    {
        if(positions.Count > 0)
        {
            transform.position = positions.Pop();
        }
        else
        {
            StopRewindTimeBody();

        }
    }

    void Record()
    {
        positions.Push(transform.position);
    }

    public void StartRewindTimeBody()
    {
        isRewinding = true;
    }

    public void StopRewindTimeBody()
    {
        isRewinding = false;
    }
}
