/*    
   Copyright (C) 2024 Ignacio Ligero
   http://www.github.com/theligero

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Ignacio Ligero
   Contacto: iligero@ucm.es
*/
using System;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    public class Separacion : ComportamientoAgente
    {
        #region parameters
        [SerializeField]
        private float maxAcceleration;

        // Umbral en el que se activa
        [SerializeField]
        private float umbral;
        // Coeficiente de reducción de la fuerza de repulsión
        [SerializeField]
        private float decayCoefficient;
        #endregion

        #region references
        // Entidades potenciales de las que huir
        public GameObject targEmpty;

        private GameObject[] targets;
        #endregion

        #region methods
        /// <summary>
        /// Separa al agente
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {
            int numRatas = targEmpty.transform.childCount;
            targets = new GameObject[numRatas];

            for (int i = 0; i < numRatas; i++)
            {
                targets[i] = targEmpty.transform.GetChild(i).gameObject;             
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
                   
                }
            }

            return result;
        }
        #endregion
    }
}