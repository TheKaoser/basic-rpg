using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHealth : MonoBehaviour
{
    public Image maxHealthYou;
    public Image differenceYou;
    public Image currentYou;

    public Image maxHealthFoe;
    public List <Image> differenceFoe = new List<Image>();
    public List <Image> currentFoe = new List<Image>();

    float waitBeforeChangingYou;
    public List <float> waitBeforeGoingDownFoe = new List<float>();

    Color32 grey = new Color32(104,104,104,100);

    void Update()
    {
        if (currentYou.fillAmount < differenceYou.fillAmount)
        {
            if (waitBeforeChangingYou > 0)
            {
                waitBeforeChangingYou -= Time.deltaTime;
            }
            else
            {
                differenceYou.fillAmount -= 0.8f*Time.deltaTime;
                differenceYou.fillAmount = Mathf.Clamp(differenceYou.fillAmount, currentYou.fillAmount, differenceYou.fillAmount);
            }
        }
        else if (currentYou.fillAmount > differenceYou.fillAmount)
        {
            if (waitBeforeChangingYou > 0)
            {
                waitBeforeChangingYou -= Time.deltaTime;
            }
            else
            {
                differenceYou.fillAmount += 0.8f*Time.deltaTime;
                differenceYou.fillAmount = Mathf.Clamp(differenceYou.fillAmount, differenceYou.fillAmount, currentYou.fillAmount);
            }
        }
        else
        {
            waitBeforeChangingYou = 0.2f;
        }

        for (int i = 0; i < 3; i++)
        {
            if (currentFoe[i].fillAmount < differenceFoe[i].fillAmount)
            {
                currentFoe[i].fillAmount = currentFoe[i].fillAmount;
                if (waitBeforeGoingDownFoe[i] > 0)
                {
                    waitBeforeGoingDownFoe[i] -= Time.deltaTime;
                }
                else
                {
                    differenceFoe[i].fillAmount -= 0.8f*Time.deltaTime;
                    differenceFoe[i].fillAmount = Mathf.Clamp(differenceFoe[i].fillAmount, currentFoe[i].fillAmount, differenceFoe[i].fillAmount);
                }
            }
            else if (currentFoe[i].fillAmount > differenceFoe[i].fillAmount)
            {
                if (waitBeforeGoingDownFoe[i] > 0)
                {
                    waitBeforeGoingDownFoe[i] -= Time.deltaTime;
                }
                else
                {
                    differenceFoe[i].fillAmount += 0.8f*Time.deltaTime;
                    differenceFoe[i].fillAmount = Mathf.Clamp(differenceFoe[i].fillAmount, differenceFoe[i].fillAmount, currentFoe[i].fillAmount);
                }
            }
            else
            {
                waitBeforeGoingDownFoe[i] = 0.2f;
            }
        }

        if (currentFoe[0].fillAmount <= 0)
        {
            maxHealthFoe.color = grey;
            maxHealthFoe.fillAmount -= Time.deltaTime*0.8f;
        }
        else
        {
            maxHealthFoe.color = new Color32(255,255,255,200);
            maxHealthFoe.fillAmount = 1;
        }

        // if (currentYou.fillAmount <= 0)
        // {
        //     maxHealthYou.color = grey;
        //     maxHealthYou.fillAmount -= Time.deltaTime*0.8f;
        // }
        // else
        // {
        //     maxHealthYou.color = new Color32(255,255,255,200);
        //     maxHealthYou.fillAmount = 1;
        // }
    }

    public void UpdateLife (int who, float newLife)
    {
        if (who == -1)
        {
            currentYou.fillAmount = newLife;
            
        }
        else
        {
            currentFoe[who].fillAmount = newLife;
        }        
    }

    public void ResetHealths (bool reset)
    {
        if (reset)
        {
            foreach (Image i in GetComponentsInChildren<Image>())
            {
                if (!i.CompareTag("Player"))
                {
                    i.enabled = true;
                }
            }
        }
        else
        {
            foreach (Image i in GetComponentsInChildren<Image>())
            {
                if (!i.CompareTag("Player"))
                {
                    i.enabled = false;
                }
            }
        }
    }
}
