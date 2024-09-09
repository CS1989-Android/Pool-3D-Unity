using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastBuchacas : MonoBehaviour
{
    // SCRIPT DESCARTADO
    RaycastHit hit;
    Bolas bolas;
    void Start()
    {

    }
    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.up*0.5f, Color.red);
        if(Physics.Raycast(transform.position,transform.up*05f, out hit))
        {
            //print("Un Objeto paso por ahi");
            bolas = hit.collider.GetComponent<Bolas>(); // Para obtener el script bolas

            if(bolas != null)
            {
                bolas.rb.constraints = RigidbodyConstraints.None; // Desactiva las restricciones de la posicion en eje Y para que las bolas puedan caer
            }
        }
    }
}
