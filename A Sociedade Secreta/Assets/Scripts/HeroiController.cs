using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroiController : MonoBehaviour
{
    private GameControler gameControler;
    //sistemas de armas
    public GameObject[] armas;

    public int vidaMaxima;
    public int vidaAtual;

    private Animator playerAnimator; 
    public Rigidbody2D playerRb;

    public bool grounded;
    public bool attacking; 
    public int idAnimation;
    public Transform groundCheck; //Responsavel por checar se o personagem esta no xao
    public bool lookLeft;
    public float speed;
    public float jumpForce;

    public GameObject balaoAlerta;
    private float h, v;

    public Collider2D standing, crounching;
    private Vector3 dir = Vector3.right;
    public GameObject hands;
    public GameObject interacao; 

    // Start is called before the first frame update
    void Start()
    {
        balaoAlerta.SetActive(false);
        foreach (var item in armas)
        {
           item.SetActive(false);
        }
        playerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
        gameControler = FindObjectOfType(typeof(GameControler)) as GameControler;
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
        playerRb.velocity = new Vector2(h * speed, playerRb.velocity.y);
        interagir();
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
            if(interacao == null){
                playerAnimator.SetTrigger("atacar");
            } else if(interacao != null) {
                if(interacao.tag.Equals("Porta")){
                    interacao.GetComponent<Porta>().tPlayer = this.transform;
                }
                interacao.SendMessage("interacao",SendMessageOptions.DontRequireReceiver);
            }
            //interacao.SendMessage("interacao",SendMessageOptions.DontRequireReceiver);
        }
        if (Input.GetButtonDown("Jump") && grounded && !attacking)
        {
//            Debug.Log(jumpForce);
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
            dir.x = x;
            //dir = dir;
        }
    }

    public void atack(int atk)
    {
        if(atk==0 )
        {
            attacking = false;
            armas[armas.Length-1].SetActive(false);
        }else if (atk == 1)
        {
            attacking = true;
        }
    }

    private void interagir()
    {
        RaycastHit2D hit = Physics2D.Raycast(hands.transform.position, dir, 5f);
        Debug.DrawRay(hands.transform.position, dir * 5f, Color.black);
    }
/// <summary>
/// Sent when another object enters a trigger collider attached to this
/// object (2D physics only).
/// </summary>
/// <param name="other">The other Collider2D involved in this collision.</param>
void OnTriggerEnter2D(Collider2D other)
{
    Debug.Log(other.gameObject.tag);
    switch (other.gameObject.tag)
        {
            case "Bau":
                interacao = other.gameObject;
                Bau bau = other.gameObject.GetComponent<Bau>();
                //if(!bau.open){
                    balaoAlerta.SetActive(true);
                //}
            break;
            case "Coletavel":
                other.gameObject.SendMessage("coletar", SendMessageOptions.DontRequireReceiver);
            break;
            case "Porta":
                interacao = other.gameObject;
                balaoAlerta.SetActive(true);
            break;
            default:
            break;
        }
}
private void OnTriggerExit2D(Collider2D other) {
     switch (other.gameObject.tag)
        {
            case "Bau":
                //interacao = other.gameObject;
               // Bau bau = other.gameObject.GetComponent<Bau>();
                //if(!bau.open){
                    //balaoAlerta.SetActive(true);
                //}
            break;
            case "Coletavel":
                //other.gameObject.SendMessage("coletar", SendMessageOptions.DontRequireReceiver);
            break;
            case "Porta":
                interacao = null;
                balaoAlerta.SetActive(false);
            break;
            default:
            break;
        }
}

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
//        Debug.Log(other.gameObject.name +other.gameObject.tag);

        switch (other.gameObject.tag)
        {
            case "Bau":
                interacao = other.gameObject;
                Bau bau = other.gameObject.GetComponent<Bau>();
               // if(!bau.open){
                    balaoAlerta.SetActive(true);
               // }
            break;
            case "Coletavel":
                Destroy(other.gameObject);
            break;
            case "Interacao":
                balaoAlerta.SetActive(true);
            break;
            default:
            break;
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        //Debug.Log(collision.gameObject.name+collision.gameObject.tag);
        if(collision.gameObject.tag == "Bau")
        {
            interacao = null;
            balaoAlerta.SetActive(false);
        }
        if(collision.gameObject.tag == "Interacao")
        {
            balaoAlerta.SetActive(false);
        }
    }
    void controleArma(int id){
        //Debug.Log("controleArma"+id);
        foreach (var item in armas)
        {
           item.SetActive(false);
        }
        armas[id].SetActive(true);
    }
}
