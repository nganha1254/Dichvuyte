CREATE DATABASE Doan1;
GO

USE Doan1;
GO
CREATE TABLE tb_Role (
    RoleId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    RoleName NVARCHAR(50) NULL,       -- Tên vai trò (Admin, Bác sĩ, Bệnh nhân, Lễ tân)
    Description NVARCHAR(100) NULL
);

  --- BẢNG TÀI KHOẢN NGƯỜI DÙNG
CREATE TABLE tb_Account (
    AccountId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Username NVARCHAR(50) NULL,
    Password NVARCHAR(50) NULL,
    FullName NVARCHAR(100) NULL,
    Phone NVARCHAR(50) NULL,
    Email NVARCHAR(100) NULL,
    RoleId INT NULL,
    LastLogin DATETIME NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (RoleId) REFERENCES tb_Role(RoleId)
);


   ---BẢNG CHUYÊN KHOA (tb_Category)
CREATE TABLE tb_Category (
    CategoryId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Title NVARCHAR(150) NULL,
    Alias NVARCHAR(150) NULL,
    Description NVARCHAR(500) NULL,
    Position INT NULL,
    SeoTitle NVARCHAR(250) NULL,
    SeoDescription NVARCHAR(500) NULL,
    SeoKeywords NVARCHAR(250) NULL,
    CreatedDate DATETIME NULL,
    CreatedBy NVARCHAR(150) NULL,
    ModifiedDate DATETIME NULL,
    ModifiedBy NVARCHAR(150) NULL
);

   ---BẢNG BÁC SĨ
CREATE TABLE tb_Doctor (
    DoctorId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    Gender NVARCHAR(10) NULL,
    DateOfBirth DATE NULL,
    Phone NVARCHAR(50) NULL,
    Email NVARCHAR(150) NULL,
    Address NVARCHAR(250) NULL,
    CategoryId INT NULL,                    -- Liên kết với khoa
    Position NVARCHAR(100) NULL,
    Qualification NVARCHAR(200) NULL,
    ExperienceYears INT NULL,
    Description NVARCHAR(MAX) NULL,
    Image NVARCHAR(500) NULL,
    AccountId INT NULL,                     -- Nếu bác sĩ có tài khoản đăng nhập
    CreatedDate DATETIME NULL,
    CreatedBy NVARCHAR(150) NULL,
    ModifiedDate DATETIME NULL,
    ModifiedBy NVARCHAR(150) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (CategoryId) REFERENCES tb_Category(CategoryId),
    FOREIGN KEY (AccountId) REFERENCES tb_Account(AccountId)
);
ALTER TABLE tb_Doctor
ADD IsNew BIT NOT NULL DEFAULT 0;
-- Tạo lại bảng dịch vụ
CREATE TABLE tb_Service (
    ServiceId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Title NVARCHAR(250) NULL,                 -- Tên dịch vụ (ví dụ: Cardiology Care)
    Alias NVARCHAR(250) NULL,                 -- Đường dẫn thân thiện (ví dụ: cardiology-care)
    Icon NVARCHAR(500) NULL,                  -- Biểu tượng / icon dịch vụ
    Image NVARCHAR(500) NULL,                 -- Hình ảnh đại diện
    ShortDescription NVARCHAR(500) NULL,      -- Mô tả ngắn
    Detail NVARCHAR(MAX) NULL,                -- Nội dung chi tiết dịch vụ
    DoctorId INT NULL,                        -- Bác sĩ phụ trách (nếu có)
    CategoryId INT NULL,                      -- Chuyên khoa liên quan
    Position INT NULL,                        -- Thứ tự hiển thị
    SeoTitle NVARCHAR(250) NULL,
    SeoDescription NVARCHAR(500) NULL,
    SeoKeywords NVARCHAR(250) NULL,
    CreatedDate DATETIME NULL,
    CreatedBy NVARCHAR(150) NULL,
    ModifiedDate DATETIME NULL,
    ModifiedBy NVARCHAR(150) NULL,
    IsNew BIT NOT NULL DEFAULT 0,             -- Đánh dấu dịch vụ mới
    IsFeatured BIT NOT NULL DEFAULT 0,        -- Hiển thị ở mục "Featured Services"
    IsActive BIT NOT NULL DEFAULT 1,          -- Trạng thái hoạt động
    FOREIGN KEY (DoctorId) REFERENCES tb_Doctor(DoctorId),
    FOREIGN KEY (CategoryId) REFERENCES tb_Category(CategoryId)
);
   ---BẢNG NHÓM DỊCH VỤ (tb_ProductCategory)
   -- Nhóm loại dịch vụ
