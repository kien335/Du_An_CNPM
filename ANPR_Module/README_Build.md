# Hướng dẫn biên dịch và chạy ANPR_Module

Dưới đây là các bước để biên dịch và chạy module này.

## Yêu cầu
- Đã cài đặt OpenCV (đường dẫn hiện tại: `C:/Users/Admin/Downloads/opencv/build`).
- Đã cài đặt CMake.
- Đã cài đặt trình biên dịch C++ (MinGW-w64).

## Cách chạy nhanh nhất (Khuyên dùng)
1.  Mở thư mục `ANPR_Module`.
2.  Chạy file `build_and_run.bat`. File này sẽ tự động cấu hình PATH và biên dịch cho bạn.

## Các phím tắt trong chương trình
- Phím **'c'**: Chụp ảnh biển số hiện tại và lưu thành `test_plate.jpg`.
- Phím **'q'**: Thoát chương trình.
