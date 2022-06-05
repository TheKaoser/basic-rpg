using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading.Tasks;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System.Linq;
using UnityEngine.UI;

public class Server : MonoBehaviour
{
    string authCode = "";
    public string username = "";
    public List <Dictionary<string, object>> items = new List <Dictionary<string, object>>();
    public Dictionary<string, object> tradingItem = new Dictionary<string, object>();
    public Dictionary <Item, string> tradingItems = new Dictionary<Item, string>();
    public Item buyingItem;
    public bool itemsSynced;
    public bool itemsOponentSynced;

    public bool tradingItemsReady;

    public bool arenaActive;

    public ItemManager itemManager;
    public SystemsManager systemsManager;

    string arenaId = "";
    public string oponent;

    public GameObject oponentGameObject;

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

    public GameObject itemInList;

    public GameObject rightOpener;
    public Sprite rewardAvailable;

    public ParticleSystem swordParticles;

    public Transform modifiableItems;

    void Start()
    {   
        if (Application.platform == RuntimePlatform.Android) 
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false).Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success) => {
            if (success) 
            {
                string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);
                auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
                if (task.IsCanceled) {
                    Debug.LogError("SignInWithCredentialAsync was canceled.");
                    return;
                }
                if (task.IsFaulted) {
                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                    return;
                }

                Firebase.Auth.FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
                username = newUser.DisplayName;
                InitializeFirebase ();
                });
            }
            else 
            {
                Debug.LogError("Authentication failed");
            }});
        }
    }

    void InitializeFirebase ()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://basicrpg-89d89.firebaseio.com/");
        if (username == "")
        {
            return;
        }
        FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(username).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                return;
            }
            else if (task.IsCompleted)
            {
                if (!task.Result.HasChildren)
                {
                    Dictionary<string, object> d = new Dictionary<string, object>();
                    d ["name"] = "Health potion";
                    d ["kind"] = "1";
                    d ["number"] = "10";
                    items.Add(d);

                    d = new Dictionary<string, object>();
                    d ["name"] = "Traveler's sword";
                    d ["kind"] = "0";
                    d ["number"] = "1";
                    items.Add(d);

                    d = new Dictionary<string, object>();
                    d ["name"] = "Traveler's shield";
                    d ["kind"] = "0";
                    d ["number"] = "1";
                    items.Add(d);

                    d = new Dictionary<string, object>();
                    d ["name"] = "Fortune scroll";
                    d ["kind"] = "1";
                    d ["number"] = "1";
                    items.Add(d);

                    task.Result.Reference.SetValueAsync(items);
                    itemsSynced = true;
                }
                else
                {
                    foreach (DataSnapshot item in task.Result.Children)
                    {
                        Dictionary<string, object> d = new Dictionary<string, object>();
                        d ["name"] = item.Child("name").Value;
                        d ["kind"] = item.Child("kind").Value;
                        d ["number"] = item.Child("number").Value;
                        if (item.HasChild("rune"))
                        {
                            d ["rune"] = item.Child("rune").Value;
                        }
                        if (item.HasChild("effect"))
                        {
                            d ["effect"] = item.Child("effect").Value;
                        }
                        items.Add(d);
                    }

                    FirebaseDatabase.DefaultInstance.RootReference.Child("Arena").ValueChanged += ShowArenaActive;

                    FirebaseDatabase.DefaultInstance.RootReference.Child("Well").GetValueAsync().ContinueWith(task1 =>
                    {
                        if (task1.IsFaulted)
                        {
                            return;
                        }
                        else if (task1.IsCompleted)
                        {
                            foreach (DataSnapshot item in task1.Result.Children)
                            {
                                if ((string)item.Child("trader").Value == username)
                                {
                                    if (!item.HasChild("name"))
                                    {
                                        bool flag = false;
                                        foreach (Dictionary <string, object> d2 in items)
                                        {
                                            if (d2.ContainsValue("Trading token"))
                                            {
                                                d2["number"] = (int.Parse((string)d2["number"]) + int.Parse((string)item.Child("price").Value)).ToString();
                                                flag = true;
                                            }
                                        }
                                        if (!flag)
                                        {
                                            Dictionary <string, object> d2 = new Dictionary<string, object>();
                                            d2 ["name"] = "Trading token";
                                            d2 ["kind"] = "2";
                                            d2 ["number"] = (string)item.Child("price").Value;
                                            items.Add(d2);
                                        }
                                        item.Reference.RemoveValueAsync();
                                    }
                                }
                            }
                        UpdateFirebase();
                        itemsSynced = true; 
                        }
                    });
                }
            }
        });
    }

    public void UpdateFirebase()
    {
        if (username == "")
        {
            return;
        }
        FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(username).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                return;
            }
            else if (task.IsCompleted)
            {
                task.Result.Reference.SetValueAsync(items);
            }
        });
    } 

    public void UpdateWell()
    {
        if (tradingItems.Count > 0)
        {
            FirebaseDatabase.DefaultInstance.RootReference.Child("Well").Push().SetValueAsync(tradingItem);
        }
    }

    public void FetchTradingItems ()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("Well").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                return;
            }
            else if (task.IsCompleted)
            {
                foreach (Item item in tradingItems.Keys)
                {
                    Destroy(item);
                }
                tradingItems.Clear();
                foreach (DataSnapshot item in task.Result.Children)
                {
                    if (item.HasChild("name"))
                    {
                        Item itemAux = null;
                        string name = item.Child("name").Value.ToString();
                        if (int.Parse((string)item.Child("kind").Value) == 0 || int.Parse((string)item.Child("kind").Value) == 2 && name.Contains("rune"))
                        {
                            string auxName = name;
                            if (auxName.Contains("+"))
                            {
                                auxName = auxName.Substring(0, auxName.IndexOf('+') -1);
                            }
                            itemAux = GameObject.Instantiate(GetComponent<Drops>().allItems.Find(x => x.itemName == auxName));
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
                            if (item.HasChild("rune"))
                            {
                                itemAux.rune = new Vector2Int (int.Parse((string)item.Child("rune").Value.ToString().Split('.')[0]), int.Parse((string)item.Child("rune").Value.ToString().Split('.')[1]));
                            }
                            if (item.HasChild("effect"))
                            {
                                itemAux.effect = int.Parse((string)item.Child("effect").Value);
                            }

                            tradingItems.Add(itemAux, (string)item.Child("number").Value + " (" + (string)item.Child("price").Value + ")." + (string)item.Child("trader").Value);
                        }
                        else
                        {
                            tradingItems.Add(GameObject.Instantiate(GetComponent<Drops>().allItems.Find(x => x.itemName == name)), (string)item.Child("number").Value + " (" + (string)item.Child("price").Value + ")." + (string)item.Child("trader").Value);
                        }
                    }
                }

                tradingItemsReady = true;
            }
        });
    }

    public void TransferItemsToFirebase ()
    {
        if (Application.platform == RuntimePlatform.Android) 
        {
            items.Clear();

            foreach (Item i in itemManager.items.Keys)
            {
                Dictionary<string, object> d = new Dictionary<string, object>();
                d ["name"] = i.itemName;
                d ["number"] = itemManager.items[i] + "";
                if (i.kind == 0)
                {
                    d ["kind"] = 0 + "";
                    if (i.rune.y != 0)
                    {
                        d ["rune"] = i.rune.x + "." + i.rune.y;
                    }
                }
                else if (i.kind == 1)
                {
                    d ["kind"] = 1 + "";
                }
                else if (i.kind == 2)
                {
                    d ["kind"] = 2 + "";
                    if (i.type == "Rune")
                    {
                        d ["effect"] = (int)i.effect + "";
                    }
                }
                items.Add(d);
            }

            UpdateFirebase();
        }
    }

    public void TradeItems ()
    {
        if (Application.platform == RuntimePlatform.Android) 
        {
            tradingItem.Clear();
            tradingItem ["name"] = itemManager.tradingItem.itemName;
            tradingItem ["number"] = itemManager.items[itemManager.tradingItem] + "";
            tradingItem ["price"] = itemManager.price + "";
            tradingItem ["trader"] = username;
            if (itemManager.tradingItem.kind == 0)
            {
                tradingItem ["kind"] = 0 + "";
                if (itemManager.tradingItem.rune.y != 0)
                {
                    tradingItem ["rune"] = itemManager.tradingItem.rune.x + "." + itemManager.tradingItem.rune.y;
                }
            }
            else if (itemManager.tradingItem.kind == 1)
            {
                tradingItem ["kind"] = 1 + "";
            }
            else if (itemManager.tradingItem.kind == 2)
            {
                tradingItem ["kind"] = 2 + "";
                if (itemManager.tradingItem.type == "Rune")
                {
                    tradingItem ["effect"] = (int)itemManager.tradingItem.effect + "";
                }
            }

            itemManager.price = 0;
            
            UpdateWell();
            UpdateFirebase();
        }
    }

    public void SearchArena()
    {
        GetComponent<Arena>().readyForAction = false;
        arenaId = "";
        FirebaseDatabase.DefaultInstance.RootReference.Child("Arena").GetValueAsync().ContinueWith(tarea =>
        {
            if (tarea.IsFaulted)
            {
                return;
            }
            else if (tarea.IsCompleted)
            {
                Debug.Log("Searching...");
                foreach (DataSnapshot arenaInstance in tarea.Result.Children)
                {
                    if (arenaInstance.ChildrenCount < 3)
                    {
                        arenaId = arenaInstance.Key;
                        Debug.Log("Found game");
                        break;
                    }
                }

                if (arenaId == "")
                {
                    arenaId = FirebaseDatabase.DefaultInstance.RootReference.Child("Arena").Push().Key;

                    FirebaseDatabase.DefaultInstance.RootReference.Child("Arena").Child(arenaId).Child(username).SetValueAsync("");
                    FirebaseDatabase.DefaultInstance.RootReference.Child("Arena").Child(arenaId).Child("seed").SetValueAsync("");
                    FirebaseDatabase.DefaultInstance.RootReference.Child("Arena").Child(arenaId).Child("seed").ValueChanged += CollectSeed;
                    Debug.Log("Created game");
                }
                else
                {
                    FirebaseDatabase.DefaultInstance.RootReference.Child("Arena").Child(arenaId).RunTransaction(mutableData =>
                    {
                        Dictionary<string, object> d = mutableData.Value as Dictionary<string, object>;
                        if (d == null)
                        {
                            Debug.Log("Dictionary is null");
                        }
                        else if (mutableData.ChildrenCount == 3)
                        {
                            SearchArena();
                            Debug.Log("2 players already, leaving");
                            return TransactionResult.Abort();
                        }
                        else if (mutableData.ChildrenCount == 2)
                        {
                            GetComponent<Combat>().seed = Time.time % 200f;
                            GetComponent<Combat>().seedCreator = true;
                            d["seed"] = GetComponent<Combat>().seed;
                            d[username] = "";
                            foreach (string s in d.Keys)
                            {
                                if (s != "seed" && s != username)
                                {
                                    oponent = s;
                                }
                            }

                            Debug.Log(oponent);
                            GetComponent<Arena>().readyForAction = true;
                            
                            Debug.Log("Joined game, seed " + GetComponent<Combat>().seed);
                        }

                        mutableData.Value = d;

                        return TransactionResult.Success(mutableData);
                    });
                }
            }
        });
    }

    public void BuyItem ()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("Well").GetValueAsync().ContinueWith(task1 =>
        {
            if (task1.IsFaulted)
            {
                return;
            }
            else if (task1.IsCompleted)
            {
                foreach (DataSnapshot item in task1.Result.Children)
                {
                    if ((string)item.Child("name").Value == buyingItem.itemName && (string)item.Child("number").Value == tradingItems[buyingItem].Split(' ')[0] && 
                    (string)item.Child("price").Value == tradingItems[buyingItem].Split('(')[1].Split(')')[0] && (string)item.Child("trader").Value == tradingItems[buyingItem].Split('.')[1])
                    {
                        Dictionary<string, object> d1 = new Dictionary<string, object>();
                        d1.Add ("price", (string)item.Child("price").Value);
                        d1.Add ("trader", (string)item.Child("trader").Value);
                        FirebaseDatabase.DefaultInstance.RootReference.Child("Well").Child(item.Key).SetValueAsync(d1).ContinueWith(task2 =>
                        {
                            if (task2.IsFaulted)
                            {
                                return;
                            }
                            else if (task2.IsCompleted)
                            {
                                itemManager.newItems.Clear();
                                if (buyingItem.kind == 0 || buyingItem.type == "Rune")
                                {
                                    itemManager.AddItem(GameObject.Instantiate(buyingItem, modifiableItems), 1);
                                }
                                else
                                {
                                    itemManager.AddItem(GetComponent<Drops>().allItems.Find(x => x.itemName == buyingItem.itemName), int.Parse(tradingItems[buyingItem].Split(' ')[0]));
                                }
                                rightOpener.transform.GetChild(0).GetComponent<Image>().sprite = rewardAvailable;
                                itemManager.currentSectionKind = ItemManager.SectionKind.rewards;
                                itemManager.UpdateItems(true);
                                TransferItemsToFirebase();
                                buyingItem = null;
                            }
                        });
                    }
                }
            }
        });
    }

    void CollectSeed(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            return;
        }

        if (GetComponent<Arena>().readyForAction || args.Snapshot.Value == null || args.Snapshot.Value.ToString() == "")
        {
            return;
        }

        GetComponent<Combat>().seed = float.Parse(args.Snapshot.Value.ToString());
        GetComponent<Combat>().seedCreator = false;
        Debug.Log("Seed received " + GetComponent<Combat>().seed + ", starting match");
        
        FirebaseDatabase.DefaultInstance.RootReference.Child("Arena").Child(arenaId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                return;
            }
            else if (task.IsCompleted)
            {
                foreach (DataSnapshot d in task.Result.Children)
                {
                    if (d.Key != "seed" && d.Key != username)
                    {
                        oponent = d.Key;
                        Debug.Log(oponent);
                        GetComponent<Arena>().readyForAction = true;
                        FirebaseDatabase.DefaultInstance.RootReference.Child("Arena").Child(arenaId).Child("seed").ValueChanged -= CollectSeed;
                        DeleteArenaId();
                        return;
                    }
                }
            }
        });
    }

    void ShowArenaActive(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            return;
        }

        if (args.Snapshot.Value != null) // && arena is not active
        {
            arenaActive = true;
        }
        else
        {
            arenaActive = false;
        }
    }

    public void DeleteArenaId()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (arenaId != "")
            {
                FirebaseDatabase.DefaultInstance.RootReference.Child("Arena").Child(arenaId).RemoveValueAsync();
            }
        }
    }

    public void SpawnOponent()
    {
        if (GetComponent<GameManager>().others.Count == 1)
        {
            return;
        }
        
        GameObject op = GameObject.Instantiate (oponentGameObject, oponentGameObject.transform.position, Quaternion.Euler(new Vector3(0, 180, 0)));
        GetComponent<GameManager>().others.Add(op);
        FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(oponent).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                return;
            }
            else if (task.IsCompleted)
            {
                foreach (DataSnapshot item in task.Result.Children)
                {
                    if ((string)item.Child("number").Value == "0")
                    {                        
                        string name = item.Child("name").Value.ToString();
                        string auxName = item.Child("name").Value.ToString();
                        if (auxName.Contains("+"))
                        {
                            auxName = auxName.Substring(0, auxName.IndexOf('+') -1);
                        }
                        Item itemAux = GetComponent<Drops>().allItems.Find(x => x.itemName == auxName);
                        
                        if (name.Contains("sword"))
                        {
                            if (name.Contains("+"))
                            {
                                op.GetComponent<Stats>().damage = itemAux.effect + 20 * int.Parse(name.Split('+')[1]) + 5;
                            }
                            else 
                            {
                                op.GetComponent<Stats>().damage = itemAux.effect + 5;
                            }
                        }
                        else
                        {
                            if (name.Contains("+"))
                            {
                                op.GetComponent<Stats>().defense += itemAux.effect + 4 * int.Parse(name.Split('+')[1]);
                            }
                            else
                            {
                                op.GetComponent<Stats>().defense += itemAux.effect;
                            }
                        }

                        bool isSword = false;
                        int childNumber = 0;
                        if (name.Contains("helmet"))
                        {
                            childNumber = 1;
                        }
                        else if (name.Contains("chest"))
                        {
                            childNumber = 2;
                        }
                        else if (name.Contains("gloves"))
                        {
                            childNumber = 3;
                        }
                        else if (name.Contains("shield"))
                        {
                            childNumber = 4;
                        }
                        else if (name.Contains("greaves"))
                        {
                            childNumber = 5;
                        }
                        else if (name.Contains("boots"))
                        {
                            childNumber = 6;
                        }
                        else
                        {
                            isSword = true;
                        }

                        if (name.Contains("Explorer's"))
                        {
                            childNumber += 7;
                        }
                        else if (name.Contains("Soldier's"))
                        {
                            childNumber += 14;
                        }
                        else if (name.Contains("Master's"))
                        {
                            childNumber += 21;
                        }
                        else if (name.Contains("Assassin's"))
                        {
                            childNumber += 28;
                        }

                        op.transform.GetChild(childNumber).GetComponent<SpriteRenderer>().enabled = true;

                        if (name.Contains("+"))
                        {
                            int upgrade = int.Parse(name.Split('+')[1]);
                            ParticleSystem particles = null;

                            if (upgrade == 1)
                            {
                                particles = GameObject.Instantiate(level0, op.transform);
                            }
                            else if (upgrade == 2)
                            {
                                particles = GameObject.Instantiate(level1, op.transform);
                            }
                            else if (upgrade == 3)
                            {
                                particles = GameObject.Instantiate(level2, op.transform);
                            }
                            else if (upgrade == 4)
                            {
                                particles = GameObject.Instantiate(level3, op.transform);
                            }
                            else if (upgrade == 5)
                            {
                                particles = GameObject.Instantiate(level4, op.transform);
                            }
                            else if (upgrade == 6)
                            {
                                particles = GameObject.Instantiate(level5, op.transform);
                            }
                            else if (upgrade == 7)
                            {
                                particles = GameObject.Instantiate(level6, op.transform);
                            }
                            else if (upgrade == 8)
                            {
                                particles = GameObject.Instantiate(level7, op.transform);
                            }
                            else if (upgrade == 9)
                            {
                                particles = GameObject.Instantiate(level8, op.transform);
                            }
                            else if (upgrade == 10)
                            {
                                particles = GameObject.Instantiate(level9, op.transform);
                            }

                            var shape1 = particles.shape;
                            var shape2 = particles.transform.GetChild(0).GetComponent<ParticleSystem>().shape;
                            shape1.spriteRenderer = op.transform.GetChild(childNumber).GetComponent<SpriteRenderer>();
                            shape2.spriteRenderer = op.transform.GetChild(childNumber).GetComponent<SpriteRenderer>();
                            if (isSword)
                            {
                                swordParticles = particles;
                            }
                        }

                        if (item.HasChild("rune"))
                        {
                            if (item.Child("rune").Value.ToString().Split('.')[0] == "0")
                            {
                                op.GetComponent<Stats>().weakening += int.Parse(item.Child("rune").Value.ToString().Split('.')[1]);
                            }
                            else if (item.Child("rune").Value.ToString().Split('.')[0] == "1")
                            {
                                op.GetComponent<Stats>().aura += int.Parse(item.Child("rune").Value.ToString().Split('.')[1]);
                            }
                            else if (item.Child("rune").Value.ToString().Split('.')[0] == "2")
                            {
                                op.GetComponent<Stats>().critical += int.Parse(item.Child("rune").Value.ToString().Split('.')[1]);
                            }
                            else if (item.Child("rune").Value.ToString().Split('.')[0] == "3")
                            {
                                op.GetComponent<Stats>().shield += int.Parse(item.Child("rune").Value.ToString().Split('.')[1]);
                            }
                            else if (item.Child("rune").Value.ToString().Split('.')[0] == "4")
                            {
                                op.GetComponent<Stats>().agility += int.Parse(item.Child("rune").Value.ToString().Split('.')[1]);
                            }
                            else if (item.Child("rune").Value.ToString().Split('.')[0] == "5")
                            {
                                op.GetComponent<Stats>().overload += int.Parse(item.Child("rune").Value.ToString().Split('.')[1]);
                            }
                            else if (item.Child("rune").Value.ToString().Split('.')[0] == "6")
                            {
                                op.GetComponent<Stats>().endurance += int.Parse(item.Child("rune").Value.ToString().Split('.')[1]);
                            }
                            else if (item.Child("rune").Value.ToString().Split('.')[0] == "7")
                            {
                                op.GetComponent<Stats>().divine += int.Parse(item.Child("rune").Value.ToString().Split('.')[1]);
                            }
                            else if (item.Child("rune").Value.ToString().Split('.')[0] == "8")
                            {
                                op.GetComponent<Stats>().regeneration += int.Parse(item.Child("rune").Value.ToString().Split('.')[1]);
                            }
                            else if (item.Child("rune").Value.ToString().Split('.')[0] == "9")
                            {
                                op.GetComponent<Stats>().drain += int.Parse(item.Child("rune").Value.ToString().Split('.')[1]);
                            }
                        }
                    }
                    else if ((string)item.Child("name").Value == "Experience")
                    {
                        int experienceRequiredToLevelUp = 10;
                        int oponentLevel = 1;
                        while (int.Parse(item.Child("number").Value.ToString()) >= experienceRequiredToLevelUp)
                        {
                            oponentLevel += 1;
                            op.GetComponent<Stats>().maxHealth += 50;
                            op.GetComponent<Stats>().currentHealth = (int)op.GetComponent<Stats>().maxHealth;
                            experienceRequiredToLevelUp += 10 * oponentLevel;
                        }
                    }
                }
                itemsOponentSynced = true;
                Debug.Log (op.GetComponent<Stats>().damage + " " + op.GetComponent<Stats>().defense + " " + op.GetComponent<Stats>().maxHealth);
            }
        });
    }

    void OnApplicationPause()
    {
        DeleteArenaId();
    }
}
