using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols
{
    public class SecureTcpClient : TcpClient, IDisposable
    {
        private string TargetHost;

        private SecureTcpClient(CreateReaderDelegate createReader, CreateWriterDelegate createWriter, string targetHost, SslProtocols enableSslProtocols, IEnumerable<string> explicitlyTrustedCertHashes) : base(createReader, createWriter)
        {
            this.TargetHost = targetHost;

            if (explicitlyTrustedCertHashes != null)
                ExplicitlyTrustedCertHashes.AddRange(explicitlyTrustedCertHashes);
        }

        public static SecureTcpClient Create(CreateReaderDelegate createReader, CreateWriterDelegate createWriter, string connectToHostname, int connectToPort, AddressFamily networkLayerProtocol = AddressFamily.InterNetwork, SslProtocols enableSslProtocols = SslProtocols.Default, IEnumerable<string> explicitlyTrustedCertHashes = null)
        {
            SecureTcpClient @this = new SecureTcpClient(createReader, createWriter, connectToHostname, enableSslProtocols, explicitlyTrustedCertHashes);
            Initialize(@this, connectToHostname, connectToPort, networkLayerProtocol);
            return @this;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Stream != null)
                {
                    Stream.Close();
                    Stream = null;
                }
            }

            base.Dispose(disposing);
        }

        private SslStream Stream = null;
        private SslProtocols EnableSslProtocols = SslProtocols.Default;

        protected override Stream GetStream(System.Net.Sockets.TcpClient client)
        {
            if (Stream == null)
            {
                Stream = new SslStream(base.GetStream(client), true, RemoteCertificateValidation, LocalCertificateSelection);
                Stream.AuthenticateAsClient(TargetHost, null, EnableSslProtocols, true);
            }

            return Stream;
        }

        private List<string> ExplicitlyTrustedCertHashes = new List<string>();

        private bool RemoteCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None) return true;
            else
                return ExplicitlyTrustedCertHashes.Contains(certificate.GetCertHashString(), StringComparer.InvariantCultureIgnoreCase);
        }

        private X509Certificate LocalCertificateSelection(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            return null;
        }
    }
}
