using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class AvatarController : MonoBehaviour
{
    public ReneverseMintManager mintManager;
    readonly Dictionary<int, string> map = new() {
        { 1, "Normal Adventurer" },
        { 2, "Winsome Adventurer" },
        { 3, "Hat Adventurer" },
        { 4, "Aged Adventurer" }
    };

    readonly Dictionary<string,int> map2 = new() {
        { "Normal Adventurer", 1},
        { "Winsome Adventurer", 2 },
        { "Hat Adventurer", 3},
        { "Aged Adventurer", 4 }
    };

    readonly Dictionary<int, string> assetTemplateIDs = new()
    {
        { 2, "d0ee6be6-9c30-4a74-bfe1-b1a490b411bc" },
        { 3, "3ea9d474-e801-4bd7-ac1f-ccb1d80ca668" },
        { 4, "598b7300-fcd5-40c2-b137-5f972a48d31f" }
    };

    public List<GameObject> select;
    public List<GameObject> locked;
    // Start is called before the first frame update
    void Awake()
    {
        PlayerPrefs.GetInt("skin", 1);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Asset asset in ReneverseManager.NFTCounter) {
            locked[map2[asset.AssetName]-1].SetActive(false);
        }

        for(int i = 1; i <= select.Count; i++) {
            select[i-1].SetActive(i == PlayerPrefs.GetInt("skin"));
        }
    }

    public async void SetSkin (int index) {
      
        foreach(Asset asset in ReneverseManager.NFTCounter) {
            if(asset.AssetName == map[index]) {
                PlayerPrefs.SetInt("skin", index);
                return;
            }
        }

        try {
            await mintManager.Mint(assetTemplateIDs[index]);
            Asset tempAsset = new(map[index], "tempDesc", "tempUrl", assetTemplateIDs[index], "tempId");
            ReneverseManager.NFTCounter.Add(tempAsset);
        }
        catch(Exception e) {
            Debug.Log(e);
        }
        
    }
}
