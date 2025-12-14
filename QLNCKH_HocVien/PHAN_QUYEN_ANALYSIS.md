# PHÂN TÍCH PHÂN QUYỀN HỆ THỐNG

## 📊 TÌNH TRẠNG HIỆN TẠI

### ✅ Đã có:
1. **3 Roles được định nghĩa:**
   - `Admin` - Quản trị viên
   - `User` - Học viên/Sinh viên
   - `GiaoVien` - Giáo viên

2. **Authentication cơ bản:**
   - Tất cả Controllers có `[Authorize]` - yêu cầu đăng nhập
   - Cookie-based authentication đã hoạt động
   - Claims được set khi đăng nhập (NameIdentifier, Name, GivenName, Role)

3. **Phân quyền hiện có:**
   - `AdminDashboard` - Chỉ Admin (`[Authorize(Roles = "Admin")]`)
   - `AuthController.Register` - Chỉ Admin (`[Authorize(Roles = "Admin")]`)
   - NavMenu ẩn "Admin Dashboard" cho non-Admin

### ❌ Chưa có:
1. **Phân quyền cho các trang (Pages):**
   - Tất cả trang đều có thể truy cập bởi bất kỳ user đã đăng nhập
   - Không có kiểm tra role cho các trang như:
     - Quản lý Sinh viên
     - Quản lý Giáo viên
     - Đăng ký Chuyên đề
     - Nộp sản phẩm
     - Lập Hội đồng
     - Chấm điểm
     - Xếp giải

2. **Phân quyền cho API endpoints:**
   - Tất cả endpoints đều chỉ yêu cầu `[Authorize]` (đã đăng nhập)
   - Không phân biệt quyền theo role:
     - POST/PUT/DELETE Sinh viên/Giáo viên - nên chỉ Admin
     - POST/PUT/DELETE Chuyên đề - nên Admin hoặc User
     - POST/PUT Chấm điểm - nên Admin hoặc GiaoVien
     - POST Xếp giải - nên chỉ Admin

3. **UI phân quyền:**
   - NavMenu không ẩn menu theo role (trừ Admin Dashboard)
   - Các nút "Thêm", "Sửa", "Xóa" hiển thị cho tất cả user

---

## 🎯 ĐỀ XUẤT PHÂN QUYỀN

### 1. **ADMIN** - Toàn quyền
- ✅ Xem và quản lý tất cả
- ✅ Quản lý Sinh viên (CRUD)
- ✅ Quản lý Giáo viên (CRUD)
- ✅ Quản lý Chuyên đề (CRUD)
- ✅ Lập Hội đồng
- ✅ Chấm điểm
- ✅ Xếp giải
- ✅ Admin Dashboard

### 2. **HỌC VIÊN (User)** - Quyền hạn chế
- ✅ Xem thông tin cá nhân
- ✅ Đăng ký Chuyên đề (chỉ của mình)
- ✅ Nộp sản phẩm (chỉ của mình)
- ✅ Xem kết quả chấm (chỉ của mình)
- ❌ Quản lý Sinh viên/Giáo viên
- ❌ Chấm điểm
- ❌ Lập Hội đồng
- ❌ Xếp giải

### 3. **GIÁO VIÊN (GiaoVien)** - Quyền trung bình
- ✅ Xem danh sách Sinh viên, Giáo viên
- ✅ Xem danh sách Chuyên đề
- ✅ Lập Hội đồng
- ✅ Chấm điểm (chỉ chuyên đề được phân công)
- ✅ Xem kết quả chấm
- ❌ Quản lý Sinh viên/Giáo viên (CRUD)
- ❌ Xếp giải

---

## 📝 CHI TIẾT PHÂN QUYỀN THEO CHỨC NĂNG

### **Quản lý Sinh viên** (`/quanlysinhvien`)
- **GET** (Xem): Admin, GiaoVien
- **POST/PUT/DELETE** (Thêm/Sửa/Xóa): Chỉ Admin

