using UnityEngine;
using System.Collections;
using Wolfpack.Characters;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Linq;

namespace Wolfpack.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance = null;
        public int health;

        public CountedConsumables[] inputInventory;

        private Dictionary<Consumable, int> inventory = new Dictionary<Consumable, int>();

        [SerializeField]
        private Image healthContent;
        [SerializeField]
        private Image itemContent;
        [SerializeField]
        private Text itemCount;
        private bool inCombat = true;

        [SerializeField]
        private PlayerCharacter player = null;

        [SerializeField]
        private int active = 0;

        private void UpdateHealth(int value)
        {
            health = value;
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            healthContent.fillAmount = MapAmount();
        }

        private float MapAmount()
        {
            return (float)health / player.maxHealth;
        }

        private void UpdateItems(string item, int count)
        {
			Knife tmp = new Knife() { name = item };
			if (inventory.ContainsKey(tmp)) {
				inventory[tmp] += count;

				//reset consumable view
				itemCount.text = inventory[tmp].ToString();
			} else {
				Debug.Log("Item - " + item + " - not found");
			}
        }

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

        private void UseItem()
        {
            Consumable tmp = inventory.Keys.ElementAt(active);
			if (inventory[tmp] > 0) {
				switch (tmp.type) {
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

        private void SetInitialReference()
        {
            player = GameObject.Find("Player").GetComponent<PlayerCharacter>();
        }

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
            Debug.Log("PlayerManager Awake");
            SetInitialReference();
            InitializeInventory();
        }

        public void Start()
        {
            Debug.Log("PlayerManager Start");
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

            player.Walk(movement.normalized);

            //get targeting input from player
            bool target = InputWrapper.GetButtonDown("Target");

            if (target)
            {
                player.ToggleTarget();
				if (player.target != null) {
					inCombat = true;
				} else {
					inCombat = false;
				}
            }

            //get attack input from player
            bool atk = InputWrapper.GetButtonDown("Attack light");

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
				if (inCombat) UseItem();
				else player.Interact();
            }
        }
    }
}
