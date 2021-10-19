### This tutorial is made for [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay).

Among multiple methods for distributing points in a sphere, the one we will be using involves **[Spherical polar coordinates](https://mathinsight.org/spherical_coordinates#:~:text=Spherical%20coordinates%20determine%20the%20position,angle%20%CE%B8%20from%20polar%20coordinates.)** in which location of a point is determined based on   
    
<img src="https://latex.codecogs.com/gif.latex?distance&space;\&space;\rho&space;\&space;from&space;\&space;origin&space;\&space;and&space;\&space;two&space;\&space;angles&space;\&space;\theta&space;\&space;and&space;\&space;\phi" title="distance \ \rho \ from \ origin \ and \ two \ angles \ \theta \ and \ \phi" />
    
![spherical_coordinates](https://user-images.githubusercontent.com/23467551/136846587-7ae62649-6762-41f8-9e08-b4eee38e9971.png)

Transformation from spherical to cartesian coordinates is given by   
     
   <img src="https://latex.codecogs.com/gif.latex?\begin{aligned}&space;x&space;=&space;\rho&space;\sin&space;\phi&space;\cos&space;\theta&space;\\&space;y&space;=&space;\rho&space;\sin&space;\phi&space;\sin&space;\theta&space;\\&space;z&space;=&space;\rho&space;\cos&space;\phi&space;\end{aligned}" title="\begin{aligned} x = \rho \sin \phi \cos \theta \\ y = \rho \sin \phi \sin \theta \\ z = \rho \cos \phi \end{aligned}" />

To selecting <img src="https://latex.codecogs.com/gif.latex?\begin{aligned}&space;\rho&space;\end{aligned}" title="\begin{aligned} \rho \end{aligned}" /> in the range [0, 1] we can get a point that is inside the unit sphere., secondly by selecting θ in the range [0-2π], and φ in the range [0-π], we can get a direction, the code that generates this distribution in C# for unity engine in shown but this applies to any platform you are using.   
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

Note that points are clustered at the origin close to axis of rotation, so what is causing this issue ? this is due to uniform distribution, recall from random uniform points inside circle tutorial that as r increases due to increased area probability of distribution of points decreases or in other words probability of distribution of points is inversely proportional to area i.e.

<img src="https://latex.codecogs.com/gif.latex?\begin{aligned}&space;f(x)&space;=&space;1/4\pi\&space;where\&space;4\pi\&space;is\&space;the\&space;area\&space;of\&space;sphere&space;\end{aligned}" title="\begin{aligned} f(x) = 1/4\pi\ where\ 4\pi\ is\ the\ area\ of\ sphere \end{aligned}" />

So to fix this probability of distribution should be linearly increasing with r, to do this let's borrow two concepts **Probability density function (PDF)** and **Cumulative distribution function (CDF)** from statistics.

In simple terms **PDF** is the probability that a random variable **X** will take a value exactly equal to **x**.   
And **CDF** is the probability that a random variable **X** will take a value less then or equal to **x**.   
So for example if you cast a die
- the probability of obtaining **_1, 2, 3, 4, 5, 6 is =16.666%_**.
- the PDF or the probability that you will get **_2_** is exactly 16.666%.
- the CDF or the probability that you will get **_2_** is 33.333%, since there are two possible values i.e. 1 and 2.

For the sake of the moment let's invert the graph of CDF.   
Noticed any similarity ? this is almost the distribution we used when uniformly distributing points in a circle ! so that's it we need the inverse of CDF by keeping θ uniform [0, 2π] and uniformly distributing φ [0, 1] we can be get a distribution that is thin at origin but linearly increases with r.

The above technique has a name **Inverse transform sampling**.

But first we have to find PDF for θ and CDF for φ, so now again for the boring part.   
The joint distribution of θ and φ can be written as

<img src="https://latex.codecogs.com/gif.latex?\begin{aligned}&space;f(\theta,&space;\phi&space;)&space;=&space;sin\theta/4\pi\&space;where\&space;r&space;=&space;1\&space;and\&space;sin\theta\&space;is\&space;the\&space;J\&space;of\&space;transformation.&space;\end{aligned}" title="\begin{aligned} f(\theta, \phi ) = sin\theta/4\pi\ where\ r = 1\ and\ sin\theta\ is\ the\ J\ of\ transformation. \end{aligned}" />

Distribution for θ by integrating w.r.t to φ ~ uniform[0, π].

<img src="https://latex.codecogs.com/gif.latex?\begin{aligned}&space;f(\theta)&space;=&space;\int_{0}^{\pi}&space;=&space;sin\phi/4\pi&space;\\&space;=&space;-cos\phi/4\pi&space;\&space;|_{0}^{\pi}\textrm{}&space;=&space;1/2\pi&space;\\&space;=>&space;\theta&space;=&space;uniform&space;(0,&space;2\pi)&space;\end{aligned}" title="\begin{aligned} f(\theta) = \int_{0}^{\pi} = sin\phi/4\pi \\ = -cos\phi/4\pi \ |_{0}^{\pi}\textrm{} = 1/2\pi \\ => \theta = uniform (0, 2\pi) \end{aligned}" />

Distribution of φ by first creating a PDF and from it a CDF.

<img src="https://latex.codecogs.com/gif.latex?\begin{aligned}&space;f(\phi)&space;=&space;\int_{0}^{2\pi&space;}&space;sin\phi&space;/&space;4\pi&space;\&space;d\theta&space;\\&space;f(\phi)&space;=&space;sin&space;\phi/2&space;\\&space;to\&space;find\&space;CDF\&space;of\&space;f(\phi)&space;\\&space;f(\phi)&space;=&space;\int_{0}^{\theta}&space;sin&space;\phi/2&space;\\&space;=&space;1/2(1-cos\phi)&space;\\&space;but\&space;f(\phi)\&space;is\&space;uniform(0,&space;1)&space;=&space;u&space;\\&space;u&space;=&space;1/2(1-cos\phi)&space;\\&space;\phi&space;=&space;\cos^{-1}(1-2u)&space;\end{aligned}" title="\begin{aligned} f(\phi) = \int_{0}^{2\pi } sin\phi / 4\pi \ d\theta \\ f(\phi) = sin \phi/2 \\ to\ find\ CDF\ of\ f(\phi) \\ f(\phi) = \int_{0}^{\theta} sin \phi/2 \\ = 1/2(1-cos\phi) \\ but\ f(\phi)\ is\ uniform(0, 1) = u \\ u = 1/2(1-cos\phi) \\ \phi = \cos^{-1}(1-2u) \end{aligned}" />

We have the distributions of φ and θ let's do this in code.

```
    void GenerateUniformPointsInSphere(int count)
    {
        generatedPoints.Clear();

        for (int i = 0; i < count; i++)
        {
            var phi = Mathf.Acos(1f - 2f * Random.Range(0f, 1f));
            var theta = 2f * Mathf.PI * Random.Range(0f, 1f);
            var r = Mathf.Pow(Random.Range(0f, 1f), 1f / 3f) * radius;

            float x = r * Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = r * Mathf.Sin(phi) * Mathf.Sin(theta);
            float z = r * Mathf.Cos(phi);

            Vector3 pos = new Vector3(x, y, z);
            generatedPoints.Add(pos);
        }
    }
```

- and that's just it uniformly distributed points inside a unit sphere.

![uniform distribution](https://user-images.githubusercontent.com/23467551/137184231-bee0badd-241d-4f16-8000-5d4f4c383544.gif)

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

### _Everything seems good now, tutorial is done, report any mistakes, provide feedback anything is welcome AND if you like it support me on [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay)._ 
