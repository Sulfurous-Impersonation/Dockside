using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishCaughtDisplayBehaviour : MonoBehaviour
{
    public ManagerScript managerScript;
    public GameObject fishImageObject;
    public GameObject catchTextObject;

    public Sprite mahimahi;
    public Sprite salmon;
    public Sprite tilapia;
    public Sprite noCatch;

    private TextMeshProUGUI catchText;
    private Image fishImage;
    private Sprite currentfishImage;

    public void displayCatchInfo()
    {
        catchText = catchTextObject.GetComponent<TextMeshProUGUI>();
        catchText.text = "You caught " + managerScript.fishName;

        switch (managerScript.fishName)
        {
            case "a Mahi-Mahi!":
                currentfishImage = mahimahi;
                break;
            case "a Salmon!":
                currentfishImage = salmon;
                break;
            case "a Tilapia!":
                currentfishImage = tilapia;
                break;
            case "nothing.":
                currentfishImage = noCatch;
                break;
        }

        fishImage = fishImageObject.GetComponent<Image>();
        fishImage.sprite = currentfishImage;
    }
}
