##SSL Certificate to Azure##

I created an SSL with openssl

openssl req -x509 -newkey rsa:4096 -keyout key.pem -out cert.pem -sha256 -days 365

Then I applied the openssl pkcs12 command to export the pfx file from the key and pem files.

openssl pkcs12 -export -in cert.pem -inkey key.pem -certpbe PBE-SHA1-3DES -out cert.pfx

I keep the resulting pfx file in the project.

When I defined the certificate on the OpenId Server side as follows, the Client Api was able to send the auth code and receive the token.

options.AddSigningCertificate(new X509Certificate2("cert.pfx", pass, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable));

options.AddEncryptionCertificate(new X509Certificate2("cert.pfx", pass, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable));