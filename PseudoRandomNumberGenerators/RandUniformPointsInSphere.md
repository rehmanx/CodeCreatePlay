### This tutorial is made for [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay).

Among multiple methods for distributing points in a sphere, the one we will be using involves **[Spherical polar coordinates](https://mathinsight.org/spherical_coordinates#:~:text=Spherical%20coordinates%20determine%20the%20position,angle%20%CE%B8%20from%20polar%20coordinates.)** in which location of a point in 3d space is determined based on **distance ρ or r from origin and two angles θ (theta) and φ (phi)**, before we proceed take a look at **[uniformly distributing points in a circle tutorial](https://github.com/rehmanx/CodeCreatePlay/blob/main/PseudoRandomNumberGenerators/RandUniformPointsInCircle.md).**

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

    const int TWO_PI = 360;

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

![tklTcJFAOG](https://user-images.githubusercontent.com/23467551/137183298-075c4fcd-e914-4e5f-8353-f01434bd8c25.gif)

This gif shows results from out current approach, this definately is not unifrom distribution, the points are clustered at the origin close to axis of rotation... recall from random uniform points inside circle tutorial that as r (distance from origin) increases area increases with it and so is it's density but we are chosing constant distribution of points for entire area of sphere this creates same number of points for the smallest to largest area of sphere which causes this clustering since for larger area larger should be the distribution of points.

The problem with our algorithm is diagnosed.. now for the solution, first let's consider the relationship between area and density of points which is they are inversely proportionally mathematically   

<img src="https://latex.codecogs.com/svg.image?f(x)&space;=&space;1/4\pi&space;r^{2}&space;\&space;where\&space;4\pi&space;r^{2}\&space;is\&space;area\&space;of\&space;sphere" title="f(x) = 1/4\pi r^{2} \ where\ 4\pi r^{2}\ is\ area\ of\ sphere" />   

What this means is we need a distribution that is **1/4π where r = 1** for whole area of sphere, but before we proceed I want to introduce two concepts from statistics which are **Probability density function (PDF)** and **Cumulative distribution function (CDF)**.

In simple terms **PDF** is the probability that a random variable **X** will take a value exactly equal to **x**.   
And **CDF** is the probability that a random variable **X** will take a value less then or equal to **x**.   
So for example if you cast a die
- the probability of obtaining **1, 2, 3, 4, 5, 6 is = 1/6 * 100 = 16.666%**.
- the PDF or the probability that you will get **2** is exactly 16.666%.
- the CDF or the probability that you will get **2** is 33.333% since there are two possible values i.e. 1 and 2, similarly probability that you will get **6** is 100&.

![meta-chart](https://user-images.githubusercontent.com/23467551/138130697-505d9360-b78a-4690-9881-5c28c4044b98.png)

![meta-chart (2)](https://user-images.githubusercontent.com/23467551/138132435-8165666c-b600-4118-8be8-cf122dd911e8.png)

For the sake of the moment let's plot a graph for CDF and invert it.

![Qe4u0](https://user-images.githubusercontent.com/23467551/138133497-99b3226b-b14d-498d-a132-41297a540246.png) 
![UA2sT](https://user-images.githubusercontent.com/23467551/138133657-a7d7ef0b-96d7-4f19-b2fa-8a0041fc70e1.png)

Noticed any similarity ? this is almost the distribution we used when uniformly distributing points in a circle and this is what we need for sphere as well, but first we need distributions of θ and φ for the area of sphere, the joint distribution of θ and φ can be written as   

<img src="https://latex.codecogs.com/svg.image?f(\theta,&space;\phi,&space;r)&space;=&space;sin\theta/4\pi&space;r^{2}\&space;\&space;where\&space;r&space;=&space;1\&space;and\&space;sin\theta\&space;is\&space;jacobian\&space;of\&space;transformation" title="f(\theta, \phi, r) = sin\theta/4\pi r^{2}\ \ where\ r = 1\ and\ sin\theta\ is\ jacobian\ of\ transformation" />

Distribution or PDF for θ by integrating w.r.t to φ ~ uniform[0, π].

<img src="https://latex.codecogs.com/gif.latex?\begin{aligned}&space;f(\theta)&space;=&space;\int_{0}^{\pi}&space;=&space;sin\phi/4\pi&space;\\&space;=&space;-cos\phi/4\pi&space;\&space;|_{0}^{\pi}\textrm{}&space;=&space;1/2\pi&space;\\&space;=>&space;\theta&space;=&space;uniform&space;(0,&space;2\pi)&space;\end{aligned}" title="\begin{aligned} f(\theta) = \int_{0}^{\pi} = sin\phi/4\pi \\ = -cos\phi/4\pi \ |_{0}^{\pi}\textrm{} = 1/2\pi \\ => \theta = uniform (0, 2\pi) \end{aligned}" />

Distribution of φ is found by first creating a PDF (integrat w.r.t to θ ~ uniform(0, 2π)), from it we create CDF and finally inverting it.

<img src="https://latex.codecogs.com/svg.image?\begin{aligned}f(\phi)&space;=&space;\int_{0}^{2\pi&space;}&space;sin\phi&space;/&space;4\pi&space;\&space;d\theta&space;\\f(\phi)&space;=&space;sin&space;\phi/2&space;\\now\&space;for\&space;CDF\\f(\phi)&space;=&space;\int_{0}^{\theta}&space;sin&space;\phi/2&space;\\=&space;1/2(1-cos\phi)&space;\\but\&space;f(\phi)\&space;is\&space;uniform(0,&space;\pi)&space;=&space;u&space;\\u&space;=&space;1/2(1-cos\phi)&space;\\&space;\phi&space;=&space;\cos^{-1}(1-2u)\end{aligned}" title="\begin{aligned}f(\phi) = \int_{0}^{2\pi } sin\phi / 4\pi \ d\theta \\f(\phi) = sin \phi/2 \\now\ for\ CDF\\f(\phi) = \int_{0}^{\theta} sin \phi/2 \\= 1/2(1-cos\phi) \\but\ f(\phi)\ is\ uniform(0, \pi) = u \\u = 1/2(1-cos\phi) \\ \phi = \cos^{-1}(1-2u)\end{aligned}" />

Now we have the distributions of φ and θ let's do this in code.

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

If you are curious about value of **r** and power function, then take a look at circle tutorial I mentioned before and that's just it uniformly distributed points inside a unit sphere.

![bTSKARi1QG](https://user-images.githubusercontent.com/23467551/137982118-5abb40e5-262b-4301-ab91-43a818ca3c0b.gif)

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

The technique we used for uniformly distributing points is called **Inverse transform sampling**, but that's not the main topic here and more on it some other day.

### _Everything seems good now, tutorial is done, report any mistakes, provide feedback anything is welcome AND if you like it support me on [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay)._ 
