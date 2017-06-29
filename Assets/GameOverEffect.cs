using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverEffect : MonoBehaviour {

    // Use this for initialization
    public GameObject Image;
    public GameObject Game;
    public GameObject Over;
    public GameObject Retry;
    public GameObject Title;

    public Sprite RetrySprite1;
    public Sprite RetrySprite2;
    public Sprite TitleSprite1;
    public Sprite TitleSprite2;


    public GameObject Boom;
    public GameObject Charge;

    public GameObject Player;
    public GameObject L_Joint;
    public GameObject R_Joint;
    public GameObject L_Wing;
    public GameObject R_Wing;

    public GameObject PlayerCadaver;
    public float Speed = 0.01f;



    private float time;
    private Vector3 TargetPosition;

    private bool IsOverCharge = false;
    private bool IsOverChargeEffect = false;
    private bool IsOverBoomEffect = false;
    private bool IsOverCADEFFect = false;
    private bool IsMoveTextOver = false;
    private bool IsRetry = true;
    private bool IsTitle = false;


    private Transform[] CADChlidren;
    private StartMove[] ChildrenMove;
    //private List<Vector3> CADVE;
    private bool IsSetOver=false;


    private Vector3 EFTStartPos;
    private Quaternion EFTStartRot;
    private GameObject Anim;
    void Start () {

        CADChlidren = PlayerCadaver.GetComponentsInChildren<Transform>();
        ChildrenMove = PlayerCadaver.GetComponentsInChildren<StartMove>();

        PlayerCadaver.SetActive(false);

        //CADVE = new List<Vector3>();

        Retry.GetComponent<Image>().sprite = RetrySprite1;
        Title.GetComponent<Image>().sprite = TitleSprite1;
    }
	
	// Update is called once per frame
	void Update () {
        Move();
	}
    void Move()
    {
        if (Main.IsGameOver)
        {
            Select();
            DeathPlayerCharge();
            DestroyPlayerColor();
            DestroyEffect();
            CadaverEffect();
            MoveText();
        }
    }
    void MoveText()
    {
        if (IsOverBoomEffect && IsOverCADEFFect && !IsMoveTextOver)
        {
            Image.GetComponent<Image>().enabled = true;
            var GAME = Game.GetComponent<RectTransform>().localPosition;
            var OVER = Over.GetComponent<RectTransform>().localPosition;

            if (GAME.y <= 100) Game.GetComponent<RectTransform>().localPosition += new Vector3(0, 10, 0);
            if (OVER.y <= 100 && GAME.y >= 100) Over.GetComponent<RectTransform>().localPosition += new Vector3(0, 10, 0);


            var RE = Retry.GetComponent<RectTransform>().localScale;
            var TI = Title.GetComponent<RectTransform>().localScale;

            if (GAME.y >= 100 && OVER.y >= 100 && 
                RE.x < 1 && RE.y < 1 && RE.z < 1 &&
                TI.x < 1 && TI.y < 1 && TI.z < 1)
            {
                Retry.GetComponent<RectTransform>().localScale += new Vector3(0.1f, 0.1f, 0.1f);
                Title.GetComponent<RectTransform>().localScale += new Vector3(0.1f, 0.1f, 0.1f);
            }
            else if (GAME.y >= 100 && OVER.y >= 100 &&
                RE.x >= 1 && RE.y >= 1 && RE.z >= 1 &&
                TI.x >= 1 && TI.y >= 1 && TI.z >= 1)
            {
                IsMoveTextOver = true;
            }
                

        }
       
    }
    void Select()
    {
        if (IsMoveTextOver)
        {
            float y = Input.GetAxis("Vertical");


            if (IsRetry && y < -0.5f) {
                IsRetry = false;  IsTitle = true;
            }
            else if (IsTitle && y > 0.5f){
                IsRetry = true;    IsTitle = false;
            }

            NextSecen();
        }
    }
    void NextSecen()
    {
        if (IsRetry)
        {
            Retry.GetComponent<Image>().sprite = RetrySprite2;
            Title.GetComponent<Image>().sprite = TitleSprite1;

            if (Input.GetKey(KeyCode.JoystickButton0)) {
                Main.IsGameOver = false;
                SceneManager.LoadScene("Main");  }

        }
        else if (IsTitle)
        {
            Retry.GetComponent<Image>().sprite = RetrySprite1;
            Title.GetComponent<Image>().sprite = TitleSprite2;

            if (Input.GetKey(KeyCode.JoystickButton0)) {
                Main.IsGameOver = false;
                SceneManager.LoadScene("Title");  }
        }
    }
    void DeathPlayerCharge()
    {
        
        EFTStartPos = Player.transform.position;
        EFTStartRot = Player.transform.rotation;

        Player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

        var L_wing = L_Wing.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < L_wing.Length; i++)
            L_wing[i].color = new Color(1, 1, 1, 1);

        var R_wing = R_Wing.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < R_wing.Length; i++)
            R_wing[i].color = new Color(1, 1, 1, 1);

        Player.GetComponent<Player>().enabled = false;
        Player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        L_Joint.GetComponent<Joint>().enabled = false;
        R_Joint.GetComponent<Joint>().enabled = false;

        L_Wing.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        R_Wing.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;


        Player.GetComponent<CircleCollider2D>().enabled = false;
        L_Joint.GetComponentInChildren<BoxCollider2D>().enabled = false;
        R_Joint.GetComponentInChildren<BoxCollider2D>().enabled = false;


        
        if (!IsOverCharge)
        {
            Anim = Instantiate(Charge, EFTStartPos, EFTStartRot);
            Anim.GetComponent<Animator>().SetTrigger("PlayerDeath");
            IsOverCharge = true;
        }
        
       if(Anim==null)
            IsOverChargeEffect = true;
       
    }
    void DestroyPlayerColor()
    {
        if (IsOverChargeEffect)
        {

            Player.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);

            var L_wing = L_Wing.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < L_wing.Length; i++)
                L_wing[i].color = new Color(0, 0, 0, 0);

            var R_wing = R_Wing.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < R_wing.Length; i++)
                R_wing[i].color = new Color(0, 0, 0, 0);

            var Trail = GameObject.FindGameObjectsWithTag("Trail");
            for (int i = 0; i < Trail.Length; i++)
                Trail[i].GetComponent<TrailRenderer>().enabled = false;
        }
}
    void DestroyEffect()
    {
        if (IsOverBoomEffect == false && IsOverChargeEffect)
        {
            

            for (int i = 0; i <9; i++)
                Instantiate(Boom, EFTStartPos + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), EFTStartRot);

            for (int i=0; i < CADChlidren.Length; i++)
            {
                //CADVE.Add(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
                CADChlidren[i].position = EFTStartPos;
                CADChlidren[i].rotation = Quaternion.Euler(0, 0, Random.Range(-360, 360));
            }

            IsOverBoomEffect = true;
        }
        
    }

    void CadaverEffect()
    {
        if (IsOverBoomEffect)
        {
            if (!IsSetOver)
            {
                for (int i = 0; i < ChildrenMove.Length; i++)
                    ChildrenMove[i].SetTargetPos(EFTStartPos + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f)));
                IsSetOver = true;
            }
           
            PlayerCadaver.SetActive(true);

            for (int i = 0; i < ChildrenMove.Length; i++)
                if (ChildrenMove[i] == null) IsOverCADEFFect = true;

        }

        
    }
}
