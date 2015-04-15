using Gem;

namespace HX
{
	public static class Const
	{
		public const int RESOLUTION_X = 1920;
		public const int RESOLUTION_Y = 1080;
		public const float RESOLUTION_RATIO = RESOLUTION_X/(float) RESOLUTION_Y;
		public static readonly Point RESOLUTION = new Point(RESOLUTION_X, RESOLUTION_Y);
	}
}