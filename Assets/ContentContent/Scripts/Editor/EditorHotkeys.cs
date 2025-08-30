using UnityEditor;

namespace ContentContent.Editor
{
	internal static class EditorHotkeys
	{
		[MenuItem("Tools/ContentContent/Toggle Inspector Lock %l")] // %l assigns Ctrl+L (or Cmd+L on Mac)
		private static void ToggleInspectorLock()
		{
			ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
			ActiveEditorTracker.sharedTracker.ForceRebuild();
		}
	}
}