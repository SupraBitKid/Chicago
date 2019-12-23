using System;
using ByteUtility;
using Meadow.Hardware;

namespace Meadow.Foundation.Displays {
	/// <summary>
	///     Provide an different interface to the SSD1306 family of OLED displays.
	/// </summary>
	public class SSD1306Bitmap {
		#region Enums

		/// <summary>
		///     Allow the programmer to set the scroll direction.
		/// </summary>
		public enum ScrollDirection {
			/// <summary>
			///     Scroll the display to the left.
			/// </summary>
			Left,
			/// <summary>
			///     Scroll the display to the right.
			/// </summary>
			Right,
			/// <summary>
			///     Scroll the display from the bottom left and vertically.
			/// </summary>
			RightAndVertical,
			/// <summary>
			///     Scroll the display from the bottom right and vertically.
			/// </summary>
			LeftAndVertical
		}

		/// <summary>
		///     Supported display types.
		/// </summary>
		public enum DisplayType {
			/// <summary>
			///     0.96 128x64 pixel display.
			/// </summary>
			OLED128x64,
			/// <summary>
			///     0.91 128x32 pixel display.
			/// </summary>
			OLED128x32,
			/// <summary>
			///     64x48 pixel display.
			/// </summary>
			OLED64x48,
			/// <summary>
			///     96x16 pixel display.
			/// </summary>
			OLED96x16,
		}

		public enum ConnectionType {
			SPI,
			I2C,
			Console,
		}

		#endregion Enums

		#region Member variables / fields

		public int Width => this.width;

		public int Height => this.height;

		/// <summary>
		///     SSD1306 SPI display
		/// </summary>
		protected ISpiPeripheral spiPeripheral;

		protected IDigitalOutputPort dataCommandPort;
		protected IDigitalOutputPort resetPort;
		protected IDigitalOutputPort chipSelectPort;
		protected ConnectionType connectionType;
		protected const bool Data = true;
		protected const bool Command = false;

		/// <summary>
		///     SSD1306 I2C display
		/// </summary>
		protected readonly II2cPeripheral i2cPeripheral;

		/// <summary>
		///     Width of the display in pixels.
		/// </summary>
		protected int width;

		/// <summary>
		///     Height of the display in pixels.
		/// </summary>
		protected int height;

		protected DisplayType displayType;

		/// <summary>
		///     Sequence of bytes that should be sent to a 128x64 OLED display to setup the device.
		/// </summary>
		protected readonly byte[] oled128x64SetupSequence =
		{
			0xae, 0xd5, 0x80, 0xa8, 0x3f, 0xd3, 0x00, 0x40 | 0x0, 0x8d, 0x14, 0x20, 0x00, 0xa0 | 0x1, 0xc8,
			0xda, 0x12, 0x81, 0xcf, 0xd9, 0xf1, 0xdb, 0x40, 0xa4, 0xa6, 0xaf
		};
		/// <summary>
		///     Sequence of bytes that should be sent to a 128x32 OLED display to setup the device.
		/// </summary>
		protected readonly byte[] oled128x32SetupSequence = {
			0xae,	//Display Off
			0xd5,	//Set clock
			0x80,	// 0b1000 clock 0 divisor  
			0xa8,	//Multiplex Ratio
			0x1f,	// 64MUX
			0xd3,	//Display offset
			0x00,	// 0
			0x40 | 0x0,	//Display start line // 0
			0x8d,	//Charge Pump
			0x14,	// Enable charge pumpb
			0x20,	//Memory address mode
			0x00,	// Horizontal Address mode
			0xa0 | 0x1,	//Segment Re-map - column 127 mapped to SEG0
			0xc8,	//COM output scan direction from COM[N-1] to COM0
			0xda,	//COM pins hardware config
			0x02,	// Sequential disable left/right remap
			0x81,	//Contrast control
			0x8f,	// contrast
			0xd9,	//Precharge period
			0x1f,	// phase one 15 phase two 1
			0xdb,	//V(COMH) Deselect Level
			0x40,	// ? x V(CC)
			0xa4,	//Entire Display On from RAM
			0xa6,	//Normal and not Inverse
			0xaf	//Display On
		};
		/// <summary>
		///     Sequence of bytes that should be sent to a 96x16 OLED display to setup the device.
		/// </summary>
		protected readonly byte[] oled96x16SetupSequence =
		{
			0xae, 0xd5, 0x80, 0xa8, 0x1f, 0xd3, 0x00, 0x40 | 0x0, 0x8d, 0x14, 0x20, 0x00, 0xa0 | 0x1, 0xc8,
			0xda, 0x02, 0x81, 0xaf, 0xd9, 0x1f, 0xdb, 0x40, 0xa4, 0xa6, 0xaf
		};
		/// <summary>
		///     Sequence of bytes that should be sent to a 64x48 OLED display to setup the device.
		/// </summary>
		protected readonly byte[] oled64x48SetupSequence =
		{
			0xae, 0xd5, 0x80, 0xa8, 0x3f, 0xd3, 0x00, 0x40 | 0x0, 0x8d, 0x14, 0x20, 0x00, 0xa0 | 0x1, 0xc8,
			0xda, 0x12, 0x81, 0xcf, 0xd9, 0xf1, 0xdb, 0x40, 0xa4, 0xa6, 0xaf
		};

