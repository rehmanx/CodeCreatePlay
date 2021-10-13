### This tutorial is made for UnityNerds Patreon page.


To generate points inside a circle , let us first visualize a circle, we are going to do this is UnityEditor mode.
 - create a new C# script **RandUniformPointsInCircle.cs**, and attach it to a **GameObject**.
 - open **RandUniformPointsInCircle.cs**, add 2 new floats **radius** and **segments**

```
public class RandUniformPointsInCircle : MonoBehaviour
{
    // public 
    public float circleRadius = 1f;
    public float segments = 30f;
}
```

- create a new folder **Editor** and in it create a new editor script **RandUniformPointsInCircleEd.cs** for **RandUniformPointsInCircle** c;lass.
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

- The **X** and **Y**  positions from origin. on the circumference of a unit circle meaning circle with radius 1, are given by **X-position** = cos (angle) and **Y-position** = sin(angle), to scale a unity circle multiply these positions with our custom radius, **X-position** = cos (angle) * radius and **Y-position** = sin (angle) * radius.

![circle_illustration](https://user-images.githubusercontent.com/23467551/135723961-eaa98723-320b-4c7b-b1d2-5d92196c002a.png)

- To draw the circle we will loop through **n** number of times where **n = number of segments +1** and increment **angle** by an **angle** step (meaning offset between two successive angles based on number of segments).    
We will use [Handles](https://docs.unity3d.com/ScriptReference/Handles.html) to draw a line from last position to current position on circumference of the circle.

```
    private void DrawCircle()
    {
        Vector3 currentPosition = new Vector3();
        Vector3 lastpoint = new Vector3();
        float currentAngle = 0;

        float angleSetep = (360 / rupc.segments);

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

            currentAngle += angleSetep;

            if (i > 0)
                Handles.DrawLine(lastpoint, currentPosition);

            lastpoint = currentPosition;
        }
    }
```

![01](https://user-images.githubusercontent.com/23467551/135799307-879e1583-24cd-4527-beb4-6ea0e5a185df.gif)

- Now we have a circle but it does not respect the transforms of the **RandUniformPointsInCircle** gameObject, this is because points generated are in world space, let's fix this by converting them from world to local space of **RandUniformPointsInCircle** gameObject using [tranform.TransformPoint](https://docs.unity3d.com/ScriptReference/Transform.TransformPoint.html) utility method of unity's Transform component.

```
    private void DrawCircle()
    {
        Vector3 currentPosition = new Vector3();
        Vector3 lastpoint = new Vector3();
        float currentAngle = 0;

        float angleSetep = (360 / rupc.segments);

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

            currentAngle += angleSetep;

            if (i > 0)
                Handles.DrawLine(lastpoint, currentPosition);

            lastpoint = currentPosition;
        }
    }
```

![02](https://user-images.githubusercontent.com/23467551/135799583-beb123ab-6da9-455b-b419-bed195f65e3a.gif)

- Everything now works as expected.
