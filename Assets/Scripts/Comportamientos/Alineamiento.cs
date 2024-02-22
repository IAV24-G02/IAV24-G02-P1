using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Networking.UnityWebRequest;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de WANDER a otro agente
    /// </summary>
    public class Alineamiento : ComportamientoAgente
    {
        //[SerializeField]
        //float tiempoMaximo = 2.0f;

        //[SerializeField]
        //float tiempoMinimo = 1.0f;

        //float t = 3.0f;
        //float actualT = 2.0f;

        protected Direccion lastDir = new Direccion();

		private float maxAngularAcceleration = 2;
		private float maxRotation = 2;

        [SerializeField]
        private float targetRadius = 2;

        [SerializeField]
        private float slowRadius = 0.1f;

        [SerializeField]
        private float timeToTarget = 0.1f;

        private float MapToRange(float angle)
        {
            // Asegúrate de que el ángulo esté entre 0 y 2π
            angle = Mathf.Repeat(angle, Mathf.PI * 2);

            // Ajusta el ángulo para que esté entre -π y π
            if (angle > Mathf.PI)
            {
                angle -= Mathf.PI * 2;
            }

            return angle;
        }


        public override Direccion GetDireccion()
		{
			Debug.Log("Meordeo 3");

			Agente objetivoAgente = objetivo.GetComponent<Agente>();
			float rotation = objetivoAgente.orientacion - agente.orientacion;

			rotation = MapToRange(rotation);

            float rotationSize = Mathf.Abs(rotation);

            float targetRotation;

            if (rotationSize < targetRadius)
			{
				//Direccion sol1 = new Direccion();
				//sol1.angular = lastDir.angular;
				//sol1.lineal = Vector3.zero;
				//lastDir = sol1;
				//return sol1;
				lastDir = new Direccion();
				return lastDir;
			}

			if (rotationSize > slowRadius)
			{
				targetRotation = maxRotation;
			}
			else
			{
				targetRotation = maxRotation * rotationSize / slowRadius;
			}

			targetRotation *= rotation / rotationSize;

            Direccion sol2 = new Direccion();

			sol2.angular = targetRotation - agente.rotacion;
			sol2.angular /= timeToTarget;

			float angularAcceleration = Mathf.Abs(sol2.angular);
			if(angularAcceleration > maxAngularAcceleration)
			{
				sol2.angular /= angularAcceleration;
				sol2.angular *= maxAngularAcceleration;
            }

			sol2.lineal = Vector3.zero;

			lastDir = sol2;

            return sol2;
        }
    }
}
/*
   class Align:
			character: Kinematic
			target: Kinematic

			maxAngularAcceleration: float
			maxRotation: float

			#The radius for arriving at the target.
			targetRadius: float

			#The radius for beginning to slow down.
			slowRadius: float

			#The time over which to achieve target spedd.
			timeToTarget: float = 0.1


			function getSteering() -> SteeringOutput:
				result = new SteeringOutput();
		
				#Get the naive direction to the target.
				rotation = target.orientation - character.orientation

				#Map the result to the (-pi, pi) interval.
				rotation = mapToRange(rotation)
				rotationSize = abs(rotation)

				#Check if we are there, return no steering.
				ir rotationSIze < targetRadius:
					return null

				#If we are outside the slowRadius, then use maximun rotation.
				if rotationSize > slowRadius:
					targetRotation = maxRotation

				#Otherwise calculate a scaled rotation.
				else:
					targetRotation = 
						maRotation * rotationSize / slowRadius
 
 
			#The final target rotation combines speed ( already in the 
				#variable) and direction.
				targetRotation *= rotation / rotationSize

				#Acceleration tries to get to the target rotation.
				result.angular = targetRotation - character.rotation
				result.angular /= timeToTarget
	
				#Check if the acceleration is too great.
				angularAcceleration = abs(result.angular)
				if angularAcceleration > maxAngularAcceleration:
					result.angular /= angularAcceleration
					result.angular	*= maxAngularAcceleration

				result.linear = 0
				return result
 
 */