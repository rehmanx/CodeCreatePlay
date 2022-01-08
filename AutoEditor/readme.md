## Unity AutoEditor 
Automatically creates Unity editor gui elements and significantly reduces the time to write a tool and allows developers to focus more on writing their programs.
It works by extending unity with some custom attributes which can be placed on C# fields, almost all C# fields and some unity's gui elements are supported, here is a complete list  

1. floatField
2. intField
3. boolField
4. vector2
5. vector3
6. boldLabel
7. text
8. textControl
9. space
10. layerField
11. tag
12. intSlider
13. floatSlider
14. colour
15. gameObject
16. texture2d
17. unityTerrainField
18. transformsList
19. gameObjectsList

Settings up a field to auto build is easy, just include the appropriate **Attribute** to top of it, all **AutoEditor's** custom attributes take some input arguments which vary depending on which type of unity editor gui element you want to create and your requirements, however name of field and input argument "_name" must be same.


```
[EditorFieldAttr(ControlType.floatField, _name: "floatField")]
public float floatField = 1.0f;
```

As of **version 1.0** Currently there are only 3 attributes,

1. EditorFieldAttribute
2. IntSliderAttribute
3. FloatSliderAttribute 

*There is a demo included in **DemoFolder** which shows entire functionality of AutoEditor.*

### Step by step instruction

1. Download this repository and import AutoEditor.cs script in your project.
2. In your project panel create a new folder **Demo** and in it create another folder **Editor**.
3. In your Demo folder create a new C# script **Demo.cs** and in your Editor folder create a new C# script **DemoEditor.cs**.
4. Open Demo.cs script and create a new class **Settings**, add some generic fields, also add appropriate attributes.

```
```

5. Now create an **AutoEditor** for settings class, autoEditor takes type of class and a reference to the object itself, and that's it.

6. To actually show them in editor open the **DemoEditor.cs** script and add the boilerplate editor code.

7. Now in **OnInspectorGUI** method just call **Build** method of **settingsAutoEd**.