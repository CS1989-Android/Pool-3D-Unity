using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camaras : MonoBehaviour
{
    public Camera camaraTaco;
    public Camera camaraAerea;
    private Camera camaraActual;
    CamaraTaco tacoScript;
    void Start()
    {
        camaraActual = camaraTaco;
        tacoScript = camaraTaco.GetComponent<CamaraTaco>();
    }
    
    public void cambioCamaras()
    {
        if(camaraActual == camaraTaco)
        {
            camaraTaco.enabled = false;
            camaraAerea.enabled = true;
            camaraActual = camaraAerea;
        } else
        {
            //reset.reseteoCamara();
            camaraTaco.enabled = true;
            camaraAerea.enabled = false;
            camaraActual = camaraTaco;
            tacoScript.reseteoCamara();

            if(tacoScript != null)
            {
                tacoScript.taco1.SetActive(true);
                tacoScript.barraFuerza.gameObject.SetActive(true);
            }
        }
    }
}
