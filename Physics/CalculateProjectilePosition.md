### This tutorial is made for [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay).

Suppose you have any kind of projectile motion like muzzle from a tank or an arrow, then you can predict it's position at time t and draw the projectile's path as a visual feedback for the player, this tutorial is second part of Projectile motion see first tutorial [here](https://github.com/rehmanx/CodeCreatePlay/blob/main/Physics/ProjectileMotion.md).

![Unity_pJ1j0qmXFT](https://user-images.githubusercontent.com/23467551/138454823-8d30cacc-3e69-4728-ae8e-16d850023e2a.png)

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

Create a new empty unity scene, create a new empty gameObject rename it as you like, create a new C# script rename it **ProjectileTrajectory** and open it up in visual studio.    We will create some public fields to represent initial data (data of projectile just before it's launch). 

```
public class ProjectileTrajectory : MonoBehaviour
{
    // --------------------PUBLIC--------------------
    // initial speed.
    [HideInInspector]
    public float speed = 10f;

    // initial velocity.
    [HideInInspector]
    public Vector3 initialVelocity = Vector3.zero;

    // difference between two successive time intervals.
    [HideInInspector]
    public float timeInterval = 1f;

    // total number of times to run simulation for.
    public float numTimes = 25f;

    // gravity.
    [HideInInspector]
    public float gravity = 9.8f;

    // --------------------PRIVATE--------------------
    // start position of projectile.
    Vector3 initialPos = Vector3.zero;
}
```

Now let's implement the equation we solved for calculating position at any tile t, this is straight forward and easy to understand. 

```
    Vector3 CalculateProjectilePos(float t, Vector3 v0, Vector3 x0, Vector3 g)
    {
        return g * (0.5f * t * t) + v0 * t + x0;
    }
```

If time t changes from 0 to n we can calculate all the positions from **t=0** to **t=n** and draw a line between them, then this line would be the trajectory of projectile, to do this create a new method **GetTrajectoryPositions** and loop over the number of times to calculate positions.

```
    public List<Vector3> GetTrajectoryPositions()
    {
        List<Vector3> allPositions = new List<Vector3>();

        for (int t = 0; t < numTimes; t++)
        {
            float time = t * timeInterval * Time.deltaTime;
            Vector3 pos = CalculateProjectilePos(time,
                initialVelocity.normalized * speed, 
                initialPos, Vector3.down * -gravity);

            allPositions.Add(pos);
        }

        return allPositions;
    }
```

I have set the initial data for my projectile as follows

![Unity_SP3lTRIxYC](https://user-images.githubusercontent.com/23467551/138330051-e7a16bd7-302b-40ee-ab09-eddc7dcb8262.png)

and here is the predicted path

![Unity_0CiJsxVXog](https://user-images.githubusercontent.com/23467551/138330178-363c87dc-648c-4701-bfcd-a71e29fdc399.png)

### Everything seems good now, tutorial is done, report any mistakes, provide feedback anything is welcome AND if you like it support me on [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay).
