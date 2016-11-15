using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;



public class Character : MonoBehaviour {
    public bool canStartTalking = false;
    public bool isTalking = false;
    public bool isTeleporting = false;
    public bool insideABuilding;

    public Animator kristyAnimator;

    public bool isWalking = false;
    public bool isRunning = false;
    public bool isOnPauseMenu = false;

    Vector3 moveDirection = Vector3.zero;

    public float movementSpeed = 1;

    public GameManager gameManager;
    public FadeUtilities fadeUtilities;
    public ProgressManager progressManager;

    private Transform cameraTransformReference;

    void Start()
    {
        kristyAnimator = this.GetComponent<Animator>();
        progressManager = GameObject.Find("ProgressManager").GetComponent<ProgressManager>();
        //cameraTransformReference = Camera.main.transform;
        //cameraTransformReference.rotation = Quaternion.Euler(new Vector3(0, Camera.main.transform.position.y, Camera.main.transform.position.z));
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "NPC") //NPC detector
        {
            canStartTalking = true;
            print(other.gameObject.GetComponent<NPC>().which);
            gameManager.nearestNPC = other.gameObject.GetComponent<NPC>().which;
            gameManager.currentNPC = other.gameObject;

            canStartTalking = true;
        }

        if (other.gameObject.tag == "Teleport") //Teleport
        {
            if (!isTeleporting)
            {
                if (other.gameObject.GetComponent<Teleport>().teleportCode == "CrayunEntrance")
                {
                    StartCoroutine(fadeUtilities.LeaveCity(gameManager, 4f));
                    progressManager.firstTimeVisitingCrayun = false;
                    //spawn position on overworld = refer to Crayun
                }

                else if (other.gameObject.GetComponent<Teleport>().teleportCode == "CanvasEntrance")
                {
                    StartCoroutine(fadeUtilities.LeaveCity(gameManager, 4f));
                }
                else if (other.gameObject.GetComponent<Teleport>().teleportCode == "HouseFrontDoor")
                {
                    LeaveBuilding();
                }

                else {
                    isTeleporting = true;
                    StartCoroutine(fadeUtilities.FadeToBlack(gameManager, 4f));
                    gameManager.teleportTarget = other.gameObject.GetComponent<Teleport>().teleportPosition;
                    Invoke("DoorTeleport", 1.0f);
                }
             }
       }

        
    }



    void OnTriggerExit(Collider collision)
    {
        canStartTalking = false;
    }

    public void Move()
    {
        Vector3 UP = new Vector3(0f, 0f, 1f);
        Vector3 DWN = new Vector3(0f, 0f, -1f);
        Vector3 LEFT = new Vector3(-1f, 0f, 0f);
        Vector3 RIGHT = new Vector3(1f, 0f, 0f);

        
        if (Input.GetKey(KeyCode.W))
        {
            isRunning = true;
            kristyAnimator.SetBool("isRunning", isRunning);
            transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(UP), 0.75f);
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime, Space.World);
            Debug.Log(Camera.main.transform.forward);
        }

        if (Input.GetKey(KeyCode.A))
        {
            isRunning = true;
            kristyAnimator.SetBool("isRunning", isRunning);
            transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(LEFT), 0.75f);
            transform.Translate(Vector3.left * movementSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.S))
        {
            isRunning = true;
            kristyAnimator.SetBool("isRunning", isRunning);
            transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(DWN), 0.75f);
            transform.Translate(Vector3.back * movementSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.D))
        {
            isRunning = true;
            kristyAnimator.SetBool("isRunning", isRunning);
            transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(RIGHT), 0.75f);
            transform.Translate(Vector3.right * movementSpeed * Time.deltaTime, Space.World);
        }

        if(!Input.anyKey)
        {
            isRunning = false;
            kristyAnimator.SetBool("isRunning", isRunning);
        }
    }

    /*
    public void CheckMenu()
    {

        if (Input.GetKey(KeyCode.P))
        {
            if (isOnPauseMenu)
            {
                isOnPauseMenu = false;
                kristyAnimator.SetBool("isOnPauseMenu", isOnPauseMenu);
            }
            else if(!isOnPauseMenu)
            {
                isOnPauseMenu = true;
                kristyAnimator.SetBool("isOnPauseMenu", isOnPauseMenu);
            }

        }
    }
    */

    void DoorTeleport()
    {
        transform.position = gameManager.teleportTarget;
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y + 17f, transform.position.z + 9.0f);
        StartCoroutine(fadeUtilities.FadeFromBlack(gameManager, 4f));


    }


    void LeaveBuilding()
    {
        this.insideABuilding = false;
    }
}
