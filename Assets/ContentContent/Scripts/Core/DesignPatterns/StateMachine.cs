namespace ContentContent.Core
{
	/// <summary>
	/// Simple generic state machine. Dependent on <see cref="State{T}"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class StateMachine<T>
	{
		public T Owner { get; private set; }
		public State<T> CurrentState { get; private set; }
		public State<T> PreviousState { get; private set; }

		public StateMachine(T owner)
		{
			Owner = owner;
			CurrentState = null;
			PreviousState = null;
		}

		public void ChangeState(State<T> newState)
		{
			if (newState == null)
				return;

			if (CurrentState != null)
			{
				PreviousState = CurrentState;
				CurrentState.OnExit(Owner);
			}
			CurrentState = newState;
			CurrentState.OnEnter(Owner);
		}

		public void ReturnToPreviousState()
		{
			if (CurrentState == null || PreviousState == null)
				return;

			ChangeState(PreviousState);
		}

		public void Update()
		{
			if (CurrentState != null)
				CurrentState.OnUpdate(Owner);
		}
	} 
}