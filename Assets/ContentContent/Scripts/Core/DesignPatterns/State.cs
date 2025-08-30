namespace ContentContent.Core
{
	/// <summary>
	/// State abstract used by a <see cref="StateMachine{T}"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[System.Serializable]
	public abstract class State<T>
	{
		public abstract void OnEnter(T owner);

		public abstract void OnExit(T owner);

		public abstract void OnUpdate(T owner);

		public abstract override string ToString();
	} 
}