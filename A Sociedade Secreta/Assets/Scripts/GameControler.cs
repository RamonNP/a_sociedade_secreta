using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
	DIALOGO,
	FIMDIALOGO,
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
	public string[] idiomaFolder;
	private bool estaPausado;

    [Header ("Materiais para iluminação")]
	public Material luz2D;
	public Material padrao2D;

    public bool missao1;
	public int idioma;

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

			case GameState.FIMDIALOGO:
			{
				StartCoroutine ("fimConversa");
				break;
			}

			default:
			{
				break;
			}
		}
	}
 	IEnumerator fimConversa()
    {
        yield return new WaitForEndOfFrame();
        MudarEstado(GameState.GAMEPLAY);
    }
// Formata uma string com indicadores para tags
	public string TextoFormatado (string frase)
	{
		string temp = frase;

		// Subtitui palavras especificas
		temp = temp.Replace ("cor=yellow", "<color=#FFFF00FF>");
		temp = temp.Replace ("cor=red", "<color=#ff0000ff>");
		temp = temp.Replace ("cor=orange", "<color=#ffa500ff>");
		temp = temp.Replace ("fimnegrito", "</b>");
		temp = temp.Replace ("negrito", "<b>");
		temp = temp.Replace ("fimcor", "</color>");

		return temp;
	}

}
