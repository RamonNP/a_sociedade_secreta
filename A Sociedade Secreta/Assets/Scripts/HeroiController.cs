using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroiController : MonoBehaviour
{
    private XmlLerDados xmlLerDados;
    private GameControler gameController;
    private AudioController audioController;
    public GameObject trail;
    public Joystick joystick;

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
    public GameObject canvasConversa;
    public Text caixaTexto;
    private bool naoPodeAtacar;          // Indica se podemos executar um ataque

    public Transform groundCheck; //Responsavel por checar se o personagem esta no xao
    public bool lookLeft;
    public float speed;
    public float jumpForce;

    public GameObject balaoAlerta;
    private float h, v;

    public Collider2D standing, crounching;
    public GameObject hands;
    public GameObject interacao; 
    public LayerMask interacaoMasck;
    public Vector3 dir = Vector3.right;
    [Header("Banco de Dados Arma")]

    public GameObject[] armas;

    public GameObject arma;
    public GameObject arco;

    public GameObject hand;

    public Transform spawnFlecha;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            gameController = GameControler.getInstance();
            xmlLerDados = XmlLerDados.getInstance();
            trail.SetActive(false);
        }
        catch (System.Exception)
        {
            
            throw;
        }
        balaoAlerta.SetActive(false);
        foreach (var item in armas)
        {
           item.SetActive(false);
        }
        if(arco != null){
            arco.SetActive(false);
        }
        playerAnimator = objetoAnimatorBody.GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
        if(xmlLerDados !=null){
            xmlLerDados.LoadDialogoData(gameController.idiomaFolder[gameController.idioma] + "/" + gameController.nomeArquivoXml); //ler o arquivo interação com itens;
        }
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
    {   if(gameController.estadoAtual == GameState.DIALOGO && Input.GetButtonDown("Fire1"))
        {
            playerRb.velocity = new Vector2(0, playerRb.velocity.y);
            playerAnimator.SetInteger("idAnimation", 0);
            interacao.SendMessage("falar", SendMessageOptions.DontRequireReceiver);
        }
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
        if(joystick!=null){
            h = joystick.Horizontal;//Input.GetAxisRaw("Horizontal");
        } else {
            h = Input.GetAxisRaw("Horizontal");
        }
        //v = joystick.Vertical;//Input.GetAxisRaw("Vertical");
        
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
        Debug.DrawRay(arma.gameObject.transform.position, dir * 0.1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(arma.gameObject.transform.position, dir, 0.1f, interacaoMasck);
        
        if(hit == true)
        {
            interacao = hit.collider.gameObject;
            balaoAlerta.SetActive(true);
        }
        else
        {
            interacao = null;
            balaoAlerta.SetActive(false);
        }
    }

    public void jump() {
        if (grounded && !attacking)
        {
            playerRb.AddForce(new Vector2(0, jumpForce));
        }
    }

    public void btnAtacar() {
        if (v>=0 && !attacking)
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
    }
    public void btnAtacarFlexa() {
        if (v >= 0 && !attacking)
        {
            if(interacao == null){
                playerAnimator.SetTrigger("flexa");
            }
        }
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
            case "Coletavel":
                other.gameObject.SendMessage("coletar", 1, SendMessageOptions.DontRequireReceiver);
            break;
            case "FlexaColetavel":
                other.gameObject.SendMessage("coletar", 2, SendMessageOptions.DontRequireReceiver);
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
        //Debug.Log(other.gameObject.tag);

        switch (other.gameObject.tag)
        {
            case "FlexaColetavel":
                other.gameObject.SendMessage("coletar", 2, SendMessageOptions.DontRequireReceiver);
                Destroy(other.gameObject);
            break;
            case "Buraco":
                gameController.morrer();
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

    public void mostrarMensagem(object item) {
        idMensagem = (string) item;
        StartCoroutine("dialogar");
    }

    private string idMensagem;
    IEnumerator dialogar() {
        print("Chave"+idMensagem+"Tamanho - "+xmlLerDados.mensagemInteracao.Count);
        caixaTexto.text = xmlLerDados.mensagemInteracao[idMensagem];
        print(caixaTexto.text);
        canvasConversa.SetActive(true);
        yield return new WaitForSeconds(4);
        canvasConversa.SetActive(false);
    }

    public void AttackFlecha (int atk)
    {
        switch (atk)
        {
            case 0:
            {
                //Debug.Log("0 - "+"EsperarNovoAtaque");
                attacking = false;
                //arcos[2].SetActive (false);
                StartCoroutine ("EsperarNovoAtaque");
                break;
            }

            case 1:
            {
                //Debug.Log("1 - "+"atack- true");
                attacking = true;
                break;
            }

            case 2:
            {
                // Instancia flecha
                //if (gameController.quantidadeFlechas[gameController.idFlechaEquipada] > 0)
                //Debug.Log("1 - "+"atack- true");
                print(gameController.quantidadeFlechas);
                if (gameController.quantidadeFlechas > 0)
                {

                    //audioController.TocarEfeito (audioController.efeitoArco, 1f);
                    gameController.quantidadeFlechas--;
                    GameObject flechaTemp = Instantiate (gameController.flechaPrefabs[gameController.idFlechaEquipada], spawnFlecha.position, spawnFlecha.localRotation);
                    flechaTemp.transform.localScale = new Vector3 (flechaTemp.transform.localScale.x * dir.x, flechaTemp.transform.localScale.y, flechaTemp.transform.localScale.z);
                    flechaTemp.GetComponent<Rigidbody2D>().velocity = new Vector2 (gameController.velocidadesFlecha[gameController.idFlechaEquipada] * dir.x, 0);
                    Destroy (flechaTemp, 2f);
                }

                break;
            }
            
            default:
            {
                break;
            }
        }
    }

    private IEnumerator EsperarNovoAtaque ()
    {
        yield return new WaitForSeconds (0.2f);
        naoPodeAtacar = false;
    }

}
