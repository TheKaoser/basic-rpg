using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSpawner : MonoBehaviour
{
    public GameObject textYou;
    public GameObject textFoe;

    public GameObject text0You;
    public GameObject text1You;
    public GameObject text2You;
    public GameObject text0Foe;
    public GameObject text1Foe;
    public GameObject text2Foe;
    public GameObject aux;

    public Dictionary <int, GameObject> textsYou = new Dictionary<int, GameObject>();
    public Dictionary <int, GameObject> textsFoe = new Dictionary<int, GameObject>();
    int i;

    public GameManager gameManager;

    void Start ()
    {
        textsYou[0] = text0You;
        textsYou[1] = text1You;
        textsYou[2] = text2You;
        textsFoe[0] = text0Foe;
        textsFoe[1] = text1Foe;
        textsFoe[2] = text2Foe;
    }

    public IEnumerator SpawnNewText(int side, GameObject whoDealsDamage, string text, float animationTime)
    {
        yield return new WaitForSeconds (animationTime);
        
        if (!whoDealsDamage || whoDealsDamage.GetComponent<Stats>().currentHealth <= 0)
        {
            yield break;
        }

        if (side == 0)
        {
            aux = textsYou[0];
            textsYou[0] = textsYou[2];
            textsYou[2] = textsYou[1];
            textsYou[1] = aux;

            Vector3 auxV = textsYou[2].GetComponent<RectTransform>().position;
            textsYou[2].GetComponent<RectTransform>().position = textsYou[0].GetComponent<RectTransform>().position;
            textsYou[0].GetComponent<RectTransform>().position = textsYou[1].GetComponent<RectTransform>().position;
            textsYou[1].GetComponent<RectTransform>().position = auxV;

            textsYou[0].GetComponent<Text>().text = text;
            textsYou[0].GetComponent<Text>().color = new Color(1f, 1f, 1f, 0.75f);
        }
        else
        {
            aux = textsFoe[0];
            textsFoe[0] = textsFoe[2];
            textsFoe[2] = textsFoe[1];
            textsFoe[1] = aux;

            Vector3 auxV = textsFoe[2].GetComponent<RectTransform>().position;
            textsFoe[2].GetComponent<RectTransform>().position = textsFoe[0].GetComponent<RectTransform>().position;
            textsFoe[0].GetComponent<RectTransform>().position = textsFoe[1].GetComponent<RectTransform>().position;
            textsFoe[1].GetComponent<RectTransform>().position = auxV;

            textsFoe[0].GetComponent<Text>().text = text;
            textsFoe[0].GetComponent<Text>().color = new Color(1f, 1f, 1f, 0.75f);
        }
    }
}
