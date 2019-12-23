using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteUtility;

namespace Meadow.Foundation.Displays {
	public static class SSD1306Extensions {

		private static readonly byte[] UGdash2832HSWEG02SetupSequence = new byte[] {
			0xAE,		// display off
			0xD5, 0x80, // Set Display Clock Divide Ratio/Oscillator Frequency 
			0xA8, 0x1F, // Set Multiplex Ratio
			0xD3, 0x00, // Set Display Offset
			0x40,		// Set Display Start Line
			0x8D, 0x14,	// 	Set Charge Pump
			0xA1,		// Set Segment Re-Map
			0xC8,		// Set COM Output Scan Direction
			0xDA, 0x02, // Set COM Pins Hardware Configuration
			0x81, 0x8F, // Set Contrast Control
			0xD9, 0xF1, // Set Pre-Charge Period
			0xDB, 0x40, // Set VCOMH Deselect Level
			0xA4,		// Set Entire Display On/Off
			0xA6,		// Set Normal/Inverse Display
			0xAF,		// Set Display On
		};

		public static void SendUGdash2832HSWEG02Startup( this SSD1306 display ) {
			display.SendCommandsInternal( UGdash2832HSWEG02SetupSequence );
		}

		public static byte GetPageCount( this SSD1306 display ) {
			return ( byte )( display.Height / 8 );
		}
		public static void DrawBitmap( this SSD1306 display, byte startColumn, byte startPage, VerticalByteBitmap bitmap ) {

			var buffer = GetBuffer( display );

			var width = Math.Min( display.Width, startColumn + bitmap.Width );

			var pageHeight = display.Height / 8;

			var pageWidth = display.Width;

			for( int yPage = 0; yPage < bitmap.Rectangle.YByteHeight; yPage++ ) {

				var singlePage = bitmap.Buffer[ yPage ];

				if( yPage + startPage < pageHeight ) {

					for( int xIndex = 0; xIndex < bitmap.Width; xIndex++ ) {
						var offset = xIndex + startColumn + ( ( yPage + startPage ) * pageWidth );

						if( offset < buffer.Length )
							buffer[ offset ] |= singlePage[ xIndex ];
					}
				}
			}
		}

		internal static byte[] GetBuffer( SSD1306 display ) {
			var properties = typeof( SSD1306 ).GetFields( System.Reflection.BindingFlags.Instance
							   | System.Reflection.BindingFlags.GetProperty
							   | System.Reflection.BindingFlags.SetProperty
							   | System.Reflection.BindingFlags.NonPublic );

			for( int index = 0; index < properties.Length; index++ ) {
				if( properties[ index ].Name.Contains( "buffer" ) )
					return properties[ index ].GetValue( display ) as byte[];
			}

			return null;
		}

		internal static void SendCommandInternal( this SSD1306 display, byte command ) {
			var method = typeof( SSD1306 ).GetMethod( "SendCommand", System.Reflection.BindingFlags.Instance
				   | System.Reflection.BindingFlags.InvokeMethod
				   | System.Reflection.BindingFlags.NonPublic );

			method.Invoke( display, new object[] { command } );
		}

		internal static void SendCommandsInternal( this SSD1306 display, byte[] commands ) {
			var method = typeof( SSD1306 ).GetMethod( "SendCommands", System.Reflection.BindingFlags.Instance
				   | System.Reflection.BindingFlags.InvokeMethod
				   | System.Reflection.BindingFlags.NonPublic );

			method.Invoke( display, new object[] { commands } );
		}
	}
}
