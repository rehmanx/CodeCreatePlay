### This tutorial is made for [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay).

Projectile motion is motion thrown into the air and subjected only to acceleration due to gravity, the path it takes is called **project's trajectory**, examples are motion of a football or that of a cannon ball when fired from a cannon.

This is a two part tutorial in this part we will create a cannon ball projectile but concept is same regardless of type of object, in second part we will visualize the projectile's trajectory the path that projectile will take before actually launching it as a visual feedback for the player. If you are a [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay) patreon you can download the project files for this tutorial here.

First let's create a simple rigidbody we will use it as our projectile object, we will give it an initial velocity when we launch it, in turn it will move in direction of velocity, after launch the only force on it will be acceleration due to gravity.

Open the start scene it has a basic cannon setup press **w** and **s** keys to move in up or down. 

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

now if we give this projectile an initial velocity and set a constant acceleration is all we need to do for moving the projectile realistically, now to programme the cannon to fire cannon balls, create a new script **Cannon** and in it add
- a new class **ProjectileData** this will hold initial data for our projectile like launch speed and gravity.
- a reference to the cannon ball projectile prefab and a float field **timeDelay** (see the comments for details).

```
public class Cannon : MonoBehaviour
{
    // data for our realistic projectile
    [System.Serializable]
    public class RealisticProjectileData
    {
        [HideInInspector]
        public Vector3 initialVelocity = Vector3.zero; // the initial velocity with which to launch the projectile
        
        public float speed = 10f; // the initial speed
        public float gravity = 9.8f;
    }
    
    // initializations
    public RealisticProjectileData realisticProjectileData = new RealisticProjectileData();
    
    [Header("References")]
    public GameObject projectile = null;
    
    [Header("Other")]
    public float fireDelay = 1f; // time delay between two fires
    float nextFire = 0; // internally used to hold time for next fire
    
    // private
    Vector3 velocity = Vector3.zero; 
}
```

To calculate the initial values like launch angle, direction to give to projectile when it is launched, we can split the motion of projectile into horizontal **X-axis and Z-axus** and vertical components **Y-axis**, initial horizontal direction of projectile is same as direction of cannon ball on **X and Z axis**

```
    public void SetProjectileData()
    {
        realisticProjectileData.initialVelocity = transform.forward; // this is initial direction in 
        realisticProjectileData.initialVelocity.y = Mathf.Deg2Rad * Vector3.Angle(Vector3.forward, transform.forward);

        realisticProjectileData.initialVelocity *= realisticProjectileData.speed;

        // the initial velocity of physical object
        velocity = realisticProjectileData.initialVelocity;
    }
```

We want to instantiate the cannon ball user hit the Space 

### Everything seems good now, tutorial is done, report any mistakes, provide feedback anything is welcome AND if you like it support me on [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay).
