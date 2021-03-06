﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
	private Hud hud;
    public GameState estadoAtual;
    private static GameControler instance;
    private SaveLoadExemplo saveLoad;
    public int gold;
    public int quantidadeFlechas;
    public string[] tiposDano;

    public GameObject[] fxDano;
     [Header("Informacoes Player")]
     public int idPersonagem;
    public int idFlechaEquipada;
    public int vidaMaxima;
    public int vidaAtual;
    public int idArma;
    public int idArmaAtual;
    public float[] velocidadesFlecha;
    public GameObject[] flechaPrefabs;

	public ItemModelo[] armaInicialPersonagem;
    private int idArmaInicial;
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

	private AudioController audioController;
    public bool missao1;
	public int idioma;
	public string nomeArquivoXml;
	private Inventario inventario;
    //private object armaInicialPersonagem;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
		inventario = FindObjectOfType(typeof(Inventario)) as Inventario;
        hud = FindObjectOfType(typeof(Hud)) as Hud;
        audioController = FindObjectOfType(typeof(AudioController)) as AudioController;
        saveLoad = FindObjectOfType(typeof(SaveLoadExemplo)) as SaveLoadExemplo;

        //painelPause.SetActive(false);
        //painelItens.SetActive(false);
        //painelItemInfo.SetActive(false);

        //validarArma();
        //Carregar (PlayerPrefs.GetString ("slot"));
        Load(PlayerPrefs.GetString ("slot")); 
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

    public void Save()
    {

        //string caminho = string.Concat (Application.persistentDataPath, "/", PlayerPrefs.GetString ("slot"));

		// Cria arquivo e formatter 
		//BinaryFormatter binaryFormatter = new BinaryFormatter ();
		//FileStream fileStream = File.Create (caminho);

        string nomeArquivoSave = string.Concat (Application.persistentDataPath, "/", PlayerPrefs.GetString ("slot"));

		//if(nomeArquivoSave == null || nomeArquivoSave.Equals("")){
		//	nomeArquivoSave = "ramonTeste";  // remover gabiarra para teste
		//}
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(nomeArquivoSave);

        PlayerData data = new PlayerData();
        data.idioma = idioma;
        data.idPersonagem = idPersonagem;
        data.gold = gold;
        data.idArma = idArma;

        data.idFlechaEquipada = idFlechaEquipada;
        //data.qtdFlechas = qtdFlechas;
        //data.qtdPocoes = qtdPocoes;
        //data.aprimoramentoArma = aprimoramentoArma;

        //if (itensInventario.Count != 0) { itensInventario.Clear(); } 
        //foreach(GameObject i in inventario.itemInventario)
        {
            //itensInventario.Add(i.name);
        }

        //data.itensInventario = itensInventario;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load(string slot)
    {
        string nomeArquivoSave = slot;

        if (File.Exists(Application.persistentDataPath + "/" + nomeArquivoSave))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + nomeArquivoSave, FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);

            idioma = data.idioma;
            gold = data.gold;
            idPersonagem = data.idPersonagem;
            idFlechaEquipada = data.idFlechaEquipada;
            //qtdFlechas = data.qtdFlechas;
            //itensInventario = data.itensInventario;
            //aprimoramentoArma = data.aprimoramentoArma;

            idArma = data.idArma;
            idArmaAtual = data.idArma;
            idArmaInicial = data.idArma;

            //inventario.itemInventario.Clear();

            //foreach(string i in itensInventario)
            {
                //inventario.itemInventario.Add(Resources.Load<GameObject>("Armas/" + i));
            }

            //inventario.itemInventario.Add(ArmaInicial[idPersonagem]);
            //GameObject tempArma = Instantiate(ArmaInicial[idPersonagem]);
            //inventario.itensCarregados.Add(tempArma);

            vidaAtual = vidaMaxima;
            //manaAtual = manaMax;

            file.Close();

            MudarEstado(GameState.GAMEPLAY);
            if(hud != null){
                hud.verificarHudPersonagem();
            }

            string nomeCena = "fase1";
            audioController.trocarMusica(audioController.musicaFase1, nomeCena, true);
        }
        else
        {
            newGame(); 
        }
    }

    void newGame()
    {
        //definir os valores iniciais do jogo
        gold = 0;
        idPersonagem = PlayerPrefs.GetInt("idPersonagem");
        idArma = 0;//armaInicialPersonagem[idPersonagem].idArma;

        idFlechaEquipada = 0;
        //qtdFlechas[0] = 25;
        //qtdFlechas[1] = 0;
        //qtdFlechas[2] = 0;

        //qtdPocoes[0] = 3;
        //qtdPocoes[1] = 3;

        Save(); 
        MudarEstado (GameState.GAMEPLAY);
		//hud.verificarHudPersonagem(); //sdsadsa
        Load(PlayerPrefs.GetString ("slot"));
    }

    public void click()
    {
        audioController.tocarFx(audioController.fxClick, 1);
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

    public void morrer() {
        string nomeCena = "fase1";
        audioController.trocarMusica(audioController.musicaFase1, nomeCena, true);
    }

}


[Serializable]
class PlayerData
{
    public int idioma;
    public int gold;
    public int idPersonagem;
    public int idArma;
    public int idFlechaEquipada;
 //   public int[] qtdFlechas;
//   public int[] qtdPocoes;
//    public List<string> itensInventario;
    public List<int> aprimoramentoArma;
}