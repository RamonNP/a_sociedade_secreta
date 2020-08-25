using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaInfo : MonoBehaviour
{
    private GameControler gameControler;
    public float danoMax;
    public float danoMin;
    public int tipoDanoTomado;
    public int idArma;

    /// <summary>idArma;>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        gameControler =  FindObjectOfType(typeof(GameControler)) as GameControler;
    }
   public void interacao() {
       gameControler.idArma = idArma;
       Destroy(this.gameObject);
   }
}
