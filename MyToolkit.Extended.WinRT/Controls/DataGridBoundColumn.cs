using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MyToolkit.Controls
{
	public abstract class DataGridBoundColumn : DataGridColumn
	{
		private Binding _binding;

        /// <summary>
        /// Gets or sets the data binding for this column. 
        /// </summary>
		public Binding Binding
		{
			get { return _binding; }
			set { _binding = value; }
		}

        /// <summary>
        /// Gets the property path which is used for sorting. 
        /// </summary>
		public override PropertyPath OrderPropertyPath
		{
			get { return _binding.Path; }
		}
	}
}