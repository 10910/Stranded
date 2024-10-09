using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Movement))]
public class Gizmos : Editor
{
    private void OnSceneGUI()
    {
        Movement playerEntity = target as Movement;
        Vector3 playerTop = playerEntity.transform.position + playerEntity.characterController.height / 2 * Vector3.up;
        Vector3 playerBottom = playerEntity.transform.position + playerEntity.characterController.height / 2 * Vector3.down;
        Handles.color = Color.yellow;
        Handles.DrawLine(playerTop, playerTop + playerEntity.topOffset * Vector3.up, 4);
        Handles.DrawLine(playerBottom, playerBottom + playerEntity.bottomOffset * Vector3.down, 4);
        //SceneVisibilityManager.instance.DisablePicking();
    }
}
