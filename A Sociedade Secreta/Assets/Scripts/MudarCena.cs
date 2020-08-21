using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MudarCena : MonoBehaviour
{
    public string cenaDestino;

    public void interacao(){
        SceneManager.LoadScene(cenaDestino);
    }
}
