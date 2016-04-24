using UnityEngine;
using System.Collections;
using Wolfpack.Character;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace Wolfpack.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance = null;
        public Consumable activeItem;
        public int health;

        [SerializeField]
        private PlayerCharacter player = null;
        private List<Consumable> inventory;

        [SerializeField]
        private Image content;

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

        private void UpdateHealth(int value)
        {
            health = value;
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            content.fillAmount = MapAmount();
        }

        private float MapAmount()
        {
            return (float)health / player.maxHealth;
        }

        private void UpdateItems(string item)
        {
            throw new NotImplementedException();
        }

        private void SetInitialReference()
        {
            player = GameObject.Find("Player").GetComponent<PlayerCharacter>();
        }

        void OnLevelWasLoaded(int level)
        {
            Awake();
        }

        void Awake()
        {
            Debug.Log("PlayerManager Awake");
            GetPlayerManager();
            SetInitialReference();
            player.OnHealthUpdated += UpdateHealth;
            player.OnItemsUpdated += UpdateItems;
        }

        void Start()
        {
            player.SetHealth(health);
            UpdateHealthBar();
        }

        void Update()
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
            }

            //get attack input from player
            bool atk = InputWrapper.GetButtonDown("Attack light");

            if (atk)
            {
                player.Attack();
            }
        }
    }
}
