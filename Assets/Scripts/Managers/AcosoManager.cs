using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcosoManager : MonoBehaviour
{
    public static AcosoManager instance;
    [Range(0, 1f)] public float diferenciaAltura;
    public Color colorFondoB;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.instance.puntero.Pressed.AddListener(PunteroPressed);
        GameManager.instance.puntero.Released.AddListener(RegresarFondoA);
        GameManager.instance.puntero.PunteroTriggerEnter.AddListener(CambiarFondo);
        GameManager.instance.puntero.PunteroTriggerEnter.AddListener(OrganizarBarras);
    }

    private void Update()
    {
        foreach (Barra barra in GameManager.instance.barras)
        {
            if (!GameManager.instance.hayInterccion)
            {
                //Recien terminar la interaccion
                if (barra.movimiento == Barra.Movimiento.manual)
                {
                    barra.movimiento = Barra.Movimiento.automatico;
                    barra.direccion = Barra.Direccion.baja;
                    barra.GetValoresEstadoA();
                    //Debe ir despues de GetValoresA
                    if (barra != GameManager.instance.protagonista) barra.velocidadBajada = 10f;
                    else barra.velocidadBajada = 4f;
                    barra.estado = Barra.Estado.transicion;
                    return;
                }
                //Tras la bajada acelerada al terminar la interaccion
                if (barra.direccion == Barra.Direccion.sube && barra.estado == Barra.Estado.transicion)
                {
                    barra.GetValoresEstadoA();
                }
            }
        }
    }


    void PunteroPressed()
    {
        foreach (Barra barra in GameManager.instance.barras)
        {
            barra.GetValoresEstadoB();
            barra.movimiento = Barra.Movimiento.manual;
        }
    }

    void CambiarFondo(Barra barraColisionada)
    {
        if (barraColisionada.index == GameManager.instance.protagonista.index)
        {
            GameManager.instance.CambiarFondo(GameManager.instance.fondoB);
        } 
        else GameManager.instance.CambiarFondo(GameManager.instance.fondoA);
    }

    void OrganizarBarras(Barra barraColisionada)
    {
        float alturaMinima = -3f;
        float extraHeightSpacing;
        if (barraColisionada.index <= 8) 
        {
            extraHeightSpacing = (float)(0.1f + (barraColisionada.index / 30f));
        }
        else if (barraColisionada.index == GameManager.instance.protagonista.index)
        {
            extraHeightSpacing = 0.65f;
        }
        else if (barraColisionada.index >= 10)
        {
            extraHeightSpacing = (float)(0.1f + ((GameManager.instance.barras.Count - barraColisionada.index) / 30f));
        }
        else extraHeightSpacing = 0;

        //Protagonista
        GameManager.instance.protagonista.heightTarget = GameManager.instance.protagonista.data.alturaMax_A - 1;

        //Barras a la izquierda
        for (int i = GameManager.instance.protagonista.index - 1; i >= 0; i--)
        {
            GameManager.instance.barras[i].heightTarget = alturaMinima + ((GameManager.instance.protagonista.index - i) * extraHeightSpacing);
        }
        //Barras a la derecha
        for (int i = GameManager.instance.protagonista.index + 1; i < GameManager.instance.barras.Count; i++)
        {
            GameManager.instance.barras[i].heightTarget = alturaMinima + ((i - GameManager.instance.protagonista.index) * extraHeightSpacing);
        }
    }

    void RegresarFondoA()
    {
        GameManager.instance.CambiarFondo(GameManager.instance.fondoA);
    }
}
