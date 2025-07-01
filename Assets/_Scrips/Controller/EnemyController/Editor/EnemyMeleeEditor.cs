using UnityEditor;

[CustomEditor(typeof(EnemyMelee))]
public class EnemyMeleeEditor : Editor
{
    // Implement this function to make a custom inspector.
    // Inside this function you can add your own custom IMGUI based GUI for the inspector of a specific object class.
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
