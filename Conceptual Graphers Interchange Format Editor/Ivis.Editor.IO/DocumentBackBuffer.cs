using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace Ivis.Editor.IO
{
	[ComVisible(false)]
	internal class DocumentBackBuffer : IDisposable
	{
		private const long MemBlockSize = 0x10000;

		private readonly MemoryMappedFile _mappedFile;

		public DocumentBackBuffer(string filePath)
		{
			_mappedFile = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open, Path.GetFileNameWithoutExtension(filePath));
		}

		public DocumentBackBuffer(long capacity)
		{
		}

		public BackBuffer Get(byte[] buffer, long offset, int length)
		{
			if (buffer == null) throw new ArgumentNullException("buffer");
			if (buffer.Length < length)
				throw new ArgumentException("'buffer' is not large enough to contian 'length' of byte.", "buffer");

			if (offset < 0) throw new ArgumentOutOfRangeException("offset");
			if (length < 0) throw new ArgumentOutOfRangeException("length");

			var bf = new BackBuffer { Buffer = buffer, StartIndex = offset, Length = length };

			using (var accessor = _mappedFile.CreateViewAccessor(offset, length))
			{
				var ofs = 0;
				while (length > 0)
				{
					var len = (int)Math.Min(length, MemBlockSize / 4);

					var count = accessor.ReadArray(offset, buffer, ofs, len);
					if (count < len)
						break;
					offset += count;
					ofs += count;
					length -= count;
				}
			}

			bf.Length = bf.Buffer.Length;

			return bf;
		}

		public void Set(BackBuffer backBuffer)
		{
			using (var accessor = _mappedFile.CreateViewAccessor(backBuffer.StartIndex, backBuffer.Length))
			{
				var length = backBuffer.Length;
				var offset = backBuffer.StartIndex;
				var ofs = 0;
				while (length > 0)
				{
					var len = (int)Math.Min(length, MemBlockSize / 4);
					accessor.WriteArray(offset, backBuffer.Buffer, ofs, len);
					offset += len;
					ofs += len;
					length -= len;
				}
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing) return;

			if (_mappedFile != null)
				_mappedFile.Dispose();
		}

		void IDisposable.Dispose()
		{
			Dispose(false);
		}
	}
}
