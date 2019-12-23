using System;
using System.Collections.Generic;
using Meadow;
using Meadow.Devices;
using Meadow.Hardware;


namespace MeadowApplication1 {
	public class PWMMultiplex : IPwmPort {
		internal IPwmPort[] innerPorts;

		public PWMMultiplex( params IPwmPort[] innerPorts ) {
			this.innerPorts = innerPorts;
		}

		public IPwmChannelInfo Channel {
			get {
				return this.innerPorts[ 0 ].Channel;
			}
		}
		public float Duration {
			get {
				return this.innerPorts[ 0 ].Duration;
			}
			set {
				for( int index = 0; index < this.innerPorts.Length; index++ ) {
					if( this.innerPorts[ index ] != null ) {
						this.innerPorts[ index ].Duration = value;
					}
				}
			}
		}
		public float Period {
			get {
				return this.innerPorts[ 0 ].Period;
			}
			set {
				for( int index = 0; index < this.innerPorts.Length; index++ ) {
					if( this.innerPorts[ index ] != null ) {
						this.innerPorts[ index ].Period = value;
					}
				}
			}
		}
		public float DutyCycle {
			get {
				return this.innerPorts[ 0 ].DutyCycle;
			}
			set {
				for( int index = 0; index < this.innerPorts.Length; index++ ) {
					if( this.innerPorts[ index ] != null ) {
						this.innerPorts[ index ].DutyCycle = value;
					}
				}
			}
		}
		public float Frequency {
			get {
				return this.innerPorts[ 0 ].Frequency;
			}
			set {
				for( int index = 0; index < this.innerPorts.Length; index++ ) {
					if( this.innerPorts[ index ] != null ) {
						this.innerPorts[ index ].Frequency = value;
					}
				}
			}
		}
		public bool Inverted {
			get {
				return this.innerPorts[ 0 ].Inverted;
			}
			set {
				for( int index = 0; index < this.innerPorts.Length; index++ ) {
					if( this.innerPorts[ index ] != null ) {
						this.innerPorts[ index ].Inverted = value;
					}
				}
			}
		}
		public TimeScaleFactor Scale {
			get {
				return this.innerPorts[ 0 ].Scale;
			}
			set {
				for( int index = 0; index < this.innerPorts.Length; index++ ) {
					if( this.innerPorts[ index ] != null ) {
						this.innerPorts[ index ].Scale = value;
					}
				}
			}
		}
		public IPin Pin {
			get {
				return this.innerPorts[ 0 ].Pin;
			}
		}
		IDigitalChannelInfo IPort<IDigitalChannelInfo>.Channel {
			get {
				return this.innerPorts[ 0 ].Channel;
			}
		}

		public void Dispose() {
			if( this.innerPorts != null ) {
				for( int index = 0; index < this.innerPorts.Length; index++ ) {
					if( this.innerPorts[ index ] != null ) {
						this.innerPorts[ index ].Dispose();
						this.innerPorts[ index ] = null;
					}
				}
				this.innerPorts = null;
			}
		}

		public void Start() {
			for( int index = 0; index < this.innerPorts.Length; index++ ) {
				if( this.innerPorts[ index ] != null ) {
					this.innerPorts[ index ].Start();
				}
			}
		}

		public void Stop() {
			for( int index = 0; index < this.innerPorts.Length; index++ ) {
				if( this.innerPorts[ index ] != null ) {
					this.innerPorts[ index ].Stop();
				}
			}
		}
	}
}
