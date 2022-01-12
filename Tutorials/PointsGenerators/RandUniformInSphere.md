### This tutorial is made for [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay).

![sbOS9EhmGv](https://user-images.githubusercontent.com/23467551/140644126-a07f3417-388c-494a-91ee-51ecc33ba75b.gif)

Among multiple methods for distributing points in a sphere, the one we will be using involves **[Spherical polar coordinates](https://mathinsight.org/spherical_coordinates#:~:text=Spherical%20coordinates%20determine%20the%20position,angle%20%CE%B8%20from%20polar%20coordinates.)** in which location of a point in 3d space is determined based on **distance ρ or r from origin and two angles θ (theta) and φ (phi)**, before we proceed take a look at **[uniformly distributing points in a circle tutorial](https://github.com/rehmanx/CodeCreatePlay/blob/main/PseudoRandomNumberGenerators/RandUniformInCircle.md).**

![spherical_coordinates](https://user-images.githubusercontent.com/23467551/136846587-7ae62649-6762-41f8-9e08-b4eee38e9971.png)

Transformation from spherical to cartesian coordinates is given by   

**x = r.sinφ.cosθ**     
**y = r.sinφ.sinθ**   
**z = r.cosθ**   

In spherical coordinates to get position of any point 

<img src="https://latex.codecogs.com/svg.image?r&space;\geq&space;0&space;\\0&space;\leq&space;\theta&space;<&space;2\pi&space;\\&space;0&space;\leq&space;\phi&space;\leq&space;\pi" title="r \geq 0 \\0 \leq \theta < 2\pi \\ 0 \leq \phi \leq \pi" />

By selecting **ρ or r** in the range [0, 1] we can get a point that is inside the unit sphere., secondly by selecting θ in the range [0-2π], and φ in the range [0-π], we can get a direction, the code that generates this distribution in C# for unity engine in shown but this applies to any platform you are using.   
I am using unity's **Gizmos** to visualize these points.

```
public class RandUniformPointsInSphere : MonoBehaviour
{
    public int count = 500; // number of points to distribute.
    public float debugPointRadius = 0.05f; // radius of debug points to visualize distributed points.

    public int radius = 15; // the radius of sphere.

    List<Vector3> generatedPoints = new List<Vector3>(); // points distributed are saved here.

    void Start()
    {
        GenerateUniformPointsInSphere(count);
    }

    const float TWO_PI = 2 * Mathf.PI;

    void GenerateUniformPointsInSphere(int count)
    {
        generatedPoints.Clear();

        for (int i = 0; i < count; i++)
        {
            var theta = Random.Range(0, TWO_PI);
            var phi = Random.Range(0, Mathf.PI);
            var r = Random.Range(0f, 1f) * radius;

            float x = r * Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = r * Mathf.Sin(phi) * Mathf.Sin(theta);
            float z = r * Mathf.Cos(phi);

            Vector3 pos = new Vector3(x, y, z);
            generatedPoints.Add(pos);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.white;

        for (int i = 0; i < generatedPoints.Count; i++)
            Gizmos.DrawSphere(generatedPoints[i], debugPointRadius);
    }
}
```

![Unity_FUwPdNUBGp](https://user-images.githubusercontent.com/23467551/140643819-7424e98c-f5f9-447a-84d4-2ed2fd390d61.jpg)

Above image shows results from out current approach, this definately is not unifrom distribution, there are two problems with this distribution.
1. the points are clustered at the origin close to axis of rotation.
2. the other problem is more points are distributed cylinderically from north to south pole of sphere.

The problem is if you consider the two angles θ and φ, if you distribute them uniformly they will wrap around a cylinder not a sphere not will favour polor locations, to elaborate it more I have chosen a fixed value of **r** say 7.5 and plotted the distribution, I have also added a cylinder from one pole to the other, notice the distribution is thin more at equator where area of sphere is highest.

![Unity_C3MfxlQJbc](https://user-images.githubusercontent.com/23467551/140644698-fe6071e1-cac0-441f-95f7-9141cbc9fdf8.png)

To solve this we need to find the distribution of θ and φ for the area of sphere, as area increases density of points decreases so they are inversely proportional, so we can write

<img src="https://latex.codecogs.com/svg.image?f(x)&space;=&space;1/4\pi&space;r^{2}&space;\&space;where\&space;4\pi&space;r^{2}\&space;is\&space;area\&space;of\&space;sphere" title="f(x) = 1/4\pi r^{2} \ where\ 4\pi r^{2}\ is\ area\ of\ sphere" />   

What this means is we need a distribution that is **1/4π where r = 1** for whole area of sphere, the joint distribution of r, θ and φ can be written as   

<img src="https://latex.codecogs.com/svg.image?f(\theta,&space;\phi,&space;r)&space;=&space;sin\theta/4\pi&space;r^{2}\&space;\&space;where\&space;r&space;=&space;1\&space;and\&space;sin\theta\&space;is\&space;jacobian\&space;of\&space;transformation" title="f(\theta, \phi, r) = sin\theta/4\pi r^{2}\ \ where\ r = 1\ and\ sin\theta\ is\ jacobian\ of\ transformation" />

To actually find the distribution of θ and φ we will use use a technique called **Inverse transform sampling**, I might actually update this tutorial sometime later and explain this technique in more detail but if you don't understand skip this part and jump right to code and copy paste.

Distribution or PDF (probability distribution function) for θ is found by integrating w.r.t to φ ~ uniform[0, π].

<img src="https://latex.codecogs.com/gif.latex?\begin{aligned}&space;f(\theta)&space;=&space;\int_{0}^{\pi}&space;=&space;sin\phi/4\pi&space;\\&space;=&space;-cos\phi/4\pi&space;\&space;|_{0}^{\pi}\textrm{}&space;=&space;1/2\pi&space;\\&space;=>&space;\theta&space;=&space;uniform&space;(0,&space;2\pi)&space;\end{aligned}" title="\begin{aligned} f(\theta) = \int_{0}^{\pi} = sin\phi/4\pi \\ = -cos\phi/4\pi \ |_{0}^{\pi}\textrm{} = 1/2\pi \\ => \theta = uniform (0, 2\pi) \end{aligned}" />

Distribution of φ is found by first creating a PDF (integrat w.r.t to θ ~ uniform(0, 2π)), from it we create CDF (commulative distribution function) and finally inverting it.

<img src="https://latex.codecogs.com/svg.image?\begin{aligned}f(\phi)&space;=&space;\int_{0}^{2\pi&space;}&space;sin\phi&space;/&space;4\pi&space;\&space;d\theta&space;\\f(\phi)&space;=&space;sin&space;\phi/2&space;\\now\&space;for\&space;CDF\\f(\phi)&space;=&space;\int_{0}^{\theta}&space;sin&space;\phi/2&space;\\=&space;1/2(1-cos\phi)&space;\\but\&space;f(\phi)\&space;is\&space;uniform(0,&space;\pi)&space;=&space;u&space;\\u&space;=&space;1/2(1-cos\phi)&space;\\&space;\phi&space;=&space;\cos^{-1}(1-2u)\end{aligned}" title="\begin{aligned}f(\phi) = \int_{0}^{2\pi } sin\phi / 4\pi \ d\theta \\f(\phi) = sin \phi/2 \\now\ for\ CDF\\f(\phi) = \int_{0}^{\theta} sin \phi/2 \\= 1/2(1-cos\phi) \\but\ f(\phi)\ is\ uniform(0, \pi) = u \\u = 1/2(1-cos\phi) \\ \phi = \cos^{-1}(1-2u)\end{aligned}" />

Now we know the distributions of φ and θ let's update the code.

```
    void GenerateUniformPointsInSphere(int count)
    {
        generatedPoints.Clear();

        for (int i = 0; i < count; i++)
        {
            var phi = Mathf.Acos(1f - 2f * Random.Range(0f, Mathf.PI));
            var theta = 2f * Mathf.PI * Random.Range(0f, 1f);
            var r = Mathf.Pow(Random.Range(0f, 1f), 1f / 4f) * radius;

            float x = r * Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = r * Mathf.Sin(phi) * Mathf.Sin(theta);
            float z = r * Mathf.Cos(phi);

            Vector3 pos = new Vector3(x, y, z);
            generatedPoints.Add(pos);
        }
    }
```

If you are curious where the **Power** function comes from see the previous circle tutorial and now for the results.

![Unity_6C5UQYg9Cn](https://user-images.githubusercontent.com/23467551/140643977-92aedbfe-5a22-44f2-a2b0-1954bf0874f3.jpg)

- currently these generated points are generated at origin in world space, if you want them to respect the transforms of the gameobject you attached the RandUniformPointsInSphere.cs script, then convert them to local space of gameobject using transform.TransformPoint utility method. 

```
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.white;

        for (int i = 0; i < generatedPoints.Count; i++)
            Gizmos.DrawSphere(transform.TransformPoint(generatedPoints[i]), debugPointRadius);
    }
```

and it's done !

### _Everything seems good now, tutorial is done, report any mistakes, provide feedback anything is welcome AND if you like it support me on [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay)._ 
