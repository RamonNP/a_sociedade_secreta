using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroiController : MonoBehaviour
{
    private GameControler gameController;
    public GameObject trail;

    [Header("Animaçoes")]
    private Animator playerAnimator; 
    public bool grounded;
    public LayerMask oQueEChao;
    public bool attacking; 
    public int idAnimation;
    public GameObject objetoAnimatorBody;
    public Rigidbody2D playerRb;
    public Sprite[] olhos;
    public GameObject olhoAtual;


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
    [Header("Banco de Dados Arma")]

    public GameObject[] armas;

    public GameObject arma;

    // Start is called before the first frame update
    void Start()
    {
        trail.SetActive(false);
        balaoAlerta.SetActive(false);
        foreach (var item in armas)
        {
           item.SetActive(false);
        }
        playerAnimator = objetoAnimatorBody.GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
        gameController = GameControler.getInstance();
        trocarArma(gameController.idArma);
    }

    private void FixedUpdate()
    {
        // Cancela comandos se nao estiver no gameplay
        if (gameController.estadoAtual != GameState.GAMEPLAY)
        {
            return;
        }
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f, oQueEChao);
        playerRb.velocity = new Vector2(h * speed, playerRb.velocity.y);
        interagir();
    }

    // Update is called once per frame
    void Update()
    {
        // Cancela comandos se nao estiver no gameplay
        if (gameController.estadoAtual != GameState.GAMEPLAY)
        {
            return;
        }
       movimentos();
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

    private void movimentos(){
         if(attacking){
            trail.SetActive(true);
        } else {
            trail.SetActive(false);
        }
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
        //print(playerRb.velocity.y);
        playerAnimator.SetFloat("speedY", playerRb.velocity.y);
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
//    Debug.Log(other.gameObject.tag);
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
            case "Interacao":
                interacao = null;
                balaoAlerta.SetActive(true);
                interacao = other.gameObject;
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
            case "Interacao":
                interacao = null;
                balaoAlerta.SetActive(false);
                interacao = null;
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
                interacao = other.gameObject;
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
    public void controleArma(int id){
        //Debug.Log("controleArma"+id);
        foreach (var item in armas)
        {
           item.SetActive(false);
        }
        armas[id].SetActive(true);
    }

      public void hurt(int hurt){
        olhoAtual.GetComponent<SpriteRenderer>().sprite = olhos[hurt];
    }

    void LateUpdate() {
        if(gameController.idArma != gameController.idArmaAtual){
            trocarArma(gameController.idArma);
        }
    }
    public void trocarArma(int id) {
        gameController.idArma = id;
        arma.GetComponent<SpriteRenderer>().sprite = gameController.Armas[id];
        gameController.idArmaAtual = gameController.idArma;
    }
}
