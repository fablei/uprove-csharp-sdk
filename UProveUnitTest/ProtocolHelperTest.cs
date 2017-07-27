﻿//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License
//    Version 2.0.
//
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using Microsoft.VisualStudio.TestTools.UnitTesting;
using UProveCrypto;
using UProveCrypto.Math;

namespace UProveUnitTest
{
    [TestClass]
    public class ProtocolHelperTest
    {

        /// <summary>
        ///A test for ToBigInteger
        ///</summary>
        [TestMethod()]
        public void EncodingTest()
        {
            // a large value
            byte[] modulus = new byte[] {
0xef, 0x09, 0x90, 0x06, 0x1d, 0xb6, 0x7a, 0x9e,
0xae, 0xba, 0x26, 0x5f, 0x1b, 0x8f, 0xa1, 0x2b,
0x55, 0x33, 0x90, 0xa8, 0x17, 0x5b, 0xcb, 0x3d,
0x0c, 0x2e, 0x5e, 0xe5, 0xdf, 0xb8, 0x26, 0xe2,
0x29, 0xad, 0x37, 0x43, 0x11, 0x48, 0xce, 0x31,
0xf8, 0xb0, 0xe5, 0x31, 0x77, 0x7f, 0x19, 0xc1,
0xe3, 0x81, 0xc6, 0x23, 0xe6, 0x00, 0xbf, 0xf7,
0xc5, 0x5a, 0x23, 0xa8, 0xe6, 0x49, 0xcc, 0xbc,
0xf8, 0x33, 0xf2, 0xdb, 0xa9, 0x9e, 0x6a, 0xd6,
0x6e, 0x52, 0x37, 0x8e, 0x92, 0xf7, 0x49, 0x2b,
0x24, 0xff, 0x8c, 0x1e, 0x6f, 0xb1, 0x89, 0xfa,
0x84, 0x34, 0xf5, 0x40, 0x2f, 0xe4, 0x15, 0x24,
0x9a, 0xe0, 0x2b, 0xf9, 0x2b, 0x3e, 0xd8, 0xea,
0xaa, 0xa2, 0x20, 0x2e, 0xc3, 0x41, 0x7b, 0x20,
0x79, 0xda, 0x4f, 0x35, 0xe9, 0x85, 0xbb, 0x42,
0xa4, 0x21, 0xcf, 0xab, 0xa8, 0x16, 0x0b, 0x66,
0x94, 0x99, 0x83, 0x38, 0x4e, 0x56, 0x36, 0x5a,
0x44, 0x86, 0xc0, 0x46, 0x22, 0x9f, 0xc8, 0xc8,
0x18, 0xf9, 0x30, 0xb8, 0x0a, 0x60, 0xd6, 0xc2,
0xc2, 0xe2, 0x0c, 0x5d, 0xf8, 0x80, 0x53, 0x4d,
0x42, 0x40, 0xd0, 0xd8, 0x1e, 0x9a, 0x37, 0x0e,
0xef, 0x67, 0x6a, 0x1c, 0x3b, 0x0e, 0xd1, 0xd8,
0xff, 0x30, 0x34, 0x0a, 0x96, 0xb2, 0x1b, 0x89,
0xf6, 0x9c, 0x54, 0xce, 0xb8, 0xf3, 0xdf, 0x17,
0xe3, 0x1b, 0xc2, 0x0c, 0x5b, 0x60, 0x1e, 0x99,
0x44, 0x45, 0xa1, 0xd3, 0x47, 0xa4, 0x5d, 0x95,
0xf4, 0x1a, 0xe0, 0x71, 0x76, 0xc7, 0x38, 0x0c,
0x60, 0xdb, 0x2a, 0xce, 0xdd, 0xee, 0xda, 0x5c,
0x59, 0x80, 0x96, 0x43, 0x62, 0xe3, 0xa8, 0xdd,
0x3f, 0x97, 0x3d, 0x6d, 0x4b, 0x24, 0x1b, 0xcf,
0x91, 0x0c, 0x7f, 0x7a, 0x02, 0xed, 0x3b, 0x60,
0x38, 0x3a, 0x01, 0x02, 0xd8, 0x06, 0x0c, 0x27};

            FieldZq field = FieldZq.CreateFieldZq(modulus);
            for (int i=0; i<20; i++)
            {
                FieldZqElement r = field.GetRandomElement(false);
                FieldZqElement r2 = field.GetElement(r.ToByteArray());
                Assert.AreEqual<FieldZqElement>(r, r2);
            }
        }

        /// <summary>
        ///A test for VerifyIssuerParameters
        ///</summary>
        [TestMethod()]
        public void VerifyIssuerParametersTest()
        {
            IssuerSetupParameters isp = new IssuerSetupParameters();
            isp.UidP = new byte[] { 1, 2, 3, 4, 5 };
            isp.E = IssuerSetupParameters.GetDefaultEValues(7);
            isp.UseRecommendedParameterSet = false;
            isp.GroupConstruction = GroupType.Subgroup;
            IssuerKeyAndParameters ikap = isp.Generate();
            IssuerParameters ip = ikap.IssuerParameters;
            ProtocolHelper.VerifyIssuerParameters(ip, false);
            byte[] g0Bytes = ip.G[0].GetEncoded();
            g0Bytes[g0Bytes.Length - 1]++;
            ip.G[0] = (SubgroupGroupElement)ip.Gq.CreateGroupElement(g0Bytes);
            try { ProtocolHelper.VerifyIssuerParameters(ip, false); Assert.Fail(); } catch (InvalidUProveArtifactException) { }
        }

    }
}