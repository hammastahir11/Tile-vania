using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput; 

public class Player : MonoBehaviour
{
    //Config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);



    //state
    bool isAlive = true;
    //public static Vector2 Respawnpoint = new Vector2(-21, 4);


    //Cached component refrence
    Rigidbody2D myrigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider2D;
    BoxCollider2D myFeet;
    float gravityScaleAtStart;




    void Start()
    {
        myrigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myrigidbody.gravityScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            return;
        }
        Run();
        climbLadder();
        Jump();
        FlipSprite();
        Die();
        
        
    }

   private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow *runSpeed, myrigidbody.velocity.y);
        myrigidbody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(myrigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
         

    }
    private void climbLadder()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myAnimator.SetBool("Climbing", false);
            myrigidbody.gravityScale=gravityScaleAtStart;
            return;
        }

        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myrigidbody.velocity.x, controlThrow * climbSpeed);
        myrigidbody.velocity = climbVelocity;
        myrigidbody.gravityScale = 0f;
        bool playerHasVerticalSpeed = Mathf.Abs(myrigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
    }

    private void Jump()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return; 
        }
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd=new Vector2(0f,jumpSpeed);
            myrigidbody.velocity += jumpVelocityToAdd;
        }
    }

    private void Die()
    {
        if (myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards"))){

             isAlive = false;
            myAnimator.SetTrigger("Dying");
            
            GetComponent<Rigidbody2D>().velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
          //  GameObject.FindGameObjectWithTag("Newplayer").transform.position = Respawnpoint;

        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myrigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myrigidbody.velocity.x), 1f);        
        }
    }

}
