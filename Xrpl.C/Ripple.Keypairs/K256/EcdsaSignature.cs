using System;
using System.IO;
using Org.BouncyCastle.Math;

namespace Ripple.Keypairs.K256
{

    using Asn1InputStream = Org.BouncyCastle.Asn1.Asn1InputStream;
    using DerInteger = Org.BouncyCastle.Asn1.DerInteger;
    using DerSequenceGenerator = Org.BouncyCastle.Asn1.DerSequenceGenerator;
    using DerSequence = Org.BouncyCastle.Asn1.DerSequence;

    public class EcdsaSignature
    {
        /// <summary>
        /// The two components of the signature. </summary>
        public BigInteger R, S;

        /// <summary>
        /// Constructs a signature with the given components. </summary>
        public EcdsaSignature(BigInteger r, BigInteger s)
        {
            R = r;
            S = s;
        }

        public static bool IsStrictlyCanonical(byte[] sig) => CheckIsCanonical(sig, true);

        public static bool CheckIsCanonical(byte[] sig, bool strict)
        {
            // Make sure signature is canonical
            // To protect against signature morphing attacks

            // Signature should be:
            // <30> <len> [ <02> <lenR> <R> ] [ <02> <lenS> <S> ]
            // where
            // 6 <= len <= 70
            // 1 <= lenR <= 33
            // 1 <= lenS <= 33

            var sigLen = sig.Length;

            if (sigLen is < 8 or > 72)
            {
                return false;
            }

            if ((sig[0] != 0x30) || (sig[1] != (sigLen - 2)))
            {
                return false;
            }

            // Find R and check its length
            int rPos = 4, rLen = sig[rPos - 1];

            if (rLen is < 1 or > 33 || ((rLen + 7) > sigLen))
            {
                return false;
            }

            // Find S and check its length
            int sPos = rLen + 6, sLen = sig[sPos - 1];
            if (sLen is < 1 or > 33 || ((rLen + sLen + 6) != sigLen))
            {
                return false;
            }

            if ((sig[rPos - 2] != 0x02) || (sig[sPos - 2] != 0x02))
            {
                return false; // R or S have wrong type
            }

            if ((sig[rPos] & 0x80) != 0)
            {
                return false; // R is negative
            }

            if ((sig[rPos] == 0) && rLen == 1)
            {
                return false; // R is zero
            }

            if ((sig[rPos] == 0) && ((sig[rPos + 1] & 0x80) == 0))
            {
                return false; // R is padded
            }

            if ((sig[sPos] & 0x80) != 0)
            {
                return false; // S is negative
            }

            if ((sig[sPos] == 0) && sLen == 1)
            {
                return false; // S is zero
            }

            if ((sig[sPos] == 0) && ((sig[sPos + 1] & 0x80) == 0))
            {
                return false; // S is padded
            }

            var rBytes = new byte[rLen];
            var bytes = new byte[sLen];

            Array.Copy(sig, rPos, rBytes, 0, rLen);
            Array.Copy(sig, sPos, bytes, 0, sLen);

            BigInteger r = new(1, rBytes), s = new(1, bytes);

            var order = Secp256K1.Order();

            if (r.CompareTo(order) != -1 || s.CompareTo(order) != -1)
            {
                return false; // R or S greater than modulus
            }

            if (strict)
            {
                return order.Subtract(s).CompareTo(s) != -1;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// DER is an international standard for serializing data structures which is widely used in cryptography.
        /// It'S somewhat like protocol buffers but less convenient. This method returns a standard DER encoding
        /// of the signature, as recognized by OpenSSL and other libraries.
        /// </summary>
        public  byte[] EncodeToDer()
        {
            return DerByteStream().ToArray();
        }

        public static EcdsaSignature DecodeFromDer(byte[] bytes)
        {
            var decoder = new Asn1InputStream(bytes);
            DerInteger r, s;
            try
            {
                var seq = (DerSequence)decoder.ReadObject();
                r = (DerInteger) seq[0];
                s = (DerInteger) seq[1];
            }
            catch (InvalidCastException)
            {
                return null;
            }
            finally
            {
                decoder.Close();
            }
            // OpenSSL deviates from the DER spec by interpreting these values as unsigned, though they should not be
            // Thus, we always use the positive versions. See: http://r6.ca/blog/20111119T211504Z.html
            return new EcdsaSignature(r.PositiveValue, s.PositiveValue);
        }

        protected internal MemoryStream DerByteStream()
        {
            // Usually 70-72 bytes.
            var bos = new MemoryStream(72);
            var seq = new DerSequenceGenerator(bos);
            seq.AddObject(new DerInteger(R));
            seq.AddObject(new DerInteger(S));
            seq.Close();
            return bos;
        }
    }

}