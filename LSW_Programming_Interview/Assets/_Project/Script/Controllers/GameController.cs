using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSW
{
    public class GameController : MonoBehaviour
    {
        public static int coins = 1000;

        public ShopItemScriptableObject[] items;

        public static GameController instance;
        private void Awake ()
        {
            instance = this;
        }
    }
}