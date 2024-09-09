using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolas : MonoBehaviour
{
    private bool esRayada;
    private bool esLisa;
    private bool esBlanca;
    private bool esNegra;
    public Rigidbody rb; // Para poder acceder desde el script de Raycast
    // Metodos publicos para poder acceder a ellos desde GameManager
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        switch (gameObject.tag)
        {
            case "Rayadas":
                esRayada = true;
                break;
            case "Lisas":
                esLisa = true;
                break;
            case "Blanca":
                esBlanca = true;
                break;
            case "Negra":
                esNegra = true;
                break;
        }
    }
    public bool esBolaLisa()
    {
        return esLisa;
    }
    public bool esBolaRayada()
    {
        return esRayada;
    }
    public bool esBolaBlanca()
    {
        return esBlanca;
    }
    public bool esBolaNegra()
    {
        return esNegra;
    }
}
