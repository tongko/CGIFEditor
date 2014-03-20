using Ivis.Editor.Drawing;
using Ivis.Editor.Properties;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Ivis.Editor
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			CustomInitialize();
		}

		private void CustomInitialize()
		{
			SuspendLayout();
			Text = Resources.ApplicationName;
			Size = Settings.Default.MainFormSize;
			Location = Settings.Default.MainFormLocation;

			Font = SystemFonts.DialogFont;
			_mnuStrip.Renderer = new MenuRenderer(this);
			//_mnuStrip.RenderMode = ToolStripRenderMode.Custom;

			ResumeLayout();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			var s = Settings.Default;
			s.MainFormLocation = Location;
			s.MainFormSize = Size;

			s.Save();
		}
	}
}
