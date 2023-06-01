using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public List<GameObject> barras = new List<GameObject>();

    [Header("Organizacion Horizontal")]
    [Range(-4, 4)] public float posicionXPrimeraBarra;
    [Range(0, 0.5f)] public float separacionHorizontal;





    [Button]
    void OrganizarHorizontalmente()
    {
        float nuevaPosicion = posicionXPrimeraBarra;
        foreach(GameObject barra in barras)
        {
            barra.transform.position = new Vector3(nuevaPosicion, barra.transform.position.y, barra.transform.position.z);
            nuevaPosicion += barra.transform.localScale.x + separacionHorizontal;
        }
    }
}
