using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocaoVida : MonoBehaviour
{
    
    private GameControler gameControler;

    /// <summary>idArma;>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        gameControler =  FindObjectOfType(typeof(GameControler)) as GameControler;
    }
   public void interacao() {
       Debug.Log(gameControler.vidaAtual);
       if(gameControler.vidaAtual < gameControler.vidaMaxima) {
           gameControler.vidaAtual = gameControler.vidaAtual +1;
       }
       Destroy(this.gameObject);
   }
}
