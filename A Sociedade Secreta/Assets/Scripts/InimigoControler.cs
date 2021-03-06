﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoControler : MonoBehaviour
{
    private Goblin goblin;
    public CameraShake cameraShake;

    [Header("Configuraçao de Vida")]
    public int vidaInimigo;
    public int vidaAtual;
    public GameObject barraVida;
    public Transform hpBar;
    public float percVida;
    public GameObject danoTXTPrefab;

    [Header("Configuraçao Resistencia")]
    //configuração de danos para RPG fogo - agua  - normal 
    public float[] ajusteDano;
    public bool olhandoEsquerda, playerEsquerda;
    private GameControler gameControler;
    //private SpriteRenderer sRender;
    [Header("Configuraçao de Knockback")]
    private HeroiController heroiController;
    public GameObject knockbackForcedPrefab;
    public Transform knockbackPosition;
    private Animator animator;
    public float knockbackX;
    private float kX;
    private bool died;
    [Header("Configuraçao de Chão")]
    public Transform groundCheck;
    public LayerMask whatIsGround;
    [Header("Configuraçao de loot")]
    public GameObject[] loots;
    public bool getHit;// indica se tomou um Hit
    public Color[] characterColor;//controle de cor do personagem
    // Start is called before the first frame update


    //public Rigidbody2D rigidbody2D;
    void Start()
    {
        vidaAtual = vidaInimigo;
        percVida = vidaInimigo;
        hpBar.localScale = new Vector3(1,1,1);
        //barraVida.SetActive(false);
        //rigidbody2D = GetComponent<Rigidbody2D>();
        gameControler = GameControler.getInstance();
        heroiController=FindObjectOfType(typeof(HeroiController)) as HeroiController;
        cameraShake = FindObjectOfType(typeof(CameraShake)) as CameraShake;
        goblin=FindObjectOfType(typeof(Goblin)) as Goblin;
        animator = GetComponent<Animator>();

        //sRender = GetComponent<SpriteRenderer>();
        //if(sRender == null){
        //    sRender = new SpriteRenderer();
        //}
        //sRender.color = characterColor[0];
        Util.changeColor(this.gameObject, characterColor[0]);
      /*  if(olhandoEsquerda){
            float x = transform.localScale.x;
            x *= -1;
            transform.localScale = new Vector3(x,transform.localScale.y,transform.localScale.z);
            float x2 = barraVida.transform.localScale.x;
            x2 *= -1;
            barraVida.transform.localScale = new Vector3(x2, barraVida.transform.localScale.y, barraVida.transform.localScale.z);
        } */
        knockbackPosition.localPosition = new Vector3(knockbackX, knockbackPosition.localPosition.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float xPlayer = heroiController.transform.position.x;
        
        
        if(xPlayer < transform.position.x){
            playerEsquerda = true;
            kX = knockbackX;
            //Debug.Log("Player esta a equerda");
        } else {
            playerEsquerda = false;
            kX = knockbackX * -1;
            //Debug.Log("Player esta a direita");
        }

        if(olhandoEsquerda && playerEsquerda){
            kX = knockbackX;
        } else if(!olhandoEsquerda && playerEsquerda){
            kX = knockbackX * -1;
        } else if(olhandoEsquerda && !playerEsquerda){
            kX = knockbackX * -1;
        } else if(!olhandoEsquerda && !playerEsquerda){
            kX = knockbackX;
        }

        knockbackPosition.localPosition = new Vector3(kX, knockbackPosition.localPosition.y, 0);
        animator.SetBool("groundead", true);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log(other.gameObject.tag);
        if(died == true){return;}
        switch (other.gameObject.tag)
        {
            case "Arma":
            if(!getHit){
                //Debug.Log("CHAMOU INTERACAO");
                cameraShake.SendMessage("interacao",SendMessageOptions.DontRequireReceiver);
                getHit = true;
                barraVida.SetActive(true);
                ArmaInfo armaInfo = other.gameObject.GetComponent<ArmaInfo>();
                float danoArma = Random.Range(armaInfo.danoMin,armaInfo.danoMax+1);
                float danoTomado = danoArma + (danoArma * (ajusteDano[armaInfo.tipoDanoTomado])/100);
                animator.SetTrigger("hit");
                vidaAtual -= Mathf.RoundToInt(danoTomado);

                percVida = (float)vidaAtual / (float)vidaInimigo;
                
                if(percVida < 0) {
                    percVida = 0;
                } 
                hpBar.localScale = new Vector3(percVida,1,1);
                if(vidaAtual <= 0){
                    goblin.estadoAtual = EstadoInimigo.MORTO;
                    died = true;
                    animator.SetTrigger("morreu");
                    //animator.SetInteger("idAnimation",3);
                    this.gameObject.layer = 15; //layer 15 estado morto não escosta em ninguem
                    StartCoroutine("loot");
                }
                GameObject danoTemp = Instantiate(danoTXTPrefab, transform.position, transform.localRotation);
                danoTemp.GetComponent<TextMesh>().text =Mathf.RoundToInt(danoTomado).ToString();
                danoTemp.GetComponent<MeshRenderer>().sortingLayerName = "HUD";

                GameObject fxTemp = Instantiate(gameControler.fxDano[armaInfo.tipoDanoTomado], transform.position, transform.localRotation);
                Destroy(fxTemp, 1f);

                int forcaX = 50;
                if(!playerEsquerda){ forcaX *= -1;}
                danoTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2(forcaX, 300));

                StartCoroutine("invulneravel");
//                Debug.Log("Dano Tomado"+ danoTomado+" do Tipo:"+gameControler.tiposDano[armaInfo.tipoDanoTomado]);
                GameObject knockbackTemp = Instantiate(knockbackForcedPrefab, knockbackPosition.position, knockbackPosition.localRotation);
                ///if(heroiController.)
                Destroy(danoTemp, 1f);
                Destroy(knockbackTemp, 0.03f);
                // Chama o metodo do outro script
				this.gameObject.SendMessage ("TomeiHit", SendMessageOptions.DontRequireReceiver);
                
            }
               
            break;
        }
    }

    public void teste(){
         GameObject knockbackTemp = Instantiate(knockbackForcedPrefab, knockbackPosition.position, knockbackPosition.localRotation);
                ///if(heroiController.)
                Destroy(knockbackTemp, 0.03f);
    }
    void Flip(){ 
        olhandoEsquerda = !olhandoEsquerda;
        float x = transform.localScale.x;
        x *= -1;
        transform.localScale = new Vector3(x,transform.localScale.y,transform.localScale.z);

        float x2 = barraVida.transform.localScale.x;
        x2 *= -1;
        barraVida.transform.localScale = new Vector3(x2, barraVida.transform.localScale.y, barraVida.transform.localScale.z);
    }

    IEnumerator loot(){
        yield return new WaitForSeconds(2f);
        GameObject fxMorte = Instantiate(gameControler.fxMorte, groundCheck.position, transform.localRotation);
        yield return new WaitForSeconds(0.5f);
        //sRender.enabled = false;
        Util.enableAndDisable(this.gameObject, false);

        // controle loot
        int qtdMoedas = Random.Range(1,4);
        for (int i = 0; i < qtdMoedas; i++)
        {
            GameObject lootTemp = Instantiate(loots[0], transform.position, transform.localRotation);
            lootTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-50,50), 100) );
            yield return new WaitForSeconds(0.2f);
        }
        int qtdFlexas = Random.Range(0,2);
        for (int i = 0; i < qtdMoedas; i++)
        {
            GameObject lootTemp = Instantiate(loots[1], transform.position, transform.localRotation);
            lootTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-50,50), 100) );
            yield return new WaitForSeconds(0.2f);
        }

        
        yield return new WaitForSeconds(2f);
        Destroy(fxMorte);
        Destroy(this.gameObject);
    }



    IEnumerator invulneravel(){
        Util.changeColor(this.gameObject, characterColor[1]);
        yield return new WaitForSeconds(0.2f);
        Util.changeColor(this.gameObject, characterColor[0]);
        yield return new WaitForSeconds(0.2f);
        Util.changeColor(this.gameObject, characterColor[1]);
        yield return new WaitForSeconds(0.2f);
        Util.changeColor(this.gameObject, characterColor[0]);
        yield return new WaitForSeconds(0.2f);
        Util.changeColor(this.gameObject, characterColor[1]);
        yield return new WaitForSeconds(0.2f);
        Util.changeColor(this.gameObject, characterColor[0]);
        yield return new WaitForSeconds(0.2f);
        getHit = false;
        barraVida.SetActive(false);
    }
}
