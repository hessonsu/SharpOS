//
// (C) 2006-2007 The SharpOS Project Team (http://www.sharpos.org)
//
// Authors:
//	Mircea-Cristian Racasan <darx_kies@gmx.net>
//	Bruce Markham <illuminus86@gmail.com>
//
// Licensed under the terms of the GNU GPL v3,
//  with Classpath Linking Exception for Libraries
//

namespace SharpOS.Kernel.Tests.CS
{
	public class String
	{
		private const string TEST_STRING = "TST\u2665";

		public static int GetLength (string value)
		{
			return value.Length;
		}

		public static int CMPGetLength ()
		{
			string value = TEST_STRING;

			if (GetLength (value) != 4)
				return 0;

			return 1;
		}

		public static int CMPGetChars ()
		{
			if (TEST_STRING[0] != 'T')
				return 0;

			if (TEST_STRING[1] != 'S')
				return 0;

			if (TEST_STRING[2] != 'T')
				return 0;

			if (TEST_STRING[3] != '\u2665')
				return 0;

			return 1;
		}

		public static int CMPGetChars2 ()
		{
			int i = 0;

			foreach (char value in TEST_STRING) {
				if (value != TEST_STRING[i++])
					return 0;
			}

			i = 0;
			// Mono uses System.CharEnumerator internally
			System.CharEnumerator OperandEnum = TEST_STRING.GetEnumerator () as System.CharEnumerator;
			while (OperandEnum.MoveNext ()) {
				if ((char)OperandEnum.Current != TEST_STRING[i++])
					return 0;
			}

			return 1;
		}

		public static int CMPBumperIndexing1 ()
		{
			//Unreachable code detected
#pragma warning disable 0219

			//Test that constant String is working properly
			const string str1 = "US";
			const string str2 = "SK";

			if ((byte)str1[0] == (byte)'U')
				return 1;
			else
				return 0;
#pragma warning restore 0219
		}
		public static int CMPBumperIndexing2 ()
		{
			//Unreachable code detected
#pragma warning disable 0219
			//int i = 0;

			//Test that constant String is working properly
			const string str1 = "US";
			const string str2 = "SK";

			if ((byte)str1[1] == (byte)'S')
				return 1;
			else
				return 0;
#pragma warning restore 0219
		}
		public static int CMPBumperLength ()
		{
			//int i = 0;

			//Test that constant String is working properly
			const string str1 = "US";
			const string str2 = "SK";

			if (str1.Length != 2)
				return 0;

			if (str2.Length != 2)
				return 0;
			else
				return 1;
		}


		[SharpOS.AOT.Attributes.String]
		public unsafe static byte* CString (string str)
		{
			return null;
		}

		public unsafe static int CMPCStringStub1 ()
		{
			byte* str = CString ("Hello");
			int success = 0;

			if (str[0] == 'H' && str[1] == 'e' && str[2] == 'l' &&
				str[3] == 'l' && str[4] == 'o')
				success = 1;

			return success;
		}

		// This can't work as the AOT expects CString (string str) to work only with constants 
		// it can't work with anything else but constants
		public static int CMPCStringPass1 ()
		{
			return CStringPasser ("Hello");
		}

		public unsafe static int CStringPasser (string rstr)
		{
			int success = 0;
			/*
			byte *str = CString (rstr);
			if (str[0] == 'H' && str[1] == 'e' && str[2] == 'l' &&
			    str[3] == 'l' && str[4] == 'o')
				success = 1;
			*/
			return success;
		}

		public static int CMPConstIndexing3 ()
		{
			//int i = 0;

			//Test that constant String is working properly
			const string str1 = "US";
			const string str2 = "SK";

			if ((byte)str1[1] == (byte)str2[0])
				return 1;
			else
				return 0;
		}

		public static int CMPConcat ()
		{
			string str1 = "US";
			string str2 = "SK";

			string res = str1 + str2;
			if (res == "USSK")
				return 1;
			else
				return 0;
		}

		public static int CMPSubString ()
		{
			string str1 = "ApplesOranges";
			string res = str1.Substring (6);

			if (res == "Oranges")
				return 1;
			else
				return 0;
		}

		public static int CMPSubString2 ()
		{
			string str1 = "ApplesOranges";
			string res = str1.Substring (6, 2);

			if (res == "Or")
				return 1;
			else
				return 0;
		}

		public static int CMPIndexOf ()
		{
			string str1 = "ApplesOranges";

			if (str1.IndexOf('A') != 0)
				return 0;

			if (str1.IndexOf ('s') != 5)
				return 0;

			if (str1.IndexOf ('X') != -1)
				return 0;

			return 1;
		}

		public static int CMPLastIndexOf ()
		{
			string str1 = "ApplesOranges";						   

			if (str1.LastIndexOf ('A') != 0)
				return 0;

			if (str1.LastIndexOf ('s') != 12)
				return 0;

			if (str1.LastIndexOf ('X') != -1)
				return 0;

			return 1;
		}
	}
}
