<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 8.0">
  <img src="https://img.shields.io/badge/Blazor-Server%20%2B%20WASM-512BD4?style=for-the-badge&logo=blazor&logoColor=white" alt="Blazor">
  <img src="https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white" alt="SQL Server">
  <img src="https://img.shields.io/badge/MudBlazor-594AE2?style=for-the-badge&logo=blazor&logoColor=white" alt="MudBlazor">
</p>

# 🎓 HỆ THỐNG QUẢN LÝ NGHIÊN CỨU KHOA HỌC HỌC VIÊN

> Hệ thống quản lý toàn diện quy trình nghiên cứu khoa học từ đăng ký đề tài đến xếp giải, xây dựng trên nền tảng .NET 8.0 với Blazor Hybrid (Server + WebAssembly).

---

## 📋 Mục lục

- [Tổng quan](#-tổng-quan)
- [Tính năng](#-tính-năng)
- [Công nghệ](#-công-nghệ)
- [Kiến trúc](#-kiến-trúc)
- [Cấu trúc dự án](#-cấu-trúc-dự-án)
- [Cơ sở dữ liệu](#-cơ-sở-dữ-liệu)
- [Cài đặt](#-cài-đặt)
- [Cấu hình](#-cấu-hình)
- [Chạy ứng dụng](#-chạy-ứng-dụng)
- [API Documentation](#-api-documentation)
- [Phân quyền](#-phân-quyền)
- [Screenshots](#-screenshots)
- [Roadmap](#-roadmap)
- [Đóng góp](#-đóng-góp)
- [License](#-license)
- [Liên hệ](#-liên-hệ)

---

## 🎯 Tổng quan

**QLNCKH_HocVien** là hệ thống quản lý nghiên cứu khoa học dành cho các cơ sở giáo dục, hỗ trợ:

- 📝 Quản lý thông tin sinh viên và giáo viên
- 📚 Đăng ký và theo dõi chuyên đề nghiên cứu
- 📤 Nộp và quản lý sản phẩm nghiên cứu
- 👥 Lập hội đồng chấm điểm
- ⭐ Chấm điểm và xếp giải tự động

---

## ✨ Tính năng

### 🔐 Authentication & Authorization
- Đăng nhập/Đăng xuất với Cookie Authentication
- Phân quyền 3 cấp: Admin, Giáo viên, Học viên
- Bảo vệ API endpoints theo role

### 👤 Quản lý danh mục
| Chức năng | Mô tả |
|-----------|-------|
| Quản lý Sinh viên | CRUD, tìm kiếm, phân trang, xuất Excel |
| Quản lý Giáo viên | CRUD, tích hợp danh mục ngoài (trình độ, chức danh, học vị) |

### 📖 Quản lý Chuyên đề NCKH
| Chức năng | Mô tả |
|-----------|-------|
| Đăng ký chuyên đề | Sinh viên đăng ký đề tài nghiên cứu |
| Nộp sản phẩm | Upload và quản lý file/tài liệu |
| Theo dõi trạng thái | Cập nhật tiến độ thực hiện |

### ⚖️ Đánh giá & Chấm điểm
| Chức năng | Mô tả |
|-----------|-------|
| Lập Hội đồng | Tạo hội đồng chấm cho từng vòng thi |
| Chấm sơ loại | 1 người chấm, lọc Top 15 mỗi lĩnh vực |
| Chấm chung khảo | Nhiều giám khảo, phiếu chấm chi tiết |
| Xếp giải tự động | Tính điểm TB, xếp hạng và trao giải |

### 🎨 UI/UX
- ✅ Giao diện hiện đại với MudBlazor
- ✅ Responsive design (Desktop, Tablet, Mobile)
- ✅ Dark Mode / Light Mode
- ✅ Loading skeletons & animations
- ✅ Toast notifications

---

## 🛠 Công nghệ

### Backend
| Công nghệ | Phiên bản | Mục đích |
|-----------|-----------|----------|
| .NET | 8.0 | Framework chính |
| ASP.NET Core | 8.0 | Web API & Blazor Server |
| Entity Framework Core | 8.0 | ORM |
| SQL Server | 2019+ | Database |

### Frontend
| Công nghệ | Phiên bản | Mục đích |
|-----------|-----------|----------|
| Blazor | Server + WASM | UI Framework |
| MudBlazor | 8.15.0 | Component Library |
| Bootstrap | 5.x | Responsive Grid |

### Thư viện bổ sung
| Thư viện | Mục đích |
|----------|----------|
| ClosedXML | Xuất file Excel |
| System.Text.Json | JSON Serialization |

---

## 🏗 Kiến trúc

```
┌─────────────────────────────────────────────────────────────┐
│                      CLIENT (Browser)                       │
│  ┌─────────────────┐  ┌─────────────────┐                  │
│  │  Blazor WASM    │  │  Blazor Server  │                  │
│  │    (Pages)      │  │   (SSR/Hybrid)  │                  │
│  └────────┬────────┘  └────────┬────────┘                  │
│           │                    │                            │
│           └────────┬───────────┘                            │
│                    │                                        │
└────────────────────┼────────────────────────────────────────┘
                     │ HTTP/SignalR
┌────────────────────┼────────────────────────────────────────┐
│                    │         SERVER                         │
│  ┌─────────────────▼─────────────────┐                     │
│  │         API Controllers           │                     │
│  │   (Auth, SinhVien, GiaoVien...)   │                     │
│  └─────────────────┬─────────────────┘                     │
│                    │                                        │
│  ┌─────────────────▼─────────────────┐                     │
│  │       Business Services           │                     │
│  │  (Validation, Business Logic)     │                     │
│  └─────────────────┬─────────────────┘                     │
│                    │                                        │
│  ┌─────────────────▼─────────────────┐                     │
│  │    Repositories + Unit of Work    │                     │
│  │      (Data Access Layer)          │                     │
│  └─────────────────┬─────────────────┘                     │
│                    │                                        │
│  ┌─────────────────▼─────────────────┐                     │
│  │     Entity Framework Core         │                     │
│  │       (ApplicationDbContext)      │                     │
│  └─────────────────┬─────────────────┘                     │
└────────────────────┼────────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────────┐
│                   SQL SERVER                                │
│  ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐      │
│  │ SinhVien │ │ GiaoVien │ │ChuyenDe  │ │ HoiDong  │ ...  │
│  └──────────┘ └──────────┘ └──────────┘ └──────────┘      │
└─────────────────────────────────────────────────────────────┘
```

### Design Patterns
- **Repository Pattern** - Abstraction layer cho data access
- **Unit of Work** - Quản lý transactions
- **Service Layer** - Business logic tách biệt
- **Factory Pattern** - API Result responses

---

## 📁 Cấu trúc dự án

```
QLNCKH_HocVien/
│
├── 📄 QLNCKH_HocVien.sln              # Solution file
├── 📄 README.md                        # Documentation
│
├── 📁 QLNCKH_HocVien/                  # Server Project
│   ├── 📁 Controllers/                 # API Controllers
│   │   ├── AuthController.cs           # Authentication
│   │   ├── SinhVienController.cs       # Sinh viên API
│   │   ├── GiaoVienController.cs       # Giáo viên API
│   │   ├── ChuyenDeNCKHController.cs   # Chuyên đề API
│   │   ├── NopSanPhamController.cs     # Nộp sản phẩm API
│   │   ├── HoiDongController.cs        # Hội đồng API
│   │   ├── KetQuaController.cs         # Kết quả chấm API
│   │   ├── XepGiaiController.cs        # Xếp giải API
│   │   ├── DashboardController.cs      # Thống kê
│   │   └── UserManagementController.cs # Quản lý user
│   │
│   ├── 📁 Data/
│   │   └── ApplicationDbContext.cs     # EF Core DbContext
│   │
│   ├── 📁 Repositories/                # Data Access Layer
│   │   ├── IRepository.cs              # Generic interface
│   │   ├── Repository.cs               # Generic implementation
│   │   ├── ISpecificRepositories.cs    # Specific interfaces
│   │   ├── SpecificRepositories.cs     # Specific implementations
│   │   └── UnitOfWork.cs               # Unit of Work
│   │
│   ├── 📁 Services/                    # Business Layer
│   │   ├── IBusinessServices.cs        # Interfaces
│   │   ├── BusinessServices.cs         # Implementations
│   │   └── CacheService.cs             # Caching
│   │
│   ├── 📁 Helpers/                     # Utilities
│   │   ├── ValidationHelper.cs         # Server-side validation
│   │   ├── ApiResultExtensions.cs      # Response helpers
│   │   └── AuthorizationHelper.cs      # Auth helpers
│   │
│   ├── 📁 Middleware/                  # ASP.NET Middleware
│   │   ├── ExceptionMiddleware.cs      # Global error handling
│   │   └── RequestLoggingMiddleware.cs # Request logging
│   │
│   ├── 📁 Migrations/                  # EF Core Migrations
│   │
│   ├── 📁 Components/                  # Blazor Server Components
│   │   ├── 📁 Layout/
│   │   │   ├── MainLayout.razor        # Main layout
│   │   │   └── NavMenu.razor           # Navigation
│   │   └── 📁 Pages/
│   │       └── *.razor                 # Server-side pages
│   │
│   ├── 📄 Program.cs                   # Application entry point
│   └── 📄 appsettings.json             # Configuration
│
└── 📁 QLNCKH_HocVien.Client/           # Client Project (WASM)
    ├── 📁 Models/                      # Data Models
    │   ├── ApiResult.cs                # API response models
    │   ├── AuthModels.cs               # Auth models
    │   ├── SinhVien.cs
    │   ├── GiaoVien.cs
    │   ├── ChuyenDeNCKH.cs
    │   ├── NopSanPham.cs
    │   ├── HoiDong.cs
    │   ├── KetQuaModels.cs
    │   ├── XepGiai.cs
    │   └── DanhMucModels.cs            # Lookup models
    │
    ├── 📁 Services/                    # API Client Services
    │   ├── AuthService.cs
    │   ├── SinhVienService.cs
    │   ├── GiaoVienService.cs
    │   ├── ChuyenDeNCKHService.cs
    │   ├── NopSanPhamService.cs
    │   ├── HoiDongService.cs
    │   ├── KetQuaService.cs
    │   ├── XepGiaiService.cs
    │   ├── DashboardService.cs
    │   └── UserManagementService.cs
    │
    ├── 📁 Pages/                       # Blazor Pages
    │   ├── QuanLySinhVien.razor
    │   ├── QuanLyGiaoVien.razor
    │   ├── DangKyChuyenDe.razor
    │   ├── NopChuyenDe.razor
    │   ├── LapHoiDong.razor
    │   ├── CapNhatKetQua.razor
    │   ├── QuanLyXepGiai.razor
    │   └── QuanLyNguoiDung.razor
    │
    ├── 📁 Shared/                      # Shared Components
    │   ├── PageHeader.razor
    │   ├── TableSkeleton.razor
    │   ├── ConfirmDialog.razor
    │   └── LoadingSpinner.razor
    │
    ├── 📁 Extensions/
    │   └── HttpClientExtensions.cs     # HTTP helpers
    │
    ├── 📁 Exceptions/
    │   └── UnauthorizedException.cs
    │
    └── 📄 Program.cs                   # WASM entry point
```

---

## 🗄 Cơ sở dữ liệu

### ERD (Entity Relationship Diagram)

```
┌─────────────────┐       ┌─────────────────┐
│    NguoiDung    │       │    SinhVien     │
├─────────────────┤       ├─────────────────┤
│ Id (PK)         │       │ Id (PK)         │
│ TenDangNhap     │       │ MaSV (Unique)   │
│ MatKhau         │       │ HoTen           │
│ HoTen           │       │ NgaySinh        │
│ VaiTro          │       │ GioiTinh        │
│ IsActive        │       │ Lop, ...        │
│ NgayTao         │       └────────┬────────┘
└─────────────────┘                │
                                   │ 1:N
┌─────────────────┐       ┌────────▼────────┐
│    GiaoVien     │       │  ChuyenDeNCKH   │
├─────────────────┤       ├─────────────────┤
│ Id (PK)         │       │ Id (PK)         │
│ MaSoCB (Unique) │       │ MaSoCD (Unique) │
│ HoTen           │       │ TenChuyenDe     │
│ HocVi, HocHam   │       │ IdHocVien (FK)  │◄────┐
│ ChucDanh, ...   │       │ IdLinhVuc       │     │
└────────┬────────┘       └────────┬────────┘     │
         │                         │              │
         │ N:M                     │ 1:N          │
         │                         │              │
┌────────▼────────┐       ┌────────▼────────┐     │
│ThanhVienHoiDong │       │   NopSanPham    │     │
├─────────────────┤       ├─────────────────┤     │
│ Id (PK)         │       │ Id (PK)         │     │
│ IdHoiDong (FK)  │       │ IdChuyenDe (FK) │     │
│ IdGiaoVien (FK) │       │ TenFile         │     │
│ VaiTro          │       │ TrangThai       │     │
└────────▲────────┘       │ NgayNop         │     │
         │                └─────────────────┘     │
         │ 1:N                                    │
┌────────┴────────┐                               │
│    HoiDong      │       ┌─────────────────┐     │
├─────────────────┤       │  KetQuaSoLoai   │     │
│ Id (PK)         │       ├─────────────────┤     │
│ IdChuyenDe (FK) │───────│ Id (PK)         │     │
│ VongThi         │       │ IdChuyenDe (FK) │─────┤
│ NgayCham        │       │ DiemSo          │     │
│ DiaDiem         │       │ KetQua          │     │
└─────────────────┘       └─────────────────┘     │
                                                  │
┌─────────────────┐       ┌─────────────────┐     │
│   PhieuCham     │       │    XepGiai      │     │
├─────────────────┤       ├─────────────────┤     │
│ Id (PK)         │       │ Id (PK)         │     │
│ IdChuyenDe (FK) │───────│ IdChuyenDe (FK) │─────┘
│ IdGiaoVien (FK) │       │ DiemTrungBinh   │
│ Diem            │       │ XepHang         │
│ YKien           │       │ TenGiai         │
└─────────────────┘       └─────────────────┘
```

### Các bảng chính

| Bảng | Mô tả | Quan hệ |
|------|-------|---------|
| `NguoiDungs` | Tài khoản người dùng | - |
| `SinhViens` | Thông tin sinh viên | 1:N → ChuyenDeNCKH |
| `GiaoViens` | Thông tin giáo viên | N:M → ThanhVienHoiDong |
| `ChuyenDeNCKHs` | Đề tài nghiên cứu | N:1 ← SinhVien |
| `NopSanPhams` | Sản phẩm nộp | N:1 ← ChuyenDe |
| `HoiDongs` | Hội đồng chấm | N:1 ← ChuyenDe |
| `ThanhVienHoiDongs` | Thành viên hội đồng | N:1 ← HoiDong, GiaoVien |
| `KetQuaSoLoais` | Kết quả sơ loại | 1:1 ← ChuyenDe |
| `PhieuChams` | Phiếu chấm chung khảo | N:1 ← ChuyenDe, GiaoVien |
| `XepGiais` | Kết quả xếp giải | 1:1 ← ChuyenDe |

---

## 🚀 Cài đặt

### Yêu cầu hệ thống

| Phần mềm | Phiên bản | Ghi chú |
|----------|-----------|---------|
| .NET SDK | 8.0+ | [Download](https://dotnet.microsoft.com/download) |
| SQL Server | 2019+ | Hoặc Azure SQL |
| Visual Studio | 2022+ | Khuyến nghị |
| VS Code | Latest | Alternative |

### Bước 1: Clone repository

```bash
git clone https://github.com/cahoa05/QLNCKH_HocVien_Blazor.git
cd QLNCKH_HocVien_Blazor/QLNCKH_HocVien
```

### Bước 2: Restore packages

```bash
dotnet restore
```

### Bước 3: Cấu hình Connection String

**Sử dụng User Secrets (Khuyến nghị):**

```bash
cd QLNCKH_HocVien
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=YOUR_SERVER;Database=QuanLyNCKH_Db;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
```

### Bước 4: Chạy Migration

```bash
dotnet ef database update
```

### Bước 5: Khởi tạo Admin (lần đầu)

Truy cập: `https://localhost:xxxx/api/auth/init-admin`

Hoặc sử dụng tài khoản mặc định:
- **Username:** `admin`
- **Password:** `Admin@123`

---

## ⚙️ Cấu hình

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=...;..."
  }
}
```

### Environment Variables

| Variable | Mô tả | Mặc định |
|----------|-------|----------|
| `ASPNETCORE_ENVIRONMENT` | Development/Production | Development |
| `ConnectionStrings__DefaultConnection` | DB Connection | - |

---

## ▶️ Chạy ứng dụng

### Development

```bash
cd QLNCKH_HocVien
dotnet run
```

Hoặc với hot reload:

```bash
dotnet watch run
```

### Production

```bash
dotnet publish -c Release -o ./publish
cd publish
dotnet QLNCKH_HocVien.dll
```

### Truy cập

| URL | Mô tả |
|-----|-------|
| `https://localhost:7xxx` | HTTPS (Development) |
| `http://localhost:5xxx` | HTTP (Development) |

---

## 📚 API Documentation

### Base URL
```
https://localhost:7xxx/api
```

### Authentication

| Endpoint | Method | Mô tả |
|----------|--------|-------|
| `/auth/login` | POST | Đăng nhập |
| `/auth/logout` | POST/GET | Đăng xuất |
| `/auth/me` | GET | Thông tin user hiện tại |
| `/auth/check` | GET | Kiểm tra trạng thái auth |
| `/auth/change-password` | POST | Đổi mật khẩu |
| `/auth/register-public` | POST | Đăng ký tài khoản |

### Sinh viên

| Endpoint | Method | Auth | Mô tả |
|----------|--------|------|-------|
| `/sinhvien` | GET | ✅ | Lấy tất cả |
| `/sinhvien/paged` | GET | ✅ | Phân trang |
| `/sinhvien/{id}` | GET | ✅ | Lấy theo ID |
| `/sinhvien` | POST | Admin | Thêm mới |
| `/sinhvien/{id}` | PUT | Admin | Cập nhật |
| `/sinhvien/{id}` | DELETE | Admin | Xóa |
| `/sinhvien/export` | GET | ✅ | Xuất Excel |

### Giáo viên

| Endpoint | Method | Auth | Mô tả |
|----------|--------|------|-------|
| `/giaovien` | GET | ✅ | Lấy tất cả |
| `/giaovien/paged` | GET | ✅ | Phân trang |
| `/giaovien/{id}` | GET | ✅ | Lấy theo ID |
| `/giaovien` | POST | Admin | Thêm mới |
| `/giaovien/{id}` | PUT | Admin | Cập nhật |
| `/giaovien/{id}` | DELETE | Admin | Xóa |

### Response Format

**Success:**
```json
{
  "success": true,
  "message": "Thành công",
  "data": { ... }
}
```

**Error:**
```json
{
  "success": false,
  "message": "Lỗi",
  "errors": ["Chi tiết lỗi 1", "Chi tiết lỗi 2"]
}
```

**Paginated:**
```json
{
  "success": true,
  "data": [...],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 100,
  "totalPages": 10,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

---

## 🔐 Phân quyền

### Roles

| Role | Mô tả | Quyền hạn |
|------|-------|-----------|
| **Admin** | Quản trị viên | Toàn quyền |
| **GiaoVien** | Giáo viên | Xem, Chấm điểm, Lập hội đồng |
| **User** | Học viên | Đăng ký, Nộp sản phẩm |

### Ma trận phân quyền

| Chức năng | Admin | GiaoVien | User |
|-----------|:-----:|:--------:|:----:|
| Quản lý Sinh viên | ✅ CRUD | 👁 Xem | ❌ |
| Quản lý Giáo viên | ✅ CRUD | 👁 Xem | ❌ |
| Đăng ký Chuyên đề | ✅ CRUD | 👁 Xem | ✅ Của mình |
| Nộp Sản phẩm | ✅ CRUD | 👁 Xem | ✅ Của mình |
| Lập Hội đồng | ✅ CRUD | ✅ CRUD | ❌ |
| Chấm điểm | ✅ Tất cả | ✅ Được phân công | ❌ |
| Xếp giải | ✅ CRUD | 👁 Xem | 👁 Xem |
| Quản lý User | ✅ CRUD | ❌ | ❌ |
| Admin Dashboard | ✅ | ❌ | ❌ |

---

## 📸 Screenshots

> *Screenshots sẽ được cập nhật sau*

| Trang | Mô tả |
|-------|-------|
| Login | Trang đăng nhập |
| Dashboard | Tổng quan hệ thống |
| Quản lý SV | Danh sách sinh viên |
| Quản lý GV | Danh sách giáo viên |
| Chấm điểm | Giao diện chấm điểm |
| Xếp giải | Kết quả xếp giải |

---

## 🗺 Roadmap

### ✅ Đã hoàn thành
- [x] Authentication & Authorization
- [x] CRUD Sinh viên, Giáo viên
- [x] Đăng ký & Nộp Chuyên đề
- [x] Lập Hội đồng
- [x] Chấm điểm Sơ loại & Chung khảo
- [x] Xếp giải tự động
- [x] Quản lý Người dùng
- [x] Xuất Excel
- [x] Dark Mode
- [x] Responsive Design

### 🚧 Đang phát triển
- [ ] Reset Password qua Email
- [ ] Audit Log
- [ ] Notifications

### 📋 Kế hoạch
- [ ] Import Excel
- [ ] Export PDF
- [ ] 2FA (Two-Factor Authentication)
- [ ] API Rate Limiting
- [ ] Unit Tests
- [ ] Docker Support

---

## 🤝 Đóng góp

Mọi đóng góp đều được hoan nghênh! Vui lòng:

1. Fork repository
2. Tạo branch mới (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Mở Pull Request

### Coding Standards

- Sử dụng C# naming conventions
- Viết comments cho public methods
- Đảm bảo code pass tất cả linter checks
- Thêm unit tests cho features mới

---

## 📄 License

Dự án này được phân phối dưới giấy phép **MIT License**.

```
MIT License

Copyright (c) 2024 cahoa05

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction...
```

---

## 📧 Liên hệ

**Tác giả:** cahoa05

- 🌐 Facebook: [https://www.facebook.com/cahoa05](https://www.facebook.com/cahoa05)
- 📧 Email: *[kyhoaca@gmail.com]*
- 💻 GitHub: [https://github.com/kyhoa013at](https://github.com/kyhoa013at)

---

<p align="center">
  <b>⭐ Nếu dự án hữu ích, hãy cho một Star! ⭐</b>
</p>

<p align="center">
  Made with ❤️ using .NET 8.0 & Blazor
</p>
