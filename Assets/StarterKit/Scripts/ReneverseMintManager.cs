using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReneverseMintManager : MonoBehaviour
{
    public GameObject TemplateIDInput;
    public Button MintButton;

    private void Start()
    {
        if(!MintButton) return;
        MintButton.onClick.AddListener(ExecuteMint);
    }

    //Function to be used for minting
    public async void ExecuteMint()
    {
        string TemplateID = TemplateIDInput.GetComponent<TMP_InputField>().text;
        await Mint(TemplateID);
    }

    //Mint function
    //Dependencies : Reneverse Manager
    public async Task Mint(string templateID)
    {
        try
        {
            var response = await ReneverseManager.ReneAPI.Game().AssetMint(templateID);
            Debug.Log(response);

            Debug.Log("Asset Minting in progress");
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
    }
}
