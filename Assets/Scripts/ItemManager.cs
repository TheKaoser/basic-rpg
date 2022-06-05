using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public Dictionary <Item, int> items = new Dictionary<Item, int>();

    public GameObject item;
    public GameObject healthPotion;
    public GameObject damagePotion;
    public GameObject defensePotion;
    public GameObject travelersSword;
    public GameObject travelerShield;

    public GameObject itemTab;

    public int currentSection;
    
    public Image center;
    public Image left;
    public Image right;
    public List <Sprite> sections = new List<Sprite>();
    public Sprite itemDetail;
    
    public Image iconOpenerRight;
    public Sprite rewards;
    public Sprite inventory;

    public List <Sprite> runes = new List<Sprite>();  

    public Text text;
    public Drops drops;

    public enum SectionKind
    {
        normal = 0,
        itemDetail = 1,
        rewards = 2
    }
    public SectionKind currentSectionKind;

    public StatManager statManager;

    public GameManager gameManager;

    public SystemsManager systemsManager;

    public Dictionary <Item, int> newItems = new Dictionary<Item, int>();

    public Transform modifiableItems;

    public Item upgradingItem;
    public Item heavenStone;
    public Item protectionStone;
    public Item damnedStone;
    public bool souls;
    public Item upgradingRune;
    public Item tradingItem;
    public int price;

    public Server server;

    void Start()
    {
        StartCoroutine(SyncItems());
    }

    IEnumerator SyncItems()
    {
        yield return new WaitUntil(() => server.itemsSynced);
        foreach (Dictionary<string, object> item in server.items)
        {
            Item itemAux = null;
            string name = (string)item["name"];
            if (int.Parse((string)item["kind"]) == 0 || int.Parse((string)item["kind"]) == 2 && name.Contains("rune"))
            {
                string auxName = name;
                if (auxName.Contains("+"))
                {
                    auxName = auxName.Substring(0, auxName.IndexOf('+') -1);
                }
                itemAux = GameObject.Instantiate(drops.allItems.Find(x => x.itemName == auxName), modifiableItems);
                if (name.Contains("+"))
                {
                    if (name.Contains("sword"))
                    {
                        itemAux.effect += 20 * int.Parse(name.Split('+')[1]);
                    }
                    else
                    {
                        itemAux.effect += 4 * int.Parse(name.Split('+')[1]);
                    }
                    itemAux.itemName = name;
                }
                if (item.ContainsKey("rune"))
                {
                    itemAux.rune = new Vector2Int (int.Parse((string)item["rune"].ToString().Split('.')[0]), int.Parse((string)item["rune"].ToString().Split('.')[1]));
                }
                if (item.ContainsKey("effect"))
                {
                    itemAux.effect = int.Parse((string)item["effect"]);
                }

                AddItem(itemAux, int.Parse((string)item["number"]));
            }
            else
            {
                AddItem(drops.allItems.Find(x => x.itemName == name), int.Parse((string)item["number"]));
                if (name == "Experience")
                {
                    statManager.GetComponent<ExperienceManager>().UpdateExperience(int.Parse((string)item["number"]));
                }
            }            
        }
        currentSectionKind = SectionKind.rewards;
        if (GetComponent<Image>().enabled)
        {
            UpdateItems(true);
        }
    }

    public void AddItem(Item item, int number)
    {
        if (item.kind == 0 && number > 0)
        {
            items[item] = 1;
            newItems[item.GetComponent<Item>()] = 1;
        }
        else if (number == 0)
        {
            statManager.EquipGear(item);
        }
        else
        {
            if (items.ContainsKey(item))
            {
                items[item] += number;
            }
            else
            {
                items.Add(item, number);
            }

            if (newItems.ContainsKey(item.GetComponent<Item>()) && number > 0)
            {
                newItems[item.GetComponent<Item>()] += number;
            }
            else if (number > 0)
            {
                newItems.Add(item.GetComponent<Item>(), number);
            }
        }
    }

    public void UpdateItems(bool changeSection)
    {
        if (!changeSection)
        {
            foreach (Image i in GetComponentsInChildren<Image>())
            {
                i.enabled = !i.enabled;
                i.raycastTarget = !i.raycastTarget;
            }

            foreach(Image i in itemTab.GetComponentsInChildren<Image>())
            {
                i.enabled = false;
            }
            foreach(Text t in itemTab.GetComponentsInChildren<Text>())
            {
                t.enabled = false;
            }
        }
        else
        {
            if (currentSectionKind == SectionKind.itemDetail)
            {
                ChangeSection ("exitItemDetail");
            }
        }

        if (transform.GetChild(0).GetChild(0).childCount > 0)
        {
            for (int i = 0; i < transform.GetChild(0).GetChild(0).childCount; i++)
            {
                Destroy(transform.GetChild(0).GetChild(0).GetChild(i).gameObject);
            }
        }
        
        if ((transform.GetChild(0).GetChild(0).childCount == 0 || changeSection) && GetComponent<Image>().enabled)
        {
            if (currentSectionKind == SectionKind.rewards)
            {
                foreach (Item i in newItems.Keys)
                {
                    GameObject newItem = GameObject.Instantiate(item, transform.GetChild(0).GetChild(0));
                    newItem.transform.GetChild(0).GetComponent<Image>().sprite = i.sprite;
                    newItem.transform.GetChild(1).GetComponent<Text>().text = i.itemName;
                    if (i.kind != 0)
                    {
                        newItem.transform.GetChild(2).GetComponent<Text>().text = newItems[i] + "";
                    }
                    else
                    {
                        newItem.transform.GetChild(2).GetComponent<Text>().text = "";
                    }

                    foreach (Item i2 in modifiableItems.GetComponentsInChildren<Item>())
                    {
                        if (i2.itemName == i.itemName && i2.rune == i.rune && i2.effect == i.effect)
                        {
                            newItem.GetComponent<LinkToItem>().linkedItem = i2;
                        }
                    }
                }
                ChangeSection("rewards");
            }
            else
            {
                foreach (Item i in items.Keys)
                {
                    if (i.kind == currentSection && items[i] > 0)
                    {
                        GameObject newItem = GameObject.Instantiate(item, transform.GetChild(0).GetChild(0));
                        newItem.transform.GetChild(0).GetComponent<Image>().sprite = i.sprite;
                        newItem.transform.GetChild(1).GetComponent<Text>().text = i.itemName;
                        if (i.kind != 0)
                        {
                            newItem.transform.GetChild(2).GetComponent<Text>().text = items[i] + "";
                        }
                        else
                        {
                            newItem.transform.GetChild(2).GetComponent<Text>().text = "";
                        }

                        foreach (Item i2 in modifiableItems.GetComponentsInChildren<Item>())
                        {
                            if (i2.itemName == i.itemName && i2.rune == i.rune && i2.effect == i.effect)
                            {
                                newItem.GetComponent<LinkToItem>().linkedItem = i2;
                            }
                        }
                    }
                }
            }
        }
    }

    public void ChangeSection (string direction)
    {
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
        else if (currentSectionKind == SectionKind.itemDetail || currentSectionKind == SectionKind.rewards)
        {
            if (direction == "itemDetail")
            {
                right.sprite = sections[currentSection];
                left.sprite = sections[currentSection];
                center.sprite = itemDetail;
                center.color = new Color(right.color.r, right.color.g, right.color.b, 0.75f);
                right.color = new Color(right.color.r, right.color.g, right.color.b, 0.3f);
                left.color = new Color(left.color.r, left.color.g, left.color.b, 0.3f);
            }
            else if (direction == "rewards")
            {
                right.sprite = sections[currentSection];
                left.sprite = sections[currentSection];
                center.sprite = rewards;
                center.color = new Color(right.color.r, right.color.g, right.color.b, 0.75f);
                right.color = new Color(right.color.r, right.color.g, right.color.b, 0.3f);
                left.color = new Color(left.color.r, left.color.g, left.color.b, 0.3f);
            }
            else
            {
                currentSectionKind = SectionKind.normal;
                foreach(Image i in itemTab.GetComponentsInChildren<Image>())
                {
                    i.enabled = false;
                }
                foreach(Text t in itemTab.GetComponentsInChildren<Text>())
                {
                    t.enabled = false;
                }
                center.sprite = sections[currentSection];
                right.sprite = sections[(currentSection + 1) % (sections.Count)];
                left.sprite = sections[(currentSection - 1 + sections.Count) % (sections.Count)];

                center.color = new Color(right.color.r, right.color.g, right.color.b, 0.75f);
                right.color = new Color(right.color.r, right.color.g, right.color.b, 0.3f);
                left.color = new Color(left.color.r, left.color.g, left.color.b, 0.3f);
                newItems.Clear();
                iconOpenerRight.sprite = inventory;
                UpdateItems(true);
            }
        }
    }

    public void ShowItemDetail (string item, Item linkedItem = null)
    {
        for (int i = 0; i < transform.GetChild(0).GetChild(0).childCount; i++)
        {
            Destroy(transform.GetChild(0).GetChild(0).GetChild(i).gameObject);
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
        itemTab.transform.GetChild(7).GetComponent<Image>().raycastTarget = true;

        Item itemAux = null;
        if (linkedItem)
        {
            itemAux = linkedItem;
        }
        else
        {
            string name = item;

            foreach (Item i in items.Keys)
            {
                if (i.itemName == name)
                {
                    itemAux = i;
                    break;
                }
            }
        }
        
        itemTab.transform.GetChild(1).GetComponent<Image>().sprite = itemAux.sprite;
        itemTab.transform.GetChild(2).GetComponent<Text>().text = itemAux.itemName;
        
        if (itemAux.kind != 0)
        {
            itemTab.transform.GetChild(3).GetComponent<Text>().text = items[itemAux] + "";
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

        if (statManager.activePotions.ContainsKey(itemAux) || statManager.activeScrolls.ContainsKey(itemAux))
        {
            itemTab.transform.GetChild(7).GetComponent<Image>().enabled = false;
            itemTab.transform.GetChild(7).GetComponent<Image>().raycastTarget = false;
            itemTab.transform.GetChild(8).GetComponent<Text>().enabled = false;
        }

        if (itemAux.kind == 0)
        {
            itemTab.transform.GetChild(8).GetComponent<Text>().text = "Equip";
        }
        else if (itemAux.kind == 1)
        {
            itemTab.transform.GetChild(8).GetComponent<Text>().text = "Consume";
        }
        else if (itemAux.kind == 2)
        {
            itemTab.transform.GetChild(8).GetComponent<Text>().text = "Use";
        }

        if (systemsManager.processing)
        {
            DisableUseButton();
        }

        if (itemAux.kind == 0 && systemsManager.currentSectionKind == SystemsManager.SectionKind.sorcerer)
        {
            if (systemsManager.currentSection != 2 && systemsManager.currentSection != 4 && systemsManager.currentSection != 5 || !systemsManager.GetComponent<Image>().enabled)
            {
                DisableUseButton();
            }
            else
            {
                itemTab.transform.GetChild(8).GetComponent<Text>().text = "Use";
            }
        }
        else if (systemsManager.currentSectionKind == SystemsManager.SectionKind.well || gameManager.place == "well")
        {
            if (systemsManager.currentSection != 1 || !systemsManager.GetComponent<Image>().enabled)
            {
                DisableUseButton();
            }
            else
            {
                itemTab.transform.GetChild(8).GetComponent<Text>().text = "Trade";
            }
        }
        else if (itemAux.kind == 1 && systemsManager.currentSectionKind == SystemsManager.SectionKind.sorcerer)
        {
            DisableUseButton();
        }
        else if (itemAux.kind == 2 && systemsManager.currentSectionKind == SystemsManager.SectionKind.sorcerer && systemsManager.currentSection == 0)
        {
            if (itemAux.type != "Essence" || items[itemAux] < 5 || !systemsManager.GetComponent<Image>().enabled)
            {
                DisableUseButton();
            }
        }
        else if (itemAux.kind == 2 && systemsManager.currentSectionKind == SystemsManager.SectionKind.sorcerer && systemsManager.currentSection == 1)
        {
            if (itemAux.type != "Soul" || items[itemAux] < 5 || !systemsManager.GetComponent<Image>().enabled)
            {
                DisableUseButton();
            }
        }
        else if (itemAux.kind == 2 && systemsManager.currentSectionKind == SystemsManager.SectionKind.sorcerer && systemsManager.currentSection == 2)
        {
            if (itemAux.itemName != "Heaven stone" && itemAux.itemName != "Protection stone" || !systemsManager.GetComponent<Image>().enabled)
            {
                DisableUseButton();
            }
        }
        else if (itemAux.kind == 2 && systemsManager.currentSectionKind == SystemsManager.SectionKind.sorcerer && systemsManager.currentSection == 3)
        {
            if (itemAux.type != "Rune" && itemAux.itemName != "Soul" || !systemsManager.GetComponent<Image>().enabled)
            {
                DisableUseButton();
            }
        }
        else if (itemAux.kind == 2 && systemsManager.currentSectionKind == SystemsManager.SectionKind.sorcerer && systemsManager.currentSection == 4)
        {
            if (itemAux.type != "Rune" || !systemsManager.GetComponent<Image>().enabled)
            {
                DisableUseButton();
            }
        }
        else if (itemAux.kind == 2 && systemsManager.currentSectionKind == SystemsManager.SectionKind.sorcerer && systemsManager.currentSection == 5)
        {
            if (itemAux.itemName != "Damned stone" || !systemsManager.GetComponent<Image>().enabled)
            {
                DisableUseButton();
            }
        }
        else if (itemAux.kind == 2)
        {
            DisableUseButton();
        }
    }

    void DisableUseButton ()
    {
        itemTab.transform.GetChild(7).GetComponent<Image>().enabled = false;
        itemTab.transform.GetChild(7).GetComponent<Image>().raycastTarget = false;
        itemTab.transform.GetChild(8).GetComponent<Text>().enabled = false;
    }

    public void UseItem (string item, Vector2Int rune)
    {
        string name = item;

        Item itemAux = null;
        foreach (Item i in items.Keys)
        {
            if (i.itemName == name && (i.rune == rune || i.effect == rune.y) && items[i] > 0)
            {
                itemAux = i;
                break;
            }
        }

        if (systemsManager.currentSectionKind == SystemsManager.SectionKind.well && systemsManager.currentSection == 1)
        {
            gameManager.GetComponent<Sounds>().PlaySound(4);
            tradingItem = itemAux;

            systemsManager.transform.GetChild(9).GetChild(1).GetComponent<Image>().sprite = itemAux.sprite;
            systemsManager.transform.GetChild(9).GetChild(1).GetComponent<Image>().enabled = true;

            systemsManager.transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "0";
            systemsManager.transform.GetChild(9).GetChild(2).GetComponent<Text>().enabled = true;

            systemsManager.transform.GetChild(9).GetChild(3).GetComponent<Image>().enabled = true;
            systemsManager.transform.GetChild(9).GetChild(3).GetComponent<Image>().raycastTarget = true;
            systemsManager.transform.GetChild(9).GetChild(4).GetComponent<Image>().enabled = true;

            systemsManager.transform.GetChild(9).GetChild(5).GetComponent<Image>().enabled = true;
            systemsManager.transform.GetChild(9).GetChild(5).GetComponent<Image>().raycastTarget = true;
            systemsManager.transform.GetChild(9).GetChild(6).GetComponent<Image>().enabled = true;
        }
        else
        {
            if (itemAux.kind == 0)
            {
                gameManager.GetComponent<Sounds>().PlaySound(4);
                if (systemsManager.currentSectionKind == SystemsManager.SectionKind.sorcerer && systemsManager.currentSection == 2)
                {
                    upgradingItem = itemAux;

                    systemsManager.transform.GetChild(4).GetChild(6).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(4).GetChild(7).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(4).GetChild(8).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(4).GetChild(9).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(4).GetChild(10).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(4).GetChild(11).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(4).GetChild(12).GetComponent<Image>().enabled = false;

                    if (itemAux.type == "Sword")
                    {
                        systemsManager.transform.GetChild(4).GetChild(6).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.type == "Helmet")
                    {
                        systemsManager.transform.GetChild(4).GetChild(7).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.type == "Chest")
                    {
                        systemsManager.transform.GetChild(4).GetChild(8).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.type == "Gloves")
                    {
                        systemsManager.transform.GetChild(4).GetChild(9).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.type == "Shield")
                    {
                        systemsManager.transform.GetChild(4).GetChild(10).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.type == "Greaves")
                    {
                        systemsManager.transform.GetChild(4).GetChild(11).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.type == "Boots")
                    {
                        systemsManager.transform.GetChild(4).GetChild(12).GetComponent<Image>().enabled = true;
                    }
                    
                    if (heavenStone)
                    {
                        systemsManager.transform.GetChild(4).GetChild(16).GetComponent<Image>().enabled = true;
                        systemsManager.transform.GetChild(4).GetChild(16).GetComponent<Image>().raycastTarget = true;
                        systemsManager.transform.GetChild(4).GetChild(17).GetComponent<Text>().enabled = true;

                        systemsManager.transform.GetChild(4).GetChild(15).GetComponent<Image>().enabled = true;
                    }
                }
                else if (systemsManager.currentSectionKind == SystemsManager.SectionKind.sorcerer && systemsManager.currentSection == 4)
                {
                    upgradingItem = itemAux;

                    systemsManager.transform.GetChild(6).GetChild(3).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(6).GetChild(4).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(6).GetChild(5).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(6).GetChild(6).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(6).GetChild(7).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(6).GetChild(8).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(6).GetChild(9).GetComponent<Image>().enabled = false;

                    if (itemAux.type == "Sword")
                    {
                        systemsManager.transform.GetChild(6).GetChild(3).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.type == "Helmet")
                    {
                        systemsManager.transform.GetChild(6).GetChild(4).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.type == "Chest")
                    {
                        systemsManager.transform.GetChild(6).GetChild(5).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.type == "Gloves")
                    {
                        systemsManager.transform.GetChild(6).GetChild(6).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.type == "Shield")
                    {
                        systemsManager.transform.GetChild(6).GetChild(7).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.type == "Greaves")
                    {
                        systemsManager.transform.GetChild(6).GetChild(8).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.type == "Boots")
                    {
                        systemsManager.transform.GetChild(6).GetChild(9).GetComponent<Image>().enabled = true;
                    }
                    
                    if (upgradingRune)
                    {
                        systemsManager.transform.GetChild(6).GetChild(16).GetComponent<Image>().enabled = true;
                        systemsManager.transform.GetChild(6).GetChild(16).GetComponent<Image>().raycastTarget = true;
                        systemsManager.transform.GetChild(6).GetChild(17).GetComponent<Text>().enabled = true;

                        systemsManager.transform.GetChild(6).GetChild(15).GetComponent<Image>().enabled = true;
                    }
                }
                else if (systemsManager.currentSectionKind == SystemsManager.SectionKind.sorcerer && systemsManager.currentSection == 5)
                {
                    upgradingItem = itemAux;

                    systemsManager.transform.GetChild(7).GetChild(7).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(7).GetChild(8).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(7).GetChild(9).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(7).GetChild(10).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(7).GetChild(11).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(7).GetChild(12).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(7).GetChild(13).GetComponent<Image>().enabled = false;

                    if (itemAux.type == "Sword")
                    {
                        systemsManager.transform.GetChild(7).GetChild(7).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.type == "Helmet")
                    {
                        systemsManager.transform.GetChild(7).GetChild(8).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.type == "Chest")
                    {
                        systemsManager.transform.GetChild(7).GetChild(9).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.type == "Gloves")
                    {
                        systemsManager.transform.GetChild(7).GetChild(10).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.type == "Shield")
                    {
                        systemsManager.transform.GetChild(7).GetChild(11).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.type == "Greaves")
                    {
                        systemsManager.transform.GetChild(7).GetChild(12).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.type == "Boots")
                    {
                        systemsManager.transform.GetChild(7).GetChild(13).GetComponent<Image>().enabled = true;
                    }

                    upgradingItem = itemAux;

                    if (itemAux.rune.y != 0)
                    {
                        systemsManager.transform.GetChild(7).GetChild(14 + itemAux.rune.x / 2).GetComponent<Image>().enabled = true;
                        systemsManager.transform.GetChild(7).GetChild(14 + itemAux.rune.x / 2).GetComponent<Image>().color = new Color (1, 1, 1, 0.5f);
                    }

                    systemsManager.transform.GetChild(7).GetChild(19).GetComponent<Image>().enabled = true;
                    systemsManager.transform.GetChild(7).GetChild(19).GetComponent<Image>().color = new Color (1, 1, 1, 0.5f);
                    
                    systemsManager.transform.GetChild(7).GetChild(22).GetComponent<Image>().enabled = true;
                    systemsManager.transform.GetChild(7).GetChild(22).GetComponent<Image>().raycastTarget = true;
                    systemsManager.transform.GetChild(7).GetChild(23).GetComponent<Text>().enabled = true;

                    systemsManager.transform.GetChild(7).GetChild(21).GetComponent<Image>().enabled = true;
                }
                else
                {
                    statManager.EquipGear(itemAux.GetComponent<Item>());
                    server.TransferItemsToFirebase();
                }
            }
            else if (itemAux.kind == 1)
            {
                if (items[itemAux] == 1)
                {
                    items.Remove(itemAux);
                }
                else
                {
                    items[itemAux] -= 1;
                }

                if (itemAux.type == "Potion")
                {
                    statManager.ActivatePotion(itemAux.GetComponent<Item>()); 
                    server.TransferItemsToFirebase();
                }
                else if (itemAux.type == "Scroll")
                {
                    statManager.ActivateScroll(itemAux.GetComponent<Item>());
                    server.TransferItemsToFirebase();
                }
            }
            else if (itemAux.kind == 2)
            {
                gameManager.GetComponent<Sounds>().PlaySound(4);
                if (itemAux.type == "Essence")
                {
                    systemsManager.transform.GetChild(2).GetChild(5).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(2).GetChild(6).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(2).GetChild(7).GetComponent<Image>().enabled = false;

                    systemsManager.transform.GetChild(2).GetChild(5 + itemAux.level).GetComponent<Image>().enabled = true;

                    systemsManager.transform.GetChild(2).GetChild(8).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(2).GetChild(9).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(2).GetChild(10).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(2).GetChild(11).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(2).GetChild(12).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(2).GetChild(13).GetComponent<Image>().enabled = false;

                    systemsManager.transform.GetChild(2).GetChild(8 + itemAux.level * 2).GetComponent<Image>().enabled = true;
                    systemsManager.transform.GetChild(2).GetChild(8 + itemAux.level * 2).GetComponent<Image>().color = new Color (1, 1, 1, 0.5f);
                    systemsManager.transform.GetChild(2).GetChild(9 + itemAux.level * 2).GetComponent<Image>().enabled = true;
                    systemsManager.transform.GetChild(2).GetChild(9 + itemAux.level * 2).GetComponent<Image>().color = new Color (1, 1, 1, 0.5f);
                    
                    systemsManager.transform.GetChild(2).GetChild(15).GetComponent<Image>().enabled = true;
                    systemsManager.transform.GetChild(2).GetChild(15).GetComponent<Image>().raycastTarget = true;
                    systemsManager.transform.GetChild(2).GetChild(16).GetComponent<Text>().enabled = true;

                    systemsManager.transform.GetChild(2).GetChild(14).GetComponent<Image>().enabled = true;
                }
                else if (itemAux.type == "Soul" && systemsManager.currentSection == 1)
                {
                    systemsManager.transform.GetChild(3).GetChild(11).GetComponent<Image>().enabled = true;

                    systemsManager.transform.GetChild(3).GetChild(17).GetComponent<Image>().enabled = true;
                    systemsManager.transform.GetChild(3).GetChild(16).GetComponent<Image>().enabled = true;
                    systemsManager.transform.GetChild(3).GetChild(16).GetComponent<Image>().color = new Color (1, 1, 1, 0.5f);
                    systemsManager.transform.GetChild(3).GetChild(15).GetComponent<Image>().enabled = true;
                    systemsManager.transform.GetChild(3).GetChild(15).GetComponent<Image>().color = new Color (1, 1, 1, 0.5f);
                    systemsManager.transform.GetChild(3).GetChild(14).GetComponent<Image>().enabled = true;
                    systemsManager.transform.GetChild(3).GetChild(14).GetComponent<Image>().color = new Color (1, 1, 1, 0.5f);
                    systemsManager.transform.GetChild(3).GetChild(13).GetComponent<Image>().enabled = true;
                    systemsManager.transform.GetChild(3).GetChild(13).GetComponent<Image>().color = new Color (1, 1, 1, 0.5f);
                    systemsManager.transform.GetChild(3).GetChild(12).GetComponent<Image>().enabled = true;
                    systemsManager.transform.GetChild(3).GetChild(12).GetComponent<Image>().color = new Color (1, 1, 1, 0.5f);

                    systemsManager.transform.GetChild(3).GetChild(18).GetComponent<Image>().enabled = true;
                    systemsManager.transform.GetChild(3).GetChild(18).GetComponent<Image>().raycastTarget = true;
                    systemsManager.transform.GetChild(3).GetChild(19).GetComponent<Text>().enabled = true;
                }
                else if (itemAux.itemName == "Heaven stone")
                {
                    heavenStone = itemAux;

                    systemsManager.transform.GetChild(4).GetChild(13).GetComponent<Image>().enabled = true;
                    
                    if (upgradingItem)
                    {
                        systemsManager.transform.GetChild(4).GetChild(16).GetComponent<Image>().enabled = true;
                        systemsManager.transform.GetChild(4).GetChild(16).GetComponent<Image>().raycastTarget = true;
                        systemsManager.transform.GetChild(4).GetChild(17).GetComponent<Text>().enabled = true;

                        systemsManager.transform.GetChild(4).GetChild(15).GetComponent<Image>().enabled = true;
                    }
                }
                else if (itemAux.itemName == "Protection stone")
                {
                    protectionStone = itemAux;

                    systemsManager.transform.GetChild(4).GetChild(14).GetComponent<Image>().enabled = true;
                }
                else if (itemAux.itemName == "Soul" && systemsManager.currentSection == 3)
                {
                    souls = true;

                    systemsManager.transform.GetChild(5).GetChild(8).GetComponent<Image>().enabled = true;

                    if (upgradingRune)
                    {
                        systemsManager.transform.GetChild(5).GetChild(10).GetComponent<Image>().enabled = true;
                        systemsManager.transform.GetChild(5).GetChild(10).GetComponent<Image>().raycastTarget = true;
                        systemsManager.transform.GetChild(5).GetChild(11).GetComponent<Text>().enabled = true;

                        systemsManager.transform.GetChild(5).GetChild(9).GetComponent<Image>().enabled = true;
                    }
                }
                else if (itemAux.type == "Rune" && systemsManager.currentSection == 3)
                {
                    upgradingRune = itemAux;

                    systemsManager.transform.GetChild(6).GetChild(3).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(6).GetChild(4).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(6).GetChild(5).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(6).GetChild(6).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(6).GetChild(7).GetComponent<Image>().enabled = false;

                    if (itemAux.itemName == "Weakening rune" || itemAux.itemName == "Aura rune")
                    {
                        systemsManager.transform.GetChild(5).GetChild(3).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.itemName == "Critical rune" || itemAux.itemName == "Shield rune")
                    {
                        systemsManager.transform.GetChild(5).GetChild(4).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.itemName == "Agility rune" || itemAux.itemName == "Overload rune")
                    {
                        systemsManager.transform.GetChild(5).GetChild(5).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.itemName == "Endurance rune" || itemAux.itemName == "Divine rune")
                    {
                        systemsManager.transform.GetChild(5).GetChild(6).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.itemName == "Regeneration rune" || itemAux.itemName == "Drain rune")
                    {
                        systemsManager.transform.GetChild(5).GetChild(7).GetComponent<Image>().enabled = true;
                    }

                    if (souls)
                    {
                        systemsManager.transform.GetChild(5).GetChild(10).GetComponent<Image>().enabled = true;
                        systemsManager.transform.GetChild(5).GetChild(10).GetComponent<Image>().raycastTarget = true;
                        systemsManager.transform.GetChild(5).GetChild(11).GetComponent<Text>().enabled = true;

                        systemsManager.transform.GetChild(5).GetChild(9).GetComponent<Image>().enabled = true;
                    }
                }
                else if (itemAux.type == "Rune" && systemsManager.currentSection == 4)
                {
                    upgradingRune = itemAux;

                    systemsManager.transform.GetChild(6).GetChild(10).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(6).GetChild(11).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(6).GetChild(12).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(6).GetChild(13).GetComponent<Image>().enabled = false;
                    systemsManager.transform.GetChild(6).GetChild(14).GetComponent<Image>().enabled = false;

                    if (itemAux.itemName == "Weakening rune" || itemAux.itemName == "Aura rune")
                    {
                        systemsManager.transform.GetChild(6).GetChild(10).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.itemName == "Critical rune" || itemAux.itemName == "Shield rune")
                    {
                        systemsManager.transform.GetChild(6).GetChild(11).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.itemName == "Agility rune" || itemAux.itemName == "Overload rune")
                    {
                        systemsManager.transform.GetChild(6).GetChild(12).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.itemName == "Endurance rune" || itemAux.itemName == "Divine rune")
                    {
                        systemsManager.transform.GetChild(6).GetChild(13).GetComponent<Image>().enabled = true;
                    }
                    else if (itemAux.itemName == "Regeneration rune" || itemAux.itemName == "Drain rune")
                    {
                        systemsManager.transform.GetChild(6).GetChild(14).GetComponent<Image>().enabled = true;
                    }

                    if (upgradingItem)
                    {
                        systemsManager.transform.GetChild(6).GetChild(16).GetComponent<Image>().enabled = true;
                        systemsManager.transform.GetChild(6).GetChild(16).GetComponent<Image>().raycastTarget = true;
                        systemsManager.transform.GetChild(6).GetChild(17).GetComponent<Text>().enabled = true;

                        systemsManager.transform.GetChild(6).GetChild(15).GetComponent<Image>().enabled = true;
                    }
                }
                else if (itemAux.itemName == "Damned stone")
                {
                    damnedStone = itemAux;

                    systemsManager.transform.GetChild(7).GetChild(20).GetComponent<Image>().enabled = true;
                }
            }
        }

        ChangeSection ("exitItemDetail");
    }

    public void ChangePrice (int changeAmount)
    {
        price = Mathf.Clamp (price + changeAmount, 0, 100000);
        if (price != 0)
        {
            systemsManager.transform.GetChild(9).GetChild(2).GetComponent<Text>().text = price + "";
            
            systemsManager.transform.GetChild(9).GetChild(7).GetComponent<Image>().enabled = true;
            systemsManager.transform.GetChild(9).GetChild(7).GetComponent<Image>().raycastTarget = true;
            systemsManager.transform.GetChild(9).GetChild(8).GetComponent<Text>().enabled = true;
        }
        else
        {
            systemsManager.transform.GetChild(9).GetChild(2).GetComponent<Text>().text = "0";

            systemsManager.transform.GetChild(9).GetChild(7).GetComponent<Image>().enabled = false;
            systemsManager.transform.GetChild(9).GetChild(7).GetComponent<Image>().raycastTarget = false;
            systemsManager.transform.GetChild(9).GetChild(8).GetComponent<Text>().enabled = false;
        }
    }
}