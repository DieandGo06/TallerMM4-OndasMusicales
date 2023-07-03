using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BarreraData", menuName = "BarretaData", order = 1)]
public class BarreraData : ScriptableObject
{
    [Header("Estado A")]
    [Range(-5.5f, 4.5f)] public float alturaMin_A;
    [Range(-5.5f, 4.5f)] public float alturaMax_A;
    [Range(0, 20)] public float velocidadSubida_A;
    [Range(0, 20)] public float velocidadBajada_A;
    [Space(-5), Header("Avanzados")]
    [Range(0, 1)] public float rangoVariacionAltura_A;
    [Range(0, 2)] public float tiempoSostenidoEnMinimo_A;
    [Range(0, 2)] public float tiempoSostenidoEnMaximo_A;

    [Space(40), Header("Estado B")]
    [Range(-5.5f, 4.5f)] public float alturaMin_B;
    [Range(-5.5f, 4.5f)] public float alturaMax_B;
    [Range(0, 20)] public float velocidadSubida_B;
    [Range(0, 20)] public float velocidadBajada_B;
    [Space(-5), Header("Avanzados")]
    [Range(0, 1f)] public float rangoVariacionAltura_B;
    [Range(0, 2)] public float tiempoSostenidoEnMinimo_B;
    [Range(0, 2)] public float tiempoSostenidoEnMaximo_B;
}
