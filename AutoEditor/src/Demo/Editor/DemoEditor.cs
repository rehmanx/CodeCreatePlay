using UnityEditor;


[CustomEditor(typeof(Demo))]
public class DemoEditor : Editor
{
    Demo demo = null;

    private void OnEnable()
    {
        demo = target as Demo;
    }

    public override void OnInspectorGUI()
    {
        demo.SettingsAutoEd.Build();

        // add some space and a label.
        EditorGUILayout.Space(25f);
        EditorGUILayout.LabelField("GameObjects List");

        CODE_CREATE_PLAY.AutoEditor.AutoEdCntrlBuilder.DrawGameObjectList(
            demo.gameObjects,
            demo.OnSelect,
            demo.OnAddGameObject,
            demo.OnRemoveGameObject,
            demo.GetSelectedGameObjectIndex,
            demo.OnChangeDetect);
    }
}
