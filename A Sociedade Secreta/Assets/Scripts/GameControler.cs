using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
	DIALOGO,
	FIM_DIALOGO,
	GAMEPLAY,
	ITENS,
	LOADGAME,
	PAUSE
}

public class GameControler : MonoBehaviour
{
    public GameState estadoAtual;
    private static GameControler instance;
    public int gold;
    public string[] tiposDano;

    public GameObject[] fxDano;
     [Header("Informacoes Player")]
     public int idPersonagem;
    public int vidaMaxima;
    public int vidaAtual;
    public int idArma;
    public int idArmaAtual;
     [Header("Bonco Player")]
     public string[] nomePersonagem, spriteSheet;
    public GameObject fxMorte;
    [Header("Banco de Dados Arma")]
    public Sprite[] Armas;

	private bool estaPausado;

    [Header ("Materiais para iluminação")]
	public Material luz2D;
	public Material padrao2D;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public static GameControler getInstance() {
        if(instance == null)
        {
            instance = GameObject.FindObjectOfType<GameControler>();
        }
        return instance;
    }

    // Update is called once per frame
void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Define novo estado do jogo
	public void MudarEstado (GameState novoEstado)
	{
		this.estadoAtual = novoEstado;

		switch (novoEstado)
		{
			case GameState.GAMEPLAY:
			{
				Time.timeScale = 1;
				break;
			}

			case GameState.PAUSE: case GameState.ITENS:
			{
				Time.timeScale = 0;
				break;
			}

			case GameState.FIM_DIALOGO:
			{
				StartCoroutine ("FimDaConversa");
				break;
			}

			default:
			{
				break;
			}
		}
	}


}
