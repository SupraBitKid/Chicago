using System.Xml.Serialization;

namespace bmfont2cs.Xml {
	/// <remarks/>
	[System.Serializable()]
	[System.ComponentModel.DesignerCategory( "code" )]
	[XmlType( AnonymousType = true )]
	public partial class BmFontInfo {

		/// <remarks/>
		[XmlAttribute( AttributeName = "face" )]
		public string Face {
			get; set;
		}

		/// <remarks/>
		[XmlAttribute( AttributeName = "size" )]
		public byte Size {
			get; set;
		}
	}
}
