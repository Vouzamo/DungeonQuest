using UnityEngine;
//using UnityEditor;
using System.Collections;

public class SaveToEditor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Space))
		{
			GameObject dungeon = GameObject.Find("Dungeon");
			//PrefabUtility.CreatePrefab("Assets/Generated.prefab", dungeon, ReplacePrefabOptions.ReplaceNameBased);
		}
	}
}