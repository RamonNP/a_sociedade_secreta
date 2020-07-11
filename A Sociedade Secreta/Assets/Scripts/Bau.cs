using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bau : MonoBehaviour
{
    private GameControler gameControler;
    private SpriteRenderer spriteRenderer;
    public Sprite[] imagemObjeto;
    public bool open;
    // Start is called before the first frame update
    void Start()
    {
        //gameControler = FindObjectOfType(typeof(GameControler)) as GameControler;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

   public void interacao() {
       if(!open) {
           open = true;
           spriteRenderer.sprite = imagemObjeto[1];
           getGameControler().moedas +=1;
       }
   }

   private GameControler getGameControler(){
       if(gameControler == null){ 
           gameControler = FindObjectOfType(typeof(GameControler)) as GameControler;
       }
       return gameControler;
   }
}
