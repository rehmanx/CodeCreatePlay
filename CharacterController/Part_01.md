### This tutorial is made for UnityNerds Patreon page.   

**Welcome to part 1 of realistic character controller series, In this part we will add the ability to move our character seamlessly in the environment and jump download the demo environment for the tutorial here.**    

- Open the start scene, they are a few things already set up for you including an environment and a player.

![static scene](https://user-images.githubusercontent.com/23467551/135841347-de48dcab-6b21-4ac8-b901-fe6bd31471af.png)

- Create a new script **CharacterMototr.cs** and attach it to **Player** gameObject.
- To organize various fields / variables of different categories like movement or physics settings, we will divide them into classes, Open **CharacterMotor.cs** and create a nested class **MovementSettings** add **[System.Serializable]** attribute otherwise fields of this class will not show up in inspector, now add 3 float fields in it **walkSpeed, runSpeed & turnSpeed**, finally create an instance of **MovementSettings**.

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMotor : MonoBehaviour
{
    [System.Serializable]
    public class MovementSettings
    {
        public float walkSpeed = 1.7f;
        public float runSpeed = 5.0f;
        public float turnSpeed = 2f;
    }

    // class instances
    public MovementSettings movementSettings = new MovementSettings();

    void Start()
    {
    }

    void Update()
    {
    }
}
```

![01](https://user-images.githubusercontent.com/23467551/135844076-4fb07d84-4d9d-4bfa-8835-e43be80da2bf.png)

- We have got a basics setup, let's move our character, moving our character involves changing it's position and to change position we are going to give it a velocity. Velocity is defined as rate of change of position over time, if we represent velocity by **v** then  

<img src="https://latex.codecogs.com/gif.latex?\begin{aligned}&space;v&space;=&space;\Delta&space;s&space;/&space;\Delta&space;t&space;\\&space;here\&space;\Delta&space;s&space;=&space;change\&space;in\&space;position\&space;and\&space;\Delta&space;t&space;=&space;chnage\&space;in\&space;time&space;\\&space;rearranging\&space;for\&space;position&space;\\&space;\Delta&space;s&space;=&space;v&space;*&space;\Delta&space;t&space;\end{aligned}" title="\begin{aligned} v = \Delta s / \Delta t \\ here\ \Delta s = change\ in\ position\ and\ \Delta t = chnage\ in\ time \\ rearranging\ for\ position \\ \Delta s = v * \Delta t \end{aligned}" />   

- Create a new Vector3 field velocity and add a new method **Move**, call it in **Update** method.

```
    // private
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        Move();
    }

    void Move()
    {
        // move our character is direction of force.
        transform.position += velocity * Time.deltaTime;
    }
```

- Recall from high school physics velocity is a vector quantity it has both a direction and magnitude (speed), by default our velocity vector is a null vector meaning all axis **X, Y, Z** are **0**, a null vector has no magnitude or direction, giving this velocity to our character will have no effect.
Let's fix this, first let's give this velocity vector a  direction, we will do this from user input, so create a new method **GetInput**.

```
    void Update()
    {
        GetInput();
        Move();
    }

    void GetInput()
    {
    }
```

- Now we have a direction to move from user input, let's give it a speed, the speed variable by default is 0, to give it a value, we will use values of **walkSpeed or runSpeed** from **MovementSettings**, first let's just do the walk and set speed = movementSettings.walkSpeed. Update the code in **Move** method.

```
    void Move()
    {
        speed = movementSettings.walkSpeed;

        // move our character is direction of force.
        transform.position += speed * direction * Time.deltaTime;
    }
```

- Enter play mode and test the character it will move in world space based on direction from user input.

![basic move](https://user-images.githubusercontent.com/23467551/135877714-a8db810d-2363-41e5-964f-d9e307ead396.gif)

- Now let's give our character ability to turn or rotate. We will do that by changing the heading (**the forward axis, in case of unity it is z-axis**) of our character, heading can be changed by rotating around Y-axis or the up-axis of our character i.e. [tranform.up](https://docs.unity3d.com/ScriptReference/Transform-up.html).
Unity internally stores rotations as [Quaternion ](https://docs.unity3d.com/ScriptReference/Quaternion.html), to create a rotation around local up-axis i.e. tranform.up we will use [Quaternion.AngleAxis](https://docs.unity3d.com/ScriptReference/Quaternion.AngleAxis.html).    
First let's add a new variable **targetRotation** of type [Quaternion](https://docs.unity3d.com/ScriptReference/Quaternion.html), set it's value to current rotation of our character in **Start** method, create a new method **Rotate** and call it from **Update** method.   
- 

```
    void Start()
    {
        targetRotation = transform.rotation;
    }

    void Update()
    {
        GetInput();
        Move();
        Rotate();
    }

    void Rotate()
    {
        targetRotation *= Quaternion.AngleAxis(Input.GetAxisRaw("Mouse X") * movementSettings.turnSpeed * Time.deltaTime, transform.up);
        transform.rotation = targetRotation;
    }
```

![worldMove](https://user-images.githubusercontent.com/23467551/135892745-d18e7e2d-1900-42bf-9e70-5b250a5f4546.gif)

- We can now change our heading, but there is a problem the character is not moving in direction it is facing. This is due to the fact that we are moving in world space, while rotation is in local axis. Let's fix this by changing our direction of movement from world to local space using the utility function [transform.transformDirection](https://docs.unity3d.com/ScriptReference/Transform.TransformDirection.html).
Update the code in **Move** method.

```
    void Move()
    {
        speed = movementSettings.walkSpeed;
        direction *= speed;

        // convert direction from world to local space
        direction = transform.TransformDirection(direction);

        // move our character is direction of force.
        transform.position += speed * direction * Time.deltaTime;
    }
```

![local move](https://user-images.githubusercontent.com/23467551/135894461-f2eddcda-1828-4ac0-a392-340dada5bf5e.gif)

- Enter play mode the character now moves as expected.
- That's it for this part, and if you want you can add more features to it for example make character only change heading when he is moving or set the speed of character to **movementSettings.runSpeed** as long as **shift** key is down, but that is up to you.    
-  In next part we will add collision detection.
