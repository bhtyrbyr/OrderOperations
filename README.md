# OrderOperations

OrderOperations, e-ticaret sipariş yönetimi için geliştirilmiş modern bir .NET 8.0 tabanlı web api uygulamasıdır. Clean Architecture prensiplerine uygun olarak tasarlanmış ve CQRS pattern'i ile MediatR kullanılarak geliştirilmiştir.

Proje, kullanıcıların ürünlerden oluşturmuş olduğu sepeti background service ve queue yapısı ile alıp siparişe çevirmektedir. Log kayıtlarında takibi kolay olabilmesi amacıyla outbox pattern yapısı 30s'lik periyoda ayarlanmış olup, asıl sipariş oluşturma işlemini yapacak olan worker ise kuyruktan mesaj gelmesi durumunda çalışmaktadır.
```
Soru:

Geliştirilmesi istenen sistemde sorun teşkil edecek durumlar var mıdır ? var ise nelerdir ? sistem tasarımı doğrumu ? yanlış ise olması gereken sistem tasarımı nedir ? 

```

* Siparişin direkt backgroundservice'e ve kuyruğa gönderilmesi:
    * DB’ye kaydedilmeden kuyruğa gönderilen sipariş kaybolabilir.
    * Kuyruk yapısının çökmesi durumunda veri kaybı yaşanır.
    * Çözüm olarak outbox pattern yapısı kuruldu. Sipariş talebi geldiğinde veritabanında boş sipariş çatısı oluşturuldu.

* Transactional tutarsızlık :
    * Sipariş DB’ye kaydedilir ama kuyruğa gönderilemezse tutarsızlık oluşur.
    * Çözüm olarak siparişin kuyruğa gönderimesinden sorumlu ayrı bir background service oluşuturuldu. Bu sayede yarım dakikalık periyotlarla işlenmemiş sipariş talebili kalmasının önüne geçildi.

* Idempotency eksikliği:
    * Mesajlar tekrar tüketildiğinde aynı sipariş birden fazla işlenebilir.
    * Çözüm olarak IdempotencyKey tutularak aynı key ile tekrar gönderilen isteklerin işlenmemesi sağlandı.

## 🏗️ Proje Yapısı

### Clean Architecture Katmanları

```
OrderOperations/
├── Src/
│   ├── Domain/                    # Domain Layer
│   │   └── OrderPoerations.Domain/
│   │       ├── Entities/          # Domain Entities
│   │       ├── Enums/            # Domain Enums
│   │       └── Common/           # Base Classes
│   │
│   ├── Application/              # Application Layer
│   │   └── OrderOperations.Application/
│   │       ├── Features/         # CQRS Commands & Queries
│   │       ├── DTOs/            # Data Transfer Objects
│   │       ├── Interfaces/      # Application Interfaces
│   │       ├── Profiles/        # AutoMapper Profiles
│   │       └── Validator/       # FluentValidation Rules
│   │
│   ├── Infrastructure/           # Infrastructure Layer
│   │   ├── OrderOperations.Persistence/    # Data Access
│   │   ├── OrderOperations.Security/       # JWT Authentication
│   │   ├── OrderOperations.Queue/          # RabbitMQ Integration
│   │   ├── OrderOperations.Services/       # Background Services
│   │   └── OrderOperations.SignalR/        # Real-time Notifications
│   │
│   ├── WebApi/                   # Presentation Layer
│   │   └── OrderOperations.WebApi/
│   │       ├── Controllers/      # API Controllers
│   │       ├── Hubs/            # SignalR Hubs
│   │       ├── Middlewares/     # Custom Middlewares
│   │       └── Services/        # Web Services
│   │
│   └── Shared/                   # Shared Libraries
│       ├── OrderOperations.Contracts/      # Message Contracts
│       └── OrderOperations.CustomExceptions/ # Custom Exceptions
│
└── Test/                         # Test Projects
    ├── OrderOperations.UnitTests/         # Unit Tests
    └── OrderOperations.IntegrationTests/  # Integration Tests
```

## 🚀 Özellikler

### Core Features
- **Sipariş Yönetimi**: Sipariş oluşturma, güncelleme ve takip
- **Sepet İşlemleri**: Ürün ekleme/çıkarma, sepet yönetimi
- **Ürün Kataloğu**: Kategori bazlı ürün yönetimi
- **Stok Takibi**: Gerçek zamanlı stok kontrolü
- **Kullanıcı Yönetimi**: JWT tabanlı kimlik doğrulama

