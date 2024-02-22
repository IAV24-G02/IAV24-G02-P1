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
using UnityEngine.TextCore.Text;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de WANDER a otro agente
    /// </summary>
    public class Merodear : Encarar
    {
        //[SerializeField]
        //float tiempoMaximo = 2.0f;

        //[SerializeField]
        //float tiempoMinimo = 1.0f;

        //float t = 3.0f;
        //float actualT = 2.0f;

        //Direccion lastDir = new Direccion();

        [SerializeField]
        private float wanderOffset;

        [SerializeField]
        private float wanderRadius;

        [SerializeField]
        private float wanderRate;

        [SerializeField]
        private float maxAcceleration;
		
		private float wanderOrientation;

        private static int randomBinomial()
        {
            // Usar el generador de números aleatorios de Unity
            float randomNumber = Random.Range(0f, 1f);
            return (randomNumber > 0.5f) ? 1 : -1;
        }

		private Vector3 radiansAsVector(float orientation)
		{
			//sin(o) = x cos(o) = z;
			return new Vector3(Mathf.Sin(orientation), 0, Mathf.Cos(orientation));
		}

        public override Direccion GetDireccion()
		{

			float targetOrientation;
			Vector3 targetPosition;

			wanderOrientation += randomBinomial() * wanderRate;

			targetOrientation = wanderOrientation + agente.orientacion;

			targetPosition = radiansAsVector(agente.orientacion);

			objetivo.transform.position = transform.position + wanderOffset * targetPosition;

            targetPosition = radiansAsVector(targetOrientation);

            objetivo.transform.position += wanderRadius * targetPosition;

            Direccion sol = base.GetDireccion();

            targetPosition = radiansAsVector(agente.orientacion);

			sol.lineal = maxAcceleration * targetPosition;

			lastDir = sol;

            return sol;

        }

        /*

			function getSteering()-> SteeringOutput:
				# 1. Calculate the target to delegate to face
				# Update the wander orientation.
				wanderOrientation += randomBinomial() * wanderRate

				# Calculate the combined target orientation.
				targetOrientation = wanderOrientation + character.orientation
				# Calculate the center of the wander circle.
				target = character.position + wanderOffset * character.orientation.asVector()
				#Calculate the target location.
				target += wanderRadius * targetOrientation.asVector()

				# 2. Delegate to face.
				result = Face.getSteering()

				# 3. Now set the linear acceleration to be at full
				# acceleration in the direction of the orientation. 
				result.linear = maxAcceleration * character. orientation.asVector()

				# Return it. 
				return result
         */


    }
}

/*
  class Wander extends Face:
				# The radius and forward offset of the wander circle.
				wanderOffset: float
				wanderRadius: float
																									
				# The maximum rate at which the wander orientation can change. 
				wanderRate: float

				# The current orientation of the wander target. 
				wanderOrientation: float
		
				# The maximum acceleration of the character. 
				maxAcceleration: float

				# Again we don't need a new target.
				#... Other data is derived from the superclass
  
 
 */
