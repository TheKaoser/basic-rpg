using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : MonoBehaviour
{
    int startingTime;

    // Start is called before the first frame update
    void Start()
    {
        startingTime = Random.Range(0, 60);

        Invoke("PlayAnimation", startingTime);
    }

    // Update is called once per frame
    void PlayAnimation()
    {
        GetComponent<Animator>().Play("Idle"); 
    }
}
