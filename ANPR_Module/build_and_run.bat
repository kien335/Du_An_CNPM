@echo off
SETLOCAL EnableDelayedExpansion

REM Them duong dan MinGW vao PATH tam thoi
SET "PATH=C:\msys64\ucrt64\bin;%PATH%"

REM Thu tim CMake neu khong co trong PATH
where cmake >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo [THONG BAO] Khong tim thay cmake trong PATH. Thu tim trong Program Files...
    if exist "C:\Program Files\CMake\bin\cmake.exe" (
        SET "PATH=C:\Program Files\CMake\bin;%PATH%"
    ) else if exist "C:\Program Files (x86)\CMake\bin\cmake.exe" (
        SET "PATH=C:\Program Files (x86)\CMake\bin;%PATH%"
    ) else (
        echo [LOI] Khong tim thay CMake. Vui long kiem tra lai vinh cai dat.
        pause
        exit /b 1
    )
)

echo ===================================================
echo   ANPR Module: Dang bat dau qua trinh tu dong...
echo ===================================================

REM Tao thu muc build neu chua co
if not exist "build" (
    mkdir "build"
)

REM Di chuyen vao thu muc build voi dau ngoac kep
cd /d "%~dp0build"

echo.
echo --- 1. Dang cau hinh dự án (CMake) ---
cmake -G "MinGW Makefiles" ..

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [LOI] Cau hinh CMake that bai.
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo --- 2. Dang bien dich (Make) ---
mingw32-make

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [LOI] Bien dich that bai.
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo --- 3. Bien dich thanh cong! Dang chay chuong trinh ---
echo (Nhan 'q' trong cua so camera de thoat)
if exist "ANPR_Module.exe" (
    "./ANPR_Module.exe"
) else (
    echo [LOI] Khong tim thay file thuc thi ANPR_Module.exe
)

cd ..
echo.
echo ===================================================
echo   Hoan thanh.
echo ===================================================
pause
