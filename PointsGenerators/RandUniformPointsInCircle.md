### This tutorial is made for [CodeCreatePlay](https://www.patreon.com/CodeCreatePlay) Patreon page.

![jFRbrSPEfr](https://user-images.githubusercontent.com/23467551/137761980-7d7c55b0-1df4-49ca-885b-c17b3deed887.gif)

Suppose you want to spawn enemies around your player in a fixed circular radius or you want to create a brush tool to paint gameobjects in your level without enemies or gameobjects clustering in any specific area then in this tutorial you can learn how to do it.

First visualize a circle, we are going to do this is UnityEditor mode.
 - create a new C# script **RandUniformPointsInCircle.cs**, and attach it to a **GameObject**.
 - a circle is defined by two variables **origin** and **radius**, for origin we can use the pivot position of our gameobject and radius can be customizable, open **RandUniformPointsInCircle.cs**, add 2 new floats **circleRadius** and **segments**.

```
public class RandUniformPointsInCircle : MonoBehaviour
{
    // public 
    public float circleRadius = 1f;
    
    // this is just for visuals, a complete circle is 360 degrees however 
    // we can change that when visualizing, the less the segments the more circle will appear jagged.
    public float segments = 30f;
}
```

- create a new folder **Editor** and in it create a new editor script **RandUniformPointsInCircleEd.cs** for **RandUniformPointsInCircle**.
- open the **RandUniformPointsInCircleEd.cs** script & create a new method **DrawCircle** and call it from **OnSceneGUI** method.

```
using System;
using UnityEditor;


[CustomEditor(typeof(RandUniformPointsInCircle))]
public class RandUniformPointsInCircleEd : UnityEditor.Editor
{
    // private
    RandUniformPointsInCircle rupc = null;

    private void OnEnable()
    {
        rupc = target as RandUniformPointsInCircle ;
    }

    private void OnSceneGUI()
    {
        DrawCircle();
    }

    private void DrawCircle()
    {
    }
}
```

- The **X** and **Y**  positions from origin on the circumference of a unit circle meaning circle with radius 1, are given by **X-position** = cos (angle) and **Y-position** = sin(angle), to scale a unit circle multiply these positions with our custom radius, **X-position** = cos (angle) * radius and **Y-position** = sin (angle) * radius.

![circle_illustration](https://user-images.githubusercontent.com/23467551/135723961-eaa98723-320b-4c7b-b1d2-5d92196c002a.png)

- To draw the circle we will loop through **n** number of times where **n = number of segments +1** and increment **angle** by an **angle step** (meaning offset between two successive angles).    
We will use [Handles](https://docs.unity3d.com/ScriptReference/Handles.html) to draw a line from last position to current position on circumference of the circle.

```
    private void DrawCircle()
    {
        Vector3 currentPosition = new Vector3();
        Vector3 lastpoint = new Vector3();
        float currentAngle = 0;

        float angleStep = (360 / rupc.segments);

        Handles.color = Color.green;

        for (int i = 0; i < rupc.segments + 1; i++)
        {
            // Mathf.Sin expects an angle in radians, however currentAngle is in degress, to convert degrees to radians
            // multiply angle in degree with PI / 180.

            currentPosition = new Vector3(
                Mathf.Sin(Mathf.PI / 180 * currentAngle) * rupc.circleRadius,
                0,
                Mathf.Cos(Mathf.PI / 180 * currentAngle) * rupc.circleRadius
                );

            currentAngle += angleStep;

            if (i > 0)
                Handles.DrawLine(lastpoint, currentPosition);

            lastpoint = currentPosition;
        }
    }
```

![01](https://user-images.githubusercontent.com/23467551/135799307-879e1583-24cd-4527-beb4-6ea0e5a185df.gif)

- Now we have a circle at the origin and with the specified radius, but right now if you move, rotate or scale **RandUniformPointsInCircle** gameObject the circle does not repsects it's transformation, this is because circle is in world space, let's fix this by converting them from world to local space of **RandUniformPointsInCircle** gameObject using [tranform.TransformPoint](https://docs.unity3d.com/ScriptReference/Transform.TransformPoint.html) utility method of unity's transform component.

```
    private void DrawCircle()
    {
        Vector3 currentPosition = new Vector3();
        Vector3 lastpoint = new Vector3();
        float currentAngle = 0;

        float angleStep = (360 / rupc.segments);

        Handles.color = Color.green;

        for (int i = 0; i < rupc.segments + 1; i++)
        {
            // Mathf.Sin expects an angle in radians, however currentAngle is in degress, to convert degrees to radians
            // multiply angle in degree with PI / 180.

            currentPosition = new Vector3(
                Mathf.Sin(Mathf.PI / 180 * currentAngle) * rupc.circleRadius,
                0,
                Mathf.Cos(Mathf.PI / 180 * currentAngle) * rupc.circleRadius
                );

            // -------------CONVERT FROM WORLD SPACE TO LOCAL SPACE
            currentPosition = rupc.transform.TransformPoint(currentPosition);

            currentAngle += angleStep;

            if (i > 0)
                Handles.DrawLine(lastpoint, currentPosition);

            lastpoint = currentPosition;
        }
    }
```

![02](https://user-images.githubusercontent.com/23467551/135799583-beb123ab-6da9-455b-b419-bed195f65e3a.gif)

- We have a circle now, let's get back to original problem... we have a circle and we want to generate some points in it that are more or less evenly spaced, a naive approach would be a choose two instances of random number generators one in range [0, 1] to select a random distance in a unit circle and another in range [0, 2π] (2π = one complete rotation of circle is radians) to set angular position, to do it in code create a new method **GenerateRandom** in **RandUniformPointsInCircle**. 

```
    /// <summary>
    /// generates random points inside a circle of radius r.
    /// </summary>
    /// <param name="count"> number of points to generate </param>
    /// <param name="radius"> radius of the circle </param>
    /// <returns></returns>
    public List<Vector3> GenerateRandom(int count, float radius)
    {
        generatedPoints.Clear();

        for (int i = 0; i < count; i++)
        {
            float theta = 2 * Mathf.PI * UnityEngine.Random.Range(0f, 1f); // angular position.
            float r = radius * UnityEngine.Random.Range(0f, 1f); // distance on circle.
            Vector3 pos = new Vector3(r * Mathf.Cos(theta), 0, r * Mathf.Sin(theta)); // convert to cartesian coordinates.
            generatedPoints.Add(pos);
        }

        Debug.LogFormat("generated {0} points", generatedPoints.Count);
        return generatedPoints;
    }
```

- To call this method I have created a button in **OnInspectorGUI** of **RandUniformPointsInCircleEd** and to visualize the generated points create a new method **VizPoints** and call it from **OnSceneGUI**.

```
[CustomEditor(typeof(RandUniformPointsInCircle))]
public class RandUniformPointsInCircleEd : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // button to call GenerateRandom method in RandUniformPointsInCircle.
        if(GUILayout.Button("GenerateRandom"))
        {
            rupc.GenerateRandom(1000, rupc.circleRadius);
            SceneView.RepaintAll();
        }
    }

    private void OnSceneGUI()
    {
        DrawCircle();
        VizPoints();
    }

    private void VizPoints()
    {
        Handles.color = Color.red;
        foreach (var pos in rupc.generatedPoints)
            Handles.DrawSolidDisc(pos, rupc.transform.up, rupc.debugPointRadius);
    }
}
```

- The result looks something like this, with points clustered at the center.

![Unity_nJS3Lq6AW2](https://user-images.githubusercontent.com/23467551/137636170-db2cabfd-05fe-4946-b6cc-1ba3e191ea56.png)

And depending on the requirements this may or may not be what you want, the reason points are generated more dense near the origin is uniform or constant probability of distribution of points across the entire area of circle, as distance from origin increases the area of circle increases with it, twice as many points are needed to fill the larger areas, in other words the density of points is inversely propertional to area mathematically 

<img src="https://latex.codecogs.com/gif.latex?f(x)&space;=&space;1/2\pi\&space;where\&space;2\pi\&space;is\&space;the\&space;area\&space;of\&space;circle\" title="f(x) = 1/2\pi\ where\ 2\pi\ is\ the\ area\ of\ circle\" />

A graph of uniform probability of distribution, as we move from 0 to 1 on x, for every value on x same probability is taken for corresponding value of y for example when x = 0.25, y = 0.25 as well.

![desmos-graph](https://user-images.githubusercontent.com/23467551/137637391-f00cd386-3349-4534-bd34-54ee5d4b0042.png)

To compensate for loss of density as distance from origin increases more probability should be given to larger areas, to do this we can utilize some builtin methods such as **power** or **square root**, so if we choose a function **y=pow(x^1/2)** then when x = 0.25, y = 0.5 meaning probabilty on y is twice of x, plotting a graph again would look like this.

![desmos-graph (1)](https://user-images.githubusercontent.com/23467551/137638437-eddb997e-922b-42c1-921b-adba7e29d3e6.png)

- To do this in code create a new method **GenerateUniform** in **RandUniformPointsInCircle**.

```
    /// <summary>
    /// uniformly generates random points inside a circle of radius r.
    /// </summary>
    /// <param name="count"> number of points to generate </param>
    /// <param name="radius"> radius of the circle </param>
    /// <returns></returns>
    public List<Vector3> GenerateUniform(int count, float radius)
    {
        generatedPoints.Clear();

        for (int i = 0; i < count; i++)
        {
            float theta = 2 * Mathf.PI * UnityEngine.Random.Range(0f, 1f);
            var r = radius * Mathf.Pow(Random.Range(0f, 1f), 1 / 2f);
            Vector3 pos = new Vector3(r * Mathf.Cos(theta), 0, r * Mathf.Sin(theta));
            generatedPoints.Add(pos);
        }

        Debug.LogFormat("generated {0} points", generatedPoints.Count);
        return generatedPoints;
    }
```

and create a new button in **RandUniformPointsInCircleEd** to call this method.

```
[CustomEditor(typeof(RandUniformPointsInCircle))]
public class RandUniformPointsInCircleEd : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // // button to call GenerateUniform method in RandUniformPointsInCircle.
        if (GUILayout.Button("GenerateUniform"))
        {
            rupc.GenerateUniform(1000, rupc.circleRadius);
            SceneView.RepaintAll();
        }
    }
}
```

- The result look something like this, distribution is much more uniform now.

![Unity_HxH38KT0B8](https://user-images.githubusercontent.com/23467551/137638954-c4888a5f-56b4-48a7-b386-1b9617d7330e.png)

Main part of tutorial is done, just one last thing as with circle these current points are being generated in world space use **transform.TransformPoint** to convert them to local space.

```
    private void VizPoints()
    {
        Handles.color = Color.red;
        foreach (var pos in rupc.generatedPoints)
            Handles.DrawSolidDisc(rupc.transform.TransformPoint(pos), rupc.transform.up, rupc.debugPointRadius);
    }
```

### _Everything seems good now, tutorial is done, report any mistakes, provide feedback anything is welcome AND if you like it support me on patreon._ 
