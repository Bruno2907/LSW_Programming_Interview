using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LSW
{
    public class CanvasClothesShop : MonoBehaviour
    {
        [Header ("Itens")]
        public ScrollRect scrollRectItems;
        public GameObject prefabItemButton;
        public ShopItemScriptableObject[] items;

        [Header ("Info")]
        public TMP_Text textName;
        public TMP_Text textDescription;
        public TMP_Text textPrice;
        public GameObject buttonBuySell;
        public GameObject buttonEquip;


        private void Start ()
        {
            GameObject itemButton;
            for (int i = 0; i < items.Length; i++)
            {
                int j = i;
                itemButton = Instantiate (prefabItemButton, scrollRectItems.content.transform);
                itemButton.GetComponent<Image> ().sprite = items[i].icon;
                itemButton.GetComponent<Button> ().onClick.AddListener (delegate
                {
                    ButtonItemShowInfo (j);
                });
            }
            ButtonItemShowInfo (0);
        }


        void ButtonItemShowInfo (int arrayIndex)
        {
            ShopItemScriptableObject i = items[arrayIndex];
            textName.text = i.name;
            textDescription.text = i.description;
            textPrice.text = i.price.ToString ();
        }
    }
}