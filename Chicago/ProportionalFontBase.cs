using ByteUtility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FontUtility {
	public abstract class ProportionalFontBase {
		protected Dictionary<char, byte[][]> fontCharacters;
		protected Dictionary<string, short> kerningOffsets;
		
		public VerticalByteBitmap GetCharacter( char character ) {
			if( this.fontCharacters.ContainsKey( character ) )
				return new VerticalByteBitmap( this.Height, this.fontCharacters[ character ] );

			return null;
		}

		public char[] GetAvailableCharacters( string text ) {
			return text.Where( ch => this.fontCharacters.ContainsKey( ch ) ).ToArray();
		}

		public VerticalByteBitmap[] GetCharactersFromString( char[] characters ) {
			return characters.Select( ch => new VerticalByteBitmap( this.Height, this.fontCharacters[ ch ] ) )
						.ToArray();
		}

		public short[] GetKerningsFromString( char[] characters ) {
			var results = new short[ characters.Length ];

			for( int index = 0; index < characters.Length - 1; index++ ) {
				var key = new string( new char[] { characters[ index ], characters[ index + 1 ] } );
				results[ index ] = this.kerningOffsets.ContainsKey( key ) ? this.kerningOffsets[ key ] : ( short )0;
				if( results[ index ] != 0 )
					Console.WriteLine( "{0} kerned {1}", key, results[ index ] );
			}

			return results;
		}

		public VerticalByteBitmap GetBitmapFromString( string text ) {
			var totalWidth = this.GetWidth( text );
			var result = new VerticalByteBitmap( totalWidth, this.Height );

			var characters = this.GetAvailableCharacters( text );
			var characterBitmaps = this.GetCharactersFromString( characters );
			var characterKernings = this.GetKerningsFromString( characters );
			var runningX = 0;

			for( int index = 0; index < characterBitmaps.Length; index++ ) {
				result.DrawBitmap( runningX, 0, characterBitmaps[ index ] );
				runningX += characterBitmaps[ index ].Width + characterKernings[ index ];
			}

			return result;
		}

		public short GetWidth( string text ) {
			short result = 0;

			var characters = this.GetAvailableCharacters( text );
			var characterBitmaps = this.GetCharactersFromString( characters );
			var characterKernings = this.GetKerningsFromString( characters );

			for( int index = 0; index < characterBitmaps.Length; index++ )
				result += ( short )( characterBitmaps[ index ].Width + characterKernings[ index ] );

			return result;
		}

		public short Height { get; protected set; }

		public short Spacing { get; protected set; }
	}
}
