using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    private GameControler gameControler;
    private HeroiController playScript;
    public GameObject vida1;
    public GameObject vida2;
    public GameObject vida3;
    public Text txtGold;

    public 
    // Start is called before the first frame update
    void Start()
    {
        playScript = FindObjectOfType(typeof(HeroiController)) as HeroiController;
        gameControler = FindObjectOfType(typeof(GameControler)) as GameControler;
    }

    // Update is called once per frame
    void Update()
    {
        controleVida();
        txtGold.text = gameControler.gold.ToString("N0").PadLeft(4,'0').Replace(",",".");
    }

    private void controleVida(){
        if(playScript.vidaAtual == 3){
            vida1.SetActive(true);
            vida2.SetActive(true);
            vida3.SetActive(true);
        } else if((playScript.vidaAtual == 2)){
            vida1.SetActive(true);
            vida2.SetActive(true);
            vida3.SetActive(false);
        } else if((playScript.vidaAtual == 1)){
            vida1.SetActive(true);
            vida2.SetActive(false);
            vida3.SetActive(false);
        } else if((playScript.vidaAtual == 0)){
            vida1.SetActive(false);
            vida2.SetActive(false);
            vida3.SetActive(false);
        }
    }
}