### Technical Features
- **Clean Architecture**: Katmanlı mimari yapısı
- **CQRS Pattern**: Command Query Responsibility Segregation
- **MediatR**: Mediator pattern implementasyonu
- **AutoMapper**: Object mapping
- **FluentValidation**: Input validation
- **Entity Framework Core**: ORM ile PostgreSQL entegrasyonu
- **RabbitMQ**: Message queuing
- **SignalR**: Real-time bildirimler
- **Docker Support**: Containerization
- **Swagger/OpenAPI**: API dokümantasyonu

## 🛠️ Teknoloji Stack

### Backend
- **.NET 8.0** - Framework
- **ASP.NET Core** - Web API
- **Entity Framework Core** - ORM
- **PostgreSQL** - Database
- **RabbitMQ** - Message Broker
- **SignalR** - Real-time Communication

### Libraries & Packages
- **MediatR** - CQRS Implementation
- **AutoMapper** - Object Mapping
- **FluentValidation** - Validation
- **JWT Bearer** - Authentication
- **Serilog** - Logging
- **Polly** - Resilience Patterns
- **Dapper** - Micro ORM (Optional)

### Testing
- **xUnit** - Testing Framework
- **Moq** - Mocking
- **FluentAssertions** - Assertions
- **Microsoft.AspNetCore.Mvc.Testing** - Integration Testing

## 🐳 Docker Support

Proje Docker ile containerize edilmiştir:

```bash
# Development Environment
docker-compose -f docker-compose.dev.yml up -d

# Production Environment  
docker-compose -f docker-compose.prod.yml up -d
```

### Services
- **API**: Port 8081
- **PostgreSQL**: Port 5433
- **RabbitMQ Management**: Port 15673

## 📋 API Endpoints

### Authentication
- `POST /api/auth/register` - Kullanıcı kaydı
- `POST /api/auth/login` - Kullanıcı girişi

### Products
- `GET /api/products` - Ürün listesi
- `GET /api/products/{id}` - Ürün detayı
- `POST /api/products` - Ürün oluşturma
- `PUT /api/products/{id}` - Ürün güncelleme
- `DELETE /api/products/{id}` - Ürün silme

### Categories
- `GET /api/categories` - Kategori listesi
- `POST /api/categories` - Kategori oluşturma

### Basket
- `GET /api/basket` - Sepet içeriği
- `POST /api/basket/add` - Sepete ürün ekleme
- `DELETE /api/basket/remove/{productId}` - Sepetten ürün çıkarma

### Orders
- `GET /api/orders` - Sipariş listesi
- `POST /api/orders` - Sipariş oluşturma
- `PUT /api/orders/{id}/status` - Sipariş durumu güncelleme

## 🔧 Kurulum

### Gereksinimler
- .NET 8.0 SDK
- PostgreSQL 15+
- RabbitMQ 3.8+
- Docker & Docker Compose (Opsiyonel)

### Local Development
```bash
# Repository'yi klonlayın
git clone <repository-url>
cd OrderOperations

# Bağımlılıkları yükleyin
dotnet restore

# Veritabanı migration'larını çalıştırın
dotnet ef database update --project Src/Infrastructure/OrderOperations.Persistence --startup-project Src/WebApi/OrderOperations.WebApi

# Uygulamayı çalıştırın
dotnet run --project Src/WebApi/OrderOperations.WebApi
```

### Docker ile Çalıştırma
```bash
# Development environment
docker-compose -f docker-compose.dev.yml up -d

# Production environment
docker-compose -f docker-compose.prod.yml up -d
```

## 🧪 Testing

```bash
# Unit Tests
dotnet test Test/OrderOperations.Tests/

# Integration Tests
dotnet test Test/OrderOperations.IntegrationTests/

# Tüm testler
dotnet test
```

## 📊 Monitoring & Logging

- **Serilog**: Structured logging
- **Swagger UI**: API documentation (http://localhost:8081/swagger)
- **RabbitMQ Management**: Message queue monitoring (http://localhost:15673)

## 🔐 Security

- JWT Bearer Token authentication
- Role-based authorization
- Input validation with FluentValidation
- CORS configuration
- HTTPS redirection
