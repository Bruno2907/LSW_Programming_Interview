using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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


        private void Start ()
        {
            GameObject itemButton;
            ShopItemScriptableObject item;
            string equipedItemGuid = PlayerPrefs.GetString ("Item_Equiped", "");
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
                bool bought = PlayerPrefs.GetInt ("Item_" + item.guid + "_bought", 0) == 1;
                itemButton.transform.GetChild(0).GetComponent<Image> ().enabled = bought;
                itemButtonList.Add (itemButton);
                if (equipedItemGuid == item.guid)
                {
                    currentEquipedItemIndex = i;
                    itemButton.transform.GetChild (1).GetComponent<Image> ().enabled = true;
                }
                else
                {
                    itemButton.transform.GetChild (1).GetComponent<Image> ().enabled = false;
                }
            }
            if (currentEquipedItemIndex == -1)
            {
                currentEquipedItemIndex = 0;
                itemButtonList[0].transform.GetChild (1).GetComponent<Image> ().enabled = true;
            }
            ButtonItemShowInfo (0);
        }


        public void ButtonInfoEquip ()
        {
            itemButtonList[currentEquipedItemIndex].transform.GetChild (1).GetComponent<Image> ().enabled = false;
            itemButtonList[currentItemIndex].transform.GetChild (1).GetComponent<Image> ().enabled = true;
            currentEquipedItemIndex = currentItemIndex;
            PlayerPrefs.SetString ("Item_Equiped", GameController.instance.items[currentEquipedItemIndex].guid);
            PlayerPrefs.Save ();
        }


        public void ButtonInfoBuy ()
        {
            ShopItemScriptableObject item = GameController.instance.items[currentItemIndex];
            bool bought = PlayerPrefs.GetInt ("Item_" + item.guid + "_bought", 0) == 1;
            if (!bought)
            {
                if (GameController.coins >= item.price)
                {
                    GameController.coins -= item.price;
                    PlayerPrefs.SetInt ("Item_" + item.guid + "_bought", 1);
                }
            }
            else
            {
                GameController.coins += Mathf.RoundToInt (item.price * sellPriceMultiplier);
                PlayerPrefs.SetInt ("Item_" + item.guid + "_bought", 0);
            }
            PlayerPrefs.Save ();
            ButtonItemShowInfo (currentItemIndex);
        }


        public void ButtonItemShowInfo (int arrayIndex)
        {
            ShopItemScriptableObject i = GameController.instance.items[arrayIndex];
            bool bought = PlayerPrefs.GetInt ("Item_" + i.guid + "_bought", 0) == 1;
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
            currentItemIndex = arrayIndex;

            buttonBuySell.GetComponentInChildren<TMP_Text> ().text = bought ? "SELL" : "BUY";
            buttonEquip.SetActive (bought);
            itemButtonList[currentItemIndex].transform.GetChild (0).GetComponent<Image> ().enabled = bought;

            textCoins.text = GameController.coins.ToString ();
        }
    }
}