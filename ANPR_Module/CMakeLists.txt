cmake_minimum_required(VERSION 3.10)
project(ANPR_Module)

# Cấu hình OpenCV từ MSYS2
set(OpenCV_DIR "C:/msys64/ucrt64/lib/cmake/opencv4")
find_package(OpenCV REQUIRED)

# Tìm Tesseract và Leptonica qua PkgConfig
find_package(PkgConfig REQUIRED)
pkg_check_modules(TESSERACT REQUIRED tesseract)
pkg_check_modules(LEPTONICA REQUIRED lept)

# Thông tin debug (tùy chọn)
message(STATUS "OpenCV version: ${OpenCV_VERSION}")
message(STATUS "Tesseract include dirs: ${TESSERACT_INCLUDE_DIRS}")

# Thêm flag cho trình biên dịch
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Thư mục chứa header
include_directories(${OpenCV_INCLUDE_DIRS} ${TESSERACT_INCLUDE_DIRS} ${LEPTONICA_INCLUDE_DIRS})

# Tạo chương trình thực thi
add_executable(ANPR_Module main.cpp)

# Liên kết với các thư viện OpenCV và Tesseract
target_link_libraries(ANPR_Module ${OpenCV_LIBS} ${TESSERACT_LIBRARIES} ${LEPTONICA_LIBRARIES})
