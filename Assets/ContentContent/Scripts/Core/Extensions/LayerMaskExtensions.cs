using UnityEngine;

namespace ContentContent.Core
{
	public static class LayerMaskExtensions
	{
		public static bool Contains(this LayerMask mask, int layer)
		{
			return (mask.value & (1 << layer)) > 0;
		}

		public static LayerMask Add(this LayerMask mask, params string[] layerNames)
		{
			return mask | NamesToMask(layerNames);
		}

		public static LayerMask Remove(this LayerMask mask, params string[] layerNames)
		{
			LayerMask invertedOriginal = ~mask;
			return ~(invertedOriginal | NamesToMask(layerNames));
		}

		public static LayerMask NamesToMask(params string[] layerNames)
		{
			LayerMask mask = (LayerMask)0;
			foreach (var name in layerNames)
			{
				mask |= (1 << LayerMask.NameToLayer(name));
			}
			return mask;
		}
	} 
}