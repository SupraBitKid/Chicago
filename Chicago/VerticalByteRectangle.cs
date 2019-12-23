using System;

namespace ByteUtility {
	/// <summary>
	/// Used to calculate 1 bit per pixel bitmap heights
	/// </summary>
	public class VerticalByteRectangle {
		protected int xStart;
		protected int xEnd;

		protected int yByteStart;
		protected int yByteEnd;
		protected int yPixelHeight;

		protected int yOffsetStart;

		private VerticalByteRectangle() { }

		/// <summary>
		/// Construct a (0,0 upper left) rectangle based upon pixel coords
		/// </summary>
		/// <param name="xPixels">horizontal start location</param>
		/// <param name="yPixels">vertical start location</param>
		/// <param name="widthPixels">horizontal size</param>
		/// <param name="heightPixels">vertical size</param>
		public VerticalByteRectangle( int xPixels, int yPixels, int widthPixels, int heightPixels ) {
			this.xStart = xPixels;
			this.xEnd = xPixels + widthPixels;

			this.yOffsetStart = yPixels % 8;
			this.yByteStart = yPixels / 8;

			this.yPixelHeight = heightPixels;

			var yEnd = yPixels + heightPixels;
			var yOffsetEnd = yEnd % 8;

			this.yByteEnd = ( yOffsetEnd > 0 ? 1 : 0 ) + ( yEnd / 8 );
		}

		/// <summary>
		/// pixel horizontal start coordinate
		/// </summary>
		public int XStart => this.xStart;

		/// <summary>
		/// pixel horizontal end coordinate
		/// </summary>
		public int XEnd => this.xEnd;

		/// <summary>
		/// pixel horizontal width
		/// </summary>
		public int XWidth => this.xEnd - this.xStart;

		/// <summary>
		/// vertical start byte
		/// </summary>
		public int YByteStart => this.yByteStart;

		/// <summary>
		/// vertical end byte
		/// </summary>
		public int YByteEnd => this.yByteEnd;

		/// <summary>
		/// vertical height in bytes
		/// </summary>
		public int YByteHeight => this.yByteEnd - this.yByteStart;

		/// <summary>
		/// vertical height in pixels
		/// </summary>
		public int YPixelHeight => this.yPixelHeight;

		/// <summary>
		/// vertical pixel offset
		/// </summary>
		public int YOffsetStart => this.yOffsetStart;
	}
}
