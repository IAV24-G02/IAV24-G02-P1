/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de HUIR a otro agente
    /// </summary>
    public class Huir : ComportamientoAgente
    {
        #region parameters
        [SerializeField]
        private float radio = 3.0f;
        #endregion
        #region references
        public List<Transform> ratsTransform;
        Transform myTransform;
        #endregion
        #region methods
        public void AddRata(GameObject rata)
        {
            ratsTransform.Add(rata.transform);
        }
        public void RemoveRata(GameObject rata)
        {
            ratsTransform.Remove(rata.transform);
        }
        #endregion

        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {
            Direccion direccion = new Direccion();
            Vector3 averagePosition = Vector3.zero;

            foreach (Transform rataTransform in ratsTransform)
            {
                if (Vector3.Distance(myTransform.position, rataTransform.position) < radio)
                {
                    averagePosition += rataTransform.position;
                }
            }

            if (ratsTransform.Count != 0 && averagePosition != Vector3.zero)
            {
                averagePosition /= ratsTransform.Count;

                direccion.lineal = myTransform.position - averagePosition;
                direccion.lineal.Normalize();
                direccion.lineal *= agente.aceleracionMax;
            }

            return direccion;
        }

        private void Start()
        {
            myTransform = GetComponent<Transform>();
        }
    }
}