CREATE TABLE tb_ProductCategory (
    ProductCategoryId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Title NVARCHAR(150) NULL,
    Alias NVARCHAR(150) NULL,
    Description NVARCHAR(500) NULL,
    Icon NVARCHAR(500) NULL,
    Position INT NULL,
    CreatedDate DATETIME NULL,
    CreatedBy NVARCHAR(150) NULL,
    ModifiedDate DATETIME NULL,
    ModifiedBy NVARCHAR(150) NULL,
    IsActive BIT NOT NULL DEFAULT 1
);


  --- BẢNG DỊCH VỤ Y TẾ (tb_Product)
   -- Danh mục dịch vụ y tế (khám, xét nghiệm, v.v.
CREATE TABLE tb_Product (
    ProductId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Title NVARCHAR(250) NULL,
    Alias NVARCHAR(250) NULL,
    ProductCategoryId INT NULL,
    Description NVARCHAR(4000) NULL,
    Detail NVARCHAR(MAX) NULL,
    Image NVARCHAR(500) NULL,
    Price INT NULL,
    PriceSale INT NULL,
    Duration NVARCHAR(50) NULL,
    DoctorId INT NULL,
    CreatedDate DATETIME NULL,
    CreatedBy NVARCHAR(150) NULL,
    ModifiedDate DATETIME NULL,
    ModifiedBy NVARCHAR(150) NULL,
    IsNew BIT NOT NULL DEFAULT 0,
    IsPopular BIT NOT NULL DEFAULT 0,
    SlotsAvailable INT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    Star INT NULL,
    FOREIGN KEY (ProductCategoryId) REFERENCES tb_ProductCategory(ProductCategoryId),
    FOREIGN KEY (DoctorId) REFERENCES tb_Doctor(DoctorId)
);


   ---BẢNG BỆNH NHÂN
CREATE TABLE tb_Patient (
    PatientId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    Gender NVARCHAR(10) NULL,
    DateOfBirth DATE NULL,
    Phone NVARCHAR(50) NULL,
    Email NVARCHAR(100) NULL,
    Address NVARCHAR(250) NULL,
    AccountId INT NULL,
    CreatedDate DATETIME NULL,
    ModifiedDate DATETIME NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (AccountId) REFERENCES tb_Account(AccountId)
);

  --- BẢNG TRẠNG THÁI ĐƠN / ĐẶT LỊCH
CREATE TABLE tb_BookingStatus (
    BookingStatusId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(50) NULL,
    Description NVARCHAR(100) NULL
);


   --BẢNG ĐẶT LỊCH / ĐƠN DỊCH VỤ (tb_Order)
CREATE TABLE tb_Order (
    OrderId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Code NCHAR(10) NULL,
    PatientId INT NULL,
    DoctorId INT NULL,
    TotalAmount INT NULL,
    BookingStatusId INT NULL,
    BookingDate DATETIME NULL,
    AppointmentDate DATETIME NULL,
    CreatedDate DATETIME NULL,
    CreatedBy NVARCHAR(150) NULL,
    ModifiedDate DATETIME NULL,
    ModifiedBy NVARCHAR(150) NULL,
    FOREIGN KEY (PatientId) REFERENCES tb_Patient(PatientId),
    FOREIGN KEY (DoctorId) REFERENCES tb_Doctor(DoctorId),
    FOREIGN KEY (BookingStatusId) REFERENCES tb_BookingStatus(BookingStatusId)
);


   ----BẢNG CHI TIẾT ĐƠN / LỊCH HẸN (tb_OrderDetail)
CREATE TABLE tb_OrderDetail (
    OrderDetailId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    OrderId INT NULL,
    ProductId INT NULL,
    Price DECIMAL(18, 0) NULL,
    Quantity INT NULL,
    CreatedBy NVARCHAR(150) NULL,
    ModifiedDate DATETIME NULL,
    ModifiedBy NVARCHAR(150) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (OrderId) REFERENCES tb_Order(OrderId),
    FOREIGN KEY (ProductId) REFERENCES tb_Product(ProductId)
);


   --BẢNG ĐÁNH GIÁ DỊCH VỤ HOẶC BÁC SĨ (tb_ProductReview)
CREATE TABLE tb_ProductReview (
    ProductReviewId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(50) NULL,
    Phone NVARCHAR(50) NULL,
    Email NVARCHAR(50) NULL,
    CreatedDate DATETIME NULL,
    Detail NVARCHAR(500) NULL,
    Star INT NULL,
    ProductId INT NULL,
    DoctorId INT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (ProductId) REFERENCES tb_Product(ProductId),
    FOREIGN KEY (DoctorId) REFERENCES tb_Doctor(DoctorId)
);


   ---BẢNG BÀI VIẾT SỨC KHỎE (tb_Blog)
CREATE TABLE tb_Blog (
    BlogId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Title NVARCHAR(250) NULL,
    Alias NVARCHAR(250) NULL,
    CategoryId INT NULL,
    Description NVARCHAR(4000) NULL,
    Detail NVARCHAR(MAX) NULL,
    Image NVARCHAR(500) NULL,
    SeoTitle NVARCHAR(250) NULL,
    SeoDescription NVARCHAR(500) NULL,
    SeoKeywords NVARCHAR(250) NULL,
    CreatedDate DATETIME NULL,
    CreatedBy NVARCHAR(150) NULL,
    ModifiedDate DATETIME NULL,
    ModifiedBy NVARCHAR(150) NULL,
    AccountId INT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (CategoryId) REFERENCES tb_Category(CategoryId),
    FOREIGN KEY (AccountId) REFERENCES tb_Account(AccountId)
);


   ----BẢNG TIN TỨC Y TẾ (tb_News)
CREATE TABLE tb_News (
    NewsId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Title NVARCHAR(250) NULL,
    Alias NVARCHAR(250) NULL,
    CategoryId INT NULL,
    Description NVARCHAR(4000) NULL,
    Detail NVARCHAR(MAX) NULL,
    Image NVARCHAR(500) NULL,
    SeoTitle NVARCHAR(250) NULL,
    SeoDescription NVARCHAR(500) NULL,
    SeoKeywords NVARCHAR(250) NULL,
    CreatedDate DATETIME NULL,
    CreatedBy NVARCHAR(150) NULL,
    ModifiedDate DATETIME NULL,
    ModifiedBy NVARCHAR(150) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (CategoryId) REFERENCES tb_Category(CategoryId)
);


  --- BẢNG LIÊN HỆ / PHẢN HỒI
CREATE TABLE tb_Contact (
    ContactId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(150) NULL,
    Phone NVARCHAR(50) NULL,
    Email NVARCHAR(150) NULL,
    Message NVARCHAR(MAX) NULL,
    IsRead BIT NOT NULL DEFAULT 0,
    CreatedDate DATETIME NULL,
    CreatedBy NVARCHAR(150) NULL,
    ModifiedDate DATETIME NULL,
    ModifiedBy NVARCHAR(150) NULL
);

  --- BẢNG MENU WEBSITE
CREATE TABLE tb_Menu (
    MenuId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Title NVARCHAR(150) NULL,
    Alias NVARCHAR(150) NULL,
    Description NVARCHAR(500) NULL,
    Levels INT NULL,
    ParentId INT NULL,
    Position INT NULL,
    CreatedDate DATETIME NULL,
    CreatedBy NVARCHAR(150) NULL,
    ModifiedDate DATETIME NULL,
    ModifiedBy NVARCHAR(150) NULL,
    IsActive BIT NOT NULL DEFAULT 1
);
CREATE TABLE tb_Departments (
    DepartmentId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    DepartmentName NVARCHAR(150) NOT NULL,   -- Tên khoa (ví dụ: Tim mạch, Thần kinh, v.v.)
    Alias NVARCHAR(150) NULL,                 -- Đường dẫn thân thiện cho URL (ví dụ: tim-mach)
    Description NVARCHAR(500) NULL,           -- Mô tả ngắn gọn về khoa
    Image NVARCHAR(500) NULL,                 -- Hình ảnh đại diện cho khoa (tùy chọn)
    Position INT NULL,                        -- Vị trí hiển thị trên website
    SeoTitle NVARCHAR(250) NULL,              -- Tiêu đề SEO cho khoa
    SeoDescription NVARCHAR(500) NULL,        -- Mô tả SEO cho khoa
    SeoKeywords NVARCHAR(250) NULL,           -- Từ khóa SEO cho khoa
    CreatedDate DATETIME NULL,                -- Ngày tạo khoa
    CreatedBy NVARCHAR(150) NULL,             -- Người tạo khoa
    ModifiedDate DATETIME NULL,               -- Ngày sửa đổi gần nhất
    ModifiedBy NVARCHAR(150) NULL,            -- Người sửa đổi gần nhất
    IsActive BIT NOT NULL DEFAULT 1,           -- Trạng thái hoạt động của khoa (1: hoạt động, 0: không hoạt động)
    CategoryId INT NULL,                      -- Khóa ngoại liên kết với bảng Chuyên khoa
    DoctorId INT NULL,                        -- Khóa ngoại liên kết với bác sĩ phụ trách
    FOREIGN KEY (CategoryId) REFERENCES tb_Category(CategoryId),  -- Liên kết với bảng tb_Category
    FOREIGN KEY (DoctorId) REFERENCES tb_Doctor(DoctorId)    
	ALTER TABLE tb_Departments
ADD IsNew BIT NOT NULL DEFAULT 0;-- Liên kết với bảng tb_Doctor
);
CREATE TABLE tb_About (
    AboutId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Title NVARCHAR(250) NULL,                  -- Tiêu đề phần giới thiệu (VD: "Về Chúng Tôi", "Giới thiệu Bệnh viện")
    Alias NVARCHAR(250) NULL,                  -- Đường dẫn thân thiện (VD: /about-us)
    ShortDescription NVARCHAR(1000) NULL,      -- Mô tả ngắn
    Detail NVARCHAR(MAX) NULL,                 -- Nội dung chi tiết (giới thiệu tổng quan)
    Image NVARCHAR(500) NULL,                  -- Hình ảnh đại diện   
    SeoTitle NVARCHAR(250) NULL,
    SeoDescription NVARCHAR(500) NULL,
    SeoKeywords NVARCHAR(250) NULL,
    CreatedDate DATETIME NULL,
    CreatedBy NVARCHAR(150) NULL,
    ModifiedDate DATETIME NULL,
    ModifiedBy NVARCHAR(150) NULL,
    IsActive BIT NOT NULL DEFAULT 1
);
-- Thêm trường CategoryId vào bảng tb_About
ALTER TABLE tb_About
ADD CategoryId INT NULL;

-- Thiết lập khóa ngoại (foreign key) giữa bảng tb_About và bảng tb_Category
ALTER TABLE tb_About
ADD CONSTRAINT FK_tb_About_tb_Category
FOREIGN KEY (CategoryId) REFERENCES tb_Category(CategoryId);
ALTER TABLE tb_About
ADD IsNew BIT NOT NULL DEFAULT 0;  -- 0 có thể đại diện là không phải mới, 1 đại diện là mới

CREATE TABLE tb_AboutDetails (
    AboutDetailsId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    AboutId INT NOT NULL,                        -- Liên kết với bảng tb_About (các thông tin chung về phần giới thiệu)
    Address NVARCHAR(500) NULL,                  -- Địa chỉ của bệnh viện
    PhoneNumber NVARCHAR(50) NULL,               -- Số điện thoại liên hệ
    Email NVARCHAR(100) NULL,                    -- Email liên hệ
    MapLink NVARCHAR(500) NULL,                  -- Link đến bản đồ (Google Map, ví dụ: Google Map URL)
    SocialLinks NVARCHAR(1000) NULL,             -- Các liên kết mạng xã hội (ví dụ: Facebook, Twitter, v.v.)
    ContactFormText NVARCHAR(1000) NULL,         -- Nội dung mô tả về form liên hệ (ví dụ: "Liên hệ với chúng tôi để biết thêm chi tiết")
    CreatedDate DATETIME NULL,                   -- Ngày tạo
    CreatedBy NVARCHAR(150) NULL,                -- Người tạo
    ModifiedDate DATETIME NULL,                  -- Ngày sửa đổi
    ModifiedBy NVARCHAR(150) NULL,               -- Người sửa đổi
    IsActive BIT NOT NULL DEFAULT 1,              -- Trạng thái hoạt động của phần chi tiết (1: hiển thị, 0: không hiển thị)
    FOREIGN KEY (AboutId) REFERENCES tb_About(AboutId)  -- Liên kết với bảng tb_About
);


INSERT INTO tb_Role (RoleName, Description)
VALUES
(N'Quản trị viên', N'Tài khoản quản trị hệ thống'),
(N'Khách hàng', N'Tài khoản người bệnh'),
(N'Biên tập viên', N'Người viết nội dung'),
(N'Kế toán', N'Xử lý đơn hàng'),
(N'Nhân viên kho', N'Quản lý kho');
INSERT INTO tb_Account (Username, Password, FullName, Phone, Email, RoleId, LastLogin, IsActive)
VALUES
(N'admin1', N'123456', N'Nguyễn Văn A', N'0988112233', N'admin1@example.com', 1, GETDATE(), 1),
(N'user1', N'123456', N'Nguyễn Thị B', N'0911223344', N'user1@example.com', 2, GETDATE(), 1),
(N'editor1', N'123456', N'Trần Văn C', N'0933445566', N'editor1@example.com', 3, GETDATE(), 1),
(N'accountant1', N'123456', N'Lê Thị D', N'0977554433', N'accountant1@example.com', 4, GETDATE(), 1),
(N'warehouse1', N'123456', N'Phạm Văn E', N'0966332211', N'warehouse1@example.com', 5, GETDATE(), 1);
INSERT INTO tb_Category (Title, Alias, Description, Position, CreatedDate, CreatedBy)
VALUES
(N'Tim mạch', N'tim-mach', N'Khám và điều trị bệnh tim mạch', 1, GETDATE(), N'admin1'),
(N'Thần kinh', N'than-kinh', N'Chuyên khoa thần kinh', 2, GETDATE(), N'admin1'),
(N'Tiêu hóa', N'tieu-hoa', N'Chuyên khoa tiêu hóa', 3, GETDATE(), N'admin1'),
(N'Nhi khoa', N'nhi-khoa', N'Chuyên khoa nhi', 4, GETDATE(), N'admin1'),
(N'Da liễu', N'da-lieu', N'Chuyên khoa da liễu', 5, GETDATE(), N'admin1');
INSERT INTO tb_Doctor (FullName, Gender, DateOfBirth, Phone, Email, Address, CategoryId, Position, Qualification, ExperienceYears, Description, Image, AccountId, CreatedDate, CreatedBy)
VALUES
(N'Nguyễn Văn An', N'Nam', '1980-05-12', N'0911223344', N'dr.nguyenvana@example.com', N'123 Đường A, Hà Nội', 1, N'Bác sĩ trưởng khoa', N'Thuộc tim mạch', 15, N'Kinh nghiệm khám và điều trị bệnh tim', N'/assets/img/200x100/1-1-200x100.jpg', 1, GETDATE(), N'admin1'),
(N'Lê Thị Bình', N'Nữ', '1985-08-20', N'0922334455', N'dr.lethib@example.com', N'234 Đường B, Hà Nội', 2, N'Bác sĩ chuyên khoa', N'Thần kinh', 12, N'Chuyên điều trị các bệnh về thần kinh', N'/assets/img/200x100/1-2-200x100.jpg', 2, GETDATE(), N'admin1'),
(N'Phạm Văn Kiên', N'Nam', '1978-02-15', N'0933445566', N'dr.phamvanc@example.com', N'345 Đường C, Hà Nội', 3, N'Bác sĩ chuyên khoa', N'Tiêu hóa', 18, N'Khám và điều trị bệnh tiêu hóa', N'/assets/img/200x100/1-3-200x100.jpg', 3, GETDATE(), N'admin1'),
(N'Trần Thị Duyên', N'Nữ', '1982-11-30', N'0944556677', N'dr.tranthid@example.com', N'456 Đường D, Hà Nội', 4, N'Bác sĩ nhi khoa', N'Nhi khoa', 14, N'Chăm sóc sức khỏe trẻ em', N'/assets/img/200x100/1-4-200x100.jpg', 4, GETDATE(), N'admin1'),
(N'Hoàng Văn Hải', N'Nam', '1975-06-18', N'0955667788', N'dr.hoangvane@example.com', N'567 Đường E, Hà Nội', 5, N'Bác sĩ da liễu', N'Da liễu', 20, N'Chuyên điều trị các bệnh da liễu', N'/assets/img/200x100/1-5-200x100.jpg', 5, GETDATE(), N'admin1');
INSERT INTO tb_ProductCategory (Title, Alias, Description, Icon, Position, CreatedDate, CreatedBy)
VALUES
(N'Khám tổng quát', N'kham-tong-quat', N'Khám sức khỏe tổng quát', N'icon1.png', 1, GETDATE(), N'admin1'),
(N'Xét nghiệm', N'xet-nghiem', N'Dịch vụ xét nghiệm', N'icon2.png', 2, GETDATE(), N'admin1'),
(N'Siêu âm', N'sieu-am', N'Dịch vụ siêu âm', N'icon3.png', 3, GETDATE(), N'admin1'),
(N'Khám chuyên khoa', N'kham-chuyen-khoa', N'Khám chuyên khoa tim, thần kinh...', N'icon4.png', 4, GETDATE(), N'admin1'),
(N'Vắc xin', N'vac-xin', N'Tiêm vắc xin phòng bệnh', N'icon5.png', 5, GETDATE(), N'admin1');
INSERT INTO tb_Product (Title, Alias, ProductCategoryId, Description, Detail, Image, Price, PriceSale, Duration, DoctorId, CreatedDate, CreatedBy)
VALUES
(N'Khám tổng quát người lớn', N'kham-tong-quat-nguoi-lon', 1, N'Khám sức khỏe tổng quát', N'Bao gồm đo huyết áp, tim mạch, xét nghiệm cơ bản', N'/assets/img/200x120/1-12-200x120.jpg', 500000, 450000, N'60 phút', 1, GETDATE(), N'admin1'),
(N'Xét nghiệm máu', N'xet-nghiem-mau', 2, N'Xét nghiệm công thức máu', N'Đo nồng độ hồng cầu, bạch cầu...', N'/assets/img/200x120/1-4-200x120.jpg', 200000, 180000, N'30 phút', 2, GETDATE(), N'admin1'),
(N'Siêu âm ổ bụng', N'sieu-am-o-bung', 3, N'Siêu âm ổ bụng', N'Kiểm tra gan, thận, dạ dày...', N'/assets/img/200x120/1-10-200x120.jpg', 350000, 300000, N'40 phút', 3, GETDATE(), N'admin1'),
(N'Khám tim mạch chuyên sâu', N'kham-tim-mach-chuyen-sau', 4, N'Khám và chẩn đoán bệnh tim', N'Đo điện tâm đồ, siêu âm tim...', N'/assets/img/200x120/1-11-200x120.jpg', 600000, 550000, N'50 phút', 1, GETDATE(), N'admin1'),
(N'Tiêm vắc xin phòng cúm', N'tiem-vac-xin-phong-cum', 5, N'Tiêm vắc xin phòng cúm', N'Dành cho trẻ em và người lớn', N'/assets/img/200x120/1-6-200x120.jpg', 150000, 150000, N'15 phút', NULL, GETDATE(), N'admin1'),
(N'Dịch vụ xét nghiệm ', N'dich-vu-xet-nghiem', 5, N'Cung cấp các dịch vụ xét nghiệm đa dạng, chính xác và nhanh chóng, hỗ trợ chẩn đoán hiệu quả cho bác sĩ và bệnh nhân.', N'Dành cho trẻ em và người lớn', N'/assets/img/200x120/1-8-200x120.jpg', 136666, 144433, N'20 phút', 3, GETDATE(), N'admin1');
INSERT INTO tb_ProductReview (Name, Phone, Email, CreatedDate, Detail, Star, ProductId, DoctorId)
VALUES
(N'Nguyễn Thị H', N'0981122334', N'nguyenthih@example.com', GETDATE(), N'Dịch vụ rất tốt', 5, 1, 1),
(N'Lê Văn K', N'0912233445', N'levank@example.com', GETDATE(), N'Rất hài lòng', 4, 2, 2),
(N'Phạm Thị L', N'0963344556', N'phamthil@example.com', GETDATE(), N'Dịch vụ ổn', 4, 3, 3),
(N'Trần Văn M', N'0934455667', N'tranvanm@example.com', GETDATE(), N'Bác sĩ tận tình', 5, 4, 1),
(N'Hoàng Thị N', N'0975566778', N'hoangthin@example.com', GETDATE(), N'Nhân viên nhiệt tình', 4, 5, NULL);
INSERT INTO tb_Blog (Title, Alias, CategoryId, Description, Detail, Image, CreatedDate, CreatedBy, AccountId)
VALUES
(N'10 cách chăm sóc tim mạch', N'cham-soc-tim-mach', 1, N'Hướng dẫn chăm sóc tim mạch', N'Chi tiết các biện pháp chăm sóc tim...', N'blog1.jpg', GETDATE(), N'editor1', 3),
(N'Cách phòng bệnh thần kinh', N'phong-benh-than-kinh', 2, N'Hướng dẫn phòng bệnh thần kinh', N'Chi tiết phòng bệnh...', N'blog2.jpg', GETDATE(), N'editor1', 3),
(N'Dinh dưỡng cho trẻ em', N'dinh-duong-tre-em', 4, N'Hướng dẫn dinh dưỡng cho trẻ', N'Chi tiết dinh dưỡng...', N'blog3.jpg', GETDATE(), N'editor1', 3),
(N'Chăm sóc da mùa đông', N'cham-soc-da-mua-dong', 5, N'Hướng dẫn chăm sóc da', N'Chi tiết chăm sóc da...', N'blog4.jpg', GETDATE(), N'editor1', 3),
(N'Khám tiêu hóa định kỳ', N'kham-tieu-hoa-dinh-ky', 3, N'Khám tiêu hóa định kỳ', N'Chi tiết khám...', N'blog5.jpg', GETDATE(), N'editor1', 3);
INSERT INTO tb_News (Title, Alias, CategoryId, Description, Detail, Image, CreatedDate, CreatedBy)
VALUES
(N'Tin tức y tế 1', N'tin-tuc-y-te-1', 1, N'Tin tức y tế tổng hợp', N'Chi tiết tin tức...', N'news1.jpg', GETDATE(), N'editor1'),
(N'Tin tức y tế 2', N'tin-tuc-y-te-2', 2, N'Tin tức thần kinh', N'Chi tiết tin tức...', N'news2.jpg', GETDATE(), N'editor1'),
(N'Tin tức y tế 3', N'tin-tuc-y-te-3', 3, N'Tin tức tiêu hóa', N'Chi tiết tin tức...', N'news3.jpg', GETDATE(), N'editor1'),
(N'Tin tức y tế 4', N'tin-tuc-y-te-4', 4, N'Tin tức nhi khoa', N'Chi tiết tin tức...', N'news4.jpg', GETDATE(), N'editor1'),
(N'Tin tức y tế 5', N'tin-tuc-y-te-5', 5, N'Tin tức da liễu', N'Chi tiết tin tức...', N'news5.jpg', GETDATE(), N'editor1');
INSERT INTO tb_Contact (Name, Phone, Email, Message, IsRead, CreatedDate, CreatedBy)
VALUES
(N'Nguyễn Văn F', N'0988776655', N'contact1@example.com', N'Tôi muốn đặt lịch khám', 0, GETDATE(), N'user1'),
(N'Lê Thị G', N'0911445566', N'contact2@example.com', N'Tôi cần tư vấn vắc xin', 0, GETDATE(), N'user1'),
(N'Phạm Văn H', N'0933556677', N'contact3@example.com', N'Tôi muốn phản hồi dịch vụ', 0, GETDATE(), N'user1'),
(N'Trần Thị I', N'0944667788', N'contact4@example.com', N'Tôi muốn đăng ký nhận tin', 0, GETDATE(), N'user1'),
(N'Hoàng Văn J', N'0955778899', N'contact5@example.com', N'Tôi có thắc mắc về giá dịch vụ', 0, GETDATE(), N'user1');
INSERT INTO tb_Menu (Title, Alias, Description, Levels, ParentId, Position, CreatedDate, CreatedBy)
VALUES
(N'Trang chủ', N'trang-chu', N'Menu trang chủ', 1, NULL, 1, GETDATE(), N'admin1'),
(N'Giới thiệu', N'gioi-thieu', N'Menu giới thiệu', 1, NULL, 2, GETDATE(), N'admin1'),
(N'Dịch vụ', N'dich-vu', N'Menu dịch vụ', 1, NULL, 3, GETDATE(), N'admin1'),
(N'Tin tức', N'tin-tuc', N'Menu tin tức', 1, NULL, 4, GETDATE(), N'admin1'),
(N'Liên hệ', N'lien-he', N'Menu liên hệ', 1, NULL, 5, GETDATE(), N'admin1');
INSERT INTO tb_Service 
(
    Title, Alias, Icon, Image, ShortDescription, Detail,
    DoctorId, CategoryId, Position,
    SeoTitle, SeoDescription, SeoKeywords,
    CreatedDate, CreatedBy,
    IsNew, IsFeatured, IsActive
)
VALUES
-- 1. Dịch vụ Tim mạch
(N'Khám tim mạch tổng quát', N'kham-tim-mach-tong-quat', N'icon-tim-mach.png', N'service1.jpg',
 N'Khám và chẩn đoán các bệnh lý về tim mạch.',
 N'Bao gồm đo huyết áp, siêu âm tim, điện tâm đồ và tư vấn điều trị chuyên sâu.',
 1, 1, 1,
 N'Khám tim mạch tổng quát', N'Dịch vụ khám và tư vấn tim mạch', N'tim mạch, khám tim',
 GETDATE(), N'admin1', 1, 1, 1),

-- 2. Dịch vụ Thần kinh
(N'Tư vấn thần kinh', N'tu-van-than-kinh', N'icon-than-kinh.png', N'service2.jpg',
 N'Tư vấn và điều trị các bệnh về thần kinh.',
 N'Khám chuyên sâu các bệnh lý về thần kinh, đau đầu, mất ngủ, rối loạn thần kinh thực vật.',
 2, 2, 2,
 N'Tư vấn thần kinh', N'Dịch vụ khám và điều trị thần kinh', N'thần kinh, đau đầu, mất ngủ',
 GETDATE(), N'admin1', 1, 0, 1),

-- 3. Dịch vụ Tiêu hóa
(N'Khám tiêu hóa chuyên sâu', N'kham-tieu-hoa-chuyen-sau', N'icon-tieu-hoa.png', N'service3.jpg',
 N'Khám và điều trị các bệnh đường tiêu hóa.',
 N'Bao gồm nội soi, siêu âm ổ bụng và tư vấn chế độ ăn uống lành mạnh.',
 3, 3, 3,
 N'Khám tiêu hóa chuyên sâu', N'Khám và điều trị bệnh tiêu hóa', N'tiêu hóa, nội soi, dạ dày',
 GETDATE(), N'admin1', 0, 1, 1),

-- 4. Dịch vụ Nhi khoa
(N'Khám sức khỏe trẻ em', N'kham-suc-khoe-tre-em', N'icon-nhi-khoa.png', N'service4.jpg',
 N'Dịch vụ khám sức khỏe định kỳ cho trẻ em.',
 N'Tư vấn dinh dưỡng, tiêm chủng và kiểm tra sức khỏe tổng quát cho trẻ nhỏ.',
 4, 4, 4,
 N'Khám sức khỏe trẻ em', N'Dịch vụ khám sức khỏe cho trẻ', N'nhi khoa, trẻ em, tiêm chủng',
 GETDATE(), N'admin1', 0, 0, 1),

-- 5. Dịch vụ Da liễu
(N'Điều trị da liễu chuyên sâu', N'dieu-tri-da-lieu', N'icon-da-lieu.png', N'service5.jpg',
 N'Khám và điều trị các bệnh lý về da.',
 N'Chăm sóc, tư vấn và điều trị các bệnh da liễu như mụn, viêm da, dị ứng.',
 5, 5, 5,
 N'Điều trị da liễu', N'Dịch vụ điều trị da liễu', N'da liễu, mụn, viêm da',
 GETDATE(), N'admin1', 1, 0, 1);
 INSERT INTO tb_Departments 
(DepartmentName, Alias, Description, Image, Position, SeoTitle, SeoDescription, SeoKeywords, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy, IsActive, CategoryId, DoctorId, IsNew)
VALUES
-- Khoa Tim mạch
(N'Tim mạch', N'tim-mach', N'Khám và điều trị bệnh tim mạch', N'department-tim-mach.jpg', 1, N'Khám tim mạch', N'Dịch vụ khám và tư vấn về bệnh tim mạch', N'tim mạch, bệnh tim, siêu âm tim', GETDATE(), N'admin1', GETDATE(), N'admin1', 1, 1, 1, 1),  -- IsNew = 1

-- Khoa Thần kinh
(N'Thần kinh', N'than-kinh', N'Khám và điều trị các bệnh lý thần kinh', N'department-than-kinh.jpg', 2, N'Khám thần kinh', N'Dịch vụ khám và điều trị bệnh thần kinh', N'thần kinh, đau đầu, mất ngủ', GETDATE(), N'admin1', GETDATE(), N'admin1', 1, 2, 2, 1),  -- IsNew = 1

-- Khoa Tiêu hóa
(N'Tiêu hóa', N'tieu-hoa', N'Khám và điều trị các bệnh lý tiêu hóa', N'department-tieu-hoa.jpg', 3, N'Khám tiêu hóa', N'Dịch vụ khám và điều trị các bệnh tiêu hóa', N'tiêu hóa, nội soi, siêu âm bụng', GETDATE(), N'admin1', GETDATE(), N'admin1', 1, 3, 3, 0),  -- IsNew = 0

-- Khoa Nhi khoa
(N'Nhi khoa', N'nhi-khoa', N'Khám và điều trị các bệnh lý về sức khỏe trẻ em', N'department-nhi-khoa.jpg', 4, N'Khám nhi khoa', N'Dịch vụ khám và điều trị bệnh lý cho trẻ em', N'nhi khoa, trẻ em, tiêm chủng', GETDATE(), N'admin1', GETDATE(), N'admin1', 1, 4, 4, 0); -- IsNew = 0

-- Thêm 5 thông tin vào bảng tb_About (giới thiệu về bệnh viện)
INSERT INTO tb_About (Title, Alias, ShortDescription, Detail, Image, SeoTitle, SeoDescription, SeoKeywords, CategoryId, IsNew, CreatedDate, CreatedBy, IsActive)
VALUES
(N'Giới thiệu Bệnh viện MediNest', N'gioi-thieu-benh-vien', 
 N'Chúng tôi là một bệnh viện chuyên cung cấp dịch vụ chăm sóc sức khỏe chất lượng cao.', 
 N'Bệnh viện MediNest cam kết mang đến dịch vụ y tế chất lượng và chăm sóc sức khỏe toàn diện. Với đội ngũ bác sĩ giàu kinh nghiệm và các dịch vụ y tế tiên tiến, chúng tôi luôn sẵn sàng đồng hành cùng bạn trên hành trình chăm sóc sức khỏe.', 
 N'about-image.jpg',
 N'Giới thiệu Bệnh viện MediNest', N'Giới thiệu về bệnh viện, đội ngũ bác sĩ và các dịch vụ y tế.', 
 N'Giới thiệu, Bệnh viện, Y tế, MediNest', 1, 1, GETDATE(), N'admin1', 1),  -- CategoryId = 1 (Bệnh viện), IsNew = 1

(N'Giới thiệu Khoa Tim mạch', N'gioi-thieu-khoa-tim-mach', 
 N'Chuyên khoa Tim mạch cung cấp dịch vụ chẩn đoán và điều trị bệnh tim mạch.', 
 N'Khoa Tim mạch tại MediNest chuyên cung cấp các dịch vụ khám và điều trị bệnh tim mạch, bao gồm siêu âm tim, điện tâm đồ và các phương pháp chẩn đoán tiên tiến.', 
 N'tim-mach-image.jpg', 
 N'Giới thiệu Khoa Tim mạch', N'Khám và điều trị bệnh tim mạch tại MediNest.', 
 N'Tim mạch, siêu âm tim, điện tâm đồ, bệnh tim', 2, 1, GETDATE(), N'admin1', 1),  -- CategoryId = 2 (Khoa Tim mạch), IsNew = 1

(N'Giới thiệu Khoa Thần kinh', N'gioi-thieu-khoa-than-kinh', 
 N'Khoa Thần kinh chuyên điều trị các bệnh lý thần kinh.', 
 N'Khoa Thần kinh tại MediNest cung cấp các dịch vụ điều trị bệnh lý thần kinh như đau đầu, mất ngủ, rối loạn thần kinh thực vật và nhiều vấn đề liên quan khác.', 
 N'than-kinh-image.jpg', 
 N'Giới thiệu Khoa Thần kinh', N'Khám và điều trị bệnh lý thần kinh tại MediNest.', 
 N'Thần kinh, đau đầu, mất ngủ, rối loạn thần kinh', 3, 1, GETDATE(), N'admin1', 1); -- CategoryId = 3 (Khoa Thần kinh), IsNew = 1
 -- CategoryId = 5 (Khoa Nhi khoa), IsNew = 1


 -- Thêm 5 thông tin vào bảng tb_AboutDetails (chi tiết liên hệ)
INSERT INTO tb_AboutDetails (AboutId, Address, PhoneNumber, Email, MapLink, SocialLinks, ContactFormText, CreatedDate, CreatedBy, IsActive)
VALUES
(1, N'4582 Magnolia Avenue, Riverside, CA 92506', N'+1 (951) 684-9123', N'contact@medinest.com', 
 N'https://www.google.com/maps/place/4582+Magnolia+Avenue,+Riverside,+CA+92506', 
 N'Facebook: /MediNest, Twitter: @MediNest, LinkedIn: /MediNest', 
 N'Điền vào form này để liên hệ với chúng tôi về các dịch vụ và đặt lịch khám.', 
 GETDATE(), N'admin1', 1);
