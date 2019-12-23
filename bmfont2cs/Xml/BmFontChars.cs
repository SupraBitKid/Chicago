using System.Xml.Serialization;

namespace bmfont2cs.Xml {
	/// <remarks/>
	[System.Serializable()]
	[System.ComponentModel.DesignerCategory( "code" )]
	[XmlType( AnonymousType = true )]
	public partial class BmFontChars {

		/// <remarks/>
		[XmlElement( "char" )]
		public BmFontCharsChar[] Characters {
			get; set;
		}

		/// <remarks/>
		[XmlAttribute( AttributeName = "count" )]
		public byte Count {
			get; set;
		}
	}
}
