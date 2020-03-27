import cv2
import numpy as np
from math import hypot

# Functions -------------------------------------------------------------------------

def templateMatch(target, source):
    target_height, target_width, _ = target.shape
    source_height, source_width, _ = source.shape

    # if(): 
    #     print("Images do not have the same shape :o, aborting template match...")
    #     return [np.zeros((1,1)), 0]

    result_height = source_height - target_height + 1
    result_width = source_width - target_width + 1

    result = np.zeros((result_height, result_width))



    caption = np.zeros((source_height, source_width))
    caption_y = 0
    caption_x = 0
    threshold_2 = 0.1
    # Source is what we want to find and tarjet is the object where we want to find the source
    ssd = 0
    # for i in range(0, result_height):
    #    for j in range(0, result_width):
    #        diff = target[i][j] - source[i][j]
    #        ssd += diff * diff
    #        if ComparationSSD(ssd,threshold_2):
    #            for i_2 in range (i, (i + source_height)):
    #                for j_2 in range (j, (j + source_width)):
    #                    diff_2 = target[i_2][j_2] - source[i_2][j_2]
    #                    ssd += diff_2 * diff_2
    #         if ComparationSSD(ssd,threshold_2):
    #           caption_y = i
    #           caption_x = j 
    #         ssd = 0
    
    for i in range(0, result_height):
        for j in range(0, result_width):
            diff = target[0][0] - source[i][j]
            ssd += diff * diff
            if hypot(ssd[0],ssd[1],ssd[2]) <= 0.1:
              for i_1 in range (0, target_height):
                  for j_1 in range (0, target_width):
                    n_diff = target[i_1][j_1] - source[i+i_1][j+j_1]
                    ssd += n_diff * n_diff
                    

            if hypot(ssd[0],ssd[1],ssd[2]) <= 0.1:
                print("lo hemos logrado")

                
            ssd = 0


    # for i in range(0, result_height):
    #     for j in range(0, result_width):
    #                 sqsum = 0
    #                 for xx in range(0, target_height):
    #                     for yy in range(0, target_width):
    #                         sqsum += (source[x + xx, y + yy] - target[xx, yy])**2

    return [result, ssd]

# -----------------------------------------------------------------------------------



target_image = cv2.imread("images/img1.png")
target_image = target_image / 255
cv2.imshow("Target Image", target_image)

source_image = cv2.imread("images/t1-img1.png")
source_image = source_image / 255
cv2.imshow("Source Image", source_image)

threshold = 0.1
operationResultImage, operationSsd = templateMatch(source_image, target_image)
#operationSsd = np.linalg.norm(operationSsd)

# foundText = "Image Not Found"
# if(operationSsd <= threshold): 
#     foundText = "Image Found"

# foundText += ", ssd: "
# foundText += str(operationSsd)
# foundText += " and threshold: "
# foundText += str(threshold)

# cv2.imshow("Result Image", operationResultImage)

# text_image = np.zeros((50, 1050, 3), np.uint8)
# font = cv2.FONT_HERSHEY_SIMPLEX
# bottom_left_corner_text = (10, 35)
# font_scale = 1
# font_color = (255, 255, 255)
# line_type = 2
# cv2.putText(
#     text_image,
#     foundText,
#     bottom_left_corner_text,
#     font,
#     font_scale,
#     font_color,
#     line_type
#     )
# cv2.imshow("Result", text_image)

# Input ---------
key = cv2.waitKey(0)
if key == 27:
    cv2.destroyAllWindows()
