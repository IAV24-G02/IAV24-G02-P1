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
| Merodear | |
| Separación | |
| Tocar Flauta | |
| GENERALES | |
| Agente | |
| Comportamiento Agente | | 
| Direccion | |
| Gestor Juego | |

## Diseño de la solución

Lo que vamos a realizar para resolver esta práctica es...

(MODIFICAR LO SIGUIENTE)

A.

B.

C.

D.

E.


El pseudocódigo del algoritmo de llegada utilizado es:
```
class Arrive:
    character: Kinematic
    target: Kinematic

    maxAcceleration: float
    maxSpeed: float

    # The radius for arriving at the target.
    targetRadius: float

    # The radius for beginning to slow down.
    slowRadius: float

    # The time over which to achieve target speed.
    timeToTarget: float = 0.1

    function getSteering() -> SteeringOutput:
        result = new SteeringOutput()

        # Get the direction to the target.
        direction = target.position - character.position
        distance = direction.length()

        # Check if we are there, return no steering.
        if distance < targetRadius:
            return null

        # If we are outside the slowRadius, then move at max speed.
        if distance > slowRadius:
            targetSpeed = maxSpeed
        # Otherwise calculate a scaled speed.
        else:
            targetSpeed = maxSpeed * distance / slowRadius

        # The target velocity combines speed and direction.
         targetVelocity = direction
        targetVelocity.normalize()
        targetVelocity *= targetSpeed

        # Acceleration tries to get to the target velocity.
        result.linear = targetVelocity - character.velocity
        result.linear /= timeToTarget

        # Check if the acceleration is too fast.
        if result.linear.length() > maxAcceleration:
            result.linear.normalize()
            result.linear *= maxAcceleration

        result.angular = 0
        return result
```

El pseudocódigo del algoritmo de movimiento de huida es...


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
|  | Característica A | ..-..-2024 |
|  | Característica B | ..-..-2024 |
|  | Característica C | ..-..-2024 |
|  | Característica D | ..-..-2024 |
|  | Característica E | ..-..-2024 |
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
