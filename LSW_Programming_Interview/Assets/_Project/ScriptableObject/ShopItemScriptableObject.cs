using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LSW
{
    [CreateAssetMenu (fileName = "Item", menuName = "ScriptableObjects/ShopItem", order = 1)]
    public class ShopItemScriptableObject : ScriptableObject
    {
        public new string name;
        [TextArea (5, 10)]
        public string description;
        public Sprite icon;
        public int price;
        public LSWEnum.ClothingType clothingType;

        [HideInInspector]
        public string guid;


        private void Reset ()
        {
            guid = System.Guid.NewGuid ().ToString ();
        }
    }
}