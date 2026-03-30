#include <iostream>
#include <leptonica/allheaders.h>
#include <opencv2/opencv.hpp>
#include <tesseract/baseapi.h>

int main() {
  // 1. Khởi tạo Tesseract API
  tesseract::TessBaseAPI *ocr = new tesseract::TessBaseAPI();

  // Khởi tạo OCR với ngôn ngữ tiếng Anh (eng)
  if (ocr->Init("C:\\msys64\\ucrt64\\share\\tessdata", "eng")) {
    std::cerr << "Lỗi: Không thể khởi tạo Tesseract OCR!" << std::endl;
    system("pause"); // Chốt chặn 1: Dừng màn hình nếu Tesseract lỗi
    return -1;
  }

  // Thiết lập chế độ phân đoạn trang (Page Segmentation Mode)
  ocr->SetPageSegMode(tesseract::PSM_SINGLE_LINE);

  // 2. Mở camera (Thêm cờ cv::CAP_DSHOW để Windows dễ nhận diện camera hơn)
  cv::VideoCapture cap(0, cv::CAP_DSHOW);
  if (!cap.isOpened()) {
    std::cerr << "Lỗi: Không thể mở camera!" << std::endl;
    system("pause"); // Chốt chặn 2: Dừng màn hình nếu không thấy Camera
    ocr->End();
    delete ocr;
    return -1;
  }

  std::cout << "Mở camera và Tesseract thành công." << std::endl;
  std::cout << "Phím 'c': Chụp ảnh và nhận diện biển số" << std::endl;
  std::cout << "Phím 'q': Thoát chương trình" << std::endl;

  cv::Mat frame;
  cv::namedWindow("ANPR Module - Live Feed", cv::WINDOW_AUTOSIZE);

  while (true) {
    cap >> frame;
    if (frame.empty()) {
      std::cerr << "Lỗi: Không nhận được frame từ camera!" << std::endl;
      break;
    }

    cv::imshow("ANPR Module - Live Feed", frame);

    char key = (char)cv::waitKey(10);

    if (key == 'q' || key == 'Q') {
      std::cout << "Đang thoát..." << std::endl;
      break;
    } else if (key == 'c' || key == 'C') {
      // 3. Xử lý OCR
      std::cout << "--- Đang nhận diện... ---" << std::endl;

      cv::Mat gray;
      cv::cvtColor(frame, gray, cv::COLOR_BGR2GRAY);

      ocr->SetImage(gray.data, gray.cols, gray.rows, 1, gray.step);
      char *outText = ocr->GetUTF8Text();

      if (outText) {
        std::cout << "Biển số nhận diện được: " << outText << std::endl;
        cv::imwrite("test_plate.jpg", frame);
        delete[] outText;
      } else {
        std::cout << "Không thể nhận diện được ký tự nào." << std::endl;
      }
      std::cout << "-------------------------" << std::endl;
    }
  }

  // 4. Giải phóng tài nguyên
  cap.release();
  cv::destroyAllWindows();
  ocr->End();
  delete ocr;

  return 0;
}