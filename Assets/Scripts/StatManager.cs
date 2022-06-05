using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject combatInterface;

    public Dictionary <Item, int> activePotions = new Dictionary<Item, int>();
    public Dictionary <Item, float> activeScrolls = new Dictionary<Item, float>();

    public SystemsManager systemsManager;
    public ItemManager itemManager;

    public Item currentSword;
    public Item currentHelmet;
    public Item currentChest;
    public Item currentGloves;
    public Item currentShield;
    public Item currentGreaves;
    public Item currentBoots;

    public Transform shines;

    public ParticleSystem particleSword;
    public ParticleSystem particleHelmet;
    public ParticleSystem particleChest;
    public ParticleSystem particleGloves;
    public ParticleSystem particleShield;
    public ParticleSystem particleGreaves;
    public ParticleSystem particleBoots;

    public ParticleSystem level0;
    public ParticleSystem level1;
    public ParticleSystem level2;
    public ParticleSystem level3;
    public ParticleSystem level4;
    public ParticleSystem level5;
    public ParticleSystem level6;
    public ParticleSystem level7;
    public ParticleSystem level8;
    public ParticleSystem level9;

    void Update()
    {
        foreach(var item in activeScrolls)
        {
            if (item.Value + 60 * 5 < Time.time)
            {
                if (item.Key.itemName == "Damage scroll")
                {
                    GetComponent<Stats>().damage = GetComponent<Stats>().damage / (1f + (float)item.Key.effect/100f);
                }
                else if (item.Key.itemName == "Defense scroll")
                {
                    GetComponent<Stats>().defense = GetComponent<Stats>().defense / (1f + (float)item.Key.effect/100f);
                }
                else if (item.Key.itemName == "Health scroll")
                {
                    GetComponent<Stats>().maxHealth = GetComponent<Stats>().maxHealth / (1f + (float)item.Key.effect/100f);
                }
                else if (item.Key.itemName == "Fortune scroll")
                {
                    gameManager.GetComponent<Drops>().fortuneBoost = 1f;
                }
                else if (item.Key.itemName == "Experience scroll")
                {
                    gameManager.GetComponent<Drops>().experienceBoost = 1f;
                }
                activeScrolls.Remove(item.Key);
                break;
            }
        }

        if (activeScrolls.Count > 0)
        {
            if (systemsManager.currentSectionKind == SystemsManager.SectionKind.normal && systemsManager.currentSection == 1)
            {
                systemsManager.UpdateItems(true);
            }
        }
    }

    public void ActivatePotion(Item potion)
    {
        if (potion.itemName == "Health potion")
        {
            GetComponent<Stats>().currentHealth += (int)((float)potion.effect * (float)GetComponent<Stats>().maxHealth / 100f);
            GetComponent<Stats>().currentHealth = Mathf.Clamp (GetComponent<Stats>().currentHealth, 41, (int)GetComponent<Stats>().maxHealth);
            combatInterface.GetComponent<UpdateHealth>().UpdateLife(-1, (float)GetComponent<Stats>().currentHealth/(float)GetComponent<Stats>().maxHealth);
        }
        else
        {
            activePotions.Add(potion, gameManager.stage);
            if (potion.itemName == "Damage potion")
            {
                GetComponent<Stats>().damage = GetComponent<Stats>().damage * (1f + (float)potion.effect/100f);
            }
            else if (potion.itemName == "Defense potion")
            {
                GetComponent<Stats>().defense = GetComponent<Stats>().defense * (1f + (float)potion.effect/100f);
            }
            else if (potion.itemName == "Fortune potion")
            {
                gameManager.GetComponent<Drops>().fortuneBoost = 1f + (float)potion.effect/100f;
            }
            else if (potion.itemName == "Experience potion")
            {
                gameManager.GetComponent<Drops>().experienceBoost = 1f + (float)potion.effect/100f;
            }
        }
        gameManager.GetComponent<Sounds>().PlaySound(3);
        systemsManager.UpdateItems(true);
    }

    public void ActivateScroll(Item scroll)
    {
        activeScrolls.Add(scroll, Time.time);
        if (scroll.itemName == "Damage scroll")
        {
            GetComponent<Stats>().damage = GetComponent<Stats>().damage * (1f + (float)scroll.effect/100f);
        }
        else if (scroll.itemName == "Defense scroll")
        {
            GetComponent<Stats>().defense = GetComponent<Stats>().defense * (1f + (float)scroll.effect/100f);
        }
        else if (scroll.itemName == "Health scroll")
        {
            GetComponent<Stats>().maxHealth = GetComponent<Stats>().maxHealth * (1f + (float)scroll.effect/100f);
        }
        else if (scroll.itemName == "Fortune scroll")
        {
            gameManager.GetComponent<Drops>().fortuneBoost = 1f + (float)scroll.effect/100f;
        }
        else if (scroll.itemName == "Experience scroll")
        {
            gameManager.GetComponent<Drops>().experienceBoost = 1f + (float)scroll.effect/100f;
        }
        gameManager.GetComponent<Sounds>().PlaySound(3);
        systemsManager.UpdateItems(true);
    }

    public void EquipGear(Item gear)
    {
        float damageBoost = 1;
        float defenseBoost = 1;

        itemManager.items[gear] = 0;

        foreach(var item in activeScrolls)
        {
            if (item.Key.itemName == "Damage scroll")
            {
               damageBoost *= (1f + (float)item.Key.effect/100f);
            }
            else if (item.Key.itemName == "Defense scroll")
            {
                defenseBoost *= (1f + (float)item.Key.effect/100f);
            }
        }
        foreach(var item in activePotions)
        {
            if (item.Key.itemName == "Damage potion")
            {
               damageBoost *= (1f + (float)item.Key.effect/100f);
            }
            else if (item.Key.itemName == "Defense potion")
            {
                defenseBoost *= (1f + (float)item.Key.effect/100f);
            }
        }

        int upgrade = 0;
        if (gear.itemName.Contains("+"))
        {
            upgrade = int.Parse(gear.itemName.Split('+')[1]);
        }

        ParticleSystem particles = null;

        if (upgrade == 1)
        {
            particles = GameObject.Instantiate(level0, shines);
        }
        else if (upgrade == 2)
        {
            particles = GameObject.Instantiate(level1, shines);
        }
        else if (upgrade == 3)
        {
            particles = GameObject.Instantiate(level2, shines);
        }
        else if (upgrade == 4)
        {
            particles = GameObject.Instantiate(level3, shines);
        }
        else if (upgrade == 5)
        {
            particles = GameObject.Instantiate(level4, shines);
        }
        else if (upgrade == 6)
        {
            particles = GameObject.Instantiate(level5, shines);
        }
        else if (upgrade == 7)
        {
            particles = GameObject.Instantiate(level6, shines);
        }
        else if (upgrade == 8)
        {
            particles = GameObject.Instantiate(level7, shines);
        }
        else if (upgrade == 9)
        {
            particles = GameObject.Instantiate(level8, shines);
        }
        else if (upgrade == 10)
        {
            particles = GameObject.Instantiate(level9, shines);
        }

        if (gear.type == "Sword")
        {
            if (currentSword)
            {
                UnequipGear(currentSword);
            }
            currentSword = gear;
            GetComponent<Stats>().damage += gear.effect * damageBoost;
            transform.GetChild(gear.level * 7 + 0).GetComponent<SpriteRenderer>().enabled = true;

            if (upgrade != 0)
            {
                var shape1 = particles.shape;
                var shape2 = particles.transform.GetChild(0).GetComponent<ParticleSystem>().shape;
                shape1.spriteRenderer = transform.GetChild(gear.level * 7 + 0).GetComponent<SpriteRenderer>();
                shape2.spriteRenderer = transform.GetChild(gear.level * 7 + 0).GetComponent<SpriteRenderer>();
                particleSword = particles;
            }
            systemsManager.sword = true;
        }
        else if (gear.type == "Helmet")
        {
            if (currentHelmet)
            {
                UnequipGear(currentHelmet);
            }
            currentHelmet = gear;
            GetComponent<Stats>().defense += gear.effect * defenseBoost;
            transform.GetChild(gear.level * 7 + 1).GetComponent<SpriteRenderer>().enabled = true;

            if (upgrade != 0)
            {
                var shape1 = particles.shape;
                var shape2 = particles.transform.GetChild(0).GetComponent<ParticleSystem>().shape;
                shape1.spriteRenderer = transform.GetChild(gear.level * 7 + 1).GetComponent<SpriteRenderer>();
                shape2.spriteRenderer = transform.GetChild(gear.level * 7 + 1).GetComponent<SpriteRenderer>();
                particleHelmet = particles;
            }
            systemsManager.helmet = true;
        }
        else if (gear.type == "Chest")
        {
            if (currentChest)
            {
                UnequipGear(currentChest);
            }
            currentChest = gear;
            GetComponent<Stats>().defense += gear.effect * defenseBoost;
            transform.GetChild(gear.level * 7 + 2).GetComponent<SpriteRenderer>().enabled = true;

            if (upgrade != 0)
            {
                var shape1 = particles.shape;
                var shape2 = particles.transform.GetChild(0).GetComponent<ParticleSystem>().shape;
                shape1.spriteRenderer = transform.GetChild(gear.level * 7 + 2).GetComponent<SpriteRenderer>();
                shape2.spriteRenderer = transform.GetChild(gear.level * 7 + 2).GetComponent<SpriteRenderer>();
                particleChest = particles;
            }
            systemsManager.chest = true;
        }
        else if (gear.type == "Gloves")
        {
            if (currentGloves)
            {
                UnequipGear(currentGloves);
            }
            currentGloves = gear;
            GetComponent<Stats>().defense += gear.effect * defenseBoost;
            transform.GetChild(gear.level * 7 + 3).GetComponent<SpriteRenderer>().enabled = true;

            if (upgrade != 0)
            {
                var shape1 = particles.shape;
                var shape2 = particles.transform.GetChild(0).GetComponent<ParticleSystem>().shape;
                shape1.spriteRenderer = transform.GetChild(gear.level * 7 + 3).GetComponent<SpriteRenderer>();
                shape2.spriteRenderer = transform.GetChild(gear.level * 7 + 3).GetComponent<SpriteRenderer>();
                particleGloves = particles;
            }
            systemsManager.gloves = true;
        }
        else if (gear.type == "Shield")
        {
            if (currentShield)
            {
                UnequipGear(currentShield);
            }
            currentShield = gear;
            GetComponent<Stats>().defense += gear.effect * defenseBoost;
            transform.GetChild(gear.level * 7 + 4).GetComponent<SpriteRenderer>().enabled = true;

            if (upgrade != 0)
            {
                var shape1 = particles.shape;
                var shape2 = particles.transform.GetChild(0).GetComponent<ParticleSystem>().shape;
                shape1.spriteRenderer = transform.GetChild(gear.level * 7 + 4).GetComponent<SpriteRenderer>();
                shape2.spriteRenderer = transform.GetChild(gear.level * 7 + 4).GetComponent<SpriteRenderer>();
                particleShield = particles;
            }
            systemsManager.shield = true;
        }
        else if (gear.type == "Greaves")
        {
            if (currentGreaves)
            {
                UnequipGear(currentGreaves);
            }
            currentGreaves = gear;
            GetComponent<Stats>().defense += gear.effect * defenseBoost;
            transform.GetChild(gear.level * 7 + 5).GetComponent<SpriteRenderer>().enabled = true;

            if (upgrade != 0)
            {
                var shape1 = particles.shape;
                var shape2 = particles.transform.GetChild(0).GetComponent<ParticleSystem>().shape;
                shape1.spriteRenderer = transform.GetChild(gear.level * 7 + 5).GetComponent<SpriteRenderer>();
                shape2.spriteRenderer = transform.GetChild(gear.level * 7 + 5).GetComponent<SpriteRenderer>();
                particleGreaves = particles;
            }
            systemsManager.greaves = true;
        }
        else if (gear.type == "Boots")
        {
            if (currentBoots)
            {
                UnequipGear(currentBoots);
            }
            currentBoots = gear;
            GetComponent<Stats>().defense += gear.effect * defenseBoost;
            transform.GetChild(gear.level * 7 + 6).GetComponent<SpriteRenderer>().enabled = true;

            if (upgrade != 0)
            {
                var shape1 = particles.shape;
                var shape2 = particles.transform.GetChild(0).GetComponent<ParticleSystem>().shape;
                shape1.spriteRenderer = transform.GetChild(gear.level * 7 + 6).GetComponent<SpriteRenderer>();
                shape2.spriteRenderer = transform.GetChild(gear.level * 7 + 6).GetComponent<SpriteRenderer>();
                particleBoots = particles;
            }
            systemsManager.boots = true;
        }
        
        if (gear.rune.x == 0)
        {
            GetComponent<Stats>().weakening += gear.rune.y;
            if (GetComponent<Stats>().weakening > 100)
            {
                GetComponent<Stats>().weakening = 100;
            }
        }
        else if (gear.rune.x == 1)
        {
            GetComponent<Stats>().aura += gear.rune.y;
            if (GetComponent<Stats>().aura > 100)
            {
                GetComponent<Stats>().aura = 100;
            }
        }
        else if (gear.rune.x == 2)
        {
            GetComponent<Stats>().critical += gear.rune.y;
            if (GetComponent<Stats>().critical > 100)
            {
                GetComponent<Stats>().critical = 100;
            }
        }
        else if (gear.rune.x == 3)
        {
            GetComponent<Stats>().shield += gear.rune.y;
            if (GetComponent<Stats>().shield > 100)
            {
                GetComponent<Stats>().shield = 100;
            }
        }
        else if (gear.rune.x == 4)
        {
            GetComponent<Stats>().agility += gear.rune.y;
            if (GetComponent<Stats>().agility > 100)
            {
                GetComponent<Stats>().agility = 100;
            }
        }
        else if (gear.rune.x == 5)
        {
            GetComponent<Stats>().overload += gear.rune.y;
            if (GetComponent<Stats>().overload > 100)
            {
                GetComponent<Stats>().overload = 100;
            }
        }
        else if (gear.rune.x == 6)
        {
            GetComponent<Stats>().endurance += gear.rune.y;
            if (GetComponent<Stats>().endurance > 100)
            {
                GetComponent<Stats>().endurance = 100;
            }
        }
        else if (gear.rune.x == 7)
        {
            GetComponent<Stats>().divine += gear.rune.y;
            if (GetComponent<Stats>().divine > 100)
            {
                GetComponent<Stats>().divine = 100;
            }
        }
        else if (gear.rune.x == 8)
        {
            GetComponent<Stats>().regeneration += gear.rune.y;
            if (GetComponent<Stats>().regeneration > 100)
            {
                GetComponent<Stats>().regeneration = 100;
            }
        }
        else if (gear.rune.x == 9)
        {
            GetComponent<Stats>().drain += gear.rune.y;
            if (GetComponent<Stats>().drain > 100)
            {
                GetComponent<Stats>().drain = 100;
            }
        }
        systemsManager.currentSection = 0;
        systemsManager.UpdateItems(true);
    }

    public void UnequipGear(Item gear)
    {
        float damageBoost = 1;
        float defenseBoost = 1;

        itemManager.items[gear] = 1;

        foreach(var item in activeScrolls)
        {
            if (item.Key.itemName == "Damage scroll")
            {
               damageBoost *= (1f + (float)item.Key.effect/100f);
            }
            else if (item.Key.itemName == "Defense scroll")
            {
                defenseBoost *= (1f + (float)item.Key.effect/100f);
            }
        }
        foreach(var item in activePotions)
        {
            if (item.Key.itemName == "Damage potion")
            {
               damageBoost *= (1f + (float)item.Key.effect/100f);
            }
            else if (item.Key.itemName == "Defense potion")
            {
                defenseBoost *= (1f + (float)item.Key.effect/100f);
            }
        }

        if (gear.type == "Sword")
        {
            currentSword = null;
            GetComponent<Stats>().damage -= gear.effect * damageBoost;
            transform.GetChild(gear.level * 7 + 0).GetComponent<SpriteRenderer>().enabled = false;
            systemsManager.sword = false;
            if (particleSword)
            {
                Destroy(particleSword.transform.GetChild(0).gameObject);
                Destroy(particleSword);
            }
        }
        else if (gear.type == "Helmet")
        {
            currentHelmet = null;
            GetComponent<Stats>().defense -= gear.effect * defenseBoost;
            transform.GetChild(gear.level * 7 + 1).GetComponent<SpriteRenderer>().enabled = false;
            systemsManager.helmet = false;
            if (particleHelmet)
            {
                Destroy(particleHelmet.transform.GetChild(0).gameObject);
                Destroy(particleHelmet);
            }
        }
        else if (gear.type == "Chest")
        {
            currentChest = null;
            GetComponent<Stats>().defense -= gear.effect * defenseBoost;
            transform.GetChild(gear.level * 7 + 2).GetComponent<SpriteRenderer>().enabled = false;
            systemsManager.chest = false;
            if (particleChest)
            {
                Destroy(particleChest.transform.GetChild(0).gameObject);
                Destroy(particleChest);
            }
        }
        else if (gear.type == "Gloves")
        {
            currentGloves = null;
            GetComponent<Stats>().defense -= gear.effect * defenseBoost;
            transform.GetChild(gear.level * 7 + 3).GetComponent<SpriteRenderer>().enabled = false;
            systemsManager.gloves = false;
            if (particleGloves)
            {
                Destroy(particleGloves.transform.GetChild(0).gameObject);
                Destroy(particleGloves);
            }
        }
        else if (gear.type == "Shield")
        {
            currentShield = null;
            GetComponent<Stats>().defense -= gear.effect * defenseBoost;
            transform.GetChild(gear.level * 7 + 4).GetComponent<SpriteRenderer>().enabled = false;
            systemsManager.shield = false;
            if (particleShield)
            {
                Destroy(particleShield.transform.GetChild(0).gameObject);
                Destroy(particleShield);
            }
        }
        else if (gear.type == "Greaves")
        {
            currentGreaves = null;
            GetComponent<Stats>().defense -= gear.effect * defenseBoost;
            transform.GetChild(gear.level * 7 + 5).GetComponent<SpriteRenderer>().enabled = false;
            systemsManager.greaves = false;
            if (particleGreaves)
            {
                Destroy(particleGreaves.transform.GetChild(0).gameObject);
                Destroy(particleGreaves);
            }
        }
        else if (gear.type == "Boots")
        {
            currentBoots = null;
            GetComponent<Stats>().defense -= gear.effect * defenseBoost;
            transform.GetChild(gear.level * 7 + 6).GetComponent<SpriteRenderer>().enabled = false;
            systemsManager.boots = false;
            if (particleBoots)
            {
                Destroy(particleBoots.transform.GetChild(0).gameObject);
                Destroy(particleBoots);
            }
        }

        if (gear.rune.x == 0)
        {
            GetComponent<Stats>().weakening -= gear.rune.y;
        }
        else if (gear.rune.x == 1)
        {
            GetComponent<Stats>().aura -= gear.rune.y;
        }
        else if (gear.rune.x == 2)
        {
            GetComponent<Stats>().critical -= gear.rune.y;
        }
        else if (gear.rune.x == 3)
        {
            GetComponent<Stats>().shield -= gear.rune.y;
        }
        else if (gear.rune.x == 4)
        {
            GetComponent<Stats>().agility -= gear.rune.y;
        }
        else if (gear.rune.x == 5)
        {
            GetComponent<Stats>().overload -= gear.rune.y;
        }
        else if (gear.rune.x == 6)
        {
            GetComponent<Stats>().endurance -= gear.rune.y;
        }
        else if (gear.rune.x == 7)
        {
            GetComponent<Stats>().divine -= gear.rune.y;
        }
        else if (gear.rune.x == 8)
        {
            GetComponent<Stats>().regeneration -= gear.rune.y;
        }
        else if (gear.rune.x == 9)
        {
            GetComponent<Stats>().drain -= gear.rune.y;
        }
        itemManager.currentSection = 0;
        itemManager.UpdateItems(true);
    }

    public void DisablePotionEffect ()
    {
        foreach(var item in activePotions)
        {
            if (item.Key.itemName == "Damage potion")
            {
                GetComponent<Stats>().damage = (GetComponent<Stats>().damage / (1f + (float)item.Key.effect/100f));
            }
            else if (item.Key.itemName == "Defense potion")
            {
                GetComponent<Stats>().defense = (GetComponent<Stats>().defense / (1f + (float)item.Key.effect/100f));
            }
            else if (item.Key.itemName == "Fortune potion")
            {
                gameManager.GetComponent<Drops>().fortuneBoost = 1f;
            }
            else if (item.Key.itemName == "Experience potion")
            {
                gameManager.GetComponent<Drops>().experienceBoost = 1f;
            }
        }
        activePotions.Clear();
    }

    public void DisableScrollEffect ()
    {
        List <Item> scrolls = new List<Item>();
        foreach (var key in activeScrolls.Keys)
        {
            scrolls.Add(key);
        }
        foreach(Item scroll in scrolls)
        {
            activeScrolls[scroll] = -300;
        }
    }
}