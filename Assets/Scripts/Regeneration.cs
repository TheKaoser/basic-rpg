using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regeneration : MonoBehaviour
{
    public UpdateHealth updateHealth;
    public SystemsManager systemsManager;
    public GameManager gameManager;

    void Start()
    {
        if (name.Contains("Oponent"))
        {
            updateHealth = GameObject.Find("Combat").GetComponent<UpdateHealth>();
        }
        StartCoroutine(Regenerate());
    }

    IEnumerator Regenerate ()
    {
        while (true)
        {
            if (gameManager && (gameManager.place == "city" || gameManager.place == "sorcerer" || gameManager.place == "well" || gameManager.place == "portal"))
            {
                GetComponent<Stats>().currentHealth += (int)(GetComponent<Stats>().maxHealth * 5f / 100f);
            }
            else
            {
                if (name.Contains("Oponent") && GetComponent<Stats>().currentHealth == 0)
                {
                    Destroy (this);
                }
                else
                {
                    GetComponent<Stats>().currentHealth += (int)(GetComponent<Stats>().maxHealth / 100f);
                }
            }

            if (!name.Contains("Oponent") || GetComponent<Stats>().currentHealth != 0)
            {
                GetComponent<Stats>().currentHealth += (int)((float)GetComponent<Stats>().regeneration * 5f / 100f * GetComponent<Stats>().maxHealth / 100f);
                GetComponent<Stats>().currentHealth = Mathf.Clamp(GetComponent<Stats>().currentHealth, 0, (int)GetComponent<Stats>().maxHealth);
            }
            
            if (updateHealth)
            {
                if (name.Contains("Oponent"))
                {
                    updateHealth.UpdateLife(0, (float)GetComponent<Stats>().currentHealth/(float)GetComponent<Stats>().maxHealth);
                }
                else
                {
                    updateHealth.UpdateLife(-1, (float)GetComponent<Stats>().currentHealth/(float)GetComponent<Stats>().maxHealth);
                }
            }
            if (systemsManager && systemsManager.currentSectionKind == SystemsManager.SectionKind.normal && systemsManager.currentSection == 1)
            {
                systemsManager.UpdateItems(true);
            }
            yield return new WaitForSeconds (2);
        }
    }
}
