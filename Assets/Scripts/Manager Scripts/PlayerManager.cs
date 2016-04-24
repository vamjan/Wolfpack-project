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

        public void OnEnable()
        {
            player.OnHealthUpdated += UpdateHealth;
            player.OnItemsUpdated += UpdateItems;
        }

        public void OnDisable()
        {
            player.OnHealthUpdated -= UpdateHealth;
            player.OnItemsUpdated -= UpdateItems;
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

        public void Awake()
        {
            SetInitialReference();
            GetPlayerManager();
        }

        public void Start()
        {
            player.SetHealth(health);
            UpdateHealthBar();
        }
    }
}
