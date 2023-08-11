using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XenofobiaManager : MonoBehaviour
{
    public static XenofobiaManager instance;



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.instance.puntero.PunteroTriggerEnter.AddListener(ColisionEnter);
        GameManager.instance.puntero.Released.AddListener(PunteroReleased);
    }

    private void Update()
    {

    }



    void PunteroReleased()
    {
        BajarBarreras();
        RegresarFondoA();
    }

    void ColisionEnter(Barra barraColisionada)
    {
        ActivarBarras(barraColisionada);
    }

    void ColisionExit(Barra barraColisionada)
    {

    }







    void ActivarBarras(Barra barraColisionada)
    {
        if (barraColisionada.index == 1 || barraColisionada.index == 2)
        {
            barraColisionada.GetValoresEstadoB();
            barraColisionada.movimiento = Barra.Movimiento.manual;
            barraColisionada.heightTarget = barraColisionada.alturaMax;
            return;
        }
        else
        {
            if (barraColisionada.esProtagonista)
            {
                GameManager.instance.CambiarFondo(GameManager.instance.fondoB);
            }

            barraColisionada.GetValoresEstadoB();
            barraColisionada.direccion = Barra.Direccion.sube;
            barraColisionada.velocidadSubida = 4f;

            barraColisionada.AtHighestPoint.AddListener(() =>
            {
                barraColisionada.GetValoresEstadoB();
                barraColisionada.AtHighestPoint.RemoveAllListeners();
            });
        }
    }

    void BajarBarreras()
    {
        foreach (Barra barra in GameManager.instance.barras)
        {
            if (barra.index == 1 || barra.index == 2)
            {
                barra.GetValoresEstadoA();
                barra.velocidadBajada = 6f;
                barra.tiempoSostenidoEnMaximo = 2f;
                barra.direccion = Barra.Direccion.sube;
                barra.movimiento = Barra.Movimiento.automatico;
            }
            else
            {
                barra.GetValoresEstadoA();
                barra.direccion = Barra.Direccion.baja;
                barra.movimiento = Barra.Movimiento.automatico;
                barra.velocidadBajada = 3f;
            }
            
            barra.AtLowestPoint.AddListener(() =>
            {
                barra.GetValoresEstadoA();
                barra.AtLowestPoint.RemoveAllListeners();
            });
        }
    }


    void RegresarFondoA()
    {
        GameManager.instance.CambiarFondo(GameManager.instance.fondoA);
    }
}
