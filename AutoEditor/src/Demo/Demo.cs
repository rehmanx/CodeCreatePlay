using System.Collections.Generic;
using UnityEngine;
using CODE_CREATE_PLAY.AutoEditor;


public class Demo : MonoBehaviour
{
    [System.Serializable]
    public class Settings
    {
        public Settings Instance = null;

        [EditorFieldAttr(ControlType.floatField, _name: "floatField")]
        public float floatField = 1.0f;

        [EditorFieldAttr(ControlType.intField, "intField")]
        public int intField = 1;

        [EditorFieldAttr(ControlType.textControl, "stringField")]
        public string stringField = "String Value";

        [EditorFieldAttr(ControlType.boolField, "boolField")]
        public bool boolField = false;

        [EditorFieldAttr(ControlType.color, "colorField")]
        public Color colorField = Color.red;

        // ***********READ_ME*********** //
        // ***********IMPORTANT*********** //
        // all other controls except intSliders and float sliders
        // use EditorFieldAttr, int and float sliders use intSliderAttr and
        // floatSliderAttr respectively.

        [IntSliderAttr(ControlType.intSlider, "intSlider", 0, 10)]
        public int intSlider = 5;

        [FloatSliderAttr(ControlType.floatSlider, "floatSlider", 0f, 1f)]
        public float floatSlider = 0.5f;

        // ----------------------------------------------------------

        // ** field responsible for adding space must be NonSerialized **
        [EditorFieldAttr(ControlType.space, "space_01")]
        [System.NonSerialized] public int space_01 = 45;

        [EditorFieldAttr(ControlType.gameObject, "gameObject")]
        public GameObject gameObject = null;

        [EditorFieldAttr(ControlType.texture2d, "texture2d")]
        public Texture2D texture2d = null;

        // -------------------------------------------------------------- //

        [EditorFieldAttr(ControlType.space, "space_01")]
        [System.NonSerialized] public int space_02 = 45;

        // ** begin layout horizontal by passing layoutHorizontal = 1 **
        [EditorFieldAttr(ControlType.boolField, "check_0", layoutHorizontal:1)]
        public bool check_0 = false;

        [EditorFieldAttr(ControlType.boolField, "check_1")]
        public bool check_1 = false;

        [EditorFieldAttr(ControlType.boolField, "check_2")]
        public bool check_2 = false;

        // ** begin layout horizontal by passing layoutHorizontal = -1 **
        [EditorFieldAttr(ControlType.boolField, "check_3", layoutHorizontal: -1)]
        public bool check_3 = false;
    }

    [SerializeField] private Settings settings = new Settings();

    private AutoEditor settingsAutoEd = null;
    public AutoEditor SettingsAutoEd
    {
        get
        {
            if(settingsAutoEd == null)
            {
                System.Object obj = settings;
                settingsAutoEd = new AutoEditor(typeof(Settings), ref obj, true);
            }

            return settingsAutoEd;
        }
    }


    public List<GameObject> gameObjects = new List<GameObject>();
    private int selectedGameObjectIDx = -1;

    public void OnAddGameObject(GameObject go)
    {
        gameObjects.Add(go);
        if (gameObjects.Count > 1)
            selectedGameObjectIDx = gameObjects.Count - 2;
    }

    public void OnRemoveGameObject(int index)
    {
        gameObjects.RemoveAt(index);
    }

    public void OnChangeDetect(int index, GameObject go)
    {
        gameObjects[index] = go;
        OnSelect(index);
    }

    public void OnSelect(int index)
    {
        selectedGameObjectIDx = index;
    }

    public int GetSelectedGameObjectIndex()
    {
        return selectedGameObjectIDx;
    }
}
