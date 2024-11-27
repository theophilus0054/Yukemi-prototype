using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AlchemyBook : MonoBehaviour
{
    public GameObject questPage;        // Reference to the QuestPage GameObject
    public GameObject questPrefab;      // Reference to the QuestPrefab GameObject (the individual quest item)
    public Transform questPageList;         // Parent transform for listing quests (e.g., a container where quests will go)
    public GameObject detailedRecipe;         // Reference to the DetailedRecipe GameObject

    private Button selectedButton;     // Tracks the selected ingredient/potion
    private Button hoveredButton;      // New field to track currently hovered button

    // References to pages
    public GameObject ingredientsPage; // Left page for ingredients
    public GameObject potionsPage;     // Left page for potions
    public GameObject detailedView;    // Right page for details

    // Book background and bookmarks
    public GameObject bookBackground;  // The book's background
    public GameObject bookmarksPanel;  // The panel containing the bookmarks

    // Buttons for bookmarks
    public Button ingredientsBookmark;
    public Button potionsBookmark;
    public Button questBookmark;

    // Button to reopen the book
    public Button openBookButton;

    // Grid for ingredients and potions
    public Transform ingredientsList;
    public Transform potionsList;

    // Prefab for list items
    public GameObject listItemPrefab;

    // Detailed view (right page)
    public Image detailedImage;
    public Text detailedDescription;
    public Text detailedTitle;

    // Colors for highlighting
    public Color normalColor = Color.white;
    public Color highlightColor = Color.green;

    private GameObject currentPage;    // Tracks the active page

    // Book background image
    public Sprite ingredientsBookSprite; // Background sprite for Ingredients page
    public Sprite potionsBookSprite;     // Background sprite for Potions page
    public Sprite questBookSprite;      // Background sprite for Quest page

    // Ingredients sprites
    public Sprite PurityBlossom;
    public Sprite SouthernStarSilt;
    public Sprite ForbiddenFruit;
    public Sprite DragonBlood;
    public Sprite FaerieDust;
    public Sprite FragmentOfNight;

    // Potions sprites
    public Sprite RestorativeRemedy;
    public Sprite MixtureOfMana;
    public Sprite SalveOfStrength;
    public Sprite TeachersTonic;
    public Sprite EclipseElixir;
    public Sprite LiquidLie;
    public Sprite AcidicAfterburn;
    public Sprite CombustiveConcoction;
    public Sprite PotionOfPoison;
    public Sprite ParalyzingPhilter;
    public Sprite DraughtOfDreams;
    public Sprite BlindingBrew;

    // Close overlay
    public Button closeOverlayButton; // The full-screen transparent button

    // Declare as class-level variables (fields)
    public string[] ingredientNames = { 
        "Purity Blossom", 
        "Southern Star Silt", 
        "Forbidden Fruit",
        "Dragon's Blood", 
        "Faerie Dust", 
        "Fragment of Night" 
    };

    public string[][] potionsRecipe =
    {
            new string[] { "Purity Blossom", "Dragon's Blood"},
            new string[] { "Southern Star Silt", "Faerie Dust"},
            new string[] { "Dragon's Blood", "Southern Star Silt"},
            new string[] { "Southern Star Silt", "Purity Blossom"},
            new string[] { "Purity Blossom", "Fragment of Night"},
            new string[] { "Faerie Dust", "Forbidden Fruit"},
            new string[] { "Forbidden Fruit", "Fragment of Night"},
            new string[] { "Forbidden Fruit", "Dragon's Blood"},
            new string[] { "Forbidden Fruit", "Purity Blossom"},
            new string[] { "Faerie Dust", "Dragon's Blood"},
            new string[] { "Southern Star Silt", "Fragment of Night"},
            new string[] { "Faerie Dust", "Fragment of Night" }
        };

    public string[] ingredientDescriptions = {
        "The purity blossom grows not too deep in the forest. It's a quite common ingredient used for basic potions such as a healing potion, I discovered that, after countless experiments, this is due to the effect of the purity blossom when combined with other ingredients. The purity blossom attempts to either cleanse the ingredient of side effects, or enhance its positive effects to a degree, how fascinating! Definitely a staple for any apothecaries or alchemists. However, not many know that this specific blossom can also be turned into a mixture that’s especially dangerous when combined with other ingredients. I found that out the hard way, I can't forget the face my friend made as he handed me the antidote he made for me, that was so embarrassing!\r\n",
        "At the far south of the kingdom, there exists a small village near a lake. When the night comes, little motes of various sizes from the southern star break apart, dusting the beach in a beautiful glittering glaze. The beach looks glamorous as the stardust lay upon them, making it an obvious spot for sightseeing. Despite the surreal scenery it gives, no one comes there often since it takes more than a couple weeks to reach there from the main capital. I would know, considering how many rations I used up to travel there, but it was definitely worth it! Definitely a location every person should visit at least once in their lives. The size of the chunks that fall off from the Southern Star vary, yet most are reduced to dust as it falls to the ground, very rarely are people able to find a relatively intact fragment. I was lucky to find a few small intact pieces on the shore, might as well make the most of it, right? When used in alchemy, it seems to mimic the arcane properties of the stars, believed to be the origin of our magic.\r\n",
        "Fruit that comes from the \"Yggdrasil\" that recently grew near the kingdom. Legend has it that Yggdrasil is a tree of life, so why is its fruit labeled as \"forbidden\"? Nobody truly knows. Rumors tell of a man who ate it, rambling like a mad man the next day, until one day he was never seen again. What is obvious, is that the effect of this fruit when combined with other ingredients tends to cause a negative result, which is perfect for brewing weaponized chemicals into potions. Despite the obvious dangers of consuming this fruit, it is very commonly found near plains in harvest season, the season when Yggdrasil bears many fruits, dropping them to the ground due to wind or gravity, so finding some for my tests is not a problem. Though, I need to be extra careful when using this as an ingredient in making any potions, lest the capital's warnings advance to something far worse.\r\n",
        "Blood from an infamous legendary creature, the dragon. They usually live alone in their lair while protecting their treasure, the treasure that they usually hoard from the villages and towns that they have destroyed. Many skilled adventurers try to hunt it because not only can you sell the body part, you can also loot their treasures, killing two birds with one stone! (though many people died when fighting them) Regardless, I was able to acquire some from my commission request, while that did almost empty my coin purse, it opened up many more opportunities for combinations, so exciting!  So far, I have discovered the effects of the dragon’s blood mimics the traits of dragons themselves, like their overflowing vigor, and their destructive power.\r\n",
        "As a mythical creature that barely appears, faeries have their own self-protecting system, which is to unleash a dust that can put their enemies to sleep or charm them when it's necessary, that's why it is considered as one of the safest ingredients to obtain, or is it? Some say the faeries are evil, some say they mean no harm, some even worship them! Though those few tend to act a tad bit strange, I’ve seen a seemingly normal adventurer, whom after taking my request to gather faerie dust, seem to have completely changed their personality, and weirdly enough, they didn’t seem to respond to their own name, correcting me with an unfamiliar name instead, i wonder what that’s about? Anyways, after some testing, faerie dust seems to invert the effects of most ingredients that are mixed into it.\r\n",
        "The rarest ingredients to ever exist, not everyone can get it, in fact, some people don't even know it exists. Those who obtained it claim that they saw the sky splitting apart, breaking into millions of pieces and leaving an empty void where the pieces once were, as if the rapture had started. Rumor has it the side effect of obtaining this ingredient is hallucination, because how can a sky that you can't even touch, split into a void? Coincidentally, I seem to have found it somehow after safely conducting an experiment of ingesting Yggdrasil’s fruit with many bodyguards, and doctors around just in case something bad happens. I only remember walking through what looked to be empty space, only occupied by distant stars, before the space in front of me broke into fragments, floating into my hand. Needless to say, that was quite an odd, and slightly dreadful experience. After I woke up, I seemed to still be holding it somehow, surprising the people around me. Later, I stored it in a container for later use. Only a tiny part needs to be broken off for its effects to work, which I discovered to have the mysterious properties of the night when combined with other ingredients.\r\n"
    };

    public string[] potionNames = {
        "Restorative Remedy", 
        "Mixture of Mana", 
        "Salve of Strength", 
        "Teacher's Tonic", 
        "Eclipse Elixir", 
        "Liquid Lie",
        "Acidic Afterburn", 
        "Combustive Concoction", 
        "Potion of Poison", 
        "Paralyzing Philter", 
        "Draught of Dreams", 
        "Blinding Brew"
    };
    
    public string[] potionDescriptions = {
        "I have discovered a way of enhancing the restorative effects of the purity blossom! With a little dragon blood filled with paramount vigor, a simple healing potion turns into what seems to be a cure-all remedy. Through my testing, it seems this potion, in addition to healing superficial wounds, cures common sickness and internal damage! Though, the cost of buying or requesting dragon’s blood is making coin purse feel lighter.\r\n",
        "A wizard friend of mine came to me to commission this one, he always found himself exhausting his mana supply after a short adventure. This potion fixes that problem, with a sprinkle of southern star silt and a pinch of faerie dust, taking care not to add in too much to invert the effects, I have made a potion that will restore one’s mana! Definitely a staple to have for wizards, or anyone that uses mana as their medium.\r\n",
        "I made this one as a gift for my friend who helped hunt a dragon. She was a bit skeptical at first, but the reaction she made afterwards definitely convinced her it was nothing to worry about. Combining the dragon’s blood she acquired for me with some southern star silt, I have concocted a salve that, when rubbed onto the body, magically enhances one’s strength temporarily! This salve is nothing to scoff at, I once used it myself to carry a lost cart full of my supplies a few meters from my abode, by the time I was done, I had a few minutes left before the effects wore off.\r\n\r\n",
        "Sometimes I feel as if I can't think of any mixtures for my projects, until I came up with the bright idea of concocting a potion with purity blossom and southern star silt. Combining the magical effects of southern star silt and the clarity one gets from purity blossoms, I have made a potion to help clear my mind of intruding thoughts, granting me intense focus in whatever activities I am doing, including brainstorming ideas for recipes! It was only after one of the teachers of a nearby school noticed me using this potion, and requesting to buy it, that I gave it the name and sold it to the public. Apparently, this potion also helps wizards, and other spellcasters, remember their spells and increase their focus to boost their spell power, I wouldn’t know because I don’t dabble in magic.\r\n",
        "I got tired of having to run from any monsters that tried to attack me every time I went on a supply run! And so, I have concocted a potion that when I pour onto myself, conceals my body, along with my possessions, how neat is that? With some purity blossom to concentrate the concealing effect of the fragments of night, I have solved that problem! Strangely enough, some shady looking people came to my little shop to buy some. It was probably a bad idea to give it to them, but I needed the money to fund my research.\r\n",
        "A noble visited my shop one day, said he needed a little help convincing the masses, something about raising taxes? I didn’t really want to do it, but the amount of money he offered me was enough to convince me, I could buy my supplies for a year with that amount of coin! So, with a heavy heart, I made a potion for him, one that will give one a silver tongue to help him persuade the masses. With some faerie dust, I have managed to invert the negative effect of the forbidden fruit, a fruit known to deceive their consumer, into a potion that helps the user itself deceive other people. Just holding it feels illegal, but I need that money.\r\n",
        "A customer came requesting me for a potion containing an acid strong enough to dissolve metals, and said her party needed it to fight possessed armor. I’ve never worked with such dangerous chemicals till now, so I had to wear extra protection, just in case. With a hefty amount of forbidden fruit, and a small shard of night, I have combined the acidic properties of the forbidden fruit, with the dangers of the night, to brew a very potent acid, strong enough to ignore practically most defenses, be it chitin, shell, or metals. I have not found a proper logical reason why this is so, but my theory is that the night seems to accelerate the corrosive effect of acid, how interesting.\r\n",
        "A group of  adventurers complained to me that they had to clear an area of treants in a dungeon. They said they wanted something that can burn down an acre of forest, hah, and they didn’t want to hire a wizard to do it, apparently it’s because they had a bad experience with them, I’m not one to pry. And so, with a swig of dragon’s blood containing its destructive power, combined with the chaotic properties of forbidden fruit, I have made a potion that allows a simple man to wield the power of a dragon's flames within the palm of their hand! Though, I worry what someone with ill intentions would do with the potion.\r\n",
        "Some cloaked stranger came to my shop one day, he seemed very suspicious, and his request didn’t help with that. Said he wanted a poison strong enough to take down ten dwarves, known for their strong livers. I of course gave him the look, but I immediately changed my mind when he suddenly appeared right in front of me, dagger right on my neck. With haste, I whip up a potion with more forbidden fruit than purity blossom, just enough to concentrate the poisonous properties of the forbidden fruit, and isolate the other properties, making what I assume to be the most potent poison in the kingdom according to my theories. I wasn’t proud of that one, but weirdly enough, some people actually bought them.\r\n",
        "Some novice adventurers requested my help in making something to help with fighting a pack of direwolves, and said they were afraid that the direwolves would be too fast for them to handle. I nodded and thought of a potion that, when thrown, creates a cloud that stops any creatures in its tracks. With some faerie dust, I managed to reverse the effect of dragon’s blood, which instead of providing strength and vigor, saps it from the victims, leaving them too weak to move their own body. I warned them to not be in the proximity of the cloud when they throw it, and judging by their long absence, I can only assume the worst.\r\n",
        "Sometimes at night I have an idea for a potion, but my body feels too exhausted to do anything. The thought itself keeps me up at night, I can’t sleep when such a good idea strikes me! And so I came up with a potion to help with my bad sleeping habits. A pinch of southern star silt, combined with fragments of night results in a concoction that embodies the night itself, specifically targeting one’s circadian rhythm. This potion should put anything that requires sleep to be forced into a deep slumber whether they want it or not, only able to be awoken by experiencing pain. Always had a good night’s sleep after that, and hey, other people seem to be experiencing my problem too, so more profits!",
        "Some heavily cladded adventurers came one day, they looked like veteran mercenaries, they wanted me to make something to disable a basilisk’s deadly gaze. According to my theory, blinding them should disable their ability, and so I went straight to my cauldron. With a sprinkle of faerie dust, I have managed to infuse the mischievous properties of the fae into the fragment of night, disabling one’s sight, letting them only see a black void. The veterans gave me a pat on the back and some gold. I was happy I saw them the next day.\r\n"
    };

    // Declare the arrays without initializing them yet
    public Sprite[] ingredientImages;
    public Sprite[] potionImages;

    void Awake()
    {
        // Initialize the arrays in Awake or Start
        ingredientImages = new Sprite[] { PurityBlossom, 
                                            SouthernStarSilt, 
                                            ForbiddenFruit, 
                                            DragonBlood, 
                                            FaerieDust, 
                                            FragmentOfNight };

        potionImages = new Sprite[] { RestorativeRemedy, 
                                        MixtureOfMana, 
                                        SalveOfStrength, 
                                        TeachersTonic, 
                                        EclipseElixir, 
                                        LiquidLie, 
                                        AcidicAfterburn, 
                                        CombustiveConcoction, 
                                        PotionOfPoison, 
                                        ParalyzingPhilter, 
                                        DraughtOfDreams, 
                                        BlindingBrew };

}
    void Start()
    {
        // Initially hide the book and bookmarks
        bookBackground.SetActive(false);
        bookmarksPanel.SetActive(false);
        detailedRecipe.SetActive(false);

        // Hide all pages
        ingredientsPage.SetActive(false);
        potionsPage.SetActive(false);
        questPage.SetActive(false);

        // Hide the DetailedView
        detailedView.SetActive(false);

        closeOverlayButton.gameObject.SetActive(false); // Hide overlay initially

        ingredientsBookmark.onClick.AddListener(() => OpenPage(ingredientsPage));
        potionsBookmark.onClick.AddListener(() => OpenPage(potionsPage));
        questBookmark.onClick.AddListener(() =>OpenPage(questPage));

        // Populate grids
        PopulateGrid(ingredientsList, ingredientNames, ingredientImages, ingredientDescriptions);
        PopulateGrid(potionsList, potionNames, potionImages, potionDescriptions, true);

        closeOverlayButton.onClick.AddListener(CloseBook);

        openBookButton.onClick.AddListener(OpenBook);
    }

    public void OpenPage(GameObject page)
    {
        // Disable all pages
        ingredientsPage.SetActive(false);
        potionsPage.SetActive(false);
        questPage.SetActive(false);

        // Enable the selected page
        page.SetActive(true);

        // Update current page reference
        currentPage = page;

        ingredientsBookmark.gameObject.SetActive(true);
        potionsBookmark.gameObject.SetActive(true);
        questBookmark.gameObject.SetActive(true);

        if (currentPage == questPage)
        {
            currentPage.GetComponent<Image>().sprite = questBookSprite;
            detailedRecipe.SetActive(false);
            questBookmark.gameObject.SetActive(false);
        } 
        else if (currentPage == potionsPage)
        {
            currentPage.GetComponent<Image>().sprite = potionsBookSprite;
            potionsBookmark.gameObject.SetActive(false);
        }
        else if (currentPage == ingredientsPage)
        {
            currentPage.GetComponent<Image>().sprite = ingredientsBookSprite;
            detailedRecipe.SetActive(false);
            ingredientsBookmark.gameObject.SetActive(false);
        }

        // Clear the detailed view
        ClearDetailedView();
    }

    public void CloseBook()
    {
        // Hide the book background and bookmarks
        bookBackground.SetActive(false);
        bookmarksPanel.SetActive(false);
        detailedRecipe.SetActive(false);

        // Hide the DetailedView
        detailedView.SetActive(false);
        closeOverlayButton.gameObject.SetActive(false); // Hide overlay\
        openBookButton.onClick.RemoveAllListeners();
        openBookButton.onClick.AddListener(OpenBook);
    }

    public void OpenBook()
    {
        // Show the book background and bookmarks
        //Debug.Log("Book opened");
        bookBackground.SetActive(true);
        bookmarksPanel.SetActive(true);
        closeOverlayButton.gameObject.SetActive(true); // Show overlay
        detailedView.SetActive(false);

        potionsBookmark.gameObject.SetActive(true);
        questBookmark.gameObject.SetActive(true);

        // Open the default page (e.g., Ingredients)
        OpenPage(ingredientsPage);
        ingredientsBookmark.gameObject.SetActive(false);
        UpdateQuestPage();
        openBookButton.onClick.RemoveAllListeners();
        openBookButton.onClick.AddListener(CloseBook);
    }

    public void SelectItem(Button itemButton, Sprite image, string description, string title = null, string ing1 = null, string ing2 = null)
    {

        // Clean up all outlines first
        CleanupAllOutlines();

        // Update selected button and add its outline
        selectedButton = itemButton;
        AddOutline(selectedButton);

        // Enable detailed view if it's not already visible
        detailedView.SetActive(true);

        if (title != null)
        {
            detailedTitle.text = title;
        } else
        {
            detailedTitle.text = itemButton.GetComponentInChildren<Text>().text;
        }

        // Update the detailed view
        if (detailedImage != null && detailedDescription != null)
        {
            detailedImage.sprite = image;
            detailedDescription.text = description;
            detailedImage.color = Color.white;
            detailedImage.preserveAspect = true;
        }
        else
        {
            detailedImage.color = Color.clear;
            Debug.LogError("DetailedView components are not assigned in the Inspector!");
        }

        if (ing1 != null && ing2 != null)
        {
            int id1 = System.Array.IndexOf(ingredientNames, ing1);
            int id2 = System.Array.IndexOf(ingredientNames, ing2);
            //Debug.Log("ing 1: " + ing1);
            //Debug.Log("ing 2: " + ing2);
            //for (int i = 0; i < ingredientNames.Length; i++)
            //{
            //    Debug.Log(ingredientNames[i]);
            //}
            if (id1 != -1 && id2 != -1)
            {
                detailedRecipe.SetActive(true);
                detailedRecipe.transform.GetChild(0).GetComponent<Image>().sprite = ingredientImages[id1];
                detailedRecipe.transform.GetChild(2).GetComponent<Image>().sprite = ingredientImages[id2];

            }
            else
            {
                Debug.LogError("Ingredients not found in the list!");
            }
        }

        // Update the description
        detailedDescription.text = !string.IsNullOrEmpty(description) ? description : "No description available";
    }

    public void CleanupAllOutlines()
    {
        detailedRecipe.SetActive(false);
        // Find all buttons in the ingredients and potions lists
        Button[] allButtons = new Button[0];
        if (ingredientsList != null)
            allButtons = ingredientsList.GetComponentsInChildren<Button>();
        if (potionsList != null)
        {
            Button[] potionButtons = potionsList.GetComponentsInChildren<Button>();
            System.Array.Resize(ref allButtons, allButtons.Length + potionButtons.Length);
            potionButtons.CopyTo(allButtons, allButtons.Length - potionButtons.Length);
        }
        if (questPageList != null)
        {
            Button[] questButtons = questPageList.GetComponentsInChildren<Button>();
            System.Array.Resize(ref allButtons, allButtons.Length + questButtons.Length);
            questButtons.CopyTo(allButtons, allButtons.Length - questButtons.Length);
        }

        // Remove outlines from all buttons
        foreach (Button button in allButtons)
        {
            if (button != null)
            {
                Outline outline = button.GetComponentInChildren<Image>().GetComponent<Outline>();
                if (outline != null)
                {
                    Destroy(outline);
                }
            }
        }
    }

    private void ClearDetailedView()
    {
        detailedImage.sprite = null;
        detailedDescription.text = "";
        detailedTitle.text = "";
    }

    public void PopulateGrid(Transform gridParent, string[] names, Sprite[] images, string[] descriptions, bool potionPopulator = false)
    {
        if (gridParent == null)
        {
            Debug.LogError("Grid parent is null! Assign the correct Transform in the Inspector.");
            return;
        }
        if (listItemPrefab == null)
        {
            Debug.LogError("ListItemPrefab is null! Assign a valid prefab in the Inspector.");
            return;
        }
        if (names == null || images == null || descriptions == null)
        {
            Debug.LogError("Names, images, or descriptions are null! Check the input arrays.");
            return;
        }
        if (names.Length != images.Length || names.Length != descriptions.Length)
        {
            Debug.LogError("Mismatch in lengths of names, images, or descriptions arrays.");
            return;
        }

        // Clear existing items
        //foreach (Transform child in gridParent)
        //{
        //    Destroy(child.gameObject);
        //}

        // Populate grid
        for (int i = 0; i < names.Length; i++)
        {
            // Instantiate the prefab
            GameObject listItem = Instantiate(listItemPrefab, gridParent);
            listItem.SetActive(true); // Activate the prefab itself
            //foreach (Transform child in listItem.transform)
            //{
            //    child.gameObject.SetActive(true); // Ensure child elements are active
            //}


            // Set the name in the Text component
            Text itemText = listItem.GetComponentInChildren<Text>();
            if (itemText != null)
            {
                //itemText.enabled = true;
                itemText.text = names[i];
            }
            else
            {
                Debug.LogError("ListItemPrefab is missing a Text component!");
            }

            // Set the sprite in the Image component
            Image itemImage = listItem.GetComponentInChildren<Image>();
            if (itemImage != null)
            {
                //itemImage.enabled = true;
                //itemImage.rectTransform.sizeDelta = new Vector2(100, 100); // Example size
                itemImage.sprite = images[i];
                itemImage.preserveAspect = true; // Ensure aspect ratio is preserved
            }
            else
            {
                Debug.LogError("ListItemPrefab is missing an Image component!");
            }

            // Set the button click behavior
            Button itemButton = listItem.GetComponent<Button>();
            //itemButton.enabled = true;
            if (itemButton != null)
            {
                string itemDescription = descriptions[i];
                //AddEventTriggers(itemButton, () => AddOutline(itemButton), () => RemoveOutline(itemButton));
                if (potionPopulator) {
                    string ingred1 = potionsRecipe[i][0];
                    string ingred2 = potionsRecipe[i][1];
                    itemButton.onClick.AddListener(() => SelectItem(itemButton, itemImage.sprite, itemDescription, null, ingred1, ingred2));
                    //Debug.Log("Potion recipe: " + ingred1 + " + " + ingred2);
                } 
                else
                {
                    itemButton.onClick.AddListener(() => SelectItem(itemButton, itemImage.sprite, itemDescription));
                }

            }
            else
            {
                Debug.LogError("ListItemPrefab is missing a Button component!");
            }

        }
    }

    public void AddOutline(Button button, bool isSelected = false)
    {
        if (button == null) return;

        Outline outline = button.GetComponentInChildren<Image>().GetComponent<Outline>();

        if (outline == null)
        {
            outline = button.GetComponentInChildren<Image>().gameObject.AddComponent<Outline>();
        }
        outline.effectColor = highlightColor;
        outline.effectDistance = new Vector2(5, 5);
    }

    private void RemoveOutline(Button button)
    {
        if (button != selectedButton && button != hoveredButton)
        {
            Outline outline = button.GetComponentInChildren<Image>().GetComponent<Outline>();
            if (outline != null)
            {
                Destroy(outline);
            }
        }
    }

    private void HandlePointerEnter(Button button)
    {
        hoveredButton = button;
        if (button != selectedButton)
        {
            AddOutline(button);
        }
    }

    private void HandlePointerExit(Button button)
    {
        hoveredButton = null;
        if (button != selectedButton)
        {
            Outline outline = button.GetComponentInChildren<Image>().GetComponent<Outline>();
            if (outline != null)
            {
                Destroy(outline);
            }
        }
    }

    private void AddEventTriggers(Button button, UnityAction onEnter, UnityAction onExit)
    {
        EventTrigger trigger = button.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }
        trigger.triggers.Clear();

        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((eventData) => HandlePointerEnter(button));

        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((eventData) => HandlePointerExit(button));

        trigger.triggers.Add(entryEnter);
        trigger.triggers.Add(entryExit);
    }

    public class PotionDetails
    {
        public string Name;
        public Sprite Image;
        public string Description;
    }

    public PotionDetails GetPotionDetails(string potionName)
    {
        // Find the potion in the list (adjust logic based on your data structure)
        for (int i = 0; i < potionNames.Length; i++)
        {
            string potionItem = potionNames[i];
            if (potionItem == potionName)
            {
                var potionDescription = ""; // Logic to fetch description

                return new PotionDetails
                {
                    Name = potionName,
                    Image = potionImages[i],
                    Description = potionDescription
                };
            }
        }
        return null;
    }

    //public void Update()
    //{
    //    UpdateQuestPage();
    //}

    private void UpdateQuestPage()
    {
        // Clear existing quests
        foreach (Transform child in questPageList.transform)
        {
            Destroy(child.gameObject);
        }

        // Populate quest list
        var activeQuests = QuestManager.Instance.GetActiveQuests();
        foreach (var quest in activeQuests)
        {
            var questItem = Instantiate(questPrefab, questPageList.transform);
            questItem.GetComponentInChildren<Text>().text = quest.PotionName;
            questItem.GetComponentInChildren<Image>().sprite = quest.PotionImage;

            Image questImage = questItem.GetComponentInChildren<Image>();
            if (questImage != null) questImage.enabled = true;

            Text questText = questItem.GetComponentInChildren<Text>();
            if (questText != null) questText.enabled = true;

            // Set the button click behavior
            Button itemButton = questItem.GetComponent<Button>();
            if (itemButton != null)
            {
                itemButton.onClick.AddListener(() => SelectItem(itemButton, quest.PotionImage, quest.Description, quest.Title));

            }
            else
            {
                Debug.LogError("ListItemPrefab is missing a Button component!");
            }
        }
    }

}
