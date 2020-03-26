import cv2
import numpy as np

# Functions -------------------------------------------------------------------------

def templateMatch(target, source, threshold):
    target_height, target_width, _ = target.shape
    source_height, source_width, _ = source.shape

    result_height = source_height - target_height + 1
    result_width = source_width - target_width + 1

    result = np.zeros((result_height, result_width))

    for i in range(0, result_height):
        for j in range(0, result_width):
                    sqsum = 0
                    for xx in range(0, target_height):
                        for yy in range(0, target_width):
                            sqsum += (source[x + xx, y + yy] - target[xx, yy])**2

    return result

# -----------------------------------------------------------------------------------

target_image = cv2.imread("images/t1-img1.png")
target_image = target_image / 255
cv2.imshow("Target Image", target_image)

source_image = cv2.imread("images/img1.png")
source_image = source_image / 255
cv2.imshow("Source Image", source_image)

result_image = templateMatch(target_image, source_image, 0.1)
cv2.imshow("Result Image", result_image)

text_image = np.zeros((50, 450, 3), np.uint8)
font = cv2.FONT_HERSHEY_SIMPLEX
bottom_left_corner_text = (10, 35)
font_scale = 1
font_color = (255, 255, 255)
line_type = 2
cv2.putText(
    text_image,
    'Image Found / Not Found',
    bottom_left_corner_text,
    font,
    font_scale,
    font_color,
    line_type
    )
cv2.imshow("Result", text_image)

# Input ---------
key = cv2.waitKey(0)
if key == 27:
    cv2.destroyAllWindows()
