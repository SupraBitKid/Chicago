using System;
using System.Collections.Generic;
using System.Text;

namespace ByteUtility {
	public class VerticalByteBitmap {
		static readonly byte[] topBitMask = new byte[] {
			0b11111111,
			0b11111110,
			0b11111100,
			0b11111000,
			0b11110000,
			0b11100000,
			0b11000000,
			0b10000000,
			0b00000000
		};
		static readonly byte[] bottomBitMask = new byte[] {
			0b00000000,
			0b00000001,
			0b00000011,
			0b00000111,
			0b00001111,
			0b00011111,
			0b00111111,
			0b01111111,
			0b11111111,
		};

		protected readonly byte[][] backingStore;
		protected readonly VerticalByteRectangle rectangle;

		public VerticalByteBitmap( int width, int height ) {
			this.rectangle = new VerticalByteRectangle( 0, 0, width, height );

			this.backingStore = this.NewBuffer( this.rectangle );
		}

		public VerticalByteBitmap( int height, byte[][] bitmap ) {
			this.rectangle = new VerticalByteRectangle( 0, 0, bitmap[ 0 ].Length, height );

			this.backingStore = bitmap;
		}

		internal VerticalByteBitmap( VerticalByteRectangle verticalRectangle, byte[][] buffer ) {
			this.backingStore = buffer;
			this.rectangle = verticalRectangle;
		}

		public VerticalByteBitmap ShiftLsbToMsb( int shiftCount ) {
			if( shiftCount > 8 )
				throw new Exception( $"Error, ShiftLsbToMsb shiftCount has to be < 8.  Value = {shiftCount}" );

			var carryShiftCount = 8 - shiftCount;

			var sourcePageCount = this.backingStore.GetLength( 0 );

			var newVerticalRectangle = new VerticalByteRectangle( 0, 0, this.rectangle.XWidth, this.rectangle.YPixelHeight + shiftCount );

			var result = this.NewBuffer( newVerticalRectangle );

			var topMask = VerticalByteBitmap.topBitMask[ shiftCount ];
			var bottomMask = VerticalByteBitmap.bottomBitMask[ shiftCount ];

			for( int x = 0; x < newVerticalRectangle.XWidth; x++ ) {

				byte carryForward = 0b00000000;

				for( int page = 0; page < newVerticalRectangle.YByteHeight; page++ ) {

					if( page < sourcePageCount ) {
						var sourceByte = this.backingStore[ page ][ x ];
						var currentByte  = ( byte )( carryForward
												| ( byte )( topMask & ( byte )( sourceByte << shiftCount ) ) );

						result[ page ][ x ] = currentByte;

						carryForward = ( byte )( bottomMask	& ( byte )( sourceByte >> carryShiftCount ) );
					}
					else {
						result[ page ][ x ] = carryForward;
						carryForward = 0;
					}
				}
			}

			return new VerticalByteBitmap( newVerticalRectangle, result );
		}

		public void DrawBitmap( int x, int page, byte[][] source ) {
			var totalPages = Math.Min( page + source.GetLength( 0 ), this.rectangle.YByteHeight );

			for( int sourcePage = 0; sourcePage < totalPages; sourcePage++ ) {
				var sourceWidth = source[ sourcePage ].Length;
				var totalWidth = Math.Min( this.rectangle.XWidth, x + sourceWidth );
				var destinationPage = page + sourcePage;

				for( int xOffset = 0; xOffset < totalWidth; xOffset++ ) {
					this.backingStore[ destinationPage ][ x + xOffset ] |= source[ sourcePage ][ xOffset ];
				}
				
			}
		}

		public void DrawBitmap( int startX, int startPage, VerticalByteBitmap source ) {
			for( int sourcePage = 0; sourcePage < source.rectangle.YByteHeight; sourcePage++ ) {

				var destinationPage = startPage + sourcePage;

				if( destinationPage < this.rectangle.YByteHeight ) {

					for( int xOffset = 0; xOffset < source.rectangle.XWidth; xOffset++ ) {
						var destinationX = startX + xOffset;

						if( destinationX < this.Width )
							this.backingStore[ destinationPage ][ destinationX ] |= source.backingStore[ sourcePage ][ xOffset ];
					}
				}
			}
		}

		public VerticalByteRectangle Rectangle => this.rectangle;

		public int Height => this.rectangle.YPixelHeight;

		public int Width => this.rectangle.XWidth;

		public byte[][] Buffer => this.backingStore;
		
		public byte[][] NewBuffer( VerticalByteRectangle verticalRectangle ) {
			
			var result = new byte[ verticalRectangle.YByteHeight ][];

			for( int index = 0; index < verticalRectangle.YByteHeight; index++ )
				result[ index ] = new byte[ verticalRectangle.XWidth ];

			return result;
		}
	}
}
