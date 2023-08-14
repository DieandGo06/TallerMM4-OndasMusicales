using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesmparoManager : MonoBehaviour
{
    public static DesmparoManager instance;
    [Header("Barras elegidas")]
    public Barra[] barrasActivadas;
    int indexBarraColisionada;





    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        barrasActivadas = new Barra[9];
        GameManager.instance.puntero.Released.AddListener(() => StartCoroutine(DesactivarBarrasActivas()));
        GameManager.instance.puntero.Pressed.AddListener(() => StopCoroutine("DesactivarBarrasActivas"));
        GameManager.instance.puntero.PunteroTriggerEnter.AddListener(CollisionEnter);
    }


    private void Update()
    {

    }



    void CollisionEnter(Barra barraColisionada)
    {
        GetBarrasActivas(barraColisionada);
        foreach (Barra barraActivada in barrasActivadas)
        {
            if (barraActivada.estado == Barra.Estado.estado_A)
            { 
                if (barraActivada != GameManager.instance.protagonista)
                {
                    barraActivada.GetValoresEstadoB();
                    barraActivada.movimiento = Barra.Movimiento.manual;
                    barraActivada.heightTarget = barraActivada.alturaMin;
                }
                else
                {
                    barraActivada.GetValoresEstadoB();
                    barraActivada.direccion = Barra.Direccion.baja;
                }
            }
        }
        GameManager.instance.CambiarFondo(GameManager.instance.fondoB);
    }

    void GetBarrasActivas(Barra barra)
    {
        List<Barra> _barras = GameManager.instance.barras;
        indexBarraColisionada = _barras.FindIndex(a => a.gameObject == barra.gameObject);
        //Cuatro barras a la izquierda de la barra colsionada
        if (indexBarraColisionada - 4 >= 0) barrasActivadas[0] = _barras[indexBarraColisionada - 4];
        else barrasActivadas[0] = _barras[indexBarraColisionada];
        //Tres barras a la izquierda de la barra colsionada
        if (indexBarraColisionada - 3 >= 0) barrasActivadas[1] = _barras[indexBarraColisionada - 3];
        else barrasActivadas[0] = _barras[indexBarraColisionada];
        //Dos barras a la izquierda de la barra colsionada
        if (indexBarraColisionada - 2 >= 0) barrasActivadas[2] = _barras[indexBarraColisionada - 2];
        else barrasActivadas[0] = _barras[indexBarraColisionada];
        //Una barras a la izquierda de la barra colsionada
        if (indexBarraColisionada - 1 >= 0) barrasActivadas[3] = _barras[indexBarraColisionada - 1];
        else barrasActivadas[1] = _barras[indexBarraColisionada];
        //Barra colisionada
        barrasActivadas[4] = _barras[indexBarraColisionada];
        //Una barras a la derecha de la barra colsionada
        if (indexBarraColisionada + 1 < _barras.Count) barrasActivadas[5] = _barras[indexBarraColisionada + 1];
        else barrasActivadas[5] = _barras[indexBarraColisionada];
        //Dos barras a la derecha de la barra colsionada
        if (indexBarraColisionada + 2 < _barras.Count) barrasActivadas[6] = _barras[indexBarraColisionada + 2];
        else barrasActivadas[6] = _barras[indexBarraColisionada];
        //Tres barras a la derecha de la barra colsionada
        if (indexBarraColisionada + 3 < _barras.Count) barrasActivadas[7] = _barras[indexBarraColisionada + 3];
        else barrasActivadas[7] = _barras[indexBarraColisionada];
        //Cuatro barras a la derecha de la barra colsionada
        if (indexBarraColisionada + 4 < _barras.Count) barrasActivadas[8] = _barras[indexBarraColisionada + 4];
        else barrasActivadas[8] = _barras[indexBarraColisionada];
    }

    IEnumerator DesactivarBarrasActivas()
    {
        yield return new WaitForSeconds(1.5f);
        foreach (Barra barra in GameManager.instance.barras)
        {
            if (barra.estado == Barra.Estado.estado_B)
            {
                barra.movimiento = Barra.Movimiento.automatico;
                barra.direccion = Barra.Direccion.sube;
                barra.GetValoresEstadoA();
            }
        }
        GameManager.instance.CambiarFondo(GameManager.instance.fondoA);
    }
}