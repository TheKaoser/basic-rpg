using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuActions : MonoBehaviour
{
    public GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;
    
    public StatManager statManager;

    public Image layoutLeft;
    public Image layoutRight;

    public Image leftOpener;
    public Image rightOpener;
    public Image use;
    public Image unequip;
    public Image craftGear;
    public Image craftRune;
    public Image upgradeGear;
    public Image upgradeRune;
    public Image addRune;
    public Image deconstruct;
    public Image increasePrice;
    public Image decreasePrice;
    public Image trade;

    public GameObject itemDetail;

    private Vector2 touchPosition;
    private Vector3 direction;
    Vector2 firstPosition;
    bool hasChangedSection;

    GameObject currentInteraction;
    GameObject firstInteraction;

    public Image centerRL;
    public Image leftRL;
    public Image rightRL;

    public Image centerLL;
    public Image leftLL;
    public Image rightLL;

    public Text text;
    bool navigatingMenus;

    public Item inItemDetailRight;
    public Item inItemDetailLeft;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            layoutRight.GetComponent<ItemManager>().UpdateItems(false);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            layoutLeft.GetComponent<SystemsManager>().UpdateItems(false);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            layoutRight.GetComponent<ItemManager>().ChangeSection("right");
            layoutLeft.GetComponent<SystemsManager>().ChangeSection("right");
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            layoutRight.GetComponent<ItemManager>().ChangeSection("left");
            layoutLeft.GetComponent<SystemsManager>().ChangeSection("left");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            layoutRight.GetComponent<ItemManager>().ShowItemDetail("Traveler's sword");
        }

        if (Input.touchCount > 0)
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            m_Raycaster.Raycast(m_PointerEventData, results);

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                currentInteraction = null;
                Touch touch = Input.GetTouch(0);
                touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                
                firstPosition = touchPosition;
                
                if (results.Count > 0)
                {
                    currentInteraction = results[0].gameObject;
                    firstInteraction = results[0].gameObject;
                    text.text = currentInteraction.name;
                    GetComponent<MapActions>().canDrag = false;
                    navigatingMenus = true;
                }

                if (results.Count > 0)
                {
                    if (results[0].gameObject.CompareTag("Item"))
                    {
                        results[0].gameObject.transform.localScale = new Vector3 (0.9f, 0.9f, 1f);  
                    }
                    else if (results[0].gameObject.name == "Opener right")
                    {
                        results[0].gameObject.transform.localScale = new Vector3 (0.9f, 0.9f, 1f);  
                    }
                    else if (results[0].gameObject.name == "Opener left")
                    {
                        results[0].gameObject.transform.localScale = new Vector3 (0.9f, 0.9f, 1f);  
                    }
                    else if (results[0].gameObject.name == "Use")
                    {
                        results[0].gameObject.transform.localScale = new Vector3 (0.9f, 0.9f, 1f);
                    }
                    else if (results[0].gameObject.CompareTag("Equiped"))
                    {
                        results[0].gameObject.transform.localScale = new Vector3 (0.9f, 0.9f, 1f);  
                    }
                    else if (results[0].gameObject.name == "Unequip")
                    {
                        results[0].gameObject.transform.localScale = new Vector3 (0.9f, 0.9f, 1f);
                    }
                    else if (results[0].gameObject.name == "Craft gear")
                    {
                        results[0].gameObject.transform.localScale = new Vector3 (0.9f, 0.9f, 1f);
                    }
                    else if (results[0].gameObject.name == "Craft rune")
                    {
                        results[0].gameObject.transform.localScale = new Vector3 (0.9f, 0.9f, 1f);
                    }
                    else if (results[0].gameObject.name == "Upgrade gear")
                    {
                        results[0].gameObject.transform.localScale = new Vector3 (0.9f, 0.9f, 1f);
                    }
                    else if (results[0].gameObject.name == "Upgrade rune")
                    {
                        results[0].gameObject.transform.localScale = new Vector3 (0.9f, 0.9f, 1f);
                    }
                    else if (results[0].gameObject.name == "Add rune")
                    {
                        results[0].gameObject.transform.localScale = new Vector3 (0.9f, 0.9f, 1f);
                    }
                    else if (results[0].gameObject.name == "Deconstruct")
                    {
                        results[0].gameObject.transform.localScale = new Vector3 (0.9f, 0.9f, 1f);
                    }
                    else if (results[0].gameObject.name == "Increase price")
                    {
                        results[0].gameObject.transform.localScale = new Vector3 (0.9f, 0.9f, 1f);
                    }
                    else if (results[0].gameObject.name == "Decrease price")
                    {
                        results[0].gameObject.transform.localScale = new Vector3 (0.9f, 0.9f, 1f);
                    }
                    else if (results[0].gameObject.name == "Trade")
                    {
                        results[0].gameObject.transform.localScale = new Vector3 (0.9f, 0.9f, 1f);
                    }
                    else if (results[0].gameObject.CompareTag("TradingItem"))
                    {
                        results[0].gameObject.transform.localScale = new Vector3 (0.9f, 0.9f, 1f);
                    }
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && firstInteraction != null && (firstInteraction.CompareTag("LayoutRight") || firstInteraction.CompareTag("Item")))
            {
                Touch touch = Input.GetTouch(0);
                touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                
                direction = (touchPosition - firstPosition);
                if (Vector2.Distance(touchPosition, firstPosition) > 0.5f)
                {
                    currentInteraction = null;
                    Unselect();
                }

                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    if (!hasChangedSection)
                    {
                        rightRL.color = new Color(rightRL.color.r, rightRL.color.g, rightRL.color.b, Mathf.Clamp(0.3f -(direction.x * 0.25f), -0.75f, 0.75f));
                        leftRL.color = new Color(leftRL.color.r, leftRL.color.g, leftRL.color.b, Mathf.Clamp(0.3f + direction.x * 0.25f, -0.75f, 0.75f));
        
                        if (rightRL.color.a > 0.7f)
                        {
                            layoutRight.GetComponent<ItemManager>().ChangeSection("right");
                            hasChangedSection = true;
                        }
                        else if (leftRL.color.a > 0.7f)
                        {
                            layoutRight.GetComponent<ItemManager>().ChangeSection("left");
                            hasChangedSection = true;
                        }
                    }
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && firstInteraction != null && (firstInteraction.CompareTag("LayoutLeft") || firstInteraction.CompareTag("Equiped") || firstInteraction.CompareTag("TradingItem")) && !layoutLeft.GetComponent<SystemsManager>().processing)
            {
                Touch touch = Input.GetTouch(0);
                touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                
                direction = (touchPosition - firstPosition);
                if (Vector2.Distance(touchPosition, firstPosition) > 0.5f)
                {
                    currentInteraction = null;
                    Unselect();
                }

                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    if (!hasChangedSection)
                    {
                        rightLL.color = new Color(rightLL.color.r, rightLL.color.g, rightLL.color.b, Mathf.Clamp(0.3f -(direction.x * 0.25f), -0.75f, 0.75f));
                        leftLL.color = new Color(leftLL.color.r, leftLL.color.g, leftLL.color.b, Mathf.Clamp(0.3f + direction.x * 0.25f, -0.75f, 0.75f));
        
                        if (rightLL.color.a > 0.7f)
                        {
                            layoutLeft.GetComponent<SystemsManager>().ChangeSection("right");
                            hasChangedSection = true;
                        }
                        else if (leftLL.color.a > 0.7f)
                        {
                            layoutLeft.GetComponent<SystemsManager>().ChangeSection("left");
                            hasChangedSection = true;
                        }
                    }
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                rightRL.color = new Color(rightRL.color.r, rightRL.color.g, rightRL.color.b, 0.3f);
                leftRL.color = new Color(leftRL.color.r, leftRL.color.g, leftRL.color.b, 0.3f);

                rightLL.color = new Color(rightLL.color.r, rightLL.color.g, rightLL.color.b, 0.3f);
                leftLL.color = new Color(leftLL.color.r, leftLL.color.g, leftLL.color.b, 0.3f);

                Unselect();

                if (results.Count > 0)
                {
                    if (currentInteraction == results[0].gameObject)
                    {
                        if (results[0].gameObject.name == "Opener right")
                        {
                            layoutRight.GetComponent<ItemManager>().UpdateItems(false);
                            GetComponent<Sounds>().PlaySound(2);
                        }
                        else if (results[0].gameObject.name == "Opener left")
                        {
                            if (layoutLeft.GetComponent<SystemsManager>().currentSectionKind == SystemsManager.SectionKind.itemDetail)
                            {
                                if (GetComponent<GameManager>().place != "well")
                                {
                                    layoutLeft.GetComponent<SystemsManager>().currentSectionKind = SystemsManager.SectionKind.normal;
                                }
                                else
                                {
                                    layoutLeft.GetComponent<SystemsManager>().currentSectionKind = SystemsManager.SectionKind.well;
                                }
                            }
                            layoutLeft.GetComponent<SystemsManager>().UpdateItems(false);
                            GetComponent<Sounds>().PlaySound(2);
                        }
                        else if (results[0].gameObject.CompareTag("Item"))
                        {
                            if (results[0].gameObject.GetComponent<LinkToItem>().linkedItem)
                            {
                                layoutRight.GetComponent<ItemManager>().ShowItemDetail("", results[0].gameObject.GetComponent<LinkToItem>().linkedItem);
                            }
                            else 
                            {
                                layoutRight.GetComponent<ItemManager>().ShowItemDetail(results[0].gameObject.transform.GetChild(1).GetComponent<Text>().text);
                            }
                        }
                        else if (results[0].gameObject.name == "Use")
                        {
                            Vector2Int rune = new Vector2Int();
                            if (results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().enabled)
                            {
                                if (results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[0] == "Weakening")
                                {
                                    rune = new Vector2Int (0, int.Parse(results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[1].TrimEnd('.')));
                                }
                                else if (results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[0] == "Aura")
                                {
                                    rune = new Vector2Int (1, int.Parse(results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[1].TrimEnd('.')));
                                }
                                else if (results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[0] == "Critical")
                                {
                                    rune = new Vector2Int (2, int.Parse(results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[1].TrimEnd('.')));
                                }
                                else if (results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[0] == "Shield")
                                {
                                    rune = new Vector2Int (3, int.Parse(results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[1].TrimEnd('.')));
                                }
                                else if (results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[0] == "Agility")
                                {
                                    rune = new Vector2Int (4, int.Parse(results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[1].TrimEnd('.')));
                                }
                                else if (results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[0] == "Overload")
                                {
                                    rune = new Vector2Int (5, int.Parse(results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[1].TrimEnd('.')));
                                }
                                else if (results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[0] == "Endurance")
                                {
                                    rune = new Vector2Int (6, int.Parse(results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[1].TrimEnd('.')));
                                }
                                else if (results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[0] == "Divine")
                                {
                                    rune = new Vector2Int (7, int.Parse(results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[1].TrimEnd('.')));
                                }
                                else if (results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[0] == "Regeneration")
                                {
                                    rune = new Vector2Int (8, int.Parse(results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[1].TrimEnd('.')));
                                }
                                else if (results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[0] == "Drain")
                                {
                                    rune = new Vector2Int (9, int.Parse(results[0].gameObject.transform.parent.GetChild(6).GetComponent<Text>().text.Split(' ')[1].TrimEnd('.')));
                                }
                            }
                            if (results[0].gameObject.transform.parent.GetChild(2).GetComponent<Text>().text.Contains("rune"))
                            {
                                rune.y = int.Parse(results[0].gameObject.transform.parent.GetChild(4).GetComponent<Text>().text.Split('(')[1].Split(')')[0]);
                            }
                            layoutRight.GetComponent<ItemManager>().UseItem(results[0].gameObject.transform.parent.GetChild(2).GetComponent<Text>().text, rune);
                        }
                        else if (results[0].gameObject.CompareTag("Equiped"))
                        {
                            layoutLeft.GetComponent<SystemsManager>().ShowItemDetail(results[0].gameObject.name);
                        }
                        else if (results[0].gameObject.name == "Unequip or Trade")
                        {
                            if (results[0].gameObject.transform.parent.GetChild(8).GetComponent<Text>().text == "Unequip")
                            {
                                GetComponent<Sounds>().PlaySound(4);
                                layoutLeft.GetComponent<SystemsManager>().UnequipItem(results[0].gameObject.transform.parent.GetChild(2).GetComponent<Text>().text);
                            }
                            else
                            {
                                layoutLeft.GetComponent<SystemsManager>().Buy();
                            }
                        }
                        else if (results[0].gameObject.name == "Craft gear")
                        {
                            StartCoroutine(layoutLeft.GetComponent<SystemsManager>().CraftGear());
                        }
                        else if (results[0].gameObject.name == "Craft rune")
                        {
                            StartCoroutine(layoutLeft.GetComponent<SystemsManager>().CraftRune());
                        }
                        else if (results[0].gameObject.name == "Upgrade gear")
                        {
                            StartCoroutine(layoutLeft.GetComponent<SystemsManager>().UpgradeGear());
                        }
                        else if (results[0].gameObject.name == "Upgrade rune")
                        {
                            StartCoroutine(layoutLeft.GetComponent<SystemsManager>().UpgradeRune());
                        }
                        else if (results[0].gameObject.name == "Add rune")
                        {
                            StartCoroutine(layoutLeft.GetComponent<SystemsManager>().AddRune());
                        }
                        else if (results[0].gameObject.name == "Deconstruct")
                        {
                            StartCoroutine(layoutLeft.GetComponent<SystemsManager>().Deconstruct());
                        }
                        else if (results[0].gameObject.name == "Increase price")
                        {
                            layoutRight.GetComponent<ItemManager>().ChangePrice(5);
                        }
                        else if (results[0].gameObject.name == "Decrease price")
                        {
                            layoutRight.GetComponent<ItemManager>().ChangePrice(-5);
                        }
                        else if (results[0].gameObject.name == "Trade")
                        {
                            layoutLeft.GetComponent<SystemsManager>().Trade();
                        }
                        else if (results[0].gameObject.CompareTag("TradingItem"))
                        {
                            layoutLeft.GetComponent<SystemsManager>().ShowItemDetail("", results[0].gameObject.GetComponent<LinkToItem>().linkedItem);
                        }
                    }
                }

                if (!layoutRight.enabled && !layoutLeft.enabled && navigatingMenus)
                {
                    GetComponent<MapActions>().canDrag = true;
                    navigatingMenus = false;
                }
                hasChangedSection = false;
            }
        }
    }

    void Unselect ()
    {
        for (int i = 0; i < layoutRight.transform.GetChild(0).GetChild(0).childCount; i++)
        {
            layoutRight.transform.GetChild(0).GetChild(0).GetChild(i).transform.localScale = new Vector3 (1f, 1f, 1f);
        }

        for (int i = 0; i < layoutLeft.transform.GetChild(0).childCount; i++)
        {
            layoutLeft.transform.GetChild(0).GetChild(i).transform.localScale = new Vector3 (1f, 1f, 1f);
        }

        for (int i = 0; i < layoutLeft.transform.GetChild(8).GetChild(0).childCount; i++)
        {
            layoutLeft.transform.GetChild(8).GetChild(0).GetChild(i).transform.localScale = new Vector3 (1f, 1f, 1f);
        }

        leftOpener.transform.localScale = new Vector3 (1f, 1f, 1f);
        rightOpener.transform.localScale = new Vector3 (1f, 1f, 1f);
        use.transform.localScale = new Vector3 (1f, 1f, 1f);
        unequip.transform.localScale = new Vector3 (1f, 1f, 1f);
        craftGear.transform.localScale = new Vector3 (1f, 1f, 1f);
        craftGear.transform.localScale = new Vector3 (1f, 1f, 1f);
        craftRune.transform.localScale = new Vector3 (1f, 1f, 1f);
        upgradeGear.transform.localScale = new Vector3 (1f, 1f, 1f);
        upgradeRune.transform.localScale = new Vector3 (1f, 1f, 1f);
        addRune.transform.localScale = new Vector3 (1f, 1f, 1f);
        deconstruct.transform.localScale = new Vector3 (1f, 1f, 1f);
        increasePrice.transform.localScale = new Vector3 (1f, 1f, 1f);
        decreasePrice.transform.localScale = new Vector3 (1f, 1f, 1f);
        trade.transform.localScale = new Vector3 (1f, 1f, 1f);
    }
}
