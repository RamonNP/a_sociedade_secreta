using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    private GameControler gameControler;
    public GameObject vida1;
    public GameObject vida2;
    public GameObject vida3;
    public Text txtGold;

    [Header ("Paineis")]
	public GameObject painelPause;
	public GameObject painelItens;
	public GameObject painelItemInfo;
    // Start is called before the first frame update
    void Start()
    {
        gameControler = GameControler.getInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel")) {
            pauseGame() ;
        }
        controleVida();
        txtGold.text = gameControler.gold.ToString("N0").PadLeft(4,'0').Replace(",",".");
    }

    private void controleVida(){
        if(gameControler.vidaAtual == 3){
            vida1.SetActive(true);
            vida2.SetActive(true);
            vida3.SetActive(true);
        } else if((gameControler.vidaAtual == 2)){
            vida1.SetActive(true);
            vida2.SetActive(true);
            vida3.SetActive(false);
        } else if((gameControler.vidaAtual == 1)){
            vida1.SetActive(true);
            vida2.SetActive(false);
            vida3.SetActive(false);
        } else if((gameControler.vidaAtual == 0)){
            vida1.SetActive(false);
            vida2.SetActive(false);
            vida3.SetActive(false);
        }
    }
    public void pauseGame() {
        bool pauseState = painelPause.activeSelf;
		pauseState = !pauseState;
		painelPause.SetActive (pauseState);

		if (pauseState)
		{
			// Pausa
			Time.timeScale = 0;
			gameControler.MudarEstado (GameState.PAUSE);

			// Controla volume
			//float volumeTemporario = audioController.volumeMaximoMusica;
			//volumeTemporario = (volumeTemporario / 20);
			//audioController.sourceMusica.volume = volumeTemporario;
			
			//primeiroPainelPause.Select ();
		}
		else 
		{
			// Despausa
			Time.timeScale = 1;
			gameControler.MudarEstado (GameState.GAMEPLAY);
			//audioController.sourceMusica.volume = audioController.volumeMaximoMusica;
		}
    }

}
