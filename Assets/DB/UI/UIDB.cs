using System.Collections.Generic;
using FullInspector;

namespace HX.UI
{
	public class UIDB : BaseScriptableObject
	{
		public static UIDB g;

		public Dictionary<AnatomyKey, AnatomyData> anatomies;

		public AnatomyVertexView anatomyVertexPrf;
		public AnatomyConnectionView anatomyConnectionPrf;
	}
}