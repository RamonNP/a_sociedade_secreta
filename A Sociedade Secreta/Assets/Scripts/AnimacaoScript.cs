using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public enum TipoAnimacao
{
	FLOAT,
	INT,
	BOOL,
	TRIGUER
}

public enum ModoAnimacao
{
	DIALOGO,
	MOVIMENTO,
	ANIMACAO,
    DESATIVAR
}
public class AnimacaoScript : MonoBehaviour
{
    public bool titulo;
    public List<int> inciarTeste;
    public GameObject cameraSeguir;
    public Fade fade;
    public GameObject interacao;
    private Config moverObjeto;
    private Animator playerAnimator; 
    [Header("Conficuração de Animaçoes")]
    //public GameObject[] objetoAnimator;
    //public string[] animacao;
    //public string[] tipoAnimacao;
    //public float[] intervaloENtreFalas;
    public Config[] configuracaoAnimacao;
     public Config[] configuracaoAnimacaoCena0;
     public Config[] configuracaoAnimacaoCena2;
     public Config[] configuracaoAnimacaoCena3;
    public Config[] configuracaoEvento;


    [Header("cena 1")]
    public bool animacao1;
    public int valor2;
    [Header("cena 2")]
    public bool animacao2;

    public Text caixaTexto;
    // Start is called before the first frame update

    public int idDialogo;
    public GameObject canvasNPC;
    public List<string> linhasDialogo;
    public List<string> respostaFala0;
    public int idFala;
    private bool dialogoOn, respondendoPergunta;
    public List<string> fala0;
    public List<string> fala1;
    public List<string> fala2;
    public List<string> fala3;
    public List<string> fala4;
    public List<string> fala5;
    public LayerMask interacaoMasck;
    public string[] idiomaFolder;
    public string nomeArquivoXml;
    public int idioma { get; private set; }

    void Start()
    {
        if(!titulo){
            fade = FindObjectOfType(typeof(Fade) ) as Fade;
            canvasNPC.SetActive(false);
            //painelResposta.SetActive(false);
            LoadDialogoData();
            StartCoroutine("iniciaCena0");

        }

    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(moverObjeto != null){
            moverObjeto.ObjetoAnimator.transform.position = Vector3.Lerp(moverObjeto.ObjetoAnimator.transform.position, moverObjeto.moverObjeto.posicaoFinal, Time.deltaTime*moverObjeto.moverObjeto.velociadade);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                interacao.GetComponent<Animator>().SetTrigger("atacar");
                //playerAnimator.SetTrigger("atacar");
            break;
            case "Coletavel":

            break;
            case "Porta":

            break;
            case "Interacao":

            break;
            default:
            break;
        }
    }
    IEnumerator iniciaCena0() {
        int i = 0;
        foreach (Config anim in configuracaoAnimacaoCena0)
        {
            if(inciarTeste.Contains(i) || inciarTeste.Count == 0){

                if(!anim.executarComAnterior){
                    yield return new WaitForSeconds(anim.tempoAnimacao);
                }
                if(anim.dialogo.Equals(ModoAnimacao.DIALOGO)) {
                    //playerAnimator = anim.ObjetoAnimator.GetComponent<Animator>();
                    interagir();
                } else if (anim.dialogo.Equals(ModoAnimacao.ANIMACAO)) {
                    rodarAnimacao(anim);
                } else if (anim.dialogo.Equals(ModoAnimacao.MOVIMENTO)) {
                    //moverObjeto = anim;
                } else if(anim.dialogo.Equals(ModoAnimacao.DESATIVAR)) {
                    anim.ObjetoAnimator.SetActive(false);
                }

            }
            i++;
        }
        yield return new WaitForSeconds(2);
        fade.fadeIn();
        canvasNPC.SetActive(false);
        yield return new WaitForSeconds(2);
        fade.fadeOut();
        StartCoroutine("iniciaCena1");

    }
    IEnumerator iniciaCena1() {
        idFala = 0;
        idDialogo = 1; 
        print("Cena1");
        cameraSeguir.transform.position = new Vector3(47f, cameraSeguir.transform.position.y, cameraSeguir.transform.position.z);
        int i = 0;
        foreach (Config anim in configuracaoAnimacao)
        {
            if(inciarTeste.Contains(i) || inciarTeste.Count == 0){

                if(!anim.executarComAnterior){
                    yield return new WaitForSeconds(anim.tempoAnimacao);
                }
                if(anim.dialogo.Equals(ModoAnimacao.DIALOGO)) {
                    //playerAnimator = anim.ObjetoAnimator.GetComponent<Animator>();
                    interagir();
                } else if (anim.dialogo.Equals(ModoAnimacao.ANIMACAO)) {
                    rodarAnimacao(anim);
                } else if (anim.dialogo.Equals(ModoAnimacao.MOVIMENTO)) {
                    //moverObjeto = anim;
                } else if(anim.dialogo.Equals(ModoAnimacao.DESATIVAR)) {
                    anim.ObjetoAnimator.SetActive(false);
                }

            }
            i++;
        }
        yield return new WaitForSeconds(2);
        fade.fadeIn();
        canvasNPC.SetActive(false);
        yield return new WaitForSeconds(2);
        fade.fadeOut();
        StartCoroutine("iniciaCena2");
        AudioController audioC = FindObjectOfType(typeof(AudioController)) as AudioController;
        audioC.trocarMusica(audioC.musicaTitulo, "Fase1", false);

    }

