using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBuchacas : MonoBehaviour
{
    Bolas bolas;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Lisas" || other.gameObject.tag == "Rayadas" || other.gameObject.tag == "Negra" || other.gameObject.tag == "Blanca")
        {
            Bolas bolas = other.gameObject.GetComponent<Bolas>();
            bolas.rb.constraints = RigidbodyConstraints.None;
        }
    }
}
