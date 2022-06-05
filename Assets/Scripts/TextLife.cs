using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLife : MonoBehaviour
{
    void Update()
    {
        GetComponent<Text>().color = new Color(1f, 1f, 1f, GetComponent<Text>().color.a - 0.2f*Time.deltaTime);
    }
}
