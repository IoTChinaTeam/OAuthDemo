using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace OAuthDemo
{
    public class IssuerSigningKeyResolver
    {
        private static Dictionary<string, string> plainCertificates = new Dictionary<string, string>
        {
            {
                // Global AAD
                @"z039zdsFuizpBfBVK1Tn25QHYO0",
                @"MIIDBTCCAe2gAwIBAgIQXxLnqm1cOoVGe62j7W7wZzANBgkqhkiG9w0BAQsFADAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MB4XDTE3MDMyNjAwMDAwMFoXDTE5MDMyNzAwMDAwMFowLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAKJGarCm4IF0/Gz5Xx/zyZwD2rdJJZtO2Ukk1Oz+Br1sLVY8I5vj5esB+lotmLEblA9N/w188vmTvykaEzUl49NA4s86x44SW6WtdQbGJ0IjpQJUalUMyy91vIBkK/7K3nBXeVBsRk7tm528leoQ05/aZ+1ycJBIU+1oGYThv8MOjyHAlXJmCaGXwXTisZ+hHjcwlMk/+ZEutHflKLIpPUNEi7j4Xw+zp9UKo5pzWIr/iJ4HjvCkFofW90AMF2xp8dMhpbVcfJGS/Ii3J66LuNLCH/HtSZ42FO+tnRL/nNzzFWUhGT92Q5VFVngfWJ3PAg1zz8I1wowLD2fiB2udGXcCAwEAAaMhMB8wHQYDVR0OBBYEFFXPbFXjmMR3BluF+2MeSXd1NQ3rMA0GCSqGSIb3DQEBCwUAA4IBAQAsd3wGVilJxDtbY1K2oAsWLdNJgmCaYdrtdlAsjGlarSQSzBH0Ybf78fcPX//DYaLXlvaEGKVKp0jPq+RnJ17oP/RJpJTwVXPGRIlZopLIgnKpWlS/PS0uKAdNvLmz1zbGSILdcF+Qf41OozD4QNsS1c9YbDO4vpC9v8x3PVjfJvJwPonzNoOsLXA+8IONSXwCApsnmrwepKu8sifsFYSwgrwxRPGTEAjkdzRJ0yMqiY/VoJ7lqJ/FBJqqAjGPGq/yI9rVoG+mbO1amrIDWHHTKgfbKk0bXGtVUbsayyHR5jSgadmkLBh5AaN/HcgDK/jINrnpiQ+/2ewH/8qLaQ3B"
            },
            {
                // Google
                @"e5739df2261c8a0ed41715e7f62cc295ddc86c16",
                @"MIIDJjCCAg6gAwIBAgIILkwzoarQmuwwDQYJKoZIhvcNAQEFBQAwNjE0MDIGA1UEAxMrZmVkZXJhdGVkLXNpZ25vbi5zeXN0ZW0uZ3NlcnZpY2VhY2NvdW50LmNvbTAeFw0xNzA1MDMxMTQzMzRaFw0xNzA1MDYxMjEzMzRaMDYxNDAyBgNVBAMTK2ZlZGVyYXRlZC1zaWdub24uc3lzdGVtLmdzZXJ2aWNlYWNjb3VudC5jb20wggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDMbZXpR1PeFrMWING/67hd35b37uw8YQl8GETiK3t+dXJc0JB75rjW7fUjkMqpQNuna7WvONFWhLwNyj+WFUAuQkzsxUqzwdySJIEE+rQVhhapGJBLetCUX0FzBqzls6bkLYfW6vy9F0s8b8CxnIYafsLHWbiQFoa80jj37sjyunfPk7aC53M1wO8crigS0RDEKmsF5Yrnfw7NnyXxX+XDDTJtAhtcIx4eo/XotsJ82v6FsO5FU8T+SFIvSMj90m3kZuREpBtIxf6z/OpQUwpBhBSGXjCJaqqrWZndZmSGTDECy7wgD/hJcTofRHxoT4kZYCEhDUlD68wDDG6fiGrnAgMBAAGjODA2MAwGA1UdEwEB/wQCMAAwDgYDVR0PAQH/BAQDAgeAMBYGA1UdJQEB/wQMMAoGCCsGAQUFBwMCMA0GCSqGSIb3DQEBBQUAA4IBAQAEkU2/PQZbjS/VWryR7mgTVdZvOPXOwJ/D9mJoJYyeSI5D9smMYn4uHpVqQ04ESIORV58795AZX4FSVROiqL/BM6GSGgHmBqFl9a2MXlYcqA1cIxCSpycanmF5fueUqMwryV5Jzft/ecuL2/zmVfJ3f2VJamcUX9jAcn3s+CVdahq6jq5tStKMDtE+YILrP2tJRL4GrfCPjQm00iCdt69TyKroB4Qqvd+ihwxc5yZ+wbyO4XTTPwj+cVO4Ox2+MpiI/MysuKuujPqVgk2I5FQFTXx8EKqXioWyfxf3y+gceTBzy61sMyQGWY3SqqIa6IAVVdLwgE2SRrjpcUfnArwg"
            },
            {
                // Google
                @"8d1dd97f23b01811392945dfb3da1393428a7b31",
                @"MIIDJjCCAg6gAwIBAgIIWc6yt7s1wmkwDQYJKoZIhvcNAQEFBQAwNjE0MDIGA1UEAxMrZmVkZXJhdGVkLXNpZ25vbi5zeXN0ZW0uZ3NlcnZpY2VhY2NvdW50LmNvbTAeFw0xNzA1MTAxMTQzMzRaFw0xNzA1MTMxMjEzMzRaMDYxNDAyBgNVBAMTK2ZlZGVyYXRlZC1zaWdub24uc3lzdGVtLmdzZXJ2aWNlYWNjb3VudC5jb20wggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCsft0K3uWVO0FdLBfT7fmMSbZy6NOx7Vj/tcjEw3Cr22I7O39f2TZoW8t6JLnTKn1r+T7f7ch9vJ66rStBeXJ1jWACEOm2vh7pEmKy3Crpr/gqOoINVnxbn6gc7AeoVzjp+rQDOep4AKeMWBOkSA7s5MfNF/8C0eo4w8uvynlXs3o2v7Igqb8C3JDDsUuKWjhoN7t5ILdaKpnd/5bNf8yxV2oa8jVGF4QeFH1DdeONGYQ7+eQ/8hM6NaT3YPXLbVu7KSnCsG7q8RvVzViXIgw3uaMwsjoYPf0A5YJG5pOYNJQmLV4uIPZkki2pO2fR4cBtTmboyZoSO3gv3bJ2Zk47AgMBAAGjODA2MAwGA1UdEwEB/wQCMAAwDgYDVR0PAQH/BAQDAgeAMBYGA1UdJQEB/wQMMAoGCCsGAQUFBwMCMA0GCSqGSIb3DQEBBQUAA4IBAQBpoSVlXm50fFup3m+sGhpJfsiOQomR/nZspua2vC4M27P8Tp8tI9anJonyp81Y2i1pA+bldyFmlgoRVBOSCvCxQa9YYkcNnS/hNPj+Z4IEmGUWNBM1LAbJpyD3DqvXbaFpqmn1LO5oJ4zEIz79wkheohzilgARnPs9Ax9Z1pyWjViwObkOhhTXHrfxBpRdaNNdu2habTcONmRY6kRFT3JtZ+mwnBNarw7DsEN+6B04seJH1yeFaC5rTeWM7aQ4EdLhwPCeSUlYEI242ebbAd+QqFlegVLPT0VVOsTKJ3B3h6S7daCfqjYs51nmc4bUkVgrKHdskNyTqnPfRnK5I9OH"
            }
        };

        class JWK
        {
            public string N { get; set; }
            public string E { get; set; }
        }

        private static Dictionary<string, JWK> jwks = new Dictionary<string, JWK>
        {
            {
                // Google
                @"7e3d8087655edd15c2e427b806ed91354ddde8d5",
                new JWK
                {
                    N = @"r8uc9nSjwAKPfREpFaYvFtKXciA9Oa6Q4m01BGt6nshCWBb752J6ec13ne939ML_njv5B2Bd2kPgFj0ams9_79TxHpto6Za_Hfp-qg9mr5YGVWFug2hiT8nc5DFVaMwDcWdonJjODANBBqYa6GOmsohQsWIVjsfsJ5jBWKALykUjb3MhWEVeGzFBbw6sqdIe7Q_mVm5hm2qbO-eKKX7AGgHKytc7LQKpVwcRSr8PqL2kXnrKuf1GEzyaWsOKmRNaRfCMA4pQeC9J-wqLP85glnCnUMOXZ-Zy1IdfgxWB6Rad9iUXkA_9Ejm8Qp1Ydh0q_WRj_TsGTy2e9TdFmdsLKQ==",
                    E = "AQAB"
                }
            },
            {
                @"347909f8a798196057ac66dfba5c26cc7757184d",
                new JWK
                {
                    N = @"k9nYHNLTq99qapEXET9yKZ710GSPNbqdVqGLasSk56K4D8DEbarBUDLUT077fS9h2GszKARqN4njPsgPqAn5p4szCGMCT2Pi5oqjlhcYOMLtQabMwq4c12sQWowk4ThBYgOSIXAG1i9mVuUf0knuQ28ko9OadALaSm8IrPlcLdF2TMnR3zAeiVKOzQmJGD5veoYtFLZthWUZ2N773Yo0Bx0mIsGHM9Tn0Dko9Z02ygcmROF4HqtxhOtbbZHsmtAhpp4ED3c2R-eXsoHwMboj8A_yVSWeuoGrUTYW_01PMllUf9xVuGbgI7UbkyeMBn6KEiAUMRJ1hsbR4IZ9rRLx4Q==",
                    E = @"AQAB"
                }
            },
            {
                @"4f71eaa1927ae6b1faf4521930bc0d4544e141fb",
                new JWK
                {
                    N = @"1WOYoHRxv3VloUBI9QhIUBXve35OiyH-hgCmCHq4pljj5uOnK-wZsE53SjarvHYHpoVDUldy-pn-Ec13nq1AqCzwgbYBKSEh6RVmznq5U3m2_Zo3ETjODGhMCTV1KDPGFuOsumrcxr4pzo0AWLztgTCjSJXwalK8w0oNkF55HGLTq8LJ9RAPrSj4X9_hHKQgQ4Z0s4hHy2ZSKLJTFCI8DZBERYk6NcuZQ2Wsez6a9juP8a7_AZ8I3zCNQs6mcSYw2DsPDj6D3du66--omNiKsuRqQaj739v8waPR71ahNYEihpEi5iKGOuG1RZMkOyPhBSqjaBupojy7VagKnKNhYQ==",
                    E = @"AQAB"
                   }
            },
            {
                @"7ad98e707981a439b84d76208ae50cd5520f1c6b",
                new JWK
                {
                    N = @"qGvhlibLIjxuSHqjsr2YUa7TwDRD58Z_IhnwCWMQkY9ycr3S6_XiqxykLuiOSf67UP9awDudml_ixLHjc3WWbJ-tWJ05Dh8cAEO1Gz7Gcznq2p2wZDvejWjXvQa0sgAYASL00mtMcBiW3V9oEJICm2DbdBmB0ZqJnDh1YZiNf8tBbGYHQWQrwtAp2GTpTpAerD3L4f2S1TDHClQxO1SPYdI2OHiGgXn0TLSJEuDgt01G5Eb4KLGKNtwJuaQ84c_ZhDftVQPvkglbbLB0LI0ch5ffjaeX_s_LvZtoG4IwlMtI8X_0-yk8GWDPLNSIfwu5cou3bXtMeIfXIvAfeLZjpw==",
                    E = @"AQAB"
                }
            }
        };

        private static Dictionary<string, SecurityKey> keyStore = new Dictionary<string, SecurityKey>(StringComparer.OrdinalIgnoreCase);

        static IssuerSigningKeyResolver()
        {
            foreach (var pair in plainCertificates)
            {
                keyStore.Add(
                    pair.Key,
                    new X509SecurityKey(
                        new X509Certificate2(
                            Convert.FromBase64String(pair.Value))));
            }

            foreach (var pair in jwks)
            {
                var rsa = RSA.Create();
                rsa.ImportParameters(new RSAParameters
                {
                    Exponent = Base64UrlEncoder.DecodeBytes(pair.Value.E),
                    Modulus = Base64UrlEncoder.DecodeBytes(pair.Value.N),
                });

                keyStore.Add(
                    pair.Key,
                    new RsaSecurityKey(rsa));
            }
        }

        public static SecurityKey Resolve(string token, SecurityToken securityToken, SecurityKeyIdentifier keyIdentifier, TokenValidationParameters validationParameters)
        {
            try
            {
                var kid = keyIdentifier.Find<NamedKeySecurityKeyIdentifierClause>().Id;

                SecurityKey key;
                if (keyStore.TryGetValue(kid, out key))
                {
                    Trace.TraceInformation($"Resolved issuer signing key for {keyIdentifier.ToLongString()}");
                    return key;
                }
                else
                {
                    Trace.TraceWarning($"Failed to resolve issuer signing key for {keyIdentifier.ToLongString()}");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Exception raised while resolving issuer signing key for {keyIdentifier.ToLongString()}: {ex}");
            }

            return null;
        }
    }
}