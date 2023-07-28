using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcosoManager : MonoBehaviour
{
    public static AcosoManager instance;
    [Range(0, 1f)] public float diferenciaAltura;
    public BarreraData dataInicialProta;
    public Color colorFondoB;
    int indexProtagonista;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.instance.protoganista.data.CopyDataFrom(dataInicialProta);
        GameManager.instance.puntero.PunteroPressed.AddListener(PunteroPressed);
        GameManager.instance.puntero.PunteroTriggerStay.AddListener(ReorganizarBarras);
        indexProtagonista = GameManager.instance.barras.FindIndex(a => a == GameManager.instance.protoganista);

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
                    if (barra != GameManager.instance.protoganista) barra.velocidadBajada = 10f;
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
        GameManager.instance.CambiarColorFondo(colorFondoB);
        foreach (Barra barra in GameManager.instance.barras)
        {
            barra.GetValoresEstadoB();
            barra.movimiento = Barra.Movimiento.manual;
        }

        if (GameManager.instance.protoganista.data.alturaMax_B > -3.2f)
        {
            GameManager.instance.protoganista.data.alturaMin_A += -0.4f;
            GameManager.instance.protoganista.data.alturaMax_A += -0.6f;
            GameManager.instance.protoganista.data.alturaMax_B += -0.4f;
        }
    }

    void ReorganizarBarras(Barra barraColisionada)
    {
        float alturaMinima = -1.7f;
        GameManager.instance.protoganista.heightTarget = GameManager.instance.protoganista.data.alturaMax_A - 1;

        //Barras a la izquierda
        for (int i = indexProtagonista - 1; i >= 0; i--)
        {
            GameManager.instance.barras[i].heightTarget = alturaMinima + ((indexProtagonista - i) * diferenciaAltura);
        }
        //Barras a la derecha
        for (int i = indexProtagonista + 1; i < GameManager.instance.barras.Count; i++)
        {
            GameManager.instance.barras[i].heightTarget = alturaMinima + ((i - indexProtagonista) * diferenciaAltura);
        }
    }
}
