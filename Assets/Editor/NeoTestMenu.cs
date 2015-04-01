using UnityEditor;

namespace HX
{
	public static class TestMenu
	{
		private const string NEO = "Assets/GO/Neo/Test/test_neo.unity";

		[MenuItem("HX/Test/Neo/NeoTest")]
		private static void NeoTest()
		{
			EditorSceneHelper.Transfer(NEO);
		}
	}
}