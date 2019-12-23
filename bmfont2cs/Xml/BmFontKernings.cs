using System.Xml.Serialization;

namespace bmfont2cs.Xml {
	/// <remarks/>
	[System.Serializable()]
	[System.ComponentModel.DesignerCategory( "code" )]
	[XmlType( AnonymousType = true )]
	public partial class BmFontKernings {

		/// <remarks/>
		[XmlElement( "kerning" )]
		public BmFontKerningsKerning[] Kerning {
			get; set;
		}

		/// <remarks/>
		[XmlAttribute( AttributeName = "count" )]
		public byte Count {
			get; set;
		}
	}
}
