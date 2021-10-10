### This tutorial is made for UnityNerds Patreon page.


To generate points inside a circle , let us first visualize a circle, we are going to do this is UnityEditor mode.
 - create a new C# script **CircleDistribution.cs**, and attach it to a **GameObject**.
 - open **CircleDistribution.cs**, add 2 new floats **radius** and **segments**

```
public class CircleDistribution : MonoBehaviour
{
    // public 
    public float circleRadius = 1f;
    public float segments = 30f;
}
```

- create a new folder **Editor** and in it create a new editor script **CircleDistributionEd.cs** for **CircleDistribution** c;lass.
- open the **CircleDistributionEd.cs** script &

- **1-** add the [CustomEditor](https://docs.unity3d.com/Manual/editor-CustomEditors.html) attribute for **CircleDistributionEd* class.
**2-** create a new method [OnSceneGUI](https://docs.unity3d.com/ScriptReference/Editor.OnSceneGUI.html)
**3-** create a new method **DrawCircle** and call it from **OnSceneGUI** method.

```
using System;
using UnityEditor;


[CustomEditor(typeof(CircleDistribution))]
public class CircleDistributionEd : UnityEditor.Editor
{
    // private
    CircleDistribution circleDistribution = null;

    private void OnEnable()
    {
        circleDistribution = target as CircleDistribution;
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

- The **X** and **Y**  positions from origin. on the circumference of a unit circle meaning circle with radius 1, are given by **X-position** = cos (angle) and **Y-position** = sin(angle), to scale a unity circle multiply these positions with our custom radius, **X-position** = cos (angle) * radius and **Y-position** = sin (angle) * radius.

![circle_illustration](https://user-images.githubusercontent.com/23467551/135723961-eaa98723-320b-4c7b-b1d2-5d92196c002a.png)

- To draw the circle we will loop through **n** number of times where **n = number of segments +1** and increment **angle** by an **angle** step (meaning offset between two successive angles based on number of segments).    
We will use [Handles](https://docs.unity3d.com/ScriptReference/Handles.html) to draw a line from last position to current position on circumference of the circle.

```
    private void DrawCircle()
    {
        Vector3 currentPosition = new Vector3();
        Vector3 lastpoint = new Vector3();
        float currentangle = 0;

        float angleSetep = (360 / cd.segments);

        Handles.color = Color.green;

        for (int i = 0; i < cd.segments + 1; i++)
        {
            currentPosition = new Vector3(
                Mathf.Sin(Mathf.Deg2Rad * currentangle) * cd.circleRadius,
                0,
                Mathf.Cos(Mathf.Deg2Rad * currentangle) * cd.circleRadius
                );

            currentangle += angleSetep;

            if (i > 0)
                Handles.DrawLine(lastpoint, currentPosition);

            lastpoint = currentPosition;
        }
    }
```

![01](https://user-images.githubusercontent.com/23467551/135799307-879e1583-24cd-4527-beb4-6ea0e5a185df.gif)

- Now we have a circle but it does not respect the transforms of the **CircleDistribution** gameObject, this is because points generated for circumference of circle are in world space, let's fix this by converting them from world to local space of **CircleDistribution** gameObject using [tranform.TransformPoint](https://docs.unity3d.com/ScriptReference/Transform.TransformPoint.html) utility method of unity's Transform component.

```
    private void DrawCircle()
    {
        Vector3 currentPosition = new Vector3();
        Vector3 lastpoint = new Vector3();
        float currentangle = 0;

        float angleSetep = (360 / cd.segments);

        Handles.color = Color.green;

        for (int i = 0; i < cd.segments + 1; i++)
        {
            currentPosition = new Vector3(
                Mathf.Sin(Mathf.Deg2Rad * currentangle) * cd.circleRadius,
                0,
                Mathf.Cos(Mathf.Deg2Rad * currentangle) * cd.circleRadius
                );

            currentPosition = cd.transform.TransformPoint(currentPosition);

            currentangle += angleSetep;

            if (i > 0)
                Handles.DrawLine(lastpoint, currentPosition);

            lastpoint = currentPosition;
        }
    }
```

![02](https://user-images.githubusercontent.com/23467551/135799583-beb123ab-6da9-455b-b419-bed195f65e3a.gif)

- Everything now works as expected.
- Now, first let's create an algorithm to generate "random distribution" of points inside the circle.
-  Open **CircleDistribution.cs** script
- **1-** add a new float **generationCount** 
- **2-** add a new List<Vector3> **generatedPoints**, to save generated points
**2-**  create a new method **RandomDistribution**, we will call this method from the editor script **CircleDistributionEd.cs** we created for  **CircleDistribution.cs**.

```
public class CircleDistribution : MonoBehaviour
{
    // public 
    public int generationCount= 100; // number of points to be generated

    [HideInInspector]
    public List<Vector3> generatedPoints = new List<Vector3>(); // generated points are saved here

    public List<Vector3> GenerateRandom()
    {
        generatedPoints.Clear();

        // generate distribution
 
        // return
        return generatedPoints;
    }
}
```

- Open **CircleDistributionEd.cs** script, we will create a button to call **GenerateRandom** method in **CircleDistribution.cs**.
- **1-** override **OnInspectorGUI** method, call base **OnInspectorGUI** to keep drawing default inspector
**2-** create a button and use if statement to check if it is pressed, if pressed call **GenerateRandom** method in **CircleDistribution.cs**.

```
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // 
        if(GUILayout.Button("GenerateRandom"))
        {
            cd.GenerateRandom();
        }
    }
```

- We can now press the button to call a method to generated points.
- The strategy for generating random points inside circle 
- **1-** first generate random points inside a unit circle 
**2-** to generate random points inside a circle of radium r, we will use two instances of random number generators **first** to choose a random angle between 0 and 2 PI (remember 2 PI is one complete rotation) to set an angular position and  **second** to choose a random position between origin and radius of the circle to get a random distance on the circle.
**3-** These random positions are generated in world space, optionally we can transform them from world space to local space of **CircleDistribution** game object, update the code in **GenerateRandom** method.


```
    public List<Vector3> GenerateRandom()
    {
        generatedPoints.Clear();

        for (int i = 0; i < generationCount; i++)
        {
            float r = circleRadius * UnityEngine.Random.Range(0f, 1f);
            float theta = 2 * Mathf.PI * UnityEngine.Random.Range(0f, 1f);
            Vector3 pos = new Vector3(r * Mathf.Cos(theta), 0, r * Mathf.Sin(theta));
            generatedPoints.Add(pos);

            // optionally
            // generatedPoints.Add(transform.TransformPoint(pos));
        }

        // Debug.LogFormat("generated {0} points", generatedPoints.Count);
        return generatedPoints;
    }
```

- We have now generated a set points, to visualize them, we will use [Handles](https://docs.unity3d.com/ScriptReference/Handles.html) to draw spheres and discs or anything you like at each position of generated data.
- Create a new float **debugPointRadius** in **CircleDistribution.cs**

```
public class CircleDistribution : MonoBehaviour
{
    // public 
    public float debugPointRadius = 0.1f;
}
```
- Create a new method **VizPoints** in **CircleDistributionEd.cs** and call it from **OnSceneGUI**

```
using System;
using UnityEditor;

[CustomEditor(typeof(CircleDistribution))]
public class CircleDistributionEd : UnityEditor.Editor
{
    private void OnSceneGUI()
    {
        DrawCircle();
        VizPoints();
    }

    private void VizPoints()
    {
        Handles.color = Color.red;
        foreach (var pos in cd.generatedPoints)
        {
            Handles.DrawSolidDisc(pos, cd.transform.up, cd.debugPointRadius);
        }
    }
}
```

- Press generate data button again, generated points will show in editor, however they are in world space, convert them to local space by [tranform.TransformPoint](https://docs.unity3d.com/ScriptReference/Transform.TransformPoint.html) utility method.

![02](https://user-images.githubusercontent.com/23467551/135800304-bb5dd282-4065-4a6f-a06a-8cfaecc7214b.gif)

```
    private void VizPoints()
    {
        Handles.color = Color.red;
        foreach (var pos in cd.generatedPoints)
        {
            Handles.DrawSolidDisc(cd.transform.TransformPoint(pos), cd.transform.up, cd.debugPointRadius);
        }
    }
```

![03](https://user-images.githubusercontent.com/23467551/135800314-4e20d08c-ecfa-47e6-89bc-9ace53ec27fc.gif)

- Generated data points now show up in editor in local space of our **CircleDistribution** gameobject, control scale of points using **debugPointRadius** in **CircleDistribution.cs**.
- Data generated using random method, is concentrated towards the center of circle, this is because as distance from center increases, circumference or the area of the circle increases as well, more points are needed to fill large area, however our algorithm does not respect that, to generate uniform points let's create another algorithm that respects increasing area of circle.
- Create a new method in **GeneratedUniform* in **CircleDistribution.cs**

```
    public List<Vector3> GenerateUniform()
    {
        generatedPoints.Clear();

        for (int i = 0; i < generationCount; i++)
        {
        }

        Debug.LogFormat("generated {0} points", generatedPoints.Count);
        return generatedPoints;
    }
```

- To distribute points uniformly, we have to consider the increasing area of the circle from origin, and give more weight or probability as distance increases, this in to compensate for the increasing area and keep the distribution uniform across the circle.   

 ```
    public List<Vector3> GenerateUniform()
    {
        generatedPoints.Clear();

        for (int i = 0; i < generationCount; i++)
        {
            var r = circleRadius * Mathf.Sqrt(UnityEngine.Random.Range(0f, 1f));
            float theta = 2 * Mathf.PI * UnityEngine.Random.Range(0f, 1f);
            Vector3 pos = new Vector3(r * Mathf.Cos(theta), 0, r * Mathf.Sin(theta));
            generatedPoints.Add(pos);
        }

        Debug.LogFormat("generated {0} points", generatedPoints.Count);
        return generatedPoints;
    }
```

- Add another button to call **GenerateUniform** method in **CircleDistribution.cs** and generate uniform data.

```
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // generate random data.
        if(GUILayout.Button("GenerateRandom"))
        {
            cd.GenerateRandom();
            HandleUtility.Repaint();
        }

        // generate uniform data.
        if (GUILayout.Button("GenerateUniform"))
        {
            cd.GenerateUniform();
            HandleUtility.Repaint();
        }
    }
``` 

![04](https://user-images.githubusercontent.com/23467551/135803016-fcc121dd-ebbb-4e80-8d8f-69db5202d06b.gif)



