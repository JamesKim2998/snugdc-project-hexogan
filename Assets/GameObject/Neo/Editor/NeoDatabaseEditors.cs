using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(NeoBodyDatabase))]
class NeoBodyDatabaseEditor : DatabaseEditor<NeoBodyType, NeoBodyData> { }

[CustomEditor(typeof(NeoArmDatabase))]
class NeoArmDatabaseEditor : DatabaseEditor<NeoArmType, NeoArmData> { }