    IEnumerator iniciaCena2() {
        idFala = 0;
        idDialogo = 2; 
        cameraSeguir.transform.position = new Vector3(34f, cameraSeguir.transform.position.y, cameraSeguir.transform.position.z);
        foreach (Config anim in configuracaoAnimacaoCena2)
        {
                if(!anim.executarComAnterior){
                    yield return new WaitForSeconds(anim.tempoAnimacao);
                }
                if(anim.dialogo.Equals(ModoAnimacao.DIALOGO)) {
                    //playerAnimator = anim.ObjetoAnimator.GetComponent<Animator>();
                    interagir();
                } else if (anim.dialogo.Equals(ModoAnimacao.ANIMACAO)) {
                    rodarAnimacao(anim);
                } else if (anim.dialogo.Equals(ModoAnimacao.MOVIMENTO)) {
                    //moverObjeto = anim;
                } else if(anim.dialogo.Equals(ModoAnimacao.DESATIVAR)) {
                    anim.ObjetoAnimator.SetActive(false);
                }
        }
        yield return new WaitForSeconds(2);
        fade.fadeIn();
        canvasNPC.SetActive(false);
        yield return new WaitForSeconds(2);

        fade.fadeIn();
        AudioController audioC = FindObjectOfType(typeof(AudioController)) as AudioController;
        audioC.trocarMusica(audioC.musicaFase1, "Load", true);
    }
    IEnumerator iniciaCena3() {
        idFala = 0;
        idDialogo = 2; 
        cameraSeguir.transform.position = new Vector3(20.4f, cameraSeguir.transform.position.y, cameraSeguir.transform.position.z);
        foreach (Config anim in configuracaoAnimacaoCena3)
        {
                if(!anim.executarComAnterior){
                    yield return new WaitForSeconds(anim.tempoAnimacao);
                }
                if(anim.dialogo.Equals(ModoAnimacao.DIALOGO)) {
                    //playerAnimator = anim.ObjetoAnimator.GetComponent<Animator>();
                    interagir();
                } else if (anim.dialogo.Equals(ModoAnimacao.ANIMACAO)) {
                    rodarAnimacao(anim);
                } else if (anim.dialogo.Equals(ModoAnimacao.MOVIMENTO)) {
                    //moverObjeto = anim;
                } else if(anim.dialogo.Equals(ModoAnimacao.DESATIVAR)) {
                    anim.ObjetoAnimator.SetActive(false);
                }
        }
        yield return new WaitForSeconds(2);
        fade.fadeIn();
        canvasNPC.SetActive(false);
        yield return new WaitForSeconds(2);
        fade.fadeOut();
        StartCoroutine("iniciaCena4");
    }

    public void rodarAnimacao(Config anim) {
        anim.ObjetoAnimator.SetActive(true);
                    if(!anim.nomeAnimcao.Equals("")) {
                        playerAnimator = anim.ObjetoAnimator.GetComponent<Animator>();
                        switch (anim.tipoAnimacao)
                        {
                            case TipoAnimacao.BOOL:
                            {
                                //Time.timeScale = 1;
                                break;
                            }
                            case TipoAnimacao.FLOAT:
                            {
                                //Time.timeScale = 1;
                                break;
                            }
                            case TipoAnimacao.INT:
                            {
                                //Time.timeScale = 1;
                                break;
                            }
                            case TipoAnimacao.TRIGUER:
                            {
                                playerAnimator.SetTrigger(anim.nomeAnimcao);
                                break;
                            }
                        }
                    }
    }
    private void interagir()
    {
        canvasNPC.transform.position = cameraSeguir.transform.position;
        //interacao = objetoAnimator[0];
        //interacao.SendMessage("", SendMessageOptions.DontRequireReceiver);
        falarHud();
    }
    public void dialogo()
    {
        //print("linhasDialogo.Count "+linhasDialogo.Count);
        //print("idFala "+idFala);
        if(idFala < linhasDialogo.Count)
        {//print("IF");
            caixaTexto.text = linhasDialogo[idFala];
            /*
            if(idDialogo == 0 && idFala == 2)
            {
                textBtnA.text = respostaFala0[0];
                textBtnB.text = respostaFala0[1];
                respondendoPergunta = true;
                btnA.Select();
                painelResposta.SetActive(true);
            } */
        }
        else //ENCERRA A CONVERSA
        {print("ELSE" +idDialogo) ;
            switch(idDialogo)
            {
                case 0:
                    idDialogo = 1;                    
                    break;
                case 1:
                    idDialogo = 3;
                    break;
                case 2:
                    idDialogo = 0;
                    break;
                case 4:
                    idDialogo = 5;
                    break;
            }

            canvasNPC.SetActive(false);
            dialogoOn = false;
            //gameController.MudarEstado(GameState.FIMDIALOGO);
        } 
    }

