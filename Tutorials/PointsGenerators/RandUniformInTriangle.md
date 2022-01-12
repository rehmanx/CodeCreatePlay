### _This tutorial is made for [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay)_.

![Jl9UXieATR](https://user-images.githubusercontent.com/23467551/140970439-db4842a6-9c93-4dc3-b6df-5834584969e2.gif)

Random uniform distribution in a triangle is a prerequisite to random uniform distribution in a polygon, the easiest way to understand this algorithm is to generate a uniform distribution in a parallelogram (squares, rectangles etc.), for a parallelogram with origin at **0** we can form a vector (**_highlightened in yellow_**) **p = u1*a + u2*b** where **u1, u2 ~ U(0, 1)** to get a uniform distribution in the parallelogram. 
 
 ![Unity_pgr7JI8xHJ](https://user-images.githubusercontent.com/23467551/141053684-abbc4ecc-043a-42d5-bfb4-06785bb89c13.png)
 
 if **u** is uniformly distributed between **0** and **1** then **v** = **1 - u** is also distributed randomly between 0 and 1, we can use this transformation to convert a point on the parallelogram to a point that is on the triangle.
 
First let's visualize the polygon, create a new unity c# script **RandUniformInTriangle** and the fields for visualizing the polygon, sample size, scale of polygon, debug size of generated points and a list to save generated points, see the comments for details, also I am using **unity's transform.TransformPoint** to convert from world to local space of the transform this script is attached to.

```
    // public fields-------------------------
    public int sampleSize = 1000; // number of points to generate
    public float scale = 5f; // scale to apply to polygon
    public float debugPointSize = 0.005f; // 

    // private fields-------------------------
    Vector3[] polygon = new Vector3[4]; // points for square or a parallelogram shaped polygon
    List<Vector3> points = new List<Vector3>(); // generated points

    private void Start()
    {
        // create points points for a square
        polygon[0] = new Vector3(0, 0, 0);
        polygon[1] = new Vector3(0, 0, 1);
        polygon[2] = new Vector3(1, 0, 1);
        polygon[3] = new Vector3(1, 0, 0);
    }
```

I am using unity's **OnDrawGizmos** method draw the polygon and the generated points.

```
    // save these points here to avoid garbage collector from triggering 
    // every frame.
    Vector3 _p1 = Vector3.zero;
    Vector3 _p2 = Vector3.zero;

    private void OnDrawGizmos()
    {
        // polygon must have 4 points to be a square or parallelogram
        if (polygon.Length < 4)
            return;

        // draw a sphere on origin.
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.TransformPoint(polygon[0]), 0.1f);

        // loop through all the polygon points and connect them.
        for (int i = 0; i < polygon.Length; i++)
        {
            _p1 = polygon[i] * scale; // extend vector by scale

            if (i + 1 >= polygon.Length)
                _p2 = polygon[0] * scale;
            else
                _p2 = polygon[i + 1] * scale;

            // other lines of triangle in blue.
            if (i == 0 || i == 3)
                Gizmos.color = Color.blue;
            else
                Gizmos.color = Color.red;

            Gizmos.DrawLine(transform.TransformPoint(_p1), transform.TransformPoint(_p2));

            // the diagonal line of triangle
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.TransformPoint(polygon[3] * scale), transform.TransformPoint(polygon[1] * scale));
        }

        // and finally visualize the generated data.
        Gizmos.color = Color.yellow;
        for (int i = 0; i < points.Count; i++)
            Gizmos.DrawSphere(transform.TransformPoint(points[i]), debugPointSize);
    }
```

a simple visualization;

![Unity_FPfg1zgwX2](https://user-images.githubusercontent.com/23467551/141091728-dc72c872-2e63-4934-8252-5cb9cf7fe7ec.png)

Let's perform the first step and generate uniform points in the parallelogram, create a new method **UniformInTriangle** and call it from **Start** method and hit play.

```
    private void Start()
    {
        // **previous code**
        for (int i = 0; i < sampleSize; i++)
            UnifromInParallelogram();
    }
    
    void UniformInTriangle()
    {
        var a = polygon[1] - polygon[0];
        var b = polygon[3] - polygon[0];

        var u1 = Random.Range(0f, 1f);
        var u2 = Random.Range(0f, 1f);

        var w = u1 * (a * 5) + u2 * (b * 5);
        points.Add(w);
    }
```

My results with sample size = 5000 and scale = 5f;

![Unity_wyKMsSYdFV](https://user-images.githubusercontent.com/23467551/141090546-4841ef8c-9185-48ad-bcd8-f5890e57d4ae.png)

Now let's perform the reflection step,

```
    void UniformInTriangle()
    {
        var a = polygon[1] - polygon[0];
        var b = polygon[3] - polygon[0];

        var u1 = Random.Range(0f, 1f);
        var u2 = Random.Range(0f, 1f);

        // ** the reflection step **
        if ((u1 + u2) > 1)
        {
            u1 = 1 - u1;
            u2 = 1 - u2;
        }

        var w = u1 * (a * scale) + u2 * (b * scale);
        points.Add(w);
    }
```

my results with same params

![Unity_SoWxc69tDz](https://user-images.githubusercontent.com/23467551/141090869-d87f9852-1c89-4717-8f33-64259a5f91cf.png)

and that's it uniform points in a triangle.

### _Everything seems good now, tutorial is done, report any mistakes, provide feedback anything is welcome AND if you like it support me on [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay)_.
