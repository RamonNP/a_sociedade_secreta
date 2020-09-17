using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static void changeMaterial(GameObject objeto, Material material) {
        foreach (Transform child in objeto.transform) {
            changeMaterial(child.gameObject, material);
            SpriteRenderer childRender = child.GetComponent<SpriteRenderer>();
            if(childRender != null) {
                childRender.material = material;
            }
        }
    }
    public static void changeColor(GameObject objeto, Color color) {
        foreach (Transform child in objeto.transform) {
            changeColor(child.gameObject, color);
            SpriteRenderer childRender = child.GetComponent<SpriteRenderer>();
            if(childRender != null) {
                childRender.color = color;
            }
        }
    }
    public static void enableAndDisable(GameObject objeto, bool enable) {
        foreach (Transform child in objeto.transform) {
            enableAndDisable(child.gameObject, enable);
            SpriteRenderer childRender = child.GetComponent<SpriteRenderer>();
            if(childRender != null) {
                childRender.enabled = enable;
            }
        }
    }

}
