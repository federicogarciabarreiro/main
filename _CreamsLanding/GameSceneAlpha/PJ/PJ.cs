using UnityEngine;
using System.Collections;

public class PJ : MonoBehaviour
{

    public LayerMask floorMask; 
    public Transform floorTester;
    private Animator animator;

    private float radio = 0.10f; 
    public float speed = 7f;
    public float jump = 10f;

    public GameObject prefabBullet;
    public Transform pointToShoot;
    public GameObject soundCoins;

    private bool inGrounded = true;
    private bool doubleJump = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        animator.SetFloat("VelX", GetComponent<Rigidbody2D>().velocity.x);
       
        animator.SetBool("enSuelo", inGrounded);
       
        inGrounded = Physics2D.OverlapCircle(floorTester.position, radio, floorMask);
        
        if (inGrounded)
                 {
            doubleJump = false;
                 }
    }

    void Update()
    {
        if (GetComponent<Rigidbody2D>().velocity.x == 0f)
             { if (Input.GetMouseButtonDown(1))
                 {
                GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
                 }
             }
    
        if (Input.GetKeyDown(KeyCode.Space))

        {
            
            if (inGrounded || !doubleJump)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jump);
                GetComponent<AudioSource>().Play();

                if (!inGrounded && !doubleJump)
                {
                    doubleJump = true;
                }
            }

        }

        if (Time.timeScale != 0 && Time.timeScale != 0.0000001f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }

       

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "BlueItem" || collider.tag == "RedItem")
        {
            soundCoins.GetComponent<AudioSource>().Play();

        }
    }

    void Shoot()
    {
        GameObject myBullet = GameObject.Instantiate(prefabBullet);
        myBullet.transform.position = pointToShoot.transform.position;
        myBullet.transform.up = pointToShoot.transform.right;
        this.gameObject.GetComponent<Score>().counter = this.gameObject.GetComponent<Score>().counter - 1f;
       }

}

