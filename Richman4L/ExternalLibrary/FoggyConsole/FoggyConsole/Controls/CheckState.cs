using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace WenceyWang . FoggyConsole . Controls
{

	/// <summary>
	///     All possible states for
	///     <code>Checkbox</code>
	/// </summary>
	public enum CheckState
	{

		/// <summary>
		///     The checkbox is in an indeterminate state,
		///     can be used to force to user to actively select one state
		/// </summary>
		Indeterminate ,

		/// <summary>
		///     The checkbox is checked
		/// </summary>
		Checked ,

		/// <summary>
		///     The checkbox is unchecked
		/// </summary>
		Unchecked

	}

}
