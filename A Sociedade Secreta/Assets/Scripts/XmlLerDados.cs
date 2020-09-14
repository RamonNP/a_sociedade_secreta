using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class XmlLerDados : MonoBehaviour
{
    public Dictionary<string, string> mensagemInteracao;
    // Start is called befo
    public static XmlLerDados instance;

    public static XmlLerDados getInstance() {
        if(instance == null)
        {
            instance = GameObject.FindObjectOfType<XmlLerDados>();
            instance.mensagemInteracao = new Dictionary<string, string>();
        }
        return instance;
    }
    void Start()
    {

       // mensagemInteracao = new Dictionary<string, string>();
       // print("mensagemInteracao"+ mensagemInteracao);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadDialogoData(string caminho)
    {
        //idiomaFolder[idioma] + "/" + nomeArquivoXml
        print(caminho);
        TextAsset xmlData = (TextAsset)Resources.Load(caminho);
        XmlDocument XmlDocument = new XmlDocument();
        XmlDocument.LoadXml(xmlData.text);
        //Debug.Log("qtd" + XmlDocument["dialogos"].ChildNodes.Count);

        foreach(XmlNode dialogo in XmlDocument["dialogos"].ChildNodes)
        {
            string dialogoName = dialogo.Attributes["name"].Value;

            foreach(XmlNode f in dialogo["falas"].ChildNodes)
            {
                if(mensagemInteracao == null){
                    mensagemInteracao = new Dictionary<string, string>();
                }
                print("chave"+dialogoName+"-"+TextoFormatado(f.InnerText));
                //mensagemInteracao[dialogoName] = TextoFormatado(f.InnerText);
                mensagemInteracao.Add(dialogoName,TextoFormatado(f.InnerText));
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
}
