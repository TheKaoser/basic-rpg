using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    public GameObject you;
    public List <GameObject> foes = new List<GameObject>();

    public GameObject combatInterface;

    List <int> currentHealthFoes = new List<int>(3);

    bool combatEnded;

    public GameManager gameManager;
    public ItemManager itemManager;
    public StatManager statManager;

    string textPrinted;

    public Image leftOpener;
    public Image rightOpener;

    public Sprite rewardAvailable;
    public Sprite inventory;

    public float seed;
    public bool seedCreator;

    public Text username;
    public Text oponentUsername;

    public void StartCombat()
    {
        Debug.Log ("Seed " + seed);
        combatInterface.GetComponentInChildren<UpdateHealth>().ResetHealths(true);

        foes.AddRange(gameManager.others);
        
        currentHealthFoes.Clear();
        for (int i = 0; i < foes.Count; i++)
        {
            currentHealthFoes.Add(foes[i].GetComponent<Stats>().currentHealth);
            combatInterface.GetComponent<UpdateHealth>().differenceFoe[i].fillAmount = 1;
            combatInterface.GetComponent<UpdateHealth>().currentFoe[i].fillAmount = 1;
            combatInterface.GetComponent<UpdateHealth>().maxHealthFoe.fillAmount = 1;
            combatInterface.GetComponent<UpdateHealth>().waitBeforeGoingDownFoe[i] = 1;
        }        

        combatEnded = false;
        
        if (gameManager.place != "arena")
        {
            foreach (Item i in itemManager.items.Keys)
            {
                if (i.itemName == "Experience" && Application.platform == RuntimePlatform.Android)
                {
                    GetComponent<Server>().items.RemoveAt(GetComponent<Server>().items.FindIndex(x => x.ContainsValue("Experience")));
                    Dictionary<string, object> d = new Dictionary<string, object>();
                    d ["name"] = "Experience";
                    d ["kind"] = "2";
                    d ["number"] = (int)(itemManager.items[i] * 0.8f) + "";
                    GetComponent<Server>().items.Add(d);

                    GetComponent<Server>().UpdateFirebase();
                    break;
                }
            }
        }
        
        if (seedCreator)
        {
            StartCoroutine("Attack", you);
            foreach (GameObject foe in foes)
            {
                StartCoroutine("Attack", foe);
            }
        }
        else
        {
            foreach (GameObject foe in foes)
            {
                StartCoroutine("Attack", foe);
            }
            StartCoroutine("Attack", you);
        }
    }

    IEnumerator Attack (GameObject attacker)
    {
        if (attacker == you && gameManager.place != "arena")
        {
            yield return new WaitForSeconds(0.3f);
        }

        while (!combatEnded && attacker != null)
        {
            DealDamage (Hit (attacker), attacker);

            float timeForNextAttack = 2f; 
            if (attacker != you && gameManager.place != "arena")
            {
                timeForNextAttack = 2f - (float)attacker.GetComponent<Stats>().agility / 100f * 1.5f;
            }
            else
            {
                timeForNextAttack = 2f - (float)attacker.GetComponent<Stats>().agility / 100f * 1.1f;
            } 

            if (attacker != you && you.GetComponent<Stats>().weakening != 0)
            {
                timeForNextAttack *= (1 + (float)you.GetComponent<Stats>().weakening / 100f);
            }

            if (attacker == you && foes[0].GetComponent<Stats>().weakening != 0)
            {
                timeForNextAttack *= (1 + (float)foes[0].GetComponent<Stats>().weakening / 100f);
            }

            yield return new WaitForSeconds(timeForNextAttack);
        }
    }

    float Hit (GameObject attacker)
    {
        float minDamage = attacker.GetComponent<Stats>().damage * 0.9f;
        float maxDamage = attacker.GetComponent<Stats>().damage * 1.1f;
        float damageDealt = (seed * 7) % (maxDamage - minDamage) + minDamage;
        seed = (seed * 7) % 200;

        damageDealt += 1.5f * (float)attacker.GetComponent<Stats>().currentHealth / (float)attacker.GetComponent<Stats>().maxHealth * (float)attacker.GetComponent<Stats>().damage 
                            * (float)attacker.GetComponent<Stats>().overload / 100f;

        if (Critical(attacker))
        {    
            attacker.GetComponent<Animator>().Play("Critical");
            damageDealt *= 2f;
            if (attacker == you || attacker.name.Contains("Oponent"))
            {
                StartCoroutine(MoveGearParticles(attacker));
                GetComponent<Sounds>().PlaySound(12);
            }  
        }
        else
        {
            if (attacker.GetComponent<Stats>().currentHealth > 0)
            {
                attacker.GetComponent<Animator>().Play("Attack");
                if (attacker == you || attacker.name.Contains("Oponent"))
                {
                    StartCoroutine(MoveGearParticles(attacker));
                    GetComponent<Sounds>().PlaySound(11);
                }
                else if (attacker.name.Contains("Bandit"))
                {
                    GetComponent<Sounds>().PlaySound(19);
                }
                else if (attacker.name.Contains("Condemned"))
                {
                    GetComponent<Sounds>().PlaySound(20);
                }
                else if (attacker.name.Contains("Demon"))
                {
                    GetComponent<Sounds>().PlaySound(21);
                }
                else if (attacker.name.Contains("Demon"))
                {
                    GetComponent<Sounds>().PlaySound(21);
                }
                else if (attacker.name.Contains("Elf"))
                {
                    GetComponent<Sounds>().PlaySound(22);
                }
                else if (attacker.name.Contains("Goblin"))
                {
                    GetComponent<Sounds>().PlaySound(23);
                }
                else if (attacker.name.Contains("Imp"))
                {
                    GetComponent<Sounds>().PlaySound(24);
                }
                else if (attacker.name.Contains("Rat"))
                {
                    GetComponent<Sounds>().PlaySound(25);
                }
                else if (attacker.name.Contains("Skeleton"))
                {
                    GetComponent<Sounds>().PlaySound(26);
                }
                else if (attacker.name.Contains("Spider"))
                {
                    GetComponent<Sounds>().PlaySound(27);
                }
                else if (attacker.name.Contains("Troll"))
                {
                    GetComponent<Sounds>().PlaySound(28);
                }
                else if (attacker.name.Contains("Wolf") || attacker.name.Contains("Firehound"))
                {
                    GetComponent<Sounds>().PlaySound(29);
                }
            }
        }
        
        if (attacker.transform.GetChild(1).GetComponentInChildren<ParticleSystem>() != null)
        {
            attacker.transform.GetChild(1).GetComponentInChildren<ParticleSystem>().Play();
        }

        return damageDealt;
    }

    IEnumerator MoveGearParticles (GameObject attacker)
    {
        if (attacker == you && statManager.particleSword)
        {
            statManager.particleSword.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            yield return new WaitForSeconds (0.3f);
            statManager.particleSword.Play(true);
        }
        else if (GetComponent<Server>().swordParticles)
        {
            GetComponent<Server>().swordParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            yield return new WaitForSeconds (0.3f);
            GetComponent<Server>().swordParticles.Play(true);
        }
    }

    bool Critical (GameObject attacker)
    {
        float rand = seed % 200f;
        seed = (seed * 7) % 200;

        if (rand <= attacker.GetComponent<Stats>().critical)
        {
            textPrinted = "Critical Hit: ";
            return true;
        }
        else
        {
            textPrinted = "Hit: ";
            return false;
        }
    }

    void DealDamage (float damageDealt, GameObject attacker)
    {
        if (foes.Count == 0)
        {
            return;
        }

        GameObject receiver;
        if (attacker == you)
        {
            receiver = foes[0];
        }
        else
        {
            receiver = you;
        }

        float rand = seed % 200f;
        seed = (seed * 7) % 200;

        if (rand <= receiver.GetComponent<Stats>().shield)
        {
            StartCoroutine(PlayShieldAnimation(attacker.GetComponent<Animator>().runtimeAnimatorController.animationClips[1].length - 0.4f, receiver));
        }
        else
        {
            float defense = (float)receiver.GetComponent<Stats>().defense;
            
            defense += 1.5f * (1 - (float)receiver.GetComponent<Stats>().currentHealth / (float)receiver.GetComponent<Stats>().maxHealth) * (float)receiver.GetComponent<Stats>().defense 
                            * (float)receiver.GetComponent<Stats>().endurance / 15f;

            if ((float)receiver.GetComponent<Stats>().divine != 0)
            {
                float maxDefense = defense * (float)receiver.GetComponent<Stats>().divine / 100f * 6f;
                defense = (seed * 7) % (maxDefense - defense) + defense;
                seed = (seed * 7) % 200;
            }

            if (attacker.GetComponent<Stats>().aura != 0)
            {
                defense *= - (float)attacker.GetComponent<Stats>().aura / 60f;
            }
            
            damageDealt *= receiver.GetComponent<Stats>().maxHealth / ((float)receiver.GetComponent<Stats>().maxHealth + defense);

            if (attacker.GetComponent<Stats>().drain > 0)
            {
                int drained = (int)(0.25f * damageDealt * (float)attacker.GetComponent<Stats>().drain / 100f);
                
                if (attacker != you)
                {
                    int newHealth = currentHealthFoes[0] + drained;
                    if (attacker.name.Contains("Oponent"))
                    {
                        StartCoroutine(ChangeHealth(foes.Count-1, -2, drained, 0.48f));
                        StartCoroutine(combatInterface.GetComponent<TextSpawner>().SpawnNewText(1, attacker, "Drained: " + drained, 0.48f));
                    }
                    else
                    {
                        StartCoroutine(ChangeHealth(foes.Count-1, -2, drained, attacker.GetComponent<Animator>().runtimeAnimatorController.animationClips[1].length));
                        StartCoroutine(combatInterface.GetComponent<TextSpawner>().SpawnNewText(1, attacker, "Drained: " + drained, attacker.GetComponent<Animator>().runtimeAnimatorController.animationClips[1].length));
                    }
                }
                else
                {
                    int newHealth = you.GetComponent<Stats>().currentHealth + drained;
                    StartCoroutine(ChangeHealth(-1, -2, drained, 0.48f));
                    StartCoroutine(combatInterface.GetComponent<TextSpawner>().SpawnNewText(0, attacker, "Drained: " + drained, 0.48f));
                }
            }

            if (receiver == you)
            {
                int newHealth = you.GetComponent<Stats>().currentHealth - (int)damageDealt;
                if (attacker.name.Contains("Oponent"))
                {
                    StartCoroutine(ChangeHealth(-1, foes.FindIndex(x => x == attacker), - (int)damageDealt, 0.48f));
                    StartCoroutine(combatInterface.GetComponent<TextSpawner>().SpawnNewText(0, attacker, textPrinted + (int)damageDealt, 0.48f));
                }
                else
                {
                    StartCoroutine(ChangeHealth(-1, foes.FindIndex(x => x == attacker), - (int)damageDealt, attacker.GetComponent<Animator>().runtimeAnimatorController.animationClips[1].length));
                    StartCoroutine(combatInterface.GetComponent<TextSpawner>().SpawnNewText(0, attacker, textPrinted + (int)damageDealt, attacker.GetComponent<Animator>().runtimeAnimatorController.animationClips[1].length));
                }
            }
            else
            {
                int newHealth = currentHealthFoes[0] - (int)damageDealt;
                StartCoroutine(ChangeHealth(foes.Count-1, -1, - (int)damageDealt, 0.48f));
                StartCoroutine(combatInterface.GetComponent<TextSpawner>().SpawnNewText(1, attacker, textPrinted + (int)damageDealt, 0.48f));
            }
        }
    }

    IEnumerator ChangeHealth (int whoLosesHealth, int whoDealsDamage, int changedAmount, float time)
    {
        yield return new WaitForSeconds(time);
        if (whoLosesHealth == -1 && foes.Count > whoDealsDamage && whoDealsDamage != -1)
        {
            if (gameManager.state == "dead")
            {
                combatInterface.GetComponent<UpdateHealth>().UpdateLife(-1, 0);
            }
            else
            {
                combatInterface.GetComponent<UpdateHealth>().UpdateLife(-1, ((float)changedAmount + you.GetComponent<Stats>().currentHealth)/you.GetComponent<Stats>().maxHealth);
                you.GetComponent<Stats>().currentHealth = (int)Mathf.Clamp(you.GetComponent<Stats>().currentHealth + changedAmount, 0, you.GetComponent<Stats>().maxHealth);
            }
        }
        else if (whoLosesHealth != -1 && foes.Count > 0 && gameManager.state != "dead")
        {
            combatInterface.GetComponent<UpdateHealth>().UpdateLife(foes.Count-1, ((float)changedAmount + foes[0].GetComponent<Stats>().currentHealth)/foes[0].GetComponent<Stats>().maxHealth);
            currentHealthFoes[0] = (int)Mathf.Clamp(changedAmount + foes[0].GetComponent<Stats>().currentHealth, 0, foes[0].GetComponent<Stats>().maxHealth);
            foes[0].GetComponent<Stats>().currentHealth = (int)Mathf.Clamp(foes[0].GetComponent<Stats>().currentHealth + changedAmount, 0, foes[0].GetComponent<Stats>().maxHealth);
        }

        if (currentHealthFoes[0] <= 0 || you.GetComponent<Stats>().currentHealth <=0)
        {
            if (foes.Count == 1)
            {
                combatEnded = true;
            }
            else if (you.GetComponent<Stats>().currentHealth <= 0)
            {
                combatEnded = true;
            }

            if (whoLosesHealth == -1)
            {
                StartCoroutine(Kill(you));
            }
            else
            {
                if (foes.Count > 0)
                {
                    StartCoroutine(Kill(foes[0]));
                }
            }
        }
    }

    IEnumerator Kill (GameObject receiver)
    {
        if (receiver == you || receiver.name.Contains("Oponent"))
        {
            GetComponent<Sounds>().PauseSound(11);
            GetComponent<Sounds>().PauseSound(12);
            GetComponent<Sounds>().PauseSound(14);
        }
        else if (receiver.name.Contains("Bandit"))
        {
            GetComponent<Sounds>().PauseSound(19);
        }
        else if (receiver.name.Contains("Condemned"))
        {
            GetComponent<Sounds>().PauseSound(20);
        }
        else if (receiver.name.Contains("Demon"))
        {
            GetComponent<Sounds>().PauseSound(21);
        }
        else if (receiver.name.Contains("Demon"))
        {
            GetComponent<Sounds>().PauseSound(21);
        }
        else if (receiver.name.Contains("Elf"))
        {
            GetComponent<Sounds>().PauseSound(22);
        }
        else if (receiver.name.Contains("Goblin"))
        {
            GetComponent<Sounds>().PauseSound(23);
        }
        else if (receiver.name.Contains("Imp"))
        {
            GetComponent<Sounds>().PauseSound(24);
        }
        else if (receiver.name.Contains("Rat"))
        {
            GetComponent<Sounds>().PauseSound(25);
        }
        else if (receiver.name.Contains("Skeleton"))
        {
            GetComponent<Sounds>().PauseSound(26);
        }
        else if (receiver.name.Contains("Spider"))
        {
            GetComponent<Sounds>().PauseSound(27);
        }
        else if (receiver.name.Contains("Troll"))
        {
            GetComponent<Sounds>().PauseSound(28);
        }
        else if (receiver.name.Contains("Wolf") || receiver.name.Contains("Firehound"))
        {
            GetComponent<Sounds>().PauseSound(29);
        }
        if (receiver == you && you.GetComponent<Stats>().currentHealth <= 0 && gameManager.state != "dead")
        {
            gameManager.state = "dead";
            foreach (ParticleSystem p in gameManager.shines.transform.GetComponentsInChildren<ParticleSystem>())
            {
                p.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            GetComponent<Sounds>().PlaySound(13);
            receiver.GetComponent<Animator>().Play("Die");
            yield return new WaitForSeconds(1f);
            StartCoroutine(gameManager.GoTo("left"));
            for (int i = 0; i < foes.Count; i++)
            {
                combatInterface.GetComponent<UpdateHealth>().differenceFoe[i].fillAmount = 0;
                combatInterface.GetComponent<UpdateHealth>().currentFoe[i].fillAmount = 0;
            }
            if (!foes[0].name.Contains("Oponent"))
            {
                you.GetComponent<ExperienceManager>().LooseExperience();
                GetComponent<Server>().TransferItemsToFirebase ();
            }
            username.enabled = false;
            oponentUsername.enabled = false;
            foes.Clear();
        }
        else if (currentHealthFoes[0] <= 0 && foes.Count > 0)
        {
            for (int i = 0; i < foes.Count-1; i++)
            {
                foes[i] = foes[i+1];
                currentHealthFoes[i] = currentHealthFoes[i+1];
            }
            foes.RemoveAt(foes.Count - 1);

            if (!receiver.name.Contains("Oponent"))
            {
                GetComponent<Sounds>().PlaySound(10);
                receiver.transform.GetChild(0).GetComponentInChildren<ParticleSystem>().Play();
                foreach (Renderer r in receiver.GetComponentsInChildren<SpriteRenderer>())
                {
                    r.enabled = false;
                }
            }
            else
            {
                receiver.GetComponent<Animator>().Play("Die");
            }
            yield return new WaitForSeconds(1f);

            if (foes.Count == 0)
            {
                if (!receiver.name.Contains("Oponent"))
                {
                    Dictionary <Item, int> aux = gameManager.GetComponent<Drops>().ChooseItems(gameManager.place, gameManager.stage, gameManager.level, you.GetComponent<ExperienceManager>().yourLevel);                    
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
                        rightOpener.transform.GetChild(0).GetComponent<Image>().sprite = rewardAvailable;
                        itemManager.currentSectionKind = ItemManager.SectionKind.rewards;
                    }
                    you.GetComponent<StatManager>().DisablePotionEffect();
                }
                gameManager.GoToMidPosition();
                gameManager.others.Clear();
                yield return new WaitForSeconds(gameManager.timeToMove);
                GetComponent<MapActions>().canDrag = true;
                foreach (Image i in leftOpener.GetComponentsInChildren<Image>())
                {
                    i.enabled = true;
                }
                foreach (Image i in rightOpener.GetComponentsInChildren<Image>())
                {
                    i.enabled = true;
                }

                if (gameManager.place == "arena")
                {
                    StartCoroutine(gameManager.GoTo("right"));
                    username.enabled = false;
                    oponentUsername.enabled = false;
                }
            }
            
            Destroy(receiver);
        }
    }

    IEnumerator PlayShieldAnimation (float time, GameObject receiver)
    {
        yield return new WaitForSeconds(time);
        GetComponent<Sounds>().PlaySound(14, 0.1f);
        receiver.GetComponent<Animator>().Play("Shield");
        if (receiver == you)
        {
            StartCoroutine(combatInterface.GetComponent<TextSpawner>().SpawnNewText(0, receiver, "Blocked", 0));
        }
        else
        {
            StartCoroutine(combatInterface.GetComponent<TextSpawner>().SpawnNewText(1, receiver, "Blocked", 0));
        }
    }
}