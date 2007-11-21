//
// (C) 2006-2007 The SharpOS Project Team (http://sharpos.sourceforge.net)
//
// Authors:
//	William Lahti <xfurious@gmail.com>
//
// Licensed under the terms of the GNU GPL v3,
//  with Classpath Linking Exception for Libraries
//

using SharpOS.AOT;
using AOTAttr = SharpOS.AOT.Attributes;

namespace SharpOS.ADC {

	public unsafe struct Processor {
		public uint				ID;
		public ProcessorType	ArchType;
		public byte*			VendorName;
		public uint				VendorID;
		public byte*			FamilyName;
		public uint				FamilyID;
		public byte*			ModelName;
		public uint				ModelID;
		public uint				ClockSpeed;
		public uint				CacheSize;
		public void*			Flags;
	}
}
