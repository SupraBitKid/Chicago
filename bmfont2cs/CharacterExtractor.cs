using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace bmfont2cs {
	public class CharacterExtractor {
		public CharacterExtractor( Xml.BmFont font, System.Drawing.Bitmap bitmap ) {
			this.font = font;
			this.bitmap = bitmap;
		}

		protected Xml.BmFont font;
		protected System.Drawing.Bitmap bitmap;

		public bool[,] GetCharacter( char fontCharacter, int cropX, int cropY ) {
			var character = this.font.Chars.Characters.First( c => c.Id == ( ushort )( byte )fontCharacter );
			var width = character.XAdvance;
			var height = this.font.Info.Size;

			var map = new bool[ width, height ];


			for( int x = 0; x < width; x++ ) {
				var use_x = x - character.XOffset + cropX;

				for( int y = 0; y < height; y++ ) {
					var use_y = y - character.YOffset + cropY;

					map[ x, y ] =
						use_x >= 0 && use_x < character.Width && use_y >= 0 && use_y < character.Height ?
							this.bitmap.GetPixel( use_x + character.X, use_y + character.Y ).A != ( byte )0 : false;
				}
			}

			return map;
		}

#if false
		public byte[][] GetCharacter( char fontCharacter, int cropX, int cropY ) {

				var bitmapData = this.bitmap.LockBits( rectangle, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format1bppIndexed );

			IntPtr ptr = bitmapData.Scan0;
			var byteWidth = Math.Abs( bitmapData.Stride );
			var rawData = new byte[ bitmapData.Height ][];

			for( int index = 0; index < bitmapData.Height; index++ ) {
				rawData[ index ] = new byte[ byteWidth ];
				var byteOffset = index * byteWidth;
				Marshal.Copy( ptr + byteOffset, rawData[ index ], 0, byteWidth );
			}

			// Unlock the bits.
			this.bitmap.UnlockBits( bitmapData );

			return rawData;
		}
#endif
	}
}