		#endregion Member variables / fields

		#region Properties

		/// <summary>
		///     Invert the entire display (true) or return to normal mode (false).
		/// </summary>
		/// <remarks>
		///     See section 10.1.10 in the datasheet.
		/// </remarks>
		public void SetInvertDisplay( bool inverted ) {
			this.SendCommand( ( byte )( inverted ? 0xa7 : 0xa6 ) );
		}

		/// <summary>
		///     Get / Set the contrast of the display.
		/// </summary>
		public void SetContrast( byte contrast ) {
			this.SendCommands( new byte[] { 0x81, contrast } );
		}

		/// <summary>
		///     Put the display to sleep (turns the display off).
		/// </summary>
		public void SetSleep( bool sleepOn ) {
			this.SendCommand( ( byte )( sleepOn ? 0xae : 0xaf ) );
		}


		#endregion Properties

		#region Constructors
		private SSD1306Bitmap() { }

		/// <summary>
		///     Create a new SSD1306 object using the default parameters for
		/// </summary>
		/// <remarks>
		///     Note that by default, any pixels out of bounds will throw and exception.
		///     This can be changed by setting the <seealso cref="IgnoreOutOfBoundsPixels" />
		///     property to true.
		/// </remarks>
		/// <param name="displayType">Type of SSD1306 display</param>
		///
		public SSD1306Bitmap( IIODevice device, ISpiBus spiBus, IPin chipSelectPin, IPin dcPin, IPin resetPin, DisplayType displayType ) {

			this.dataCommandPort = device.CreateDigitalOutputPort( dcPin, false );
			this.resetPort = device.CreateDigitalOutputPort( resetPin, true );
			this.chipSelectPort = device.CreateDigitalOutputPort( chipSelectPin, false );

			this.spiPeripheral = new SpiPeripheral( spiBus, chipSelectPort );

			this.connectionType = ConnectionType.SPI;

			this.InitSSD1306( displayType );
		}

		/// <summary>
		///     Create a new SSD1306 object using the default parameters for
		/// </summary>
		/// <remarks>
		///     Note that by default, any pixels out of bounds will throw and exception.
		///     This can be changed by setting the <seealso cref="IgnoreOutOfBoundsPixels" />
		///     property to true.
		/// </remarks>
		/// <param name="address">Address of the bus on the I2C display.</param>
		/// <param name="displayType">Type of SSD1306 display</param>
		public SSD1306Bitmap( II2cBus i2cBus, byte address, DisplayType displayType ) {

			this.displayType = displayType;
			this.i2cPeripheral = new I2cPeripheral( i2cBus, address );
			this.connectionType = ConnectionType.I2C;

			this.InitSSD1306( displayType );
		}

		public SSD1306Bitmap( DisplayType displayType ) {
			this.displayType = displayType;
			this.connectionType = ConnectionType.Console;

			this.InitSSD1306( displayType );
		}

		protected void InitSSD1306( DisplayType displayType ) {

			switch( displayType ) {
				case DisplayType.OLED128x64:
				case DisplayType.OLED64x48:
					this.width = 128;
					this.height = 64;
					this.SendCommands( this.oled128x64SetupSequence );
					break;
				case DisplayType.OLED128x32:
					this.width = 128;
					this.height = 32;
					this.SendCommands( this.oled128x32SetupSequence );
					break;
				case DisplayType.OLED96x16:
					this.width = 96;
					this.height = 16;
					this.SendCommands( this.oled96x16SetupSequence );
					break;
			}

			//this.SetInvertDisplay( false );
			//this.SetSleep( false );
			//this.SetContrast( 0xff );
			//this.StopScrolling();
		}

