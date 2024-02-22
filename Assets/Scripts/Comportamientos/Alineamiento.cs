using UnityEngine;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de WANDER a otro agente
    /// </summary>
    public class Alineamiento : ComportamientoAgente
    {
        
        #region properties
        private float maxAngularAcceleration = 2;
		private float maxRotation = 2;
        #endregion

        #region parameters
        [SerializeField]
        private float targetRadius = 2;

        [SerializeField]
        private float slowRadius = 0.1f;

        [SerializeField]
        private float timeToTarget = 0.1f;

        protected Direccion lastDir = new Direccion();
        #endregion

        #region methods
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
    		Agente objetivoAgente = objetivo.GetComponent<Agente>();
			float rotation = objetivoAgente.orientacion - agente.orientacion;

			rotation = MapToRange(rotation);

            float rotationSize = Mathf.Abs(rotation);

            float targetRotation;

            if (rotationSize < targetRadius)
			{				
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

            Direccion sol = new Direccion();

			sol.angular = targetRotation - agente.rotacion;
			sol.angular /= timeToTarget;

			float angularAcceleration = Mathf.Abs(sol.angular);
			if(angularAcceleration > maxAngularAcceleration)
			{
				sol.angular /= angularAcceleration;
				sol.angular *= maxAngularAcceleration;
            }

			sol.lineal = Vector3.zero;

			lastDir = sol;

            return sol;
        }
        #endregion
    }
}