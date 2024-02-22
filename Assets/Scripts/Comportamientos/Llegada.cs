/*    
   Copyright (C) 2024 Ignacio Ligero
   http://www.github.com/theligero

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

   Autor: Ignacio Ligero
   Contacto: iligero@ucm.es
*/
using UCM.IAV.Movimiento;
using UnityEngine;
using UnityEngine.UIElements;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de SEGUIR a otro agente
    /// </summary>
    public class Llegada : ComportamientoAgente
    {
        public float distancia;
        private Rigidbody rigidbody;
        public float acelMaxima;
        public float velMaxima;

        // El radio para llegar al objetivo
        [SerializeField]
        private float rObjetivo;

        // El radio en el que se empieza a ralentizarse
        [SerializeField]
        private float rRalentizado;

        // El tiempo en el que conseguir la aceleracion objetivo
        private float timeToTarget = 0.1f;

        public void Start()
        {
            objetivo = GameObject.Find("Avatar");
            rigidbody = agente.GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Obtiene la direcci�n
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {
            // Consigue la direcci�n hacia el objetivo
            Direccion direccion = new Direccion();
            direccion.lineal = objetivo.transform.position - transform.position;
            distancia = direccion.lineal.magnitude;
        
            // Comprueba si ya ha llegado
            if (distancia < rObjetivo)
            {
                direccion.lineal = Vector3.zero;
                rigidbody.velocity = Vector3.zero;
            }         
            //Si estamos fuera del rRalentizado
            else if (distancia > rRalentizado)
                velMaxima = agente.velocidadMax; // entonces se mueve a velocidad m�xima
            // En otro caso calcula la velocidad en escala
            else
                velMaxima = agente.velocidadMax * distancia / rRalentizado;

            // La velocidad objetivo combina velocidad y direcci�n
            direccion.lineal.Normalize();
            direccion.lineal *= velMaxima;

            // La aceleraci�n intenta conseguir la velocidad objetivo
            return direccion;
        }
    }
}
