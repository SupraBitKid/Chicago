using System;
using ByteUtility;
using FontUtility;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;


namespace Meadow.Foundation.Graphics {
	public static class GraphicsLibraryExtension {

		public static int DrawProportionalText( this GraphicsLibrary graphicsLibrary, ProportionalFontBase fontBase, int x, int y, string text ) {
			var totalWidth = fontBase.GetWidth( text );
			graphicsLibrary.DrawBitmap( x, y, totalWidth, fontBase.Height, fontBase.GetBitmapFromString( text ), DisplayBase.BitmapMode.Copy );
			return fontBase.GetWidth( text );
		}

		public static void DrawBitmap( this GraphicsLibrary graphicsLibrary, int x, int y, int width, int height, Bitmap bitmap, DisplayBase.BitmapMode bitmapMode ) {
			graphicsLibrary.
		}

	}
}
