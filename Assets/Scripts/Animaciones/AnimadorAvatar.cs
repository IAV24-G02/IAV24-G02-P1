/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimadorAvatar: MonoBehaviour
{

    Animator animator;
    Rigidbody myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(myRigidbody.velocity.magnitude >= 0)
            animator.SetInteger("speed", (int)myRigidbody.velocity.magnitude);
    }
}
