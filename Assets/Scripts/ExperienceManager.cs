using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public int yourLevel = 1;
    public int currentExperience = 0;
    public int experienceRequiredToLevelUp = 10;
    public UpdateHealth updateHealth;
    public ParticleSystem levelUpParticles;
    public Sounds sounds;
    public ItemManager itemManager;
    public Drops drops;

    public void UpdateExperience (int newExperience)
    {
        currentExperience += newExperience;
        currentExperience = Mathf.Clamp(currentExperience, 0, 50500);
        while (currentExperience >= experienceRequiredToLevelUp)
        {
            levelUpParticles.Play(true);
            sounds.PlaySound(16);
            yourLevel += 1;
            GetComponent<Stats>().maxHealth += 50;
            GetComponent<Stats>().currentHealth = (int)GetComponent<Stats>().maxHealth;
            updateHealth.UpdateLife(-1, 1);
            experienceRequiredToLevelUp += 10 * yourLevel;
        }
    }

    public void LooseExperience ()
    {
        int previusExperience = currentExperience;
        currentExperience = (int)(currentExperience * 0.8f);
        yourLevel = 1;
        experienceRequiredToLevelUp = 10;
        
        GetComponent<Stats>().maxHealth = 100;
        while (currentExperience >= experienceRequiredToLevelUp)
        {
            yourLevel += 1;
            GetComponent<Stats>().maxHealth += 50;
            experienceRequiredToLevelUp += 10 * yourLevel;
        }
        
        if (currentExperience != 0)
        {
            itemManager.AddItem(drops.allItems.Find(x => x.itemName == "Experience"), -(previusExperience - currentExperience));
        }
        else if (itemManager.items.ContainsKey(drops.allItems.Find(x => x.itemName == "Experience")))
        {
            itemManager.items.Remove(drops.allItems.Find(x => x.itemName == "Experience"));
        }
    }
}