		#endregion Constructors

		#region Methods

		private void SendColPagePreamble( VerticalByteRectangle rectangle ) {
			this.SendCommands( new byte[] { 0x21, ( byte )rectangle.XStart, ( byte )rectangle.XEnd
							  , 0x22, ( byte )rectangle.YByteStart, ( byte )rectangle.YByteEnd } );
		}

		/// <summary>
		///     Send a command to the display.
		/// </summary>
		/// <param name="command">Command byte to send to the display.</param>
		private void SendCommand( byte command ) {
			if( this.connectionType == ConnectionType.SPI ) {
				this.dataCommandPort.State = Command;
				this.spiPeripheral.WriteByte( command );
			}
			else {
				if( this.connectionType == ConnectionType.I2C ) {
					this.i2cPeripheral.WriteBytes( new byte[] { 0x00, command } );
				}
				else {
					Console.WriteLine( "byte: {0}", command );
				}
			}
		}

		/// <summary>
		///     Send a sequence of commands to the display.
		/// </summary>
		/// <param name="commands">List of commands to send.</param>
		protected void SendCommands( byte[] commands ) {
			if( this.connectionType == ConnectionType.SPI ) {
				this.dataCommandPort.State = Command;
				this.spiPeripheral.WriteBytes( commands );
			}
			else {
				if( this.connectionType == ConnectionType.I2C ) {
					this.i2cPeripheral.WriteByte( 0x00 );
					this.i2cPeripheral.WriteBytes( commands );
				}
				else {
					Console.WriteLine( "bytes: {0}", commands.Length );
				}
			}
		}

		/// <summary>
		///     Clear the display buffer.
		/// </summary>
		public void Clear(  ) {
			var rectangle = new VerticalByteRectangle( 0, 0, this.width, this.height );

			this.SendColPagePreamble( rectangle );

			var totalBytes = ( this.width * this.height ) / 8;

			this.SendCommands( new byte[ totalBytes ] );
			//for( int index = 0; index < totalBytes; index++ )
			//	this.SendCommand( 0 );
		}


		public void DrawBitmap( int x, int y, VerticalByteBitmap bitmap ) {
			var rectangle = new VerticalByteRectangle( x, y, bitmap.Width, bitmap.Height );

			this.SendColPagePreamble( rectangle );

			var buffer = bitmap.ShiftLsbToMsb( rectangle.YOffsetStart );

			//for( int index = 0; index < buffer.GetLength( 0 ); index++ ) {
			//	this.SendCommands( buffer[ index ] );
			//}
		}

#if false
		/// <summary>
		///     Start the display scrollling in the specified direction.
		/// </summary>
		/// <param name="direction">Direction that the display should scroll.</param>
		public void StartScrolling( ScrollDirection direction ) {
			StartScrolling( direction, 0x00, 0xff );
		}

		/// <summary>
		///     Start the display scrolling.
		/// </summary>
		/// <remarks>
		///     In most cases setting startPage to 0x00 and endPage to 0xff will achieve an
		///     acceptable scrolling effect.
		/// </remarks>
		/// <param name="direction">Direction that the display should scroll.</param>
		/// <param name="startPage">Start page for the scroll.</param>
		/// <param name="endPage">End oage for the scroll.</param>
		public void StartScrolling( ScrollDirection direction, byte startPage, byte endPage ) {
			StopScrolling();
			byte[] commands;
			if( ( direction == ScrollDirection.Left ) || ( direction == ScrollDirection.Right ) ) {
				commands = new byte[] { 0x26, 0x00, startPage, 0x00, endPage, 0x00, 0xff, 0x2f };
				if( direction == ScrollDirection.Left ) {
					commands[ 0 ] = 0x27;
				}
			}
			else {
				byte scrollDirection = ( byte )( direction == ScrollDirection.LeftAndVertical ? 0x2a : 0x29 );
				commands = new byte[]
					{ 0xa3, 0x00, (byte) _height, scrollDirection, 0x00, startPage, 0x00, endPage, 0x01, 0x2f };
			}
			SendCommands( commands );
		}
#endif 
		/// <summary>
		///     Turn off scrolling.
		/// </summary>
		/// <remarks>
		///     Datasheet states that scrolling must be turned off before changing the
		///     scroll direction in order to prevent RAM corruption.
		/// </remarks>
		public void StopScrolling() {
			this.SendCommand( 0x2e );
		}
#endregion Methods
	}
}