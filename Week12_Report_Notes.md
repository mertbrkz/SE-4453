# Week 12 - Project Report Notes

## 1. Docker Yapılandırması
App Service standartlarına uygun ve SSH destekli Docker yapılandırması.

### Dockerfile
`Config5/MertApi/Dockerfile` dosyasında:
- **Base Image**: `mcr.microsoft.com/dotnet/aspnet:8.0` kullanıldı.
- **SSH Kurulumu**: `openssh-server` kuruldu ve root şifresi `Docker!` olarak ayarlandı.
- **Portlar**: Uygulama için `8080`, SSH için `2222` portları dışarı açıldı (EXPOSE).
- **Config**: `sshd_config` dosyası kopyalanarak Azure App Service uyumlu SSH ayarları yapıldı.

### Entrypoint Script
`Config5/MertApi/entrypoint.sh` scripti:
- Hem SSH servisini (`service ssh start`) hem de .NET uygulamasını (`dotnet MertApi.dll`) aynı anda başlatır.

## 2. Azure CLI Komutları
Kullanılan komutların sırası ve açıklamaları:

1. **Azure Container Registry (ACR) Oluşturma**:
   ```bash
   az acr create --name mertbrkzacr --resource-group MertConfiguration5_RG --sku Basic --admin-enabled true
   ```
   *Amacı: Docker imajlarını saklamak için özel bir kayıt defteri oluşturur.*

2. **User Assigned Identity Oluşturma**:
   ```bash
   az identity create --name mertbrkz-id --resource-group MertConfiguration5_RG
   ```
   *Amacı: App Service'in ACR'ye güvenli erişimi için yönetilen bir kimlik oluşturur.*

3. **Role Assignment (Yetkilendirme)**:
   ```bash
   az role assignment create --assignee <Identity-Principal-ID> --scope <ACR-Resource-ID> --role AcrPull
   ```
   *Amacı: Oluşturulan kimliğe, ACR'den sadece imaj çekme (pull) yetkisi verir.*

4. **App Service'e Kimlik Atama ve Konfigürasyon**:
   ```bash
   az webapp identity assign --name mertbrkz-config5-app --resource-group MertConfiguration5_RG --identities <Identity-ID>
   az webapp config container set --name mertbrkz-config5-app --resource-group MertConfiguration5_RG --docker-custom-image-name "mertbrkzacr.azurecr.io/mertapi:latest" --docker-registry-server-url "https://mertbrkzacr.azurecr.io" --assign-identity <Identity-ID>
   ```
   *Amacı: App Service'i Docker moduna geçirir ve imajları çekmek için oluşturduğumuz kimliği kullanmasını söyler.*

## 3. GitHub Actions Workflow
`.github/workflows/deploy.yml` dosyası:
- **Tetikleyici**: `feature/docker-integration` veya `main` branch'ine push yapıldığında çalışır.
- **Adımlar**:
  1. **Login via Azure CLI**: Servis Principal kullanarak Azure'a giriş yapar.
  2. **Build and Push**: Docker imajını oluşturur ve ACR'ye `latest` ve `commit-sha` etiketleriyle yükler.
  3. **Deploy to Azure Web App**: Güncel imajı App Service'e deploy eder.

---
**Not:** Bu dosya proje klasörünüzde mevcuttur. Raporunuza kopyalayabilirsiniz.
