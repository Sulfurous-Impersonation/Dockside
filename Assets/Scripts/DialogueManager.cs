/*** 
*file: DialogueManager.cs 
*Members: Juniper Watson, Andrew Sanford
*class: CS 4700 – Game Development 
*assignment: program 4
*date last modified: 12/4/2022 
* 
*purpose: This scripts manages the dialogue, including what dialogue shows up
*in the dialogue box and what sounds play during dialogue. Also takes care of
*the sell menu.
**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText; //name displayed in dialogue box
    public Text dialogueText; //dialogue displayed in dialogue box

    public Text fishText;
    public Text fishSoldText;
    public Text fishOwnedText;

    public Image fishImage;

    public Sprite mahimahiSprite;
    public Sprite tilapiaSprite;
    public Sprite salmonSprite;

    public GameObject sellButton;
    public GameObject festivalTransition;

    public AudioSource dialogueBoxClose;
    public AudioSource dialogueBoxOpen;

    public Animator animator; //reference to animator to handle dialogue box animation
    public Animator animatorSell; //reference to animator to handle sell menu animation

    private Queue<string> sentences; //queue of sentences in the dialogue
    private PlayerMovement player;
    private ManagerScript gameManager;
    private AudioSource audioSource;
    public AudioClip[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        player = FindObjectOfType<PlayerMovement>();
        gameManager = FindObjectOfType<ManagerScript>();
        audioSource = FindObjectOfType<AudioSource>();
    }

    public void StartDialogue (Dialogue dialogue)
    {
        //checks if game is complete on dialogue start since sell isn't accessible after game complete
        if (gameManager.gameComplete)
        {
            festivalTransition.SetActive(true);
        }

        dialogueBoxOpen.Play();
        //turn off player movement when dialogue starts
        player.canMove = false;
        //animate DialogueBox_Open, pulls dialogue box onto screen
        animator.SetBool("IsOpen", true);
        //set nameText to dialogue trigger's name
        nameText.text = dialogue.name;
        
        sentences.Clear(); //ensure queue is empty before starting new dialogue

        foreach (string sentence in dialogue.sentences)
            sentences.Enqueue(sentence);
        
        DisplayNextSentence();
    }

    public void DisplayNextSentence ()
    {
        //play click sound effect
        //audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length-1)]);

        //check if queue is empty
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        if(sentences.Count == 1 && gameManager.activeSellButton)
        {
            sellButton.SetActive(true);
        }

        //else
        //get next sentence
        string sentence = sentences.Dequeue();
        StopAllCoroutines(); //ensure previous sentence is finished animating
        StartCoroutine(TypeSentence(sentence)); //animate sentence
    }

    public void DisplaySellScreen()
    {
        EndDialogue();

        player.canMove = false;

        animatorSell.SetBool("IsOpen", true);
        
        //play click sound effect
        //audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length-1)]);

        switch (nameText.text)
        {
            case "Taqueria Lady":
                fishText.text = "Mahi-Mahi";
                fishImage.sprite = mahimahiSprite;
                fishSoldText.text = "Sold: " + gameManager.mahimahiSold.ToString();
                fishOwnedText.text = "Owned: " + gameManager.mahimahi.ToString();
                break;

            case "Gordon":
                fishText.text = "Tilapia";
                fishImage.sprite = tilapiaSprite;
                fishSoldText.text = "Sold: " + gameManager.tilapiaSold.ToString();
                fishOwnedText.text = "Owned: " + gameManager.tilapia.ToString();
                break;

            case "Sushi Chef":
                fishText.text = "Salmon";
                fishImage.sprite = salmonSprite;
                fishSoldText.text = "Sold: " + gameManager.salmonSold.ToString();
                fishOwnedText.text = "Owned: " + gameManager.salmon.ToString();
                break;
        }
    }

    //animate sentence by adding one letter at a time
    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
    }

    //Pulls dialogue box off of screen
    private void EndDialogue()
    {
        dialogueBoxClose.Play();
        //animate DialogueBox_Close
        animator.SetBool("IsOpen", false);
        //disable dialogue sell button
        sellButton.SetActive(false);
        //allow player to move once dialogue is closed
        player.canMove = true;
    }

    public void SellFish()
    {
        switch (nameText.text)
        {
            case "Taqueria Lady":
                if(gameManager.mahimahi > 0)
                {
                    gameManager.mahimahi--;
                    gameManager.mahimahiSold++;
                    fishSoldText.text = "Sold: " + gameManager.mahimahiSold.ToString();
                    fishOwnedText.text = "Owned: " + gameManager.mahimahi.ToString();
                }
                break;

            case "Gordon":
                if (gameManager.tilapia > 0)
                {
                    gameManager.tilapia--;
                    gameManager.tilapiaSold++;
                    fishSoldText.text = "Sold: " + gameManager.tilapiaSold.ToString();
                    fishOwnedText.text = "Owned: " + gameManager.tilapia.ToString();
                }
                break;

            case "Sushi Chef":
                if (gameManager.salmon > 0)
                {
                    gameManager.salmon--;
                    gameManager.salmonSold++;
                    fishSoldText.text = "Sold: " + gameManager.salmonSold.ToString();
                    fishOwnedText.text = "Owned: " + gameManager.salmon.ToString();
                }
                break;
        }
    }

    public void CloseSellSceen()
    {
        animatorSell.SetBool("IsOpen", false);

        //checks if game is complete on close sell screen for player convenience
        if (gameManager.gameComplete)
        {
            festivalTransition.SetActive(true);
        }

        player.canMove = true;
    }
}

