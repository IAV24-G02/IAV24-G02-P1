# IAV - Práctica 1

## Autores
- Yi (Laura) Wang Qiu (LauraWangQiu)
- Agustín Castro De Troya (AgusCDT)
- Ignacio Ligero Martín (theligero)
- Adolfo Jaime Rodulfo Guío (ARodulfo)

## Propuesta
Este proyecto es una práctica de la asignatura de Inteligencia Artificial para Videojuegos del Grado en Desarrollo de Videojuegos de la UCM, cuyo enunciado original es este: [Plaga de Ratas](https://narratech.com/es/inteligencia-artificial-para-videojuegos/percepcion-y-movimiento/plaga-de-ratas/).

Esta práctica consiste en recrear los comportamientos de una serie de personajes en una escena con obstáculos:

- Flautista: será nuestro _player_ al que controlaremos mediante el teclado y ratón. Se podrá mover por toda la escena sin atravesar los obstáculos y podrá tocar la flauta y dejar de hacerlo.

- Perro: fiel compañero del flautista. Perseguirá al flautista con cierto control de llegada siempre y cuando no haya ratas cerca, en ese caso, huye y deja de perseguir al flautista.

- Ratas: se mueven erráticamente por la escena siempre y cuando el flautista no toque su flauta, en ese caso, las ratas empiezan a perseguir al flautista con cierto control de llegada entre ellas y con el flautista y puede producir que el perro huya.


## Punto de partida

Se parte de un proyecto base de **Unity 2022.3.5f1** proporcionado por el profesor y disponible en este repositorio: [IAV-Movimiento](https://github.com/Narratech/IAV-Movimiento)

| Clases | Información |
| - | - |
| ANIMACIONES | |
| Animador Animal | Establece un booleano de movimiento si se está moviendo o no dependiendo de la velocidad del rigidbody y de un _threshold_ al componente de Animator para que se reproduzca la animación que le corresponda. |
| Animador Avatar | Asigna el valor de la velocidad del rigidbody de la entidad al componente de Animator para que se reproduzca la animación que le corresponda.  |
| Seguimiento Camara | Calcula la posición interpolada entre el _target_ (el _player_) y la propia entidad (la cámara) con cierta velocidad de suavizado y cierto _offset_ para producir un comportamiento de seguimiento. |
| COMPORTAMIENTOS | |
| Control Jugador | |
| Huir | |
| Llegada | |
| Merodear | Componente que hará que las ratas deambulen mientras no se toque la flauta. Tiene variables para los tiempos y direcciones utilizadas para las orientaciones. |
| Separación | Se encargará de que las entidades no se solapen cuando sigan a otra entidad. Establece valores para los targets, el umbral de activación y el coeficiente de repulsión.  |
| Tocar Flauta | Se encarga de gestionar las acciones cuando se toca o no la flauta. Si pulsamos clic derecho activamos la flauta y con ello los efectos audiovisuales de la misma, además de activar los comportamientos de Separación y Llegada de las ratas. Si dejamos de clicar se desactiva todo lo anterior y empieza el Merodeo de las Ratas. |
| GENERALES | |
| Agente | |
| Comportamiento Agente | | 
| Direccion | |
| Gestor Juego | |

## Diseño de la solución

Lo que vamos a realizar para resolver esta práctica es...

A. Modificar el input de tocar la flauta para que se realice con el `clic derecho` e implementar una `caja de texto` y un `botón` para poder introducir un número de ratas específico.

B. [NACHO]

C. Implementar el movimiento del perro para la huida causado por la cercanía de las ratas respecto al perro.

La situación es la que se muestra en la siguiente imagen:

![Figure 3.34: A stable equilibrium](images/C.png)

El círculo blanco representa al perro y su comportamiento normal es seguir al Target, el player. Sin embargo, hay enemigos (las ratas) que hacen que quiera evitarlos pasando a un comportamiento de huida y siendo un comportamiento más prioritario. Esto significa que hay comportamientos de dirección combinados: persecución y huida con prioridad del de huida frente al de persecución.

El pseudocódigo del algoritmo de huida sería muy parecido al de persecución pero intercambiando la posición del target con el de la propia entidad. Aún así, en este caso hablamos de huir de un grupo de ratas y no de huir de solo un target por lo que habría que modificar algo más:
```
class Static:
    position: Vector
    orientation: float

class KinematicSteeringOutput:
    velocity: Vector
    rotation: float

class FleeFromAGroup:
    character: Static
    # List of targets.
    target: Static[]

    maxSpeed: float

    function newOrientation(current: float, velocity: Vector) -> float:
        # Make sure we have a velocity.
        if velocity.length() > 0:
            # Calculate orientation from the velocity.
            return atan2(-static.x, -static.z)

        # Otherwise use the current orientation
        else:
            return current


    function getSteering() -> KinematicSteeringOutput:
        result = new KinematicSteeringOutput()

        # Calculate the average point of all targets.
        averagePosition = new Vector()
        for target in targets:
            averagePosition += target.position
        averagePosition /= len(targets)

        # Obtain the direction to move away from the average point of the targets.
        result.velocity = character.position - averagePosition

        # The velocity is along this direction, at full speed.
        result.velocity.normalize()
        result.velocity *= maxSpeed

        # Face in the direction we want to move.
        character.orientation = newOrientation(
                character.orientation,
                result.velocity)
            
        result.rotation = 0
        return result
```

Se podría modificar un poco más haciendo que solo compruebe dentro de un radio respecto al character (perro) en vez de con todos los targets que estén en la escena:

```
# Radius or range with respect to the character
fleeRange: float

for target in targets:
    if (character.position - target.position).length() <= fleeRange:
        averagePosition += target.position
```

Luego, contando con el comportamiento de persecución del apartado anterior, necesitaremos el siguiente pseudocódigo para combinarlos y priorizar el comportamiento de huida:
```
class SteeringOutput:
    linear: Vector
    angular: float

class BlendedSteering:
    class BehaviourAndWeight:
        behavior: SteeringBehaviour
        weight: float

    behaviors: BehaviourAndWeight[]

    # The overall maximum acceleration and rotation.
    maxAcceleration: float
    maxRotation: float

    function getSteering() -> SteeringOutput:
        result = new SteeringOutput()

        # Accumulate all accelerations.
        for b in behaviors:
            result += b.weight * b.behavior.getSteering()
        
        # Crop the result and return.
        result.linear = max(result.linear, maxAcceleration)
        result.angular = max(result.angular, maxRotation)
        return result
```

- `SteeringOutput`: estructura o clase que almacena la aceleración lineal y angular resultante.

- `BlendedSteering`: clase que se encarga de combinar múltiples SteeringBehaviours con diferentes pesos para crear un comportamiento compuesto más complejo y versátil.
    - Contiene la clase `BehaviourAndWeight` que asocia un SteeringBehaviour (un comportamiento de dirección individual) con un weight (peso). El peso es el que determina cuánto influirá este comportamiento en el comportamiento final compuesto.
    - Propiedades:
        - _behaviors_: lista de instancias de BehaviourAndWeight, representando los diferentes comportamientos de dirección y sus pesos asociados que se combinarán.
        - _maxAcceleration_ y _maxRotation_: aceleraciones lineal y angular máximas que la entidad puede alcanzar, respectivamente. Estos valores se utilizan para limitar el resultado de la mezcla de comportamientos para asegurar que no sobrepasen las capacidades físicas de la entidad.
        - Método _getSteering()_: calcula y devuelve el resultado compuesto de todos los comportamientos de dirección mezclados. Inicia creando un nuevo SteeringOutput. Luego, itera sobre cada BehaviourAndWeight en la lista behaviors, acumulando las aceleraciones ponderadas de cada comportamiento en result. Después de sumar todas las aceleraciones, el método ajusta para que no superen maxAcceleration y maxRotation. Finalmente, devuelve result, que ahora contiene la aceleración combinada que debe aplicarse a la entidad.

Para comprobar si hay una diferencia de aceleración significativa en la combinación de comportamientos, se usaría el siguiente pseudocódigo:

```
# Should be a small value, effectively zero.
epsilon: float

class PrioritySteering:
    # Holds a list of BlendedSteering instances, which in turn
    # contain sets of behaviors with their blending weights.
    groups: BlendedSteering[]

    function getSteering() -> SteeringOutput:
        for group in groups:
            # Create the steering structure for accumulation.
            steering = group.getSteering()

            # Check if we're above the threshold, if so return.
            if steering.linear.length() > epsilon or
               abs(steering.angular) > epsilon:
                return steering

        # If we get here, it means that no group had a large enough
        # acceleration, so return the small acceleration from the
        # final group.
        return steering
```

- _epsilon_: constante que se utiliza para determinar si una aceleración (tanto lineal como angular) es efectivamente cero, es decir, lo suficientemente pequeña como para ser considerada insignificante.

- `PrioritySteering`: 
    - _groups_: lista de instancias de BlendedSteering. Cada BlendedSteering representa un grupo de comportamientos de dirección mezclados con sus respectivos pesos. Los grupos están ordenados por prioridad, de modo que el sistema evalúa cada grupo en orden hasta encontrar uno que produzca una aceleración significativa.
    - Método _getSteering()_: itera a través de cada grupo de BlendedSteering en groups, evaluando los comportamientos mezclados en cada grupo para determinar si producen una aceleración significativa.
    Para cada grupo, se llama al método getSteering() del BlendedSteering para obtener la aceleración resultante de mezclar sus comportamientos.
    Luego se verifica si la magnitud de la aceleración lineal del resultado o si el valor absoluto de la aceleración angular son mayores que epsilon. Si alguna de estas condiciones se cumple, significa que el grupo actual produce una aceleración suficientemente significativa, y el método devuelve inmediatamente este resultado de aceleración.
    Si se itera a través de todos los grupos sin encontrar una aceleración significativa, el método devuelve la aceleración del último grupo evaluado, aunque esta sea pequeña.

D. [ALFONSO]

E. [AGUSTÍN]

## Pruebas y métricas

- Característica A: link no disponible
- Característica B: link no disponible
- Característica C: link no disponible
- Característica D: link no disponible
- Característica E: link no disponible

## Ampliaciones

Se han realizado las siguientes ampliaciones

- 

## Producción

Las tareas se han realizado y el esfuerzo ha sido repartido entre los autores. Observa la tabla de abajo para ver el estado y las fechas de realización de las mismas. Puedes visitar nuestro proyecto de GitHub en el siguiente [link](https://github.com/orgs/IAV24-G02/projects/1).


| Estado  |  Tarea  |  Fecha  |  
|:-:|:--|:-:|
|  | Diseño: Primer borrador | ..-..-2024 |
| ✔ | Característica A: Tocar flauta con el clic derecho | 01-02-2024 |
|  | Característica A: Introducir número de ratas específico | ..-..-2024 |
|  | Característica B | ..-..-2024 |
|  | Característica C | ..-..-2024 |
|  | Característica D | ..-..-2024 |
|  | Característica E | ..-..-2024 |
|  |  OTROS  | |
|  | Movimiento del avatar con el clic izquierdo | ..-..-2024 |
|  |  OPCIONALES  | |
|  | Generador pseudoaleatorio | ..-..-2024 |
|  | Competición de flautistas | ..-..-2024 |
|  | Distracción de las ratas con trozos de queso | ..-..-2024 |
|  | Percepción del perro mediante la vista | ..-..-2024 |
|  | Evasión de los obstáculos mediante la vista | ..-..-2024 |
|  | Menú | ..-..-2024 |
|  | HUD | ..-..-2024 |

## Referencias

Los recursos de terceros utilizados son de uso público.

- *AI for Games*, Ian Millington.
- [Kaykit Medieval Builder Pack](https://kaylousberg.itch.io/kaykit-medieval-builder-pack)
- [Kaykit Dungeon](https://kaylousberg.itch.io/kaykit-dungeon)
- [Kaykit Animations](https://kaylousberg.itch.io/kaykit-animations)
