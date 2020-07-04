using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroiController : MonoBehaviour
{


    private Animator playerAnimator;
    public Rigidbody2D playerRb;

    public bool grounded;
    public bool attacking;
    public int idAnimation;
    public Transform groundCheck; //Responsavel por checar se o personagem esta no xao
    public bool lookLeft;
    public float speed;
    public float jumpForce;

    private float h, v;

    public Collider2D standing, crounching;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
        playerRb.velocity = new Vector2(h * speed, playerRb.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        
        if (h > 0 && lookLeft == true)
        {
            flip();
        } else if(h < 0 && lookLeft == false)
        {
            flip();
        }

        if(v < 0)
        {
            idAnimation = 2;
            if (grounded)
            {
                h = 0;
            }
        } else
        {
            if(h != 0)
            {
                idAnimation = 1;
            }else
            {
                idAnimation = 0;
            }

        }

        if (Input.GetButtonDown("Fire1") && v>=0 && !attacking)
        {
            playerAnimator.SetTrigger("atacar");
        }
        if (Input.GetButtonDown("Jump") && grounded && !attacking)
        {
            Debug.Log(jumpForce);
            playerRb.AddForce(new Vector2(0, jumpForce));
            //playerAnimator.SetTrigger("atacar");
        }

        if (attacking && grounded)
        {
            h = 0;
        }

        if(grounded)
        {
            if(v < 0 )
            {
                crounching.enabled = true;
                standing.enabled = false;
            } else
            {
                crounching.enabled = false;
                standing.enabled = true;
            }
        } else if (v != 0)
        {
            crounching.enabled = false;
            standing.enabled = true;
        }

        playerAnimator.SetBool("groundead",grounded);
        playerAnimator.SetInteger("idAnimation", idAnimation);
        playerAnimator.SetFloat("speedY", playerRb.velocity.y);
    }

    void flip()
    {
        if (!attacking)
        {
            lookLeft = !lookLeft;
            float x = transform.localScale.x;
            x *= -1;
            transform.localScale = new Vector3(x,transform.localScale.y,transform.localScale.z);
        }
    }

    public void atack(int atk)
    {
        if(atk==0 )
        {
            attacking = false;
        }else if (atk == 1)
        {
            attacking = true;
        }
    }
}
