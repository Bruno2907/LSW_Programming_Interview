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

        private int currentItemIndex = -1;
        private int currentEquipedItemIndex = -1;
        private List<GameObject> itemButtonList = new();


        [Header ("Info")]
        public TMP_Text textName;
        public TMP_Text textDescription;
        public ScrollRect scrollRectDescription;
        public TMP_Text textPrice;
        public GameObject buttonBuySell;
        public GameObject buttonEquip;
        public float sellPriceMultiplier = 0.5f;


        [Header ("Preview")]
        public TMP_Text textCoins;
        public Image imageEquiped;


        // PlayerPrefs
        private readonly string ppKeyItemEquiped = "Item_Equiped";
        private readonly string ppKeyItemBought = "Item_{0}_Bought";


        private void Start ()
        {
            GameObject itemButton;
            ShopItemScriptableObject item;
            string equipedItemGuid = PlayerPrefs.GetString (ppKeyItemEquiped, "");
            for (int i = 0; i < GameController.instance.items.Length; i++)
            {
                int j = i;
                item = GameController.instance.items[i];
                itemButton = Instantiate (prefabItemButton, scrollRectItems.content.transform);
                itemButton.GetComponent<Image> ().sprite = item.icon;
                itemButton.GetComponent<Button> ().onClick.AddListener (delegate
                {
                    ButtonItemShowInfo (j);
                });
                bool bought = PlayerPrefs.GetInt (string.Format(ppKeyItemBought, item.guid), 0) == 1;
                itemButton.transform.GetChild(0).GetComponent<Image> ().enabled = bought;
                itemButton.transform.GetChild (1).GetComponent<Image> ().enabled = false;
                itemButtonList.Add (itemButton);
                if (equipedItemGuid == item.guid)
                {
                    currentEquipedItemIndex = i;
                }
            }
            if (currentEquipedItemIndex == -1)
            {
                currentEquipedItemIndex = 0;
                itemButtonList[0].transform.GetChild (0).GetComponent<Image> ().enabled = true;
                PlayerPrefs.SetInt (string.Format (ppKeyItemBought, GameController.instance.items[0].guid), 1);
                PlayerPrefs.SetString (ppKeyItemEquiped, GameController.instance.items[0].guid);
                PlayerPrefs.Save ();
            }
            itemButtonList[currentEquipedItemIndex].transform.GetChild (1).GetComponent<Image> ().enabled = true;
            imageEquiped.sprite = GameController.instance.items[currentEquipedItemIndex].icon;
            ButtonItemShowInfo (0);
        }


        public void ButtonInfoEquip ()
        {
            itemButtonList[currentEquipedItemIndex].transform.GetChild (1).GetComponent<Image> ().enabled = false;
            itemButtonList[currentItemIndex].transform.GetChild (1).GetComponent<Image> ().enabled = true;
            currentEquipedItemIndex = currentItemIndex;
            imageEquiped.sprite = GameController.instance.items[currentEquipedItemIndex].icon;
            PlayerPrefs.SetString (ppKeyItemEquiped, GameController.instance.items[currentEquipedItemIndex].guid);
            PlayerPrefs.Save ();
            ButtonItemShowInfo (currentItemIndex);
        }


        public void ButtonInfoBuy ()
        {
            ShopItemScriptableObject item = GameController.instance.items[currentItemIndex];
            bool bought = PlayerPrefs.GetInt (string.Format (ppKeyItemBought, item.guid), 0) == 1;
            if (!bought)
            {
                if (GameController.coins >= item.price)
                {
                    GameController.coins -= item.price;
                    PlayerPrefs.SetInt (string.Format (ppKeyItemBought, item.guid), 1);
                }
            }
            else
            {
                GameController.coins += Mathf.RoundToInt (item.price * sellPriceMultiplier);
                PlayerPrefs.SetInt (string.Format (ppKeyItemBought, item.guid), 0);
            }
            PlayerPrefs.Save ();
            ButtonItemShowInfo (currentItemIndex);
        }


        public void ButtonItemShowInfo (int arrayIndex)
        {
            ShopItemScriptableObject i = GameController.instance.items[arrayIndex];
            bool bought = PlayerPrefs.GetInt (string.Format (ppKeyItemBought, i.guid), 0) == 1;
            bool equiped = PlayerPrefs.GetString (ppKeyItemEquiped, "") == i.guid;
            textName.text = i.name;
            textDescription.text = i.description;
            LayoutRebuilder.ForceRebuildLayoutImmediate (scrollRectDescription.content);
            if (bought)
            {
                textPrice.text = "+" + Mathf.RoundToInt (i.price * sellPriceMultiplier).ToString ();
            }
            else 
            {
                textPrice.text = "-" + i.price.ToString ();
            }
            if (equiped)
            {
                textPrice.text = "";
            }
            currentItemIndex = arrayIndex;
            buttonBuySell.GetComponentInChildren<TMP_Text> ().text = bought ? "SELL" : "BUY";
            buttonBuySell.SetActive (!equiped);
            buttonEquip.SetActive (bought);
            itemButtonList[currentItemIndex].transform.GetChild (0).GetComponent<Image> ().enabled = bought;
            textCoins.text = GameController.coins.ToString ();
        }
    }
}