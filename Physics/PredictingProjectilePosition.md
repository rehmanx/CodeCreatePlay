### This tutorial is made for [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay).

Suppose you have any kind of projectile motion like muzzle from a tank or an arrow, then you can predict it's position at time t and draw the projectile's path as a visual feedback for the player.

When the projectile is launched it's initial data is given by

<img src="https://latex.codecogs.com/svg.image?\begin{aligned}position&space;=&space;x_{0}&space;\\initial&space;velocity&space;=&space;v_{0}&space;\\acceleration\&space;due\&space;to\&space;gravity\&space;=&space;g&space;=&space;-9.8ms^{-2}&space;\\time&space;=&space;t&space;=&space;0&space;\end{aligned}&space;" title="\begin{aligned}position = x_{0} \\initial velocity = v_{0} \\acceleration\ due\ to\ gravity\ = g = -9.8ms^{-2} \\time = t = 0 \end{aligned} " />

We know that acceleration is rate of change of velocity overtime 

<img src="https://latex.codecogs.com/svg.image?g&space;=&space;\Delta&space;v&space;/&space;\Delta&space;t&space;" title="g = \Delta v / \Delta t " />

 We can rearrange this equation for velocity, since this is a rate function to find velocity at any time t we will have to integrate it to find an individual instance of velocity so,

<img src="https://latex.codecogs.com/svg.image?\begin{aligned}v(t)&space;=&space;\int&space;g\&space;dt&space;\\=&space;g&space;\int&space;dt\&space;\because&space;c\int&space;dt&space;=&space;\int&space;c\&space;dt\&space;where\&space;c\&space;=&space;constant\&space;of\&space;integral&space;\\=&space;gt&space;&plus;&space;c\&space;\because&space;\int&space;dt&space;=&space;t&space;&plus;&space;c&space;\\v(t)&space;=&space;gt&space;&plus;&space;v_{0}\&space;(velocity\&space;at\&space;any\&space;given\&space;instance)\end{aligned}" title="\begin{aligned}v(t) = \int g\ dt \\= g \int dt\ \because c\int dt = \int c\ dt\ where\ c\ = constant\ of\ integral \\= gt + c\ \because \int dt = t + c \\v(t) = gt + v_{0}\ (velocity\ at\ any\ given\ instance)\end{aligned}" />

We now know velocity at any given instance we can use it to calculate position since

<img src="https://latex.codecogs.com/svg.image?\begin{aligned}v&space;=&space;\Delta&space;x&space;/&space;\Delta&space;t\&space;where\&space;v\&space;is\&space;average\&space;velocity\end{aligned}" title="\begin{aligned}v = \Delta x / \Delta t\ where\ v\ is\ average\ velocity\end{aligned}" />

rearranging this equation for time becomes

<img src="https://latex.codecogs.com/svg.image?\begin{aligned}\Delta&space;x&space;=&space;v\Delta&space;t\end{aligned}" title="\begin{aligned}\Delta x = v\Delta t\end{aligned}" />

integrating to find individual instances of positions

<img src="https://latex.codecogs.com/svg.image?\begin{aligned}x(t)&space;=&space;\int&space;v\&space;dt&space;\\=&space;putting\&space;value\&space;of\&space;v&space;\\=&space;\int&space;gt&space;&plus;&space;v_{0}\&space;dt&space;\\(taking\&space;constants\&space;out)&space;\\=&space;g&space;\int&space;t\&space;dt\&space;&plus;\&space;v_{0}&space;\int&space;dt\&space;&space;\\=&space;g(1/2t^{2})&space;&plus;&space;v_{0}t&space;&plus;&space;c\&space;\because&space;\int&space;t^{n}\&space;dt&space;=&space;\frac{1}{n&plus;1}t^{n&plus;1}&space;&plus;&space;c&space;\\x(t)&space;=&space;gt^{2}/2&space;&plus;&space;v_{0}t&space;&plus;&space;x_{0}\&space;(position\&space;at\&space;any\&space;given\&space;instance)\end{aligned}" title="\begin{aligned}x(t) = \int v\ dt \\= putting\ value\ of\ v \\= \int gt + v_{0}\ dt \\(taking\ constants\ out) \\= g \int t\ dt\ +\ v_{0} \int dt\ \\= g(1/2t^{2}) + v_{0}t + c\ \because \int t^{n}\ dt = \frac{1}{n+1}t^{n+1} + c \\x(t) = gt^{2}/2 + v_{0}t + x_{0}\ (position\ at\ any\ given\ instance)\end{aligned}" />

