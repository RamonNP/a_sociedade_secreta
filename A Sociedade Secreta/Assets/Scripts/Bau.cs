using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bau : MonoBehaviour
{

    private HeroiController heroiController;
    private GameControler gameControler;
    private SpriteRenderer spriteRenderer;
    public Sprite[] imagemObjeto;
    public bool open;
    public GameObject[] loots;
    // Start is called before the first frame update
    void Start()
    {
        //HeroiController
        heroiController = FindObjectOfType(typeof(HeroiController)) as HeroiController;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

   public void interacao() {
        Hud hud = FindObjectOfType(typeof(Hud)) as Hud;
        if(!open) {
            if(!hud.temChave) {
                heroiController.SendMessage("mostrarMensagem","NaoBau",SendMessageOptions.DontRequireReceiver); //SendMessage não tiver a chave avisa o heroicontroler
                return;
            }
            open = true;
            spriteRenderer.sprite = imagemObjeto[1];
            StartCoroutine("gerarLoot");
            GetComponent<Collider2D>().enabled = false;
        }
   }

IEnumerator gerarLoot() {
     // controle loot
    int qtdMoedas = Random.Range(1,200);
    for (int i = 0; i < qtdMoedas; i++)
    {
        int rand = 0;
        rand = Random.Range(0, 1);
        GameObject lootTemp = Instantiate(loots[rand], transform.position, transform.localRotation);
        lootTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-50,50), 100) );
        yield return new WaitForSeconds(0.2f);
    }
}
   private GameControler getGameControler(){
       if(gameControler == null){ 
           gameControler = GameControler.getInstance();
       }
       return gameControler;
   }
}
