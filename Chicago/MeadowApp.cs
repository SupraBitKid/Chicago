using System;
using System.Diagnostics;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Meadow.Gateway.WiFi;
using FontUtility;
using ByteUtility;

namespace ChicagoApp {
	public class MeadowApp : App<F7Micro, MeadowApp> {
		const float pwmFrequency = 400000.0f;
		const uint i2cFrequency = 400000;

		IPwmPort redPwm;
		IPwmPort bluPwm;
		IPwmPort grePwm;
		int delayInMilliseconds = 0;
		SSD1306 display;
		ProportionalFontBase chicagoFont;
		ProportionalFontBase cornerFont;
		VerticalByteBitmap line1;
		Byte line1Page;
		VerticalByteBitmap line2;
		Byte line2Page;
		VerticalByteBitmap topLeftCorner;
		VerticalByteBitmap topRightCorner;
		VerticalByteBitmap bottomLeftCorner;
		VerticalByteBitmap bottomRightCorner;

		public MeadowApp() {
			this.ConfigurePorts();
			this.BlinkLeds();
			while( true ) ;
		}

		public void ConfigurePorts() {
			Console.WriteLine( "Creating Outputs..." );
			this.redPwm = Device.CreatePwmPort( Device.Pins.OnboardLedRed, pwmFrequency, 0f );
			this.bluPwm = Device.CreatePwmPort( Device.Pins.OnboardLedBlue, pwmFrequency, 0f );
			this.grePwm = Device.CreatePwmPort( Device.Pins.OnboardLedGreen, pwmFrequency, 0f );

			var busForDisplay = Device.CreateI2cBus( ) as I2cBus;

			busForDisplay.SetFrequency( i2cFrequency );

			Console.WriteLine( "I2C bus frequency: {0}", busForDisplay.Frequency );

			this.display = new SSD1306( busForDisplay, 0x3C, SSD1306.DisplayType.OLED128x32 );
			this.display.SendUGdash2832HSWEG02Startup();
			this.display.InvertDisplay = true;
			this.display.Clear( true );

			this.chicagoFont = new ShikaakwaProportionalFont();
			this.cornerFont = new CornerProportionalFont();

			var yPosition = 1;
			this.line1 = this.chicagoFont.GetBitmapFromString( "Programmed in C#" ).ShiftLsbToMsb( yPosition );
			this.line1Page = 0;
			yPosition += this.chicagoFont.Spacing;
			this.line2 = this.chicagoFont.GetBitmapFromString( "by SupraBitKid" ).ShiftLsbToMsb( yPosition % 8 );
			this.line2Page = ( byte )( yPosition / 8 );

			this.topLeftCorner = this.cornerFont.GetCharacter( 'a' );
			this.topRightCorner = this.cornerFont.GetCharacter( 'b' );
			this.bottomLeftCorner = this.cornerFont.GetCharacter( 'c' );
			this.bottomRightCorner = this.cornerFont.GetCharacter( 'd' );
		}

		public void BlinkLeds() {

			while( true ) {
				this.display.Clear( false );
				this.display.DrawBitmap( 0, 0, this.topLeftCorner );
				this.display.DrawBitmap( ( byte )( this.display.Width - this.topRightCorner.Width ), 0, this.topRightCorner );
				this.display.DrawBitmap( 0, ( byte )( this.display.GetPageCount() - 1 ), this.bottomLeftCorner );
				this.display.DrawBitmap( ( byte )( this.display.Width - this.bottomRightCorner.Width ),
										( byte )( this.display.GetPageCount() - 1 ), this.bottomRightCorner );
				this.display.DrawBitmap( 6, this.line1Page, this.line1 );
				this.display.DrawBitmap( 6, this.line2Page, this.line2 );
				this.display.Show();

				this.RampUpDown( this.redPwm, 3000, 1 );
				this.RampUpDown( this.grePwm, 3000, 1 );
				this.RampUpDown( this.bluPwm, 3000, 1 );
			}
		}

		public void RampUpDown( IPwmPort port, int totalDurationInMs, int count ) {
			for( int iteration = 0; iteration < count; iteration++ )
				this.RampUpDown( port, totalDurationInMs );
		}

		public void RampUpDown( IPwmPort pwmPort, int totalDurationInMs ) {
			var iterations = this.BreathingBrightnessProgressionHalf.Length;
			var delay = ( totalDurationInMs / iterations ) - this.delayInMilliseconds;

			if( delay <= 0 )
				delay = 0;

			var stopwatch = Stopwatch.StartNew();

			for( int index = 0; index < iterations; index++ ) {
				pwmPort.DutyCycle = 1 - this.BreathingBrightnessProgressionHalf[ index ];

				pwmPort.Start();

				Thread.Sleep( delay );
			}

			stopwatch.Stop();
			var totalTime = ( int )stopwatch.ElapsedMilliseconds;

			//Console.WriteLine( "actual elapsed: {0}", totalTime );

			var diff = totalTime - totalDurationInMs;
			var factor = diff / iterations;

			this.delayInMilliseconds += factor;
			
			//Console.WriteLine( "delayInMilliseconds: {0}", this.delayInMilliseconds );

			pwmPort.Stop();
		}

		//private readonly float[] BreathingBrightnessProgression = {
		//	0.0781f, 0.0820f, 0.0859f, 0.0938f, 0.1016f, 0.1094f, 0.1211f, 0.1328f, 0.1484f, 0.1602f, 0.1758f, 0.1953f,
		//	0.2148f, 0.2344f, 0.2578f, 0.2852f, 0.3125f, 0.3398f, 0.3711f, 0.4023f, 0.4375f, 0.4727f, 0.5117f, 0.5508f,
		//	0.5898f, 0.6289f, 0.6719f, 0.7109f, 0.7500f, 0.7891f, 0.8242f, 0.8594f, 0.8906f, 0.9219f, 0.9453f, 0.9648f,
		//	0.9805f, /*0.9922f, 0.9922f, 0.9922f, */0.9805f, 0.9648f, 0.9453f, 0.9219f, 0.8906f, 0.8594f, 0.8242f, 0.7891f,
		//	0.7500f, 0.7109f, 0.6719f, 0.6289f, 0.5898f, 0.5508f, 0.5117f, 0.4727f, 0.4375f, 0.4023f, 0.3711f, 0.3398f,
		//	0.3125f, 0.2852f, 0.2578f, 0.2344f, 0.2148f, 0.1953f, 0.1758f, 0.1602f, 0.1484f, 0.1328f, 0.1211f, 0.1094f,
		//	0.1016f, 0.0938f, 0.0859f, 0.0820f, 0.0781f, 0.0781f, 0.0781f, 0.0781f, 0.0781f, 0.0781f, 0.0781f, 0.0781f,
		//	0.0781f, 0.0781f, 0.0781f
		//};
		private readonly float[] BreathingBrightnessProgressionHalf = {
			0.0781f, 0.0859f, 0.1016f, 0.1211f, 0.1484f, 0.1758f,
			0.2148f, 0.2578f, 0.3125f, 0.3711f, 0.4375f, 0.5117f,
			0.5898f, 0.6719f, 0.7500f, 0.8242f, 0.8906f, 0.9453f,
			0.9805f, 0.9805f, 0.9453f, 0.8906f, 0.8242f,
			0.7500f, 0.6719f, 0.5898f, 0.5117f, 0.4375f, 0.3711f,
			0.3125f, 0.2578f, 0.2148f, 0.1758f, 0.1484f, 0.1211f,
			0.1016f, 0.0859f, 0.0781f, 0.0781f, 0.0781f, 0.0781f,
			0.0781f, 0.0781f
		};
	}
}
