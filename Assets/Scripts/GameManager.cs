using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject you;
    public List <GameObject> others = new List<GameObject>();

    public GameObject firehound;
    public GameObject imp;
    public GameObject condemned;
    public GameObject demon;
    
    public GameObject wolf;
    public GameObject elf;
    public GameObject bandit;
    public GameObject spider;

    public GameObject rat;
    public GameObject goblin;
    public GameObject skeleton;
    public GameObject troll;

    public Text placeText;
    public string place = "city";
    public int stage = 0;
    public int level = 1;
    public string state = "alive";

    public ParticleSystem travelEffect;
    public ParticleSystem deadEffect;
    public ParticleSystem currentTravelEffect;
    public GameObject modifiableItems;
    public GameObject shines;

    public GameObject portalGameObject;
    public GameObject sorcererGameObject;
    public GameObject wellGameObject;
    public GameObject chestGameObject;

    public Sprite citySprite;
    public Sprite volcanoSprite;
    public Sprite forestSprite;
    public Sprite dungeonSprite;

    public Sprite portalSprite;
    public Sprite arenaSprite;
    public Sprite sorcererPlaceSprite;
    public Sprite sorcererMenuSprite;
    public Sprite wellPlaceSprite;
    public Sprite wellMenuSprite;
    public Sprite chestPlaceSprite;
    public Sprite chestMenuSprite;
    public Sprite statsSprite;

    public Sprite levelUpSprite;
    public Sprite levelDownSprite;
    public Sprite fightSprite;
    
    public Image right;
    public Image left;
    public Image up;
    public Image down;

    public Image rightOpener;
    public Image leftOpener;

    Vector3 velocityOther = Vector3.zero;
    Vector3 velocityOther2 = Vector3.zero;
    Vector3 velocityOther3 = Vector3.zero;
    bool goToInteractPosition;
    bool goToMidPosition;
    float currentTime = 0f;
    public float timeToMove = 0.835f;
    float timeToTravel = 0.5f;

    public GameObject combatInterface;
    public ItemManager itemManager;
    public SystemsManager systemsManager;

    public GameObject updateManager;

    void Awake ()
    {
        Travel(1);
        GetComponent<Sounds>().PlaySound(30, 0, true);
        combatInterface.GetComponentInChildren<UpdateHealth>().ResetHealths(false);
        ChangeSprites();
        combatInterface.GetComponent<UpdateHealth>().UpdateLife(-1, (float)you.GetComponent<Stats>().currentHealth/(float)you.GetComponent<Stats>().maxHealth);
    }

    void Update()
    {
        if (GetComponent<MapActions>().canDrag)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                StartCoroutine(GoTo("up"));
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                StartCoroutine(GoTo("down"));
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                StartCoroutine(GoTo("right"));
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                StartCoroutine(GoTo("left"));
            }
        }

        if (goToInteractPosition)
        {
            if (currentTime <= timeToMove)
            {
                currentTime += Time.deltaTime;
                you.transform.position = new Vector3(Mathf.Lerp(0f, -2f, currentTime / timeToMove), you.transform.position.y, 0);
            }
            else
            {
                you.transform.position = new Vector3(-2f, you.transform.position.y, 0);
                goToInteractPosition = false;
                currentTime = 0f;
                if (others[0].CompareTag("Foe"))
                {
                    GetComponent<Combat>().StartCombat();
                }
            }

            for (int i = 0; i<others.Count; i++)
            {
                if (currentTime > timeToMove/2)
                {
                    if (i == 0)
                    {
                        others[i].transform.position = Vector3.SmoothDamp(others[i].transform.position, new Vector3(3f, others[i].transform.position.y, 0), ref velocityOther, 0.2f);
                    }
                    else if (i == 1)
                    {
                        others[i].transform.position = Vector3.SmoothDamp(others[i].transform.position, new Vector3(5f, others[i].transform.position.y, 0), ref velocityOther2, 0.2f);
                    }
                    else
                    {
                        others[i].transform.position = Vector3.SmoothDamp(others[i].transform.position, new Vector3(7f, others[i].transform.position.y, 0), ref velocityOther3, 0.2f);
                    }
                }
            }
        }

        if (goToMidPosition)
        {
            if (currentTime <= timeToMove)
            {
                currentTime += Time.deltaTime;
                you.transform.position = new Vector3(Mathf.Lerp(-2f, 0f, currentTime / timeToMove), you.transform.position.y, 0);
            }
            if (currentTime >= timeToMove)
            {
                you.transform.position = new Vector3(0f, you.transform.position.y, 0);
                goToMidPosition = false;
                currentTime = 0f;
            }

            if (others.Count != 0)
            {
                if (others[0].name != "Portal(Clone)" && others[0].name != "Chest(Clone)")
                {
                    others[0].transform.position = Vector3.SmoothDamp(others[0].transform.position, new Vector3(15f, others[0].transform.position.y, 0), ref velocityOther, 0.2f);
                }
                else if (currentTime > timeToMove/2)
                {
                    others[0].transform.position = new Vector3 (15, others[0].transform.position.y, 0);
                }
            }
        }

        if (state == "dead" && you.GetComponent<Stats>().currentHealth == you.GetComponent<Stats>().maxHealth)
        {
            PlayDeadEffect(false);
            you.GetComponent<Animator>().Play("Revive");
            state = "alive";
            StartCoroutine (GoTo("revive"));
        }
    }

    public IEnumerator GoTo (string direction)
    {
        currentTime = 0;
        GetComponent<MapActions>().canDrag = false;
        foreach (Image i in leftOpener.GetComponentsInChildren<Image>())
        {
            i.enabled = false;
        }
        foreach (Image i in rightOpener.GetComponentsInChildren<Image>())
        {
            i.enabled = false;
        }
        
        if (direction == "left" && state == "dead")
        {
            Invoke("ClearOthers", timeToMove/2);
            combatInterface.GetComponentInChildren<UpdateHealth>().ResetHealths(false);
            PlayDeadEffect(true);
            GetComponent<Sounds>().PlaySound(9, 0, true);
            GetComponent<Sounds>().PauseSound(31);
            GetComponent<Sounds>().PauseSound(32);
            GetComponent<Sounds>().PauseSound(33);
            GetComponent<Sounds>().PauseSound(34);
            currentTravelEffect = GameObject.Instantiate(travelEffect);
            GetComponent<Sounds>().PlaySound(0);
            yield return new WaitForSeconds(timeToTravel);
            Travel(1);
            yield return new WaitForSeconds(timeToMove-timeToTravel);
            place = "city";
            ChangeSprites();
        }
        else if (direction == "right" && (place == "volcano" || place == "forest" || place == "dungeon")) 
        {
            ClearOthers();
            HighlightSprite("right");
            if (stage < 10)
            {
                SpawnRandomEnemy();
                GetComponent<Combat>().seed = Time.time % 200f;
                GoToInteractPosition();
                yield return new WaitForSeconds(timeToMove);
                stage++;
            }
            else
            {
                others.Add(GameObject.Instantiate(chestGameObject));
                GoToInteractPosition();
                yield return new WaitForSeconds(timeToMove);
                GetComponent<Sounds>().PlaySound(10);
                others[0].GetComponentInChildren<ParticleSystem>().Play();
                others[0].GetComponent<SpriteRenderer>().enabled = false;
                Dictionary <Item, int> aux = GetComponent<Drops>().ChooseItems(place, stage, level, you.GetComponent<ExperienceManager>().yourLevel);                    
                foreach (Item i in aux.Keys)
                {
                    if (i.itemName == "Experience")
                    {
                        you.GetComponent<ExperienceManager>().UpdateExperience(aux[i]);
                    }
                    itemManager.AddItem(i, aux[i]);
                }
                GetComponent<Server>().TransferItemsToFirebase ();
                if (aux.Count > 0)
                {
                    rightOpener.transform.GetChild(0).GetComponent<Image>().sprite = chestMenuSprite;
                    itemManager.currentSectionKind = ItemManager.SectionKind.rewards;
                }
                you.GetComponent<StatManager>().DisablePotionEffect();
                yield return new WaitForSeconds(0.6f);
                GoToMidPosition();
                yield return new WaitForSeconds(timeToMove);
                ChangeSprites();
                stage = 0;
                GetComponent<MapActions>().canDrag = true;
                foreach (Image i in leftOpener.GetComponentsInChildren<Image>())
                {
                    i.enabled = true;
                }
                foreach (Image i in rightOpener.GetComponentsInChildren<Image>())
                {
                    i.enabled = true;
                }
            }
            ChangeSprites();
        }
        else if (direction == "left" && place == "city") 
        {
            currentTravelEffect = GameObject.Instantiate(travelEffect);
            GetComponent<Sounds>().PlaySound(0);
            GetComponent<Sounds>().PauseSound(30);
            yield return new WaitForSeconds(timeToTravel);
            Travel(5);
            yield return new WaitForSeconds(timeToMove-timeToTravel);
            GetComponent<Sounds>().PlaySound(34, 0.2f, true);
            place = "arena";
            ChangeSprites();
            StartCoroutine(GetComponent<Arena>().PrepareForCombat());
            GetComponent<MapActions>().canDrag = true;
        }
        else
        {
            HighlightSprite(direction);
            if (direction == "revive") 
            {
                GetComponent<Sounds>().PauseSound(9);
                yield return new WaitForSeconds(2.5f);
                GoToMidPosition();
                yield return new WaitForSeconds(timeToMove);
                GetComponent<Sounds>().PlaySound(30, 0, true);
                foreach (ParticleSystem p in shines.transform.GetComponentsInChildren<ParticleSystem>())
                {
                    p.Play(true);
                }
                place = "city";
                ChangeSprites();
            }
            else if (direction == "right" && place == "city") 
            {
                others.Add(GameObject.Instantiate(portalGameObject));
                GoToInteractPosition();
                GetComponent<Sounds>().PlaySound(15, 0.2f, true, true);
                yield return new WaitForSeconds(timeToMove);
                place = "portal";
                ChangeSprites();
            }
            else if (direction == "up" && place == "city") 
            {
                others.Add(GameObject.Instantiate(sorcererGameObject));
                GoToInteractPosition();
                GetComponent<Sounds>().PlaySound(17, 0.6f);
                yield return new WaitForSeconds(timeToMove);
                place = "sorcerer";
                systemsManager.currentSection = 0;
                systemsManager.currentSectionKind = SystemsManager.SectionKind.sorcerer;
                ChangeSprites();
            }
            else if (direction == "down" && place == "city") 
            {
                others.Add(GameObject.Instantiate(wellGameObject));
                GoToInteractPosition();
                GetComponent<Sounds>().PlaySound(18, 0.1f);
                yield return new WaitForSeconds(timeToMove);
                place = "well";
                systemsManager.currentSection = 0;
                systemsManager.currentSectionKind = SystemsManager.SectionKind.well;
                ChangeSprites();
            }
            else if (direction == "right" && place == "portal") 
            {
                Invoke("ClearOthers", timeToMove/2);
                GoToMidPosition();
                yield return new WaitForSeconds(timeToTravel);
                currentTravelEffect = GameObject.Instantiate(travelEffect);
                GetComponent<Sounds>().PauseSound(15);
                GetComponent<Sounds>().PlaySound(0);
                GetComponent<Sounds>().PauseSound(30);
                yield return new WaitForSeconds(timeToTravel);
                GetComponent<Sounds>().PlaySound(32, 0, true);
                Travel(3);
                place = "forest";
                stage = 0;
                level = 1;
                ChangeSprites();
            }
            else if (direction == "left" && place == "portal") 
            {
                Invoke("ClearOthers", timeToMove/2);
                others[0].name = "Portal";
                GoToMidPosition();
                GetComponent<Sounds>().PauseSound(15);
                yield return new WaitForSeconds(timeToMove);
                place = "city";
                ChangeSprites();
            }
            else if (direction == "up" && place == "portal") 
            {
                Invoke("ClearOthers", timeToMove/2);
                GoToMidPosition();
                yield return new WaitForSeconds(timeToTravel);
                currentTravelEffect = GameObject.Instantiate(travelEffect);
                GetComponent<Sounds>().PauseSound(15);
                GetComponent<Sounds>().PlaySound(0);
                GetComponent<Sounds>().PauseSound(30);
                yield return new WaitForSeconds(timeToTravel);
                GetComponent<Sounds>().PlaySound(31, 0, true);
                Travel(2);
                place = "volcano";
                stage = 0;
                level = 1;
                ChangeSprites();
            }
            else if (direction == "down" && place == "portal") 
            {
                Invoke("ClearOthers", timeToMove/2);
                GoToMidPosition();
                yield return new WaitForSeconds(timeToTravel);
                currentTravelEffect = GameObject.Instantiate(travelEffect);
                GetComponent<Sounds>().PauseSound(15);
                GetComponent<Sounds>().PlaySound(0);
                GetComponent<Sounds>().PauseSound(30);
                yield return new WaitForSeconds(timeToTravel);
                GetComponent<Sounds>().PlaySound(33, 0, true);
                Travel(4);
                place = "dungeon";
                stage = 0;
                level = 1;
                ChangeSprites();
            }
            else if ((direction == "right" || direction == "left" || direction == "up" || direction == "down") && (place == "sorcerer" || place == "well")) 
            {
                Invoke("ClearOthers", timeToMove/2);
                GoToMidPosition();
                yield return new WaitForSeconds(timeToMove);
                place = "city";
                systemsManager.currentSection = 0;
                systemsManager.currentSectionKind = SystemsManager.SectionKind.normal;
                ChangeSprites();
            }
            else if ((direction == "right" || direction == "left" || direction == "up" || direction == "down") && place == "arena") 
            {
                GetComponent<Server>().DeleteArenaId();
                currentTravelEffect = GameObject.Instantiate(travelEffect);
                GetComponent<Sounds>().PlaySound(0);
                GetComponent<Sounds>().PauseSound(34);
                yield return new WaitForSeconds(timeToTravel);
                Travel (1);
                yield return new WaitForSeconds(timeToMove-timeToTravel);
                GetComponent<Sounds>().PlaySound(30, 0.2f, true);
                place = "city";
                ChangeSprites();
            }
            else if (direction == "left" && (place == "volcano" || place == "forest" || place == "dungeon")) 
            {
                ClearOthers();
                combatInterface.GetComponentInChildren<UpdateHealth>().ResetHealths(false);
                currentTravelEffect = GameObject.Instantiate(travelEffect);
                GetComponent<Sounds>().PlaySound(0);
                GetComponent<Sounds>().PauseSound(31);
                GetComponent<Sounds>().PauseSound(32);
                GetComponent<Sounds>().PauseSound(33);
                yield return new WaitForSeconds(timeToTravel);
                Travel(1);
                yield return new WaitForSeconds(timeToMove-timeToTravel);
                GetComponent<Sounds>().PlaySound(30, 0.2f, true);
                place = "city";
                ChangeSprites();
            }
            else if (direction == "up" && (place == "volcano" || place == "forest" || place == "dungeon")) 
            {
                level += 1;
                stage = 0;
                ChangeSprites();
            }
            else if (direction == "down" && (place == "volcano" || place == "forest" || place == "dungeon")) 
            {
                level -= 1;
                stage = 0;
                ChangeSprites();
            }
            GetComponent<MapActions>().canDrag = true;
            foreach (Image i in leftOpener.GetComponentsInChildren<Image>())
            {
                i.enabled = true;
            }
            foreach (Image i in rightOpener.GetComponentsInChildren<Image>())
            {
                i.enabled = true;
            }
        }
    }

    void SpawnRandomEnemy ()
    {
        if (stage < 9)
        {
            for (int i = 0; i < stage/3+1; i++)
            {
                int rand = Random.Range(1,4);
                if (rand == 1)
                {
                    if (place == "volcano")
                    {
                        others.Add(GameObject.Instantiate(firehound, firehound.transform.position + new Vector3(i*2, -0.5f*i, 0), Quaternion.identity));
                    }
                    else if (place == "forest")
                    {
                        others.Add(GameObject.Instantiate(wolf, wolf.transform.position + new Vector3(i*2, -0.5f*i, 0), Quaternion.identity));
                    }
                    else
                    {
                        others.Add(GameObject.Instantiate(rat, rat.transform.position + new Vector3(i*2, -0.5f*i, 0), Quaternion.identity));
                    }
                }
                else if (rand == 2)
                {
                    if (place == "volcano")
                    {
                        others.Add(GameObject.Instantiate(imp, imp.transform.position + new Vector3(i*2, -0.5f*i, 0), Quaternion.identity));
                    }
                    else if (place == "forest")
                    {
                        others.Add(GameObject.Instantiate(elf, elf.transform.position + new Vector3(i*2, -0.5f*i, 0), Quaternion.identity));
                    }
                    else
                    {
                        others.Add(GameObject.Instantiate(goblin, goblin.transform.position + new Vector3(i*2, -0.5f*i, 0), Quaternion.identity));
                    }
                }
                else
                {
                    if (place == "volcano")
                    {
                        others.Add(GameObject.Instantiate(condemned, condemned.transform.position + new Vector3(i*2, -0.5f*i, 0), Quaternion.identity));
                    }
                    else if (place == "forest")
                    {
                        others.Add(GameObject.Instantiate(bandit, bandit.transform.position + new Vector3(i*2, -0.5f*i, 0), Quaternion.identity));
                    }
                    else
                    {
                        others.Add(GameObject.Instantiate(skeleton, skeleton.transform.position + new Vector3(i*2, -0.5f*i, 0), Quaternion.identity));
                    }
                }
                int j = 0;
                foreach (Renderer r  in others[i].GetComponentsInChildren<SpriteRenderer>())
                {
                    r.sortingOrder = 2 * i + j;
                    j++;
                }
            }
            
            foreach (GameObject foe in others)
            {
                if (level != 1)
                {
                    foe.GetComponent<Stats>().maxHealth = foe.GetComponent<Stats>().maxHealth * (level - 1) * 1.5f;
                    foe.GetComponent<Stats>().currentHealth = (int)foe.GetComponent<Stats>().maxHealth;
                    foe.GetComponent<Stats>().damage = foe.GetComponent<Stats>().damage * (level - 1) * 1.5f;
                    foe.GetComponent<Stats>().defense = foe.GetComponent<Stats>().defense * (level - 1) * 1.5f;
                }
            }
        }
        else if (stage == 9)
        {
            if (place == "volcano")
            {
                others.Add(GameObject.Instantiate(demon));
            }
            else if (place == "forest")
            {
                others.Add(GameObject.Instantiate(spider));
            }
            else
            {
                others.Add(GameObject.Instantiate(troll));
            }
            if (level != 1)
            {
                others[0].GetComponent<Stats>().maxHealth = others[0].GetComponent<Stats>().maxHealth * (level - 1) * 1.5f;
                others[0].GetComponent<Stats>().currentHealth = (int)others[0].GetComponent<Stats>().maxHealth;
                others[0].GetComponent<Stats>().damage = others[0].GetComponent<Stats>().damage * (level - 1) * 1.5f;
                others[0].GetComponent<Stats>().defense = others[0].GetComponent<Stats>().defense * (level - 1) * 1.5f;
            }
        }
    }

    void Travel (int place)
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(you);
        DontDestroyOnLoad(combatInterface.transform.parent.gameObject);
        DontDestroyOnLoad(deadEffect.gameObject);
        DontDestroyOnLoad(modifiableItems);
        DontDestroyOnLoad(shines);
        DontDestroyOnLoad(updateManager);
        if (currentTravelEffect)
        {
            DontDestroyOnLoad(currentTravelEffect.gameObject);
        }
        SceneManager.LoadScene(place);
    }

    public void GoToInteractPosition ()
    {
        velocityOther = Vector3.zero;
        velocityOther2 = Vector3.zero;
        velocityOther3 = Vector3.zero;
        you.GetComponent<Animator>().Play("Walk Backwards");
        GetComponent<Sounds>().PlaySound(1);
        goToInteractPosition = true;
    }

    public void GoToMidPosition ()
    {
        velocityOther = Vector3.zero;
        velocityOther2 = Vector3.zero;
        velocityOther3 = Vector3.zero;
        you.GetComponent<Animator>().Play("Walk");
        GetComponent<Sounds>().PlaySound(1);
        goToMidPosition = true;
    }

    void PlayDeadEffect (bool activate)
    {
        if (activate)
        {
            deadEffect.Play(true);
            you.transform.position = new Vector3 (15, you.transform.position.y, 1);
        }
        else
        {
            deadEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            you.transform.position = new Vector3 (-2, you.transform.position.y, 1);
        }
    }

    void ChangeSprites ()
    {
        if (place == "city")
        {
            right.sprite = portalSprite;
            left.sprite = arenaSprite;
            up.sprite = sorcererPlaceSprite;
            down.sprite = wellPlaceSprite;
            right.color = new Color(1f, 1f, 1f, 0f);
            left.color = new Color(1f, 1f, 1f, 0f);
            up.color = new Color(1f, 1f, 1f, 0f);
            down.color = new Color(1f, 1f, 1f, 0f);
            leftOpener.transform.GetChild(0).GetComponent<Image>().sprite = statsSprite;
        }
        else if (place == "portal")
        {
            right.sprite = forestSprite;
            left.sprite = citySprite;
            up.sprite = volcanoSprite;
            down.sprite = dungeonSprite;
            right.color = new Color(1f, 1f, 1f, 0f);
            left.color = new Color(1f, 1f, 1f, 0f);
            up.color = new Color(1f, 1f, 1f, 0f);
            down.color = new Color(1f, 1f, 1f, 0f);
        }
        else if (place == "arena" || place == "sorcerer" || place == "well")
        {
            right.sprite = citySprite;
            left.sprite = citySprite;
            up.sprite = citySprite;
            down.sprite = citySprite;
            if (place == "arena")
            {
                right.color = new Color(1f, 1f, 1f, 0f);
                left.color = new Color(1f, 1f, 1f, 0f);
                up.color = new Color(1f, 1f, 1f, 0f);
                down.color = new Color(1f, 1f, 1f, 0f);
            }
            else
            {
                right.color = new Color(1f, 1f, 1f, 0f);
                left.color = new Color(1f, 1f, 1f, 0f);
                up.color = new Color(1f, 1f, 1f, 0f);
                down.color = new Color(1f, 1f, 1f, 0f);
                if (place == "sorcerer")
                {
                    leftOpener.transform.GetChild(0).GetComponent<Image>().sprite = sorcererMenuSprite;
                }
                else
                {
                    leftOpener.transform.GetChild(0).GetComponent<Image>().sprite = wellMenuSprite;
                }
            }
        }
        else if (place == "forest" || place == "volcano" || place == "dungeon")
        {
            if (stage == 10)
            {
                right.sprite = chestPlaceSprite;
                left.sprite = citySprite;
                up.sprite = levelUpSprite;
                down.sprite = levelDownSprite;
                right.color = new Color(1f, 1f, 1f, 0f);
                left.color = new Color(1f, 1f, 1f, 0f);
                up.color = new Color(1f, 1f, 1f, 0f);
                down.color = new Color(1f, 1f, 1f, 0f);
            }
            else
            {    
                right.sprite = fightSprite;
                left.sprite = citySprite;
                up.sprite = levelUpSprite;
                down.sprite = levelDownSprite;
                right.color = new Color(1f, 1f, 1f, 0f);
                left.color = new Color(1f, 1f, 1f, 0f);
                up.color = new Color(1f, 1f, 1f, 0f);
                down.color = new Color(1f, 1f, 1f, 0f);
            }
        }
        if (place == "forest" || place == "dungeon" || place == "volcano")
        {
            if (stage == 0)
            {
                placeText.text =  place.ToUpper()[0] + place.Substring(1) + " " + level + "-" + 1;
            }
            else
            {
                placeText.text =  place.ToUpper()[0] + place.Substring(1) + " " + level + "-" + stage;
            }
        }
        else
        {
            placeText.text =  place.ToUpper()[0] + place.Substring(1);
        }
    }

    void HighlightSprite (string direction)
    {
        if (direction == "right")
        {
            right.color = new Color(right.color.r, right.color.g, right.color.b, 0.5f);
        }
        else if (direction == "left")
        {
            left.color = new Color(left.color.r, left.color.g, left.color.b, 0.5f);
        }
        else if (direction == "up")
        {
            up.color = new Color(up.color.r, up.color.g, up.color.b, 0.5f);
        }
        else if (direction == "down")
        {
            down.color = new Color(down.color.r, down.color.g, down.color.b, 0.5f);
        }
    }

    void ClearOthers ()
    {
        if (others.Count > 0)
        {
            for (int i = 0; i < others.Count; i++)
            {
                GameObject.Destroy(others[i].gameObject);
            }
            others.Clear();
        }
    }
}
