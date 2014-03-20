using System.Drawing;
using System.Windows.Forms;

namespace Ivis.Editor.Drawing
{
	class MenuRenderer : ToolStripSystemRenderer
	{
		public MenuRenderer(Form parentForm)
		{
			ParentForm = parentForm;
		}

		public Form ParentForm { get; private set; }

		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			var g = e.Graphics;
			g.Clear(ParentForm.BackColor);
		}

		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			var g = e.Graphics;
			var rect = e.AffectedBounds;

			var p = new Pen(ParentForm.BackColor);
			g.DrawRectangle(p, rect);
		}
	}
}
