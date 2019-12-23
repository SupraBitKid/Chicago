using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bmfont2cs.Xml;

namespace bmfont2cs {
	public static class OutputGenerator {

		public static string AsComments( string lineStart, bool[,] boolArray ) {
			StringBuilder stringBuilder = new StringBuilder();

			for( int y = 0; y < boolArray.GetLength( 1 ); y++ ) {
				stringBuilder.AppendFormat( "{0}//", lineStart );

				for( int x = 0; x < boolArray.GetLength( 0 ); x++ ) {
					stringBuilder.Append( boolArray[ x, y ] ? "O" : "." );
				}
				stringBuilder.AppendLine();
			}

			return stringBuilder.ToString();
		}

		public static string AsCSharpByteArray( string lineStart, byte[][] byteArray ) {
			StringBuilder stringBuilder = new StringBuilder();

			for( var y = 0; y < byteArray.Length; y++ ) {
				stringBuilder.AppendFormat( "{0}new byte[] {{ ", lineStart );
				for( var x = 0; x < byteArray[ y ].Length; x++ ) {
					stringBuilder.AppendFormat( "0x{0:x2}", byteArray[ y ][ x ] );
					if( x < byteArray[ y ].Length - 1 )
						stringBuilder.Append( ", " );
				}
				if( y < byteArray.Length - 1 )
					stringBuilder.AppendLine( " }," );
				else
					stringBuilder.AppendLine( " } " );
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// horizontal bytes width / 8 bytes wide
		/// </summary>
		/// <param name="boolArray"></param>
		/// <returns>byte bitmap yBit, xByte</returns>
		public static byte[][] As1BitPixelFormat( bool[,] boolArray ) {
			var width = (boolArray.GetLength( 0 ) / 8) + ( boolArray.GetLength( 0 ) % 8 > 0 ? 1 : 0 );
			var height = boolArray.GetLength( 1 );
			var result = new byte[ height ][];

			for( int y = 0; y < height; y++ ) {
				result[ y ] = new byte[ width ];
				for( int x = 0; x < width; x++ ) {
					result[ y ][ x ] = GetHorizontalByte( boolArray, y, x );
				}
			}

			return result;
		}

		/// <summary>
		/// vertical bytes width bytes wide
		/// </summary>
		/// <param name="boolArray"></param>
		/// <returns>byte bitmap yByte, xBit </returns>
		public static byte[][] As1BitPageFormat( bool[,] boolArray ) {
			var height = (boolArray.GetLength( 1 ) / 8) + ( boolArray.GetLength( 1 ) % 8 > 0 ? 1 : 0 );
			var width = boolArray.GetLength( 0 );
			var result = new byte[ height ][];

			for( int y = 0; y < height; y++ ) {
				result[ y ] = new byte[ width ];
				for( int x = 0; x < width; x++ ) {
					result[ y ][ x ] = GetVerticalByte( boolArray, x, y );
				}
			}

			return result;
		}

		public static string AsKerningArray( string lineStart, IEnumerable<ushort> characters, BmFontKernings kernings ) {
			var result = new StringBuilder();

			for( var index = 0; index < kernings.Kerning.Length; index++ ) {
				var kerning = kernings.Kerning[ index ];
				if( characters.Contains( kerning.First ) && characters.Contains( kerning.Second ) ) {
					result.AppendFormat( "{0}{{\"{1}{2}\",{3}}},\n", lineStart, ( char )( byte )kerning.First, ( char )( byte )kerning.Second, kerning.Amount );
				}
			}

			return result.ToString();
		}

		private static byte GetHorizontalByte( bool[,] boolArray, int y, int horizontalByte ) {
			int xOffset = horizontalByte * 8;

			return ( byte )( ( GetBit( boolArray, xOffset + 0, y ) ? 0b00000001 : 0 ) |
				( GetBit( boolArray, xOffset + 1, y ) ? 0b00000010 : 0 ) |
				( GetBit( boolArray, xOffset + 2, y ) ? 0b00000100 : 0 ) |
				( GetBit( boolArray, xOffset + 3, y ) ? 0b00001000 : 0 ) |
				( GetBit( boolArray, xOffset + 4, y ) ? 0b00010000 : 0 ) |
				( GetBit( boolArray, xOffset + 5, y ) ? 0b00100000 : 0 ) |
				( GetBit( boolArray, xOffset + 6, y ) ? 0b01000000 : 0 ) |
				( GetBit( boolArray, xOffset + 7, y ) ? 0b10000000 : 0 ) );

		}

		private static byte GetVerticalByte( bool[,] boolArray, int x, int verticalByte ) {
			int yOffset = verticalByte * 8;

			return ( byte )( ( GetBit( boolArray, x, yOffset + 7 ) ? 0b10000000 : 0 ) |
				( GetBit( boolArray, x, yOffset + 6 ) ? 0b01000000 : 0 ) |
				( GetBit( boolArray, x, yOffset + 5 ) ? 0b00100000 : 0 ) |
				( GetBit( boolArray, x, yOffset + 4 ) ? 0b00010000 : 0 ) |
				( GetBit( boolArray, x, yOffset + 3 ) ? 0b00001000 : 0 ) |
				( GetBit( boolArray, x, yOffset + 2 ) ? 0b00000100 : 0 ) |
				( GetBit( boolArray, x, yOffset + 1 ) ? 0b00000010 : 0 ) |
				( GetBit( boolArray, x, yOffset + 0 ) ? 0b00000001 : 0 ) );
		}

		private static bool GetBit( bool[,] boolArray, int x, int y ) {
			return ( x < boolArray.GetLength( 0 ) && y < boolArray.GetLength( 1 ) ) ?
				boolArray[ x, y ] :
				false;
		}


	}
}
