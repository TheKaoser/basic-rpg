using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drops : MonoBehaviour
{
    public float experienceBoost = 1f;
    public float fortuneBoost = 1f;

    Dictionary <string, float> baseDrops = new Dictionary<string, float>();
    Dictionary <string, float> baseGear = new Dictionary<string, float>();

    Dictionary <Item, int> itemsDropped = new Dictionary<Item, int>();

    public List <Item> allItems = new List<Item>();

    public Transform modifiableItems;

    void Start()
    {
        baseDrops.Add("Soul", 20f);
        baseDrops.Add("Rune", 0.5f);
        baseDrops.Add("Trading token", 8f);

        baseDrops.Add("Health scroll", 1f);
        baseDrops.Add("Damage scroll", 1f);
        baseDrops.Add("Fortune scroll", 1f);
        baseDrops.Add("Experience scroll", 1f);
        baseDrops.Add("Defense scroll", 1f);

        baseDrops.Add("Health potion", 10f);
        baseDrops.Add("Damage potion", 2f);
        baseDrops.Add("Defense potion", 2f);
        baseDrops.Add("Experience potion", 2f);
        baseDrops.Add("Fortune potion", 2f);

        baseDrops.Add("Heaven stone", 0.6f);
        baseDrops.Add("Damned stone", 0.3f);
        baseDrops.Add("Protection stone", 0.3f);
        
        baseGear.Add("sword", 0.6f);
    }

    public Dictionary <Item, int> ChooseItems(string place, int stage, int level, int yourlevel)
    {    
        itemsDropped.Clear();
        float levelmultiplier = 1f;

        if (yourlevel > level) 
        {
            levelmultiplier = 1f - (((float)yourlevel/4f - (float)level) * 0.5f);
        }
        else if (level > yourlevel)
        {
            levelmultiplier = 1f + ((level - (float)yourlevel/4f) * 0.35f);
        }
        
        float experienceDrop = 1f;

        // float currentLevelStat = Mathf.Pow(1.05f, ((float)level - 1f));
        float currentLevelStat = 1f;
        
        float dropRate = currentLevelStat * levelmultiplier;

        if (stage >= 4 && stage <= 6)
        {
            dropRate *= 2.2f;
            experienceDrop *= 2.2f;
        }
        else if (stage >= 7 && stage <= 9)
        {
            dropRate *= 3.4f;
            experienceDrop *= 3.4f;
        }
        else if (stage >= 10)
        {
            dropRate *= 5f;
            experienceDrop *= 5f;
        }

        experienceDrop *= experienceBoost;
        dropRate *= fortuneBoost;

        if (place == "forest")
        {
            baseDrops.Add("Enchanted essence", 20f);
        }
        else if (place == "volcano")
        {
            baseDrops.Add("Demonic essence", 20f);
        }
        else if (place == "dungeon")
        {
            baseDrops.Add("Dark essence", 20f);
        }
        
        foreach (string s in baseDrops.Keys)
        {            
            int possibleNumber = 0; 
           
            
            if ((baseDrops[s] * dropRate) % 100f >= UnityEngine.Random.Range(0f, 100f))
            {  
                possibleNumber += (int)((baseDrops[s] * dropRate)/100f + 1);
            }
            else
            {
                possibleNumber += (int)((baseDrops[s] * dropRate)/100f);
            }

            float multiplier = UnityEngine.Random.Range(1f,100f);
            if(multiplier < 50)
            {
                multiplier = 0;
            }
            else if (multiplier < 80)
            {
                multiplier = 1;
            }
            else if (multiplier < 90)
            {
                multiplier = 2;
            }
            else if (multiplier < 95)
            {
                multiplier = 3;
            }
            else
            {
                multiplier = 4;
            }

            if (possibleNumber > 0 && multiplier > 0)
            {
                if (s == "Rune")
                {
                    Item newRune = null;
                    
                    Vector2Int rune = GenerateRune();
                    if (rune.x == 0)
                    {
                        newRune = GameObject.Instantiate(allItems.Find(x => x.itemName == "Weakening rune"), modifiableItems);
                    }
                    else if (rune.x == 1)
                    {
                        newRune = GameObject.Instantiate(allItems.Find(x => x.itemName == "Aura rune"), modifiableItems);
                    }
                    else if (rune.x == 2)
                    {
                        newRune = GameObject.Instantiate(allItems.Find(x => x.itemName == "Critical rune"), modifiableItems);
                    }
                    else if (rune.x == 3)
                    {
                        newRune = GameObject.Instantiate(allItems.Find(x => x.itemName == "Shield rune"), modifiableItems);
                    }
                    else if (rune.x == 4)
                    {
                        newRune = GameObject.Instantiate(allItems.Find(x => x.itemName == "Agility rune"), modifiableItems);
                    }
                    else if (rune.x == 5)
                    {
                        newRune = GameObject.Instantiate(allItems.Find(x => x.itemName == "Overload rune"), modifiableItems);
                    }
                    else if (rune.x == 6)
                    {
                        newRune = GameObject.Instantiate(allItems.Find(x => x.itemName == "Endurance rune"), modifiableItems);
                    }
                    else if (rune.x == 7)
                    {
                        newRune = GameObject.Instantiate(allItems.Find(x => x.itemName == "Divine rune"), modifiableItems);
                    }
                    else if (rune.x == 8)
                    {
                        newRune = GameObject.Instantiate(allItems.Find(x => x.itemName == "Regeneration rune"), modifiableItems);
                    }
                    else if (rune.x == 9)
                    {
                        newRune = GameObject.Instantiate(allItems.Find(x => x.itemName == "Drain rune"), modifiableItems);
                    }

                    newRune.effect = rune.y;
                    itemsDropped.Add(newRune, 1);
                }
                else if (s == "Soul" || s == "Health potion" || s == "Enchanted essence" || s == "Dark essence" || s == "Demonic essence")
                {
                    itemsDropped.Add(allItems.Find(x => x.itemName == s), (int)(possibleNumber * multiplier));
                }        
                else  
                {
                    itemsDropped.Add(allItems.Find(x => x.itemName == s), 1);
                }  
            }           
        }

        if (place == "forest")
        {
            baseDrops.Remove("Enchanted essence");
        }
        else if (place == "volcano")
        {
            baseDrops.Remove("Demonic essence");
        }
        else if (place == "dungeon")
        {
            baseDrops.Remove("Dark essence");
        }

        int expObtained = (int)((experienceDrop * levelmultiplier) *5f);
        if (expObtained < 0)
        {
            expObtained = 0;
        }
        if (expObtained != 0)
        {
            itemsDropped.Add(allItems.Find(x => x.itemName == "Experience"), expObtained);
        }

        ChooseGear(place, level, stage);

        return itemsDropped;
    }

    void ChooseGear(string place, int level, int stage)
    {
        int levelInTier = level - 1;
        int tier = tier = level - 1;
        tier = tier / 5;
        tier += 1;
        levelInTier = levelInTier % 5;  
        levelInTier += 1;

        float dropRate = Mathf.Pow(1.05f, ((float)levelInTier - 1f));  

        if (stage >= 4 && stage <= 6)
        {
            dropRate *= 1.2f;
        }
        else if (stage >= 7 && stage <= 9)
        {
            dropRate *= 3.4f;
        }
        else if (stage >= 10)
        {
            dropRate *= 5f;
        }

        if (place == "forest")
        {
            baseGear.Add("gloves", 1.5f);
            baseGear.Add("shield", 1.5f);
        }
        else if (place == "volcano")
        {
            baseGear.Add("helmet", 1.5f);
            baseGear.Add("chest", 1.5f);
        }
        else if (place == "dungeon")
        {
            baseGear.Add("boots", 1.5f);
            baseGear.Add("greaves", 1.5f);
        }

        foreach (string s in baseGear.Keys)
        {
            if (baseGear[s] * dropRate >= UnityEngine.Random.Range(1f,100f))
            {   
                float upgrade = UnityEngine.Random.Range(1f,100f);
                if (upgrade < 50)
                {
                    upgrade = 0;
                }
                else if (upgrade < 80)
                {
                    upgrade = 1;
                }
                else if (upgrade < 90)
                {
                    upgrade = 2;
                }
                else if (upgrade < 95)
                {
                    upgrade = 3;
                }
                else
                {
                    upgrade = 4;
                }

                Item gear = null;
                if (tier == 1)
                {
                    gear = GameObject.Instantiate(allItems.Find(x => x.itemName == "Traveler's " + s), modifiableItems);
                }
                else if (tier == 2)
                {
                    gear = GameObject.Instantiate(allItems.Find(x => x.itemName == "Explorer's " + s), modifiableItems);
                }
                else if (tier == 3)
                {
                    gear = GameObject.Instantiate(allItems.Find(x => x.itemName == "Soldier's " + s), modifiableItems);
                }
                else if (tier == 4)
                {
                    gear = GameObject.Instantiate(allItems.Find(x => x.itemName == "Master's " + s), modifiableItems);
                }
                else if (tier == 5)
                {
                    gear = GameObject.Instantiate(allItems.Find(x => x.itemName == "Assassin's " + s), modifiableItems);
                }
                if (upgrade != 0)
                {
                    gear.itemName += " +" + upgrade;

                    if (gear.type == "Sword")
                    {
                        gear.effect += 20 * upgrade;
                    }
                    else
                    {
                        gear.effect += 4 * upgrade;
                    }
                }
                if (UnityEngine.Random.Range(1f,100f) < 20)
                {
                    gear.rune = GenerateRune();
                }
                itemsDropped.Add(gear, 1);
            }
        }

        if (place == "forest")
        {
            baseGear.Remove("gloves");
            baseGear.Remove("shield");
        }
        else if (place == "volcano")
        {
            baseGear.Remove("helmet");
            baseGear.Remove("chest");
        }
        else if (place == "dungeon")
        {
            baseGear.Remove("boots");
            baseGear.Remove("greaves");
        } 
    }

    Vector2Int GenerateRune ()
    {
        float runebooster = UnityEngine.Random.Range(1,100f);

        if (runebooster < 60)
        {
            runebooster = 1;
        }
        else if (runebooster < 80)
        {
            runebooster = 2;
        } 
        else if (runebooster < 90)
        {
            runebooster = 3;
        }
        else if (runebooster < 95)
        {
            runebooster = 4;
        }
        else
        {
            runebooster = 5;
        }

        return new Vector2Int (UnityEngine.Random.Range(0,10), (int)runebooster);
    }
}