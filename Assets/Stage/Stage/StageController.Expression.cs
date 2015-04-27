using Gem;
using Gem.Expression;
using UnityEngine;

namespace HX.Stage
{
	public partial class StageController
	{
		private Bind mBinds;

		private Bind MakeBinds()
		{
			var _ret = new Bind
			{
				{
					"BIND_EVENT", _args =>
					{
						var _key = (string) _args[0];
						switch (_key)
						{
							case "ON_UPDATE":
								return 0;
							default:
								L.E(L.M.ENUM_UNDEFINED(_key));
								break;
						}

						return null;
					}
				},
				{
					"BIND_STAGE", _args =>
					{
						var _key = (string) _args[0];
						switch (_key)
						{
							case "NEO_POSITION":
							{
								if (!neo) break;
								return (Vector2) neo.transform.position;
							}
							case "MARKER_POSITION":
							{
								var _markerKey = (string)_args[1];
								var _marker = world.FindMarker(_markerKey);
								if (_marker == null) break;
								return _marker.position;
							}
							default:
								L.E(L.M.ENUM_UNDEFINED(_key));
								break;
						}

						return Vector2.zero;
					}
				}
			};

			return _ret;
		}


		void SetupExpression()
		{
			if (mBinds != null)
			{
				L.E("already setuped.");
				return;
			}

			mBinds = MakeBinds();
			Solver.g.Add(mBinds);
		}

		void PurgeExpression()
		{
			Solver.g.Remove(mBinds);
		}
	}
}
