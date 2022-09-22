using UnityEngine;

using UnityEditor;

public class GameManager : MonoBehaviour
{
    #region EXPOSED_FIELDS
    public Miners miners;
    #endregion
}

[CustomEditor(typeof(GameManager))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameManager myScript = (GameManager)target;
        if (GUILayout.Button("Spawn Miner"))
        {
            myScript.miners.SpawnMiner();
        }
    }
}