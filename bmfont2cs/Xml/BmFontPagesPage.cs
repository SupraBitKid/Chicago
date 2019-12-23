using System.Xml.Serialization;

namespace bmfont2cs.Xml {
	/// <remarks/>
	[System.Serializable()]
	[System.ComponentModel.DesignerCategory( "code" )]
	[XmlType( AnonymousType = true )]
	public partial class BmFontPagesPage {

		/// <remarks/>
		[XmlAttribute( AttributeName = "id" )]
		public byte Id {
			get; set;
		}

		/// <remarks/>
		[XmlAttribute( AttributeName = "file" )]
		public string File {
			get; set;
		}
	}
}
