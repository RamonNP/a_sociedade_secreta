using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MudarCena : MonoBehaviour
{
    public string cenaDestino;

private Fade fade;
/// <summary>
/// Start is called on the frame when a script is enabled just before
/// any of the Update methods is called the first time.
/// </summary>
void Start()
{
    fade = FindObjectOfType(typeof(Fade)) as Fade;
}
public void interacao(){
   StartCoroutine("acionarProimaFase");
}
IEnumerator acionarProimaFase() {
    fade.fadeIn();
    yield return new WaitWhile(() => fade.fume.color.a < 0.9f);
     SceneManager.LoadScene(cenaDestino);
}
}
