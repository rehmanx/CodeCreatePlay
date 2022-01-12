using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace CODE_CREATE_PLAY
{
    namespace AutoEditor
    {
        public static class AutoEdCntrlBuilder
        {
            static Vector3 vector3;
            static Vector2 vector2;
            static FieldInfo info;

            public static void CreateControl(EditorFieldAttr attr, Type t, ref System.Object obj)
            {
                if (attr.CtrlType == ControlType.intField)
                {
                    info = t.GetField(attr.Name);
                    if (info != null)
                    {
                        int intVal = (int)info.GetValue(obj);
                        intVal = EditorGUILayout.IntField(attr.Name, intVal);
                        info.SetValue(obj, intVal);
                    }

                }
                else if (attr.CtrlType == ControlType.floatField)
                {
                    info = t.GetField(attr.Name);
                    if (info != null)
                    {
                        float ctrl = (float)info.GetValue(obj);
                        ctrl = EditorGUILayout.FloatField(attr.Name, ctrl);
                        info.SetValue(obj, ctrl);
                    }
                }
                else if (attr.CtrlType == ControlType.boolField)
                {
                    GUILayout.BeginHorizontal();
                    info = t.GetField(attr.Name);
                    bool ctrl = false;
                    if (info != null)
                    {
                        ctrl = (bool)info.GetValue(obj);
                        ctrl = EditorGUILayout.Toggle("", ctrl, GUILayout.Width(17));
                        GUILayout.Label(attr.Name);
                        info.SetValue(obj, ctrl);
                    }
                    GUILayout.EndHorizontal();

                    if (ctrl && attr.Message != "")
                        EditorGUILayout.HelpBox(attr.Message, MessageType.Info);
                }
                else if (attr.CtrlType == ControlType.vector2)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(2);
                    GUILayout.Label(attr.Name, GUILayout.MinWidth(125));
                    GUILayout.Space(-19);
                    info = t.GetField(attr.Name);
                    if (info != null)
                    {
                        vector2 = (Vector2)info.GetValue(obj);
                        vector2 = EditorGUILayout.Vector2Field("", (Vector2)vector2);
                        info.SetValue(obj, (Vector2)vector2);
                    }
                    GUILayout.EndHorizontal();
                }
                else if (attr.CtrlType == ControlType.vector3)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(2);
                    GUILayout.Label(attr.Name, GUILayout.MinWidth(125));
                    GUILayout.Space(-19);
                    info = t.GetField(attr.Name);
                    if (info != null)
                    {
                        vector3 = (Vector3)info.GetValue(obj);
                        vector3 = EditorGUILayout.Vector3Field("", (Vector3)vector3);
                        info.SetValue(obj, (Vector3)vector3);
                    }
                    GUILayout.EndHorizontal();
                }
                else if (attr.CtrlType == ControlType.boldLabel)
                {
                    GUILayout.Space(5f);
                    GUILayout.Label(attr.Name, AutoEditor.labelStyle_bold);
                    GUILayout.Space(3f);
                }
                else if (attr.CtrlType == ControlType.text)
                {
                    GUILayout.BeginHorizontal();
                    info = t.GetField(attr.Name);
                    if (info != null)
                    {
                        GUILayout.Label(attr.Name);
                        GUILayout.Space(-250f);
                        GUILayout.Label(info.GetValue(obj).ToString());
                    }
                    GUILayout.EndHorizontal();
                }
                else if (attr.CtrlType == ControlType.textControl)
                {
                    GUILayout.BeginHorizontal();

                    info = t.GetField(attr.Name);
                    if (info != null)
                    {
                        GUILayout.Label(attr.Name);
                        string ctrl = (string)info.GetValue(obj);
                        ctrl = GUILayout.TextField(ctrl, GUILayout.MinWidth(250));
                        info.SetValue(obj, ctrl);
                    }

                    GUILayout.EndHorizontal();
                }
                else if (attr.CtrlType == ControlType.tag)
                {
                    info = t.GetField(attr.Name);
                    if (info != null)
                    {
                        // GUILayout.Label(attr.Name);
                        string ctrl = (string)info.GetValue(obj);
                        ctrl = EditorGUILayout.TagField(attr.Name, ctrl);
                        info.SetValue(obj, ctrl);
                    }
                }
                else if (attr.CtrlType == ControlType.space)
                {
                    info = t.GetField(attr.Name);
                    if (info != null)
                    {
                        int space = (int)info.GetValue(obj);

                        GUILayout.Space(space);
                    }
                }
                else if (attr.CtrlType == ControlType.layerField)
                {
                    info = t.GetField(attr.Name);
                    if (info != null)
                    {
                        LayerMask mask = (LayerMask)info.GetValue(obj);
                        mask = EditorGUILayout.LayerField(attr.Name, (LayerMask)mask);
                        info.SetValue(obj, (LayerMask)mask);
                    }
                }
                else if (attr.CtrlType == ControlType.gameObject)
                {
                    info = t.GetField(attr.Name);
                    if (info != null)
                    {
                        GameObject go = default;

                        try
                        {
                            go = (GameObject)info.GetValue(obj);
                        }
                        catch (InvalidCastException)
                        {
                            info.SetValue(obj, null);
                            go = (GameObject)info.GetValue(obj);
                            // Debug.Log(except.Message);
                        }

                        go = (UnityEngine.GameObject)EditorGUILayout.ObjectField(attr.Name, (UnityEngine.GameObject)go,
                            typeof(GameObject), allowSceneObjects: false);

                        info.SetValue(obj, (UnityEngine.GameObject)go);
                    }
                }
                else if (attr.CtrlType == ControlType.texture2d)
                {
                    info = t.GetField(attr.Name);
                    if (info != null)
                    {
                        Texture2D texture = (Texture2D)info.GetValue(obj);
                        texture = (UnityEngine.Texture2D)EditorGUILayout.ObjectField(attr.Name, (UnityEngine.Texture2D)texture,
                            typeof(Texture2D), allowSceneObjects: false);
                        info.SetValue(obj, (UnityEngine.Texture2D)texture);
                    }
                }
                else if (attr.CtrlType == ControlType.unityTerrainField)
                {
                    info = t.GetField(attr.Name);
                    if (info != null)
                    {
                        Terrain terrain = (Terrain)info.GetValue(obj);
                        terrain = (Terrain)EditorGUILayout.ObjectField(attr.Name, (Terrain)terrain,
                            typeof(Terrain), allowSceneObjects: true);
                        info.SetValue(obj, (Terrain)terrain);
                    }
                }
                else if (attr.CtrlType == ControlType.intSlider)
                {
                    info = t.GetField(attr.Name);
                    if (info != null)
                    {
                        int intVal = (int)info.GetValue(obj);

                        // upcast EditorFieldControl to it's inherited FloatSliderAttr
                        IntSliderAttr upcastAttr = attr as IntSliderAttr;

                        intVal = EditorGUILayout.IntSlider(attr.Name, intVal, upcastAttr.minVal, upcastAttr.maxVal);
                        info.SetValue(obj, intVal);
                    }
                }
                else if (attr.CtrlType == ControlType.floatSlider)
                {
                    info = t.GetField(attr.Name);
                    if (info != null)
                    {
                        float intVal = (float)info.GetValue(obj);

                        // upcast EditorFieldControl to it's inherited FloatSliderAttr
                        FloatSliderAttr upcastAttr = attr as FloatSliderAttr;

                        intVal = EditorGUILayout.Slider(attr.Name, intVal, upcastAttr.minVal, upcastAttr.maxVal);
                        info.SetValue(obj, intVal);
                    }
                }
                else if (attr.CtrlType == ControlType.color)
                {
                    info = t.GetField(attr.Name);
                    if (info != null)
                    {
                        Color c = (Color)info.GetValue(obj);
                        c = EditorGUILayout.ColorField(attr.Name, c);
                        info.SetValue(obj, (Color)c);
                    }
                }
            }

            public static void DrawGameObjectList(List<GameObject> gameObjects,
                System.Action<int> onSelect,
                System.Action<GameObject> onAdd,
                System.Action<int> onRemove,
                System.Func<int> getSelectedItemIndex,
                System.Action<int, GameObject> onChangeDetect)
            {
                if (gameObjects.Count == 0)
                    onAdd(null);

                GameObject previousGO = null;
                Color oldColor = default;

                for (int i = 0; i < gameObjects.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(2);

                    previousGO = gameObjects[i];

                    // create an object field for this PaintMesh
                    gameObjects[i] = (GameObject)EditorGUILayout.ObjectField(
                    gameObjects[i], typeof(GameObject), true);

                    if (previousGO != null && previousGO != gameObjects[i])
                    {
                        // Debug.Log("change detected");
                        onChangeDetect(i, gameObjects[i]);
                    }

                    // --------------------------------------------------------------
                    // switch to selected button colour if this PaintMesh is selected.
                    oldColor = GUI.backgroundColor;
                    if (i == getSelectedItemIndex()) // get selected
                    { GUI.backgroundColor = Color.green; }

                    // create a button to select this PaintMesh.
                    if (gameObjects[i] != null && GUILayout.Button("Select")) // set selected
                        onSelect(i);

                    // revert to old colour.
                    GUI.backgroundColor = oldColor;
                    // -----------------------------------------------------------------

                    // create a button to remove this Gameobject.
                    oldColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.red;
                    if (gameObjects[i] != null && GUILayout.Button("-"))
                    {
                        onRemove(i);

                        if (i == 0 && gameObjects.Count > 0)
                            onSelect(i); // set selected

                        else if (i > 0 && gameObjects.Count > 0)
                            onSelect(i - 1); // set selected

                        else
                            onSelect(-1); // set selected
                    }
                    GUI.backgroundColor = oldColor;
                    // -----------------------------------------------------------------

                    GUILayout.EndHorizontal();
                }

                // add a new null PaintMesh if count is > 0.
                if (gameObjects.Count > 0 && gameObjects[gameObjects.Count - 1] != null)
                {
                    var xx = gameObjects[gameObjects.Count - 1];
                    if (xx != null)
                        onAdd(null);
                }

            }
        }

        [SerializeField]
        public class AutoEditor
        {
            public static bool LAYOUT_VERTICAL = false;
            [System.NonSerialized] public static GUIStyle labelStyle_bold;
            [System.NonSerialized] public Color textColor = new Color(0.8f, 0.8f, 0.8f, 1);

            Type type;
            System.Object obj;
            readonly bool createFoldOut = false;
            bool bfoldOut = false;
            string foldOutName = "";


            public AutoEditor(Type _type, ref System.Object _obj, 
                bool _createFoldout = false, string _foldOutName = "Foldout")
            {
                type = _type;
                obj = _obj;
                createFoldOut = _createFoldout;
                foldOutName = _foldOutName;
            }

            public void Build(float vOffset = 0, float hOffset = 0)
            {
                if (createFoldOut)
                {
                    bfoldOut = EditorGUILayout.Foldout(bfoldOut, foldOutName);
                    if(bfoldOut)
                        Layout(vOffset, hOffset);
                }
                else
                {
                    Layout(vOffset, hOffset);
                }
            }

            private void Layout(float vOffset, float hOffset)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(hOffset);

                GUILayout.BeginVertical();

                foreach (FieldInfo field in type.GetFields())
                {
                    foreach (Attribute attr in field.GetCustomAttributes(true))
                    {
                        // --------------**------------  //
                        if (attr is EditorFieldAttr atr)
                        {
                            // 1 = start layout horizontal
                            if (atr.LayoutHorizontal == 1)
                            {
                                GUILayout.BeginHorizontal();
                                CreateControl(atr);
                            }

                            // -1 = end layout horizontal
                            else if (atr.LayoutHorizontal == -1)
                            {
                                CreateControl(atr);
                                GUILayout.EndHorizontal();
                            }

                            else
                            {
                                CreateControl(atr);
                            }
                        }
                        // --------------**------------  //
                    }
                }

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            void CreateControl(EditorFieldAttr atr)
            {
                AutoEdCntrlBuilder.CreateControl(atr, type, ref obj);
            }
        }


        [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
        public class EditorFieldAttr : Attribute
        {
            public ControlType CtrlType { get; }

            public string Name { get; } = "";

            // 1 = start layout horizontal
            // 0 = keep layout horizontal
            // -1 = end layout horizontal
            // some other int = dont layout horizontal
            public int LayoutHorizontal { get; set; } = -1;

            public string Message { get; set; } = "";

            public EditorFieldAttr(ControlType t, string _name, int layoutHorizontal = -23, string msg = "")
            {
                CtrlType = t;
                Name = _name;
                LayoutHorizontal = layoutHorizontal;
                Message = msg;
            }
        }


        public class IntSliderAttr : EditorFieldAttr
        {
            public int minVal = 1;
            public int maxVal = 100;

            public IntSliderAttr(ControlType t, string _name, int min, int max) : base(t, _name)
            {
                minVal = min;
                maxVal = max;
            }
        }


        public class FloatSliderAttr : EditorFieldAttr
        {
            public float minVal = 0.5f;
            public float maxVal = 50f;

            public FloatSliderAttr(ControlType t, string _name, float min, float max) : base(t, _name)
            {
                minVal = min;
                maxVal = max;
            }
        }


        public enum ControlType
        {
            floatField,
            intField,
            boolField,
            vector2,
            vector3,
            boldLabel,
            text,
            textControl,
            space,
            layerField,
            tag,
            intSlider,
            floatSlider,
            color,
            gameObject,
            texture2d,
            unityTerrainField,
        }
    } 
}