so we have the position at any given instance, you can also verify the above equation for position at an instance t by putting t = 0 and solving, result would be initial position which is accurate since at time t = 0 we have not moved at all, now let's do this in code.   

Create a new empty unity scene, create a new empty gameObject rename it as you like, create a new C# script rename it **PredictingProjectile** and open it up in visual studio.    

We will create some public fields to represent initial data (data of projectile just before it's launch). 

```
public class PredictingProjectile : MonoBehaviour
{
    // public                                   
    public float speed = 10f;                   // initial speed
    public Vector3 initialVel = Vector3.zero;   // initial velocity
    public float timeInterval = 1f;             // difference between two successive time intervals
    public float numTimes = 25f;                   // total number of times to run simulation for
    public float gravity = 9.8f;                // gravity

    // private.
    Vector3 initialPos = Vector3.zero;          // start position of projectile
}
```

Now let's implement the equation we solved for calculating position at any tile t, this is straight forward and easy to understand. 

```
    /// <summary>
    /// Calculates and returns position of projectile at time t.
    /// </summary>
    /// <param name="t" time></param>
    /// <param name="v0" initial velocity></param>
    /// <param name="x0" initial position></param>
    /// <param name="g" acceleration></param>
    /// <returns></returns>
    Vector3 PredictProjectilePos(float t, Vector3 v0, Vector3 x0, Vector3 g)
    {
        return g * (0.5f * t * t) + v0 * t + x0;
    }
```

If time t changes from 0 to n we can calculate positions at **t = n** and at **t = n+1** and draw a line between them, doing this from t = 0 to t = n we can draw the trajectory or the path of the projectile. We will do this using unity's **OnDrawGizmos** method, see comments in code if anything is not easily understood.

```
    private void OnDrawGizmos()
    {
        initialPos = transform.position;

        // visualize initial position of projectile by drawing a sphere
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(initialPos, 0.1f);

        for (int t = 0; t < numTimes; t++)
        {
            // first get the two time intervals i.e. t = n and t = n+1 
            float time_0 = t * timeInterval * Time.deltaTime;
            float time_1 = (t + 1) * timeInterval * Time.deltaTime;
            
             // now to calculate positions at both times

            // we are normalizing initialVelocity vector to set a direction first and then multiplying with speed to 
            // give a speed in that direction.

            Vector3 pos_1 = PredictProjectilePos(time_0, initialVelocity.normalized * speed, initialPos, Vector3.down * gravity);
            Vector3 pos_2 = PredictProjectilePos(time_1, initialVelocity.normalized * speed, initialPos, Vector3.down * gravity);

            // draw a line between both positions 
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pos_1, pos_2);
        }
    }
```

I have set the initial data for my projectile as follows

![Unity_SP3lTRIxYC](https://user-images.githubusercontent.com/23467551/138330051-e7a16bd7-302b-40ee-ab09-eddc7dcb8262.png)

and here is the predicted path

![Unity_0CiJsxVXog](https://user-images.githubusercontent.com/23467551/138330178-363c87dc-648c-4701-bfcd-a71e29fdc399.png)

### Everything seems good now, tutorial is done, report any mistakes, provide feedback anything is welcome AND if you like it support me on [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay).
