using System;
using System.Reflection;

namespace Meadow.Hardware {
	public static class I2cBusExtensions {
		public static void SetFrequency( this I2cBus device, uint frequency ) {

			var fields = typeof( I2cBus ).GetFields( System.Reflection.BindingFlags.Instance
												   | System.Reflection.BindingFlags.GetField
												   | System.Reflection.BindingFlags.SetField
												   | System.Reflection.BindingFlags.NonPublic );

			for( int index = 0; index < fields.Length; index++ )
				if( fields[ index ].Name.Contains( "Frequency" ) ) {
					fields[ index ].SetValue( device, frequency );
					return;
				}
		}
	}
}
