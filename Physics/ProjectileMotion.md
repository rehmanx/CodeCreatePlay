### This tutorial is made for [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay).

![cannonDemo](https://user-images.githubusercontent.com/23467551/140632479-f8757ce1-ca00-46f2-b129-f8229abf7f4b.gif)

Projectile motion is the motion of a body when projected or thrown into the air and subjected only to acceleration due to gravity, the path it takes is called **project's trajectory**, examples are motion of a football or that of a cannon ball when fired from a cannon.

This is a two part tutorial in this part we will create a cannon ball projectile but concept is same regardless of type of object, in second part we will visualize the projectile's trajectory the path that projectile will take before actually launching it as a visual feedback for the player. If you are a [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay) patreon you can download the project files for this tutorial here.

Open the start scene it has a basic cannon setup press **w** and **s** keys to move in up or down. 

First let's create a simple rigidbody we will use it as our projectile object, if we give this projectile an initial velocity, then it will move in direction of velocity, after launch the only force on it will be acceleration due to gravity.

We know that 
1. velocity is rate of change of position over time
2. and position is rate of change of velocity 

Let's do this in code and create a simple custom rigidbody, create a new C# script "Projectile" and add the following code, see the comments for details.

```
public class Projectile : MonoBehaviour
{
    // physical object properties--------
    Vector3 position = Vector3.zero;
    Vector3 velocity = Vector3.zero;
    Vector3 acceleration = Vector3.zero;
    // ---------------------------------

    public Vector3 Acceleration { set { acceleration = value; } get { return acceleration; } }
    public Vector3 Velocity { set { velocity = value; } get { return velocity; } }
    public Vector3 Position { get { return position; } set { position = value; } }

    void Update()
    {
        // update physical object
        velocity += acceleration * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
```

now all we need is to give this projectile an initial velocity and set a constant acceleration to move it realistically, now to programme the cannon to fire cannon balls, create a new script **Cannon** and in it add
- a new class **ProjectileData** this will hold initial data for our projectile like launch speed and gravity.
- a reference to the cannon ball projectile prefab and a float field **timeDelay** (see the comments for details).

```
public class Cannon : MonoBehaviour
{
    // data for our realistic projectile
    [System.Serializable]
    public class ProjectileData
    {
        [HideInInspector]
        public Vector3 velocity = Vector3.zero; // the initial velocity with which to launch the projectile
        
        public float speed = 10f; // the initial speed
        public float gravity = 9.8f;
    }
    
    // initializations
    public ProjectileData projectileData = new ProjectileData();
    
    [Header("References")]
    public Projectile projectile = null; // the actual projectile prefab
    public Transform launchPos = null; // projectile will be instantiated and launched from this position
    
    [Header("Other")]
    public float fireDelay = 1f; // time delay between two fires
    float nextFire = 0; // internally used to hold time for next fire
    
    // private
    // the initial velocity and acceleration calculated from ProjectileData to give to cannon ball when it is launched.
    Vector3 velocity = Vector3.zero;
    Vector3 acceleration = Vector3.zero;
}
```

To launch the projectile we only have to give it an initial velocity, remember velocity is a vector quantity with both speed and direction, in this case the direction is the forward facing direction of cannon and speed is set manually, so now to do this in code create a new method **CalculateProjectileData** to calculate and set initial launch data for projectile.

```
    public void CalculateProjectileData()
    {
        projectileData.velocity = transform.forward;
        projectileData.velocity *= projectileData.speed;

        // the initial velocity and acceleration of projectile
        velocity = projectileData.velocity;
        acceleration = Vector3.down * projectileData.gravity;
    }
```

We want to fire the cannon when user hits the Space key, when the cannon is fired we want to first intantiate the cannon ball projectile prefab then set it's initial data like velocity and constant acceleration due to gravity, in update method first chcek if user has pressed the space key if yes then first call **CalculateProjectileData** to calculate initial data for projectile and then finally I have created a new method **Fire** to actually instantiate and set initial launch data for projectile, the cannon ball projectile is instantiated at **launchPos**.

```
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire)
        {
            CalculateProjectileData(); // set the initial data for the projectile.
            Fire(); 
            
            nextFire = Time.time + fireDelay;
        }
    }
    
    void Fire()
    {
        // instantiate cannon ball prefab at launch position.
        var cannonBall = Instantiate(projectile, launchPos.position, Quaternion.identity);
                
        // give it an initial velocity and an acceleration.
        cannonBall.Acceleration = acceleration;
        cannonBall.Velocity = velocity;
    }
```

and the results... change values from inspector to get different trajectories.

This was simple, but now for the fun part, right now although we can change some variables like speed, gravity and launch angle and have the projectile follow completly different paths at differnt speeds but we have no control of where the projectile will land, in this part we will actually make the projectile always hit some particular target, so ok let's do that.

**Review of kinematic equations**
we all have studied kinematic equations in our high school physics it's a good time to put that knowledge to good use.

![CodeCogsEqn (1)](https://user-images.githubusercontent.com/23467551/140335336-f5bfee09-3e5a-4b8b-97b0-571b4f1313b3.gif)

We can divide the motion of projectile into vertical x and y components seperately and solve for motion on x and y axis seperately

![CodeCogsEqn (4)](https://user-images.githubusercontent.com/23467551/140366332-233000f7-3164-4995-88f9-c969ebff1f9b.gif)

So, this equation will tell us the X and Y position of a projectile at any point in time, that's handy, however, we already know where we want our projectile to be, we want it to be on our target, we know the X and Y position of our target and we know our initial velocity, so we have a few knowns, if we can solve for time, we can solve for our gravity constant, here is the math for it

![CodeCogsEqn (5)](https://user-images.githubusercontent.com/23467551/140370699-83c66e0e-c609-448d-b874-075f460bb19f.gif)

All we have to do is try to solve for the knowns and we can figure out the necessary gravity constant, let's do this straight in the code I have added comments with each step, first add a new class **CustomProjectileData** to hold data for our custom projectile 

```
    // data for our custom projectile
    [System.Serializable]
    public class CustomProjectileData
    {
        public Transform target = null;
        public float speed = 10f;
    }
    
    // initializations
    public CustomProjectileData customProjectileData = new CustomProjectileData();
```

now create a new method **CalculateCustomProjectileData** to calculate and set the initial velocity and gravity values for our custom projectile, I have added comments with each step to better explain

```
    public void CalculateCustomProjectileData()
    {
        // get a vector from our current position to target position.
        Vector3 fireVec = customProjectileData.target.position - transform.position;

        // find distance from our cannon's position to target's position
        float totalDistance = Vector3.Distance(customProjectileData.target.position, transform.position);

        fireVec.Normalize(); // normalize fireVec to convert it to direction 
        // y-comp of fireVec is launch angle remember to convert it to radians.
        fireVec.y = Vector3.Angle(transform.forward, Vector3.forward) * Mathf.Deg2Rad;
        fireVec *= customProjectileData.speed; // scale magnitude of fireVec to set an initial launch speed

        // this is speed  we are moving on horizontal xz-axis, it is found by finding distance of fireVec
        float vx = Mathf.Sqrt(fireVec.x * fireVec.x + fireVec.z * fireVec.z);

        // time
        float t = totalDistance / vx;

        // now for vertical componetns
        float vy = fireVec.y; // speed on y or vertical axis
        float y = customProjectileData.target.position.y - transform.position.y; // this is height

        // putting values in equation we derieved
        float gravity = (2f * (y - vy * t)) / (t * t);

        // the initial velocity and acceleration of projectile
        velocity = fireVec;
        acceleration = Vector3.down * -gravity;
    }
```

I have added an enum to change which type of calculation to be done when we hit space key so now the updated code is

```
    public enum Type
    {
        Realistic,
        Custom
    }

    // initializations
    public Type type = Type.Realistic;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire)
        {
            switch (type)
            {
                case Type.Realistic:
                    SetProjectileData();
                    break;

                case Type.Custom:
                    CalculateCustomProjectileData();
                    break;
            }

            nextFire = Time.time + fireDelay;
            Fire();
        }
    }
```

and here are the final results, cannon ball always hit the target no matter what initial speed or launch angle are and that's it for first part in next part we will calculate and draw the projectile's trajectory, see you till then.

### Everything seems good now, tutorial is done, report any mistakes, provide feedback anything is welcome AND if you like it support me on [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay).
