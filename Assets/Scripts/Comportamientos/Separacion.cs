/*    
   Copyright (C) 2024 Ignacio Ligero
   http://www.github.com/theligero

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Ignacio Ligero
   Contacto: iligero@ucm.es
*/
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    public class Separacion : ComportamientoAgente
    {
        public float maxAcceleration;

        // Entidades potenciales de las que huir
        public GameObject targEmpty;

        // Umbral en el que se activa
        [SerializeField]
        float umbral;

        // Coeficiente de reducción de la fuerza de repulsión
        [SerializeField]
        float decayCoefficient;

        private GameObject[] targets;

        /// <summary>
        /// Separa al agente
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {
            int numRatas = targEmpty.transform.childCount;
            targets = new GameObject[numRatas];

            // targEmpty = transform.parent.gameObject;

            for (int i = 0; i < numRatas; i++)
            {
                targets[i] = targEmpty.transform.GetChild(i).gameObject;
                //Debug.Log(targets[i].transform.position);
            }

            Direccion result = new Direccion();
            
            // Comprueba cada uno de los objetivos
            foreach(GameObject target in targets)
            {
                if (gameObject == target) continue;

                // Comprueba si el objetivo está cerca
                Vector3 direccion = new Vector3();
                direccion = transform.position - target.transform.position;
                float distancia = direccion.magnitude;

                if (distancia < umbral)
                {
                    // Calcula la fuerza de repulsión
                    // (usando la ley del inversa del cuadrado).
                    float strength = Math.Min(
                    decayCoefficient / (distancia * distancia),
                    maxAcceleration);

                    // Añade la aceleración
                    direccion.Normalize();
                    result.lineal += strength * direccion;

                    // Debug.Log(result.lineal);
                }
            }

            return result;
        }
    }
}