using UnityEditor;

namespace HX
{

	[CustomEditor(typeof(NeoBodyDatabase))]
	class NeoBodyDatabaseEditor : DatabaseEditor<NeoBodyType, NeoBodyData> { }

	[CustomEditor(typeof(NeoArmDatabase))]
	class NeoArmDatabaseEditor : DatabaseEditor<NeoArmType, NeoArmData> { }

}