using System.Text;

namespace Ivis.Editor.IO
{
	public class Document
	{
		public static readonly Encoding DefaultEncoding = Encoding.UTF8;

		public Document(string filePath)
		{
		}

		public string FullName { get; set; }

		public Encoding Encoding { get; set; }

		public long TotalLines { get; set; }

		public long Length { get; set; }
	}
}
