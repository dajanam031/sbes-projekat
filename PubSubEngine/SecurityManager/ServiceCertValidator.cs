using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.IdentityModel.Selectors;
using System.Security.Principal;

namespace SecurityManager
{
    // provera da li je sertifikat istekao da li je klijent sam izdavalac sertifikata
    public class ServiceCertValidator : X509CertificateValidator
    {
        public override void Validate(X509Certificate2 certificate)
        {
            /// This will take service's certificate from storage
			X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine,
                Formatter.ParseName(WindowsIdentity.GetCurrent().Name));

            if (!certificate.Issuer.Equals(srvCert.Issuer))
            {
                throw new Exception("Certificate is not from the valid issuer.");
            }

            DateTime expirationDate = DateTime.Parse(certificate.GetExpirationDateString());
            if (expirationDate < DateTime.Now)
            {
                throw new Exception($"Certificate expired on [{expirationDate}]");
            }
        }
    }
}
