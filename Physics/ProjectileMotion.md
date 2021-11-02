### This tutorial is made for [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay).

Projectile motion is motion thrown into the air and subjected only to acceleration due to gravity, the path it takes is called **project's trajectory**, examples are motion of a football or that of a cannon ball when fired from a cannon.

This is a two part tutorial in this part we will create a cannon ball projectile but concept is same regardless of type of object, in second part we will visualize the projectile's trajectory the path that projectile will take before actually launching it as a visual feedback for the player. If you are a [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay) patreon you can download the project files for this tutorial here.

First let's create a simple rigidbody we will use it as our projectile object, we will give it an initial velocity when we launch it, in turn it will move in direction of velocity, after launch the only force on it will be acceleration due to gravity.

We know that 
1. velocity is rate of change of position over time
2. and position is rate of change of velocity 

Let's do this in code and create a simple rigidbody projectile, create a new C# script "Projectile" and add the following code, see the comments for details.

```
public class Projectile : MonoBehaviour
{
    // physical object------------------
    Vector3 position = Vector3.zero;
    Vector3 velocity = Vector3.zero;
    Vector3 acceleration = Vector3.zero;
    Vector3 lastPos = Vector3.zero;
    // ---------------------------------

    public Vector3 Acceleration { set { acceleration = value; } get { return acceleration; } }
    public Vector3 Velocity { set { velocity = value; } get { return velocity; } }
    public Vector3 Position { get { return position; } }

    void Start()
    {
    }

    void Update()
    {
        // update physical object
        lastPos = transform.position;
        velocity += acceleration * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
    }

    public void OnCollisionEnter(Collision collision)
    {
    }
}
```

that's all we need to do for moving the projectile realistically.

### Everything seems good now, tutorial is done, report any mistakes, provide feedback anything is welcome AND if you like it support me on [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay).
