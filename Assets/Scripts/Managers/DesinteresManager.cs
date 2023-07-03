using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class DesinteresManager : MonoBehaviour
{
    public static DesinteresManager instance;
    [Range(0, 1f)] public float diferenciaAltura;

    //privadas
    List<BarraMovimiento> barrasScript = new List<BarraMovimiento>();




    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.instance.puntero.punteroMantieneColision.AddListener(ReorganizarBarras);

        foreach (GameObject script in GameManager.instance.barras)
        {
            barrasScript.Add(script.GetComponent<BarraMovimiento>());
        }
    }


    void ReorganizarBarras(Collider2D colliderBarra)
    {
        int index = GameManager.instance.barras.FindIndex(a => a == colliderBarra.gameObject);

        //Barra Principal
        barrasScript[index].heightTarget = GameManager.instance.punteroPosition.y;

        //Barras a la izquierda
        for (int i = index-1; i >= 0; i--)
        {
            barrasScript[i].heightTarget = GameManager.instance.punteroPosition.y - ((index-i) * diferenciaAltura);
        }
        //Barras a la derecha
        for (int i = index + 1; i < GameManager.instance.barras.Count; i++)
        {
            barrasScript[i].heightTarget = GameManager.instance.punteroPosition.y - ((i-index) * diferenciaAltura);
        }


    }

}
