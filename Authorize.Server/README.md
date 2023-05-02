# OpenIDDict Project

### Generate SSL
1. To created a SSL with openssl, execute the following command:
```
openssl req -x509 -newkey rsa:4096 -keyout key.pem -out cert.pem -sha256 -days 365
```
2. Then apply the openssl pkcs12 command to export the pfx file from the key and pem files, with following command:

```
openssl pkcs12 -export -in cert.pem -inkey key.pem -certpbe PBE-SHA1-3DES -out cert.pfx
```

3. Copy the cert.pfx file to the project root folder.
4. Repeat the same steps to create the encrypted.pfx file.
5. Update Startup.cs
```
services.AddOpenIddict().AddServer(options =>
{
    if (CurrentEnvironment.IsDevelopment())
    {
        options.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();
    }
    else
    {
        options.AddSigningCertificate(new X509Certificate2("cert.pfx", pass, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable));
        options.AddEncryptionCertificate(new X509Certificate2("encrypted.pfx", pass, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable));
    }
})
```