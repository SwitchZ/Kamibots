using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    /*
    *For all the means, Player = each one of kamibots, not Kristy.
    */

    public Vector2 gridPosition = Vector2.zero; //? makes dealing with world positions more easy: stores player position in terms of grid. ?

    public Vector3 moveDestination; //moveDestination is a target position
    public float moveSpeed = 10.0f; //moveSpeed is speed of which player moves there

    public bool moving = false;  //status flag: Move option chosen
    public bool attacking = false; //status flag: Attack option chosen
    public bool eliminated = false;
    public bool lit = false;

    public bool characterIsIdle = true;
    public bool characterIsMoving = false;
    public bool characterIsAttacking = false;
    public bool characterIsInactive = false;
    public bool characterIsTakingDamage = false;

    public string playerName = "Kamibot";
    public int HP = 10; //armor points
    public int MaxHP = 10;
    public int battery = 10; //battery points
    public int shots = 10; //shot points
    public int MaxShots = 10;
    public int actionPoints = 2;
    public int exp = 0;
    public int level = 1;

    public Transform HPBall;
    public Transform Sword;
    public Transform arm;

    public Specs specs;

    public int baseMovementRange = 0;
    public int baseAttackRange = 0;
    public int baseDamageReduction = 0;
    public int baseDamageBase = 0;

    public Animator kamibotAnimator;
    public int attackerCode;

    [Header("Materials")]
    public Material Kinderbot;
    public Material Mantial;
    public Material Aquasniper;
    public Material Sliceknight;
    public Material Metaman;

    // all the methods until line 90 are responsible for checking player's equipments and give him the status associated.
    public int damageReduction
    {
        get
        {
            int ret = baseDamageReduction;

            if (specs != null) ret += specs.alterDamageReduction;

            return ret;
        }
        set { }
    }

    public int damageBase
    {
        get
        {
            int ret = baseDamageBase;

            if (specs != null) ret += specs.alterDamageBase;

            return ret;
        }
        set { }
    }

    public int movementPerActionPoint
    {
        get
        {
            int ret = baseMovementRange;

            if (specs != null) ret += specs.alterMovementRange;

            return ret;
        }
        set { }
    }

    public int attackRange
    {
        get
        {
            int ret = baseAttackRange;

            if (specs != null) ret += specs.alterAttackRange;

            return ret;
        }
        set { }
    }


    //movement animation
    public List<Vector3> positionQueue = new List<Vector3>(); //positionQueue is something like a list of waypoints (remember Unreal classes?)
    //

    void Awake()
    {
        moveDestination = transform.position;

    }

    void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        kamibotAnimator.SetInteger("Attacker", this.attackerCode);

        /*
        if (actionPoints == 0) //ACTIONS ZERO
        {
            transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.gray;

        }
        */
        if (battery <= 4) characterisTired();
        else characterisNotTired();

        /*
        if (battery == 0) //STAMINA ZERO
        {
            characterisInactive();
            HPBall.GetComponent<Renderer>().material.color = Color.Lerp(HPBall.GetComponent<Renderer>().material.color, Color.black, 0.1f); //led OFF

        }
        */

        //HP display: visuals
        if (HP <= 0 || battery <= 0)
        {
            HPBall.GetComponent<Renderer>().material.color = Color.Lerp(HPBall.GetComponent<Renderer>().material.color, Color.clear, 0.1f);
        }
        else if (HP > MaxHP / 2) //Good HP? Paint blue slowly
            HPBall.GetComponent<Renderer>().material.color = Color.Lerp(HPBall.GetComponent<Renderer>().material.color, Color.blue, 0.1f);
        else if (HP <= MaxHP / 2 && HP > MaxHP / 4) //half HP? Paint yellow slowly
            HPBall.GetComponent<Renderer>().material.color = Color.Lerp(HPBall.GetComponent<Renderer>().material.color, Color.yellow, 0.1f);
        else if (HP <= MaxHP / 4 && HP > 1) //quarter HP? Paint red slowly
            HPBall.GetComponent<Renderer>().material.color = Color.Lerp(HPBall.GetComponent<Renderer>().material.color, Color.red, 0.1f);
        else if (HP == 1) //1 HP? Danger!
            StartCoroutine(BlinkLowHP(Color.red)); //blink red light
    }

    public virtual void TurnUpdate() //inherited on UserPlayer and AIPlayer
    {
        if (actionPoints <= 0) //basically means if you did all the actions you could, no moves or attacks for you
        {
            moving = false; 
            attacking = false;
        }
    }

    IEnumerator BlinkLowHP(Color currentColor)
    {
        if (lit)
        {
            HPBall.GetComponent<Renderer>().material.color = Color.clear;
            yield return new WaitForSeconds(0.125f);
            lit = false;
        }
        else
        {
            HPBall.GetComponent<Renderer>().material.color = currentColor;
            yield return new WaitForSeconds(0.125f);
            lit = true;
        }

    }

    public void OnMouseEnter() //return index of player under mouse
    {
        if(BattleManager.instance.playerUnderMouseIndex != BattleManager.instance.players.IndexOf(this))
            BattleManager.instance.playerUnderMouseIndex = BattleManager.instance.players.IndexOf(this);
        BattleManager.instance.anyPlayerSelected = true;
    }

    public void OnMouseExit() //return (no player under mouse)
    {
        BattleManager.instance.anyPlayerSelected = false;
    }

    public virtual void SpecsUp()
    {


    }


    void OnTriggerEnter(Collider other)
    {
        if ( this.GetComponent<Player>().playerName == "Metaman" && (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy")) ){
            Debug.Log("Colidiu!");
            this.specs = other.GetComponent<Player>().specs;
            this.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = other.transform.GetChild(1).gameObject.GetComponent<Renderer>().material;
            this.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = new Color(0.9f, 0.9f, 0.5f);
            this.attackerCode = other.GetComponent<Player>().attackerCode;
        }
    }


    //auxiliar methods for animations
    public void characterisWalking()
    {

        kamibotAnimator.SetBool("IsWalking", true);
        kamibotAnimator.SetBool("IsIdle", false);
        kamibotAnimator.SetBool("IsInactive", false);
    }

    public void characterisAttacking()
    {

        kamibotAnimator.SetBool("IsAttacking", true);
        kamibotAnimator.SetBool("IsWalking", false);
        kamibotAnimator.SetBool("IsIdle", false);

        //what kind of kamibot is attacking? 0=aqua,1=slice,2=mantial,3=meta
        kamibotAnimator.SetInteger("Attacker", attackerCode);

    }

    public void characterisIdle()
    {
        kamibotAnimator.SetBool("IsIdle", true);
        kamibotAnimator.SetBool("IsWalking", false);
        kamibotAnimator.SetBool("IsAttacking", false);
        kamibotAnimator.SetBool("IsInactive", false);
        //this.characterIsWalking = false;
    }

    public void characterisInactive()
    {
        kamibotAnimator.SetBool("IsInactive", true);
        kamibotAnimator.SetBool("IsWalking", false);
        kamibotAnimator.SetBool("IsIdle", false);
    }

    public void characterisDead()
    {
        kamibotAnimator.SetBool("IsDead", true);
    }

    public void characterisTired()
    {
        kamibotAnimator.SetBool("IsTired", true);
    }

    public void characterisNotTired()
    {
        kamibotAnimator.SetBool("IsTired", false);
    }

    public void applyDamage()
    {
        BattleManager.instance.target.HP -= BattleManager.instance.amountOfDamage;
        BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].actionPoints = 0;

        if(this.playerName == "Sliceknight") AudioSource.PlayClipAtPoint(BattleManager.instance.sword, transform.position);
        if (this.playerName == "Mantial" || this.playerName == "Kinderbot") AudioSource.PlayClipAtPoint(BattleManager.instance.punch, transform.position);
    }

    public void ShootWater()
    {
        //spawn water
        AudioSource.PlayClipAtPoint(BattleManager.instance.shot, transform.position);
        GameObject aquashot = (GameObject)Instantiate(BattleManager.instance.waterShot, BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].arm.position, Quaternion.Euler(new Vector3(-180, 180, -180)));
        aquashot.transform.LookAt(new Vector3(BattleManager.instance.target.transform.position.x,
            BattleManager.instance.target.transform.position.y,
             BattleManager.instance.target.transform.position.z));
    }
}
