using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{

    public Dictionary<string, string> questDescs = new Dictionary<string, string>
    {
        { "Restorative Remedy", "Hello, I'm an archer from a newly formed party! I'm forming a party with 2 of my childhood friends! Our party consists of a vanguard, a mage, and an archer(me!). But recently i'm worried about my vanguard because he always acting tough while seemed hurt, he also refuses to invite a healer and tell me my mage's basic healing skill is enough, i know it isn't though because i can see it from his face, i'm the one who know him the best after all!~ so with that in mind, can you give me a potion that can heal him when he's hurt? Thank you!"},
        { "Mixture of Mana", "Good day to you, i'm a newbie mage that recently joined a guild, i'm currently the only mage in my party but sadly because both my not-talented-self and my lack of experience my mana always exhausted before our party can complete the mission, so i usually only fire my magic when it's really important, since i don't want to be their weight, can you make me a potion to enhance my mana capacity? wait that's impossible, how about restoring my mana instead? either is fine, thanks" },
        { "Salve of Strength", "Hi, i'm a new adventurer with a vanguard role, recently i feel like i'm not strong enough to protect both my partymates (one mage and one archer), i even my my archer super worried to the point where she actually bought a healing potion for me, i'm really embarrassed! Can you make me a potion that boosts my physical strength, so that she won't worry about me anymore. Thank you"},
        { "Teacher's Tonic", "Greetings, I am a wizard of the capital. These days I feel my memory slipping, causing me trouble in the middle of a battle when I needed to cast a spell that I forgot the incantation for, some spells even came out worse than I expected. I require your services for a potion that will increase my focus, something that will help with my memory problem, care to help an elder with their woes?"},
        { "Eclipse Elixir", "I'll go straight to the point, a new dungeon appeared near the neighboring city, the guild put up a quest for that dungeon's map making, to make it easy because i want to go there alone (so i can get the full prize for myself) and i don't want to waste my energy fighting monsters, can you make me a potion that enhance my agility? wait actually, can you make it turn me invisible instead? i think that would make it easier for me (also cooler too)" },
        { "Liquid Lie", "Salutations, little rosette, care to give some assistance? You see, I've met up with a wonderful dragon, we started off on the wrong foot, but with my charms, I've convinced it to not view me as an enemy. But oh! My current charms aren't strong enough to persuade her hand in marriage! Just imagine, a family of half dragons, strong as my fiance, and as charming as me! Oh please, little rosette, grant me a potion that will help me achieve that dream!"},
        { "Acidic Afterburn", "Hey there, I'm a local dungeon delver, and you see, me and my friends are stuck on a certain floor, we've been having trouble fighting some iron golems in the dungeon, our weapons just can't pierce its defenses, and our spells seem to bounce off of it!  Maybe you can brew something that can melt the iron off the golems and expose their cores?"},
        { "Combustive Concoction", "Oi, lemme get down to business, I'm hunting down a yeti, and I don't think my weapons will do anything against that thick fur. I've been out in the mountains for days on end tracking this thing down, I can't just back down now, and I ain't getting anywhere close to it, that thing can wreck me! Give me something that'll singe the fur off that thing."},
        { "Potion of Poison", "Hello, I require something that will help me 'take care' of a 'rat' problem. You see, this 'rat' has been all over the place, flinging its filthy tongue around, spreading a special plague that has been haunting me. I need you to brew me something, something that i can put into the 'cheese' that i have set up for this particular, cheeky, 'rat', understood?"},
        { "Paralyzing Philter", "Howdy, I'm a gunslingr from the western side of the kingdom, anticpatin' a quick battle tomorrow, facin' off 'gainst sum wild feral ocelots, shoulda started practicin' my aim yesterday, but oh well. Real wild them ocelots are, mauled ma brother a week ago, weren't nothin' I could do. Jus gimme sum'n to make 'em stay still won'tcha, darlin'?"},
        { "Draught of Dreams", "I'm going to hunt a dragon with my party tomorrow, it's pretty hard to fight head-to-head with it, and we have 2 mages + 1 swordsman that can kill the dragon within less than 10 seconds, so can you make me a potion that can lower the dragon's awareness so that we can do a surprise attack and kill it before it can fight?"},
        { "Blinding Brew", "I'm planning to raid a cyclops' den, but their number is too much and I can't think we can defeat them with the way they guard the den. Oh wait, I heard that they really rely on their eyes to fight, so maybe can you make me a potion to disrupt their vision? That way maybe we can clear the cyclops den effortlessly!"}
    };
    public static QuestManager Instance { get; private set; }

    [SerializeField] private AlchemyBook alchemyBook;
    private List<Quest> activeQuests = new List<Quest>();
    private const int MaxQuests = 3;

    public UnityEvent OnQuestListUpdated = new UnityEvent();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        //AddQuest("Paralyzing Philter");
        //AddQuest("Restorative Remedy");
        //AddQuest("Liquid Lie");
    }

    public void AddQuest(string potionName)
    {
        if (activeQuests.Count >= MaxQuests)
        {
            Debug.LogWarning("Cannot add more quests. Maximum limit reached.");
            return;
        }

        var potionDetails = alchemyBook.GetPotionDetails(potionName);
        if (potionDetails != null)
        {
            Debug.Log("added quest " + potionName);
            var quest = new Quest
            {
                PotionName = potionName,
                PotionImage = potionDetails.Image,
                Description = questDescs[potionName],
            };
            activeQuests.Add(quest);
            OnQuestListUpdated.Invoke();
        }
        else
        {
            Debug.LogError($"Potion '{potionName}' not found in the AlchemyBook!");
        }
    }

    public void RemoveQuest(int index)
    {

            if (index >= 0 && index < activeQuests.Count && activeQuests[index] != null)
            {
                activeQuests.RemoveAt(index);
                OnQuestListUpdated.Invoke();
            }
            else
            {
                //Debug.LogError("Invalid quest index.");
            }

    }

    public List<Quest> GetActiveQuests()
    {
        return new List<Quest>(activeQuests); // Return a copy to prevent modification
    }

    //public void deleteFirst()
    //{
    //    RemoveQuest(0);
    //    OnQuestListUpdated.Invoke();
    //}

    //public void addDummy()
    //{
    //    AddQuest("Potion D");
    //    OnQuestListUpdated.Invoke();
    //}

    public void ShowQuestByIndex(int index)
    {
        // Check if the index is valid
        if (index < 0 || index >= activeQuests.Count)
        {
            Debug.LogError($"Invalid quest index: {index}. Ensure it's within the range of active quests.");
            return;
        }

        // Open the AlchemyBook
        alchemyBook.OpenBook();

        // Open the Quest Page
        alchemyBook.OpenPage(alchemyBook.questPage);

        // Get the button corresponding to the n-th quest
        Transform questButtonTransform = alchemyBook.questPageList.GetChild(index);

        if (questButtonTransform != null)
        {
            Button questButton = questButtonTransform.GetComponent<Button>();
            if (questButton != null)
            {
                // Simulate a button click to activate the detailed view
                questButton.onClick.Invoke();
                //alchemyBook.SelectItem(questButton, questButtonTransform.GetComponentInChildren<Image>().sprite, questButtonTransform.GetComponentInChildren<Text>().text);
                //alchemyBook.AddOutline(questButton);
            }
            else
            {
                Debug.LogError("Quest button not found at the specified index.");
            }
        }
        else
        {
            Debug.LogError($"Quest button not found at index {index} in questList.");
        }
    }

    //public void openFirstQuest()
    //{
    //    ShowQuestByIndex(0);
    //}

    [System.Serializable]
    public class Quest
    {
        public string PotionName;
        public Sprite PotionImage;
        public string Description;
        public string Title;
    }
}
