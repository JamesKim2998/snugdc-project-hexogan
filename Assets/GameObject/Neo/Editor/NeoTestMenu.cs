using UnityEditor;

public static class NeoTestMenu {

	private const string NEO = "Assets/GameObject/Neo/Test/test_neo.unity";

	[MenuItem("Test/Neo/NeoTest")]
	private static void NeoTest()
	{
		EditorSceneHelper.Transfer(NEO);
	}
}
