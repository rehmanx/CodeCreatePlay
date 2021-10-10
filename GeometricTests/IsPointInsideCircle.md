### This tutorial is made for UnityNerds Patreon page.

![04](https://user-images.githubusercontent.com/23467551/135803323-cc07d18f-3087-4011-9abb-5747e2378b1f.gif)

We will write all tests using static methods, which makes it easier to access them from anywhere in code, start by creating a new C# script **GeometricTests.cs**, attach it to an empty gameObject, create a new folder **Editor** and in it create an editor script **GeometricTestsEd.cs** for **GeometricTests**.

#### Point Inside Circle Test

- Open **GeometricTests.cs** and create a new static function call it "IsPointInsideCircle".

```
public class GeometricTests : MonoBehaviour
{
    public static bool IsPointInsideCircle()
    {
        return false;
    }
}
```

- Open the editor script for **GeometricTests**, before we can test for anything, let's organize things
- **1-** create an enum **TestType**,call it **testType**, since we will be adding more tests later, we will use this to select the test to perform from editor.
**2-** override the public function **OnInspectorGUI** and call it's base method.
**3-** add a function **DrawCustomInspector** and call it from **OnInspectorGUI**.
**4-** use [EditorGUILayout.EnumPopup](https://docs.unity3d.com/ScriptReference/EditorGUILayout.EnumPopup.html) to draw a property for our **testType** field we created in earlier.
**5-** finally add a **switch** statement for all the different type tests.

```
[CustomEditor(typeof(GeometryTests))]
public class GeometricTestsEd : Editor
{
    GeometricTests geometricTests;

    public enum TestType
    {
        PointInsideCircle,
        CirclesIntersectionTest,
        LineSphereIntersectionTest,
    }

    public TestType testType = TestType.PointInsideCircle;

    public void OnEnable()
    {
        geometricTests = target as GeometricTests;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawCustomInspector();
    }

    void DrawCustomInspector()
    {
        testType = (TestType) EditorGUILayout.EnumPopup("TestType", testType);

        switch(testType)
        {
            case TestType.PointInsideCircle:

                circleRadius = EditorGUILayout.FloatField("CircleRadius", circleRadius);

                break;

            case TestType.CirclesIntersectionTest:
                break;

            case TestType.LineSphereIntersectionTest:

                break;
        }
    }
 
```

- Before performing **IsPointInsideCircle** test, let us first visualize a circle.
- **1-** In editor script, add two new methods **OnSceneGUI** and **DrawCircle**, call **DrawCircle** from **OnSceneGUI**.
**2-** We will draw a circle in **DrawCircle** method, if you don't know how to draw a circle then go to this tutorial or just copy paste the code.

```
    private void OnSceneGUI()
    {
        DrawCircle();
    }

    // ---------CODE FOR DRAWING A CIRCLE--------------------------------------
    // for visualizing circle.
    public int segments = 20;
    public float circleRadius = 5f;

    void DrawCircle()
    {
        Vector3 currentPosition = new Vector3();
        Vector3 lastpoint = new Vector3();
        float currentAngle = 0;

        float angleSetep = (360 / segments);

        Handles.color = Color.green;

        for (int i = 0; i < segments + 1; i++)
        {
            // Mathf.Sin expects an angle in radians, however currentAngle is in degress, to convert degrees to radians
            // multiply angle in degree with PI / 180.

            currentPosition = new Vector3(
                Mathf.Sin(Mathf.PI / 180 * currentAngle) * circleRadius + geometryTests.transform.position.x,
                geometryTests.transform.position.y,
                Mathf.Cos(Mathf.PI / 180 * currentAngle) * circleRadius + geometryTests.transform.position.z
                );

            currentAngle += angleSetep;

            if (i > 0)
                Handles.DrawLine(lastpoint, currentPosition);

            lastpoint = currentPosition;
        }
    }
    // -----------------------------------------------
```

- Compile these scripts you will see a circle in scene view.

![3SA6rhLQuU](https://user-images.githubusercontent.com/23467551/135787447-530c2dac-89c2-4f32-97fb-4262460f0152.gif)

- Ok we can now visualize a circle, lets generate some random points, I am using **RandomUniformPointsInCircle** method (tutorial **here**), to generate these points but you can use any method, create a new script **Utility.cs** and copy paste the following static code.

```
public static class Utils
{
    public static List<Vector3> GenerateUniform(float radius, int count)
    {
        List<Vector3> generatedPoints = new List<Vector3>();

        for (int i = 0; i < count; i++)
        {
            var r = radius * Mathf.Sqrt(UnityEngine.Random.Range(0f, 1f));
            float theta = 2 * Mathf.PI * UnityEngine.Random.Range(0f, 1f);
            Vector3 pos = new Vector3(r * Mathf.Cos(theta), 0, r * Mathf.Sin(theta));
            generatedPoints.Add(pos);
        }

        Debug.LogFormat("generated {0} points", generatedPoints.Count);
        return generatedPoints;
    }
}
```

- now in **GeometricTestsEd** add a new field List<Vector3> **generatedPoints**, and update **OnEnable** method to store generated points from **Utils.GenerateUniform** method.
- To visualize the positions in **generatedPoints**, add a new method **DrawGeneratedPoints** and call it from **OnSceneGUI**.

```
    public void OnEnable()
    {
        geometricTests = target as GeometricTests;
        generatedPoints = Utils.GenerateUniform(circleRadius * 3, 100);
    }

    private void OnSceneGUI()
    {
        DrawCircle();
        DrawGeneratedPoints();
    }

    void DrawGeneratedPoints()
    {
        Handles.color = Color.blue;
        foreach (var pos in generatedPoints)
        {
            Handles.DrawWireDisc(pos, Vector3.up, 0.1f);
        }
    }
```

- head back to editor, generated positions are now visible as blue discs.

![02](https://user-images.githubusercontent.com/23467551/135787652-f0eec21a-6b85-48ea-927b-472d924113dc.gif)

- Ok now for finally testing if the generated points are inside the circle or not, if the points are inside circle draw them red otherwise keep drawing them blue.
Head back to **GeometryTests.cs** and update **IsPointInsideCircle** method, for a point to be inside the circle the distance from it to the center of circle is less than radius of circle, symbolically this is represented as

   <img src="https://latex.codecogs.com/gif.latex?\sqrt{\left&space;|&space;x_{p}&space;-&space;x_{c}&space;\right&space;|&space;&plus;&space;\left&space;|&space;y_{p}&space;-&space;y_{c}&space;\right&space;|&space;}&space;<&space;r" title="\sqrt{\left | x_{p} - x_{c} \right | + \left | y_{p} - y_{c} \right | } < r" />.
      
   <img src="https://latex.codecogs.com/gif.latex?where&space;\;&space;\left&space;(&space;x_c,&space;y_c&space;\right&space;)&space;\;&space;is&space;\;&space;center&space;\;&space;of&space;\;&space;circle&space;\;&space;and&space;\;&space;\left&space;(&space;x_p,&space;y_p&space;\right&space;)&space;\;&space;is&space;\;&space;location&space;\;&space;of&space;\;&space;point" title="where \; \left ( x_c, y_c \right ) \; is \; center \; of \; circle \; and \; \left ( x_p, y_p \right ) \; is \; location \; of \; point" />

```
    /// <summary>
    /// Check if a 3d-point is inside circle.
    /// </summary>
    /// <param name="center" center of circle></param>
    /// <param name="radius" radius of circle></param>
    /// <param name="testPoint the 3d point to test"></param>
    public static bool IsPointInsideCircle(Vector3 center, float radius, Vector3 testPoint)
    {
        // distance between circle centre and the point to test
        float distance = Vector3.Distance(center, testPoint);

        // if point is inside the circle
        if (distance < radius)
            return true;

        return false;
    }
```

- To visualize the points inside the circle, update the **DrawGeneratedPoints** method and add the **IsPointInsideCircle** test, if a point is inside the circle they are drawn yellow otherwise blue, since a circle is 2d and in our case lie in XZ plane, the test is only valid if Y-position of both circle and testPoint is 0 as in 2D or same, we will go with later and set the Y-positions of the generated points to that of circle.

```
    void DrawGeneratedPoints()
    {
        foreach (var pos in generatedPoints)
        {
            Vector3 _pos = pos + new Vector3(0, geometricTests.transform.position.y, 0);

            if (GeometricTests.IsPointInsideCircle(geometricTests.transform.position, circleRadius, _pos))
                Handles.color = Color.yellow;
            else
                Handles.color = Color.blue;

            Handles.DrawWireDisc(_pos, Vector3.up, 0.1f);
        }
    }
```

![03](https://user-images.githubusercontent.com/23467551/135787891-7ece7bd1-1792-4322-b359-0170eb068518.gif)


