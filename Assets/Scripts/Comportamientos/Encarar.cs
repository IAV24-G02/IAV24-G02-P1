using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de WANDER a otro agente
    /// </summary>
    public class Encarar : Alineamiento
    {
        //[SerializeField]
        //float tiempoMaximo = 2.0f;

        //[SerializeField]
        //float tiempoMinimo = 1.0f;

        //float t = 3.0f;
        //float actualT = 2.0f;

        public override Direccion GetDireccion()
        {
            Vector3 direccion = new Vector3();
            direccion = objetivo.transform.position - transform.position;
            float distancia = direccion.magnitude;

            if (distancia == 0)
                return lastDir;///////////////////////

            Agente objetivoAgente = objetivo.GetComponent<Agente>();
            objetivoAgente.orientacion = Mathf.Atan2(-direccion.x, direccion.z);

            return base.GetDireccion();
        }

        /*
			class Face extends Align:
					#Overrides the Align.target member.
					target: Kinematic

					#... Other data is derived fromt he superClass...
					#Implemented as it was in Pursue
					function getSteering -> SteeringOutput: 
						#1. Calculate the target to delegate to align
						#Work out the direction to target.
						direction = target.position - character.position
		
						#Check for a zero direction, and make no change if so.
						if direction.length() == 0:
							return target

						#2. Delegate to align.
						Align.target = explicitTarget
						Align.target.orientation = atan2(-direction.x, direction.z)
						return Align.getSteering

			
         */


    }
}
