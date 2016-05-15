using UnityEngine;
using System.Collections;
using Wolfpack.Characters;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Linq;

namespace Wolfpack.Managers
{
	/// <summary>
	/// Class of Player Manager to take care of player invetory and user input.
	/// Singleton design pattern.
	/// </summary>
    public class PlayerManager : MonoBehaviour
    {
		//static instance of PlayerManager, which allows it to be accessed by any other script
        public static PlayerManager instance = null;
		//players actual health
        public int health;
		//input inventory used for initialization of actual inventory at the start of the game (Unity does not like abstract classes). Setup in UnityEditor.
        public CountedConsumables[] inputInventory;
		//inventory that is used to access items
        private Dictionary<Consumable, int> inventory = new Dictionary<Consumable, int>();

		//health hub slider
        [SerializeField]
        private Image healthContent = null;
		//item content image
        [SerializeField]
		private Image itemContent = null;
		//itm count label
        [SerializeField]
		private Text itemCount = null;
		//reference to player
        [SerializeField]
        private PlayerCharacterScript player = null;
		//index of active item from inevntory
        [SerializeField]
        private int active = 0;

		/// <summary>
		/// Updates the health value.
		/// </summary>
		/// <param name="value">Value.</param>
        private void UpdateHealth(int value)
        {
            health = value;
            UpdateHealthBar();
        }

		/// <summary>
		/// Updates the health on GUI.
		/// </summary>
        private void UpdateHealthBar()
        {
            healthContent.fillAmount = MapAmount();
        }

		/// <summary>
		/// Maps the health amount to GUI.
		/// </summary>
		/// <returns>Fill value</returns>
        private float MapAmount()
        {
            return (float)health / player.maxHealth;
        }

		/// <summary>
		/// Updates the items in the inventory.
		/// Used for picking up items.
		/// </summary>
		/// <param name="item">Item</param>
		/// <param name="count">Count</param>
        private void UpdateItems(string item, int count)
        {
			Knife tmp = new Knife() { name = item }; //using consumable GetHashCode and Equals to update inventory
			if (inventory.ContainsKey(tmp)) {
				inventory[tmp] += count;

				//reset consumable view
				if(inventory.ElementAt(active).Key.Equals(tmp))
					itemCount.text = inventory[tmp].ToString();
			} else {
				Debug.Log("Item - " + item + " - not found");
			}
        }

		/// <summary>
		/// Toggles the item player is using.
		/// Redraw GUI.
		/// </summary>
        private void ToggleItem()
        {
			if (inventory.Count != 0)
            {
				active = (active + 1) % inventory.Count;
				Consumable tmp = inventory.ElementAt(active).Key;
				itemContent.sprite = tmp.icon;
				itemCount.text = inventory[tmp].ToString();
            }
            else
            {
                Debug.Log("Inventory is empty");
            }
            
        }

		/// <summary>
		/// Use item on index given by active.
		/// </summary>
        private void UseItem()
        {
            Consumable tmp = inventory.Keys.ElementAt(active);
			if (inventory[tmp] > 0) {
				switch (tmp.type) { //might be unneccesary now, but will be neede when more items come
					case Target.SELF:
						tmp.Use(player.gameObject);
						break;
					case Target.ENEMY:
						tmp.Use(player.gameObject);
						break;
					default:
						Debug.Log("Targeting policy " + tmp.type + " is invalid");
						break;
				}
				inventory[tmp]--;
				itemCount.text = inventory[tmp].ToString();
			}
        }

		/// <summary>
		/// Initializes the inventory from inputInventory.
		/// </summary>
        private void InitializeInventory()
        {
            foreach(var item in inputInventory)
            {
                switch (item.name)
                {
                    case "Knife":
                        inventory.Add(new Knife()
                        {
                            name = item.name,
                            damage = item.damage,
                            icon = item.icon,
                            speed = item.speed,
                            type = item.type,
							knifePrefab = item.prefab
                        }, item.count);
                        break;
                    case "Potion":
                        inventory.Add(new Potion()
                        {
                            name = item.name,
                            damage = item.damage,
                            icon = item.icon,
                            type = item.type
                        }, item.count);
                        break;
                    default:
                        Debug.Log("Invalid item " + item.name);
                        break;
                }
            }
        }

		/// <summary>
		/// Set initial references to neccesary objects and validate data.
		/// </summary>
        private void SetInitialReference()
        {
            player = GameObject.Find("Player").GetComponent<PlayerCharacterScript>();
        }

		/// <summary>
		/// Gets the player manager.
		/// Enforces the singleton design pattern. Every scene has the same PlayerManager thanks to it.
		/// Saves gamedata when transitioning to next scene.
		/// </summary>
        public void GetPlayerManager()
        {
            if (instance != null && instance != this)
			{
                Destroy(this.gameObject);
                return;
            }
            else
            {
                instance = this;
            }
            DontDestroyOnLoad(this.gameObject);
        }

        public void OnLevelWasLoaded(int level)
        {
            if (this != instance) return;
            SetInitialReference();
			OnEnable();
			Start();
        }

        public void OnEnable()
        {
            player.OnHealthUpdated += UpdateHealth;
            player.OnItemsUpdated += UpdateItems;
        }

        public void OnDisable()
        {
            //not necessary
            /*
            player.OnHealthUpdated -= UpdateHealth;
            player.OnItemsUpdated -= UpdateItems;
            */
        }

        public void Awake()
        {
            GetPlayerManager();
			Debug.Log("PlayerManager Awake " + this.GetHashCode());
            SetInitialReference();
            InitializeInventory();
        }

        public void Start()
        {
			Debug.Log("PlayerManager Start " + this.GetHashCode());
            player.SetHealth(health);
            UpdateHealthBar();
            ToggleItem();
        }

        public void Update()
        {
            Vector2 movement;
            // Gets information about input axis
            float inputX = InputWrapper.GetAxisRaw("Horizontal");
            float inputY = InputWrapper.GetAxisRaw("Vertical");

            // Prepare the movement for each direction
            movement = new Vector2(inputX, inputY);

            // Makes the movement relative to time
            movement *= Time.deltaTime;

			//get dodge input from player
			bool dodge = InputWrapper.GetButtonDown("Dodge");

			if (dodge)
			{
				player.StartDodge();
			}

			player.Walk(movement.normalized);

            //get targeting input from player
            bool target = InputWrapper.GetButtonDown("Target");

            if (target)
            {
				if (!player.inCombat)
					player.Interact();
				else
                	player.ToggleTarget();
            }

            //get attack input from player
            bool atk = InputWrapper.GetButtonDown("Attack");

            if (atk)
            {
                player.Attack();
            }

            bool toggleItem = InputWrapper.GetButtonDown("Toggle item");

            if(toggleItem)
            {
                ToggleItem();
            }

            bool useItem = InputWrapper.GetButtonDown("Use item");

            if(useItem)
            {
				UseItem();
            }
        }
    }
}
