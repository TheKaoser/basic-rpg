using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemsManager : MonoBehaviour
{
    public ItemManager itemManager;
    public GameObject itemTab;
    public bool sword;
    public bool helmet;
    public bool chest;
    public bool gloves;
    public bool shield;
    public bool greaves;
    public bool boots;

    public int currentSection;
    
    public Image center;
    public Image left;
    public Image right;
    public List <Sprite> sections = new List<Sprite>();
    public List <Sprite> sorcererSections = new List<Sprite>();
    public List <Sprite> wellSections = new List<Sprite>();
    public Sprite itemDetail;
    public GameObject stat;

    public Sprite experience;
    public Sprite health;
    public Sprite damage;
    public Sprite defense;

    public ExperienceManager experienceManager;

    public List <Sprite> runes = new List<Sprite>();

    public enum SectionKind
    {
        normal = 0,
        itemDetail = 1,
        sorcerer = 2,
        well = 3
    }
    public SectionKind currentSectionKind;

    public StatManager statManager;
    public Drops drops;

    public Image rightOpener;
    public Sprite rewardAvailable;

    public Transform modifiableItems;
    
    public bool processing;

    public Sounds sounds;
    public Server server;

    public GameObject itemInList;

    public void UpdateItems(bool changeSection)
    {
        if (!changeSection)
        {
            itemManager.heavenStone = null;
            itemManager.protectionStone = null;
            itemManager.upgradingItem = null;
            itemManager.upgradingRune = null;
            itemManager.souls = false;
            itemManager.damnedStone = null;
            itemManager.tradingItem = null;
            itemManager.price = 0;
            foreach (Image i in GetComponentsInChildren<Image>())
            {
                if (i.CompareTag("LayoutLeft"))
                {
                    i.enabled = !i.enabled;
                    i.raycastTarget = !i.raycastTarget;
                }
            }

            foreach(Image i in itemTab.GetComponentsInChildren<Image>())
            {
                i.enabled = false;
            }
            foreach(Text t in itemTab.GetComponentsInChildren<Text>())
            {
                t.enabled = false;
            }

            if (currentSectionKind == SectionKind.sorcerer)
            {
                right.sprite = sorcererSections[(currentSection + 1) % (sorcererSections.Count)];
                left.sprite = sorcererSections[(currentSection - 1 + sorcererSections.Count) % (sorcererSections.Count)];
                center.sprite = sorcererSections[currentSection];
            }
            else if (currentSectionKind == SectionKind.normal)
            {
                right.sprite = sections[(currentSection + 1) % (sections.Count)];
                left.sprite = sections[(currentSection - 1 + sections.Count) % (sections.Count)];
                center.sprite = sections[currentSection];
            }
            else if (currentSectionKind == SectionKind.well)
            {
                right.sprite = wellSections[(currentSection + 1) % (wellSections.Count)];
                left.sprite = wellSections[(currentSection - 1 + wellSections.Count) % (wellSections.Count)];
                center.sprite = wellSections[currentSection];
            }

            if (itemManager.currentSectionKind == ItemManager.SectionKind.itemDetail)
            {
                itemManager.ChangeSection ("exitItemDetail");
            }
        }
        else 
        {
            if (currentSectionKind == SectionKind.itemDetail)
            {
                ChangeSection ("exitItemDetail");
            }
        }

        if (transform.GetChild(1).GetChild(0).childCount > 0)
        {
            for (int i = 0; i < transform.GetChild(1).GetChild(0).childCount; i++)
            {
                Destroy(transform.GetChild(1).GetChild(0).GetChild(i).gameObject);
            }
        }

        server.tradingItemsReady = false;
        if (transform.GetChild(8).GetChild(0).childCount > 0)
        {
            for (int i = 0; i < transform.GetChild(8).GetChild(0).childCount; i++)
            {
                Destroy(transform.GetChild(8).GetChild(0).GetChild(i).gameObject);
            }
        }
        
        foreach (Image i in transform.GetChild(0).GetComponentsInChildren<Image>())
        {
            i.enabled = false;
            i.raycastTarget = false;
        }
        foreach (Image i in transform.GetChild(1).GetComponentsInChildren<Image>())
        {
            i.enabled = false;
        }
        foreach (Image i in transform.GetChild(2).GetComponentsInChildren<Image>())
        {
            i.enabled = false;
            i.raycastTarget = false;
        }
        foreach (Text t in transform.GetChild(2).GetComponentsInChildren<Text>())
        {
            t.enabled = false;
        }
        foreach (Image i in transform.GetChild(3).GetComponentsInChildren<Image>())
        {
            i.enabled = false;
            i.raycastTarget = false;
        }
        foreach (Text t in transform.GetChild(3).GetComponentsInChildren<Text>())
        {
            
            t.enabled = false;
        }
        foreach (Image i in transform.GetChild(4).GetComponentsInChildren<Image>())
        {
            i.enabled = false;
            i.raycastTarget = false;
        }
        foreach (Text t in transform.GetChild(4).GetComponentsInChildren<Text>())
        {
            t.enabled = false;
        }
        foreach (Image i in transform.GetChild(5).GetComponentsInChildren<Image>())
        {
            i.enabled = false;
            i.raycastTarget = false;
        }
        foreach (Text t in transform.GetChild(5).GetComponentsInChildren<Text>())
        {
            t.enabled = false;
        }
        foreach (Image i in transform.GetChild(6).GetComponentsInChildren<Image>())
        {
            i.enabled = false;
            i.raycastTarget = false;
        }
        foreach (Text t in transform.GetChild(6).GetComponentsInChildren<Text>())
        {
            t.enabled = false;
        }
        foreach (Image i in transform.GetChild(7).GetComponentsInChildren<Image>())
        {
            i.enabled = false;
            i.raycastTarget = false;
        }
        foreach (Text t in transform.GetChild(7).GetComponentsInChildren<Text>())
        {
            t.enabled = false;
        }
        foreach (Image i in transform.GetChild(8).GetComponentsInChildren<Image>())
        {
            i.enabled = false;
            i.raycastTarget = false;
        }
        foreach (Text t in transform.GetChild(8).GetComponentsInChildren<Text>())
        {
            t.enabled = false;
        }
        foreach (Image i in transform.GetChild(9).GetComponentsInChildren<Image>())
        {
            i.enabled = false;
            i.raycastTarget = false;
        }
        foreach (Text t in transform.GetChild(9).GetComponentsInChildren<Text>())
        {
            t.enabled = false;
        }

        if (GetComponent<Image>().enabled && currentSection == 0 && currentSectionKind == SectionKind.normal)
        {
            transform.GetChild(0).GetComponent<Image>().enabled = true;

            if (sword)
            {
                transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;
                transform.GetChild(0).GetChild(0).GetComponent<Image>().raycastTarget = true;
            }
            if (helmet)
            {
                transform.GetChild(0).GetChild(1).GetComponent<Image>().enabled = true;
                transform.GetChild(0).GetChild(1).GetComponent<Image>().raycastTarget = true;
            }
            if (chest)
            {
                transform.GetChild(0).GetChild(2).GetComponent<Image>().enabled = true;
                transform.GetChild(0).GetChild(2).GetComponent<Image>().raycastTarget = true;
            }
            if (gloves)
            {
                transform.GetChild(0).GetChild(3).GetComponent<Image>().enabled = true;
                transform.GetChild(0).GetChild(3).GetComponent<Image>().raycastTarget = true;
            }
            if (shield)
            {
                transform.GetChild(0).GetChild(4).GetComponent<Image>().enabled = true;
                transform.GetChild(0).GetChild(4).GetComponent<Image>().raycastTarget = true;
            }
            if (greaves)
            {
                transform.GetChild(0).GetChild(5).GetComponent<Image>().enabled = true;
                transform.GetChild(0).GetChild(5).GetComponent<Image>().raycastTarget = true;
            }
            if (boots)
            {
                transform.GetChild(0).GetChild(6).GetComponent<Image>().enabled = true;
                transform.GetChild(0).GetChild(6).GetComponent<Image>().raycastTarget = true;
            }
        }
        else if (GetComponent<Image>().enabled && currentSection == 1 && currentSectionKind == SectionKind.normal)
        {
            foreach (Image i in transform.GetChild(1).GetComponentsInChildren<Image>())
            {
                i.enabled = true;
            }

            GameObject newStat = GameObject.Instantiate(stat, transform.GetChild(1).GetChild(0));
            newStat.transform.GetChild(0).GetComponent<Image>().enabled = false;
            newStat.transform.GetChild(1).GetComponent<Text>().text = "LEVEL";
            newStat.transform.GetChild(2).GetComponent<Text>().text = experienceManager.yourLevel + "";

            newStat = GameObject.Instantiate(stat, transform.GetChild(1).GetChild(0));
            newStat.transform.GetChild(0).GetComponent<Image>().sprite = experience;
            newStat.transform.GetChild(1).GetComponent<Text>().text = "Exp.";
            newStat.transform.GetChild(2).GetComponent<Text>().text = statManager.GetComponent<ExperienceManager>().currentExperience + "/" + (int)statManager.GetComponent<ExperienceManager>().experienceRequiredToLevelUp;

            newStat = GameObject.Instantiate(stat, transform.GetChild(1).GetChild(0));
            newStat.transform.GetChild(0).GetComponent<Image>().sprite = health;
            newStat.transform.GetChild(1).GetComponent<Text>().text = "Health";
            newStat.transform.GetChild(2).GetComponent<Text>().text = statManager.GetComponent<Stats>().currentHealth + "/" + (int)statManager.GetComponent<Stats>().maxHealth;

            newStat = GameObject.Instantiate(stat, transform.GetChild(1).GetChild(0));
            newStat.transform.GetChild(0).GetComponent<Image>().sprite = damage;
            newStat.transform.GetChild(1).GetComponent<Text>().text = "Damage";
            newStat.transform.GetChild(2).GetComponent<Text>().text = (int)statManager.GetComponent<Stats>().damage + "";

            newStat = GameObject.Instantiate(stat, transform.GetChild(1).GetChild(0));
            newStat.transform.GetChild(0).GetComponent<Image>().sprite = defense;
            newStat.transform.GetChild(1).GetComponent<Text>().text = "Defense";
            newStat.transform.GetChild(2).GetComponent<Text>().text = (int)statManager.GetComponent<Stats>().defense + "";


            if (statManager.GetComponent<Stats>().weakening > 0)
            {
                newStat = GameObject.Instantiate(stat, transform.GetChild(1).GetChild(0));
                newStat.transform.GetChild(0).GetComponent<Image>().sprite = runes[0];
                newStat.transform.GetChild(1).GetComponent<Text>().text = "Weakening";
                newStat.transform.GetChild(2).GetComponent<Text>().text = statManager.GetComponent<Stats>().weakening + "";
            }
            if (statManager.GetComponent<Stats>().aura > 0)
            {
                newStat = GameObject.Instantiate(stat, transform.GetChild(1).GetChild(0));
                newStat.transform.GetChild(0).GetComponent<Image>().sprite = runes[0];
                newStat.transform.GetChild(1).GetComponent<Text>().text = "Aura";
                newStat.transform.GetChild(2).GetComponent<Text>().text = statManager.GetComponent<Stats>().aura + "";
            }
            if (statManager.GetComponent<Stats>().critical > 0)
            {
                newStat = GameObject.Instantiate(stat, transform.GetChild(1).GetChild(0));
                newStat.transform.GetChild(0).GetComponent<Image>().sprite = runes[1];
                newStat.transform.GetChild(1).GetComponent<Text>().text = "Critical";
                newStat.transform.GetChild(2).GetComponent<Text>().text = statManager.GetComponent<Stats>().critical + "";
            }
            if (statManager.GetComponent<Stats>().shield > 0)
            {
                newStat = GameObject.Instantiate(stat, transform.GetChild(1).GetChild(0));
                newStat.transform.GetChild(0).GetComponent<Image>().sprite = runes[1];
                newStat.transform.GetChild(1).GetComponent<Text>().text = "Shield";
                newStat.transform.GetChild(2).GetComponent<Text>().text = statManager.GetComponent<Stats>().shield + "";
            }
            if (statManager.GetComponent<Stats>().agility > 0)
            {
                newStat = GameObject.Instantiate(stat, transform.GetChild(1).GetChild(0));
                newStat.transform.GetChild(0).GetComponent<Image>().sprite = runes[2];
                newStat.transform.GetChild(1).GetComponent<Text>().text = "Agility";
                newStat.transform.GetChild(2).GetComponent<Text>().text = statManager.GetComponent<Stats>().agility + "";
            }
            if (statManager.GetComponent<Stats>().overload > 0)
            {
                newStat = GameObject.Instantiate(stat, transform.GetChild(1).GetChild(0));
                newStat.transform.GetChild(0).GetComponent<Image>().sprite = runes[2];
                newStat.transform.GetChild(1).GetComponent<Text>().text = "Overload";
                newStat.transform.GetChild(2).GetComponent<Text>().text = statManager.GetComponent<Stats>().overload + "";
            }
            if (statManager.GetComponent<Stats>().endurance > 0)
            {
                newStat = GameObject.Instantiate(stat, transform.GetChild(1).GetChild(0));
                newStat.transform.GetChild(0).GetComponent<Image>().sprite = runes[3];
                newStat.transform.GetChild(1).GetComponent<Text>().text = "Endurance";
                newStat.transform.GetChild(2).GetComponent<Text>().text = statManager.GetComponent<Stats>().endurance + "";
            }
            if (statManager.GetComponent<Stats>().divine > 0)
            {
                newStat = GameObject.Instantiate(stat, transform.GetChild(1).GetChild(0));
                newStat.transform.GetChild(0).GetComponent<Image>().sprite = runes[3];
                newStat.transform.GetChild(1).GetComponent<Text>().text = "Divine";
                newStat.transform.GetChild(2).GetComponent<Text>().text = statManager.GetComponent<Stats>().divine + "";
            }
            if (statManager.GetComponent<Stats>().regeneration > 0)
            {
                newStat = GameObject.Instantiate(stat, transform.GetChild(1).GetChild(0));
                newStat.transform.GetChild(0).GetComponent<Image>().sprite = runes[4];
                newStat.transform.GetChild(1).GetComponent<Text>().text = "Regeneration";
                newStat.transform.GetChild(2).GetComponent<Text>().text = statManager.GetComponent<Stats>().regeneration + "";
            }
            if (statManager.GetComponent<Stats>().drain > 0)
            {
                newStat = GameObject.Instantiate(stat, transform.GetChild(1).GetChild(0));
                newStat.transform.GetChild(0).GetComponent<Image>().sprite = runes[4];
                newStat.transform.GetChild(1).GetComponent<Text>().text = "Drain";
                newStat.transform.GetChild(2).GetComponent<Text>().text = statManager.GetComponent<Stats>().drain + "";
            }

            if (statManager.activePotions.Count > 0 || statManager.activeScrolls.Count > 0)
            {
                newStat = GameObject.Instantiate(stat, transform.GetChild(1).GetChild(0));
                newStat.transform.GetChild(0).GetComponent<Image>().enabled = false;
                newStat.transform.GetChild(1).GetComponent<Text>().text = "ACTIVE EFFECTS";
                newStat.transform.GetChild(2).GetComponent<Text>().text = "";

                foreach (Item potion in statManager.activePotions.Keys)
                {
                    newStat = GameObject.Instantiate(stat, transform.GetChild(1).GetChild(0));
                    newStat.transform.GetChild(0).GetComponent<Image>().sprite = potion.sprite;
                    newStat.transform.GetChild(1).GetComponent<Text>().text = potion.name;
                    newStat.transform.GetChild(2).GetComponent<Text>().text = "";
                }

                foreach (Item scroll in statManager.activeScrolls.Keys)
                {
                    newStat = GameObject.Instantiate(stat, transform.GetChild(1).GetChild(0));
                    newStat.transform.GetChild(0).GetComponent<Image>().sprite = scroll.sprite;
                    newStat.transform.GetChild(1).GetComponent<Text>().text = scroll.name;
                    newStat.transform.GetChild(2).GetComponent<Text>().text = (int)(300f - (Time.time - statManager.activeScrolls[scroll])) + "";
                }
            }
        }
        else if (GetComponent<Image>().enabled && currentSection == 0 && currentSectionKind == SectionKind.sorcerer)
        {
            transform.GetChild(2).GetChild(0).GetComponent<Image>().enabled = true;
            transform.GetChild(2).GetChild(1).GetComponent<Image>().enabled = true;
            transform.GetChild(2).GetChild(2).GetComponent<Image>().enabled = true;
            transform.GetChild(2).GetChild(3).GetComponent<Image>().enabled = true;
            transform.GetChild(2).GetChild(4).GetComponent<Image>().enabled = true;
        }
        else if (GetComponent<Image>().enabled && currentSection == 1 && currentSectionKind == SectionKind.sorcerer)
        {
            transform.GetChild(3).GetChild(0).GetComponent<Image>().enabled = true;
            transform.GetChild(3).GetChild(1).GetComponent<Image>().enabled = true;
            transform.GetChild(3).GetChild(2).GetComponent<Image>().enabled = true;
            transform.GetChild(3).GetChild(3).GetComponent<Image>().enabled = true;
            transform.GetChild(3).GetChild(4).GetComponent<Image>().enabled = true;
            transform.GetChild(3).GetChild(5).GetComponent<Image>().enabled = true;
            transform.GetChild(3).GetChild(6).GetComponent<Image>().enabled = true;
            transform.GetChild(3).GetChild(7).GetComponent<Image>().enabled = true;
            transform.GetChild(3).GetChild(8).GetComponent<Image>().enabled = true;
            transform.GetChild(3).GetChild(9).GetComponent<Image>().enabled = true;
            transform.GetChild(3).GetChild(10).GetComponent<Image>().enabled = true;
        }
        else if (GetComponent<Image>().enabled && currentSection == 2 && currentSectionKind == SectionKind.sorcerer)
        {
            transform.GetChild(4).GetChild(0).GetComponent<Image>().enabled = true;
            transform.GetChild(4).GetChild(1).GetComponent<Image>().enabled = true;
            transform.GetChild(4).GetChild(2).GetComponent<Image>().enabled = true;
            transform.GetChild(4).GetChild(3).GetComponent<Image>().enabled = true;
            transform.GetChild(4).GetChild(4).GetComponent<Image>().enabled = true;
            transform.GetChild(4).GetChild(5).GetComponent<Image>().enabled = true;
        }
        else if (GetComponent<Image>().enabled && currentSection == 3 && currentSectionKind == SectionKind.sorcerer)
        {
            transform.GetChild(5).GetChild(0).GetComponent<Image>().enabled = true;
            transform.GetChild(5).GetChild(1).GetComponent<Image>().enabled = true;
            transform.GetChild(5).GetChild(2).GetComponent<Image>().enabled = true;
        }
        else if (GetComponent<Image>().enabled && currentSection == 4 && currentSectionKind == SectionKind.sorcerer)
        {
            transform.GetChild(6).GetChild(0).GetComponent<Image>().enabled = true;
            transform.GetChild(6).GetChild(1).GetComponent<Image>().enabled = true;
            transform.GetChild(6).GetChild(2).GetComponent<Image>().enabled = true;
        }
        else if (GetComponent<Image>().enabled && currentSection == 5 && currentSectionKind == SectionKind.sorcerer)
        {
            transform.GetChild(7).GetChild(0).GetComponent<Image>().enabled = true;
            transform.GetChild(7).GetChild(1).GetComponent<Image>().enabled = true;
            transform.GetChild(7).GetChild(2).GetComponent<Image>().enabled = true;
            transform.GetChild(7).GetChild(3).GetComponent<Image>().enabled = true;
            transform.GetChild(7).GetChild(4).GetComponent<Image>().enabled = true;
            transform.GetChild(7).GetChild(5).GetComponent<Image>().enabled = true;
            transform.GetChild(7).GetChild(6).GetComponent<Image>().enabled = true;
        }
        else if (GetComponent<Image>().enabled && currentSection == 0 && currentSectionKind == SectionKind.well)
        {
            transform.GetChild(8).GetComponent<Image>().enabled = true;
            server.FetchTradingItems();
            StartCoroutine(PrintTradingItems());
        }
        else if (GetComponent<Image>().enabled && currentSection == 1 && currentSectionKind == SectionKind.well)
        {
            transform.GetChild(9).GetChild(0).GetComponent<Image>().enabled = true;
        }
    }

    IEnumerator PrintTradingItems()
    {
        yield return new WaitUntil(() => server.tradingItemsReady);
        if (!GetComponent<Image>().enabled || currentSection != 0)
        {
            yield break;
        }

        foreach (Item i in server.tradingItems.Keys)
        {
            GameObject newItem = GameObject.Instantiate(itemInList, transform.GetChild(8).GetChild(0));

            newItem.transform.GetChild(0).GetComponent<Image>().sprite = i.sprite;
            newItem.transform.GetChild(1).GetComponent<Text>().text = i.itemName;
            newItem.transform.GetChild(2).GetComponent<Text>().text = server.tradingItems[i].Split('.')[0];

            newItem.GetComponent<LinkToItem>().linkedItem = i;
        }
    }

    public void ChangeSection (string direction)
    {
        itemManager.heavenStone = null;
        itemManager.protectionStone = null;
        itemManager.upgradingItem = null;
        itemManager.upgradingRune = null;
        itemManager.souls = false;
        itemManager.damnedStone = null;
        itemManager.tradingItem = null;
        itemManager.price = 0;
        if (currentSectionKind == SectionKind.normal)
        {
            if (direction == "right")
            {
                currentSection = (currentSection + 1) % sections.Count;
            }
            else if (direction == "left")
            {
                currentSection = (currentSection - 1 + sections.Count) % sections.Count;
            }
            right.sprite = sections[(currentSection + 1) % (sections.Count)];
            left.sprite = sections[(currentSection - 1 + sections.Count) % (sections.Count)];
            center.sprite = sections[currentSection];
            center.color = new Color(right.color.r, right.color.g, right.color.b, 0.75f);
            right.color = new Color(right.color.r, right.color.g, right.color.b, 0.3f);
            left.color = new Color(left.color.r, left.color.g, left.color.b, 0.3f);
            UpdateItems(true);
        }
        else if (currentSectionKind == SectionKind.itemDetail)
        {
            if (direction == "itemDetail")
            {
                if (server.GetComponent<GameManager>().place != "well")
                {
                    right.sprite = sections[currentSection];
                    left.sprite = sections[currentSection];
                    center.sprite = itemDetail;
                }
                else
                {
                    center.sprite = itemDetail;
                    right.sprite = wellSections[currentSection];
                    left.sprite = wellSections[currentSection];
                }
                center.color = new Color(right.color.r, right.color.g, right.color.b, 0.75f);
                right.color = new Color(right.color.r, right.color.g, right.color.b, 0.3f);
                left.color = new Color(left.color.r, left.color.g, left.color.b, 0.3f);
            }
            else
            {
                if (server.GetComponent<GameManager>().place != "well")
                {
                    currentSectionKind = SectionKind.normal;

                    center.sprite = sections[currentSection];
                    right.sprite = sections[(currentSection + 1) % (sections.Count)];
                    left.sprite = sections[(currentSection - 1 + sections.Count) % (sections.Count)];
                }
                else
                {
                    currentSectionKind = SectionKind.well;

                    center.sprite = wellSections[currentSection];
                    right.sprite = wellSections[(currentSection + 1) % (wellSections.Count)];
                    left.sprite = wellSections[(currentSection - 1 + wellSections.Count) % (wellSections.Count)];
                }
                foreach(Image i in itemTab.GetComponentsInChildren<Image>())
                {
                    i.enabled = false;
                }
                foreach(Text t in itemTab.GetComponentsInChildren<Text>())
                {
                    t.enabled = false;
                }

                center.color = new Color(right.color.r, right.color.g, right.color.b, 0.75f);
                right.color = new Color(right.color.r, right.color.g, right.color.b, 0.3f);
                left.color = new Color(left.color.r, left.color.g, left.color.b, 0.3f);
                UpdateItems(true);
            }
        }
        else if (currentSectionKind == SectionKind.sorcerer)
        {
            if (direction == "right")
            {
                currentSection = (currentSection + 1) % sorcererSections.Count;
            }
            else if (direction == "left")
            {
                currentSection = (currentSection - 1 + sorcererSections.Count) % sorcererSections.Count;
            }
            right.sprite = sorcererSections[(currentSection + 1) % (sorcererSections.Count)];
            left.sprite = sorcererSections[(currentSection - 1 + sorcererSections.Count) % (sorcererSections.Count)];
            center.sprite = sorcererSections[currentSection];
            center.color = new Color(right.color.r, right.color.g, right.color.b, 0.75f);
            right.color = new Color(right.color.r, right.color.g, right.color.b, 0.3f);
            left.color = new Color(left.color.r, left.color.g, left.color.b, 0.3f);
            UpdateItems(true);
            if (itemManager.currentSectionKind == ItemManager.SectionKind.itemDetail)
            {
                itemManager.ChangeSection ("exitItemDetail");
            }
        }
        else if (currentSectionKind == SectionKind.well)
        {
            if (direction == "right")
            {
                currentSection = (currentSection + 1) % wellSections.Count;
            }
            else if (direction == "left")
            {
                currentSection = (currentSection - 1 + wellSections.Count) % wellSections.Count;
            }
            right.sprite = wellSections[(currentSection + 1) % (wellSections.Count)];
            left.sprite = wellSections[(currentSection - 1 + wellSections.Count) % (wellSections.Count)];
            center.sprite = wellSections[currentSection];
            center.color = new Color(right.color.r, right.color.g, right.color.b, 0.75f);
            right.color = new Color(right.color.r, right.color.g, right.color.b, 0.3f);
            left.color = new Color(left.color.r, left.color.g, left.color.b, 0.3f);
            UpdateItems(true);
            if (itemManager.currentSectionKind == ItemManager.SectionKind.itemDetail)
            {
                itemManager.ChangeSection ("exitItemDetail");
            }
        }
    }

    public void ShowItemDetail (string part, Item linkedItem = null)
    {
        Item itemAux = null;
        if (linkedItem)
        {
            itemAux = linkedItem;
            server.buyingItem = linkedItem;

            for (int i = 0; i < transform.GetChild(8).GetChild(0).childCount; i++)
            {
                Destroy(transform.GetChild(8).GetChild(0).GetChild(i).gameObject);
            }
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().enabled = !transform.GetChild(0).GetComponent<Image>().enabled; 
            transform.GetChild(0).GetComponent<Image>().raycastTarget = !transform.GetChild(0).GetComponent<Image>().raycastTarget;
            
            if (!transform.GetChild(0).GetComponent<Image>().enabled)
            {
                foreach (Image i in transform.GetChild(0).GetComponentsInChildren<Image>())
                {
                    i.enabled = false;
                    i.raycastTarget = false;
                }
            }
            
            foreach (Item i in itemManager.items.Keys)
            {
                if (i.type == part && itemManager.items[i] == 0)
                {
                    itemAux = i;
                    break;
                }
            }
        }

        currentSectionKind = SectionKind.itemDetail;
        ChangeSection ("itemDetail");

        foreach(Image i in itemTab.GetComponentsInChildren<Image>())
        {
            i.enabled = true;
        }
        foreach(Text t in itemTab.GetComponentsInChildren<Text>())
        {
            t.enabled = true;
        }

        itemTab.transform.GetChild(1).GetComponent<Image>().sprite = itemAux.sprite;
        itemTab.transform.GetChild(2).GetComponent<Text>().text = itemAux.itemName;

        if (itemAux.kind != 0)
        {
            itemTab.transform.GetChild(3).GetComponent<Text>().text = server.tradingItems[server.buyingItem].Split(' ')[0];
        }
        else
        {
            itemTab.transform.GetChild(3).GetComponent<Text>().text = "";
        }

        itemTab.transform.GetChild(4).GetComponent<Text>().text = itemAux.description;
        
        if (itemAux.type == "Rune")
        {
            itemTab.transform.GetChild(4).GetComponent<Text>().text += " (" + itemAux.effect + ").";
        }
        else if (itemAux.kind == 0)
        {
            if (itemAux.type == "Sword")
            {
                itemTab.transform.GetChild(4).GetComponent<Text>().text = "Damage " + (int)itemAux.effect + ".";
            }
            else 
            {
                itemTab.transform.GetChild(4).GetComponent<Text>().text = "Defense " + (int)itemAux.effect + ".";
            }
        }

        if (itemAux.kind == 0 && itemAux.rune.y != 0)
        {
            itemTab.transform.GetChild(5).GetComponent<Image>().sprite = runes[itemAux.rune.x / 2];
            
            if (itemAux.rune.x == 0)
            {
                itemTab.transform.GetChild(6).GetComponent<Text>().text = "Weakening " + itemAux.rune.y + ".";
            }
            else if (itemAux.rune.x == 1)
            {
                itemTab.transform.GetChild(6).GetComponent<Text>().text = "Aura " + itemAux.rune.y + ".";
            }
            else if (itemAux.rune.x == 2)
            {
                itemTab.transform.GetChild(6).GetComponent<Text>().text = "Critical " + itemAux.rune.y + ".";
            }
            else if (itemAux.rune.x == 3)
            {
                itemTab.transform.GetChild(6).GetComponent<Text>().text = "Shield " + itemAux.rune.y + ".";
            }
            else if (itemAux.rune.x == 4)
            {
                itemTab.transform.GetChild(6).GetComponent<Text>().text = "Agility " + itemAux.rune.y + ".";
            }
            else if (itemAux.rune.x == 5)
            {
                itemTab.transform.GetChild(6).GetComponent<Text>().text = "Overload " + itemAux.rune.y + ".";
            }
            else if (itemAux.rune.x == 6)
            {
                itemTab.transform.GetChild(6).GetComponent<Text>().text = "Endurance " + itemAux.rune.y + ".";
            }
            else if (itemAux.rune.x == 7)
            {
                itemTab.transform.GetChild(6).GetComponent<Text>().text = "Divine " + itemAux.rune.y + ".";
            }
            else if (itemAux.rune.x == 8)
            {
                itemTab.transform.GetChild(6).GetComponent<Text>().text = "Regeneration " + itemAux.rune.y + ".";
            }
            else if (itemAux.rune.x == 9)
            {
                itemTab.transform.GetChild(6).GetComponent<Text>().text = "Drain " + itemAux.rune.y + ".";
            }
        }
        else
        {
            itemTab.transform.GetChild(5).GetComponent<Image>().enabled = false;
            itemTab.transform.GetChild(6).GetComponent<Text>().enabled = false;
        }

        if (linkedItem)
        {
            if (itemManager.items.ContainsKey(drops.allItems.Find(x => x.itemName == "Trading token")))
            {
                if (itemManager.items[drops.allItems.Find(x => x.itemName == "Trading token")] >= int.Parse(server.tradingItems[server.buyingItem].Split('(')[1].Split(')')[0]))
                {
                    itemTab.transform.GetChild(8).GetComponent<Text>().text = "Trade";
                    itemTab.transform.GetChild(7).GetComponent<Image>().raycastTarget = true;
                }
                else
                {
                    itemTab.transform.GetChild(7).GetComponent<Image>().enabled = false;
                    itemTab.transform.GetChild(8).GetComponent<Text>().enabled = false;
                }
            }
            else
            {
                itemTab.transform.GetChild(7).GetComponent<Image>().enabled = false;
                itemTab.transform.GetChild(8).GetComponent<Text>().enabled = false;
            }
        }
        else
        {
            itemTab.transform.GetChild(7).GetComponent<Image>().raycastTarget = true;
            itemTab.transform.GetChild(8).GetComponent<Text>().text = "Unequip";
        }
    }

    public void UnequipItem (string item)
    {
        Item itemAux = null;
        foreach (Item i in itemManager.items.Keys)
        {
            if (i.itemName == item && itemManager.items[i] == 0)
            {
                itemAux = i;
                break;
            }
        }

        if (itemAux.kind == 0)
        {
            statManager.UnequipGear(itemAux);
            server.TransferItemsToFirebase();
        }

        ChangeSection ("exitItemDetail");
    }

    public IEnumerator CraftGear ()
    {
        sounds.PlaySound(5);
        processing = true;
        int essence = -1;
        if (transform.GetChild(2).GetChild(5).GetComponent<Image>().enabled)
        {
            essence = 0;
        }
        else if (transform.GetChild(2).GetChild(6).GetComponent<Image>().enabled)
        {
            essence = 1;
        }
        else if (transform.GetChild(2).GetChild(7).GetComponent<Image>().enabled)
        {
            essence = 2;
        }

        if (essence == -1)
        {
            yield break;
        }

        string name = "";
        if (essence == 0)
        {
            name = "Demonic essence";
        }
        else if (essence == 1)
        {
            name = "Enchanted essence";
        }
        else if (essence == 2)
        {
            name = "Dark essence";
        }
        Item essenceAux = null;
        foreach (Item i in itemManager.items.Keys)
        {
            if (i.itemName == name)
            {
                essenceAux = i;
                break;
            }
        }

        int essenceNumber = itemManager.items[essenceAux];
        itemManager.items.Remove(essenceAux);
        server.TransferItemsToFirebase();

        transform.GetChild(2).GetChild(15).GetComponent<Image>().enabled = false;
        transform.GetChild(2).GetChild(15).GetComponent<Image>().raycastTarget = false;
        transform.GetChild(2).GetChild(16).GetComponent<Text>().enabled = false;

        transform.GetChild(2).GetChild(14).GetComponent<Image>().enabled = true;
        
        if (itemManager.currentSectionKind == ItemManager.SectionKind.itemDetail)
        {
            itemManager.ChangeSection ("exitItemDetail");
        }
        
        int random = Random.Range(0, 2);
        sounds.PlaySound(6);
        while (transform.GetChild(2).GetChild(1).GetComponent<Image>().fillAmount < 1)
        {
            transform.GetChild(2).GetChild(1).GetComponent<Image>().fillAmount += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        
        if (random == 0)
        {
            sounds.PlaySound(6);
            transform.GetChild(2).GetChild(8 + essence * 2).GetComponent<Image>().color = new Color (1, 1, 1, 1f);
            while (transform.GetChild(2).GetChild(2).GetComponent<Image>().fillAmount < 1)
            {
                transform.GetChild(2).GetChild(2).GetComponent<Image>().fillAmount += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
            sounds.PlaySound(8);
        }
        else
        {
            sounds.PlaySound(7);
            sounds.PlaySound(6);
            transform.GetChild(2).GetChild(1).GetComponent<Image>().fillAmount = 0f;
            while (transform.GetChild(2).GetChild(3).GetComponent<Image>().fillAmount < 1)
            {
                transform.GetChild(2).GetChild(3).GetComponent<Image>().fillAmount += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
            sounds.PlaySound(6);
            transform.GetChild(2).GetChild(9 + essence * 2).GetComponent<Image>().color = new Color (1, 1, 1, 1f);
            while (transform.GetChild(2).GetChild(4).GetComponent<Image>().fillAmount < 1)
            {
                transform.GetChild(2).GetChild(4).GetComponent<Image>().fillAmount += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
            sounds.PlaySound(8);
        }
        
        string gear;
        if (essence == 0)
        {
            if (random == 0)
            {
                if (experienceManager.yourLevel < 20)
                {
                    gear = "Traveler's helmet";
                }
                else if (experienceManager.yourLevel < 40)
                {
                    gear = "Explorer's helmet";
                }
                else if (experienceManager.yourLevel < 60)
                {
                    gear = "Soldier's helmet";
                }
                else if (experienceManager.yourLevel < 80)
                {
                    gear = "Master's helmet";
                }
                else
                {
                    gear = "Assassin's helmet";
                }
            }
            else
            {
                if (experienceManager.yourLevel < 20)
                {
                    gear = "Traveler's chest";
                }
                else if (experienceManager.yourLevel < 40)
                {
                    gear = "Explorer's chest";
                }
                else if (experienceManager.yourLevel < 60)
                {
                    gear = "Soldier's chest";
                }
                else if (experienceManager.yourLevel < 80)
                {
                    gear = "Master's chest";
                }
                else
                {
                    gear = "Assassin's chest";
                }
            }
        }
        else if (essence == 1)
        {
            if (random == 0)
            {
                if (experienceManager.yourLevel < 20)
                {
                    gear = "Traveler's gloves";
                }
                else if (experienceManager.yourLevel < 40)
                {
                    gear = "Explorer's gloves";
                }
                else if (experienceManager.yourLevel < 60)
                {
                    gear = "Soldier's gloves";
                }
                else if (experienceManager.yourLevel < 80)
                {
                    gear = "Master's gloves";
                }
                else
                {
                    gear = "Assassin's gloves";
                }
            }
            else
            {
                if (experienceManager.yourLevel < 20)
                {
                    gear = "Traveler's shield";
                }
                else if (experienceManager.yourLevel < 40)
                {
                    gear = "Explorer's shield";
                }
                else if (experienceManager.yourLevel < 60)
                {
                    gear = "Soldier's shield";
                }
                else if (experienceManager.yourLevel < 80)
                {
                    gear = "Master's shield";
                }
                else
                {
                    gear = "Assassin's shield";
                }
            }
        }
        else
        {
            if (random == 0)
            {
                if (experienceManager.yourLevel < 20)
                {
                    gear = "Traveler's greaves";
                }
                else if (experienceManager.yourLevel < 40)
                {
                    gear = "Explorer's greaves";
                }
                else if (experienceManager.yourLevel < 60)
                {
                    gear = "Soldier's greaves";
                }
                else if (experienceManager.yourLevel < 80)
                {
                    gear = "Master's greaves";
                }
                else
                {
                    gear = "Assassin's greaves";
                }
            }
            else
            {
                if (experienceManager.yourLevel < 20)
                {
                    gear = "Traveler's boots";
                }
                else if (experienceManager.yourLevel < 40)
                {
                    gear = "Explorer's boots";
                }
                else if (experienceManager.yourLevel < 60)
                {
                    gear = "Soldier's boots";
                }
                else if (experienceManager.yourLevel < 80)
                {
                    gear = "Master's boots";
                }
                else
                {
                    gear = "Assassin's boots";
                }
            }
        }

        Item instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == gear), modifiableItems);

        // int runePower = 0;
        int gearPower = 0;
        for (int i = 0; i < essenceNumber; i++)
        {
            // if (Random.Range(0,10) == 0)
            // {
            //     runePower++;
            // }

            if (Random.Range(0,50) == 0)
            {
                gearPower++;
            }
        }

        // runePower = Mathf.Clamp(runePower, 0, 50);
        gearPower = Mathf.Clamp(gearPower, 0, 10);

        // instance.rune = new Vector2Int (Random.Range(0,10), runePower);
        if (gearPower != 0)
        {
            instance.itemName += " +" + gearPower;
        }
        if(instance.type == "Sword")
        {
            instance.effect += 20 * gearPower;
        }
        else
        {
            instance.effect += 4 * gearPower;
        }

        if (itemManager.currentSectionKind == ItemManager.SectionKind.itemDetail)
        {
            itemManager.ChangeSection ("exitItemDetail");
        }

        itemManager.AddItem(instance, 1);

        server.TransferItemsToFirebase();

        transform.GetChild(2).GetChild(8 + essence * 2).GetComponent<Image>().enabled = false;
        transform.GetChild(2).GetChild(9 + essence * 2).GetComponent<Image>().enabled = false;
        transform.GetChild(2).GetChild(14).GetComponent<Image>().enabled = false;

        transform.GetChild(2).GetChild(1).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(2).GetChild(2).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(2).GetChild(3).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(2).GetChild(4).GetComponent<Image>().fillAmount = 0;

        transform.GetChild(2).GetChild(5).GetComponent<Image>().enabled = false;
        transform.GetChild(2).GetChild(6).GetComponent<Image>().enabled = false;
        transform.GetChild(2).GetChild(7).GetComponent<Image>().enabled = false;

        rightOpener.transform.GetChild(0).GetComponent<Image>().sprite = rewardAvailable;
        itemManager.currentSectionKind = ItemManager.SectionKind.rewards;
        itemManager.UpdateItems(true);
        processing = false;
    }

    public IEnumerator CraftRune ()
    {
        sounds.PlaySound(5);
        processing = true;

        Item soulsAux = null;
        foreach (Item i in itemManager.items.Keys)
        {
            if (i.itemName == "Soul")
            {
                soulsAux = i;
                break;
            }
        }
        
        int soulNumber = itemManager.items[soulsAux];

        itemManager.items.Remove(soulsAux);
        itemManager.souls = false;

        server.TransferItemsToFirebase();

        transform.GetChild(3).GetChild(18).GetComponent<Image>().enabled = false;
        transform.GetChild(3).GetChild(18).GetComponent<Image>().raycastTarget = false;
        transform.GetChild(3).GetChild(19).GetComponent<Text>().enabled = false;

        sounds.PlaySound(6);
        int random = Random.Range(0, 10);
        while (transform.GetChild(3).GetChild(1).GetComponent<Image>().fillAmount < 1)
        {
            transform.GetChild(3).GetChild(1).GetComponent<Image>().fillAmount += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        
        if (random > 1)
        {
            sounds.PlaySound(6);
            transform.GetChild(3).GetChild(1).GetComponent<Image>().fillAmount = 0f;
            while (transform.GetChild(3).GetChild(3).GetComponent<Image>().fillAmount < 1)
            {
                transform.GetChild(3).GetChild(3).GetComponent<Image>().fillAmount += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }
        if (random > 3)
        {
            sounds.PlaySound(7);
            sounds.PlaySound(6);
            transform.GetChild(3).GetChild(3).GetComponent<Image>().fillAmount = 0f;
            while (transform.GetChild(3).GetChild(5).GetComponent<Image>().fillAmount < 1)
            {
                transform.GetChild(3).GetChild(5).GetComponent<Image>().fillAmount += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }
        if (random > 5)
        {
            sounds.PlaySound(7);
            sounds.PlaySound(6);
            transform.GetChild(3).GetChild(5).GetComponent<Image>().fillAmount = 0f;
            while (transform.GetChild(3).GetChild(7).GetComponent<Image>().fillAmount < 1)
            {
                transform.GetChild(3).GetChild(7).GetComponent<Image>().fillAmount += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }
        if (random > 7)
        {
            sounds.PlaySound(7);
            sounds.PlaySound(6);
            transform.GetChild(3).GetChild(7).GetComponent<Image>().fillAmount = 0f;
            while (transform.GetChild(3).GetChild(9).GetComponent<Image>().fillAmount < 1)
            {
                transform.GetChild(3).GetChild(9).GetComponent<Image>().fillAmount += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }

        sounds.PlaySound(6);
        if (random == 0 || random == 1)
        {
            transform.GetChild(3).GetChild(12).GetComponent<Image>().color = new Color (1, 1, 1, 1f);
            while (transform.GetChild(3).GetChild(2).GetComponent<Image>().fillAmount < 1)
            {
                transform.GetChild(3).GetChild(2).GetComponent<Image>().fillAmount += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }
        else if (random == 2 || random == 3)
        {
            transform.GetChild(3).GetChild(13).GetComponent<Image>().color = new Color (1, 1, 1, 1f);
            while (transform.GetChild(3).GetChild(4).GetComponent<Image>().fillAmount < 1)
            {
                transform.GetChild(3).GetChild(4).GetComponent<Image>().fillAmount += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }
        else if (random == 4 || random == 5)
        {
            transform.GetChild(3).GetChild(14).GetComponent<Image>().color = new Color (1, 1, 1, 1f);
            while (transform.GetChild(3).GetChild(6).GetComponent<Image>().fillAmount < 1)
            {
                transform.GetChild(3).GetChild(6).GetComponent<Image>().fillAmount += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }
        else if (random == 6 || random == 7)
        {
            transform.GetChild(3).GetChild(15).GetComponent<Image>().color = new Color (1, 1, 1, 1f);
            while (transform.GetChild(3).GetChild(8).GetComponent<Image>().fillAmount < 1)
            {
                transform.GetChild(3).GetChild(8).GetComponent<Image>().fillAmount += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }
        else if (random == 8 || random == 9)
        {
            transform.GetChild(3).GetChild(16).GetComponent<Image>().color = new Color (1, 1, 1, 1f);
            while (transform.GetChild(3).GetChild(10).GetComponent<Image>().fillAmount < 1)
            {
                transform.GetChild(3).GetChild(10).GetComponent<Image>().fillAmount += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }
        sounds.PlaySound(8);

        int runePower = 0;
        for (int i = 0; i < soulNumber; i++)
        {
            if (Random.Range(0,50) == 0)
            {
                runePower++;
            }
        }

        Item instance = null;
        if (random == 0)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Weakening rune"), modifiableItems);
            instance.level = 0;
        }
        else if (random == 1)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Aura rune"), modifiableItems);
            instance.level = 1;
        }
        else if (random == 2)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Critical rune"), modifiableItems);
            instance.level = 2;
        }
        else if (random == 3)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Shield rune"), modifiableItems);
            instance.level = 3;
        }
        else if (random == 4)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Agility rune"), modifiableItems);
            instance.level = 4;
        }
        else if (random == 5)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Overload rune"), modifiableItems);
            instance.level = 5;
        }
        else if (random == 6)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Endurance rune"), modifiableItems);
            instance.level = 6;
        }
        else if (random == 7)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Divine rune"), modifiableItems);
            instance.level = 7;
        }
        else if (random == 8)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Regeneration rune"), modifiableItems);
            instance.level = 8;
        }
        else if (random == 9)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Drain rune"), modifiableItems);
            instance.level = 9;
        }

        runePower = Mathf.Clamp(runePower, 1, 50);

        instance.effect = runePower;

        if (itemManager.currentSectionKind == ItemManager.SectionKind.itemDetail)
        {
            itemManager.ChangeSection ("exitItemDetail");
        }
        itemManager.AddItem(instance, 1);

        server.TransferItemsToFirebase();

        transform.GetChild(3).GetChild(17).GetComponent<Image>().enabled = false;
        transform.GetChild(3).GetChild(16).GetComponent<Image>().enabled = false;
        transform.GetChild(3).GetChild(15).GetComponent<Image>().enabled = false;
        transform.GetChild(3).GetChild(14).GetComponent<Image>().enabled = false;
        transform.GetChild(3).GetChild(13).GetComponent<Image>().enabled = false;
        transform.GetChild(3).GetChild(12).GetComponent<Image>().enabled = false;

        transform.GetChild(3).GetChild(1).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(3).GetChild(2).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(3).GetChild(3).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(3).GetChild(4).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(3).GetChild(5).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(3).GetChild(6).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(3).GetChild(7).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(3).GetChild(8).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(3).GetChild(9).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(3).GetChild(10).GetComponent<Image>().fillAmount = 0;

        transform.GetChild(3).GetChild(11).GetComponent<Image>().enabled = false;

        rightOpener.transform.GetChild(0).GetComponent<Image>().sprite = rewardAvailable;
        itemManager.currentSectionKind = ItemManager.SectionKind.rewards;
        itemManager.UpdateItems(true);
        processing = false;
    }

    public IEnumerator UpgradeGear ()
    {
        sounds.PlaySound(5);
        processing = true;

        if (itemManager.items[itemManager.heavenStone] > 1)
        {
            itemManager.AddItem(itemManager.heavenStone, -1);
        }
        else 
        {
            itemManager.items.Remove(itemManager.heavenStone);
        }

        bool hasProtectionStone = false;
        if (itemManager.protectionStone)
        {
            hasProtectionStone = true;
            if (itemManager.items[itemManager.protectionStone] > 1)
            {
                itemManager.AddItem(itemManager.protectionStone, -1);
            }
            else 
            {
                itemManager.items.Remove(itemManager.protectionStone);
            }
        }
        else 
        {
            hasProtectionStone = false;
            itemManager.items.Remove(itemManager.upgradingItem);
        }

        itemManager.heavenStone = null;
        itemManager.protectionStone = null;

        server.TransferItemsToFirebase();

        transform.GetChild(4).GetChild(16).GetComponent<Image>().enabled = false;
        transform.GetChild(4).GetChild(16).GetComponent<Image>().raycastTarget = false;
        transform.GetChild(4).GetChild(17).GetComponent<Text>().enabled = false;

        if (itemManager.currentSectionKind == ItemManager.SectionKind.itemDetail)
        {
            itemManager.ChangeSection ("exitItemDetail");
        }
        
        sounds.PlaySound(6);
        while (transform.GetChild(4).GetChild(1).GetComponent<Image>().fillAmount < 1)
        {
            transform.GetChild(4).GetChild(1).GetComponent<Image>().fillAmount += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        if (hasProtectionStone)
        {
            sounds.PlaySound(6);
            while (transform.GetChild(4).GetChild(3).GetComponent<Image>().fillAmount < 1)
            {
                transform.GetChild(4).GetChild(3).GetComponent<Image>().fillAmount += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }

            sounds.PlaySound(6);
            while (transform.GetChild(4).GetChild(5).GetComponent<Image>().fillAmount < 1)
            {
                transform.GetChild(4).GetChild(5).GetComponent<Image>().fillAmount += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
            transform.GetChild(4).GetChild(3).GetComponent<Image>().fillAmount = 0f;
            transform.GetChild(4).GetChild(5).GetComponent<Image>().fillAmount = 0f;
        }
        else
        {
            sounds.PlaySound(6);
            while (transform.GetChild(4).GetChild(2).GetComponent<Image>().fillAmount < 1)
            {
                transform.GetChild(4).GetChild(2).GetComponent<Image>().fillAmount += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
            transform.GetChild(4).GetChild(2).GetComponent<Image>().fillAmount = 0f;
        }

        int random = Random.Range(0, 4);
        
        if (random > 0)
        {
            sounds.PlaySound(8);
            int upgrade = 0;
            if (itemManager.upgradingItem.itemName.Contains("+"))
            {
                if (int.Parse(itemManager.upgradingItem.itemName.Split('+')[1]) != 10)
                {
                    if (itemManager.upgradingItem.type == "Sword")
                    {
                        itemManager.upgradingItem.effect += 20;
                    }
                    else
                    {
                        itemManager.upgradingItem.effect += 4;
                    }
                    upgrade = int.Parse(itemManager.upgradingItem.itemName.Split('+')[1]) + 1;
                    itemManager.upgradingItem.itemName = itemManager.upgradingItem.itemName.Split('+')[0] + "+" + upgrade;
                }
            }
            else 
            {
                if (itemManager.upgradingItem.type == "Sword")
                {
                    itemManager.upgradingItem.effect += 20;
                }
                else
                {
                    itemManager.upgradingItem.effect += 4;
                }
                itemManager.upgradingItem.itemName += " +" + 1;
            }

            if (itemManager.currentSectionKind == ItemManager.SectionKind.itemDetail)
            {
                itemManager.ChangeSection ("exitItemDetail");
            }
            itemManager.AddItem(GameObject.Instantiate(itemManager.upgradingItem, modifiableItems), 1);
            itemManager.items.Remove(itemManager.upgradingItem);
            Destroy(itemManager.upgradingItem.gameObject);

            rightOpener.transform.GetChild(0).GetComponent<Image>().sprite = rewardAvailable;
            itemManager.currentSectionKind = ItemManager.SectionKind.rewards;
        }
        else if (hasProtectionStone)
        {
            sounds.PlaySound(7);
            sounds.PlaySound(6);
            while (transform.GetChild(4).GetChild(4).GetComponent<Image>().fillAmount < 1)
            {
                transform.GetChild(4).GetChild(4).GetComponent<Image>().fillAmount += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        }
        else 
        {
            sounds.PlaySound(7);
            Destroy(itemManager.upgradingItem.gameObject);
        }

        itemManager.upgradingItem = null;

        server.TransferItemsToFirebase();

        transform.GetChild(4).GetChild(6).GetComponent<Image>().enabled = false;
        transform.GetChild(4).GetChild(7).GetComponent<Image>().enabled = false;
        transform.GetChild(4).GetChild(8).GetComponent<Image>().enabled = false;
        transform.GetChild(4).GetChild(9).GetComponent<Image>().enabled = false;
        transform.GetChild(4).GetChild(10).GetComponent<Image>().enabled = false;
        transform.GetChild(4).GetChild(11).GetComponent<Image>().enabled = false;
        transform.GetChild(4).GetChild(12).GetComponent<Image>().enabled = false;
        transform.GetChild(4).GetChild(13).GetComponent<Image>().enabled = false;
        transform.GetChild(4).GetChild(14).GetComponent<Image>().enabled = false;
        transform.GetChild(4).GetChild(15).GetComponent<Image>().enabled = false;

        transform.GetChild(4).GetChild(1).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(4).GetChild(2).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(4).GetChild(3).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(4).GetChild(4).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(4).GetChild(5).GetComponent<Image>().fillAmount = 0;

        itemManager.UpdateItems(true);
        processing = false;
    }

    public IEnumerator UpgradeRune ()
    {
        sounds.PlaySound(5);
        processing = true;

        sounds.PlaySound(8);
        Item soulsAux = null;
        foreach (Item i in itemManager.items.Keys)
        {
            if (i.itemName == "Soul")
            {
                soulsAux = i;
                break;
            }
        }
        int soulNumber = itemManager.items[soulsAux];
        itemManager.items.Remove(soulsAux);

        itemManager.items.Remove(itemManager.upgradingRune);
        itemManager.souls = false;

        server.TransferItemsToFirebase();

        transform.GetChild(5).GetChild(10).GetComponent<Image>().enabled = false;
        transform.GetChild(5).GetChild(10).GetComponent<Image>().raycastTarget = false;
        transform.GetChild(5).GetChild(11).GetComponent<Text>().enabled = false;

        if (itemManager.currentSectionKind == ItemManager.SectionKind.itemDetail)
        {
            itemManager.ChangeSection ("exitItemDetail");
        }
        
        sounds.PlaySound(6);
        while (transform.GetChild(5).GetChild(1).GetComponent<Image>().fillAmount < 1)
        {
            transform.GetChild(5).GetChild(1).GetComponent<Image>().fillAmount += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        sounds.PlaySound(6);
        while (transform.GetChild(5).GetChild(2).GetComponent<Image>().fillAmount < 1)
        {
            transform.GetChild(5).GetChild(2).GetComponent<Image>().fillAmount += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        int runePower = 0;
        for (int i = 0; i < soulNumber; i++)
        {
            if (Random.Range(0,75) == 0)
            {
                runePower++;
            }
        }

        itemManager.upgradingRune.effect = Mathf.Clamp(itemManager.upgradingRune.effect + runePower, 1, 50);
        if (itemManager.currentSectionKind == ItemManager.SectionKind.itemDetail)
        {
            itemManager.ChangeSection ("exitItemDetail");
        }
        itemManager.AddItem(GameObject.Instantiate(itemManager.upgradingRune, modifiableItems), 1);
        
        Destroy(itemManager.upgradingRune.gameObject);

        itemManager.upgradingRune = null;

        server.TransferItemsToFirebase();

        transform.GetChild(5).GetChild(3).GetComponent<Image>().enabled = false;
        transform.GetChild(5).GetChild(4).GetComponent<Image>().enabled = false;
        transform.GetChild(5).GetChild(5).GetComponent<Image>().enabled = false;
        transform.GetChild(5).GetChild(6).GetComponent<Image>().enabled = false;
        transform.GetChild(5).GetChild(7).GetComponent<Image>().enabled = false;
        transform.GetChild(5).GetChild(8).GetComponent<Image>().enabled = false;
        transform.GetChild(5).GetChild(9).GetComponent<Image>().enabled = false;

        transform.GetChild(5).GetChild(1).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(5).GetChild(2).GetComponent<Image>().fillAmount = 0;

        rightOpener.transform.GetChild(0).GetComponent<Image>().sprite = rewardAvailable;
        itemManager.currentSectionKind = ItemManager.SectionKind.rewards;
        itemManager.UpdateItems(true);
        processing = false;
    }

    public IEnumerator AddRune ()
    {
        sounds.PlaySound(5);
        processing = true;

        itemManager.items.Remove(itemManager.upgradingRune);
        itemManager.items.Remove(itemManager.upgradingItem);
        
        server.TransferItemsToFirebase();

        transform.GetChild(6).GetChild(16).GetComponent<Image>().enabled = false;
        transform.GetChild(6).GetChild(16).GetComponent<Image>().raycastTarget = false;
        transform.GetChild(6).GetChild(17).GetComponent<Text>().enabled = false;

        if (itemManager.currentSectionKind == ItemManager.SectionKind.itemDetail)
        {
            itemManager.ChangeSection ("exitItemDetail");
        }
        
        sounds.PlaySound(6);
        while (transform.GetChild(6).GetChild(1).GetComponent<Image>().fillAmount < 1)
        {
            transform.GetChild(6).GetChild(1).GetComponent<Image>().fillAmount += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        sounds.PlaySound(6);
        while (transform.GetChild(6).GetChild(2).GetComponent<Image>().fillAmount < 1)
        {
            transform.GetChild(6).GetChild(2).GetComponent<Image>().fillAmount += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        if (itemManager.upgradingRune.itemName == "Weakening rune")
        {
            if (itemManager.upgradingItem.rune.x == 0)
            {
                itemManager.upgradingItem.rune = new Vector2Int (0, Mathf.Clamp(itemManager.upgradingItem.rune.y + (int)itemManager.upgradingRune.effect, 0, 50));
            }
            else
            {
                itemManager.upgradingItem.rune = new Vector2Int (0, (int)itemManager.upgradingRune.effect);
            }
        }
        else if (itemManager.upgradingRune.itemName == "Aura rune")
        {
            if (itemManager.upgradingItem.rune.x == 1)
            {
                itemManager.upgradingItem.rune = new Vector2Int (1, Mathf.Clamp(itemManager.upgradingItem.rune.y + (int)itemManager.upgradingRune.effect, 0, 50));
            }
            else
            {
                itemManager.upgradingItem.rune = new Vector2Int (1, (int)itemManager.upgradingRune.effect);
            }
        }
        else if (itemManager.upgradingRune.itemName == "Critical rune")
        {
            if (itemManager.upgradingItem.rune.x == 2)
            {
                itemManager.upgradingItem.rune = new Vector2Int (2, Mathf.Clamp(itemManager.upgradingItem.rune.y + (int)itemManager.upgradingRune.effect, 0, 50));
            }
            else
            {
                itemManager.upgradingItem.rune = new Vector2Int (2, (int)itemManager.upgradingRune.effect);
            }
        }
        else if (itemManager.upgradingRune.itemName == "Shield rune")
        {
            if (itemManager.upgradingItem.rune.x == 3)
            {
                itemManager.upgradingItem.rune = new Vector2Int (3, Mathf.Clamp(itemManager.upgradingItem.rune.y + (int)itemManager.upgradingRune.effect, 0, 50));
            }
            else
            {
                itemManager.upgradingItem.rune = new Vector2Int (3, (int)itemManager.upgradingRune.effect);
            }
        }
        else if (itemManager.upgradingRune.itemName == "Agility rune")
        {
            if (itemManager.upgradingItem.rune.x == 4)
            {
                itemManager.upgradingItem.rune = new Vector2Int (4, Mathf.Clamp(itemManager.upgradingItem.rune.y + (int)itemManager.upgradingRune.effect, 0, 50));
            }
            else
            {
                itemManager.upgradingItem.rune = new Vector2Int (4, (int)itemManager.upgradingRune.effect);
            }
        }
        else if (itemManager.upgradingRune.itemName == "Overload rune")
        {
            if (itemManager.upgradingItem.rune.x == 5)
            {
                itemManager.upgradingItem.rune = new Vector2Int (5, Mathf.Clamp(itemManager.upgradingItem.rune.y + (int)itemManager.upgradingRune.effect, 0, 50));
            }
            else
            {
                itemManager.upgradingItem.rune = new Vector2Int (5, (int)itemManager.upgradingRune.effect);
            }
        }
        else if (itemManager.upgradingRune.itemName == "Endurance rune")
        {
            if (itemManager.upgradingItem.rune.x == 6)
            {
                itemManager.upgradingItem.rune = new Vector2Int (6, Mathf.Clamp(itemManager.upgradingItem.rune.y + (int)itemManager.upgradingRune.effect, 0, 50));
            }
            else
            {
                itemManager.upgradingItem.rune = new Vector2Int (6, (int)itemManager.upgradingRune.effect);
            }
        }
        else if (itemManager.upgradingRune.itemName == "Divine rune")
        {
            if (itemManager.upgradingItem.rune.x == 7)
            {
                itemManager.upgradingItem.rune = new Vector2Int (7, Mathf.Clamp(itemManager.upgradingItem.rune.y + (int)itemManager.upgradingRune.effect, 0, 50));
            }
            else
            {
                itemManager.upgradingItem.rune = new Vector2Int (7, (int)itemManager.upgradingRune.effect);
            }
        }
        else if (itemManager.upgradingRune.itemName == "Regeneration rune")
        {
            if (itemManager.upgradingItem.rune.x == 8)
            {
                itemManager.upgradingItem.rune = new Vector2Int (8, Mathf.Clamp(itemManager.upgradingItem.rune.y + (int)itemManager.upgradingRune.effect, 0, 50));
            }
            else
            {
                itemManager.upgradingItem.rune = new Vector2Int (8, (int)itemManager.upgradingRune.effect);
            }
        }
        else if (itemManager.upgradingRune.itemName == "Drain rune")
        {
            if (itemManager.upgradingItem.rune.x == 9)
            {
                itemManager.upgradingItem.rune = new Vector2Int (9, Mathf.Clamp(itemManager.upgradingItem.rune.y + (int)itemManager.upgradingRune.effect, 0, 50));
            }
            else
            {
                itemManager.upgradingItem.rune = new Vector2Int (9, (int)itemManager.upgradingRune.effect);
            }
        }
        
        sounds.PlaySound(8);
        if (itemManager.currentSectionKind == ItemManager.SectionKind.itemDetail)
        {
            itemManager.ChangeSection ("exitItemDetail");
        }
        itemManager.AddItem(GameObject.Instantiate(itemManager.upgradingItem, modifiableItems), 1);
        
        Destroy(itemManager.upgradingItem.gameObject);
        Destroy(itemManager.upgradingRune.gameObject);

        itemManager.upgradingItem = null;
        itemManager.upgradingRune = null;

        server.TransferItemsToFirebase();

        transform.GetChild(6).GetChild(3).GetComponent<Image>().enabled = false;
        transform.GetChild(6).GetChild(4).GetComponent<Image>().enabled = false;
        transform.GetChild(6).GetChild(5).GetComponent<Image>().enabled = false;
        transform.GetChild(6).GetChild(6).GetComponent<Image>().enabled = false;
        transform.GetChild(6).GetChild(7).GetComponent<Image>().enabled = false;
        transform.GetChild(6).GetChild(8).GetComponent<Image>().enabled = false;
        transform.GetChild(6).GetChild(9).GetComponent<Image>().enabled = false;
        transform.GetChild(6).GetChild(10).GetComponent<Image>().enabled = false;
        transform.GetChild(6).GetChild(11).GetComponent<Image>().enabled = false;
        transform.GetChild(6).GetChild(12).GetComponent<Image>().enabled = false;
        transform.GetChild(6).GetChild(13).GetComponent<Image>().enabled = false;
        transform.GetChild(6).GetChild(14).GetComponent<Image>().enabled = false;
        transform.GetChild(6).GetChild(15).GetComponent<Image>().enabled = false;

        transform.GetChild(6).GetChild(1).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(6).GetChild(2).GetComponent<Image>().fillAmount = 0;

        rightOpener.transform.GetChild(0).GetComponent<Image>().sprite = rewardAvailable;
        itemManager.currentSectionKind = ItemManager.SectionKind.rewards;
        itemManager.UpdateItems(true);
        processing = false;
    }

    public IEnumerator Deconstruct ()
    {
        sounds.PlaySound(5);
        processing = true;

        if (itemManager.damnedStone && itemManager.upgradingItem.rune.y != 0)
        {
            if (itemManager.items[itemManager.damnedStone] > 1)
            {
                itemManager.AddItem(itemManager.damnedStone, -1);
            }
            else 
            {
                itemManager.items.Remove(itemManager.damnedStone);
            }
        }
        itemManager.items.Remove(itemManager.upgradingItem);
        server.TransferItemsToFirebase();

        transform.GetChild(7).GetChild(22).GetComponent<Image>().enabled = false;
        transform.GetChild(7).GetChild(22).GetComponent<Image>().raycastTarget = false;
        transform.GetChild(7).GetChild(23).GetComponent<Text>().enabled = false;

        if (itemManager.currentSectionKind == ItemManager.SectionKind.itemDetail)
        {
            itemManager.ChangeSection ("exitItemDetail");
        }

        if (!itemManager.damnedStone)
        {
            transform.GetChild(7).GetChild(20).GetComponent<Image>().enabled = false;
            itemManager.damnedStone = null;
            
            bool continueSystem = true;
            if (itemManager.upgradingItem.rune.y != 0)
            {
                sounds.PlaySound(6);
                while (transform.GetChild(7).GetChild(1).GetComponent<Image>().fillAmount < 1)
                {
                    transform.GetChild(7).GetChild(1).GetComponent<Image>().fillAmount += 0.01f;
                    yield return new WaitForSeconds(0.01f);
                }

                if (Random.Range(0,4) == 0)
                {
                    sounds.PlaySound(6);
                    ExtractRune();
                    while (transform.GetChild(7).GetChild(2).GetComponent<Image>().fillAmount < 1)
                    {
                        transform.GetChild(7).GetChild(2).GetComponent<Image>().fillAmount += 0.01f;
                        yield return new WaitForSeconds(0.01f);
                    }
                    sounds.PlaySound(8);
                    continueSystem = false;
                }
                else
                {
                    sounds.PlaySound(7);
                }
            }
            if (continueSystem)
            {   
                sounds.PlaySound(6);
                transform.GetChild(7).GetChild(1).GetComponent<Image>().fillAmount = 0f;
                while (transform.GetChild(7).GetChild(5).GetComponent<Image>().fillAmount < 1)
                {
                    transform.GetChild(7).GetChild(5).GetComponent<Image>().fillAmount += 0.01f;
                    yield return new WaitForSeconds(0.01f);
                }

                sounds.PlaySound(6);
                transform.GetChild(7).GetChild(19).GetComponent<Image>().color = new Color (1, 1, 1, 1f);
                while (transform.GetChild(7).GetChild(6).GetComponent<Image>().fillAmount < 1)
                {
                    transform.GetChild(7).GetChild(6).GetComponent<Image>().fillAmount += 0.01f;
                    yield return new WaitForSeconds(0.01f);
                }
                sounds.PlaySound(8);

                int souls = 0;

                if (itemManager.upgradingItem.rune.y != 0)
                {
                    souls += Random.Range(1, 15 * itemManager.upgradingItem.rune.y);
                }

                // souls += Random.Range(1, 5 * itemManager.upgradingItem.level);

                if (itemManager.upgradingItem.itemName.Contains("+"))
                {
                    int upgrade = int.Parse(itemManager.upgradingItem.itemName.Split('+')[1]);
                    souls += Random.Range(1, 10 * upgrade);
                }

                if (itemManager.currentSectionKind == ItemManager.SectionKind.itemDetail)
                {
                    itemManager.ChangeSection ("exitItemDetail");
                }
                itemManager.AddItem(drops.allItems.Find(x => x.itemName == "Soul"), souls);
            }
        }
        else
        {
            sounds.PlaySound(6);
            while (transform.GetChild(7).GetChild(3).GetComponent<Image>().fillAmount < 1)
            {
                transform.GetChild(7).GetChild(3).GetComponent<Image>().fillAmount += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }

            sounds.PlaySound(6);
            while (transform.GetChild(7).GetChild(4).GetComponent<Image>().fillAmount < 1)
            {
                transform.GetChild(7).GetChild(4).GetComponent<Image>().fillAmount += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }

            sounds.PlaySound(6);
            ExtractRune();
            while (transform.GetChild(7).GetChild(2).GetComponent<Image>().fillAmount < 1)
            {
                transform.GetChild(7).GetChild(2).GetComponent<Image>().fillAmount += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
            sounds.PlaySound(8);
        }
        
        Destroy(itemManager.upgradingItem.gameObject);

        itemManager.upgradingItem = null;
        itemManager.damnedStone = null;

        server.TransferItemsToFirebase();

        transform.GetChild(7).GetChild(7).GetComponent<Image>().enabled = false;
        transform.GetChild(7).GetChild(8).GetComponent<Image>().enabled = false;
        transform.GetChild(7).GetChild(9).GetComponent<Image>().enabled = false;
        transform.GetChild(7).GetChild(10).GetComponent<Image>().enabled = false;
        transform.GetChild(7).GetChild(11).GetComponent<Image>().enabled = false;
        transform.GetChild(7).GetChild(12).GetComponent<Image>().enabled = false;
        transform.GetChild(7).GetChild(13).GetComponent<Image>().enabled = false;
        transform.GetChild(7).GetChild(14).GetComponent<Image>().enabled = false;
        transform.GetChild(7).GetChild(15).GetComponent<Image>().enabled = false;
        transform.GetChild(7).GetChild(16).GetComponent<Image>().enabled = false;
        transform.GetChild(7).GetChild(17).GetComponent<Image>().enabled = false;
        transform.GetChild(7).GetChild(18).GetComponent<Image>().enabled = false;
        transform.GetChild(7).GetChild(19).GetComponent<Image>().enabled = false;
        transform.GetChild(7).GetChild(20).GetComponent<Image>().enabled = false;
        transform.GetChild(7).GetChild(21).GetComponent<Image>().enabled = false;

        transform.GetChild(7).GetChild(1).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(7).GetChild(2).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(7).GetChild(3).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(7).GetChild(4).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(7).GetChild(5).GetComponent<Image>().fillAmount = 0;
        transform.GetChild(7).GetChild(6).GetComponent<Image>().fillAmount = 0;

        rightOpener.transform.GetChild(0).GetComponent<Image>().sprite = rewardAvailable;
        itemManager.currentSectionKind = ItemManager.SectionKind.rewards;
        itemManager.UpdateItems(true);
        processing = false;
    }

    void ExtractRune ()
    {
        Item instance = null;
        if (itemManager.upgradingItem.rune.x == 0)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Weakening rune"), modifiableItems);
            transform.GetChild(7).GetChild(14).GetComponent<Image>().color = new Color (1, 1, 1, 1f);
        }
        else if (itemManager.upgradingItem.rune.x == 1)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Aura rune"), modifiableItems);
            transform.GetChild(7).GetChild(14).GetComponent<Image>().color = new Color (1, 1, 1, 1f);
        }
        else if (itemManager.upgradingItem.rune.x == 2)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Critical rune"), modifiableItems);
            transform.GetChild(7).GetChild(15).GetComponent<Image>().color = new Color (1, 1, 1, 1f);
        }
        else if (itemManager.upgradingItem.rune.x == 3)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Shield rune"), modifiableItems);
            transform.GetChild(7).GetChild(15).GetComponent<Image>().color = new Color (1, 1, 1, 1f);
        }
        else if (itemManager.upgradingItem.rune.x == 4)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Agility rune"), modifiableItems);
            transform.GetChild(7).GetChild(16).GetComponent<Image>().color = new Color (1, 1, 1, 1f);
        }
        else if (itemManager.upgradingItem.rune.x == 5)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Overload rune"), modifiableItems);
            transform.GetChild(7).GetChild(16).GetComponent<Image>().color = new Color (1, 1, 1, 1f);
        }
        else if (itemManager.upgradingItem.rune.x == 6)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Endurance rune"), modifiableItems);
            transform.GetChild(7).GetChild(17).GetComponent<Image>().color = new Color (1, 1, 1, 1f);
        }
        else if (itemManager.upgradingItem.rune.x == 7)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Divine rune"), modifiableItems);
            transform.GetChild(7).GetChild(17).GetComponent<Image>().color = new Color (1, 1, 1, 1f);
        }
        else if (itemManager.upgradingItem.rune.x == 8)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Regeneration rune"), modifiableItems);
            transform.GetChild(7).GetChild(18).GetComponent<Image>().color = new Color (1, 1, 1, 1f);
        }
        else if (itemManager.upgradingItem.rune.x == 9)
        {
            instance = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == "Drain rune"), modifiableItems);
            transform.GetChild(7).GetChild(18).GetComponent<Image>().color = new Color (1, 1, 1, 1f);
        }
        instance.effect = itemManager.upgradingItem.rune.y;
        if (itemManager.currentSectionKind == ItemManager.SectionKind.itemDetail)
        {
            itemManager.ChangeSection ("exitItemDetail");
        }
        itemManager.AddItem(instance, 1);
    }

    public void Trade ()
    {
        sounds.PlaySound(35);
        server.TradeItems();

        itemManager.items.Remove(itemManager.tradingItem);

        if (itemManager.tradingItem.kind == 0 || itemManager.tradingItem.itemName.Contains("rune"))
        {
            Destroy(itemManager.tradingItem.gameObject);
        }
        itemManager.tradingItem = null;

        server.TransferItemsToFirebase();

        transform.GetChild(9).GetChild(1).GetComponent<Image>().enabled = false;

        transform.GetChild(9).GetChild(2).GetComponent<Text>().enabled = false;

        transform.GetChild(9).GetChild(3).GetComponent<Image>().enabled = false;
        transform.GetChild(9).GetChild(3).GetComponent<Image>().raycastTarget = false;
        transform.GetChild(9).GetChild(4).GetComponent<Image>().enabled = false;

        transform.GetChild(9).GetChild(5).GetComponent<Image>().enabled = false;
        transform.GetChild(9).GetChild(5).GetComponent<Image>().raycastTarget = false;
        transform.GetChild(9).GetChild(6).GetComponent<Image>().enabled = false;

        transform.GetChild(9).GetChild(7).GetComponent<Image>().enabled = false;
        transform.GetChild(9).GetChild(7).GetComponent<Image>().raycastTarget = false;
        transform.GetChild(9).GetChild(8).GetComponent<Text>().enabled = false;

        itemManager.UpdateItems(true);
    }

    public void Buy ()
    {      
        sounds.PlaySound(35);  
        itemTab.transform.GetChild(7).GetComponent<Image>().enabled = false;
        itemTab.transform.GetChild(7).GetComponent<Image>().raycastTarget = false;
        itemTab.transform.GetChild(8).GetComponent<Text>().enabled = false;

        itemManager.AddItem(drops.allItems.Find(x => x.itemName == "Trading token"), -int.Parse(server.tradingItems[server.buyingItem].Split('(')[1].Split(')')[0]));
        UpdateItems(false);

        server.BuyItem();
    }
}