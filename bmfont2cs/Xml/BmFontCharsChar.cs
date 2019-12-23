using System.Xml.Serialization;

namespace bmfont2cs.Xml {
	/// <remarks/>
	[System.Serializable()]
	[System.ComponentModel.DesignerCategory( "code" )]
	[XmlType( AnonymousType = true )]
	public partial class BmFontCharsChar {

		/// <remarks/>
		[XmlAttribute( AttributeName = "id" )]
		public ushort Id {
			get; set;
		}

		/// <remarks/>
		[XmlAttribute( AttributeName = "x" )]
		public byte X {
			get; set;
		}

		/// <remarks/>
		[XmlAttribute( AttributeName = "y" )]
		public byte Y {
			get; set;
		}

		/// <remarks/>
		[XmlAttribute( AttributeName = "width" )]
		public byte Width {
			get; set;
		}

		/// <remarks/>
		[XmlAttribute( AttributeName = "height" )]
		public byte Height {
			get; set;
		}

		/// <remarks/>
		[XmlAttribute( AttributeName = "xoffset" )]
		public sbyte XOffset {
			get; set;
		}

		/// <remarks/>
		[XmlAttribute( AttributeName = "yoffset" )]
		public byte YOffset {
			get; set;
		}

		/// <remarks/>
		[XmlAttribute( AttributeName = "xadvance" )]
		public byte XAdvance {
			get; set;
		}
	}
}
