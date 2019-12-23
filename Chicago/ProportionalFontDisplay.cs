using System;

using Meadow.Foundation;
using Meadow.Foundation.Displays;

namespace MeadowApplication1 {
	public static class DisplayBaseExtension { 
	//public class ProportionalFontDisplay<DB> : DisplayBase where DB : DisplayBase {
	//	protected DB instance;

	//	private ProportionalFontDisplay() { }
	//	public ProportionalFontDisplay( DB instance ) {
	//		this.instance = instance;
	//	}
		
	//	public override DisplayColorMode ColorMode => instance.ColorMode;
		
	//	public override uint Width => instance.Width;

	//	public override uint Height => instance.Width;

	//	public override void Clear( bool updateDisplay = false ) => instance.Clear( updateDisplay );

	//	public override void DrawBitmap( int x, int y, int width, int height, byte[] bitmap, BitmapMode bitmapMode ) => instance
	//		.DrawBitmap( x, y, width, height, bitmap, bitmapMode );

	//	public override void DrawBitmap( int x, int y, int width, int height, byte[] bitmap, Color color ) => instance
	//		.DrawBitmap( x, y, width, height, bitmap, color );

	//	public override void DrawPixel( int x, int y, Color color ) => instance
	//		.DrawPixel( x, y, color );

	//	public override void DrawPixel( int x, int y, bool colored ) => instance
	//		.DrawPixel( x, y, colored );

	//	public override void Show() => instance.Show();

		public static void DrawBitmap( this DisplayBase displayBase, int x, int y, byte[][] buffer, DisplayBase.BitmapMode bitmapMode ) {
			for( int row = 0; row < buffer.Length; row++ ) {
				displayBase.DrawBitmap( x, y + row, buffer[ row ].Length, 1, buffer[ row ], bitmapMode );
			}
		}
	}
}
