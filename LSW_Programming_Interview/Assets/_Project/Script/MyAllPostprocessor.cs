using UnityEditor;

namespace LSW
{
    class MyAllPostprocessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            foreach (string str in importedAssets)
            {
                //Debug.Log ("Reimported Asset: " + str);
                ShopItemScriptableObject asset = (ShopItemScriptableObject)AssetDatabase.LoadAssetAtPath (str, typeof (ShopItemScriptableObject));
                if (asset != null)
                {
                    asset.guid = System.Guid.NewGuid ().ToString ();
                }
                //Debug.Log (asset);
            }
        }
    }
}