    void prepararDialogo()
    {
        linhasDialogo.Clear();
        //print("idDialogo "+idDialogo);
        switch (idDialogo)
        {
            case 0:
                foreach (string s in fala0)
                {
                    linhasDialogo.Add(s);
                }
                break;
            case 1:
                foreach (string s in fala1)
                {
                    linhasDialogo.Add(s);
                }
                break;
            case 2:
                foreach (string s in fala2)
                {
                    linhasDialogo.Add(s);
                }
                break;
            case 3:
                foreach (string s in fala3)
                {
                    linhasDialogo.Add(s);
                }
                break;
            case 4:
                foreach (string s in fala4)
                {
                    linhasDialogo.Add(s);
                }
                break;
            case 5:
                foreach (string s in fala5)
                {
                    linhasDialogo.Add(s);
                }
                break;
        }
    }
    public void falarHud()
    {
        prepararDialogo();
        dialogo();
        canvasNPC.SetActive(true);
        dialogoOn = true;        
        idFala += 1;

    }

    void LoadDialogoData()
    {
        TextAsset xmlData = (TextAsset)Resources.Load(idiomaFolder[idioma] + "/" + nomeArquivoXml);
        XmlDocument XmlDocument = new XmlDocument();
        XmlDocument.LoadXml(xmlData.text);
        //Debug.Log("qtd" + XmlDocument["dialogos"].ChildNodes.Count);

        foreach(XmlNode dialogo in XmlDocument["dialogos"].ChildNodes)
        {
            string dialogoName = dialogo.Attributes["name"].Value;

            foreach(XmlNode f in dialogo["falas"].ChildNodes)
            {
                switch(dialogoName)
                {
                    case "fala0":
                        fala0.Add(TextoFormatado(f.InnerText));
                        break;
                    case "fala1":
                        fala1.Add(TextoFormatado(f.InnerText));
                        break;
                    case "fala2":
                        fala2.Add(TextoFormatado(f.InnerText));
                        break;
                    case "fala3":
                        fala3.Add(TextoFormatado(f.InnerText));
                        break;
                    case "fala4":
                        fala4.Add(TextoFormatado(f.InnerText));
                        break;
                    case "fala5":
                        fala5.Add(TextoFormatado(f.InnerText));
                        break;
                    case "resposta0":
                        respostaFala0.Add(TextoFormatado(f.InnerText));
                        break;
                }
            }
        }
    }
    
    public string TextoFormatado (string frase)
	{
		string temp = frase;

		// Subtitui palavras especificas
		temp = temp.Replace ("**cor=yellow", "<color=#FFFF00FF>");
		temp = temp.Replace ("**cor=green", "<color=#008A22>");
		temp = temp.Replace ("**cor=red", "<color=#ff0000ff>");
		temp = temp.Replace ("**cor=orange", "<color=#ffa500ff>");
		temp = temp.Replace ("**cor=prata", "<color=#9F9999>");
		temp = temp.Replace ("fimnegrito**", "</b>");
		temp = temp.Replace ("**negrito", "<b>");
		temp = temp.Replace ("fimcor**", "</color>");

		return temp;
	}
    public void eventoAnimacao(int obj){
        Config con = configuracaoEvento[obj];
        if(con.dialogo.Equals(ModoAnimacao.DIALOGO)) {
            //playerAnimator = anim.ObjetoAnimator.GetComponent<Animator>();
            interagir();
        } else if(con.dialogo.Equals(ModoAnimacao.ANIMACAO)){
            con.ObjetoAnimator.SetActive(true);
            if(!con.nomeAnimcao.Equals("")) {
                con.ObjetoAnimator.GetComponent<Animator>().SetTrigger(con.nomeAnimcao);
            }
        } else if(con.dialogo.Equals(ModoAnimacao.MOVIMENTO)) {
            moverObjeto = con;
        } else if(con.dialogo.Equals(ModoAnimacao.DESATIVAR)) {
            con.ObjetoAnimator.SetActive(false);
        }
    }
}
[Serializable]
public class Config
{
    public float tempoAnimacao;
    public ModoAnimacao dialogo = ModoAnimacao.DIALOGO;
    public GameObject ObjetoAnimator;
    public string nomeAnimcao;
    public TipoAnimacao tipoAnimacao;
    public Boolean executarComAnterior = false; 
    public MoverObjeto moverObjeto;
}
[Serializable]
public class MoverObjeto
{
    public Vector3 posicaoFinal;
    public float velociadade;
 
}