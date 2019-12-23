using System.Xml.Serialization;

namespace bmfont2cs.Xml {
	/// <remarks/>
	[System.Serializable()]
	[System.ComponentModel.DesignerCategory( "code" )]
	[XmlType( AnonymousType = true )]
	public partial class BmFontCommon {

		/// <remarks/>
		[XmlAttribute( AttributeName = "lineHeight" )]
		public byte LineHeight {
			get; set;
		}

		/// <remarks/>
		[XmlAttribute( AttributeName = "scaleW" )]
		public ushort ScaleW {
			get; set;
		}

		/// <remarks/>
		[XmlAttribute( AttributeName = "scaleH" )]
		public byte ScaleH {
			get; set;
		}

		/// <remarks/>
		[XmlAttribute( AttributeName = "pages" )]
		public byte Pages {
			get; set;
		}
	}
}