### **Quản lý Giáo viên** (`/quanlygiaovien`)
- **GET** (Xem): Admin, GiaoVien
- **POST/PUT/DELETE** (Thêm/Sửa/Xóa): Chỉ Admin

### **Đăng ký Chuyên đề** (`/dangkychuyende`)
- **GET** (Xem): Tất cả
- **POST** (Đăng ký): Admin, User (chỉ đăng ký cho chính mình)
- **PUT/DELETE** (Sửa/Xóa): Admin, User (chỉ chuyên đề của mình)

### **Nộp sản phẩm** (`/nopchuyende`)
- **GET** (Xem): Tất cả
- **POST** (Nộp): Admin, User (chỉ nộp cho chuyên đề của mình)
- **PUT** (Cập nhật): Admin, User (chỉ sản phẩm của mình)

### **Lập Hội đồng** (`/laphoidong`)
- **GET** (Xem): Admin, GiaoVien
- **POST/PUT/DELETE** (Thêm/Sửa/Xóa): Admin, GiaoVien

### **Chấm điểm** (`/capnhatketqua`)
- **GET** (Xem): Admin, GiaoVien
- **POST/PUT** (Chấm): Admin, GiaoVien (chỉ chuyên đề được phân công)

### **Xếp giải** (`/xepgiai`)
- **GET** (Xem): Tất cả
- **POST/PUT/DELETE** (Thêm/Sửa/Xóa): Chỉ Admin

---

## 🔧 CẦN THỰC HIỆN

### 1. Thêm phân quyền cho API Controllers
```csharp
// Ví dụ: SinhVienController
[HttpPost]
[Authorize(Roles = "Admin")]  // Chỉ Admin
public async Task<ActionResult> CreateSinhVien(...)

[HttpPut("{id}")]
[Authorize(Roles = "Admin")]  // Chỉ Admin
public async Task<ActionResult> UpdateSinhVien(...)

[HttpDelete("{id}")]
[Authorize(Roles = "Admin")]  // Chỉ Admin
public async Task<ActionResult> DeleteSinhVien(...)
```

### 2. Thêm phân quyền cho Pages
```razor
@page "/quanlysinhvien"
@attribute [Authorize(Roles = "Admin,GiaoVien")]
@using Microsoft.AspNetCore.Authorization
```

### 3. Cập nhật NavMenu để ẩn/hiện menu theo role
```razor
@if (_isAdmin || _isGiaoVien)
{
    <MudNavLink Href="quanlysinhvien">Quản lý Sinh viên</MudNavLink>
}
```

### 4. Ẩn nút thao tác trong UI theo role
```razor
@if (_isAdmin)
{
    <MudButton OnClick="OpenAddDialog">Thêm</MudButton>
    <MudButton OnClick="OpenEditDialog">Sửa</MudButton>
    <MudButton OnClick="DeleteItem">Xóa</MudButton>
}
```

---

## 📌 LƯU Ý

1. **Kiểm tra quyền ở cả Client và Server:**
   - Client: Ẩn/hiện UI, redirect nếu không có quyền
   - Server: Validate quyền trong API, trả về 403 Forbidden nếu không có quyền

2. **Quyền sở hữu (Ownership):**
   - User chỉ có thể sửa/xóa chuyên đề/sản phẩm của chính mình
   - Cần kiểm tra `IdHocVien` trong database

3. **Quyền phân công:**
   - GiaoVien chỉ có thể chấm chuyên đề được phân công trong Hội đồng
   - Cần kiểm tra `ThanhVienHoiDong` và `PhieuCham`

---

## ✅ KẾT LUẬN

**Hiện tại project CHƯA có phân quyền rõ ràng** cho Admin, Học viên, và Giáo viên. Chỉ có:
- Authentication cơ bản (yêu cầu đăng nhập)
- 1 trang có phân quyền (Admin Dashboard)
- 1 API endpoint có phân quyền (Register - Admin only)

**Cần triển khai phân quyền đầy đủ** theo đề xuất trên để đảm bảo bảo mật và quyền truy cập phù hợp cho từng role.

