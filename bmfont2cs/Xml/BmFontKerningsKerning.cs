using System.Xml.Serialization;

namespace bmfont2cs.Xml {
	/// <remarks/>
	[System.Serializable()]
	[System.ComponentModel.DesignerCategory( "code" )]
	[XmlType( AnonymousType = true )]
	public partial class BmFontKerningsKerning {

		/// <remarks/>
		[XmlAttribute( AttributeName = "first" )]
		public ushort First {
			get; set;
		}

		/// <remarks/>
		[XmlAttribute( AttributeName = "second" )]
		public ushort Second {
			get; set;
		}

		/// <remarks/>
		[XmlAttribute( AttributeName = "amount" )]
		public sbyte Amount {
			get; set;

		}
	}
}
