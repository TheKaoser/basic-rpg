using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arena : MonoBehaviour
{
    public bool readyForAction;
    public StatManager statManager;
    public Text username; 
    public Text oponentUsername; 
    bool increasingOpacity;
    public Image left;
    bool signalActive;

    void Start()
    {
        StartCoroutine (ShowArenaActive());
    }

    public IEnumerator PrepareForCombat()
    {
        GetComponent<Server>().SearchArena();
        yield return new WaitUntil(() => readyForAction);
        
        GetComponent<Server>().SpawnOponent();
        yield return new WaitUntil(() => GetComponent<Server>().itemsOponentSynced);
        
        username.text = GetComponent<Server>().username;
        username.enabled = true;
        oponentUsername.text = GetComponent<Server>().oponent;
        oponentUsername.enabled = true;

        statManager.DisablePotionEffect();
        statManager.DisableScrollEffect();

        GetComponent<MapActions>().canDrag = false;
        GetComponent<GameManager>().GoToInteractPosition();
    }

    IEnumerator ShowArenaActive()
    {
        increasingOpacity = true;

        while (true)
        {
            if (GetComponent<Server>().arenaActive)
            {
                if (GetComponent<MapActions>().isDragging || GetComponent<GameManager>().place == "arena")
                {
                    if (signalActive)
                    {
                        signalActive = false;
                        increasingOpacity = true;
                        left.color = new Color(1, 1, 1, 0);
                    }
                }
                else
                {
                    signalActive = true;
                    if (increasingOpacity)
                    {
                        left.color = new Color(1, 1, 1, Mathf.Clamp(left.color.a + 0.1f, 0f, 0.75f));
                        if (left.color.a == 0.75f)
                        {
                            increasingOpacity = false;
                        }
                    }
                    else
                    {
                        left.color = new Color(1, 1, 1, Mathf.Clamp(left.color.a - 0.1f, 0f, 0.75f));
                        if (left.color.a == 0f)
                        {
                            increasingOpacity = true;
                        }
                    }
                }
            }
            else
            {
                increasingOpacity = true;
                left.color = new Color(1, 1, 1, 0);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
