using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public FadeUtilities fadeUtilities;

    public Character kristy;
    public GameObject todd;
    public NPC placeholder;
    public GameObject kristyFace;

    public GameObject tutorialPoem;

    public AudioClip City;
    public AudioClip Battle;
    public AudioClip Battle2;

    public AudioClip dialoguePage;
    public AudioClip fanfare;

    private AudioSource source;


    public CanvasGroup fadeCanvasGroup;

    [Header("Conversation auxiliar variables")]
    public int nearestNPC = 0; //conversation number code
    public GameObject currentNPC;
    int currentLine = 0; //current line of conversation

    public bool firstTimeEntering = false;
    public Animation cityNameSlideInstant;

    [Header("Dialogue Canvas Elements")]
    public Text dialogue;
    public GameObject dialogueCanvas;
    public GameObject nextDialogueBox;
    public GameObject dialogueArrow;
    public GameObject speakerSprite;
    public GameObject speakerName;

    public GameObject cityNameDisplay;
    public GameObject KUC;

    public bool startingBattle = false;
    public bool enteredCity = false;

    public Vector3 teleportTarget;

    [Header("Dialogue Facesets")]
    public Sprite toddBlushing;
    public Sprite toddExcited;
    public Sprite toddSigh;
    public Sprite toddSmiling;
    public Sprite toddSpeaking;
    public Sprite toddNormal;
    public Sprite toddShocked;
    public Sprite toddShockedMore;
    public Sprite toddTooBlushed;
    public Sprite toddSpeakingBlushed;
    public Sprite toddAwe;
    public Sprite toddNaughty;

    public Sprite adaSpeaking;
    public Sprite adaSmiling;
    public Sprite adaLaughing;
    public Sprite adaShocked;

    [Header("Progress pointers")]
    public ProgressManager progressManager; //transition canvas


    void Awake()
    {


    }

    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();

        //music
        if (!source.isPlaying)
        {
            source.clip = City;
            source.Play();
        }

        kristyFace = kristy.transform.GetChild(5).gameObject;
        
        progressManager = GameObject.Find("ProgressManager").GetComponent<ProgressManager>();
        todd = GameObject.Find("Todd");

        //Just left battle? No city name display
        if (progressManager.justLeftBattle)
        {
            Destroy(tutorialPoem);
            Destroy(GameObject.Find("CityNameCanvas"));
            progressManager.justLeftBattle = false;
        }

        //Not first time playing = no cutscene
        if (!progressManager.firstTimeVisitingCrayun)
        {
            Camera.main.GetComponent<Animator>().enabled = false;
        }

        //Todd was defeated previously
        if (progressManager.defeatedTodd)
        {
            
            kristy.gameObject.transform.position = progressManager.kristyPosition;
            Camera.main.transform.position = progressManager.kristyPosition;
            kristy.gameObject.transform.rotation = progressManager.kristyRotation;
            progressManager.ResetKristyPosition();

            //start post-victory conversation

            //start conversation events
            dialogueCanvas.SetActive(true);
            kristy.isTalking = true;
            kristy.canStartTalking = false;

            currentNPC = GameObject.Find("Todd");
            currentNPC.transform.LookAt (new Vector3(kristy.transform.position.x,
                                        currentNPC.transform.position.y,
                                        kristy.transform.position.z));

            currentNPC.GetComponent<NPC>().which = 100; //this will guarantee the new conversation
            nearestNPC = 100; //this will load the new conversation

            Conversation.loadConversation(nearestNPC);
            dialogue.text = Conversation.loadConversation(nearestNPC)[currentLine];

            updateConversationSprites(nearestNPC); //set faceset
        }
        
        startingBattle = false;
        StartCoroutine(fadeUtilities.FadeFromBlack(this, 1f));
    }
	
	// Update is called once per frame
    void LateUpdate()
    {
        UpdateCamera();
    }

	void Update () {
        if(!kristy.isTalking)
            if(Camera.main.transform.position.y > 90) kristy.Move();


        //music
        if (!source.isPlaying && startingBattle && !source.loop)
        {
            source.clip = Battle2;
            source.Play();
            source.loop = true;
        }
        
        ///Dialogue events
        ///

        if (Input.GetButtonDown("Enter") && !startingBattle && !kristy.isRunning)
        {
            if (kristy.canStartTalking)
            {
                //start conversation
                print("talk");

                dialogueCanvas.SetActive(true);
                kristy.isTalking = true;
                kristy.canStartTalking = false;
                Conversation.loadConversation(nearestNPC);
                dialogue.text = Conversation.loadConversation(nearestNPC)[currentLine];
                //placeholder.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(kristy.transform.position), 0.75f); ;
                if(nearestNPC != 20) currentNPC.transform.LookAt(new Vector3(kristy.transform.position.x,
                                        currentNPC.transform.position.y,
                                        kristy.transform.position.z));
                updateConversationSprites(nearestNPC); //set faceset


            }
            else if (kristy.isTalking)
            {
                if (kristy.isTalking && currentLine == Conversation.loadConversation(nearestNPC).Length-1)
                {
                    dialogueCanvas.SetActive(false);
                    kristy.isTalking = false;
                    //kristy.canStartTalking = true;
                    currentLine = 0;
                    onEndedConversation(nearestNPC); //so what happens after ending the conversation?
                }
                else {
                    currentLine++;
                    AudioSource.PlayClipAtPoint(dialoguePage, transform.position);
                    dialogue.text = Conversation.loadConversation(nearestNPC)[currentLine]; //set dialogue line
                    print(currentLine);
                    updateConversationSprites(nearestNPC); //set faceset
                }


            }
            
            else {
                print("nope!");
            }
        }
        
        if (currentLine == Conversation.loadConversation(nearestNPC).Length - 1)
        {
            nextDialogueBox.SetActive(false);
        }
        else
        {
            nextDialogueBox.SetActive(true);
        }
        ///
    }

    public void updateConversationSprites(int whatConversation)
    {
        switch (whatConversation)
        {
            case 0: //Todd's dialogue
                //switch dialogue avatar to Todd's reactions, following dialogue lines
                speakerSprite.SetActive(true);
                speakerName.SetActive(true);
                if (currentLine == 0)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddAwe;

                }
                else if (currentLine == 1)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddSpeaking;


                }
                else if (currentLine == 2)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddNormal;


                }
                else if (currentLine == 3)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddBlushing;
                    Debug.Log("linha 3" + currentLine);
                }
                else if (currentLine == 4)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddShockedMore;
                }
                else if (currentLine == 4)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddSpeakingBlushed;
                }
                else if (currentLine == 5)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddSpeakingBlushed;
                }
                else if (currentLine == 6)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddTooBlushed;
                }
                else if (currentLine == 7)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddSmiling;
                }
                else if (currentLine == 8)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddSpeaking;
                }
                else if (currentLine == 9)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddNormal;
                    KUC.SetActive(true);
                }
                else if (currentLine == 10)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddBlushing;
                }
                else if (currentLine == 11)
                {
                    KUC.SetActive(false);
                    speakerSprite.GetComponent<Image>().sprite = toddNaughty;
                }
                else if (currentLine == 12)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddShockedMore;
                }
                else if (currentLine == 13)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddSigh;
                }
                else if (currentLine == 14)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddExcited;
                    dialogue.fontSize = 60;
                }
                break;

            case 100:
                //switch dialogue avatar to Todd's reactions, following dialogue lines
                speakerSprite.SetActive(true);
                speakerName.SetActive(true);
                if (currentLine == 0)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddAwe;
                }
                else if (currentLine == 1)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddSpeaking;
                }
                else if (currentLine == 2)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddShockedMore;
                }
                else if (currentLine == 3)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddSigh;
                }
                else if (currentLine == 4)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddSmiling;
                }
                else if (currentLine == 5)
                {
                    AudioSource.PlayClipAtPoint(fanfare, transform.position);
                    speakerSprite.SetActive(false);
                    speakerName.SetActive(false);
                }
                else if (currentLine == 6)
                {
                    speakerSprite.SetActive(true);
                    speakerName.SetActive(true);
                }
                else if (currentLine == 7)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddShockedMore;
                }
                else if (currentLine == 8)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddShocked;
                }
                else if (currentLine == 9)
                {
                    speakerSprite.GetComponent<Image>().sprite = toddNormal;
                }
                else if (currentLine == 10)
                {
                    speakerSprite.SetActive(false);
                    speakerName.SetActive(false);
                    Destroy(todd);
                }
                break;


            case 101:
                //switch dialogue avatar to Todd's reactions, following dialogue lines
                speakerSprite.SetActive(true);
                speakerName.SetActive(true);
                speakerName.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Ada (phone)";
                if (currentLine == 0)
                {
                    speakerSprite.GetComponent<Image>().sprite = adaSmiling;
                }
                else if (currentLine == 1)
                {
                    speakerSprite.GetComponent<Image>().sprite = adaSmiling;
                }
                else if (currentLine == 2)
                {
                    speakerSprite.GetComponent<Image>().sprite = adaSpeaking;
                }
                else if (currentLine == 3)
                {
                    speakerSprite.GetComponent<Image>().sprite = adaLaughing;
                }
                else if (currentLine == 4)
                {
                    speakerSprite.GetComponent<Image>().sprite = adaSpeaking;
                }
                else if (currentLine == 5)
                {
                    speakerSprite.SetActive(false);
                    speakerName.SetActive(false);
                }
                else if (currentLine == 6)
                {
                    speakerSprite.SetActive(true);
                    speakerName.SetActive(true);
                    speakerSprite.GetComponent<Image>().sprite = adaShocked;
                    dialogue.fontSize = 50;
                }
                break;

            default:
                speakerSprite.SetActive(false);
                speakerName.SetActive(false);
                break;
        }
    }

    public void onEndedConversation(int whatConversation)
    {
        switch (whatConversation)
        {
            case 0: //the conversation that leads to battle
                startingBattle = true;
                StartCoroutine( GoToBattle());
                break;
            case 100:
                

                //start Ada conversation events
                dialogueCanvas.SetActive(true);
                kristy.isTalking = true;
                kristy.canStartTalking = false;

                currentNPC = GameObject.Find("Ada");
                /*currentNPC.transform.LookAt(new Vector3(kristy.transform.position.x,
                                            this.transform.position.y,
                                            kristy.transform.position.z)); */

                currentNPC.GetComponent<NPC>().which = 101; //this will guarantee the new conversation
                nearestNPC = 101; //this will load the new conversation

                Conversation.loadConversation(nearestNPC);
                dialogue.text = Conversation.loadConversation(nearestNPC)[currentLine];

                updateConversationSprites(nearestNPC); //set faceset

                break;
            case 101:
                PlayerPrefs.SetInt("GameBeatenBefore", 1);
                break;

            default:
                break;
        }
    }

    IEnumerator GoToBattle()
    {
        while (fadeCanvasGroup.alpha < 1f) //trocar transição para o troço redondo
        {
            fadeCanvasGroup.alpha += Time.deltaTime;
            Camera.main.transform.position -= new Vector3(0f, 30*Time.deltaTime, -17 * Time.deltaTime);
            yield return null;
        }
        progressManager.SaveKristyPosition();
        SceneManager.LoadScene("Battlefield");
    }

    void UpdateCamera()
    {
        if (!startingBattle && kristy.transform.position.z >= -110.0f && 
            SceneManager.GetActiveScene().name == "CrayunTown")
            FollowCharacterCamera.UpdateCameraOutdoors();
        else if(SceneManager.GetActiveScene().name == "InsideBuilding")
            FollowCharacterCamera.UpdateCameraInsideBuilding();
    }


    //Pause menu
    void TogglePauseMenu()
    {

    }
}
